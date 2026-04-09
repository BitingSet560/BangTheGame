using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

        FindObjectOfType<Regla>().AsignarRoles(jugadores);

        Debug.Log("Partida iniciada");

        foreach (var j in jugadores)
        {
            Debug.Log($"{j.nombre} → {j.rol.nombre}");
        }
    }
}