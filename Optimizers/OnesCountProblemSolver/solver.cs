///  
/// The :mod:`opt.SingleObjective.Teaching.OnesCountProblem.solver` contains programming code that optimize :ref:`Max Ones<Problem_Max_Ones>` Problem with various optimization techniques.
/// 
namespace SingleObjective.Teaching.OnesCountProblem {
    
    using sys;
    
    using Path = pathlib.Path;
    
    using randrange = random.randrange;
    
    using seed = random.seed;
    
    using datetime = datetime.datetime;
    
    using BitArray = bitstring.BitArray;
    
    using xr = xarray;
    
    using Model = linopy.Model;
    
    using OutputControl = uo.Algorithm.OutputControl.OutputControl;
    
    using FinishControl = uo.Algorithm.Metaheuristic.finishControl.FinishControl;
    
    using AdditionalStatisticsControl = uo.Algorithm.Metaheuristic.additionalStatisticsControl.AdditionalStatisticsControl;
    
    using TeOptimizerConstructionParameters = uo.Algorithm.Exact.TotalEnumeration.te_optimizer.TeOptimizerConstructionParameters;
    
    using VnsOptimizerConstructionParameters = uo.Algorithm.Metaheuristic.VariableNeighborhoodSearch.vns_optimizer.VnsOptimizerConstructionParameters;
    
    using OnesCountProblemIntegerLinearProgrammingSolverConstructionParameters = opt.SingleObjective.Teaching.OnesCountProblem.ones_count_problem_ilp_linopy.OnesCountProblemIntegerLinearProgrammingSolverConstructionParameters;
    
    using ensure_dir = uo.utils.files.ensure_dir;
    
    using logger = uo.utils.logger.logger;
    
    using default_parameters_cl = opt.SingleObjective.Teaching.OnesCountProblem.command_line.default_parameters_cl;
    
    using parse_arguments = opt.SingleObjective.Teaching.OnesCountProblem.command_line.parse_arguments;
    
    using OnesCountProblem = opt.SingleObjective.Teaching.OnesCountProblem.ones_count_problem.OnesCountProblem;
    
    using OnesCountProblemBinaryIntSolution = opt.SingleObjective.Teaching.OnesCountProblem.ones_count_problemBinaryIntSolution.OnesCountProblemBinaryIntSolution;
    
    using OnesCountProblemBinaryIntSolutionVnsSupport = opt.SingleObjective.Teaching.OnesCountProblem.ones_count_problemBinaryIntSolutionVnsSupport.OnesCountProblemBinaryIntSolutionVnsSupport;
    
    using OnesCountProblemBinaryBitArraySolution = opt.SingleObjective.Teaching.OnesCountProblem.ones_count_problem_binary_bit_array_solution.OnesCountProblemBinaryBitArraySolution;
    
    using OnesCountProblemBinaryBitArraySolutionVnsSupport = opt.SingleObjective.Teaching.OnesCountProblem.ones_count_problem_binary_bit_array_solutionVnsSupport.OnesCountProblemBinaryBitArraySolutionVnsSupport;
    
    using OnesCountProblemBinaryBitArraySolutionTeSupport = opt.SingleObjective.Teaching.OnesCountProblem.ones_count_problem_binary_bit_array_solutionTeSupport.OnesCountProblemBinaryBitArraySolutionTeSupport;
    
    using OnesCountProblemSolver = opt.SingleObjective.Teaching.OnesCountProblem.ones_count_problem_solver.OnesCountProblemSolver;
    
    using System.Collections.Generic;
    
    using System;
    
    public static class solver {
        
        public static object directory = Path(_file__).resolve();
        
        static solver() {
            sys.path.append(directory);
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent);
            sys.path.append(directory.parent.parent.parent);
            @" 
Solver.

Which solver will be executed depends of command-line parameter algorithm.
";
            main();
        }
        
        ///  
        ///     This function executes solver.
        /// 
        ///     Which solver will be executed depends of command-line parameter algorithm.
        ///     
        public static object main() {
            object solution;
            object solution_type;
            object rSeed;
            object OutputControl;
            object outputFileName;
            object outputFileExt;
            object outputFilePathParts;
            object shouldAddTimestampToFileName;
            object writeToOutputFile;
            object isMinimization;
            try {
                logger.debug("Solver started.");
                var parameters = default_parameters_cl;
                var read_parameters_cl = parse_arguments();
                foreach (var param_keyValue in read_parameters_cl._get_kwargs()) {
                    var key = param_keyValue[0];
                    var val = param_keyValue[1];
                    logger.debug("key:{} value:{}".format(key, val));
                    if (key is not null && val is not null) {
                        parameters[key] = val;
                    }
                }
                logger.debug("Execution parameters: " + parameters.ToString());
                /// set optimization type (minimization or maximization)
                if (parameters["optimization_type"] == "minimization") {
                    isMinimization = true;
                } else if (parameters["optimization_type"] == "maximization") {
                    isMinimization = false;
                } else {
                    throw new ValueError("Either minimization or maximization should be selected.");
                }
                /// write to output file setup
                if (parameters["writeToOutputFile"] is null) {
                    writeToOutputFile = false;
                } else {
                    writeToOutputFile = @bool(parameters["writeToOutputFile"]);
                }
                /// output file setup
                if (writeToOutputFile) {
                    if (parameters["outputFileNameAppendTimeStamp"] is null) {
                        shouldAddTimestampToFileName = false;
                    } else {
                        shouldAddTimestampToFileName = @bool(parameters["outputFileNameAppendTimeStamp"]);
                    }
                    if (parameters["outputFilePath"] is not null && parameters["outputFilePath"] != "") {
                        outputFilePathParts = parameters["outputFilePath"].split("/");
                    } else {
                        outputFilePathParts = new List<string> {
                            "outputs",
                            "out"
                        };
                    }
                    var outputFileNameExt = outputFilePathParts[^1];
                    var outputFileName_parts = outputFileNameExt.split(".");
                    if (outputFileName_parts.Count > 1) {
                        outputFileExt = outputFileName_parts[^1];
                        outputFileName_parts.pop();
                        outputFileName = ".".join(outputFileName_parts);
                    } else {
                        outputFileExt = "txt";
                        outputFileName = outputFileName_parts[0];
                    }
                    var dt = datetime.now();
                    outputFilePathParts.pop();
                    var outputFile_dir = "/".join(outputFilePathParts);
                    if (shouldAddTimestampToFileName) {
                        outputFilePathParts.append(outputFileName + "-maxones-" + parameters["algorithm"] + "-" + parameters["solutionType"] + "-" + parameters["optimization_type"][0:3:] + "-" + dt.strftime("%Y-%m-%d-%H-%M-%S.%f") + "." + outputFileExt);
                    } else {
                        outputFilePathParts.append(outputFileName + "-maxones-" + parameters["algorithm"] + "-" + parameters["solutionType"] + "-" + parameters["optimization_type"][0:3:] + "." + outputFileExt);
                    }
                    var outputFilePath = "/".join(outputFilePathParts);
                    logger.debug("Output file path: " + outputFilePath.ToString());
                    ensure_dir(outputFile_dir);
                    var outputFile = open(outputFilePath, "w", encoding: "utf-8");
                }
                /// output control setup
                if (writeToOutputFile) {
                    var output_fields = parameters["outputFields"];
                    var output_moments = parameters["outputMoments"];
                    OutputControl = OutputControl(writeToOutput: true, outputFile: outputFile, fields: output_fields, moments: output_moments);
                } else {
                    OutputControl = OutputControl(writeToOutput: false);
                }
                /// input file setup
                var input_filePath = parameters["inputFilePath"];
                var input_format = parameters["inputFormat"];
                /// random seed setup
                if (Convert.ToInt32(parameters["randomSeed"]) > 0) {
                    rSeed = Convert.ToInt32(parameters["randomSeed"]);
                    logger.info(String.Format("RandomSeed is predefined. Predefined seed value:  %d", rSeed));
                    if (writeToOutputFile) {
                        outputFile.write(String.Format("# RandomSeed is predefined. Predefined seed value:  %d\n", rSeed));
                    }
                    random.seed(rSeed);
                } else {
                    rSeed = randrange(sys.maxsize);
                    logger.info(String.Format("RandomSeed is not predefined. Generated seed value:  %d", rSeed));
                    if (writeToOutputFile) {
                        outputFile.write(String.Format("# RandomSeed is not predefined. Generated seed value:  %d\n", rSeed));
                    }
                    seed(rSeed);
                }
                /// finishing criteria setup
                var finish_criteria = parameters["finishCriteria"];
                var max_numberEvaluations = parameters["finishEvaluationsMax"];
                var max_numberIterations = parameters["finishIterationsMax"];
                var max_time_for_execution_in_seconds = parameters["finishSecondsMax"];
                var finishControl = FinishControl(criteria: finish_criteria, evaluationsMax: max_numberEvaluations, iterationsMax: max_numberIterations, secondsMax: max_time_for_execution_in_seconds);
                /// solution evaluations and calculations cache setup
                var evaluationCacheIsUsed = parameters["solutionEvaluationCacheIsUsed"];
                var evaluationCacheMaxSize = parameters["solutionEvaluationCacheMaxSize"];
                var calculation_solutionDistanceCacheIsUsed = parameters["solutionDistanceCalculationCacheIsUsed"];
                var calculation_solutionDistanceCacheMaxSize = parameters["solutionDistanceCalculationCacheMaxSize"];
                /// additional statistic control setup
                var additionalStatistics_keep = parameters["additionalStatisticsKeep"];
                var maxLocalOptima = parameters["additionalStatisticsMaxLocalOptima"];
                var additionalStatisticsControl = AdditionalStatisticsControl(keep: additionalStatistics_keep, maxLocalOptima: maxLocalOptima);
                /// problem to be solved
                var problem = OnesCountProblem.from_input_file(input_filePath: input_filePath, input_format: input_format);
                var start_time = datetime.now();
                if (writeToOutputFile) {
                    outputFile.write("# {} started at: {}\n".format(parameters["algorithm"], start_time.ToString()));
                    outputFile.write("# Execution parameters: {}\n".format(parameters));
                }
                /// select among algorithm types
                if (parameters["algorithm"] == "variable_neighborhood_search") {
                    /// parameters for VNS process setup
                    var kMin = parameters["kMin"];
                    var kMax = parameters["kMax"];
                    var localSearchType = parameters["localSearchType"];
                    /// initial solution and vns support
                    solution_type = parameters["solutionType"];
                    object vns_support = null;
                    if (solution_type == "BitArray") {
                        solution = OnesCountProblemBinaryBitArraySolution(randomSeed: rSeed);
                        vns_support = OnesCountProblemBinaryBitArraySolutionVnsSupport();
                    } else if (solution_type == "int") {
                        solution = OnesCountProblemBinaryIntSolution(rSeed);
                        vns_support = OnesCountProblemBinaryIntSolutionVnsSupport();
                    } else {
                        throw new ValueError("Invalid solution/representation type is chosen.");
                    }
                    /// solver construction parameters
                    var vns_construction_params = VnsOptimizerConstructionParameters();
                    vns_construction_params.OutputControl = OutputControl;
                    vns_construction_params.TargetProblem = problem;
                    vns_construction_params.initialSolution = solution;
                    vns_construction_params.problemSolutionVnsSupport = vns_support;
                    vns_construction_params.finishControl = finishControl;
                    vns_construction_params.randomSeed = rSeed;
                    vns_construction_params.additionalStatisticsControl = additionalStatisticsControl;
                    vns_construction_params.kMin = kMin;
                    vns_construction_params.kMax = kMax;
                    vns_construction_params.maxLocalOptima = maxLocalOptima;
                    vns_construction_params.localSearchType = localSearchType;
                    var solver = OnesCountProblemSolver.from_variable_neighborhood_search(vns_construction_params);
                } else if (parameters["algorithm"] == "total_enumeration") {
                    /// initial solution and te support
                    solution_type = parameters["solutionType"];
                    object te_support = null;
                    if (solution_type == "BitArray") {
                        solution = OnesCountProblemBinaryBitArraySolution(rSeed);
                        solution.isCaching = evaluationCacheIsUsed;
                        te_support = OnesCountProblemBinaryBitArraySolutionTeSupport();
                    } else {
                        throw new ValueError("Invalid solution/representation type is chosen.");
                    }
                    /// solver construction parameters
                    var te_construction_params = TeOptimizerConstructionParameters();
                    te_construction_params.OutputControl = OutputControl;
                    te_construction_params.TargetProblem = problem;
                    te_construction_params.initialSolution = solution;
                    te_construction_params.problemSolutionTeSupport = te_support;
                    solver = OnesCountProblemSolver.from_total_enumeration(te_construction_params);
                } else if (parameters["algorithm"] == "integer_linear_programming") {
                    /// solver construction parameters
                    var ilp_construction_params = OnesCountProblemIntegerLinearProgrammingSolverConstructionParameters();
                    ilp_construction_params.OutputControl = OutputControl;
                    ilp_construction_params.TargetProblem = problem;
                    solver = OnesCountProblemSolver.from_integer_linear_programming(ilp_construction_params);
                } else {
                    throw new ValueError("Invalid optimization algorithm is chosen.");
                }
                solver.Opt.optimize();
                logger.debug("Method -{}- search finished.".format(parameters["algorithm"]));
                logger.info("Best solution code: {}".format(solver.Opt.bestSolution.stringRepresentation()));
                logger.info("Best solution objective: {}, fitness: {}".format(solver.Opt.bestSolution.objectiveValue, solver.Opt.bestSolution.fitnessValue));
                logger.info("Number of iterations: {}, evaluations: {}".format(solver.Opt.iteration, solver.Opt.evaluation));
                logger.info("Execution: {} - {}".format(solver.Opt.executionStarted, solver.Opt.executionEnded));
                logger.debug("Solver ended.");
                return;
            } catch (Exception) {
                if (hasattr(exp, "message")) {
                    logger.exception(String.Format("Exception: %s\n", exp.message));
                } else {
                    logger.exception(String.Format("Exception: %s\n", exp.ToString()));
                }
            }
        }
        
        /// This means that if this script is executed, then 
        /// main() will be executed
        static solver() {
            if (_name__ == "__main__") {
            }
        }
    }
}
