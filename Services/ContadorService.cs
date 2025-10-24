namespace MiProyecto8;

public interface IContadorService
{
    int ObtenerValor();
}
    
public class ContadorService : IContadorService
{
    private int _contador = 0;
    public ContadorService()
    {
        _contador = new Random().Next(1, 100);
    }
    public int ObtenerValor()
    {
        return _contador;
    }
    
}

