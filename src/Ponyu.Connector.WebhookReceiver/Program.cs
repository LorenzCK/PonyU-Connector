using Ponyu.Connector;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

async Task ProcessRaw(HttpContext context)
{
    var request = context.Request;

    using var reader = new StreamReader(request.Body);
    var payload = await reader.ReadToEndAsync();
    
    Console.WriteLine("{0} request to {1}://{2}{3} from {4}:{5}",
        request.Method, request.Scheme, request.Host, request.Path,
        context.Connection.RemoteIpAddress, context.Connection.RemotePort);
    Console.WriteLine("Payload ({0} bytes): {1}", request.ContentLength ?? 0, payload);
}

app.MapGet("/webhook/ponyu", ProcessRaw);
app.MapPost("/webhook/ponyu", ProcessRaw);
app.MapPut("/webhook/ponyu", ProcessRaw);

async Task ProcessPayload(HttpContext context)
{
    var result = await Client.ProcessWebhook(context.Request);
    result?.Match(
        (scp) => Console.WriteLine("State change webhook: order {0} state changed to {1} ({2})", scp.OrderId, scp.Status, scp.Timestamp),
        (dp) => Console.WriteLine("Delay webhook: order {0} new pickup {1} new delivery {2} ({3})", dp.OrderId, dp.PickupDueDate, dp.RequestedDeliveryDate, dp.Timestamp)
    );
}

app.MapGet("/", ProcessPayload);
app.MapPost("/", ProcessPayload);
app.MapPut("/", ProcessPayload);

app.Run();
