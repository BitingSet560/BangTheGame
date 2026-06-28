using UnityEngine;
using UnityEngine.UI;

// Controla lo principal de la escena de partida
public class GameUI : MonoBehaviour
{
    // Header
    public Text txtTurno;
    public Text txtRonda;

    // Panel Jugadores
    public Transform panelJugadores;

    // Dados (manejados por PanelDadosUI)
    public Text   txtLanzamientosRestantes;
    public Button btnLanzar;
    public Button btnTerminar;

    // Log
    public Text txtLog;

    // Ciclo de vida

    void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("No se logro iniciar la partida.");
            return;
        }

        InicializarUI();
        if (btnTerminar != null) btnTerminar.onClick.AddListener(OnTerminarTurno);
    }

    // INICIALIZACIÓN

    void InicializarUI()
    {
        ActualizarHeader();
        CrearTarjetasJugadores();

        AgregarLog("★  ¡Comienza la partida!  ★");
        AgregarLog($"► Turno de  {GameManager.Instance.JugadorActual?.nombre}");
    }

    // ACTUALIZAR UI

    void ActualizarHeader()
    {
        var jugador = GameManager.Instance.JugadorActual;
        if (txtTurno != null)
            txtTurno.text = jugador != null
                ? $"TURNO DE:  {jugador.nombre.ToUpper()}"
                : "TURNO DE:  ---";
        if (txtRonda != null)
            txtRonda.text = $"RONDA  {GameManager.Instance.rondaActual}";
    }

    void CrearTarjetasJugadores()
    {
        if (panelJugadores == null) return;

        foreach (Transform child in panelJugadores)
            Destroy(child.gameObject);

        var jugadores = GameManager.Instance.jugadores;
        var activo    = GameManager.Instance.JugadorActual;

        foreach (var jugador in jugadores)
        {
            bool esActivo  = jugador == activo;
            bool esSheriff = jugador.rol?.nombre == "Sheriff";

            // Contenedor
            var card = new GameObject($"Card_{jugador.nombre}");
            card.transform.SetParent(panelJugadores, false);

            var rect = card.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(160f, 110f);

            var img = card.AddComponent<Image>();
            img.color = esActivo
                ? new Color(0.55f, 0.41f, 0.08f, 1f)
                : new Color(0.18f, 0.11f, 0.06f, 0.97f);

            // Borde cuando es el jugador activo
            if (esActivo)
            {
                var goBorde = new GameObject("Borde");
                goBorde.transform.SetParent(card.transform, false);
                var rBorde = goBorde.AddComponent<RectTransform>();
                rBorde.anchorMin = Vector2.zero;
                rBorde.anchorMax = Vector2.one;
                rBorde.offsetMin = new Vector2(-3f, -3f);
                rBorde.offsetMax = new Vector2( 3f,  3f);
                var iBorde = goBorde.AddComponent<Image>();
                iBorde.color = new Color(1f, 0.85f, 0.20f, 1f);
                goBorde.transform.SetAsFirstSibling();
            }

            // Nombre
            CrearTextoHijo("TxtNombre", card.transform,
                jugador.nombre.ToUpper(),
                fontSize:    18,
                estilo:      FontStyle.Bold,
                alineacion:  TextAnchor.MiddleCenter,
                color:       new Color(0.96f, 0.90f, 0.78f),
                anchorMin:   new Vector2(0f, 0.55f),
                anchorMax:   new Vector2(1f, 1.00f),
                offsetMin:   new Vector2(6f,  4f),
                offsetMax:   new Vector2(-6f,-4f));

            // Rol
            string textoRol = esSheriff ? "★ SHERIFF" :
                              jugador.rol?.nombre == "Ayudante" ? "✦ AYUDANTE" :
                              jugador.rol?.nombre == "Forajido" ? "◈ FORAJIDO" :
                              jugador.rol?.nombre == "Renegado" ? "◆ RENEGADO" : "???";
            Color colorRol = esSheriff
                ? new Color(1.00f, 0.80f, 0.10f)
                : new Color(0.65f, 0.55f, 0.42f);
            CrearTextoHijo("TxtRol", card.transform,
                textoRol,
                fontSize:    13,
                estilo:      FontStyle.Normal,
                alineacion:  TextAnchor.MiddleCenter,
                color:       colorRol,
                anchorMin:   new Vector2(0f, 0.28f),
                anchorMax:   new Vector2(1f, 0.55f),
                offsetMin:   new Vector2(6f, 0f),
                offsetMax:   new Vector2(-6f,0f));

            // HP (balas)
            int hp = jugador.balas > 0 ? jugador.balas : (esSheriff ? 5 : 4);
            string hpStr = new string('●', hp) + new string('○', Mathf.Max(0, 9 - hp));
            CrearTextoHijo("TxtHP", card.transform,
                hpStr,
                fontSize:    14,
                estilo:      FontStyle.Normal,
                alineacion:  TextAnchor.MiddleCenter,
                color:       new Color(0.90f, 0.25f, 0.25f),
                anchorMin:   new Vector2(0f, 0f),
                anchorMax:   new Vector2(1f, 0.28f),
                offsetMin:   new Vector2(4f, 4f),
                offsetMax:   new Vector2(-4f,0f));
        }
    }

    // ACCIONES DE BOTONES

    void OnTerminarTurno()
    {
        GameManager.Instance.SiguienteTurno();

        var panelDados = FindObjectOfType<PanelDadosUI>();
        panelDados?.ResetearParaNuevoTurno();

        ActualizarHeader();
        CrearTarjetasJugadores();
        AgregarLog($"► Turno de {GameManager.Instance.JugadorActual?.nombre}");
    }

    // HELPERS

    void AgregarLog(string mensaje)
    {
        if (txtLog == null) return;
        txtLog.text += $"\n{mensaje}";
    }

    static void CrearTextoHijo(string nombre, Transform padre,
        string contenido, int fontSize, FontStyle estilo, TextAnchor alineacion, Color color,
        Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
    {
        var go   = new GameObject(nombre);
        go.transform.SetParent(padre, false);
        var rect = go.AddComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = offsetMin;
        rect.offsetMax = offsetMax;
        var txt  = go.AddComponent<Text>();
        txt.text      = contenido;
        txt.fontSize  = fontSize;
        txt.fontStyle = estilo;
        txt.alignment = alineacion;
        txt.color     = color;
        txt.font      = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        txt.horizontalOverflow = HorizontalWrapMode.Overflow;
        txt.verticalOverflow   = VerticalWrapMode.Overflow;
    }
}
