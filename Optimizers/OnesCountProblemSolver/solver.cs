
namespace SingleObjective.Teaching.OnesCountProblem
{
    using UniversalOptimizer.TargetSolution;
    using UniversalOptimizer.Algorithm;
    using UniversalOptimizer.Algorithm.Metaheuristic;
    using UniversalOptimizer.Algorithm.Exact.TotalEnumeration;
    using UniversalOptimizer.Algorithm.Metaheuristic.VariableNeighborhoodSearch;

    using SingleObjective.Teaching.OnesCountProblem;
    using static SingleObjective.Teaching.OnesCountProblem.CommandLineHelper;


    using System;
    using System.Text;
    using Serilog;
    using Serilog.Formatting.Json;
    using Serilog.Events;
    using CommandLine;

    public static class Solver
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
            Log.Debug(string.Format("Execution parameters: {0}", opts));
            // set optimization type(minimization or maximization)
            return 0;

        }


        //            var parameters = default_parameters_cl;
        //                var read_parameters_cl = parse_arguments();
        //                foreach (var param_keyValue in read_parameters_cl._get_kwargs())
        //                {
        //                    var key = param_keyValue[0];
        //                    var val = param_keyValue[1];
        //                    Log.Debug("key:{} value:{}".format(key, val));
        //                    if (key is not null && val is not null)
        //                    {
        //                        parameters[key] = val;
        //                    }
        //                }
        //                Log.Debug("Execution parameters: " + parameters.ToString());
        //                /// set optimization type (minimization or maximization)
        //                if (parameters["optimization_type"] == "minimization")
        //                {
        //                    isMinimization = true;
        //                }
        //                else if (parameters["optimization_type"] == "maximization")
        //                {
        //                    isMinimization = false;
        //                }
        //                else
        //                {
        //                    throw new ValueError("Either minimization or maximization should be selected.");
        //                }
        //                /// write to output file setup
        //                if (parameters["writeToOutputFile"] is null)
        //                {
        //                    writeToOutputFile = false;
        //                }
        //                else
        //                {
        //                    writeToOutputFile = @bool(parameters["writeToOutputFile"]);
        //                }
        //                /// output file setup
        //                if (writeToOutputFile)
        //                {
        //                    if (parameters["outputFileNameAppendTimeStamp"] is null)
        //                    {
        //                        shouldAddTimestampToFileName = false;
        //                    }
        //                    else
        //                    {
        //                        shouldAddTimestampToFileName = @bool(parameters["outputFileNameAppendTimeStamp"]);
        //                    }
        //                    if (parameters["outputFilePath"] is not null && parameters["outputFilePath"] != "")
        //                    {
        //                        outputFilePathParts = parameters["outputFilePath"].split("/");
        //                    }
        //                    else
        //                    {
        //                        outputFilePathParts = new List<string> {
        //                            "outputs",
        //                            "out"
        //                        };
        //                    }
        //                    var outputFileNameExt = outputFilePathParts[^1];
        //                    var outputFileNameParts = outputFileNameExt.split(".");
        //                    if (outputFileNameParts.Count > 1)
        //                    {
        //                        outputFileExt = outputFileNameParts[^1];
        //                        outputFileNameParts.pop();
        //                        outputFileName = ".".join(outputFileNameParts);
        //                    }
        //                    else
        //                    {
        //                        outputFileExt = "txt";
        //                        outputFileName = outputFileNameParts[0];
        //                    }
        //                    var dt = datetime.now();
        //                    outputFilePathParts.pop();
        //                    var outputFileDir = "/".join(outputFilePathParts);
        //                    if (shouldAddTimestampToFileName)
        //                    {
        //                        outputFilePathParts.append(outputFileName + "-maxones-" + parameters["algorithm"] + "-" + parameters["solutionType"] + "-" + parameters["optimization_type"][0:3:] + "-" + dt.strftime("%Y-%m-%d-%H-%M-%S.%f") + "." + outputFileExt);
        //                    }
        //                    else
        //                    {
        //                        outputFilePathParts.append(outputFileName + "-maxones-" + parameters["algorithm"] + "-" + parameters["solutionType"] + "-" + parameters["optimization_type"][0:3:] + "." + outputFileExt);
        //                    }
        //                    var outputFilePath = "/".join(outputFilePathParts);
        //                    Log.Debug("Output file path: " + outputFilePath.ToString());
        //                    ensureDir(outputFileDir);
        //                    var outputFile = open(outputFilePath, "w", encoding: "utf-8");
        //                }
        //                /// output control setup
        //                if (writeToOutputFile)
        //                {
        //                    var output_fields = parameters["outputFields"];
        //                    var output_moments = parameters["outputMoments"];
        //                    OutputControl = OutputControl(writeToOutput: true, outputFile: outputFile, fields: output_fields, moments: output_moments);
        //                }
        //                else
        //                {
        //                    OutputControl = OutputControl(writeToOutput: false);
        //                }
        //                /// input file setup
        //                var input_filePath = parameters["inputFilePath"];
        //                var input_format = parameters["inputFormat"];
        //                /// random seed setup
        //                if (Convert.ToInt32(parameters["randomSeed"]) > 0)
        //                {
        //                    rSeed = Convert.ToInt32(parameters["randomSeed"]);
        //                    logger.info(String.Format("RandomSeed is predefined. Predefined seed value:  %d", rSeed));
        //                    if (writeToOutputFile)
        //                    {
        //                        outputFile.write(String.Format("# RandomSeed is predefined. Predefined seed value:  %d\n", rSeed));
        //                    }
        //                    random.seed(rSeed);
        //                }
        //                else
        //                {
        //                    rSeed = randrange(sys.maxsize);
        //                    logger.info(String.Format("RandomSeed is not predefined. Generated seed value:  %d", rSeed));
        //                    if (writeToOutputFile)
        //                    {
        //                        outputFile.write(String.Format("# RandomSeed is not predefined. Generated seed value:  %d\n", rSeed));
        //                    }
        //                    seed(rSeed);
        //                }
        //                /// finishing criteria setup
        //                var finish_criteria = parameters["finishCriteria"];
        //                var max_numberEvaluations = parameters["finishEvaluationsMax"];
        //                var max_numberIterations = parameters["finishIterationsMax"];
        //                var max_time_for_execution_in_seconds = parameters["finishSecondsMax"];
        //                var finishControl = FinishControl(criteria: finish_criteria, evaluationsMax: max_numberEvaluations, iterationsMax: max_numberIterations, secondsMax: max_time_for_execution_in_seconds);
        //                /// solution evaluations and calculations cache setup
        //                var evaluationCacheIsUsed = parameters["solutionEvaluationCacheIsUsed"];
        //                var evaluationCacheMaxSize = parameters["solutionEvaluationCacheMaxSize"];
        //                var calculation_solutionDistanceCacheIsUsed = parameters["solutionDistanceCalculationCacheIsUsed"];
        //                var calculation_solutionDistanceCacheMaxSize = parameters["solutionDistanceCalculationCacheMaxSize"];
        //                /// additional statistic control setup
        //                var additionalStatistics_keep = parameters["additionalStatisticsKeep"];
        //                var maxLocalOptima = parameters["additionalStatisticsMaxLocalOptima"];
        //                var additionalStatisticsControl = AdditionalStatisticsControl(keep: additionalStatistics_keep, maxLocalOptima: maxLocalOptima);
        //                /// problem to be solved
        //                var problem = OnesCountProblem.from_input_file(input_filePath: input_filePath, input_format: input_format);
        //                var start_time = datetime.now();
        //                if (writeToOutputFile)
        //                {
        //                    outputFile.write("# {} started at: {}\n".format(parameters["algorithm"], start_time.ToString()));
        //                    outputFile.write("# Execution parameters: {}\n".format(parameters));
        //                }
        //                /// select among algorithm types
        //                if (parameters["algorithm"] == "variable_neighborhood_search")
        //                {
        //                    /// parameters for VNS process setup
        //                    var kMin = parameters["kMin"];
        //                    var kMax = parameters["kMax"];
        //                    var localSearchType = parameters["localSearchType"];
        //                    /// initial solution and vns support
        //                    solution_type = parameters["solutionType"];
        //                    object vns_support = null;
        //                    if (solution_type == "BitArray")
        //                    {
        //                        solution = OnesCountProblemBinaryBitArraySolution(randomSeed: rSeed);
        //                        vns_support = OnesCountProblemBinaryBitArraySolutionVnsSupport();
        //                    }
        //                    else if (solution_type == "int")
        //                    {
        //                        solution = OnesCountProblemBinaryUIntSolution(rSeed);
        //                        vns_support = OnesCountProblemBinaryUIntSolutionVnsSupport();
        //                    }
        //                    else
        //                    {
        //                        throw new ValueError("Invalid solution/representation type is chosen.");
        //                    }
        //                    /// solver construction parameters
        //                    var vnsConstructionParams = VnsOptimizerConstructionParameters();
        //                    vnsConstructionParams.OutputControl = OutputControl;
        //                    vnsConstructionParams.TargetProblem = problem;
        //                    vnsConstructionParams.solutionTemplate = solution;
        //                    vnsConstructionParams.problemSolutionVnsSupport = vns_support;
        //                    vnsConstructionParams.finishControl = finishControl;
        //                    vnsConstructionParams.randomSeed = rSeed;
        //                    vnsConstructionParams.additionalStatisticsControl = additionalStatisticsControl;
        //                    vnsConstructionParams.kMin = kMin;
        //                    vnsConstructionParams.kMax = kMax;
        //                    vnsConstructionParams.maxLocalOptima = maxLocalOptima;
        //                    vnsConstructionParams.localSearchType = localSearchType;
        //                    var solver = OnesCountProblemSolver.FromVariableNeighborhoodSearch(vnsConstructionParams);
        //                }
        //                else if (parameters["algorithm"] == "total_enumeration")
        //                {
        //                    /// initial solution and te support
        //                    solution_type = parameters["solutionType"];
        //                    object te_support = null;
        //                    if (solution_type == "BitArray")
        //                    {
        //                        solution = OnesCountProblemBinaryBitArraySolution(rSeed);
        //                        solution.isCaching = evaluationCacheIsUsed;
        //                        te_support = OnesCountProblemBinaryBitArraySolutionTeSupport();
        //                    }
        //                    else
        //                    {
        //                        throw new ValueError("Invalid solution/representation type is chosen.");
        //                    }
        //                    /// solver construction parameters
        //                    var te_constructionParams = TeOptimizerConstructionParameters();
        //                    te_constructionParams.OutputControl = OutputControl;
        //                    te_constructionParams.TargetProblem = problem;
        //                    te_constructionParams.solutionTemplate = solution;
        //                    te_constructionParams.problemSolutionTeSupport = te_support;
        //                    solver = OnesCountProblemSolver.from_total_enumeration(te_constructionParams);
        //                }
        //                else if (parameters["algorithm"] == "integer_linear_programming")
        //                {
        //                    /// solver construction parameters
        //                    var ilp_constructionParams = OnesCountProblemIntegerLinearProgrammingSolverConstructionParameters();
        //                    ilp_constructionParams.OutputControl = OutputControl;
        //                    ilp_constructionParams.TargetProblem = problem;
        //                    solver = OnesCountProblemSolver.from_integer_linear_programming(ilp_constructionParams);
        //                }
        //                else
        //                {
        //                    throw new ValueError("Invalid optimization algorithm is chosen.");
        //                }
        //                solver.Opt.optimize();
        //                Log.Debug("Method -{}- search finished.".format(parameters["algorithm"]));
        //                logger.info("Best solution code: {}".format(solver.Opt.bestSolution.stringRepresentation()));
        //                logger.info("Best solution objective: {}, fitness: {}".format(solver.Opt.bestSolution.objectiveValue, solver.Opt.bestSolution.fitnessValue));
        //                logger.info("Number of iterations: {}, evaluations: {}".format(solver.Opt.iteration, solver.Opt.evaluation));
        //                logger.info("Execution: {} - {}".format(solver.Opt.executionStarted, solver.Opt.executionEnded));
        //                Log.Debug("Solver ended.");
        //                return;
        //            }
        //            catch (Exception)
        //            {
        //                if (hasattr(exp, "message"))
        //                {
        //                    logger.exception(String.Format("Exception: %s\n", exp.message));
        //                }
        //                else
        //                {
        //                    logger.exception(String.Format("Exception: %s\n", exp.ToString()));
        //                }
        //            }
        //        }

        //        /// This means that if this script is executed, then 
        //        /// main() will be executed
        //        static solver()
        //        {
        //            if (_name__ == "__main__")
        //            {
        //            }

    }
}


