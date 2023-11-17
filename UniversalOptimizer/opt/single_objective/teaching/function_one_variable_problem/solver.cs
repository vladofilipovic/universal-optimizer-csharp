///  
/// The :mod:`opt.single_objective.teaching.ones_count_problem.solver` contains programming code that optimize :ref:`Max Ones<Problem_Max_Ones>` Problem with various optimization techniques.
/// 
namespace single_objective.teaching.function_one_variable_problem {
    
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
    
    using default_parameters_cl = opt.single_objective.teaching.function_one_variable_problem.command_line.default_parameters_cl;
    
    using parse_arguments = opt.single_objective.teaching.function_one_variable_problem.command_line.parse_arguments;
    
    using FunctionOneVariableProblem = opt.single_objective.teaching.function_one_variable_problem.function_one_variable_problem.FunctionOneVariableProblem;
    
    using FunctionOneVariableProblemBinaryIntSolution = opt.single_objective.teaching.function_one_variable_problem.function_one_variable_problem_binary_int_solution.FunctionOneVariableProblemBinaryIntSolution;
    
    using FunctionOneVariableProblemBinaryIntSolutionVnsSupport = opt.single_objective.teaching.function_one_variable_problem.function_one_variable_problem_binary_int_solution_vns_support.FunctionOneVariableProblemBinaryIntSolutionVnsSupport;
    
    using FunctionOneVariableProblemSolver = opt.single_objective.teaching.function_one_variable_problem.function_one_variable_problem_solver.FunctionOneVariableProblemSolver;
    
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
            object rSeed;
            object OutputControl;
            object output_file_name;
            object output_file_ext;
            object output_file_path_parts;
            object should_add_timestamp_to_file_name;
            object write_to_output_file;
            object is_minimization;
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
                    is_minimization = true;
                } else if (parameters["optimization_type"] == "maximization") {
                    is_minimization = false;
                } else {
                    throw new ValueError("Either minimization or maximization should be selected.");
                }
                /// write to output file setup
                if (parameters["writeToOutputFile"] is null) {
                    write_to_output_file = false;
                } else {
                    write_to_output_file = @bool(parameters["writeToOutputFile"]);
                }
                /// output file setup
                if (write_to_output_file) {
                    if (parameters["outputFileNameAppendTimeStamp"] is null) {
                        should_add_timestamp_to_file_name = false;
                    } else {
                        should_add_timestamp_to_file_name = @bool(parameters["outputFileNameAppendTimeStamp"]);
                    }
                    if (parameters["outputFilePath"] is not null && parameters["outputFilePath"] != "") {
                        output_file_path_parts = parameters["outputFilePath"].split("/");
                    } else {
                        output_file_path_parts = new List<string> {
                            "outputs",
                            "out"
                        };
                    }
                    var output_file_name_ext = output_file_path_parts[^1];
                    var output_file_name_parts = output_file_name_ext.split(".");
                    if (output_file_name_parts.Count > 1) {
                        output_file_ext = output_file_name_parts[^1];
                        output_file_name_parts.pop();
                        output_file_name = ".".join(output_file_name_parts);
                    } else {
                        output_file_ext = "txt";
                        output_file_name = output_file_name_parts[0];
                    }
                    var dt = datetime.now();
                    output_file_path_parts.pop();
                    var output_file_dir = "/".join(output_file_path_parts);
                    if (should_add_timestamp_to_file_name) {
                        output_file_path_parts.append(output_file_name + "-maxones-" + parameters["algorithm"] + "-" + parameters["solutionType"] + "-" + parameters["optimization_type"][0:3:] + "-" + dt.strftime("%Y-%m-%d-%H-%M-%S.%f") + "." + output_file_ext);
                    } else {
                        output_file_path_parts.append(output_file_name + "-maxones-" + parameters["algorithm"] + "-" + parameters["solutionType"] + "-" + parameters["optimization_type"][0:3:] + "." + output_file_ext);
                    }
                    var output_file_path = "/".join(output_file_path_parts);
                    logger.debug("Output file path: " + output_file_path.ToString());
                    ensure_dir(output_file_dir);
                    var output_file = open(output_file_path, "w", encoding: "utf-8");
                }
                /// output control setup
                if (write_to_output_file) {
                    var output_fields = parameters["outputFields"];
                    var output_moments = parameters["outputMoments"];
                    OutputControl = OutputControl(write_to_output: true, output_file: output_file, fields: output_fields, moments: output_moments);
                } else {
                    OutputControl = OutputControl(write_to_output: false);
                }
                /// input file setup
                var input_file_path = parameters["inputFilePath"];
                var input_format = parameters["inputFormat"];
                /// random seed setup
                if (Convert.ToInt32(parameters["randomSeed"]) > 0) {
                    rSeed = Convert.ToInt32(parameters["randomSeed"]);
                    logger.info(String.Format("RandomSeed is predefined. Predefined seed value:  %d", rSeed));
                    if (write_to_output_file) {
                        output_file.write(String.Format("# RandomSeed is predefined. Predefined seed value:  %d\n", rSeed));
                    }
                    random.seed(rSeed);
                } else {
                    rSeed = randrange(sys.maxsize);
                    logger.info(String.Format("RandomSeed is not predefined. Generated seed value:  %d", rSeed));
                    if (write_to_output_file) {
                        output_file.write(String.Format("# RandomSeed is not predefined. Generated seed value:  %d\n", rSeed));
                    }
                    seed(rSeed);
                }
                /// finishing criteria setup
                var finish_criteria = parameters["finishCriteria"];
                var max_number_evaluations = parameters["finishEvaluationsMax"];
                var max_number_iterations = parameters["finishIterationsMax"];
                var max_time_for_execution_in_seconds = parameters["finishSecondsMax"];
                var finish_control = FinishControl(criteria: finish_criteria, evaluations_max: max_number_evaluations, iterations_max: max_number_iterations, seconds_max: max_time_for_execution_in_seconds);
                /// solution evaluations and calculations cache setup
                var evaluationCacheIsUsed = parameters["solutionEvaluationCacheIsUsed"];
                var evaluationCacheMaxSize = parameters["solutionEvaluationCacheMaxSize"];
                var calculation_solutionDistanceCacheIsUsed = parameters["solutionDistanceCalculationCacheIsUsed"];
                var calculation_solutionDistanceCacheMaxSize = parameters["solutionDistanceCalculationCacheMaxSize"];
                /// additional statistic control setup
                var additional_statistics_keep = parameters["additionalStatisticsKeep"];
                var max_local_optima = parameters["additionalStatisticsMaxLocalOptima"];
                var additional_statistics_control = AdditionalStatisticsControl(keep: additional_statistics_keep, max_local_optima: max_local_optima);
                /// problem to be solved
                var problem = FunctionOneVariableProblem.from_input_file(input_file_path: input_file_path, input_format: input_format);
                var start_time = datetime.now();
                if (write_to_output_file) {
                    output_file.write("# {} started at: {}\n".format(parameters["algorithm"], start_time.ToString()));
                    output_file.write("# Execution parameters: {}\n".format(parameters));
                }
                /// select among algorithm types
                if (parameters["algorithm"] == "variable_neighborhood_search") {
                    /// parameters for VNS process setup
                    var k_min = parameters["kMin"];
                    var k_max = parameters["kMax"];
                    var local_search_type = parameters["localSearchType"];
                    /// initial solution and vns support
                    var solution_type = parameters["solutionType"];
                    object vns_support = null;
                    if (solution_type == "int") {
                        var number_of_intervals = parameters["solutionNumberOfIntervals"];
                        var solution = FunctionOneVariableProblemBinaryIntSolution(domain_from: problem.domain_low, domain_to: problem.domain_high, number_of_intervals: number_of_intervals, randomSeed: rSeed);
                        vns_support = FunctionOneVariableProblemBinaryIntSolutionVnsSupport();
                    } else {
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
                    vns_construction_params.max_local_optima = max_local_optima;
                    vns_construction_params.local_search_type = local_search_type;
                    var solver = FunctionOneVariableProblemSolver.from_variable_neighborhood_search(vns_construction_params);
                } else {
                    throw new ValueError("Invalid optimization algorithm is chosen.");
                }
                solver.opt.optimize();
                logger.debug("Method -{}- search finished.".format(parameters["algorithm"]));
                logger.info("Best solution code: {}".format(solver.opt.best_solution.stringRepresentation()));
                logger.info("Best solution objective: {}, fitness: {}".format(solver.opt.best_solution.objectiveValue, solver.opt.best_solution.fitnessValue));
                logger.info("Number of iterations: {}, evaluations: {}".format(solver.opt.iteration, solver.opt.evaluation));
                logger.info("Execution: {} - {}".format(solver.opt.execution_started, solver.opt.execution_ended));
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
