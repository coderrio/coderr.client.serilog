﻿using System;
using Serilog;
using Serilog.Events;

namespace Coderr.Client.Serilog.Demo
{
    class Program
    {
        static void Main()
        {
            var url = new Uri("http://localhost:60473/");
            Err.Configuration.Credentials(url,
                "1a68bc3e123c48a3887877561b0982e2",
                "bd73436e965c4f3bb0578f57c21fde69");


            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Coderr()
                .CreateLogger();

            Log.Logger.Write(LogEventLevel.Information, "Hello World!");
            Log.Logger.Write(LogEventLevel.Error, new Exception("My world"), "Hello World!");
        }
    }
}
