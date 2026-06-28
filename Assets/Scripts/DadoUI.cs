using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// Controla la representación visual del dado

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class DadoUI : MonoBehaviour
{
    // Referencias UI

    [Tooltip("Icono del dado")]
    public Text txtSimbolo;

    [Tooltip("Nombre del dado")]
    public Text txtNombre;

    [Tooltip("Imagen de fondo del dado")]
    public Image imgFondo;

    [Tooltip("Imagen del borde que indica si está bloqueado")]
    public Image imgBorde;

    // Paleta de colores

    [Header("Colores")]
    public Color colorNormal       = new Color(0.18f, 0.11f, 0.06f, 0.97f);
    public Color colorBloqueado    = new Color(0.55f, 0.41f, 0.08f, 1.00f);
    public Color colorAnimando     = new Color(0.12f, 0.08f, 0.04f, 0.97f);
    public Color colorBordeBloqueo = new Color(1.00f, 0.85f, 0.20f, 1.00f);

    // Estado privado

    private int    _indiceDado;
    private bool   _animando;
    private Button _boton;

    // Iconos ASCII para animación
    private static readonly string[] IconosAnimacion =
        { "! !", ">> ", "~ ~", "# #", "* *", "- -" };

    private static readonly UnityEngine.Color[] ColoresAnimacion =
    {
        new UnityEngine.Color(0.95f, 0.22f, 0.10f),
        new UnityEngine.Color(0.85f, 0.50f, 0.10f),
        new UnityEngine.Color(0.95f, 0.82f, 0.18f),
        new UnityEngine.Color(0.72f, 0.58f, 0.12f),
        new UnityEngine.Color(0.65f, 0.65f, 0.82f),
        new UnityEngine.Color(0.55f, 0.52f, 0.42f),
    };

    // Inicialización

    private void Awake()
    {
        _boton = GetComponent<Button>();
        _boton.onClick.AddListener(OnClickDado);
    }

    private void OnDestroy()
    {
        if (_boton != null) _boton.onClick.RemoveListener(OnClickDado);
    }

    /// Inicia el dado
    public void Inicializar(int indiceDado)
    {
        _indiceDado = indiceDado;
    }

    // Actualización visual

    /// Actualiza el diseño del dado
    public void Actualizar(Dado dado)
    {
        if (dado == null) return;
        _animando = dado.EstaAnimando;

        // Fondo
        if (imgFondo != null)
            imgFondo.color = _animando        ? colorAnimando  :
                             dado.EstaBloqueado ? colorBloqueado :
                             dado.Resultado.ColorFondo();

        // Borde de bloqueo
        if (imgBorde != null)
            imgBorde.enabled = dado.EstaBloqueado;

        // Icono
        if (txtSimbolo != null)
        {
            if (_animando)
            {
                int idx = UnityEngine.Random.Range(0, IconosAnimacion.Length);
                txtSimbolo.text  = IconosAnimacion[idx];
                txtSimbolo.color = ColoresAnimacion[idx];
            }
            else
            {
                txtSimbolo.text  = dado.Resultado.Icono();
                txtSimbolo.color = dado.Resultado.ColorAccento();
            }
        }

        // Nombre del símbolo
        if (txtNombre != null)
            txtNombre.text = _animando ? "..." : dado.Resultado.NombreCorto();

        // Interactividad del botón
        if (_boton != null)
        {
            var gestor = GestorDados.Instance;
            _boton.interactable = gestor != null
                && gestor.PrimerLanzamientoRealizado
                && gestor.LanzamientosRestantes > 0
                && !gestor.EstaAnimando;
        }
    }

    // Interacción
    private void OnClickDado()
    {
        if (GestorDados.Instance == null) return;
        bool cambiado = GestorDados.Instance.AlternarBloqueo(_indiceDado);
        if (cambiado)
            Actualizar(GestorDados.Instance.Dados[_indiceDado]);
    }
}
