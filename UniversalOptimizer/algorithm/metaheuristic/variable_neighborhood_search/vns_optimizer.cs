
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
    /// Instance of this class encapsulate Variable Neighborhood Search optimization algorithm.
    /// </summary>
    /// <seealso cref="UniversalOptimizer.Algorithm.Metaheuristic.SingleSolutionMetaheuristic" />
    public class VnsOptimizer<R_co, A_co> : SingleSolutionMetaheuristic<R_co, A_co> 
    {
        private readonly IProblemSolutionVnsSupport<R_co, A_co> _problemSolutionVnsSupport;
        private readonly Dictionary<string, ProblemSolutionVnsSupportLocalSearchMethod<R_co, A_co>> _implementedLocalSearches;

        private readonly ProblemSolutionVnsSupportLocalSearchMethod<R_co, A_co> _lsMethod;

        private readonly ProblemSolutionVnsSupportShakingMethod<R_co, A_co> _shakingMethod;

        private int _kCurrent;
        private readonly int _kMax;
        private readonly int _kMin;

        private readonly string _localSearchType;

        public TargetSolution<R_co, A_co>? _currentSolution;

        /// <summary>
        /// Initializes a new instance of the <see cref="VnsOptimizer{R_co, A_co}"/> class.
        /// </summary>
        /// <param name="finishControl">The finish control.</param>
        /// <param name="randomSeed">The random seed.</param>
        /// <param name="additionalStatisticsControl">The additional statistics control.</param>
        /// <param name="outputControl">The output control.</param>
        /// <param name="targetProblem">The target problem.</param>
        /// <param name="solutionTemplate">The initial solution.</param>
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
            TargetSolution<R_co, A_co>? solutionTemplate,
            IProblemSolutionVnsSupport<R_co, A_co> problemSolutionVnsSupport,
            int kMin,
            int kMax,
            string localSearchType)
            : base("VnsOptimizer", finishControl: finishControl, randomSeed: randomSeed, additionalStatisticsControl: additionalStatisticsControl, outputControl: outputControl, targetProblem: targetProblem, solutionTemplate: solutionTemplate)
        {
            _localSearchType = localSearchType;
            _implementedLocalSearches = new Dictionary<string, ProblemSolutionVnsSupportLocalSearchMethod<R_co, A_co>>();
            if(problemSolutionVnsSupport is null)
                throw new NullReferenceException(nameof(problemSolutionVnsSupport));
            _problemSolutionVnsSupport = problemSolutionVnsSupport;
            _implementedLocalSearches.Add("localSearchBestImprovement", problemSolutionVnsSupport.LocalSearchBestImprovement);
            _implementedLocalSearches.Add("localSearchFirstImprovement", problemSolutionVnsSupport.LocalSearchFirstImprovement);
            if (!_implementedLocalSearches.TryGetValue(_localSearchType, out ProblemSolutionVnsSupportLocalSearchMethod<R_co, A_co>? value))
            {
                throw new ArgumentException(String.Format("Value '{0}' for VNS localSearchType is not supported", _localSearchType));
            }
            _lsMethod = value;
            _shakingMethod = problemSolutionVnsSupport.Shaking;
            _kMin = kMin;
            _kMax = kMax;
        }


        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public override object Clone()
        {
            return new VnsOptimizer<R_co, A_co>(this.FinishControl, this.RandomSeed, this.AdditionalStatisticsControl, this.OutputControl, this.TargetProblem, this.SolutionTemplate, this._problemSolutionVnsSupport, this.KMin, this.KMax, this._localSearchType);
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
            if (SolutionTemplate is null)
            {
                throw new ArgumentNullException(nameof(SolutionTemplate));
            }
            _kCurrent = KMin;
            CurrentSolution = (TargetSolution<R_co, A_co>)SolutionTemplate!.Clone();
            CurrentSolution.InitRandom(TargetProblem);
            CurrentSolution.Evaluate(TargetProblem);
            CopyToBestSolution(CurrentSolution);   
        }

        /// 
        /// One iteration within main loop of the VNS algorithm
        /// 
        public override void MainLoopIteration()
        {
            if (CurrentSolution is null)
            {
                throw new ArgumentNullException(nameof(CurrentSolution));
            }
            WriteOutputValuesIfNeeded("beforeStepInIteration", "shaking");
            if (!_shakingMethod((int)_kCurrent, TargetProblem, CurrentSolution!, this))
            {
                WriteOutputValuesIfNeeded("afterStepInIteration", "shaking");
                return;
            }
            WriteOutputValuesIfNeeded("afterStepInIteration", "shaking");
            Iteration += 1;
            while (_kCurrent <= KMax)
            {
                WriteOutputValuesIfNeeded("beforeStepInIteration", "ls");
                _lsMethod((int)_kCurrent, TargetProblem, CurrentSolution, this);
                WriteOutputValuesIfNeeded("afterStepInIteration", "ls");
                /// update auxiliary structure that keeps all solution codes
                AdditionalStatisticsControl.AddToAllSolutionCodesIfRequired(CurrentSolution.StringRepresentation());
                AdditionalStatisticsControl.AddToMoreLocalOptimaIfRequired(CurrentSolution.StringRepresentation(), CurrentSolution.FitnessValue, BestSolution!.StringRepresentation());
                var new_is_better = QualityOfSolution.IsFirstFitnessBetter(CurrentSolution.QualitySingle, BestSolution.QualitySingle, TargetProblem.IsMinimization);
                var make_move = new_is_better;
                if (new_is_better is null)
                {
                    if (CurrentSolution.StringRepresentation() == BestSolution.StringRepresentation())
                    {
                        make_move = false;
                    }
                    else
                    {
                        Log.Debug("VnsOptimizer::MainLoopIteration: Same solution quality, generating random true with probability 0.5");
                        make_move = (new Random()).NextDouble() < 0.5;
                    }
                }
                if (make_move == true)
                {
                    CopyToBestSolution(CurrentSolution);
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
            for(int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            s += groupStart;
            s = base.StringRep(delimiter, indentation, indentationSymbol, "", "");
            s += delimiter;
            s += "currentSolution=" + _currentSolution!.StringRep(delimiter, indentation + 1, indentationSymbol, groupStart, groupEnd) + delimiter;
            for(int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            s += "kMin=" + KMin.ToString() + delimiter;
            for(int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            s += "kMax=" + KMax.ToString() + delimiter;
            s += delimiter;
            for(int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            s += "_localSearchType=" + _localSearchType.ToString() + delimiter;
            for(int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            s += groupEnd;
            return s;
        }

    }
}

