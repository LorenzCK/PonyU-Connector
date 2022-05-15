var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

async Task Process(HttpContext context)
{
    using var reader = new StreamReader(context.Request.Body);
    var payload = await reader.ReadToEndAsync();
    Console.WriteLine("Request: {0}", payload);
}

app.MapGet("/", Process);

app.MapGet("/webhook/ponyu", Process);

app.Run();
