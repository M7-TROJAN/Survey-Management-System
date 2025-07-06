using Scalar.AspNetCore;
using SurveyBasket.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependecies(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = "swagger";
        options.SwaggerEndpoint("/openapi/v1.json", "TROJAN API");
    });

    // Scalar UI
    app.MapScalarApiReference(options =>
    {
        options.OpenApiRoutePattern = "/openapi/v1.json"; // This should match the OpenAPI route pattern 
        options.Title = "TROJAN API";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();