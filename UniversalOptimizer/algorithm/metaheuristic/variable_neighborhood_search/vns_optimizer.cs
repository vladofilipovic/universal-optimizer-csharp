
namespace UniversalOptimizer.Algorithm.Metaheuristic.VariableNeighborhoodSearch
{

    using UniversalOptimizer.Algorithm;

    using UniversalOptimizer.TargetProblem;

    using UniversalOptimizer.TargetSolution;

    using UniversalOptimizer.Algorithm.Metaheuristic.VariableNeighborhoodSearch;

    using System.Collections.Generic;

    using System;

    using System.Linq;

    using Serilog;


    /// <summary>
    /// Instance of this class represents constructor parameters for VNS algorithm.
    /// </summary>
    public class VnsOptimizerConstructionParameters<R_co, A_co>
    {
        public required AdditionalStatisticsControl AdditionalStatisticsControl { get; set; }
        public required FinishControl FinishControl { get; set; }
        public required TargetSolution<R_co, A_co> InitialSolution { get; set; }
        public int KMax { get; set; }
        public int KMin { get; set; }
        public required string LocalSearchType { get; set; }
        public required OutputControl OutputControl { get; set; }
        public required IProblemSolutionVnsSupport<R_co, A_co> ProblemSolutionVnsSupport { get; set; }
        public int RandomSeed { get; set; }
        public required TargetProblem TargetProblem { get; set; }
    }

    /// <summary>
    /// Instance of this class encapsulate Variable Neighborhood Search optimization algorithm.
    /// </summary>
    /// <seealso cref="UniversalOptimizer.Algorithm.Metaheuristic.SingleSolutionMetaheuristic" />
    public class VnsOptimizer<R_co, A_co> : SingleSolutionMetaheuristic<R_co, A_co>
    {
        private Dictionary<string, ProblemSolutionVnsSupportLocalSearchMethod<R_co, A_co>> _implementedLocalSearches;

        private ProblemSolutionVnsSupportLocalSearchMethod<R_co, A_co> _lsMethod;

        private ProblemSolutionVnsSupportShakingMethod<R_co, A_co> _shakingMethod;

        private int? _kCurrent;
        private int? _kMax;
        private int? _kMin;

        private string? _localSearchType;

        public TargetSolution<R_co, A_co> currentSolution;

        /// <summary>
        /// Initializes a new instance of the <see cref="VnsOptimizer{R_co, A_co}"/> class.
        /// </summary>
        /// <param name="finishControl">The finish control.</param>
        /// <param name="randomSeed">The random seed.</param>
        /// <param name="additionalStatisticsControl">The additional statistics control.</param>
        /// <param name="outputControl">The output control.</param>
        /// <param name="targetProblem">The target problem.</param>
        /// <param name="initialSolution">The initial solution.</param>
        /// <param name="problemSolutionVnsSupport">The problem solution VNS support.</param>
        /// <param name="kMin">The k minimum.</param>
        /// <param name="kMax">The k maximum.</param>
        /// <param name="localSearchType">Type of the local search.</param>
        /// <exception cref="Exception">String.Format("Value '{0}' for VNS localSearchType is not supported", _localSearchType)</exception>
        public VnsOptimizer(
            FinishControl finishControl,
            int? randomSeed,
            AdditionalStatisticsControl additionalStatisticsControl,
            OutputControl outputControl,
            TargetProblem targetProblem,
            TargetSolution<R_co, A_co> initialSolution,
            IProblemSolutionVnsSupport<R_co, A_co> problemSolutionVnsSupport,
            int? kMin,
            int? kMax,
            string? localSearchType)
            : base("VnsOptimizer", finishControl: finishControl, randomSeed: randomSeed, additionalStatisticsControl: additionalStatisticsControl, outputControl: outputControl, targetProblem: targetProblem, initialSolution: initialSolution)
        {
            _localSearchType = localSearchType;
            if (problemSolutionVnsSupport is not null)
            {
                _implementedLocalSearches = new Dictionary<string, ProblemSolutionVnsSupportLocalSearchMethod<R_co, A_co>>();
                _implementedLocalSearches.Add("LocalSearchBestImprovement", problemSolutionVnsSupport.LocalSearchBestImprovement);
                _implementedLocalSearches.Add("LocalSearchFirstImprovement", problemSolutionVnsSupport.LocalSearchFirstImprovement);
                if (!_implementedLocalSearches.ContainsKey(_localSearchType))
                {
                    throw new Exception(String.Format("Value '{0}' for VNS localSearchType is not supported", _localSearchType));
                }
                _lsMethod = _implementedLocalSearches[_localSearchType];
                _shakingMethod = problemSolutionVnsSupport.Shaking;
            }
            _kMin = kMin;
            _kMax = kMax;
        }

        /// <summary>
        /// Forms optimizer from the construction tuple.
        /// </summary>
        /// <param name="constructionTuple">The construction tuple.</param>
        /// <returns></returns>
        public static VnsOptimizer<R_co, A_co> FromConstructionTuple(VnsOptimizerConstructionParameters<R_co, A_co> constructionTuple)
        {
            return new(constructionTuple.FinishControl, constructionTuple.RandomSeed, constructionTuple.AdditionalStatisticsControl, constructionTuple.OutputControl, constructionTuple.TargetProblem, constructionTuple.InitialSolution, constructionTuple.ProblemSolutionVnsSupport, constructionTuple.KMin, constructionTuple.KMax, constructionTuple.LocalSearchType);
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object Clone()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the k minimum.
        /// </summary>
        /// <value>
        /// The k minimum.
        /// </value>
        public int KMin
        {
            get
            {
                return (int)_kMin;
            }
        }

        /// <summary>
        /// Gets the k maximum.
        /// </summary>
        /// <value>
        /// The k maximum.
        /// </value>
        public int KMax
        {
            get
            {
                return (int)_kMax;
            }
        }

        /// 
        /// Initialization of the VNS algorithm
        /// 
        public override void Init()
        {
            _kCurrent = KMin;
            CurrentSolution.InitRandom(TargetProblem);
            CurrentSolution.Evaluate(TargetProblem);
            CopyToBestSolution(CurrentSolution);
        }

        /// 
        /// One iteration within main loop of the VNS algorithm
        /// 
        public override void MainLoopIteration()
        {
            WriteOutputValuesIfNeeded("beforeStepInIteration", "shaking");
            var solutionReps = new List<R_co>();
            if (!_shakingMethod((int)_kCurrent, TargetProblem, currentSolution, this, solutionReps))
            {
                WriteOutputValuesIfNeeded("afterStepInIteration", "shaking");
                return;
            }
            WriteOutputValuesIfNeeded("afterStepInIteration", "shaking");
            Iteration += 1;
            while (_kCurrent <= KMax)
            {
                WriteOutputValuesIfNeeded("beforeStepInIteration", "ls");
                currentSolution = _lsMethod((int)_kCurrent, TargetProblem, currentSolution, this);
                WriteOutputValuesIfNeeded("afterStepInIteration", "ls");
                /// update auxiliary structure that keeps all solution codes
                AdditionalStatisticsControl.AddToAllSolutionCodesIfRequired(currentSolution.StringRepresentation());
                AdditionalStatisticsControl.AddToMoreLocalOptimaIfRequired(currentSolution.StringRepresentation(), currentSolution.FitnessValue, BestSolution.StringRepresentation());
                var new_is_better = IsFirstSolutionBetter(currentSolution, BestSolution);
                var make_move = new_is_better;
                if (new_is_better is null)
                {
                    if (currentSolution.StringRepresentation() == BestSolution.StringRepresentation())
                    {
                        make_move = false;
                    }
                    else
                    {
                        Log.Debug("VnsOptimizer::MainLoopIteration: Same solution quality, generating random true with probability 0.5");
                        make_move = (new Random()).NextDouble() < 0.5;
                    }
                }
                if ((bool)make_move)
                {
                    CopyToBestSolution(currentSolution);
                    _kCurrent = KMin;
                }
                else
                {
                    _kCurrent += 1;
                }
            }
        }

        /// <summary>
        /// String representation of the VNS Optimizer instance.
        /// </summary>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="indentation">The indentation.</param>
        /// <param name="indentationSymbol">The indentation symbol.</param>
        /// <param name="groupStart">The group start.</param>
        /// <param name="groupEnd">The group end.</param>
        /// <returns></returns>
        public new string StringRep(
            string delimiter,
            int indentation = 0,
            string indentationSymbol = "",
            string groupStart = "{",
            string groupEnd = "}")
        {
            var s = delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += groupStart;
            s = base.StringRep(delimiter, indentation, indentationSymbol, "", "");
            s += delimiter;
            s += "currentSolution=" + currentSolution.StringRep(delimiter, indentation + 1, indentationSymbol, groupStart, groupEnd) + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "kMin=" + KMin.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "kMax=" + KMax.ToString() + delimiter;
            s += delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "_localSearchType=" + _localSearchType.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += groupEnd;
            return s;
        }

    }
}

