using ApiGateway.Ocelot.DI;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration, builder.Environment);
builder.Services.AddSwaggerForOcelot(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseSwaggerForOcelotUI(options =>
{
    options.PathToSwaggerGenerator = app.Configuration.GetSection("OcelotOptions:PathToSwaggerGenerator").Value;
});

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.UseCors("MyCorsPolicy");

await app
    .UseAuthentication()
    .UseWebSockets()
    .UseOcelot();

app.Run();