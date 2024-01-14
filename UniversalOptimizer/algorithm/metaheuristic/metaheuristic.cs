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
    using System.Text;


    /// <summary>
    /// This class represent metaheuristic
    /// </summary>
    /// <seealso cref="uo.Algorithm" />
    public abstract class Metaheuristic<R_co, A_co> : Algorithm<R_co, A_co> 
    {

        public AdditionalStatisticsControl AdditionalStatisticsControl { get; set; }

        public FinishControl FinishControl { get; set; }

        public int RandomSeed { get; set; }

        private readonly Random _randomGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="Metaheuristic{R_co, A_co}"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="finishControl">The finish control.</param>
        /// <param name="randomSeed">The random seed.</param>
        /// <param name="additionalStatisticsControl">The additional statistics control.</param>
        /// <param name="outputControl">The output control.</param>
        /// <param name="targetProblem">The target problem.</param>
        protected Metaheuristic(
            string name,
            FinishControl finishControl,
            int? randomSeed,
            AdditionalStatisticsControl additionalStatisticsControl,
            OutputControl outputControl,
            TargetProblem targetProblem,
            TargetSolution<R_co, A_co>? solutionTemplate)
            : base(name, outputControl: outputControl, targetProblem: targetProblem, solutionTemplate: solutionTemplate)
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
            _randomGenerator = new Random(RandomSeed);
            AdditionalStatisticsControl = additionalStatisticsControl;
        }

        /// <summary>
        /// Gets the random generator.
        /// </summary>
        /// <value>
        /// The random generator.
        /// </value>
        public Random RandomGenerator 
        {  
            get { return _randomGenerator; } 
        }

        /// <summary>
        /// One iteration within main loop of the metaheuristic algorithm.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void MainLoopIteration() => throw new NotImplementedException();

        /// <summary>
        /// Calculate time elapsed during execution of the metaheuristic algorithm.
        /// </summary>
        /// <returns>elapsed time (in seconds)</returns>
        public double ElapsedSeconds()
        {
            var delta = DateTime.UtcNow - ExecutionStarted;
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
                Log.Debug("Iteration: " + this.Iteration.ToString() + ", Evaluations: " + this.Evaluation.ToString() + ", Best solution objective: " + this.BestSolution!.ObjectiveValue.ToString() + ", Best solution fitness: " + this.BestSolution!.FitnessValue.ToString() + ", Best solution: " + this.BestSolution!.StringRepresentation());
            }
        }

        public void UpdateAdditionalStatisticsIfRequired(TargetSolution<R_co,A_co> solution)
        {
            if (AdditionalStatisticsControl == null)
                throw new FieldAccessException(nameof(AdditionalStatisticsControl));
            if (!AdditionalStatisticsControl.IsActive)
            {
                return;
            }
            if (AdditionalStatisticsControl.KeepAllSolutionCodes)
                AdditionalStatisticsControl.AddToAllSolutionCodes(solution.StringRepresentation());
            if (AdditionalStatisticsControl.KeepMoreLocalOptima)
                AdditionalStatisticsControl.AddToMoreLocalOptima(solution.StringRepresentation(), solution.FitnessValue, BestSolution?.StringRepresentation()??"invalid");
        }


        /// <summary>
        /// Executing optimization by the metaheuristic algorithm.
        /// </summary>
        public override void Optimize()
        {
            this.ExecutionStarted = DateTime.UtcNow;
            this.Init();
            this.WriteOutputHeadersIfNeeded();
            WriteOutputValuesIfNeeded("beforeAlgorithm", "b_a");
            MainLoop();
            this.ExecutionEnded = DateTime.UtcNow;
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
            s.Append("randomSeed=" + this.RandomSeed.ToString() + delimiter);
            for(int i=0; i<indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append("finishControl=" + this.FinishControl.ToString() + delimiter);
            for(int i=0; i<indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append("additionalStatisticsControl=" + this.AdditionalStatisticsControl.ToString() + delimiter);
            for(int i=0; i<indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append("_iteration=" + this.Iteration.ToString() + delimiter);
            for(int i=0; i<indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append("_iterationBestFound=" + IterationBestFound.ToString() + delimiter);
            for(int i=0; i<indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            for(int i=0; i<indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append(groupEnd);
            return s.ToString();
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
            return s.ToString();
        }

    }
}
