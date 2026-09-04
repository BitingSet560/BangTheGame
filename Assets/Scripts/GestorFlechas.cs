using System;
using UnityEngine;

public class GestorFlechas : MonoBehaviour
{
    public static GestorFlechas Instance { get; private set; }

    public const int TOTAL_FLECHAS = 9;

    public int FlechasDisponibles { get; private set; }

    public event Action<int> OnFlechasDisponiblesCambiadas;
    public event Action<Jugador, int> OnJugadorRecibeFlechas;
    public event Action OnAtaqueIndigena;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        FlechasDisponibles = TOTAL_FLECHAS;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void OnEnable()
    {
        if (GestorDados.Instance != null)
        {
            GestorDados.Instance.OnTiradaFinalizada +=
                RevisarFlechasDeTirada;
        }
    }

    private void OnDisable()
    {
        if (GestorDados.Instance != null)
        {
            GestorDados.Instance.OnTiradaFinalizada -=
                RevisarFlechasDeTirada;
        }
    }

    public void Reiniciar()
    {
        FlechasDisponibles = TOTAL_FLECHAS;

        OnFlechasDisponiblesCambiadas?.Invoke(
            FlechasDisponibles
        );
    }


    public void TomarFlechas(
        Jugador jugador,
        int cantidad)
    {
        if (jugador == null)
        {
            Debug.LogError(
                "No se puede dar flechas a un jugador null."
            );
            return;
        }

        if (cantidad <= 0)
            return;


        for (int i = 0; i < cantidad; i++)
        {
            // El jugador toma una flecha
            if (FlechasDisponibles > 0)
            {
                FlechasDisponibles--;

                jugador.flechas++;

                OnJugadorRecibeFlechas?.Invoke(
                    jugador,
                    1
                );

                OnFlechasDisponiblesCambiadas?.Invoke(
                    FlechasDisponibles
                );
            }


            // Si se acabaron las flechas,
            // ocurre el ataque indígena
            if (FlechasDisponibles <= 0)
            {
                ResolverAtaqueIndigena();
            }
        }
    }

    private void RevisarFlechasDeTirada(Dado[] dados)
    {
        Jugador jugadorActual =
            GameManager.Instance.JugadorActual;

        if (jugadorActual == null)
            return;

        foreach (Dado dado in dados)
        {
            // Solo nos interesan flechas nuevas
            if (dado.Resultado != SimboloDado.Flecha)
                continue;

            // Si ya estaba bloqueado por regla,
            // esa flecha ya fue procesada.
            if (dado.BloqueadoPorRegla)
                continue;


            // 1. Bloquear dado automáticamente
            GestorDados.Instance.BloquearPorRegla(dado.Indice);

            // 2. Dar la flecha al jugador
            TomarFlechas(jugadorActual, 1);

            Debug.Log(
                $"{jugadorActual.nombre} obtiene una flecha " +
                $"del dado {dado.Indice + 1}"
            );
        }
    }

    private void ResolverAtaqueIndigena()
    {
        Debug.Log(
            "¡ATAQUE INDÍGENA!"
        );

        OnAtaqueIndigena?.Invoke();


        foreach (Jugador jugador
            in GameManager.Instance.jugadores)
        {
            if (jugador.flechas <= 0)
                continue;

            int danio = jugador.flechas;

            jugador.RecibirDanio(danio);
        }


        // Todas las flechas regresan al centro
        foreach (Jugador jugador
            in GameManager.Instance.jugadores)
        {
            jugador.flechas = 0;
        }


        FlechasDisponibles = TOTAL_FLECHAS;

        OnFlechasDisponiblesCambiadas?.Invoke(
            FlechasDisponibles
        );
    }
}