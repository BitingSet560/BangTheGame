using UnityEngine;
using UnityEngine.UI;
using System;

public class JugadorItemUI : MonoBehaviour
{
    public Text txtNombre;
    public Button btnEliminar;

    public void Configurar(string nombre, Action onRemove)
    {
        txtNombre.text = nombre;

        btnEliminar.onClick.RemoveAllListeners();
        btnEliminar.onClick.AddListener(() => onRemove());
    }
}