namespace MiProyecto8;

public class SaludoService : ISaludoService
{
    public string Saludar(string nombre)
    {
        return $"ey qpd, {nombre}, Saludo desde el servicio inyectado";
    }
}

public class SaludoFormal(string nombre) : ISaludoService
{       
    public string Saludar(string nombre)
    {
        return $"Buen dia, Sr/Sra. {nombre}";
    }
}
public class SaludoInformal(string nombre): ISaludoService
{
    public string Saludar(string nombre)
    {
        return $"Que onda, {nombre}";
    }
}