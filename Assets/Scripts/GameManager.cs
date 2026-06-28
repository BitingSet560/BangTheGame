using System.Collections.Generic;
using UnityEngine;

// Controla lo principal de la partida
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [HideInInspector] public List<Jugador> jugadores = new List<Jugador>();
    [HideInInspector] public int rondaActual  = 1;
    [HideInInspector] public int indiceTurno  = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Determina el jugador actual
    public Jugador JugadorActual =>
        jugadores != null && jugadores.Count > 0
            ? jugadores[indiceTurno % jugadores.Count]
            : null;

    // Determina el siguiente turno
    public void SiguienteTurno()
    {
        indiceTurno++;
        if (jugadores.Count > 0 && indiceTurno % jugadores.Count == 0)
            rondaActual++;
    }
}
