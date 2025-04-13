using Common.Middlewares;
using Match.API.DI;
using Match.BusinessLogic.DI;
using Match.BusinessLogic.Hubs;
using Match.DataAccess.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.RegisterDataAccess(builder.Configuration);
builder.Services.RegisterBusinessLogic(builder.Configuration);
builder.Services.RegisterApi(builder.Configuration);


var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();

app.MapHub<ChatHub>("/chat");
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.UseCors("MyCorsPolicy");

app.Run();