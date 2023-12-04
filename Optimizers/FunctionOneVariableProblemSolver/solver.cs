/// <summary>
/// 
/// </summary>
namespace SingleObjective.Teaching.FunctionOneVariableProblem
{
    using UniversalOptimizer.Algorithm;
    using UniversalOptimizer.Algorithm.Metaheuristic;
    using UniversalOptimizer.Algorithm.Metaheuristic.VariableNeighborhoodSearch;
    using static SingleObjective.Teaching.FunctionOneVariableProblem.CommandLineHelper;

    using System.Collections.Generic;
    using System;

    using Serilog;
    using Serilog.Formatting.Json;
    using Serilog.Events;
    using CommandLine;
    using System.Runtime.CompilerServices;
    using System.Text;
    using UniversalOptimizer.TargetSolution;
    using UniversalOptimizer.TargetProblem;

    public class Solver
    {

        ///  
        ///     This function executes solver.
        /// 
        ///     Which solver will be executed depends of command-line parameter algorithm.
        ///     
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

            try
            {
                Log.Debug("Solver started.");
                Parser.Default.ParseArguments<VariableNeighborhoodSearchOptions, IdleOptions>(args)
                    .MapResult(
                          (VariableNeighborhoodSearchOptions opts) => ExecuteVns(opts),
                          (IdleOptions opts) => ExecuteIdle(opts),
                          errs => 1);

                //return;
                // });
            }
            catch (Exception exp)
            {
                Log.Fatal(string.Format("Exception: %s\n", exp.Message));
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
            // set optimization type(minimization or maximization)
            bool isMinimization = true;
            if (opts.OptimizationType == "minimization")
            {
                isMinimization = true;
            }
            else if (opts.OptimizationType == "maximization")
            {
                isMinimization = false;
            }
            else
            {
                throw new Exception("Either minimization or maximization should be selected.");
            }
            /// output file setup
            StreamWriter outputFile = new StreamWriter("tmp.tmp");
            OutputControl outputControl = default;
            if (opts.WriteToOutputFile)
            {
                bool shouldAddTimestampToFileName = opts.OutputFileNameAppendTimeStamp;
                string[] outputFilePathParts;
                if (opts.OutputFilePath != "")
                {
                    outputFilePathParts = opts.OutputFilePath.Split("/");
                }
                else
                {
                    outputFilePathParts = new string[] {
                        "outputs",
                        "out"
                    };
                }
                string outputFileNameExt = outputFilePathParts[^1];
                string[] outputFileNameParts = outputFileNameExt.Split(".");
                string outputFileExt;
                string outputFileName;
                if (outputFileNameParts.Count() > 1)
                {
                    outputFileExt = outputFileNameParts[^1];
                    outputFileNameParts = outputFileNameParts.Take(outputFileNameParts.Count() - 1).ToArray();
                    outputFileName = String.Join('.', outputFileNameParts);
                }
                else
                {
                    outputFileExt = "txt";
                    outputFileName = outputFileNameParts[0];
                }
                var dt = DateTime.Now;
                outputFilePathParts = outputFilePathParts.Take(outputFilePathParts.Count() - 1).ToArray();
                var outputFileDir = String.Join('/', outputFilePathParts);
                if (shouldAddTimestampToFileName)
                {
                    outputFilePathParts.Append(outputFileName + "-fun1v-vns-" + opts.SolutionType + "-" + opts.OptimizationType.Substring(0, 3) + "-" + dt.ToString("%Y-%m-%d-%H-%M-%S.%f") + "." + outputFileExt);
                }
                else
                {
                    outputFilePathParts.Append(outputFileName + "-fun1v-vns-" + opts.SolutionType + "-" + opts.OptimizationType.Substring(0, 3) + "." + outputFileExt);
                }
                string outputFilePath = String.Join('/', outputFilePathParts);
                Log.Debug(String.Format("Output file path: {0}", outputFilePath));
                bool dirExists = System.IO.Directory.Exists(outputFileDir);
                if (!dirExists)
                    System.IO.Directory.CreateDirectory(outputFileDir);
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
                Log.Information(string.Format("RandomSeed is predefined. Predefined seed value: %d", rSeed));
                if (opts.WriteToOutputFile)
                {
                    outputFile.Write(string.Format("# RandomSeed is predefined. Predefined seed value:  %d\n", rSeed));
                }
            }
            else
            {
                rSeed = (new Random()).Next();
                Log.Information(string.Format("RandomSeed is not predefined. Generated seed value:  %d", rSeed));
                if (opts.WriteToOutputFile)
                {
                    outputFile.Write(string.Format("# RandomSeed is not predefined. Generated seed value:  %d\n", rSeed));
                }
            }
            Random randomGenerator = new Random(rSeed);
            /// finishing criteria setup
            var finishControl = new FinishControl(criteria: opts.FinishCriteria, evaluationsMax: opts.FinishEvaluationsMax, iterationsMax: opts.FinishIterationsMax, secondsMax: opts.FinishSecondsMax);
            /// solution evaluations and calculations cache setup
            /// additional statistic control setup
            var additionalStatisticsControl = new AdditionalStatisticsControl(keep: opts.AdditionalStatisticsKeep, maxLocalOptima: opts.AdditionalStatisticsMaxLocalOptima);
            /// problem to be solved
            var problem = new FunctionOneVariableProblem(isMinimization: isMinimization, inputFilePath: opts.InputFilePath, inputFormat: opts.InputFormat);
            var startTime = DateTime.Now;
            if (opts.WriteToOutputFile)
            {
                outputFile.Write(string.Format("# {} started at: {}\n", "vns", startTime));
                outputFile.Write(string.Format("# execution parameters: {}\n", opts));
            }
            if (opts.SolutionType == "int")
            {
                /// initial solution and vns support
                TargetSolution<int, double> solution = default;
                IProblemSolutionVnsSupport<int, double> vns_support = default;
                var numberOfIntervals = opts.SolutionNumberOfIntervals;
                solution = new FunctionOneVariableProblemBinaryIntSolution(domainFrom: problem.DomainLow, domainTo: problem.DomainHigh, numberOfIntervals: numberOfIntervals, randomSeed: rSeed);
                vns_support = new FunctionOneVariableProblemBinaryIntSolutionVnsSupport();
                /// solver construction parameters
                var vnsConstructionParams = new VnsOptimizerConstructionParameters<int, double>()
                {
                    OutputControl = outputControl,
                    TargetProblem = problem,
                    InitialSolution = solution,
                    ProblemSolutionVnsSupport = vns_support,
                    FinishControl = finishControl,
                    RandomSeed = rSeed,
                    AdditionalStatisticsControl = additionalStatisticsControl,
                    KMin = opts.KMin,
                    KMax = opts.KMax,
                    LocalSearchType = opts.LocalSearchType
                };
                var solver = new FunctionOneVariableProblemSolver("vns", finishControl: finishControl,
                    outputControl: outputControl,
                    targetProblem: problem,
                    initialSolution: solution,
                    vnsProblemSolutionSupport: vns_support,
                    vnsRandomSeed: rSeed,
                    vnsAdditionalStatisticsControl: additionalStatisticsControl,
                    vnsKMin: opts.KMin,
                    vnsKMax: opts.KMax,
                    vnsLocalSearchType: opts.LocalSearchType);
                solver.Opt.Optimize();
                Log.Debug("Method VNS finished.");
                Log.Information(string.Format( "Best solution code: {}", solver.Opt.BestSolution.StringRepresentation()));
                Log.Information(string.Format( "Best solution objective: {}, fitness: {}", solver.Opt.BestSolution.ObjectiveValue, solver.Opt.BestSolution.FitnessValue));
                Log.Information(string.Format( "Number of iterations: {}, evaluations: {}", solver.Opt.Iteration, solver.Opt.Evaluation));
                Log.Information("Execution: {} - {}", solver.Opt.ExecutionStarted, solver.Opt.ExecutionEnded);
                Log.Debug("Solver ended.");
            }
            else
            {
                throw new Exception("Invalid solution/representation type is chosen.");
            }
            Log.Debug("VNS ended.");
            return 0;
        }
    }
}

