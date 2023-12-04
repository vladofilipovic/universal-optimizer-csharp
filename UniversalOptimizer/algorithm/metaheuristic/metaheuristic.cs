/// <summary>
/// This module describes the class Metaheuristic
/// </summary>
/// 
namespace UniversalOptimizer.Algorithm.Metaheuristic
{
    using UniversalOptimizer.Algorithm;
    using UniversalOptimizer.TargetProblem;
    using UniversalOptimizer.TargetSolution;

    using System;
    using System.Linq;

    using Serilog;


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
            int? randomSeed,
            AdditionalStatisticsControl additionalStatisticsControl,
            OutputControl outputControl,
            TargetProblem targetProblem)
            : base(name, outputControl: outputControl, targetProblem: targetProblem)
        {
            FinishControl = finishControl;
            if (randomSeed is not null && randomSeed != 0)
            {
                RandomSeed = (int)randomSeed;
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
                Log.Debug("Iteration: " + this.Iteration.ToString() + ", Evaluations: " + this.Evaluation.ToString() + ", Best solution objective: " + this.BestSolution.ObjectiveValue.ToString() + ", Best solution fitness: " + this.BestSolution.FitnessValue.ToString() + ", Best solution: " + this.BestSolution.StringRepresentation());
            }
        }

        /// <summary>
        /// Executing optimization by the metaheuristic algorithm.
        /// </summary>
        public virtual void Optimize()
        {
            this.ExecutionStarted = DateTime.Now;
            this.Init();
            this.WriteOutputHeadersIfNeeded();
            WriteOutputValuesIfNeeded("beforeAlgorithm", "b_a");
            MainLoop();
            this.ExecutionEnded = DateTime.Now;
            WriteOutputValuesIfNeeded("afterAlgorithm", "a_a");
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
            s += "randomSeed=" + this.RandomSeed.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "finishControl=" + this.FinishControl.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "additionalStatisticsControl=" + this.AdditionalStatisticsControl.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "_iteration=" + this.Iteration.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "_iterationBestFound=" + IterationBestFound.ToString() + delimiter;
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

        /// <summary>
        /// String representation of the metaheuristic instance.
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
