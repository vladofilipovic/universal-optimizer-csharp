//  
// The :mod:`opt.single_objective.teaching.ones_count_problem.solver` contains programming code that optimize :ref:`Max Ones<Problem_Max_Ones>` Problem with various optimization techniques.
// 
namespace single_objective.teaching.ones_count_problem {
    
    using sys;
    
    using Path = pathlib.Path;
    
    using randrange = random.randrange;
    
    using seed = random.seed;
    
    using datetime = datetime.datetime;
    
    using BitArray = bitstring.BitArray;
    
    using xr = xarray;
    
    using Model = linopy.Model;
    
    using OutputControl = uo.algorithm.output_control.OutputControl;
    
    using FinishControl = uo.algorithm.metaheuristic.finish_control.FinishControl;
    
    using AdditionalStatisticsControl = uo.algorithm.metaheuristic.additional_statistics_control.AdditionalStatisticsControl;
    
    using TeOptimizerConstructionParameters = uo.algorithm.exact.total_enumeration.te_optimizer.TeOptimizerConstructionParameters;
    
    using VnsOptimizerConstructionParameters = uo.algorithm.metaheuristic.variable_neighborhood_search.vns_optimizer.VnsOptimizerConstructionParameters;
    
    using OnesCountProblemIntegerLinearProgrammingSolverConstructionParameters = opt.single_objective.teaching.ones_count_problem.ones_count_problem_ilp_linopy.OnesCountProblemIntegerLinearProgrammingSolverConstructionParameters;
    
    using ensure_dir = uo.utils.files.ensure_dir;
    
    using logger = uo.utils.logger.logger;
    
    using default_parameters_cl = opt.single_objective.teaching.ones_count_problem.command_line.default_parameters_cl;
    
    using parse_arguments = opt.single_objective.teaching.ones_count_problem.command_line.parse_arguments;
    
    using OnesCountProblem = opt.single_objective.teaching.ones_count_problem.ones_count_problem.OnesCountProblem;
    
    using OnesCountProblemBinaryIntSolution = opt.single_objective.teaching.ones_count_problem.ones_count_problem_binary_int_solution.OnesCountProblemBinaryIntSolution;
    
    using OnesCountProblemBinaryIntSolutionVnsSupport = opt.single_objective.teaching.ones_count_problem.ones_count_problem_binary_int_solution_vns_support.OnesCountProblemBinaryIntSolutionVnsSupport;
    
    using OnesCountProblemBinaryBitArraySolution = opt.single_objective.teaching.ones_count_problem.ones_count_problem_binary_bit_array_solution.OnesCountProblemBinaryBitArraySolution;
    
    using OnesCountProblemBinaryBitArraySolutionVnsSupport = opt.single_objective.teaching.ones_count_problem.ones_count_problem_binary_bit_array_solution_vns_support.OnesCountProblemBinaryBitArraySolutionVnsSupport;
    
    using OnesCountProblemBinaryBitArraySolutionTeSupport = opt.single_objective.teaching.ones_count_problem.ones_count_problem_binary_bit_array_solution_te_support.OnesCountProblemBinaryBitArraySolutionTeSupport;
    
    using OnesCountProblemSolver = opt.single_objective.teaching.ones_count_problem.ones_count_problem_solver.OnesCountProblemSolver;
    
    using System.Collections.Generic;
    
    using System;
    
    public static class solver {
        
        public static object directory = Path(@__file__).resolve();
        
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
        
        //  
        //     This function executes solver.
        // 
        //     Which solver will be executed depends of command-line parameter algorithm.
        //     
        public static object main() {
            object solution;
            object solution_type;
            object r_seed;
            object output_control;
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
                foreach (var param_key_value in read_parameters_cl._get_kwargs()) {
                    var key = param_key_value[0];
                    var val = param_key_value[1];
                    logger.debug("key:{} value:{}".format(key, val));
                    if (key is not null && val is not null) {
                        parameters[key] = val;
                    }
                }
                logger.debug("Execution parameters: " + parameters.ToString());
                // set optimization type (minimization or maximization)
                if (parameters["optimization_type"] == "minimization") {
                    is_minimization = true;
                } else if (parameters["optimization_type"] == "maximization") {
                    is_minimization = false;
                } else {
                    throw new ValueError("Either minimization or maximization should be selected.");
                }
                // write to output file setup
                if (parameters["writeToOutputFile"] is null) {
                    write_to_output_file = false;
                } else {
                    write_to_output_file = @bool(parameters["writeToOutputFile"]);
                }
                // output file setup
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
                // output control setup
                if (write_to_output_file) {
                    var output_fields = parameters["outputFields"];
                    var output_moments = parameters["outputMoments"];
                    output_control = OutputControl(write_to_output: true, output_file: output_file, fields: output_fields, moments: output_moments);
                } else {
                    output_control = OutputControl(write_to_output: false);
                }
                // input file setup
                var input_file_path = parameters["inputFilePath"];
                var input_format = parameters["inputFormat"];
                // random seed setup
                if (Convert.ToInt32(parameters["randomSeed"]) > 0) {
                    r_seed = Convert.ToInt32(parameters["randomSeed"]);
                    logger.info(String.Format("RandomSeed is predefined. Predefined seed value:  %d", r_seed));
                    if (write_to_output_file) {
                        output_file.write(String.Format("# RandomSeed is predefined. Predefined seed value:  %d\n", r_seed));
                    }
                    random.seed(r_seed);
                } else {
                    r_seed = randrange(sys.maxsize);
                    logger.info(String.Format("RandomSeed is not predefined. Generated seed value:  %d", r_seed));
                    if (write_to_output_file) {
                        output_file.write(String.Format("# RandomSeed is not predefined. Generated seed value:  %d\n", r_seed));
                    }
                    seed(r_seed);
                }
                // finishing criteria setup
                var finish_criteria = parameters["finishCriteria"];
                var max_number_evaluations = parameters["finishEvaluationsMax"];
                var max_number_iterations = parameters["finishIterationsMax"];
                var max_time_for_execution_in_seconds = parameters["finishSecondsMax"];
                var finish_control = FinishControl(criteria: finish_criteria, evaluations_max: max_number_evaluations, iterations_max: max_number_iterations, seconds_max: max_time_for_execution_in_seconds);
                // solution evaluations and calculations cache setup
                var evaluation_cache_is_used = parameters["solutionEvaluationCacheIsUsed"];
                var evaluation_cache_max_size = parameters["solutionEvaluationCacheMaxSize"];
                var calculation_solution_distance_cache_is_used = parameters["solutionDistanceCalculationCacheIsUsed"];
                var calculation_solution_distance_cache_max_size = parameters["solutionDistanceCalculationCacheMaxSize"];
                // additional statistic control setup
                var additional_statistics_keep = parameters["additionalStatisticsKeep"];
                var max_local_optima = parameters["additionalStatisticsMaxLocalOptima"];
                var additional_statistics_control = AdditionalStatisticsControl(keep: additional_statistics_keep, max_local_optima: max_local_optima);
                // problem to be solved
                var problem = OnesCountProblem.from_input_file(input_file_path: input_file_path, input_format: input_format);
                var start_time = datetime.now();
                if (write_to_output_file) {
                    output_file.write("# {} started at: {}\n".format(parameters["algorithm"], start_time.ToString()));
                    output_file.write("# Execution parameters: {}\n".format(parameters));
                }
                // select among algorithm types
                if (parameters["algorithm"] == "variable_neighborhood_search") {
                    // parameters for VNS process setup
                    var k_min = parameters["kMin"];
                    var k_max = parameters["kMax"];
                    var local_search_type = parameters["localSearchType"];
                    // initial solution and vns support
                    solution_type = parameters["solutionType"];
                    object vns_support = null;
                    if (solution_type == "BitArray") {
                        solution = OnesCountProblemBinaryBitArraySolution(random_seed: r_seed);
                        vns_support = OnesCountProblemBinaryBitArraySolutionVnsSupport();
                    } else if (solution_type == "int") {
                        solution = OnesCountProblemBinaryIntSolution(r_seed);
                        vns_support = OnesCountProblemBinaryIntSolutionVnsSupport();
                    } else {
                        throw new ValueError("Invalid solution/representation type is chosen.");
                    }
                    // solver construction parameters
                    var vns_construction_params = VnsOptimizerConstructionParameters();
                    vns_construction_params.output_control = output_control;
                    vns_construction_params.target_problem = problem;
                    vns_construction_params.initial_solution = solution;
                    vns_construction_params.problem_solution_vns_support = vns_support;
                    vns_construction_params.finish_control = finish_control;
                    vns_construction_params.random_seed = r_seed;
                    vns_construction_params.additional_statistics_control = additional_statistics_control;
                    vns_construction_params.k_min = k_min;
                    vns_construction_params.k_max = k_max;
                    vns_construction_params.max_local_optima = max_local_optima;
                    vns_construction_params.local_search_type = local_search_type;
                    var solver = OnesCountProblemSolver.from_variable_neighborhood_search(vns_construction_params);
                } else if (parameters["algorithm"] == "total_enumeration") {
                    // initial solution and te support
                    solution_type = parameters["solutionType"];
                    object te_support = null;
                    if (solution_type == "BitArray") {
                        solution = OnesCountProblemBinaryBitArraySolution(r_seed);
                        solution.is_caching = evaluation_cache_is_used;
                        te_support = OnesCountProblemBinaryBitArraySolutionTeSupport();
                    } else {
                        throw new ValueError("Invalid solution/representation type is chosen.");
                    }
                    // solver construction parameters
                    var te_construction_params = TeOptimizerConstructionParameters();
                    te_construction_params.output_control = output_control;
                    te_construction_params.target_problem = problem;
                    te_construction_params.initial_solution = solution;
                    te_construction_params.problem_solution_te_support = te_support;
                    solver = OnesCountProblemSolver.from_total_enumeration(te_construction_params);
                } else if (parameters["algorithm"] == "integer_linear_programming") {
                    // solver construction parameters
                    var ilp_construction_params = OnesCountProblemIntegerLinearProgrammingSolverConstructionParameters();
                    ilp_construction_params.output_control = output_control;
                    ilp_construction_params.target_problem = problem;
                    solver = OnesCountProblemSolver.from_integer_linear_programming(ilp_construction_params);
                } else {
                    throw new ValueError("Invalid optimization algorithm is chosen.");
                }
                solver.opt.optimize();
                logger.debug("Method -{}- search finished.".format(parameters["algorithm"]));
                logger.info("Best solution code: {}".format(solver.opt.best_solution.string_representation()));
                logger.info("Best solution objective: {}, fitness: {}".format(solver.opt.best_solution.objective_value, solver.opt.best_solution.fitness_value));
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
        
        // This means that if this script is executed, then 
        // main() will be executed
        static solver() {
            if (@__name__ == "__main__") {
            }
        }
    }
}
