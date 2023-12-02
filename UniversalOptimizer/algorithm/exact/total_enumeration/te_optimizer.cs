

namespace UniversalOptimizer.Algorithm.Exact.TotalEnumeration
{

    using UniversalOptimizer.Algorithm;

    using UniversalOptimizer.TargetProblem;

    using UniversalOptimizer.TargetSolution;

    using System;

    using System.Linq;

    using Serilog;


    /// <summary>
    /// Instance of this  class represents constructor parameters for total enumeration algorithm.
    /// </summary>
    public class TeOptimizerConstructionParameters<R_co, A_co>
    {
        public required TargetSolution<R_co, A_co> InitialSolution { get; set; }
        public required OutputControl OutputControl { get; set; }
        public required TargetProblem TargetProblem { get; set; }
        public required IProblemSolutionTeSupport<R_co, A_co> ProblemSolutionTeSupport { get; set; }
    }

    /// 
    ///     This class represent total enumeration algorithm
    ///     
    public class TeOptimizer<R_co, A_co> : Algorithm<R_co, A_co>
    {

        private TargetSolution<R_co, A_co> _currentSolution;
        private ProblemSolutionTeSupportCanProgressMethod<R_co, A_co> _canProgressMethod;
        private ProblemSolutionTeSupportProgressMethod<R_co, A_co> _progressMethod;
        private ProblemSolutionTeSupportResetMethod<R_co, A_co> _resetMethod;
        private ProblemSolutionTeSupportOverallNumberOfEvaluationsMethod<R_co, A_co> _overallNumberOfEvaluationsMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeOptimizer{R_co, A_co}"/> class.
        /// </summary>
        /// <param name="outputControl">The output control.</param>
        /// <param name="targetProblem">The target problem.</param>
        /// <param name="initialSolution">The initial solution.</param>
        /// <param name="problemSolutionTeSupport">The problem solution TE support.</param>
        public TeOptimizer(OutputControl outputControl, TargetProblem targetProblem, TargetSolution<R_co, A_co> initialSolution, IProblemSolutionTeSupport<R_co, A_co> problemSolutionTeSupport)
            : base("TotalEnumeration", outputControl: outputControl, targetProblem: targetProblem)
        {
            /// total enumeration support
            if (problemSolutionTeSupport is not null)
            {
                _resetMethod = problemSolutionTeSupport.Reset;
                _progressMethod = problemSolutionTeSupport.Progress;
                _canProgressMethod = problemSolutionTeSupport.CanProgress;
                _overallNumberOfEvaluationsMethod = problemSolutionTeSupport.OverallNumberOfEvaluations;
            }
            /// current solution
            _currentSolution = initialSolution;
            CopyToBestSolution(initialSolution);
        }

        /// <summary>
        /// From the optimizer, based on construction tuple.
        /// </summary>
        /// <typeparam name="R_co">The type of the co.</typeparam>
        /// <typeparam name="A_co">The type of the co.</typeparam>
        /// <param name="constructionTuple">The construction tuple.</param>
        /// <returns></returns>
        public static TeOptimizer<R_co, A_co> FromConstructionTuple<R_co, A_co>(TeOptimizerConstructionParameters<R_co, A_co> constructionTuple)
        {
            return new TeOptimizer<R_co, A_co>(constructionTuple.OutputControl, constructionTuple.TargetProblem, constructionTuple.InitialSolution, constructionTuple.ProblemSolutionTeSupport);
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
        /// Gets or sets the current solution used during TE execution.
        /// </summary>
        /// <value>
        /// The current solution.
        /// </value>
        public TargetSolution<R_co, A_co> CurrentSolution
        {
            get
            {
                return _currentSolution;
            }
            set
            {
                _currentSolution = value;
            }
        }

        /// 
        /// Initialization of the total enumeration algorithm
        /// 
        public override void Init()
        {
            _resetMethod(TargetProblem, CurrentSolution, this);
            WriteOutputValuesIfNeeded("beforeEvaluation", "b_e");
            Evaluation += 1;
            CurrentSolution.Evaluate(TargetProblem);
            WriteOutputValuesIfNeeded("afterEvaluation", "a_e");
            CopyToBestSolution(CurrentSolution);
            Iteration = 1;
        }

        /// <summary>
        /// Method for optimization with Total Enumeration.
        /// </summary>
        /// <returns></returns>
        public override void Optimize()
        {
            ExecutionStarted = DateTime.Now;
            Init();
            Log.Debug("Overall number of evaluations: " + _overallNumberOfEvaluationsMethod(TargetProblem, CurrentSolution, this));
            WriteOutputHeadersIfNeeded();
            WriteOutputValuesIfNeeded("beforeAlgorithm", "b_a");
            while (true)
            {
                WriteOutputValuesIfNeeded("beforeIteration", "b_i");
                Iteration += 1;
                _progressMethod(this.TargetProblem, CurrentSolution, this);
                bool? new_is_better = IsFirstSolutionBetter(CurrentSolution, BestSolution);
                if ((bool)new_is_better)
                {
                    CopyToBestSolution(CurrentSolution);
                }
                WriteOutputValuesIfNeeded("afterIteration", "a_i");
                if (!_canProgressMethod(TargetProblem, CurrentSolution, this))
                {
                    break;
                }
            }
            ExecutionEnded = DateTime.Now;
            this.WriteOutputValuesIfNeeded("afterAlgorithm", "a_a");
        }

        /// <summary>
        /// String representation of the Total Enumeration optimizer instance.
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
            s += "currentSolution=" + CurrentSolution.StringRep(delimiter, indentation + 1, indentationSymbol, groupStart, groupEnd) + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += groupEnd;
            return s;
        }

    }
}

