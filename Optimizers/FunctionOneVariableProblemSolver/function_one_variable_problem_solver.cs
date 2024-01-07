///  
/// The :mod:`opt.SingleObjective.Teaching.FunctionOneVariableProblem_solver` contains programming code that optimize :ref:`Max Function 1 Variable Problem` with various optimization techniques.
/// 
namespace SingleObjective.Teaching.FunctionOneVariableProblem
{

    using UniversalOptimizer.TargetProblem;

    using UniversalOptimizer.TargetSolution;

    using UniversalOptimizer.Algorithm;

    using UniversalOptimizer.Algorithm.Metaheuristic;

    using UniversalOptimizer.Algorithm.Metaheuristic.VariableNeighborhoodSearch;
    using System.Runtime.CompilerServices;


    /// 
    ///     Instance of the class :class:`FunctionOneVariableProblemSolverConstructionParameters` represents constructor parameters for max ones problem solver.
    ///     
    public class FunctionOneVariableProblemSolverConstructionParameters<R_co, A_co>
    {

        public FinishControl FinishControl { get; set; }

        public TargetSolution<R_co, A_co> InitialSolution { get; set; }

        public string? Method { get; set; }

        public OutputControl OutputControl { get; set; }

        public TargetProblem TargetProblem { get; set; }

        public AdditionalStatisticsControl VnsAdditionalStatisticsControl { get; set; }

        public int? VnsKMax { get; set; }

        public int? VnsKMin { get; set; }

        public string? VnsLocalSearchType { get; set; }

        public IProblemSolutionVnsSupport<R_co, A_co> VnsProblemSolutionSupport { get; set; }

        public int? VnsRandomSeed { get; set; }

    }

    /// <summary>
    /// Instance of this class encapsulate any of the developed solvers Function One Variable Problem.
    /// </summary>
    public class FunctionOneVariableProblemSolver
    {

        private Metaheuristic<int,double> _optimizer;

        public FunctionOneVariableProblemSolver(
            string? method = null,
            FinishControl finishControl = null,
            OutputControl outputControl = null,
            TargetProblem targetProblem = null,
            TargetSolution<int, double> initialSolution = null,
            IProblemSolutionVnsSupport<int, double> vnsProblemSolutionSupport = null,
            int? vnsRandomSeed = null,
            AdditionalStatisticsControl vnsAdditionalStatisticsControl = null,
            int? vnsKMin = null,
            int? vnsKMax = null,
            string? vnsLocalSearchType = null)
        {
            _optimizer = null;
            if (method == "variable_neighborhood_search")
            {
                _optimizer = new VnsOptimizer<int,double>(finishControl: finishControl, outputControl: outputControl, targetProblem: targetProblem, initialSolution: initialSolution, problemSolutionVnsSupport: vnsProblemSolutionSupport, randomSeed: vnsRandomSeed, additionalStatisticsControl: vnsAdditionalStatisticsControl, kMin: vnsKMin, kMax: vnsKMax, localSearchType: vnsLocalSearchType);
            }
            else
            {
                throw new ArgumentException(String.Format("Invalid optimization method {0} - should be: '{1}'.", method, "variable_neighborhood_search"));
            }
        }

        /// <summary>
        /// Create new `FunctionOneVariableProblemSolver` instance from construction parameters tuple.
        /// </summary>
        /// <param name="constructionParams">The construction parameters.</param>
        /// <returns></returns>
        public static FunctionOneVariableProblemSolver FromConstructionTuple(FunctionOneVariableProblemSolverConstructionParameters<int, double> constructionParams) => new FunctionOneVariableProblemSolver(method: constructionParams.Method, finishControl: constructionParams.FinishControl, outputControl: constructionParams.OutputControl, targetProblem: constructionParams.TargetProblem, initialSolution: constructionParams.InitialSolution, vnsProblemSolutionSupport: constructionParams.VnsProblemSolutionSupport, vnsRandomSeed: constructionParams.VnsRandomSeed, vnsAdditionalStatisticsControl: constructionParams.VnsAdditionalStatisticsControl, vnsKMin: constructionParams.VnsKMin, vnsKMax: constructionParams.VnsKMax, vnsLocalSearchType: constructionParams.VnsLocalSearchType);

        /// 
        /// Property getter for the optimizer used for solving
        /// 
        /// :return: optimizer
        /// return type `Optimizer`
        /// 
        public Metaheuristic<int,double> Opt
        {
            get
            {
                return _optimizer;
            }
        }
    }

}

