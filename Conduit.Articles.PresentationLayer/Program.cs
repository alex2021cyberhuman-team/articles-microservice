using Conduit.Shared.Events.Models.Users.Register;
using Conduit.Shared.Events.Models.Users.Update;
using Conduit.Shared.Events.Services.RabbitMQ;
using Conduit.Shared.Startup;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

#region ServicesConfiguration

var services = builder.Services;
var environment = builder.Environment;
var configuration = builder.Configuration;

services.AddControllers();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Conduit.Auth.WebApi", Version = "v1" });
});

services.AddHealthChecks().Services
    .AddW3CLogging(configuration.GetSection("W3C").Bind).AddHttpClient()
    .RegisterRabbitMqWithHealthCheck(configuration.GetSection("RabbitMQ").Bind);

#endregion

var app = builder.Build();

#region AppConfiguration

if (environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        c.SwaggerEndpoint("/swagger/v1/swagger.json",
            "Conduit.Auth.WebApi v1"));
    IdentityModelEventSource.ShowPII = true;
}

app.UseW3CLogging();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(x =>
{
    x.MapControllers();
    x.MapHealthChecks("/health");
});

var initializationScope = app.Services.CreateScope();

await initializationScope.WaitHealthyServicesAsync(TimeSpan.FromHours(1));
await initializationScope.InitializeQueuesAsync();

#endregion

app.Run();
