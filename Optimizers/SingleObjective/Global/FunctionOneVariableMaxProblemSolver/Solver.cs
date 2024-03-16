/// <summary>
/// 
/// </summary>
namespace SingleObjective.Teaching.FunctionOneVariableProblem
{
    using UniversalOptimizer.Algorithm;
    using UniversalOptimizer.Algorithm.Metaheuristic;
    using UniversalOptimizer.Algorithm.Metaheuristic.VariableNeighborhoodSearch;
    using static SingleObjective.Teaching.FunctionOneVariableProblem.CommandLineHelper;

    using System;
    using Serilog;
    using Serilog.Formatting.Json;
    using Serilog.Events;
    using CommandLine;
    using System.Text;
    using UniversalOptimizer.Solution;

    public class Solver
    {
        ///  
        ///     This function executes solver.
        /// 
        ///     Which solver will be executed depends of command-line parameter algorithm.
        ///     
        static void Main(string[] args)
        {
            if (!Directory.Exists("logs"))
                Directory.CreateDirectory("logs");
            Log.Logger = new LoggerConfiguration()
                            // add console as logging target
                            .WriteTo.Console()
                            // add a logging target for warnings and higher severity  logs
                            // structured in JSON format
                            .WriteTo.File(new JsonFormatter(),
                                          "important.json",
                                          restrictedToMinimumLevel: LogEventLevel.Warning)
                            // add a rolling file for all logs
                            .WriteTo.File("logs/all-.log",
                                          rollingInterval: RollingInterval.Hour)
                            // set default minimum level
                            .MinimumLevel.Debug()
                            .CreateLogger();
            try
            {
                Log.Debug("Solver started.");
                _ = Parser.Default.ParseArguments<VariableNeighborhoodSearchOptions, IdleOptions>(args)
                    .MapResult(
                          (VariableNeighborhoodSearchOptions opts) => ExecuteVns(opts),
                          (IdleOptions opts) => ExecuteIdle(opts),
                          errs => 1);
            }
            catch (Exception exp)
            {
                Log.Fatal(string.Format("Exception: {0}\n", exp.Message));
            }
        }

        private static int ExecuteIdle(IdleOptions opts)
        {
            Log.Debug("Idle started.");
            Log.Debug("Idle ended.");
            return 0;
        }

        private static int ExecuteVns(VariableNeighborhoodSearchOptions opts)
        {
            Log.Debug("VNS started.");
            Log.Debug(String.Format("Execution parameters: {0}", opts));
            /// output file setup
            StreamWriter outputFile = new("tmp.tmp");
            OutputControl outputControl = new OutputControl();
            if (opts.WriteToOutputFile)
            {
                bool shouldAddTimestampToFileName = opts.OutputFileNameAppendTimeStamp;
                List<string> outputFilePathParts =[];
                if (opts.OutputFilePath != "")
                {
                    string[] temp = opts.OutputFilePath.Split("/");
                    foreach(var s in temp)
                        outputFilePathParts.Add(s);
                }
                else
                {
                    outputFilePathParts = ["outputs", "out"];
                }
                string[] outputFileNameParts = outputFilePathParts[^1].Split(".");
                string outputFileExt;
                StringBuilder outputFileNameSB;
                if (outputFileNameParts.Length > 1)
                {
                    outputFileExt = outputFileNameParts[^1];
                    outputFileNameSB = new StringBuilder("");
                    for(int i = 0; i < outputFileNameParts.Length-1; i++)
                        outputFileNameSB.Append( (((i==0)?"":".") + outputFileNameParts[i]) );
                }
                else
                {
                    outputFileExt = "txt";
                    outputFileNameSB = new StringBuilder( outputFileNameParts[0] );
                }
                string outputFileName = outputFileNameSB.ToString();
                var dt = DateTime.UtcNow;
                outputFilePathParts.RemoveAt(outputFilePathParts.Count - 1);
                var outputFileDir = String.Join("/", outputFilePathParts);
                if (shouldAddTimestampToFileName)
                {
                    outputFilePathParts.Add(outputFileName + "-funmax1v-vns-" + opts.SolutionType + "-" + dt.ToString("yyyy-MM-dd-HH-mm-ss.fff") + "." + outputFileExt);
                }
                else
                {
                    outputFilePathParts.Add(outputFileName + "-funmax1v-vns-" + opts.SolutionType + "." + outputFileExt);
                }
                string outputFilePath = String.Join('/', outputFilePathParts);
                Log.Debug(String.Format("Output file path: {0}", outputFilePath));
                bool dirExists = Directory.Exists(outputFileDir);
                if (!dirExists)
                    Directory.CreateDirectory(outputFileDir);
                outputFile = new StreamWriter(outputFilePath, append: true, Encoding.UTF8);
                // output control setup
                if (opts.WriteToOutputFile)
                {
                    outputControl = new OutputControl(writeToOutput: true, outputFile: outputFile, fields: opts.OutputFields, moments: opts.OutputMoments);
                }
                else
                {
                    outputControl = new OutputControl(writeToOutput: false);
                }
            }
            /// random seed setup
            int rSeed;
            if (opts.RandomSeed > 0)
            {
                rSeed = opts.RandomSeed;
                Log.Information(string.Format("RandomSeed is predefined. Predefined seed value: {0}", rSeed));
                if (opts.WriteToOutputFile)
                {
                    outputFile.Write(string.Format("# RandomSeed is predefined. Predefined seed value: {0}\n", rSeed));
                }
            }
            else
            {
                rSeed = (new Random()).Next();
                Log.Information(string.Format("RandomSeed is not predefined. Generated seed value: {0}", rSeed));
                if (opts.WriteToOutputFile)
                {
                    outputFile.Write(string.Format("# RandomSeed is not predefined. Generated seed value: {0}\n", rSeed));
                }
            }
            /// finishing criteria setup
            var finishControl = new FinishControl(criteria: opts.FinishCriteria, evaluationsMax: opts.FinishEvaluationsMax, iterationsMax: opts.FinishIterationsMax, secondsMax: opts.FinishSecondsMax);
            /// solution evaluations and calculations cache setup
            /// additional statistic control setup
            var additionalStatisticsControl = new AdditionalStatisticsControl(isActive: opts.AdditionalStatisticsIsActive, keep: opts.AdditionalStatisticsKeep, maxLocalOptimaCount: opts.AdditionalStatisticsMaxLocalOptimaCount);
            /// problem to be solved
            var problem = new FunctionOneVariableMaxProblem(inputFilePath: opts.InputFilePath, inputFormat: opts.InputFormat);
            var startTime = DateTime.UtcNow;
            if (opts.WriteToOutputFile)
            {
                outputFile.Write(string.Format("# {0} started at: {1}\n", "vns", startTime));
                outputFile.Write(string.Format("# execution parameters: {0}\n", opts));
            }
            if (opts.SolutionType == "uint")
            {
                /// initial solution and vns support
                var numberOfIntervals = opts.SolutionNumberOfIntervals;
                Solution<uint, double> solution = new FunctionOneVariableMaxProblemBinaryUintSolution(domainFrom: problem.DomainLow, domainTo: problem.DomainHigh, numberOfIntervals: numberOfIntervals, randomSeed: rSeed);
                IProblemSolutionVnsSupport<uint, double> vns_support = new FunctionOneVariableMaxProblemBinaryUintSolutionVnsSupport();
                /// solver construction 
                var solver = new VnsOptimizer<uint, double>(
                    finishControl: finishControl,
                    randomSeed: rSeed,
                    additionalStatisticsControl: additionalStatisticsControl,
                    outputControl: outputControl,   
                    problem: problem,
                    solutionTemplate: solution, 
                    problemSolutionVnsSupport: vns_support,
                    kMin: opts.KMin,
                    kMax: opts.KMax,
                    localSearchType: opts.LocalSearchType);
                if (solver is not null)
                {
                    var bs = solver.Optimize();
                    Log.Debug("Method VNS finished.");
                    Log.Information(string.Format("best solution representation:{0:b}, best solution argument:{1}, best solution objective:{2}, best solution fitness:{3}, iterations:{4}, evaluations:{5}, start time:{6}, stop time:{7}", bs!.Representation, bs!.StringRepresentation(), bs.ObjectiveValue, bs.FitnessValue, solver.Iteration, solver.Evaluation, solver.ExecutionStarted, solver.ExecutionEnded));
                    Log.Debug("Solver ended.");
                }
                else {
                    NullReferenceException nullReferenceException = new (nameof(solver));
                    throw nullReferenceException;
                }
            }
            else
            {
                throw new ArgumentException("Invalid solution/representation type is chosen.");
            }
            Log.Debug("VNS ended.");
            return 0;
        }
    }
}

