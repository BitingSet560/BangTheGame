using System;
using UnityEngine;


[Serializable]
public class Dado
{
    
    // Indica el resultado del dado
    public SimboloDado Resultado { get; private set; }

    // Indica el bloque del dado
    public bool EstaBloqueado { get; private set; }

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
    public void AlternarBloqueo()
    {
        EstaBloqueado = !EstaBloqueado;
    }

    // Bloquea el dado
    public void Bloquear() => EstaBloqueado = true;

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
        Resultado      = SimboloDado.Default;
    }

    //Log
    public override string ToString() =>
        $"Dado[{Indice}] {Resultado.Emoji()} {(EstaBloqueado ? "(🔒)" : "")}";
}
