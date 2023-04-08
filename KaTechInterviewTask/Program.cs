using KaTechInterviewTask.Configs;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;
var services = builder.Services;

var CredentialConfig = Configuration.GetSection("Credentials").Get<CredentialConfig>();
var URLConfig = Configuration.GetSection("URLs").Get<URLConfig>();

services.AddControllers();
services.AddSingleton(CredentialConfig);
services.AddSingleton(URLConfig);



services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
