using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog;

namespace UniversalOptimizer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                            // add console as logging target
                            .WriteTo.Console()
                            // add a logging target for warnings and higher severity  logs
                            // structured in JSON format
                            .WriteTo.File(new JsonFormatter(),
                                          "important.json",
                                          restrictedToMinimumLevel: LogEventLevel.Warning)
                            // add a rolling file for all logs
                            .WriteTo.File("all-.logs",
                                          rollingInterval: RollingInterval.Day)
                            // set default minimum level
                            .MinimumLevel.Debug()
                            .CreateLogger();
            // testing the developed ComplexCounterBitArrayFull class 
            //    var cc = new ComplexCounterBitArrayFull(6);
            //    var can_progress = cc.reset();
            //    foreach (var i in Enumerable.Range(1, 100 - 1)) {
            //        Console.WriteLine(cc.current_state().bin);
            //        can_progress = cc.progress();
            //    }
            //}

        }
    }
}