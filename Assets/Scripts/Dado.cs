using System;
using UnityEngine;


[Serializable]
public class Dado
{
    
    // Indica el resultado del dado
    public SimboloDado Resultado { get; private set; }

    // Indica el bloque del dado
    public bool EstaBloqueado { get; private set; }

    // Indica si el dado esta bloqueado por una regla
    public bool BloqueadoPorRegla { get; private set; }

    // Indica si el dado está siendo animado
    public bool EstaAnimando { get; private set; }

    // Indica el índice del dado en el conjunto
    public int Indice { get; private set; }

    // Constructor
    public Dado(int indice)
    {
        Indice          = indice;
        Resultado       = SimboloDado.Default;
        EstaBloqueado   = false;
        EstaAnimando    = false;
    }

    public SimboloDado Lanzar(System.Random rng)
    {
        if (EstaBloqueado)
            return Resultado;

        SimboloDado[] caras = SimboloDadoExtensions.GetCarasEstandar();

        Resultado = caras[rng.Next(caras.Length)];

        return Resultado;
    }

    // Alterna el estado de bloqueo del dado.
    public bool AlternarBloqueo()
    {
        if (BloqueadoPorRegla)
            return false;

        EstaBloqueado = !EstaBloqueado;

        return true;
    }
    // Bloquear dado por reglas
    public void BloquearPorRegla()
    {
        EstaBloqueado = true;
        BloqueadoPorRegla = true;
    }

    // Bloquea el dado
    public void Bloquear()
    {
        if (BloqueadoPorRegla)
            return;

        EstaBloqueado = true;
    }

    // Desbloquea el dado
    public void Desbloquear() => EstaBloqueado = false;

    // Marca el dado como iniciando su animación
    internal void IniciarAnimacion()  => EstaAnimando = true;

    // Marca el dado como finalizado su animación
    internal void FinalizarAnimacion() => EstaAnimando = false;

    // Resetea el dado a su estado inicial para el siguiente turno
    public void ResetearParaTurno()
    {
        EstaBloqueado  = false;
        EstaAnimando   = false;
        BloqueadoPorRegla = false;
        Resultado      = SimboloDado.Default;
    }

    //Log
    public override string ToString() =>
        $"Dado[{Indice}] {Resultado.Emoji()} {(EstaBloqueado ? "(🔒)" : "")}";
}
