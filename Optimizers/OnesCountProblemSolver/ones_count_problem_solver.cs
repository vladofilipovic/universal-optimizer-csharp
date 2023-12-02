///  
/// The :mod:`opt.SingleObjective.Teaching.ones_count_problem.solver` contains programming code that optimize :ref:`Max Ones Problem` with various optimization techniques.
/// 
namespace SingleObjective.Teaching.ones_count_problem {
    
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
    
    using FinishControl = uo.Algorithm.Metaheuristic.finishControl.FinishControl;
    
    using AdditionalStatisticsControl = uo.Algorithm.Metaheuristic.additionalStatisticsControl.AdditionalStatisticsControl;
    
    using TeOptimizerConstructionParameters = uo.Algorithm.Exact.TotalEnumeration.te_optimizer.TeOptimizerConstructionParameters;
    
    using TeOptimizer = uo.Algorithm.Exact.TotalEnumeration.te_optimizer.TeOptimizer;
    
    using ProblemSolutionTeSupport = uo.Algorithm.Exact.TotalEnumeration.problemSolutionTeSupport.ProblemSolutionTeSupport;
    
    using VnsOptimizerConstructionParameters = uo.Algorithm.Metaheuristic.VariableNeighborhoodSearch.vns_optimizer.VnsOptimizerConstructionParameters;
    
    using VnsOptimizer = uo.Algorithm.Metaheuristic.VariableNeighborhoodSearch.vns_optimizer.VnsOptimizer;
    
    using ProblemSolutionVnsSupport = uo.Algorithm.Metaheuristic.VariableNeighborhoodSearch.problemSolutionVnsSupport.ProblemSolutionVnsSupport;
    
    using OnesCountProblemIntegerLinearProgrammingSolverConstructionParameters = opt.SingleObjective.Teaching.ones_count_problem.ones_count_problem_ilp_linopy.OnesCountProblemIntegerLinearProgrammingSolverConstructionParameters;
    
    using OnesCountProblemIntegerLinearProgrammingSolver = opt.SingleObjective.Teaching.ones_count_problem.ones_count_problem_ilp_linopy.OnesCountProblemIntegerLinearProgrammingSolver;
    
    using OnesCountProblem = opt.SingleObjective.Teaching.ones_count_problem.ones_count_problem.OnesCountProblem;
    
    using OnesCountProblemBinaryIntSolution = opt.SingleObjective.Teaching.ones_count_problem.ones_count_problemBinaryIntSolution.OnesCountProblemBinaryIntSolution;
    
    using OnesCountProblemBinaryIntSolutionVnsSupport = opt.SingleObjective.Teaching.ones_count_problem.ones_count_problemBinaryIntSolutionVnsSupport.OnesCountProblemBinaryIntSolutionVnsSupport;
    
    using OnesCountProblemBinaryBitArraySolution = opt.SingleObjective.Teaching.ones_count_problem.ones_count_problem_binary_bit_array_solution.OnesCountProblemBinaryBitArraySolution;
    
    using OnesCountProblemBinaryBitArraySolutionVnsSupport = opt.SingleObjective.Teaching.ones_count_problem.ones_count_problem_binary_bit_array_solutionVnsSupport.OnesCountProblemBinaryBitArraySolutionVnsSupport;
    
    using OnesCountProblemBinaryBitArraySolutionTeSupport = opt.SingleObjective.Teaching.ones_count_problem.ones_count_problem_binary_bit_array_solutionTeSupport.OnesCountProblemBinaryBitArraySolutionTeSupport;
    
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
            
            public object finishControl;
            
            public object initialSolution;
            
            public object method;
            
            public object OutputControl;
            
            public object TargetProblem;
            
            public object te_problemSolution_support;
            
            public object vns_additionalStatisticsControl;
            
            public object vns_kMax;
            
            public object vns_kMin;
            
            public object vns_localSearchType;
            
            public object vns_problemSolution_support;
            
            public object vnsRandomSeed;
            
            public object method = null;
            
            public object finishControl = null;
            
            public object OutputControl = null;
            
            public object TargetProblem = null;
            
            public object initialSolution = null;
            
            public object vns_problemSolution_support = null;
            
            public object vnsRandomSeed = null;
            
            public object vns_additionalStatisticsControl = null;
            
            public object vns_kMin = null;
            
            public object vns_kMax = null;
            
            public object vns_localSearchType = null;
            
            public object te_problemSolution_support = null;
        }
        
        /// 
        ///     Instance of the class :class:`MaxOneProblemSolver` any of the developed solvers max ones problem.
        ///     
        public class OnesCountProblemSolver {
            
            private object _optimizer;
            
            public OnesCountProblemSolver(
                string method = null,
                object finishControl = null,
                object OutputControl = null,
                object TargetProblem = null,
                object initialSolution = null,
                object vns_problemSolution_support = null,
                int vnsRandomSeed = null,
                object vns_additionalStatisticsControl = null,
                int vns_kMin = null,
                int vns_kMax = null,
                string vns_localSearchType = null,
                object te_problemSolution_support = null) {
                _optimizer = null;
                if (method == "variable_neighborhood_search") {
                    _optimizer = VnsOptimizer(finishControl: finishControl, OutputControl: OutputControl, TargetProblem: TargetProblem, initialSolution: initialSolution, problemSolutionVnsSupport: vns_problemSolution_support, randomSeed: vnsRandomSeed, additionalStatisticsControl: vns_additionalStatisticsControl, kMin: vns_kMin, kMax: vns_kMax, localSearchType: vns_localSearchType);
                } else if (method == "total_enumeration") {
                    _optimizer = TeOptimizer(OutputControl: OutputControl, TargetProblem: TargetProblem, initialSolution: initialSolution, problemSolutionTeSupport: te_problemSolution_support);
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
            public static void FromConstructionTuple(object cls, object construction_params = null) {
                return cls(method: construction_params.method, finishControl: construction_params.finishControl, OutputControl: construction_params.OutputControl, TargetProblem: construction_params.TargetProblem, initialSolution: construction_params.initialSolution, vns_problemSolution_support: construction_params.vns_problemSolution_support, vnsRandomSeed: construction_params.vnsRandomSeed, vns_additionalStatisticsControl: construction_params.vns_additionalStatisticsControl, vns_kMin: construction_params.vns_kMin, vns_kMax: construction_params.vns_kMax, vns_localSearchType: construction_params.vns_localSearchType, te_problemSolution_support: construction_params.te_problemSolution_support);
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
                @params.finishControl = vns_construction_params.finishControl;
                @params.OutputControl = vns_construction_params.OutputControl;
                @params.TargetProblem = vns_construction_params.TargetProblem;
                @params.initialSolution = vns_construction_params.initialSolution;
                @params.vns_problemSolution_support = vns_construction_params.problemSolutionVnsSupport;
                @params.vnsRandomSeed = vns_construction_params.randomSeed;
                @params.vns_additionalStatisticsControl = vns_construction_params.additionalStatisticsControl;
                @params.vns_kMin = vns_construction_params.kMin;
                @params.vns_kMax = vns_construction_params.kMax;
                @params.vns_localSearchType = vns_construction_params.localSearchType;
                return cls.FromConstructionTuple(@params);
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
                @params.initialSolution = te_construction_params.initialSolution;
                @params.te_problemSolution_support = te_construction_params.problemSolutionTeSupport;
                return cls.FromConstructionTuple(@params);
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
                return cls.FromConstructionTuple(@params);
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
