using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Gestion de los dados la resolución de los dados no aplicaria aqui, se haria en el turno manager
public class GestorDados : MonoBehaviour
{
    public const int TOTAL_DADOS          = 5;
    public const int MAX_LANZAMIENTOS     = 3;
    public const float DURACION_ANIMACION = 0.8f;   

    public Dado[] Dados { get; private set; }

    public int LanzamientosRestantes { get; private set; }

    public bool PrimerLanzamientoRealizado { get; private set; }

    public bool EstaAnimando { get; private set; }

    public event Action OnAntesDetirar;
    public event Action<Dado[]> OnDadosResueltos;
    public event Action<Dado> OnDadoBloqueoCambiado;

    public event Action<int> OnLanzamientosRestantesCambiado;

    private System.Random _rng = new System.Random();

    public static GestorDados Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        InicializarDados();
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    private void InicializarDados()
    {
        Dados = new Dado[TOTAL_DADOS];
        for (int i = 0; i < TOTAL_DADOS; i++)
            Dados[i] = new Dado(i);
    }

    public void IniciarNuevoTurno()
    {
        foreach (var dado in Dados)
            dado.ResetearParaTurno();

        LanzamientosRestantes       = MAX_LANZAMIENTOS;
        PrimerLanzamientoRealizado  = false;
        EstaAnimando                = false;

        OnLanzamientosRestantesCambiado?.Invoke(LanzamientosRestantes);
    }

    public bool Lanzar()
    {
        if (LanzamientosRestantes <= 0 || EstaAnimando) return false;

        StartCoroutine(CorrutinaLanzamiento());
        return true;
    }
    
    public bool AlternarBloqueo(int indiceDado)
    {
        if (!PrimerLanzamientoRealizado) return false;
        if (LanzamientosRestantes <= 0)  return false;
        if (EstaAnimando)                return false;
        if (indiceDado < 0 || indiceDado >= TOTAL_DADOS) return false;

        Dados[indiceDado].AlternarBloqueo();
        OnDadoBloqueoCambiado?.Invoke(Dados[indiceDado]);
        return true;
    }

    public int ContarSimbolo(SimboloDado simbolo, bool soloDesbloqueados = false)
    {
        int count = 0;
        foreach (var dado in Dados)
        {
            if (soloDesbloqueados && dado.EstaBloqueado) continue;
            if (dado.Resultado == simbolo) count++;
        }
        return count;
    }

    public Dictionary<SimboloDado, int> ObtenerConteos()
    {
        var conteos = new Dictionary<SimboloDado, int>();
        foreach (SimboloDado s in Enum.GetValues(typeof(SimboloDado)))
            conteos[s] = 0;

        foreach (var dado in Dados)
            conteos[dado.Resultado]++;

        return conteos;
    }

    public string ObtenerResumenResultados()
    {
        var partes = new string[TOTAL_DADOS];
        for (int i = 0; i < TOTAL_DADOS; i++)
            partes[i] = Dados[i].Resultado.Icono();
        return string.Join("  ", partes);
    }

    private IEnumerator CorrutinaLanzamiento()
    {
        EstaAnimando = true;
        foreach (var dado in Dados)
            if (!dado.EstaBloqueado) dado.IniciarAnimacion();

        OnAntesDetirar?.Invoke();

        float tiempoInicio     = Time.time;
        float tiempoIntermedio = DURACION_ANIMACION * 0.7f;
        float intervalo        = 0.12f;   

        while (Time.time - tiempoInicio < tiempoIntermedio)
        {
            foreach (var dado in Dados)
                if (!dado.EstaBloqueado)
                    dado.Lanzar(_rng);

            OnDadosResueltos?.Invoke(Dados);
            yield return new WaitForSeconds(intervalo);
        }

        LanzamientosRestantes--;
        PrimerLanzamientoRealizado = true;

        foreach (var dado in Dados)
        {
            if (!dado.EstaBloqueado) dado.Lanzar(_rng);
            dado.FinalizarAnimacion();
        }

        EstaAnimando = false;

        OnDadosResueltos?.Invoke(Dados);
        OnLanzamientosRestantesCambiado?.Invoke(LanzamientosRestantes);
    }

    public void DebugMostrarEstado()
    {
        Debug.Log($"Lanzamientos restantes: {LanzamientosRestantes}/{MAX_LANZAMIENTOS}");
        foreach (var dado in Dados)
            Debug.Log($"{dado}");
    }
}
