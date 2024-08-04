using Microsoft.AspNetCore.Server.Kestrel.Core;
using Motk.Combat.Server.gRPC;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<MetaStoreStub>();

builder.Services.AddGrpc();
builder.Services.AddMagicOnion();
builder.Services.AddLogging();
builder.WebHost.UseKestrel(options =>
{
  options.ConfigureEndpointDefaults(endPointOptions =>
  {
    endPointOptions.Protocols = HttpProtocols.Http2;
  });
});

var app = builder.Build();
app.MapMagicOnionService();
app.Run();