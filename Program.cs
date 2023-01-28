using Microsoft.Extensions.Hosting;


var app = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults();

app.ConfigureFunctionsWorkerDefaults
    (
        workerApplication =>
        {
            workerApplication.UseMiddleware<ExceptionLoggingMiddleware>();

            workerApplication.UseWhen<ExceptionLoggingMiddleware>((context) =>
            {
                return context.FunctionDefinition.InputBindings.Values
                            .First(a => a.Type.EndsWith("Trigger")).Type == "httpTrigger";
            });
        }
    );

var host = app.Build();

host.Run();