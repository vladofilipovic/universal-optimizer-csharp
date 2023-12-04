/// <summary>
/// 
/// </summary>
namespace SingleObjective.Teaching.FunctionOneVariableProblem
{
    using UniversalOptimizer.Algorithm;

    using System.Collections.Generic;

    using System;

    using Serilog;
    using Serilog.Formatting.Json;
    using Serilog.Events;
    using CommandLine;
    using static SingleObjective.Teaching.FunctionOneVariableProblem.CommandLineHelper;
    using System.Runtime.CompilerServices;

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

                //        if (parameters["algorithm"] == "variable_neighborhood_search")
                //        {
                //            /// parameters for VNS process setup
                //            var kMin = parameters["kMin"];
                //            var kMax = parameters["kMax"];
                //            var localSearchType = parameters["localSearchType"];
                //            /// initial solution and vns support
                //            var solution_type = parameters["solutionType"];
                //            object vns_support = null;
                //            if (solution_type == "int")
                //            {
                //                var numberOfIntervals = parameters["solutionNumberOfIntervals"];
                //                var solution = FunctionOneVariableProblemBinaryIntSolution(domainFrom: problem.domainLow, domainTo: problem.domainHigh, numberOfIntervals: numberOfIntervals, randomSeed: rSeed);
                //                vns_support = FunctionOneVariableProblemBinaryIntSolutionVnsSupport();
                //            }
                //            else
                //            {
                //                throw new ValueError("Invalid solution/representation type is chosen.");
                //            }
                //            /// solver construction parameters
                //            var vnsConstructionParams = VnsOptimizerConstructionParameters();
                //            vnsConstructionParams.OutputControl = OutputControl;
                //            vnsConstructionParams.TargetProblem = problem;
                //            vnsConstructionParams.initialSolution = solution;
                //            vnsConstructionParams.problemSolutionVnsSupport = vns_support;
                //            vnsConstructionParams.finishControl = finishControl;
                //            vnsConstructionParams.randomSeed = rSeed;
                //            vnsConstructionParams.additionalStatisticsControl = additionalStatisticsControl;
                //            vnsConstructionParams.kMin = kMin;
                //            vnsConstructionParams.kMax = kMax;
                //            vnsConstructionParams.maxLocalOptima = maxLocalOptima;
                //            vnsConstructionParams.localSearchType = localSearchType;
                //            var solver = FunctionOneVariableProblemSolver.FromVariableNeighborhoodSearch(vnsConstructionParams);
                //        }
                //        else
                //        {
                //            throw new ValueError("Invalid optimization algorithm is chosen.");
                //        }
                //    }
                //solver.Opt.optimize();
                //Log.Debug("Method -{}- search finished.".format(parameters["algorithm"]));
                //logger.info("Best solution code: {}".format(solver.Opt.bestSolution.stringRepresentation()));
                //logger.info("Best solution objective: {}, fitness: {}".format(solver.Opt.bestSolution.objectiveValue, solver.Opt.bestSolution.fitnessValue));
                //logger.info("Number of iterations: {}, evaluations: {}".format(solver.Opt.iteration, solver.Opt.evaluation));
                //logger.info("Execution: {} - {}".format(solver.Opt.executionStarted, solver.Opt.executionEnded));
                //Log.Debug("Solver ended.");
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
                //var outputFile = open(outputFilePath, "w", encoding: "utf-8");
                //        }
                //        /// output control setup
                //        if (writeToOutputFile)
                //        {
                //            var output_fields = parameters["outputFields"];
                //            var output_moments = parameters["outputMoments"];
                //            OutputControl = OutputControl(writeToOutput: true, outputFile: outputFile, fields: output_fields, moments: output_moments);
                //        }
                //        else
                //        {
                //            OutputControl = OutputControl(writeToOutput: false);
                //        }
                //        /// input file setup
                //        var input_filePath = parameters["inputFilePath"];
                //        var input_format = parameters["inputFormat"];
                //        /// random seed setup
                //        if (Convert.ToInt32(parameters["randomSeed"]) > 0)
                //        {
                //            rSeed = Convert.ToInt32(parameters["randomSeed"]);
                //            logger.info(string.Format("RandomSeed is predefined. Predefined seed value:  %d", rSeed));
                //            if (writeToOutputFile)
                //            {
                //                outputFile.write(string.Format("# RandomSeed is predefined. Predefined seed value:  %d\n", rSeed));
                //            }
                //            random.seed(rSeed);
                //        }
                //        else
                //        {
                //            rSeed = randrange(sys.maxsize);
                //            logger.info(string.Format("RandomSeed is not predefined. Generated seed value:  %d", rSeed));
                //            if (writeToOutputFile)
                //            {
                //                outputFile.write(string.Format("# RandomSeed is not predefined. Generated seed value:  %d\n", rSeed));
                //            }
                //            seed(rSeed);
                //        }
                //        /// finishing criteria setup
                //        var finish_criteria = parameters["finishCriteria"];
                //        var max_numberEvaluations = parameters["finishEvaluationsMax"];
                //        var max_numberIterations = parameters["finishIterationsMax"];
                //        var max_time_for_execution_in_seconds = parameters["finishSecondsMax"];
                //        var finishControl = FinishControl(criteria: finish_criteria, evaluationsMax: max_numberEvaluations, iterationsMax: max_numberIterations, secondsMax: max_time_for_execution_in_seconds);
                //        /// solution evaluations and calculations cache setup
                //        var evaluationCacheIsUsed = parameters["solutionEvaluationCacheIsUsed"];
                //        var evaluationCacheMaxSize = parameters["solutionEvaluationCacheMaxSize"];
                //        var calculation_solutionDistanceCacheIsUsed = parameters["solutionDistanceCalculationCacheIsUsed"];
                //        var calculation_solutionDistanceCacheMaxSize = parameters["solutionDistanceCalculationCacheMaxSize"];
                //        /// additional statistic control setup
                //        var additionalStatistics_keep = parameters["additionalStatisticsKeep"];
                //        var maxLocalOptima = parameters["additionalStatisticsMaxLocalOptima"];
                //        var additionalStatisticsControl = AdditionalStatisticsControl(keep: additionalStatistics_keep, maxLocalOptima: maxLocalOptima);
                //        /// problem to be solved
                //        var problem = FunctionOneVariableProblem.from_input_file(input_filePath: input_filePath, input_format: input_format);
                //        var start_time = datetime.now();
                //        if (writeToOutputFile)
                //        {
                //            outputFile.write("# {} started at: {}\n".format(parameters["algorithm"], start_time.ToString()));
                //            outputFile.write("# Execution parameters: {}\n".format(parameters));
                //        }
                //        /// select among algorithm types
                Log.Debug("VNS ended.");
                return 0;
            }
        }
    }
}
