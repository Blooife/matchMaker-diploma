using Common.Middlewares;
using User.API.DI;
using User.BusinessLogic.DI;
using User.DataAccess.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterDataAccess(builder.Configuration);
builder.Services.ConfigureBusinessLogic(builder.Configuration);
builder.Services.ConfigureApi(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.MapControllers();
app.UseCors("MyCorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

await app.ApplyMigrations(app.Services);

app.Run();