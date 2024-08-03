using Cysharp.Net.Http;
using Grpc.Net.Client;
using MagicOnion.Client;
using Motk.Combat.Client.gRPC.Motk.Combat.Server.gRPC;
using UnityEngine;

namespace Motk.Combat.Client.gRPC
{
  public class GrpcClient
  {
    public async void Start()
    {
      var channel = GrpcChannel.ForAddress("http://localhost:5001", new GrpcChannelOptions
      {
        HttpHandler = new YetAnotherHttpHandler { Http2Only = true }
      });
      var client = MagicOnionClient.Create<IMyFirstService>(channel);
      var result = await client.SumAsync(123, 456);
      Debug.Log($"Result={result}");
    }
  }
}