using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Panel de los dados
public class PanelDadosUI : MonoBehaviour
{
    public Transform panelDados;

    public Text txtLanzamientosRestantes;

    public Button btnLanzar;

    public Text txtLog;

    private const float DADO_ANCHO     = 160f;   
    private const float DADO_ALTO      = 160f;   
    private const int   FONT_EMOJI     = 52;     
    private const int   FONT_NOMBRE    = 16;     

    private static readonly Color COLOR_DADO_NORMAL    = new Color(0.18f, 0.11f, 0.06f, 0.97f);
    private static readonly Color COLOR_DADO_BLOQUEADO = new Color(0.55f, 0.41f, 0.08f, 1.00f);
    private static readonly Color COLOR_DADO_ANIMANDO  = new Color(0.12f, 0.08f, 0.04f, 0.97f);
    private static readonly Color COLOR_BORDE_BLOQUEO  = new Color(1.00f, 0.85f, 0.20f, 1.00f);
    private static readonly Color COLOR_TEXTO_SIMBOLO  = Color.white;
    private static readonly Color COLOR_TEXTO_NOMBRE   = new Color(0.96f, 0.90f, 0.78f);

    private readonly List<DadoUI> _dadoUIs = new List<DadoUI>();

    private void Awake()
    {
        if (btnLanzar != null)
            btnLanzar.onClick.AddListener(OnLanzar);
    }

    private void Start()
    {
        if (GestorDados.Instance != null)
            Inicializar();
        else
            Debug.LogWarning("[PanelDadosUI] GestorDados.Instance es null en Start. " +
                             "Asegúrate de que GestorDados está en la escena.");
    }

    private void OnEnable()
    {
        if (GestorDados.Instance != null) SuscribirEventos();
    }

    private void OnDisable()  => DesuscribirEventos();
    private void OnDestroy()
    {
        DesuscribirEventos();
        if (btnLanzar != null) btnLanzar.onClick.RemoveListener(OnLanzar);
    }

    private void Inicializar()
    {
        CrearWidgetsDados();
        SuscribirEventos();
        GestorDados.Instance.IniciarNuevoTurno();
        ActualizarContador(GestorDados.Instance.LanzamientosRestantes);
        ActualizarBloqueabilidadDados();
    }

    private void CrearWidgetsDados()
    {
        if (panelDados == null)
        {
            Debug.LogError("[PanelDadosUI] panelDados no está asignado.");
            return;
        }

        foreach (Transform child in panelDados)
            Destroy(child.gameObject);
        _dadoUIs.Clear();

        for (int i = 0; i < GestorDados.TOTAL_DADOS; i++)
        {
            var go   = new GameObject($"Dado_{i}");
            go.transform.SetParent(panelDados, false);

            var rect = go.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(DADO_ANCHO, DADO_ALTO);

            var imgFondo = go.AddComponent<Image>();
            imgFondo.color = COLOR_DADO_NORMAL;

            var btn = go.AddComponent<Button>();
            var btnColors = btn.colors;
            btnColors.normalColor      = Color.white;
            btnColors.highlightedColor = new Color(0.80f, 0.80f, 0.80f, 1f);
            btnColors.pressedColor     = new Color(0.60f, 0.60f, 0.60f, 1f);
            btnColors.disabledColor    = new Color(0.50f, 0.50f, 0.50f, 0.50f);
            btn.colors = btnColors;

            // Borde
            var goBorde = new GameObject("Borde");
            goBorde.transform.SetParent(go.transform, false);
            var rectBorde = goBorde.AddComponent<RectTransform>();
            rectBorde.anchorMin = Vector2.zero;
            rectBorde.anchorMax = Vector2.one;
            rectBorde.offsetMin = new Vector2(-4f, -4f);
            rectBorde.offsetMax = new Vector2( 4f,  4f);
            var imgBorde = goBorde.AddComponent<Image>();
            imgBorde.color   = COLOR_BORDE_BLOQUEO;
            imgBorde.enabled = false;

            // Número del dado
            var goNum  = new GameObject("TxtNumero");
            goNum.transform.SetParent(go.transform, false);
            var rectNum = goNum.AddComponent<RectTransform>();
            rectNum.anchorMin = new Vector2(0f, 0.82f);
            rectNum.anchorMax = new Vector2(0.35f, 1f);
            rectNum.offsetMin = new Vector2(6f, 0f);
            rectNum.offsetMax = new Vector2(0f, -4f);
            var txtNum = goNum.AddComponent<Text>();
            txtNum.text      = $"{i + 1}";
            txtNum.fontSize  = 14;
            txtNum.alignment = TextAnchor.UpperLeft;
            txtNum.color     = new Color(0.65f, 0.55f, 0.40f, 0.8f);
            txtNum.font      = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            // Icono
            var goEmoji  = new GameObject("TxtSimbolo");
            goEmoji.transform.SetParent(go.transform, false);
            var rectEmoji = goEmoji.AddComponent<RectTransform>();
            rectEmoji.anchorMin = new Vector2(0f, 0.32f);
            rectEmoji.anchorMax = new Vector2(1f, 0.92f);
            rectEmoji.offsetMin = Vector2.zero;
            rectEmoji.offsetMax = Vector2.zero;
            var txtEmoji = goEmoji.AddComponent<Text>();
            txtEmoji.text      = "—";
            txtEmoji.fontSize  = FONT_EMOJI;
            txtEmoji.alignment = TextAnchor.MiddleCenter;
            txtEmoji.color     = COLOR_TEXTO_SIMBOLO;
            txtEmoji.font      = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            txtEmoji.horizontalOverflow = HorizontalWrapMode.Overflow;
            txtEmoji.verticalOverflow   = VerticalWrapMode.Overflow;

            // Texto
            var goNombre  = new GameObject("TxtNombre");
            goNombre.transform.SetParent(go.transform, false);
            var rectNombre = goNombre.AddComponent<RectTransform>();
            rectNombre.anchorMin = new Vector2(0f, 0f);
            rectNombre.anchorMax = new Vector2(1f, 0.32f);
            rectNombre.offsetMin = new Vector2(4f, 4f);
            rectNombre.offsetMax = new Vector2(-4f, 0f);

            var imgNombreBg = goNombre.AddComponent<Image>();
            imgNombreBg.color = new Color(0f, 0f, 0f, 0.35f);

            var txtNombre = new GameObject("TxtLabel");
            txtNombre.transform.SetParent(goNombre.transform, false);
            var rectLabel = txtNombre.AddComponent<RectTransform>();
            rectLabel.anchorMin = Vector2.zero;
            rectLabel.anchorMax = Vector2.one;
            rectLabel.offsetMin = new Vector2(2f, 2f);
            rectLabel.offsetMax = new Vector2(-2f, -2f);
            var txt = txtNombre.AddComponent<Text>();
            txt.text      = "---";
            txt.fontSize  = FONT_NOMBRE;
            txt.alignment = TextAnchor.MiddleCenter;
            txt.fontStyle = FontStyle.Bold;
            txt.color     = COLOR_TEXTO_NOMBRE;
            txt.font      = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            txt.horizontalOverflow = HorizontalWrapMode.Overflow;
            txt.verticalOverflow   = VerticalWrapMode.Overflow;

            // Candado
            var goLock  = new GameObject("TxtLock");
            goLock.transform.SetParent(go.transform, false);
            var rectLock = goLock.AddComponent<RectTransform>();
            rectLock.anchorMin = new Vector2(0.65f, 0.82f);
            rectLock.anchorMax = new Vector2(1f, 1f);
            rectLock.offsetMin = new Vector2(0f, 0f);
            rectLock.offsetMax = new Vector2(-6f, -4f);
            var txtLock = goLock.AddComponent<Text>();
            txtLock.text      = "[X]";
            txtLock.fontSize  = 14;
            txtLock.alignment = TextAnchor.UpperRight;
            txtLock.color     = new Color(1f, 0.85f, 0.20f, 1f);
            txtLock.font      = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            txtLock.enabled   = false;

            var dadoUI = go.AddComponent<DadoUI>();
            dadoUI.txtSimbolo  = txtEmoji;
            dadoUI.txtNombre   = txt;
            dadoUI.imgFondo    = imgFondo;
            dadoUI.imgBorde    = imgBorde;
            dadoUI.colorNormal    = COLOR_DADO_NORMAL;
            dadoUI.colorBloqueado = COLOR_DADO_BLOQUEADO;
            dadoUI.colorAnimando  = COLOR_DADO_ANIMANDO;
            dadoUI.Inicializar(i);

            _dadoUIs.Add(dadoUI);
        }
    }

    private void SuscribirEventos()
    {
        var g = GestorDados.Instance;
        if (g == null) return;
        g.OnDadosResueltos                += OnDadosResueltos;
        g.OnDadoBloqueoCambiado           += OnDadoBloqueoCambiado;
        g.OnLanzamientosRestantesCambiado += ActualizarContador;
    }

    private void DesuscribirEventos()
    {
        var g = GestorDados.Instance;
        if (g == null) return;
        g.OnDadosResueltos                -= OnDadosResueltos;
        g.OnDadoBloqueoCambiado           -= OnDadoBloqueoCambiado;
        g.OnLanzamientosRestantesCambiado -= ActualizarContador;
    }

    private void OnDadosResueltos(Dado[] dados)
    {
        for (int i = 0; i < _dadoUIs.Count && i < dados.Length; i++)
            _dadoUIs[i].Actualizar(dados[i]);

        ActualizarBloqueabilidadDados();

        if (!GestorDados.Instance.EstaAnimando)
        {
            string resumen = GestorDados.Instance.ObtenerResumenResultados();
            AgregarLog($"  [dados] Resultado: {resumen}");

            var conteos = GestorDados.Instance.ObtenerConteos();
            if (conteos[SimboloDado.Gatling]  >= 3) AgregarLog("  ### METRALLETA! Todos los rivales pierden 1 PV");
            if (conteos[SimboloDado.Dinamita] >= 3) AgregarLog("  *** DINAMITA! Jugador actual pierde 1 PV y termina su turno.");
        }
    }

    private void OnDadoBloqueoCambiado(Dado dado)
    {
        if (dado.Indice < _dadoUIs.Count)
            _dadoUIs[dado.Indice].Actualizar(dado);

        string estado = dado.EstaBloqueado ? "[X] bloqueado" : "[ ] desbloqueado";
        AgregarLog($"  Dado {dado.Indice + 1} {estado}: {dado.Resultado.NombreCorto()}");
    }

    private void ActualizarContador(int restantes)
    {
        if (txtLanzamientosRestantes != null)
        {
            txtLanzamientosRestantes.text = restantes switch
            {
                3 => ">> Tirada 1 de 3",
                2 => ">> Tirada 2 de 3",
                1 => ">> Tirada 3 de 3  (ultima)",
                0 => "[OK] Sin tiradas -- Termina el turno",
                _ => $"Lanzamientos: {restantes}"
            };
        }

        if (btnLanzar != null)
            btnLanzar.interactable = restantes > 0 && !GestorDados.Instance.EstaAnimando;
    }

    private void ActualizarBloqueabilidadDados()
    {
        for (int i = 0; i < _dadoUIs.Count; i++)
            if (GestorDados.Instance != null)
                _dadoUIs[i].Actualizar(GestorDados.Instance.Dados[i]);
    }

    // Lanzar
    private void OnLanzar()
    {
        if (GestorDados.Instance == null) return;

        bool lanzado = GestorDados.Instance.Lanzar();
        if (!lanzado) return;

        string jugador = GameManager.Instance?.JugadorActual?.nombre ?? "?";
        int tiradaNum  = GestorDados.MAX_LANZAMIENTOS - GestorDados.Instance.LanzamientosRestantes;
        AgregarLog($"  [>>] {jugador} -- Tirada {tiradaNum}/{GestorDados.MAX_LANZAMIENTOS}");

        if (btnLanzar != null) btnLanzar.interactable = false;
    }

    public void ResetearParaNuevoTurno()
    {
        if (GestorDados.Instance != null)
            GestorDados.Instance.IniciarNuevoTurno();

        for (int i = 0; i < _dadoUIs.Count; i++)
            if (GestorDados.Instance != null)
                _dadoUIs[i].Actualizar(GestorDados.Instance.Dados[i]);

        ActualizarContador(GestorDados.Instance?.LanzamientosRestantes ?? GestorDados.MAX_LANZAMIENTOS);
    }

    private void AgregarLog(string mensaje)
    {
        if (txtLog == null) return;
        txtLog.text += $"\n{mensaje}";
    }
}
