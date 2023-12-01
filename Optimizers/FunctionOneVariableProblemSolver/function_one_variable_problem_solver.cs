///  
/// The :mod:`opt.SingleObjective.Teaching.FunctionOneVariableProblem_solver` contains programming code that optimize :ref:`Max Function 1 Variable Problem` with various optimization techniques.
/// 
namespace UniversalOptimizer.Opt.SingleObjective.Teaching
{

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

    using ProblemSolutionTeSupport = uo.Algorithm.Exact.TotalEnumeration.problem_solution_te_support.ProblemSolutionTeSupport;

    using VnsOptimizerConstructionParameters = uo.Algorithm.Metaheuristic.VariableNeighborhoodSearch.vns_optimizer.VnsOptimizerConstructionParameters;

    using VnsOptimizer = uo.Algorithm.Metaheuristic.VariableNeighborhoodSearch.vns_optimizer.VnsOptimizer;

    using ProblemSolutionVnsSupport = uo.Algorithm.Metaheuristic.VariableNeighborhoodSearch.problem_solution_vns_support.ProblemSolutionVnsSupport;

    using FunctionOneVariableProblem = Teaching.FunctionOneVariableProblem.FunctionOneVariableProblem.FunctionOneVariableProblem;

    using FunctionOneVariableProblemBinaryIntSolution = Teaching.FunctionOneVariableProblem.FunctionOneVariableProblemBinaryIntSolution.FunctionOneVariableProblemBinaryIntSolution;

    using FunctionOneVariableProblemBinaryIntSolutionVnsSupport = Teaching.FunctionOneVariableProblem.FunctionOneVariableProblemBinaryIntSolution_vns_support.FunctionOneVariableProblemBinaryIntSolutionVnsSupport;

    public static class FunctionOneVariableProblem_solver
    {

        public static object directory = Path(_file__).resolve();

        static FunctionOneVariableProblem_solver()
        {
            sys.path.append(directory);
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent);
            sys.path.append(directory.parent.parent.parent);
        }

        /// 
        ///     Instance of the class :class:`FunctionOneVariableProblemSolverConstructionParameters` represents constructor parameters for max ones problem solver.
        ///     
        public class FunctionOneVariableProblemSolverConstructionParameters
        {

            public object finishControl;

            public object initial_solution;

            public object method;

            public object OutputControl;

            public object TargetProblem;

            public object vns_additionalStatisticsControl;

            public object vns_k_max;

            public object vns_k_min;

            public object vns_local_search_type;

            public object vns_problem_solution_support;

            public object vnsRandomSeed;

            public object method = null;

            public object finishControl = null;

            public object OutputControl = null;

            public object TargetProblem = null;

            public object initial_solution = null;

            public object vns_problem_solution_support = null;

            public object vnsRandomSeed = null;

            public object vns_additionalStatisticsControl = null;

            public object vns_k_min = null;

            public object vns_k_max = null;

            public object vns_local_search_type = null;
        }

        /// 
        ///     Instance of the class :class:`FunctionOneVariableProblemSolver` any of the developed solvers max ones problem.
        ///     
        public class FunctionOneVariableProblemSolver
        {

            private object _optimizer;

            public FunctionOneVariableProblemSolver(
                string method = null,
                object finishControl = null,
                object OutputControl = null,
                object TargetProblem = null,
                object initial_solution = null,
                object vns_problem_solution_support = null,
                int vnsRandomSeed = null,
                object vns_additionalStatisticsControl = null,
                int vns_k_min = null,
                int vns_k_max = null,
                string vns_local_search_type = null)
            {
                _optimizer = null;
                if (method == "variable_neighborhood_search")
                {
                    _optimizer = VnsOptimizer(finishControl: finishControl, OutputControl: OutputControl, TargetProblem: TargetProblem, initial_solution: initial_solution, problem_solution_vns_support: vns_problem_solution_support, randomSeed: vnsRandomSeed, additionalStatisticsControl: vns_additionalStatisticsControl, k_min: vns_k_min, k_max: vns_k_max, local_search_type: vns_local_search_type);
                }
                else
                {
                    throw new ValueError("Invalid optimization method {} - should be: '{}'.".format(method, "variable_neighborhood_search"));
                }
            }

            /// 
            /// Additional constructor. Create new `FunctionOneVariableProblemSolver` instance from construction parameters
            /// 
            /// :param `FunctionOneVariableProblemSolverConstructionParameters` construction_params: parameters for construction 
            /// 
            [classmethod]
            public static void from_construction_tuple(object cls, object construction_params = null)
            {
                return cls(method: construction_params.method, finishControl: construction_params.finishControl, OutputControl: construction_params.OutputControl, TargetProblem: construction_params.TargetProblem, initial_solution: construction_params.initial_solution, vns_problem_solution_support: construction_params.vns_problem_solution_support, vnsRandomSeed: construction_params.vnsRandomSeed, vns_additionalStatisticsControl: construction_params.vns_additionalStatisticsControl, vns_k_min: construction_params.vns_k_min, vns_k_max: construction_params.vns_k_max, vns_local_search_type: construction_params.vns_local_search_type);
            }

            /// 
            /// Additional constructor. Create new `OnesCountProblemSolver` instance when solving method is `Variable Neighborhood Search`
            /// 
            /// :param VnsOptimizerConstructionParameters vns_construction_params: construction parameters 
            /// 
            [classmethod]
            public static void from_variable_neighborhood_search(object cls, object vns_construction_params = null)
            {
                var @params = new FunctionOneVariableProblemSolverConstructionParameters();
                @params.method = "variable_neighborhood_search";
                @params.finishControl = vns_construction_params.finishControl;
                @params.OutputControl = vns_construction_params.OutputControl;
                @params.TargetProblem = vns_construction_params.TargetProblem;
                @params.initial_solution = vns_construction_params.initial_solution;
                @params.vns_problem_solution_support = vns_construction_params.problem_solution_vns_support;
                @params.vnsRandomSeed = vns_construction_params.randomSeed;
                @params.vns_additionalStatisticsControl = vns_construction_params.additionalStatisticsControl;
                @params.vns_k_min = vns_construction_params.k_min;
                @params.vns_k_max = vns_construction_params.k_max;
                @params.vns_local_search_type = vns_construction_params.local_search_type;
                return cls.from_construction_tuple(@params);
            }

            /// 
            /// Property getter for the optimizer used for solving
            /// 
            /// :return: optimizer
            /// return type `Optimizer`
            /// 
            public object opt
            {
                get
                {
                    return _optimizer;
                }
            }
        }
    }
}
