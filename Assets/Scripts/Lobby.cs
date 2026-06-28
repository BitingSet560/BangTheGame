using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    public TMP_InputField inputNombre;
    public Button btnAgregar;
    public Button btnIniciar;

    public Transform content; // ScrollView/Viewport/Content
    public GameObject jugadorPrefab;

    private readonly List<Jugador> jugadores = new List<Jugador>();

    void Start()
    {
        btnAgregar.onClick.AddListener(AgregarJugador);
        btnIniciar.onClick.AddListener(IniciarPartida);
    }

    void AgregarJugador()
    {
        string nombre = inputNombre.text.Trim();

        if (string.IsNullOrEmpty(nombre)) return;

        if (jugadores.Exists(j => j.nombre == nombre))
        {
            Debug.Log("Nombre repetido");
            return;
        }

        if (jugadores.Count >= 8)
        {
            Debug.Log("Máximo 8 jugadores");
            return;
        }

        GameObject go = new GameObject(nombre);
        Jugador jugador = go.AddComponent<Jugador>();
        jugador.nombre = nombre;

        jugadores.Add(jugador);

        CrearUIJugador(jugador);

        inputNombre.text = "";
    }

    void CrearUIJugador(Jugador jugador)
    {
        GameObject item = Instantiate(jugadorPrefab, content);

        var ui = item.GetComponent<JugadorItemUI>();

        if (ui == null)
        {
            Debug.LogError($"[LobbyManager] El prefab '{jugadorPrefab.name}' no tiene el componente JugadorItemUI. Agrégalo desde el Inspector.", item);
            return;
        }

        ui.Configurar(jugador.nombre, () =>
        {
            jugadores.Remove(jugador);
            Destroy(item);
        });
    }

    void IniciarPartida()
    {
        if (jugadores.Count < 3)
        {
            Debug.Log("Mínimo 3 jugadores");
            return;
        }

        // Asignar roles a los jugadores
        Regla regla = FindFirstObjectByType<Regla>();
        if (regla == null)
        {
            regla = gameObject.AddComponent<Regla>();
        }
        regla.AsignarRoles(jugadores);

        if (GameManager.Instance == null)
        {
            GameObject gm = new GameObject("GameManager");
            gm.AddComponent<GameManager>();
        }

        GameManager.Instance.jugadores   = jugadores;
        GameManager.Instance.rondaActual = 1;
        GameManager.Instance.indiceTurno = 0;
        foreach (var j in jugadores)
        {
            Debug.Log($"{j.nombre} → {j.rol.nombre}");
            DontDestroyOnLoad(j.gameObject);
        }

        // Cargar la escena de partida
        SceneManager.LoadScene("Game");
    }
}