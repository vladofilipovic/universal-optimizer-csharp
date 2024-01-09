namespace UniversalOptimizer.Algorithm
{
    using TargetProblem;
    using TargetSolution;


    using System;
    using System.Linq;
    using System.Transactions;
    using System.Data.SqlTypes;

    /// <summary>
    /// This class describes Algorithm
    /// </summary>
    /// <seealso cref="uo.Algorithm.Optimizer" />
    public abstract class Algorithm<R_co, A_co> : Optimizer<R_co, A_co>  
    {
        private TargetSolution<R_co, A_co>? _solutionTemplate;
        private int _evaluation;
        private int _iteration;
        private int _iterationBestFound;

        public Algorithm(string name, OutputControl outputControl, TargetProblem targetProblem, TargetSolution<R_co, A_co>? solutionTemplate)
                : base(name, outputControl: outputControl, targetProblem: targetProblem)
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
        public TargetSolution<R_co, A_co>? SolutionTemplate
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
        public override void CopyToBestSolution(TargetSolution<R_co, A_co>? solution)
        {
            base.CopyToBestSolution(solution);
            if (solution is not null)
            {
                this.IterationBestFound = _iteration;
            }
        }

        /// <summary>
        /// Determines whether [is first solution better] [the specified sol1].
        /// </summary>
        /// <param name="sol1">The first solution.</param>
        /// <param name="sol2">The second solution.</param>
        /// <returns>
        ///   <c>true</c> if [is first solution better] [the specified sol1]; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="Exception">Target problem have to be defined within
        /// algorithm.</exception>
        public bool? IsFirstSolutionBetter(TargetSolution<R_co, A_co> sol1, TargetSolution<R_co, A_co> sol2)
        {
            double? fit2;
            double? fit1;
            if (TargetProblem is null)
            {
                throw new ArgumentException("Target problem have to be defined within algorithm.");
            }
            bool isMinimization = TargetProblem.IsMinimization;
            if (sol1 is null)
            {
                fit1 = null;
            }
            else
            {
                fit1 = sol1.CalculateQuality(TargetProblem).FitnessValue;
            }
            if (sol2 is null)
            {
                fit2 = null;
            }
            else
            {
                fit2 = sol2.CalculateQuality(TargetProblem).FitnessValue;
            }
            /// with fitness is better than without fitness
            if (fit1 is null)
            {
                if (fit2 is not null)
                {
                    return false;
                }
                else
                {
                    return null;
                }
            }
            else if (fit2 is null)
            {
                return true;
            }
            /// if better, return true
            if (isMinimization && fit1 < fit2 || !isMinimization && fit1 > fit2)
            {
                return true;
            }
            /// if same fitness, return None
            if (fit1 == fit2)
            {
                return null;
            }
            /// otherwise, return false
            return false;
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
            var s = delimiter;
            for(int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            s = groupStart;
            for(int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            s += "name=" + Name + delimiter;
            for(int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            s += "TargetProblem=" + TargetProblem.StringRep(delimiter, indentation + 1, indentationSymbol, "{", "}") + delimiter;
            for(int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            s += "OutputControl=" + OutputControl.StringRep(delimiter, indentation + 1, indentationSymbol, "{", "}") + delimiter;
            s += "_evaluation=" + _evaluation.ToString() + delimiter;
            for(int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            s += "executionStarted=" + ExecutionStarted.ToString() + delimiter;
            for(int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            s += "executionEnded=" + ExecutionEnded.ToString() + delimiter;
            for(int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            s += groupEnd;
            return s;
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

