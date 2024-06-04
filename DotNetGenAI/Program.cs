using Amazon.BedrockRuntime;
using DotNetGenAI.Models;
using DotNetGenAI.Models.Cohere;
using System.Text.Json;
using System.Text;
using DotNetGenAI.Models.StableDiffusion;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<AmazonBedrockRuntimeClient>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Temp1", "Temp2"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

// Prompts endpoint
app.MapPost("/prompts/text", async (AmazonBedrockRuntimeClient client, TextPromptRequest request) =>
{
    var coherePrompt = new CoherePrompt(request.Prompt);

    var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(coherePrompt));
    var stream = new MemoryStream(bytes);

    var requestModel = new Amazon.BedrockRuntime.Model.InvokeModelRequest()
    {
        ModelId = "cohere.command-text-v14",
        ContentType = "application/json",
        Accept = "*/*",
        Body = stream
    };

    var response = await client.InvokeModelAsync(requestModel);

    var responseBody = await new StreamReader(response.Body).ReadToEndAsync();
    var data = JsonSerializer.Deserialize<CohereResponse>(responseBody);

    if (data?.Generations == null || data.Generations.Length == 0)
    {
        // e.g NotFound result / response wrapper
    }

    var genResult = data!.Generations![0].Text!.Trim();

    return new TextPromptReponse(genResult);
});

// Stable Diffusion Prompt
app.MapPost("/prompts/image", async (AmazonBedrockRuntimeClient client, ImagePromptRequest request) =>
{
    var sdPrompt = new StablePrompt(request.Prompt);
    var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(sdPrompt));
    var stream = new MemoryStream(bytes);

    var requestModel = new Amazon.BedrockRuntime.Model.InvokeModelRequest()
    {
        ModelId = "stability.stable-diffusion-xl-v1",
        ContentType = "application/json",
        Accept = "*/*",
        Body = stream
    };

    var response = await client.InvokeModelAsync(requestModel);

    string pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "genai-images");
    Directory.CreateDirectory(pathToSave);

    var data = JsonSerializer.Deserialize<StableResponse>(response.Body);

    foreach (var artifact in data!.Artifacts)
    {
        int suffix = 1;
        var imageBytes = Convert.FromBase64String(artifact.Base64);
        var fileName = $"{request.Prompt.Replace(' ', '-')}-{suffix}.webp";
        var filePath = Path.Combine(pathToSave, fileName);
        File.WriteAllBytes(filePath, imageBytes);
        suffix++;
    }

    return Results.Ok();
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
