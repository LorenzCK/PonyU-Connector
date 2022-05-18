var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

async Task Process(HttpContext context)
{
    var request = context.Request;

    using var reader = new StreamReader(request.Body);
    var payload = await reader.ReadToEndAsync();
    
    Console.WriteLine("{0} request to {1}://{2}{3} from {4}:{5}",
        request.Method, request.Scheme, request.Host, request.Path,
        context.Connection.RemoteIpAddress, context.Connection.RemotePort);
    Console.WriteLine("Payload ({0} bytes): {1}", request.ContentLength ?? 0, payload);
}

app.MapGet("/", Process);
app.MapPost("/", Process);
app.MapPut("/", Process);

app.MapGet("/webhook/ponyu", Process);
app.MapPost("/webhook/ponyu", Process);
app.MapPut("/webhook/ponyu", Process);

app.Run();
