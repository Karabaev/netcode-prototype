using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();
builder.Services.AddMagicOnion();
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