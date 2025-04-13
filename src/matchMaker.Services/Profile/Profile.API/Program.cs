using Common.Middlewares;
using Profile.API.DI;
using Profile.BusinessLogic.DI;
using Profile.DataAccess.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterBusinessLogic(builder.Configuration);
builder.Services.RegisterAPI(builder.Configuration);
builder.Services.RegisterDataAccess(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseCors("MyCorsPolicy");
app.MapControllers();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.ApplyMigrations(app.Services);

app.Run();