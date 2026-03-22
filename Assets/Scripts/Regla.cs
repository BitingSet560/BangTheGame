using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Regla : MonoBehaviour
{

    public bool estadoDeJuego = true;
    public List<Jugador> jugadoresActivos;
    public List<Jugador> jugadoresActivosTEMP;
    public List<Jugador> jugadoresTableroPolicias;
    public List<Jugador> jugadoresTableroForajidos;
    public List<Jugador> jugadoresTableroRenegado;
    // Start is called before the first frame update

    Dictionary<int, List<Rol>> configuracionRoles = new() {
        { 3 , new List<Rol> {
            new Rol { nombre = "Sherrif" },
            new Rol { nombre = "Renegado" },
            new Rol { nombre = "Forajido" },
        } }
    };

    System.Random Random = new System.Random();

    void Start()
    {
        List<Jugador> jugadores = new List<Jugador>();
        jugadores.Add( new Jugador() { 
            rol = new Rol(), nombre = "Endo", balas = 8    
        });
        jugadores.Add(new Jugador()
        {
            rol = new Rol(),
            nombre = "Mike",
            balas = 7
        });
        jugadores.Add(new Jugador()
        {
            rol = new Rol(),
            nombre = "Papu",
            balas = 4
        });

        System.Console.WriteLine(jugadores);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    List<Rol> mezclarRoles(List<Rol> listaRoles)
    {
        return listaRoles.OrderBy(x => Random.Next()).ToList();
    }
    void asignarRoles(List<Jugador> listaJugadores)
    {
        var roles = configuracionRoles[listaJugadores.Count];
        var rolesMezcla = mezclarRoles(roles);
        for (int i = 0; i <= listaJugadores.Count; i++)
        {
            listaJugadores[i].rol = rolesMezcla[i];
        }
    }
}
