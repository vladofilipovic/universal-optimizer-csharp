

namespace UniversalOptimizer.Algorithm.Exact.TotalEnumeration
{

    using UniversalOptimizer.Algorithm;
    using UniversalOptimizer.TargetProblem;
    using UniversalOptimizer.TargetSolution;

    using System;
    using System.Linq;
    using Serilog;
    using System.Reflection.Metadata.Ecma335;


    /// 
    ///     This class represent total enumeration algorithm
    ///     
    public class TeOptimizer<R_co, A_co> : Algorithm<R_co, A_co> 
    {

        private TargetSolution<R_co, A_co>? _currentSolution;
        private IProblemSolutionTeSupport<R_co, A_co> _problemSolutionTeSupport;
        private readonly ProblemSolutionTeSupportCanProgressMethod<R_co, A_co> _canProgressMethod;
        private readonly ProblemSolutionTeSupportProgressMethod<R_co, A_co> _progressMethod;
        private readonly ProblemSolutionTeSupportResetMethod<R_co, A_co> _resetMethod;
        private readonly ProblemSolutionTeSupportOverallNumberOfEvaluationsMethod<R_co, A_co> _overallNumberOfEvaluationsMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeOptimizer{R_co, A_co}"/> class.
        /// </summary>
        /// <param name="outputControl">The output control.</param>
        /// <param name="targetProblem">The target problem.</param>
        /// <param name="solutionTemplate">The template for the solution.</param>
        /// <param name="problemSolutionTeSupport">The problem solution TE support.</param>
        public TeOptimizer(OutputControl outputControl, TargetProblem targetProblem, TargetSolution<R_co, A_co>? solutionTemplate, IProblemSolutionTeSupport<R_co, A_co> problemSolutionTeSupport)
            : base("TotalEnumeration", outputControl: outputControl, targetProblem: targetProblem, solutionTemplate: solutionTemplate)
        {
            _problemSolutionTeSupport = problemSolutionTeSupport;
            /// total enumeration support
            _resetMethod = problemSolutionTeSupport.Reset;
            _progressMethod = problemSolutionTeSupport.Progress;
            _canProgressMethod = problemSolutionTeSupport.CanProgress;
            _overallNumberOfEvaluationsMethod = problemSolutionTeSupport.OverallNumberOfEvaluations;
            /// current and best solution
            _currentSolution = null;
            CopyToBestSolution(null);
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override object Clone() { 
            return new TeOptimizer<R_co, A_co>(this.OutputControl, this.TargetProblem, this.SolutionTemplate, this._problemSolutionTeSupport);
        }

        /// <summary>
        /// Gets or sets the current solution used during TE execution.
        /// </summary>
        /// <value>
        /// The current solution.
        /// </value>
        public TargetSolution<R_co, A_co>? CurrentSolution
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
            if (SolutionTemplate is null) 
                throw new ArgumentNullException(nameof(SolutionTemplate));
            CurrentSolution = (TargetSolution<R_co, A_co>) SolutionTemplate.Clone();
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
            if (CurrentSolution is null)
                throw new ArgumentNullException(nameof(CurrentSolution));
            Log.Debug("Overall number of evaluations: " + _overallNumberOfEvaluationsMethod(TargetProblem, CurrentSolution, this));
            WriteOutputHeadersIfNeeded();
            WriteOutputValuesIfNeeded("beforeAlgorithm", "b_a");
            while (true)
            {
                WriteOutputValuesIfNeeded("beforeIteration", "b_i");
                Iteration += 1;
                _progressMethod(this.TargetProblem, CurrentSolution, this);
                bool? new_is_better = QualityOfSolution.IsFirstFitnessBetter(CurrentSolution.QualitySingle, BestSolution!.QualitySingle, TargetProblem.IsMinimization == true);
                if(new_is_better == true)
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
            for(int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            s += groupStart;
            s = base.StringRep(delimiter, indentation, indentationSymbol, "", "");
            s += delimiter;
            if( CurrentSolution is not null )
                s += "CurrentSolution=" + CurrentSolution.StringRep(delimiter, indentation + 1, indentationSymbol, groupStart, groupEnd) + delimiter;
            else
                s += "CurrentSolution=null" + delimiter;
            for (int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            s += groupEnd;
            return s;
        }

    }
}

