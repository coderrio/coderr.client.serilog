Serilog sink for Coderr
=======================

This library does two things:

1. All logged exceptions are reported to Coderr for further analysis
2. The 100 latest log entries are automatically added to all reported errors.

## Getting started

Configure Coderr with the application key and shared secret from your Coderr server.
Then activate the logger:

```csharp
class Program
{
    static void Main(string[] args)
    {
        var url = new Uri("https://yourCoderrServer");
        Err.Configuration.Credentials(url,
            "yourAppKey",
            "yourSharedSecret");

        // serilog config
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Coderr() // This line activates coderr
            .CreateLogger();

        Log.Logger.Write(LogEventLevel.Information, "Hello World!");

        // this exception will be reported to Coderr
        Log.Logger.Write(LogEventLevel.Error, new Exception("My world"), "Hello World!");
    }
}
```

https://coderr.io/documentation/getting-started