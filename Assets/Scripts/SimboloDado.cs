public enum SimboloDado
{
    Bang = 0,
    Flecha = 1,
    Cerveza = 2,
    Gatling = 3,
    Dinamita = 4,
    Default = 5
}

public static class SimboloDadoExtensions
{
    public static string Emoji(this SimboloDado s) => s switch
    {
        SimboloDado.Bang     => "BANG",
        SimboloDado.Flecha   => "FLECHA",
        SimboloDado.Cerveza  => "CERVEZA",
        SimboloDado.Gatling  => "METRALLETA",
        SimboloDado.Dinamita => "DINAMITA",
        _                    => "?"
    };

    public static string Icono(this SimboloDado s) => s switch
    {
        SimboloDado.Bang     => "! !",    
        SimboloDado.Flecha   => ">> ",    
        SimboloDado.Cerveza  => "~ ~",    
        SimboloDado.Gatling  => "# #",    
        SimboloDado.Dinamita => "* *",    
        _                    => " ? "
    };

    public static string NombreCorto(this SimboloDado s) => s switch
    {
        SimboloDado.Bang     => "BANG",
        SimboloDado.Flecha   => "FLECHA",
        SimboloDado.Cerveza  => "CERVEZA",
        SimboloDado.Gatling  => "METRALLETA",
        SimboloDado.Dinamita => "DINAMITA",
        _                    => "?"
    };

    public static string Descripcion(this SimboloDado s) => s switch
    {
        SimboloDado.Bang     => "Ataca al objetivo (-1 PV)",
        SimboloDado.Flecha   => "Toma una flecha",
        SimboloDado.Cerveza  => "Recupera 1 PV",
        SimboloDado.Gatling  => "Con 3 METRALLETAS: todos los rivales pierden 1 PV",
        SimboloDado.Dinamita => "Con 3 DINAMITAS: jugador actual pierde 1 PV y termina su turno",
        _                    => ""
    };

    public static UnityEngine.Color ColorAccento(this SimboloDado s) => s switch
    {
        SimboloDado.Bang     => new UnityEngine.Color(0.95f, 0.22f, 0.10f), 
        SimboloDado.Flecha   => new UnityEngine.Color(0.85f, 0.50f, 0.10f), 
        SimboloDado.Cerveza  => new UnityEngine.Color(0.95f, 0.82f, 0.18f), 
        SimboloDado.Gatling  => new UnityEngine.Color(0.72f, 0.58f, 0.12f), 
        SimboloDado.Dinamita => new UnityEngine.Color(0.65f, 0.65f, 0.82f), 
        _                    => UnityEngine.Color.white
    };

    public static UnityEngine.Color ColorFondo(this SimboloDado s) => s switch
    {
        SimboloDado.Bang     => new UnityEngine.Color(0.24f, 0.09f, 0.07f), 
        SimboloDado.Flecha   => new UnityEngine.Color(0.22f, 0.13f, 0.06f), 
        SimboloDado.Cerveza  => new UnityEngine.Color(0.20f, 0.18f, 0.05f), 
        SimboloDado.Gatling  => new UnityEngine.Color(0.18f, 0.15f, 0.05f), 
        SimboloDado.Dinamita => new UnityEngine.Color(0.14f, 0.13f, 0.18f), 
        _                    => new UnityEngine.Color(0.18f, 0.11f, 0.06f)
    };
    private static readonly SimboloDado[] CarasEstandar =
    {
        SimboloDado.Bang,
        SimboloDado.Flecha,
        SimboloDado.Cerveza,
        SimboloDado.Gatling,
        SimboloDado.Dinamita
    };

    public static SimboloDado[] GetCarasEstandar() => (SimboloDado[])CarasEstandar.Clone();
}
