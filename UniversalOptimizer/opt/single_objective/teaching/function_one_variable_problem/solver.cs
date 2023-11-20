///  
/// The :mod:`opt.single_objective.teaching.ones_count_problem.solver` contains programming code that optimize :ref:`Max Ones<Problem_Max_Ones>` Problem with various optimization techniques.
/// 
namespace UniversalOptimizer.opt.single_objective.teaching
{

    using sys;

    using Path = pathlib.Path;

    using randrange = random.randrange;

    using seed = random.seed;

    using datetime = datetime.datetime;

    using BitArray = bitstring.BitArray;

    using xr = xarray;

    using Model = linopy.Model;

    using OutputControl = uo.Algorithm.OutputControl.OutputControl;

    using FinishControl = uo.Algorithm.metaheuristic.finish_control.FinishControl;

    using AdditionalStatisticsControl = uo.Algorithm.metaheuristic.additional_statistics_control.AdditionalStatisticsControl;

    using TeOptimizerConstructionParameters = uo.Algorithm.exact.total_enumeration.te_optimizer.TeOptimizerConstructionParameters;

    using VnsOptimizerConstructionParameters = uo.Algorithm.metaheuristic.variable_neighborhood_search.vns_optimizer.VnsOptimizerConstructionParameters;

    using ensure_dir = uo.utils.files.ensure_dir;

    using logger = uo.utils.logger.logger;

    using default_parameters_cl = teaching.function_one_variable_problem.command_line.default_parameters_cl;

    using parse_arguments = teaching.function_one_variable_problem.command_line.parse_arguments;

    using FunctionOneVariableProblem = teaching.function_one_variable_problem.function_one_variable_problem.FunctionOneVariableProblem;

    using FunctionOneVariableProblemBinaryIntSolution = teaching.function_one_variable_problem.function_one_variable_problem_binary_int_solution.FunctionOneVariableProblemBinaryIntSolution;

    using FunctionOneVariableProblemBinaryIntSolutionVnsSupport = teaching.function_one_variable_problem.function_one_variable_problem_binary_int_solution_vns_support.FunctionOneVariableProblemBinaryIntSolutionVnsSupport;

    using FunctionOneVariableProblemSolver = teaching.function_one_variable_problem.function_one_variable_problem_solver.FunctionOneVariableProblemSolver;

    using System.Collections.Generic;

    using System;

    public static class solver
    {

        public static object directory = Path(_file__).resolve();

        static solver()
        {
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
        public static object main()
        {
            object rSeed;
            object OutputControl;
            object outputFile_name;
            object outputFile_ext;
            object outputFile_path_parts;
            object should_add_timestamp_to_file_name;
            object writeToOutputFile;
            object is_minimization;
            try
            {
                logger.debug("Solver started.");
                var parameters = default_parameters_cl;
                var read_parameters_cl = parse_arguments();
                foreach (var param_keyValue in read_parameters_cl._get_kwargs())
                {
                    var key = param_keyValue[0];
                    var val = param_keyValue[1];
                    logger.debug("key:{} value:{}".format(key, val));
                    if (key is not null && val is not null)
                    {
                        parameters[key] = val;
                    }
                }
                logger.debug("Execution parameters: " + parameters.ToString());
                /// set optimization type (minimization or maximization)
                if (parameters["optimization_type"] == "minimization")
                {
                    is_minimization = true;
                }
                else if (parameters["optimization_type"] == "maximization")
                {
                    is_minimization = false;
                }
                else
                {
                    throw new ValueError("Either minimization or maximization should be selected.");
                }
                /// write to output file setup
                if (parameters["writeToOutputFile"] is null)
                {
                    writeToOutputFile = false;
                }
                else
                {
                    writeToOutputFile = @bool(parameters["writeToOutputFile"]);
                }
                /// output file setup
                if (writeToOutputFile)
                {
                    if (parameters["outputFileNameAppendTimeStamp"] is null)
                    {
                        should_add_timestamp_to_file_name = false;
                    }
                    else
                    {
                        should_add_timestamp_to_file_name = @bool(parameters["outputFileNameAppendTimeStamp"]);
                    }
                    if (parameters["outputFilePath"] is not null && parameters["outputFilePath"] != "")
                    {
                        outputFile_path_parts = parameters["outputFilePath"].split("/");
                    }
                    else
                    {
                        outputFile_path_parts = new List<string> {
                            "outputs",
                            "out"
                        };
                    }
                    var outputFile_name_ext = outputFile_path_parts[^1];
                    var outputFile_name_parts = outputFile_name_ext.split(".");
                    if (outputFile_name_parts.Count > 1)
                    {
                        outputFile_ext = outputFile_name_parts[^1];
                        outputFile_name_parts.pop();
                        outputFile_name = ".".join(outputFile_name_parts);
                    }
                    else
                    {
                        outputFile_ext = "txt";
                        outputFile_name = outputFile_name_parts[0];
                    }
                    var dt = datetime.now();
                    outputFile_path_parts.pop();
                    var outputFile_dir = "/".join(outputFile_path_parts);
                    if (should_add_timestamp_to_file_name)
                    {
                        outputFile_path_parts.append(outputFile_name + "-maxones-" + parameters["algorithm"] + "-" + parameters["solutionType"] + "-" + parameters["optimization_type"][0:3:] + "-" + dt.strftime("%Y-%m-%d-%H-%M-%S.%f") + "." + outputFile_ext);
                    }
                    else
                    {
                        outputFile_path_parts.append(outputFile_name + "-maxones-" + parameters["algorithm"] + "-" + parameters["solutionType"] + "-" + parameters["optimization_type"][0:3:] + "." + outputFile_ext);
                    }
                    var outputFile_path = "/".join(outputFile_path_parts);
                    logger.debug("Output file path: " + outputFile_path.ToString());
                    ensure_dir(outputFile_dir);
                    var outputFile = open(outputFile_path, "w", encoding: "utf-8");
                }
                /// output control setup
                if (writeToOutputFile)
                {
                    var output_fields = parameters["outputFields"];
                    var output_moments = parameters["outputMoments"];
                    OutputControl = OutputControl(writeToOutput: true, outputFile: outputFile, fields: output_fields, moments: output_moments);
                }
                else
                {
                    OutputControl = OutputControl(writeToOutput: false);
                }
                /// input file setup
                var input_file_path = parameters["inputFilePath"];
                var input_format = parameters["inputFormat"];
                /// random seed setup
                if (Convert.ToInt32(parameters["randomSeed"]) > 0)
                {
                    rSeed = Convert.ToInt32(parameters["randomSeed"]);
                    logger.info(string.Format("RandomSeed is predefined. Predefined seed value:  %d", rSeed));
                    if (writeToOutputFile)
                    {
                        outputFile.write(string.Format("# RandomSeed is predefined. Predefined seed value:  %d\n", rSeed));
                    }
                    random.seed(rSeed);
                }
                else
                {
                    rSeed = randrange(sys.maxsize);
                    logger.info(string.Format("RandomSeed is not predefined. Generated seed value:  %d", rSeed));
                    if (writeToOutputFile)
                    {
                        outputFile.write(string.Format("# RandomSeed is not predefined. Generated seed value:  %d\n", rSeed));
                    }
                    seed(rSeed);
                }
                /// finishing criteria setup
                var finish_criteria = parameters["finishCriteria"];
                var max_numberEvaluations = parameters["finishEvaluationsMax"];
                var max_numberIterations = parameters["finishIterationsMax"];
                var max_time_for_execution_in_seconds = parameters["finishSecondsMax"];
                var finish_control = FinishControl(criteria: finish_criteria, evaluations_max: max_numberEvaluations, iterations_max: max_numberIterations, seconds_max: max_time_for_execution_in_seconds);
                /// solution evaluations and calculations cache setup
                var evaluationCacheIsUsed = parameters["solutionEvaluationCacheIsUsed"];
                var evaluationCacheMaxSize = parameters["solutionEvaluationCacheMaxSize"];
                var calculation_solutionDistanceCacheIsUsed = parameters["solutionDistanceCalculationCacheIsUsed"];
                var calculation_solutionDistanceCacheMaxSize = parameters["solutionDistanceCalculationCacheMaxSize"];
                /// additional statistic control setup
                var additional_statistics_keep = parameters["additionalStatisticsKeep"];
                var maxLocalOptima = parameters["additionalStatisticsMaxLocalOptima"];
                var additional_statistics_control = AdditionalStatisticsControl(keep: additional_statistics_keep, maxLocalOptima: maxLocalOptima);
                /// problem to be solved
                var problem = FunctionOneVariableProblem.from_input_file(input_file_path: input_file_path, input_format: input_format);
                var start_time = datetime.now();
                if (writeToOutputFile)
                {
                    outputFile.write("# {} started at: {}\n".format(parameters["algorithm"], start_time.ToString()));
                    outputFile.write("# Execution parameters: {}\n".format(parameters));
                }
                /// select among algorithm types
                if (parameters["algorithm"] == "variable_neighborhood_search")
                {
                    /// parameters for VNS process setup
                    var k_min = parameters["kMin"];
                    var k_max = parameters["kMax"];
                    var local_search_type = parameters["localSearchType"];
                    /// initial solution and vns support
                    var solution_type = parameters["solutionType"];
                    object vns_support = null;
                    if (solution_type == "int")
                    {
                        var number_of_intervals = parameters["solutionNumberOfIntervals"];
                        var solution = FunctionOneVariableProblemBinaryIntSolution(domain_from: problem.domain_low, domain_to: problem.domain_high, number_of_intervals: number_of_intervals, randomSeed: rSeed);
                        vns_support = FunctionOneVariableProblemBinaryIntSolutionVnsSupport();
                    }
                    else
                    {
                        throw new ValueError("Invalid solution/representation type is chosen.");
                    }
                    /// solver construction parameters
                    var vns_construction_params = VnsOptimizerConstructionParameters();
                    vns_construction_params.OutputControl = OutputControl;
                    vns_construction_params.TargetProblem = problem;
                    vns_construction_params.initial_solution = solution;
                    vns_construction_params.problem_solution_vns_support = vns_support;
                    vns_construction_params.finish_control = finish_control;
                    vns_construction_params.randomSeed = rSeed;
                    vns_construction_params.additional_statistics_control = additional_statistics_control;
                    vns_construction_params.k_min = k_min;
                    vns_construction_params.k_max = k_max;
                    vns_construction_params.maxLocalOptima = maxLocalOptima;
                    vns_construction_params.local_search_type = local_search_type;
                    var solver = FunctionOneVariableProblemSolver.from_variable_neighborhood_search(vns_construction_params);
                }
                else
                {
                    throw new ValueError("Invalid optimization algorithm is chosen.");
                }
                solver.opt.optimize();
                logger.debug("Method -{}- search finished.".format(parameters["algorithm"]));
                logger.info("Best solution code: {}".format(solver.opt.bestSolution.stringRepresentation()));
                logger.info("Best solution objective: {}, fitness: {}".format(solver.opt.bestSolution.objectiveValue, solver.opt.bestSolution.fitnessValue));
                logger.info("Number of iterations: {}, evaluations: {}".format(solver.opt.iteration, solver.opt.evaluation));
                logger.info("Execution: {} - {}".format(solver.opt.executionStarted, solver.opt.executionEnded));
                logger.debug("Solver ended.");
                return;
            }
            catch (Exception)
            {
                if (hasattr(exp, "message"))
                {
                    logger.exception(string.Format("Exception: %s\n", exp.message));
                }
                else
                {
                    logger.exception(string.Format("Exception: %s\n", exp.ToString()));
                }
            }
        }

        /// This means that if this script is executed, then 
        /// main() will be executed
        static solver()
        {
            if (_name__ == "__main__")
            {
            }
        }
    }
}
