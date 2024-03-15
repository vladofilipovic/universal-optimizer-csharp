namespace UniversalOptimizer.Algorithm
{
    using Problem;
    using Solution;

    using System;
    using System.Data.SqlTypes;
    using System.Linq;
    using System.Text;
    using System.Transactions;

    /// <summary>
    /// This class describes Algorithm
    /// </summary>
    /// <seealso cref="uo.Algorithm.Optimizer" />
    public abstract class Algorithm<R_co, A_co> : Optimizer<R_co, A_co>
    {
        private readonly Solution<R_co, A_co>? _solutionTemplate;
        private int _evaluation;
        private int _iteration;
        private int _evaluationBestFound;
        private int _iterationBestFound;

        protected Algorithm(string name, OutputControl outputControl, Problem problem, Solution<R_co, A_co>? solutionTemplate)
                : base(name, outputControl: outputControl, problem: problem)
        {
            _solutionTemplate = solutionTemplate;
            _evaluation = 0;
            _iteration = 0;
            _iterationBestFound = 0;
        }

        /// <summary>
        /// Gets the solution template.
        /// </summary>
        /// <value>
        /// The solution template.
        /// </value>
        public Solution<R_co, A_co>? SolutionTemplate
        {
            get
            {
                return _solutionTemplate;
            }
        }

        /// <summary>
        /// Property getter and setter for current number of evaluations during algorithm execution.
        /// </summary>
        /// <value>
        /// The current number of evaluations.
        /// </value>
        public int Evaluation
        {
            get
            {
                return _evaluation;
            }
            set
            {
                _evaluation = value;
            }
        }

        /// <summary>
        /// Property getter and setter for the iteration number of algorithm execution.
        /// </summary>
        /// <value>
        /// The iteration.
        /// </value>
        public int Iteration
        {
            get
            {
                return _iteration;
            }
            set
            {
                _iteration = value;
            }
        }

        /// <summary>
        /// Property getter and setter for the iteration number when the best solution is found.
        /// </summary>
        /// <value>
        /// The iteration.
        /// </value>
        public int EvaluationBestFound
        {
            get
            {
                return _evaluationBestFound;
            }
            set
            {
                _evaluationBestFound = value;
            }
        }
        /// <summary>
        /// Property getter and setter for the iteration number when the best solution is found.
        /// </summary>
        /// <value>
        /// The iteration.
        /// </value>
        public int IterationBestFound
        {
            get
            {
                return _iterationBestFound;
            }
            set
            {
                _iterationBestFound = value;
            }
        }

        /// <summary>
        /// Copies function argument to become the best solution within optimizer instance and update
        /// info about time and iteration when the best solution is updated.
        /// </summary>
        /// <param name="solution">The solution that is source for coping operation.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Bug", "S4275:Getters and setters should access the expected fields", Justification = "<Pending>")]
        public override Solution<R_co, A_co>? BestSolution
        {
            get
            {
                return base.BestSolution;
            }

            set
            {
                base.BestSolution = value;
                if (value is not null)
                {
                    IterationBestFound = _iteration;
                    EvaluationBestFound = _evaluation;
                }
            }
        }

        /// <summary>
        /// Determines whether [is first better] [the specified sol1].
        /// </summary>
        /// <param name="sol1">The sol1.</param>
        /// <param name="sol2">The sol2.</param>
        /// <param name="problem">The problem.</param>
        /// <returns></returns>
        /// <exception cref="System.MissingFieldException">
        /// IsMultiObjective
        /// or
        /// IsMinimization
        /// </exception>
        /// <exception cref="System.NotImplementedException">Comparison between solutions for multi objective optimization is not currently supported.</exception>
        public virtual bool? IsFirstBetter(Solution<R_co, A_co> sol1, Solution<R_co, A_co> sol2, Problem problem)
        {
            if(problem.IsMultiObjective is null)
                throw new MissingFieldException(nameof(problem.IsMultiObjective));
            if (problem.IsMinimization is null)
                throw new MissingFieldException(nameof(problem.IsMinimization));
            if (!(bool)problem.IsMultiObjective)
            {
                double? fit1 = sol1.FitnessValue;
                double? fit2 = sol2.FitnessValue;
                if (fit1 is null)
                {
                    if (fit2 is not null)
                        return false;
                    else
                        return null;
                }
                else
                {
                    if (fit2 is null)
                        return true;
                    if (((bool)problem.IsMinimization && fit1 < fit2) || ((bool)problem.IsMinimization && fit1 > fit2))
                        return true;
                    if (fit1 == fit2)
                        return null;
                    return false;
                }
            }
            else
            {
                throw new NotImplementedException("Comparison between solutions for multi objective optimization is not currently supported.");
            }
        }


        /// <summary>
        /// Initializes this algorithm.
        /// </summary>
        public abstract void Init();


        /// <summary>
        /// String representation of the algorithm instance.
        /// </summary>
        /// <param name="delimiter">The delimiter between fields.</param>
        /// <param name="indentation">The indentation level.</param>
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
            for (int i = 0; i < indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append(groupStart);
            for (int i = 0; i < indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append("name=" + Name + delimiter);
            for (int i = 0; i < indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append("Problem=" + Problem.StringRep(delimiter, indentation + 1, indentationSymbol, "{", "}") + delimiter);
            for (int i = 0; i < indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append("OutputControl=" + OutputControl.StringRep(delimiter, indentation + 1, indentationSymbol, "{", "}") + delimiter);
            s.Append("_evaluation=" + _evaluation.ToString() + delimiter);
            for (int i = 0; i < indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append("executionStarted=" + ExecutionStarted.ToString() + delimiter);
            for (int i = 0; i < indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append("executionEnded=" + ExecutionEnded.ToString() + delimiter);
            for (int i = 0; i < indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append(groupEnd);
            return s.ToString();
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString() => StringRep("|");

    }
}

