
namespace UniversalOptimizer.Algorithm.Metaheuristic
{

    using UniversalOptimizer.Algorithm;

    using UniversalOptimizer.TargetProblem;

    using UniversalOptimizer.TargetSolution;

    using System;

    using System.Linq;


    /// <summary>
    /// This class represent single solution metaheuristic.
    /// </summary>
    /// <typeparam name="R_co">The type of the co.</typeparam>
    /// <typeparam name="A_co">The type of the co.</typeparam>
    /// <seealso cref="UniversalOptimizer.Algorithm.Metaheuristic.Metaheuristic&lt;R_co, A_co&gt;" />
    public abstract class SingleSolutionMetaheuristic<R_co, A_co> : Metaheuristic<R_co, A_co>
    {

        private TargetSolution<R_co, A_co> _currentSolution;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleSolutionMetaheuristic{R_co, A_co}"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="finishControl">The finish control.</param>
        /// <param name="randomSeed">The random seed.</param>
        /// <param name="additionalStatisticsControl">The additional statistics control.</param>
        /// <param name="outputControl">The output control.</param>
        /// <param name="targetProblem">The target problem.</param>
        /// <param name="initialSolution">The initial solution.</param>
        public SingleSolutionMetaheuristic(
                string name,
                FinishControl finishControl,
                int? randomSeed,
                AdditionalStatisticsControl additionalStatisticsControl,
                OutputControl outputControl,
                TargetProblem targetProblem,
                TargetSolution<R_co, A_co> initialSolution)
                : base(name, finishControl: finishControl, randomSeed: randomSeed, additionalStatisticsControl: additionalStatisticsControl, outputControl: outputControl, targetProblem: targetProblem)
        {
            if (initialSolution is not null)
            {
                _currentSolution = (TargetSolution<R_co, A_co>)initialSolution.Clone();
            }
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public object Clone()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Property getter and setter for the current solution used during single solution 
        /// metaheuristic execution.
        /// </summary>
        /// <value>
        /// The current solution.
        /// </value>
        public TargetSolution<R_co,A_co> CurrentSolution
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

        /// <summary>
        /// String representation of the metaheuristic instance.
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
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += groupStart;
            s = base.StringRep(delimiter, indentation, indentationSymbol, "", "");
            s += delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "currentSolution=" + CurrentSolution.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
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
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var s = this.StringRep("|");
            return s;
        }

    }
}

