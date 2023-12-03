using System.Text.Json;
using Microsoft.AspNetCore.OData;
using RL.Data;
using MediatR;
using Microsoft.AspNetCore.Cors.Infrastructure;
// using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMediatR(typeof(Program));
builder.Services.AddSqlite<RLContext>("Data Source=Database.db");
builder.Services.AddControllers()
    .AddOData(options => options.Select().Filter().Expand().OrderBy())
    .AddJsonOptions(options =>
    {
        // options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        // options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<EnableQueryFiler>();
});
// builder.services.AddSwaggerGen(c =>
// {
//     c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
// });

var corsPolicy = "allowLocal";
builder.Services.AddCors(options =>
{
    var policyBuilder = new CorsPolicyBuilder();
    policyBuilder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
    // .SetPreflightMaxAge(TimeSpace.FromSeconds(3600));
    options.AddPolicy(name: "CorsPolicy", policyBuilder.Build());
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RL v1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();