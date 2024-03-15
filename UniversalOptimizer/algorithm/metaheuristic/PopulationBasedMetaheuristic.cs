
namespace UniversalOptimizer.Algorithm.Metaheuristic
{

    using UniversalOptimizer.Algorithm;

    using UniversalOptimizer.Problem;

    using Solution;

    using System;

    using System.Linq;
    using System.Text;

    /// <summary>
    /// This class represent population metaheuristic.
    /// </summary>
    /// <typeparam name="R_co">The type of the co.</typeparam>
    /// <typeparam name="A_co">The type of the co.</typeparam>
    /// <seealso cref="UniversalOptimizer.Algorithm.Metaheuristic.Metaheuristic&lt;R_co, A_co&gt;" />
    public abstract class PopulationBasedMetaheuristic<R_co, A_co> : Metaheuristic<R_co, A_co> 
    {

        private IEnumerable<Solution<R_co,A_co>> _currentPopulation = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="PopulationBasedMetaheuristic{R_co, A_co}"/> 
        /// class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="finishControl">The finish control.</param>
        /// <param name="randomSeed">The random seed.</param>
        /// <param name="additionalStatisticsControl">The additional statistics control.</param>
        /// <param name="outputControl">The output control.</param>
        /// <param name="problem">The target problem.</param>
        /// <param name="solutionTemplate">The template for the solution.</param>
        public PopulationBasedMetaheuristic(
            string name,
            FinishControl finishControl,
            int randomSeed,
            AdditionalStatisticsControl additionalStatisticsControl,
            OutputControl outputControl,
            Problem problem,
            Solution<R_co, A_co>? solutionTemplate)
            : base(name, finishControl: finishControl, randomSeed: randomSeed, additionalStatisticsControl: additionalStatisticsControl, outputControl: outputControl, problem: problem, solutionTemplate: solutionTemplate)
        {
            _currentPopulation = [];
            BestSolution = null;
        }

        /// <summary>
        /// Property getter for the current population within population based metaheuristic execution.
        /// </summary>
        /// <value>
        /// The current solutions.
        /// </value>
        /// 
        public IEnumerable<Solution<R_co, A_co>> CurrentPopulation
        {
            get
            {
                return _currentPopulation;
            }
            set
            {
                _currentPopulation = value;
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
            StringBuilder s = new StringBuilder(delimiter);
            for(int i=0; i<indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append(groupStart);
            s.Append(base.StringRep(delimiter, indentation, indentationSymbol, "", ""));
            s.Append(delimiter);
            for(int i=0; i<indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append("CurrentPopulation=" + CurrentPopulation.ToString() + delimiter);
            for(int i=0; i<indentation; i++)
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
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var s = this.StringRep("|");
            return s;
        }

    }
}

