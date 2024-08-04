using Grpc.Net.Client;

namespace Motk.Combat.Client.gRPC
{
  public class GrpcChannelState
  {
    public GrpcChannel GrpcChannel { get; set; } = null!;
  }
}