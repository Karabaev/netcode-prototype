namespace Motk.Client.Core
{
  public delegate void NetworkMessageHandler<T>(in T payload) where T : struct;

  public delegate void NetworkMessageHandler();
}