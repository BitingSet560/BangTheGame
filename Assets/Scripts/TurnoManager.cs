using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnoManager : MonoBehaviour
{
    public static TurnoManager Instance { get; private set; }

    private Dictionary<SimboloDado, int> resultados;

    private bool resolviendo;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    public void ResolverDadosFinales()
    {
        if (resolviendo)
            return;

        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager no encontrado.");
            return;
        }

        if (GestorDados.Instance == null)
        {
            Debug.LogError("GestorDados no encontrado.");
            return;
        }

        if (!GestorDados.Instance.PrimerLanzamientoRealizado)
        {
            Debug.Log("Debes lanzar los dados antes de resolver.");
            return;
        }

        resultados = GestorDados.Instance.ObtenerConteos();

        StartCoroutine(ResolverSecuencia());
    }


    private IEnumerator ResolverSecuencia()
    {
        resolviendo = true;

        Jugador jugadorActual =
            GameManager.Instance.JugadorActual;


        // =========================
        // 1. DINAMITA
        // =========================

        if (resultados[SimboloDado.Dinamita] >= 3)
        {
            Debug.Log(
                $"{jugadorActual.nombre} obtuvo 3 dinamitas."
            );

            jugadorActual.RecibirDanio(1);

            yield return new WaitForSeconds(1f);

            resolviendo = false;

            FinalizarTurno();

            yield break;
        }


        // =========================
        // 2. GATLING
        // =========================

        if (resultados[SimboloDado.Gatling] >= 3)
        {
            ResolverGatling(jugadorActual);

            yield return new WaitForSeconds(1f);
        }


        // =========================
        // 3. FLECHAS
        // =========================

        int cantidadFlechas =
            resultados[SimboloDado.Flecha];

        if (cantidadFlechas > 0)
        {
            ResolverFlechas(
                jugadorActual,
                cantidadFlechas
            );

            yield return new WaitForSeconds(1f);
        }


        // =========================
        // 4. BANG
        // =========================

        int cantidadBang =
            resultados[SimboloDado.Bang];

        if (cantidadBang > 0)
        {
            Debug.Log(
                $"{jugadorActual.nombre} tiene " +
                $"{cantidadBang} BANG por resolver."
            );

            // Aquí después esperaremos
            // que el jugador seleccione objetivos.
        }


        // =========================
        // 5. CERVEZA
        // =========================

        int cantidadCervezas =
            resultados[SimboloDado.Cerveza];

        if (cantidadCervezas > 0)
        {
            Debug.Log(
                $"{jugadorActual.nombre} tiene " +
                $"{cantidadCervezas} CERVEZAS por resolver."
            );

            // Aquí después podremos seleccionar
            // jugador para curar.
        }


        Debug.Log(
            "Resolución automática terminada."
        );

        resolviendo = false;
    }


    private void ResolverGatling(
        Jugador jugadorActual)
    {
        Debug.Log(
            $"GATLING activado por {jugadorActual.nombre}"
        );

        foreach (Jugador jugador
            in GameManager.Instance.jugadores)
        {
            if (jugador == jugadorActual)
                continue;

            jugador.RecibirDanio(1);
        }
    }


    private void ResolverFlechas(
        Jugador jugadorActual,
        int cantidad)
    {
        Debug.Log(
            $"{jugadorActual.nombre} obtiene " +
            $"{cantidad} flecha(s)."
        );

        // Próximamente:
        // GestorFlechas.Instance.TomarFlechas(
        //     jugadorActual,
        //     cantidad
        // );
    }


    public void FinalizarTurno()
    {
        GameManager.Instance.SiguienteTurno();

        GestorDados.Instance.IniciarNuevoTurno();

        Debug.Log(
            $"Turno de: " +
            $"{GameManager.Instance.JugadorActual.nombre}"
        );
    }
}