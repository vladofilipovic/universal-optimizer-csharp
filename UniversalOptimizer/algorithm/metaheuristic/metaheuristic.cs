/// <summary>
/// This module describes the class Metaheuristic
/// </summary>
/// 
namespace UniversalOptimizer.algorithm.metaheuristic
{


    using uo.TargetProblem;

    using uo.TargetSolution;

    using System;

    using System.Linq;
    using UniversalOptimizer.algorithm;



    /// <summary>
    /// This class represent metaheuristic
    /// </summary>
    /// <seealso cref="uo.Algorithm" />
    public abstract class Metaheuristic<R_co, A_co> : Algorithm<R_co, A_co>, ICloneable
    {

        public AdditionalStatisticsControl AdditionalStatisticsControl { get; set; }

        public FinishControl FinishControl { get; set; }

        public int RandomSeed { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Metaheuristic{R_co, A_co}"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="finishControl">The finish control.</param>
        /// <param name="randomSeed">The random seed.</param>
        /// <param name="additionalStatisticsControl">The additional statistics control.</param>
        /// <param name="outputControl">The output control.</param>
        /// <param name="targetProblem">The target problem.</param>
        public Metaheuristic(
            string name,
            FinishControl finishControl,
            int randomSeed,
            AdditionalStatisticsControl additionalStatisticsControl,
            OutputControl outputControl,
            TargetProblem targetProblem)
            : base(name, outputControl: outputControl, targetProblem: targetProblem)
        {
            FinishControl = finishControl;
            if (randomSeed != 0)
            {
                RandomSeed = randomSeed;
            }
            else
            {
                RandomSeed = new Random().Next();
            }
            AdditionalStatisticsControl = additionalStatisticsControl;
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
        /// One iteration within main loop of the metaheuristic algorithm.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void MainLoopIteration()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculate time elapsed during execution of the metaheuristic algorithm.
        /// </summary>
        /// <returns>elapsed time (in seconds)</returns>
        public double ElapsedSeconds()
        {
            var delta = DateTime.Now - ExecutionStarted;
            return delta.TotalSeconds;
        }

        /// <summary>
        /// Main loop of the metaheuristic algorithm.
        /// </summary>
        /// 
        public void MainLoop()
        {
            while (!FinishControl.IsFinished(Evaluation, Iteration, ElapsedSeconds()))
            {
                WriteOutputValuesIfNeeded("beforeIteration", "b_i");
                MainLoopIteration();
                WriteOutputValuesIfNeeded("afterIteration", "a_i");
                logger.debug("Iteration: " + this.iteration.ToString() + ", Evaluations: " + this.evaluation.ToString() + ", Best solution objective: " + this.bestSolution.objectiveValue.ToString() + ", Best solution fitness: " + this.bestSolution.fitnessValue.ToString() + ", Best solution: " + this.bestSolution.stringRepresentation().ToString());
            }
        }

        /// 
        /// Executing optimization by the metaheuristic algorithm
        /// 
        public virtual object optimize()
        {
            this.executionStarted = datetime.now();
            this.init();
            this.writeOutputHeadersIfNeeded();
            WriteOutputValuesIfNeeded("beforeAlgorithm", "b_a");
            MainLoop();
            this.executionEnded = datetime.now();
            WriteOutputValuesIfNeeded("afterAlgorithm", "a_a");
        }

        /// 
        /// String representation of the Metaheuristic instance
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
            s += "randomSeed=" + this.randomSeed.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "finishControl=" + this.finishControl.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "additionalStatisticsControl=" + this.additionalStatisticsControl.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "_iteration=" + _iteration.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "_iterationBestFound=" + _iterationBestFound.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += groupEnd;
            return s;
        }

        /// 
        /// String representation of the `Metaheuristic` instance
        /// 
        /// :return: string representation of the `Metaheuristic` instance
        /// return type str
        /// 
        [abstractmethod]
        public override string ToString()
        {
            var s = this.stringRep("|");
            return s;
        }

        /// 
        /// String representation of the `Metaheuristic` instance
        /// 
        /// :return: string representation of the `Metaheuristic` instance
        /// return type str
        /// 
        [abstractmethod]
        public virtual string _repr__()
        {
            var s = this.stringRep("\n");
            return s;
        }

        /// 
        /// Formatted the `Metaheuristic` instance
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
