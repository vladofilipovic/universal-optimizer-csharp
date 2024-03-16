

namespace UniversalOptimizer.Algorithm.Exact.TotalEnumeration
{

    using UniversalOptimizer.Algorithm;
    using UniversalOptimizer.Problem;
    using UniversalOptimizer.Solution;

    using System;
    using System.Linq;
    using Serilog;
    using System.Reflection.Metadata.Ecma335;
    using System.Text;


    /// 
    ///     This class represent total enumeration algorithm
    ///     
    public class TeOptimizer<R_co, A_co> : Algorithm<R_co, A_co> 
    {

        private Solution<R_co, A_co>? _currentSolution;
        private readonly IProblemSolutionTeSupport<R_co, A_co> _problemSolutionTeSupport;
        private readonly ProblemSolutionTeSupportCanProgressMethod<R_co, A_co> _canProgressMethod;
        private readonly ProblemSolutionTeSupportProgressMethod<R_co, A_co> _progressMethod;
        private readonly ProblemSolutionTeSupportResetMethod<R_co, A_co> _resetMethod;
        private readonly ProblemSolutionTeSupportOverallNumberOfEvaluationsMethod<R_co, A_co> _overallNumberOfEvaluationsMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeOptimizer{R_co, A_co}"/> class.
        /// </summary>
        /// <param name="outputControl">The output control.</param>
        /// <param name="problem">The target problem.</param>
        /// <param name="solutionTemplate">The template for the solution.</param>
        /// <param name="problemSolutionTeSupport">The problem solution TE support.</param>
        public TeOptimizer(OutputControl outputControl, Problem problem, Solution<R_co, A_co>? solutionTemplate, IProblemSolutionTeSupport<R_co, A_co> problemSolutionTeSupport)
            : base("TotalEnumeration", outputControl: outputControl, problem: problem, solutionTemplate: solutionTemplate)
        {
            _problemSolutionTeSupport = problemSolutionTeSupport;
            /// total enumeration support
            _resetMethod = problemSolutionTeSupport.Reset;
            _progressMethod = problemSolutionTeSupport.Progress;
            _canProgressMethod = problemSolutionTeSupport.CanProgress;
            _overallNumberOfEvaluationsMethod = problemSolutionTeSupport.OverallNumberOfEvaluations;
            /// current and best solution
            _currentSolution = null;
            BestSolution = null;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override object Clone() { 
            return new TeOptimizer<R_co, A_co>(this.OutputControl, this.Problem, this.SolutionTemplate, this._problemSolutionTeSupport);
        }

        /// <summary>
        /// Gets or sets the current solution used during TE execution.
        /// </summary>
        /// <value>
        /// The current solution.
        /// </value>
        public Solution<R_co, A_co>? CurrentSolution
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
            CurrentSolution = (Solution<R_co, A_co>) SolutionTemplate.Clone();
            _resetMethod(Problem, CurrentSolution, this);
            WriteOutputValuesIfNeeded("beforeEvaluation", "b_e");
            Evaluation += 1;
            CurrentSolution.Evaluate(Problem);
            WriteOutputValuesIfNeeded("afterEvaluation", "a_e");
            BestSolution = CurrentSolution;
            Iteration = 1;
        }

        /// <summary>
        /// Method for optimization with Total Enumeration.
        /// </summary>
        /// <returns></returns>
        public override Solution<R_co, A_co>? Optimize()
        {            
            ExecutionStarted = DateTime.UtcNow;
            Init();
            if (CurrentSolution is null)
                throw new ArgumentNullException(nameof(CurrentSolution));
            Log.Debug("Overall number of evaluations: " + _overallNumberOfEvaluationsMethod(Problem, CurrentSolution, this));
            WriteOutputHeadersIfNeeded();
            WriteOutputValuesIfNeeded("beforeAlgorithm", "b_a");
            while (true)
            {
                WriteOutputValuesIfNeeded("beforeIteration", "b_i");
                Iteration += 1;
                _progressMethod(Problem, CurrentSolution, this);
                bool? new_is_better = IsFirstBetter(CurrentSolution, BestSolution!, Problem);
                if(new_is_better == true)
                {
                    BestSolution = CurrentSolution;
                }
                WriteOutputValuesIfNeeded("afterIteration", "a_i");
                if (!_canProgressMethod(Problem, CurrentSolution, this))
                {
                    break;
                }
            }
            ExecutionEnded = DateTime.UtcNow;
            this.WriteOutputValuesIfNeeded("afterAlgorithm", "a_a");
            return this.BestSolution;
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
            StringBuilder s = new StringBuilder(delimiter);
            for(int i=0; i<indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append(groupStart);
            s.Append(base.StringRep(delimiter, indentation, indentationSymbol, "", ""));
            s.Append(delimiter);
            if( CurrentSolution is not null )
                s.Append("CurrentSolution=" + CurrentSolution.StringRep(delimiter, indentation + 1, indentationSymbol, groupStart, groupEnd) + delimiter);
            else
                s.Append("CurrentSolution=null" + delimiter);
            for (int i=0; i<indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append(groupEnd);
            return s.ToString();
        }

    }
}

