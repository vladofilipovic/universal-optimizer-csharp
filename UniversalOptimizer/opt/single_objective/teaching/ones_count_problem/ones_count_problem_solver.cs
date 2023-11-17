///  
/// The :mod:`opt.single_objective.teaching.ones_count_problem.solver` contains programming code that optimize :ref:`Max Ones Problem` with various optimization techniques.
/// 
namespace single_objective.teaching.ones_count_problem {
    
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
    
    using TargetProblem = uo.TargetProblem.TargetProblem.TargetProblem;
    
    using TargetSolution = uo.TargetSolution.TargetSolution.TargetSolution;
    
    using OutputControl = uo.Algorithm.OutputControl.OutputControl;
    
    using Optimizer = uo.Algorithm.optimizer.Optimizer;
    
    using FinishControl = uo.Algorithm.metaheuristic.finish_control.FinishControl;
    
    using AdditionalStatisticsControl = uo.Algorithm.metaheuristic.additional_statistics_control.AdditionalStatisticsControl;
    
    using TeOptimizerConstructionParameters = uo.Algorithm.exact.total_enumeration.te_optimizer.TeOptimizerConstructionParameters;
    
    using TeOptimizer = uo.Algorithm.exact.total_enumeration.te_optimizer.TeOptimizer;
    
    using ProblemSolutionTeSupport = uo.Algorithm.exact.total_enumeration.problem_solution_te_support.ProblemSolutionTeSupport;
    
    using VnsOptimizerConstructionParameters = uo.Algorithm.metaheuristic.variable_neighborhood_search.vns_optimizer.VnsOptimizerConstructionParameters;
    
    using VnsOptimizer = uo.Algorithm.metaheuristic.variable_neighborhood_search.vns_optimizer.VnsOptimizer;
    
    using ProblemSolutionVnsSupport = uo.Algorithm.metaheuristic.variable_neighborhood_search.problem_solution_vns_support.ProblemSolutionVnsSupport;
    
    using OnesCountProblemIntegerLinearProgrammingSolverConstructionParameters = opt.single_objective.teaching.ones_count_problem.ones_count_problem_ilp_linopy.OnesCountProblemIntegerLinearProgrammingSolverConstructionParameters;
    
    using OnesCountProblemIntegerLinearProgrammingSolver = opt.single_objective.teaching.ones_count_problem.ones_count_problem_ilp_linopy.OnesCountProblemIntegerLinearProgrammingSolver;
    
    using OnesCountProblem = opt.single_objective.teaching.ones_count_problem.ones_count_problem.OnesCountProblem;
    
    using OnesCountProblemBinaryIntSolution = opt.single_objective.teaching.ones_count_problem.ones_count_problem_binary_int_solution.OnesCountProblemBinaryIntSolution;
    
    using OnesCountProblemBinaryIntSolutionVnsSupport = opt.single_objective.teaching.ones_count_problem.ones_count_problem_binary_int_solution_vns_support.OnesCountProblemBinaryIntSolutionVnsSupport;
    
    using OnesCountProblemBinaryBitArraySolution = opt.single_objective.teaching.ones_count_problem.ones_count_problem_binary_bit_array_solution.OnesCountProblemBinaryBitArraySolution;
    
    using OnesCountProblemBinaryBitArraySolutionVnsSupport = opt.single_objective.teaching.ones_count_problem.ones_count_problem_binary_bit_array_solution_vns_support.OnesCountProblemBinaryBitArraySolutionVnsSupport;
    
    using OnesCountProblemBinaryBitArraySolutionTeSupport = opt.single_objective.teaching.ones_count_problem.ones_count_problem_binary_bit_array_solution_te_support.OnesCountProblemBinaryBitArraySolutionTeSupport;
    
    public static class ones_count_problem_solver {
        
        public static object directory = Path(_file__).resolve();
        
        static ones_count_problem_solver() {
            sys.path.append(directory);
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent);
            sys.path.append(directory.parent.parent.parent);
        }
        
        /// 
        ///     Instance of the class :class:`MaxOneProblemSolverConstructionParameters` represents constructor parameters for max ones problem solver.
        ///     
        public class MaxOneProblemSolverConstructionParameters {
            
            public object finish_control;
            
            public object initial_solution;
            
            public object method;
            
            public object OutputControl;
            
            public object TargetProblem;
            
            public object te_problem_solution_support;
            
            public object vns_additional_statistics_control;
            
            public object vns_k_max;
            
            public object vns_k_min;
            
            public object vns_local_search_type;
            
            public object vns_problem_solution_support;
            
            public object vnsRandomSeed;
            
            public object method = null;
            
            public object finish_control = null;
            
            public object OutputControl = null;
            
            public object TargetProblem = null;
            
            public object initial_solution = null;
            
            public object vns_problem_solution_support = null;
            
            public object vnsRandomSeed = null;
            
            public object vns_additional_statistics_control = null;
            
            public object vns_k_min = null;
            
            public object vns_k_max = null;
            
            public object vns_local_search_type = null;
            
            public object te_problem_solution_support = null;
        }
        
        /// 
        ///     Instance of the class :class:`MaxOneProblemSolver` any of the developed solvers max ones problem.
        ///     
        public class OnesCountProblemSolver {
            
            private object _optimizer;
            
            public OnesCountProblemSolver(
                string method = null,
                object finish_control = null,
                object OutputControl = null,
                object TargetProblem = null,
                object initial_solution = null,
                object vns_problem_solution_support = null,
                int vnsRandomSeed = null,
                object vns_additional_statistics_control = null,
                int vns_k_min = null,
                int vns_k_max = null,
                string vns_local_search_type = null,
                object te_problem_solution_support = null) {
                _optimizer = null;
                if (method == "variable_neighborhood_search") {
                    _optimizer = VnsOptimizer(finish_control: finish_control, OutputControl: OutputControl, TargetProblem: TargetProblem, initial_solution: initial_solution, problem_solution_vns_support: vns_problem_solution_support, randomSeed: vnsRandomSeed, additional_statistics_control: vns_additional_statistics_control, k_min: vns_k_min, k_max: vns_k_max, local_search_type: vns_local_search_type);
                } else if (method == "total_enumeration") {
                    _optimizer = TeOptimizer(OutputControl: OutputControl, TargetProblem: TargetProblem, initial_solution: initial_solution, problem_solution_te_support: te_problem_solution_support);
                } else if (method == "integer_linear_programming") {
                    _optimizer = OnesCountProblemIntegerLinearProgrammingSolver(OutputControl: OutputControl, problem: TargetProblem);
                } else {
                    throw new ValueError("Invalid optimization method {} - should be one of: '{}', '{}', '{}'.".format(method, "variable_neighborhood_search", "total_enumeration", "integer_linear_programming"));
                }
            }
            
            /// 
            /// Additional constructor. Create new `OnesCountProblemSolver` instance from construction parameters
            /// 
            /// :param `MaxOneProblemSolverConstructionParameters` construction_params: parameters for construction 
            /// 
            [classmethod]
            public static void from_construction_tuple(object cls, object construction_params = null) {
                return cls(method: construction_params.method, finish_control: construction_params.finish_control, OutputControl: construction_params.OutputControl, TargetProblem: construction_params.TargetProblem, initial_solution: construction_params.initial_solution, vns_problem_solution_support: construction_params.vns_problem_solution_support, vnsRandomSeed: construction_params.vnsRandomSeed, vns_additional_statistics_control: construction_params.vns_additional_statistics_control, vns_k_min: construction_params.vns_k_min, vns_k_max: construction_params.vns_k_max, vns_local_search_type: construction_params.vns_local_search_type, te_problem_solution_support: construction_params.te_problem_solution_support);
            }
            
            /// 
            /// Additional constructor. Create new `OnesCountProblemSolver` instance when solving method is `Variable Neighborhood Search`
            /// 
            /// :param VnsOptimizerConstructionParameters vns_construction_params: construction parameters 
            /// 
            [classmethod]
            public static void from_variable_neighborhood_search(object cls, object vns_construction_params = null) {
                var @params = new MaxOneProblemSolverConstructionParameters();
                @params.method = "variable_neighborhood_search";
                @params.finish_control = vns_construction_params.finish_control;
                @params.OutputControl = vns_construction_params.OutputControl;
                @params.TargetProblem = vns_construction_params.TargetProblem;
                @params.initial_solution = vns_construction_params.initial_solution;
                @params.vns_problem_solution_support = vns_construction_params.problem_solution_vns_support;
                @params.vnsRandomSeed = vns_construction_params.randomSeed;
                @params.vns_additional_statistics_control = vns_construction_params.additional_statistics_control;
                @params.vns_k_min = vns_construction_params.k_min;
                @params.vns_k_max = vns_construction_params.k_max;
                @params.vns_local_search_type = vns_construction_params.local_search_type;
                return cls.from_construction_tuple(@params);
            }
            
            /// 
            /// Additional constructor. Create new `OnesCountProblemSolver` instance when solving method is `Total Enumeration`
            /// 
            /// :param TeOptimizerConstructionParameters te_construction_params: construction parameters 
            /// 
            [classmethod]
            public static void from_total_enumeration(object cls, object te_construction_params = null) {
                var @params = new MaxOneProblemSolverConstructionParameters();
                @params.method = "total_enumeration";
                @params.OutputControl = te_construction_params.OutputControl;
                @params.TargetProblem = te_construction_params.TargetProblem;
                @params.initial_solution = te_construction_params.initial_solution;
                @params.te_problem_solution_support = te_construction_params.problem_solution_te_support;
                return cls.from_construction_tuple(@params);
            }
            
            /// 
            /// Additional constructor. Create new `OnesCountProblemSolver` instance when solving method is `Integer Linear Programming`
            /// 
            /// :param `OnesCountProblemIntegerLinearProgrammingSolverConstructionParameters` ilp_construction_params: construction parameters 
            /// 
            [classmethod]
            public static void from_integer_linear_programming(object cls, object ilp_construction_params = null) {
                var @params = new MaxOneProblemSolverConstructionParameters();
                @params.method = "integer_linear_programming";
                @params.OutputControl = ilp_construction_params.OutputControl;
                @params.TargetProblem = ilp_construction_params.TargetProblem;
                return cls.from_construction_tuple(@params);
            }
            
            /// 
            /// Property getter for the optimizer used for solving
            /// 
            /// :return: optimizer
            /// return type `Optimizer`
            /// 
            public object opt {
                get {
                    return _optimizer;
                }
            }
        }
    }
}
