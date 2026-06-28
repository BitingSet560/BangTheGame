using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Regla : MonoBehaviour
{

    public bool estadoDeJuego = true;
    public List<Jugador> jugadoresPolicias;
    public List<Jugador> jugadoresForajidos;
    public List<Jugador> jugadoresRenegado;
    // Start is called before the first frame update

    private readonly Dictionary<int, List<Rol>> configuracionRoles = new()
    {
        { 3, new List<Rol> {
            new() { nombre = "Sheriff" },
            new() { nombre = "Renegado" },
            new() { nombre = "Forajido" },
        }},
        { 4, new List<Rol> {
            new() { nombre = "Sheriff" },
            new() { nombre = "Renegado" },
            new() { nombre = "Forajido" },
            new() { nombre = "Forajido" },
        }},
        { 5, new List<Rol> {
            new() { nombre = "Sheriff" },
            new() { nombre = "Renegado" },
            new() { nombre = "Forajido" },
            new() { nombre = "Forajido" },
            new() { nombre = "Ayudante" },
        }},
        { 6, new List<Rol> {
            new() { nombre = "Sheriff" },
            new() { nombre = "Renegado" },
            new() { nombre = "Forajido" },
            new() { nombre = "Forajido" },
            new() { nombre = "Forajido" },
            new() { nombre = "Ayudante" },
        }},
        { 7, new List<Rol> {
            new() { nombre = "Sheriff" },
            new() { nombre = "Renegado" },
            new() { nombre = "Forajido" },
            new() { nombre = "Forajido" },
            new() { nombre = "Forajido" },
            new() { nombre = "Ayudante" },
            new() { nombre = "Ayudante" },
        }},
        { 8, new List<Rol> {
            new() { nombre = "Sheriff" },
            new() { nombre = "Renegado" },
            new() { nombre = "Renegado" },
            new() { nombre = "Forajido" },
            new() { nombre = "Forajido" },
            new() { nombre = "Forajido" },
            new() { nombre = "Ayudante" },
            new() { nombre = "Ayudante" },
        }},
    };

    private readonly System.Random Random = new();

    void Start()
    {
    }

    void Update()
    {
        
    }


    List<Rol> MezclarRoles(List<Rol> listaRoles)
    {
        for (int i = listaRoles.Count - 1; i > 0; i--)
        {
            int j = Random.Next(i + 1);
            (listaRoles[i], listaRoles[j]) = (listaRoles[j], listaRoles[i]);
        }

        return listaRoles;
    }

    public void AsignarRoles(List<Jugador> listaJugadores)
    {
        if (!configuracionRoles.TryGetValue(listaJugadores.Count, out var rolesBase))
        {
            return;
        }

        var roles       = rolesBase.Select(r => new Rol { nombre = r.nombre }).ToList();
        var rolesMezcla = MezclarRoles(roles);

        for (int i = 0; i < listaJugadores.Count; i++)
            listaJugadores[i].rol = rolesMezcla[i];
    }

    void SepararJugadores(List<Jugador> listaJugadores)
    { 
        jugadoresPolicias = listaJugadores.Where(j => j.rol.nombre == "Sheriff" || j.rol.nombre == "Ayudante").ToList();
        jugadoresForajidos = listaJugadores.Where(j => j.rol.nombre == "Forajido").ToList();
        jugadoresRenegado = listaJugadores.Where(j => j.rol.nombre == "Renegado").ToList();
    }
}
