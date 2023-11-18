
namespace uo.Algorithm
{


    using uo.Algorithm;
    using uo.TargetProblem;
    using uo.TargetSolution;


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

        private int _evaluation;
        private int _iteration;
        private int _iterationBestFound;

        public Algorithm(string name, OutputControl outputControl, TargetProblem targetProblem)
                : base(name, outputControl: outputControl, targetProblem: targetProblem)
        {
            _evaluation = 0;
            _iteration = 0;
            _iterationBestFound = 0;
        }


        /// <summary>
        /// Property getter for current number of evaluations during algorithm execution.
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
        /// Property getter for the iteration number of algorithm execution.
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
        /// Copies function argument to become the best solution within optimizer instance and update
        /// info about time and iteration when the best solution is updated.
        /// </summary>
        /// <param name="solution">The solution that is source for coping operation.</param>
        /// <returns></returns>
        public override void CopyToBestSolution(TargetSolution<R_co, A_co> solution)
        {
            base.CopyToBestSolution(solution);
            _iterationBestFound = this._iteration;
        }

        /// 
        /// Checks if first solution is better than the second one
        /// 
        /// :param TargetSolution sol1: first solution
        /// :param TargetSolution sol2: second solution
        /// :return: `True` if first solution is better, `False` if first solution is worse, `None` if fitnesses of both 
        ///         solutions are equal
        /// return type bool
        /// 
        public virtual bool is_first_solution_better(TargetSolution<R_co,A_co> sol1, TargetSolution<R_co, A_co> sol2)
        {
            QualityOfSolution fit2;
            QualityOfSolution fit1;
            if (this.TargetProblem is null)
            {
                throw new Exception("Target problem have to be defined within algorithm.");
            }
            if (this.TargetProblem.IsMinimization is null)
            {
                throw new ValueError("Information if minimization or maximization is set within metaheuristic target problemhave to be defined.");
            }
            var is_minimization = this.TargetProblem.is_minimization;
            if (sol1 is null)
            {
                fit1 = null;
            }
            else
            {
                fit1 = sol1.CalculateQuality(this.TargetProblem).fitnessValue;
            }
            if (sol2 is null)
            {
                fit2 = null;
            }
            else
            {
                fit2 = sol2.CalculateQuality(this.TargetProblem).fitnessValue;
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
            if (is_minimization && fit1 < fit2 || !is_minimization && fit1 > fit2)
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


        /// 
        /// String representation of the 'Algorithm' instance
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
        public virtual string StringRep(
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
            s = groupStart;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "name=" + this.name + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "TargetProblem=" + this.TargetProblem.stringRep(delimiter, indentation + 1, indentationSymbol, "{", "}") + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "_OutputControl=" + _OutputControl.stringRep(delimiter, indentation + 1, indentationSymbol, "{", "}") + delimiter;
            s += "_evaluation=" + _evaluation.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "executionStarted=" + this.executionStarted.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "executionEnded=" + this.executionEnded.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += groupEnd;
            return s;
        }

        /// 
        /// String representation of the 'Algorithm' instance
        /// 
        /// :return: string representation of the 'Algorithm' instance
        /// return type str
        /// 
        [abstractmethod]
        public override string ToString()
        {
            return this.StringRep("|");
        }

        /// 
        /// Representation of the 'Algorithm' instance
        /// 
        /// :return: string representation of the 'Algorithm' instance
        /// return type str
        /// 
        [abstractmethod]
        public virtual string _repr__()
        {
            return this.StringRep("\n");
        }

        /// 
        /// Formatted 'Algorithm' instance
        /// 
        /// :param str spec: format specification
        /// :return: formatted 'Algorithm' instance
        /// return type str
        /// 
        [abstractmethod]
        public virtual string _format__(string spec)
        {
            return this.StringRep("|");
        }
    }
}
}
