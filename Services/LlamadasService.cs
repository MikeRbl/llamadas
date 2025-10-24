namespace MiProyecto8;

public interface ILlamada
{
    Task<string> EjecutarLlamadaAsync(string nombre, int delayEnMilisegundos);
}

public class Llamadas : ILlamada
{
    public async Task<string> EjecutarLlamadaAsync(string nombre, int delayEnMilisegundos)
    {
        await Task.Delay(delayEnMilisegundos);
        
        double segundos = delayEnMilisegundos / 1000.0;

        string resultado = $"{nombre} en tiempo {segundos}s";

        return resultado;
    }
}