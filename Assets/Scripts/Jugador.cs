using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    public Rol rol;
    public int balas;
    public string nombre;
    //public List<Flecha> flechas;
    //public Personaje personaje;
    //public Utilitarios utilitarios; // Separar responsabilidades *Pendiente*

    void Start()
    {
/*        personaje = utilitarios.asignarPersonaje();
        balas = personaje.vidaMaxima;
        flechas.Clear();*/
        
    }
    void Update()
    {
        
    }

}

public class Utilitarios
{
    public List<Personaje> personajes = new List<Personaje>();
    void start()
    {
        personajes.Add(new Personaje
        {
            nombre = "Bart Cassidy",
            vidaMaxima = 8
        });
        personajes.Add(new Personaje
        {
            nombre = "Black Jack",
            vidaMaxima = 8
        });
        personajes.Add(new Personaje
        {
            nombre = "Calamity Jannet",
            vidaMaxima = 8
        });
        personajes.Add(new Personaje
        {
            nombre = "El Gringo",
            vidaMaxima = 7
        });
        personajes.Add(new Personaje
        {
            nombre = "Jessy Jhones",
            vidaMaxima = 9
        });
        personajes.Add(new Personaje
        {
            nombre = "Jourdonnais",
            vidaMaxima = 7
        });
        personajes.Add(new Personaje
        {
            nombre = "Kit Carlson",
            vidaMaxima = 7
        });
        personajes.Add(new Personaje
        {
            nombre = "Lucky Duke",
            vidaMaxima = 8
        });
        personajes.Add(new Personaje
        {
            nombre = "Paul Regret",
            vidaMaxima = 9
        });
        personajes.Add(new Personaje
        {
            nombre = "Pedro Ramirez",
            vidaMaxima = 8
        });
        personajes.Add(new Personaje
        {
            nombre = "Rose Doolan",
            vidaMaxima = 9
        });
        personajes.Add(new Personaje
        {
            nombre = "Sid Ketchum",
            vidaMaxima = 8
        });
        personajes.Add(new Personaje
        {
            nombre = "Slab El Asesino",
            vidaMaxima = 8
        });
        personajes.Add(new Personaje
        {
            nombre = "Suzy Lafayette",
            vidaMaxima = 8
        });
        personajes.Add(new Personaje
        {
            nombre = "Buitre Sam",
            vidaMaxima = 9
        });
        personajes.Add(new Personaje
        {
            nombre = "Willy El Niño",
            vidaMaxima = 8
        });
    }

    public Personaje asignarPersonaje()
    {
        System.Random numeroRandom = new System.Random();
        int numeroDePersonaje = numeroRandom.Next(0, 15);
        Personaje personaje = personajes[numeroDePersonaje];
        personajes.RemoveAt(numeroDePersonaje);
        return personaje;
    }
}
public class Rol
{
    public string nombre { get; set; }

    // public Habilidades habilidad;
    public int getRol()
    {
        System.Random numeroRandom = new System.Random();
        int equipo = numeroRandom.Next(1, 3);
        return equipo;
    }
}

public class Personaje
{
    public string nombre { get; set; }
    public Habilidades habilidad;
    public int vidaMaxima { get; set; }
}


public class Habilidades
{
    void efecto()
    {

    }


}

public class Flecha
{
    public int TipoFlecha; // 1. Normal y 2.Dorada
}