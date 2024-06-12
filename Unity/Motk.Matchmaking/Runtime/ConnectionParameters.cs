namespace Motk.Matchmaking
{
  public class ConnectionParameters
  {
    public string Host { get; }
    
    public ushort Port { get; }

    public ConnectionParameters(string host, ushort port)
    {
      Host = host;
      Port = port;
    }
  }
}