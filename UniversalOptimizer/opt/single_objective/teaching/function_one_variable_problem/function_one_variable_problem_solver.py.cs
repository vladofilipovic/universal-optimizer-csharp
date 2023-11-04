//  
// The :mod:`opt.single_objective.teaching.function_one_variable_problem_solver` contains programming code that optimize :ref:`Max Function 1 Variable Problem` with various optimization techniques.
// 
namespace single_objective.teaching.function_one_variable_problem {
    
    using sys;
    
    using Path = pathlib.Path;
    
    using dataclass = dataclasses.dataclass;
    
    using randrange = random.randrange;
    
    using seed = random.seed;
    
    using datetime = datetime.datetime;
    
    using BitArray = bitstring.BitArray;
    
    using xr = xarray;
    
    using Model = linopy.Model;
    
    using ensure_dir = uo.utils.files.ensure_dir;
    
    using logger = uo.utils.logger.logger;
    
    using TargetProblem = uo.target_problem.target_problem.TargetProblem;
    
    using TargetSolution = uo.target_solution.target_solution.TargetSolution;
    
    using OutputControl = uo.algorithm.output_control.OutputControl;
    
    using Optimizer = uo.algorithm.optimizer.Optimizer;
    
    using FinishControl = uo.algorithm.metaheuristic.finish_control.FinishControl;
    
    using AdditionalStatisticsControl = uo.algorithm.metaheuristic.additional_statistics_control.AdditionalStatisticsControl;
    
    using TeOptimizerConstructionParameters = uo.algorithm.exact.total_enumeration.te_optimizer.TeOptimizerConstructionParameters;
    
    using TeOptimizer = uo.algorithm.exact.total_enumeration.te_optimizer.TeOptimizer;
    
    using ProblemSolutionTeSupport = uo.algorithm.exact.total_enumeration.problem_solution_te_support.ProblemSolutionTeSupport;
    
    using VnsOptimizerConstructionParameters = uo.algorithm.metaheuristic.variable_neighborhood_search.vns_optimizer.VnsOptimizerConstructionParameters;
    
    using VnsOptimizer = uo.algorithm.metaheuristic.variable_neighborhood_search.vns_optimizer.VnsOptimizer;
    
    using ProblemSolutionVnsSupport = uo.algorithm.metaheuristic.variable_neighborhood_search.problem_solution_vns_support.ProblemSolutionVnsSupport;
    
    using FunctionOneVariableProblem = opt.single_objective.teaching.function_one_variable_problem.function_one_variable_problem.FunctionOneVariableProblem;
    
    using FunctionOneVariableProblemBinaryIntSolution = opt.single_objective.teaching.function_one_variable_problem.function_one_variable_problem_binary_int_solution.FunctionOneVariableProblemBinaryIntSolution;
    
    using FunctionOneVariableProblemBinaryIntSolutionVnsSupport = opt.single_objective.teaching.function_one_variable_problem.function_one_variable_problem_binary_int_solution_vns_support.FunctionOneVariableProblemBinaryIntSolutionVnsSupport;
    
    public static class function_one_variable_problem_solver {
        
        public static object directory = Path(@__file__).resolve();
        
        static function_one_variable_problem_solver() {
            sys.path.append(directory);
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent);
            sys.path.append(directory.parent.parent.parent);
        }
        
        // 
        //     Instance of the class :class:`FunctionOneVariableProblemSolverConstructionParameters` represents constructor parameters for max ones problem solver.
        //     
        public class FunctionOneVariableProblemSolverConstructionParameters {
            
            public object finish_control;
            
            public object initial_solution;
            
            public object method;
            
            public object output_control;
            
            public object target_problem;
            
            public object vns_additional_statistics_control;
            
            public object vns_k_max;
            
            public object vns_k_min;
            
            public object vns_local_search_type;
            
            public object vns_problem_solution_support;
            
            public object vns_random_seed;
            
            public object method = null;
            
            public object finish_control = null;
            
            public object output_control = null;
            
            public object target_problem = null;
            
            public object initial_solution = null;
            
            public object vns_problem_solution_support = null;
            
            public object vns_random_seed = null;
            
            public object vns_additional_statistics_control = null;
            
            public object vns_k_min = null;
            
            public object vns_k_max = null;
            
            public object vns_local_search_type = null;
        }
        
        // 
        //     Instance of the class :class:`FunctionOneVariableProblemSolver` any of the developed solvers max ones problem.
        //     
        public class FunctionOneVariableProblemSolver {
            
            public object @__optimizer;
            
            public FunctionOneVariableProblemSolver(
                string method = null,
                object finish_control = null,
                object output_control = null,
                object target_problem = null,
                object initial_solution = null,
                object vns_problem_solution_support = null,
                int vns_random_seed = null,
                object vns_additional_statistics_control = null,
                int vns_k_min = null,
                int vns_k_max = null,
                string vns_local_search_type = null) {
                this.@__optimizer = null;
                if (method == "variable_neighborhood_search") {
                    this.@__optimizer = VnsOptimizer(finish_control: finish_control, output_control: output_control, target_problem: target_problem, initial_solution: initial_solution, problem_solution_vns_support: vns_problem_solution_support, random_seed: vns_random_seed, additional_statistics_control: vns_additional_statistics_control, k_min: vns_k_min, k_max: vns_k_max, local_search_type: vns_local_search_type);
                } else {
                    throw new ValueError("Invalid optimization method {} - should be: '{}'.".format(method, "variable_neighborhood_search"));
                }
            }
            
            // 
            //         Additional constructor. Create new `FunctionOneVariableProblemSolver` instance from construction parameters
            // 
            //         :param `FunctionOneVariableProblemSolverConstructionParameters` construction_params: parameters for construction 
            //         
            [classmethod]
            public static void from_construction_tuple(object cls, object construction_params = null) {
                return cls(method: construction_params.method, finish_control: construction_params.finish_control, output_control: construction_params.output_control, target_problem: construction_params.target_problem, initial_solution: construction_params.initial_solution, vns_problem_solution_support: construction_params.vns_problem_solution_support, vns_random_seed: construction_params.vns_random_seed, vns_additional_statistics_control: construction_params.vns_additional_statistics_control, vns_k_min: construction_params.vns_k_min, vns_k_max: construction_params.vns_k_max, vns_local_search_type: construction_params.vns_local_search_type);
            }
            
            // 
            //         Additional constructor. Create new `OnesCountProblemSolver` instance when solving method is `Variable Neighborhood Search`
            // 
            //         :param VnsOptimizerConstructionParameters vns_construction_params: construction parameters 
            //         
            [classmethod]
            public static void from_variable_neighborhood_search(object cls, object vns_construction_params = null) {
                var @params = new FunctionOneVariableProblemSolverConstructionParameters();
                @params.method = "variable_neighborhood_search";
                @params.finish_control = vns_construction_params.finish_control;
                @params.output_control = vns_construction_params.output_control;
                @params.target_problem = vns_construction_params.target_problem;
                @params.initial_solution = vns_construction_params.initial_solution;
                @params.vns_problem_solution_support = vns_construction_params.problem_solution_vns_support;
                @params.vns_random_seed = vns_construction_params.random_seed;
                @params.vns_additional_statistics_control = vns_construction_params.additional_statistics_control;
                @params.vns_k_min = vns_construction_params.k_min;
                @params.vns_k_max = vns_construction_params.k_max;
                @params.vns_local_search_type = vns_construction_params.local_search_type;
                return cls.from_construction_tuple(@params);
            }
            
            // 
            //         Property getter for the optimizer used for solving
            // 
            //         :return: optimizer
            //         :rtype: `Optimizer`
            //         
            public object opt {
                get {
                    return this.@__optimizer;
                }
            }
        }
    }
}
