using System;
using Serilog;
using Serilog.Events;

namespace Coderr.Client.Serilog.Demo
{
    class Program
    {
        static void Main()
        {
            var url = new Uri("https://localhost:44393/");
            Err.Configuration.Credentials(url,
                "5a617e0773b94284bef33940e4bc8384",
                "3fab63fb846c4dd289f67b0b3340fefc");


            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Coderr()
                .CreateLogger();

            Log.Logger.Write(LogEventLevel.Information, "Hello World!");
            try
            {
                throw new Exception("My world");
            }
            catch (Exception ex)
            {
                Log.Logger.Write(LogEventLevel.Error, ex, "Hello World!");
            }
            
        }
    }
}
