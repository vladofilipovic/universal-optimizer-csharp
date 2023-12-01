
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
        /// <param name="initial_solution">The initial solution.</param>
        public SingleSolutionMetaheuristic(
                string name,
                FinishControl finishControl,
                int randomSeed,
                AdditionalStatisticsControl additionalStatisticsControl,
                OutputControl outputControl,
                TargetProblem targetProblem,
                TargetSolution<R_co, A_co> initial_solution)
                : base(name, finishControl: finishControl, randomSeed: randomSeed, additionalStatisticsControl: additionalStatisticsControl, outputControl: outputControl, targetProblem: targetProblem)
        {
            if (initial_solution is not null)
            {
                _currentSolution = (TargetSolution<R_co, A_co>)initial_solution.Clone();
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

        /// 
        /// Property getter for the current solution used during single solution metaheuristic execution
        /// 
        /// :return: instance of the :class:`uo.TargetSolution.TargetSolution` class subtype -- current solution of the problem 
        /// return type :class:`TargetSolution`        
        /// 
        /// 
        /// Property setter for the current solution used during single solution metaheuristic execution
        /// 
        /// :param value: the current solution
        /// :type value: :class:`TargetSolution`
        /// 
        public object currentSolution
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
        /// String representation of the SingleSolutionMetaheuristic instance
        /// 
        /// :param delimiter: delimiter between fields
        /// :type delimiter: str
        /// :param indentation: level of indentation
        /// :type indentation: int, optional, default value 0
        /// :param indentationSymbol: indentation symbol
        /// :type indentationSymbol: str, optional, default value ''
        /// :param groupStart: group start string 
        /// :type groupStart: str, optional, default value '{'
        /// :param groupEnd: group end string 
        /// :type groupEnd: str, optional, default value '}'
        /// :return: string representation of instance that controls output
        /// return type str
        /// 
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
            s = base.stringRep(delimiter, indentation, indentationSymbol, "", "");
            s += delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "currentSolution=" + currentSolution.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += groupEnd;
            return s;
        }

        /// 
        /// String representation of the `SingleSolutionMetaheuristic` instance
        /// 
        /// :return: string representation of the `SingleSolutionMetaheuristic` instance
        /// return type str
        /// 
        [abstractmethod]
        public override string ToString()
        {
            var s = this.stringRep("|");
            return s;
        }

        /// 
        /// String representation of the `SingleSolutionMetaheuristic` instance
        /// 
        /// :return: string representation of the `SingleSolutionMetaheuristic` instance
        /// return type str
        /// 
        [abstractmethod]
        public virtual string _repr__()
        {
            var s = this.stringRep("\n");
            return s;
        }

        /// 
        /// Formatted the `SingleSolutionMetaheuristic` instance
        /// 
        /// :param str spec: format specification
        /// :return: formatted `Metaheuristic` instance
        /// return type str
        /// 
        [abstractmethod]
        public virtual string _format__(string spec)
        {
            return StringRep("|");
        }
    }
}
}
