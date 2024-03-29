
namespace SingleObjective.Teaching.OnesCountProblem
{
    using UniversalOptimizer.utils;
    using UniversalOptimizer.Solution;
    using UniversalOptimizer.Algorithm;
    using UniversalOptimizer.Algorithm.Metaheuristic.VariableNeighborhoodSearch;
    using SingleObjective.Teaching.OnesCountProblem;

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UniversalOptimizer.Problem;
    using UniversalOptimizer.Algorithm.Metaheuristic;

    public class OnesCountMaxProblemBinaryBitArraySolutionVnsSupport: IProblemSolutionVnsSupport<BitArray, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnesCountMaxProblemBinaryBitArraySolutionVnsSupport"/> class.
        /// </summary>
        public OnesCountMaxProblemBinaryBitArraySolutionVnsSupport()
        {
        }

        /// <summary>
        /// Random VNS shaking of several parts such that new solution code does not differ more
        /// than supplied from all solution codes inside collection.
        /// </summary>
        /// <param name="k">The k parameter for VNS.</param>
        /// <param name="problem">The problem that is solved.</param>
        /// <param name="solution">The solution used for the problem that is solved.</param>
        /// <param name="optimizer">The optimizer that is executed.</param>
        /// <returns>
        /// if shaking is successful
        /// </returns>
        public bool Shaking(int k, Problem problem, Solution<BitArray, string> solution, Metaheuristic<BitArray, string> optimizer)
        {
            if (problem == null)
                throw new ArgumentNullException(string.Format("Parameter '{0}' is null.", nameof(problem)));
            if (problem is not OnesCountMaxProblem)
                throw new ArgumentException(string.Format("Parameter '{0}' have not type 'OnesCountMaxProblem'.", nameof(problem)));
            OnesCountMaxProblem ocProblem = (OnesCountMaxProblem)problem;
            if (solution.Representation == null)
                throw new ArgumentNullException(string.Format("Parameter '{0}' is null.", nameof(solution.Representation)));
            if (optimizer.FinishControl.IsFinished(optimizer.Evaluation, optimizer.Iteration, optimizer.ElapsedSeconds()))
            {
                return false;
            }
            var tries = 0;
            var limit = 10000;
            while (tries < limit)
            {
                var positions = new List<int>();
                for (int i=0; i<k; i++)
                {
                    _ = positions.Append(optimizer.RandomGenerator.Next(0, ocProblem.Dimension));
                }
                var repr = new BitArray(solution.Representation);
                foreach (int pos in positions)
                {
                    repr[pos] = !repr[pos];
                }
                solution.Representation = repr;
                var all_ok = true;
                if (solution.Representation!.CountOnes() > ocProblem.Dimension)
                {
                    all_ok = false;
                }
                if (all_ok)
                {
                    break;
                }
            }
            if (tries < limit)
            {
                optimizer.Evaluation += 1;
                if (optimizer.FinishControl.IsFinished(optimizer.Evaluation, optimizer.Iteration, optimizer.ElapsedSeconds()))
                {
                    return false;
                }
                optimizer.WriteOutputValuesIfNeeded("beforeEvaluation", "b_e");
                solution.Evaluate(problem);
                optimizer.WriteOutputValuesIfNeeded("afterEvaluation", "a_e");
                optimizer.WriteOutputValuesIfNeeded("afterStepInIteration", "shaking");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Executes "best improvement" variant of the local search procedure.
        /// </summary>
        /// <param name="k">The k parameter for VNS.</param>
        /// <param name="problem">The problem that is solved.</param>
        /// <param name="solution">The solution used for the problem that is solved.</param>
        /// <param name="optimizer">The optimizer that is executed.</param>
        /// <returns>
        /// if the local search procedure is successful.
        /// </returns>
        public bool LocalSearchBestImprovement(int k, Problem problem, Solution<BitArray, string> solution, Metaheuristic<BitArray, string> optimizer)
        {
            if (problem == null)
                throw new ArgumentNullException(string.Format("Parameter '{0}' is null.", nameof(problem)));
            if (problem is not OnesCountMaxProblem)
                throw new ArgumentException(string.Format("Parameter '{0}' have not type 'OnesCountMaxProblem'.", nameof(problem)));
            OnesCountMaxProblem ocProblem = (OnesCountMaxProblem)problem;
            if (solution.Representation == null)
                throw new ArgumentNullException(string.Format("Parameter '{0}' is null.", nameof(solution.Representation)));
            if (optimizer.FinishControl.IsFinished(optimizer.Evaluation, optimizer.Iteration, optimizer.ElapsedSeconds()))
            {
                return false;
            }
            if (k < 1 || k > ocProblem.Dimension)
            {
                return false;
            }
            OnesCountMaxProblemBinaryBitArraySolution startSolution = (OnesCountMaxProblemBinaryBitArraySolution)solution.Clone();
            OnesCountMaxProblemBinaryBitArraySolution bestSolution = (OnesCountMaxProblemBinaryBitArraySolution)solution.Clone();
            bool betterSolutionFound = false; 
            /// initialize indexes
            var indexes = new ComplexCounterUniformAscending(k, ocProblem.Dimension);
            var in_loop = indexes.Reset();
            while (in_loop)
            {
                /// collect positions for inversion from indexes
                var positions = indexes.CurrentState();
                /// invert and compare, switch of new is better
                for ( var i = 0; i<positions.Length; i++ )
                {
                    solution.Representation[i] = !solution.Representation[i];
                }
                optimizer.Evaluation += 1;
                if (optimizer.FinishControl.IsFinished(optimizer.Evaluation, optimizer.Iteration, optimizer.ElapsedSeconds()))
                {
                    solution.CopyFrom(startSolution);
                    return false;
                }
                optimizer.WriteOutputValuesIfNeeded("beforeEvaluation", "b_e");
                solution.Evaluate(problem);
                optimizer.WriteOutputValuesIfNeeded("afterEvaluation", "a_e");
                if (optimizer.IsFirstBetter(solution, bestSolution, problem) == true)
                {
                    betterSolutionFound = true;
                    bestSolution.CopyFrom(solution);
                }
                for (var i = 0; i < positions.Length; i++)
                {
                    solution.Representation[i] = !solution.Representation[i];
                }
                /// increment indexes and set in_loop according to the state
                in_loop = indexes.Progress();
            }
            if (betterSolutionFound)
            {
                solution.CopyFrom(bestSolution);
                return true;
            }
            solution.CopyFrom(startSolution);
            return false;
        }

        /// <summary>
        /// Executes "first improvement" variant of the local search procedure.
        /// </summary>
        /// <param name="k">The k parameter for VNS.</param>
        /// <param name="problem">The problem that is solved.</param>
        /// <param name="solution">The solution used for the problem that is solved.</param>
        /// <param name="optimizer">The optimizer that is executed.</param>
        /// <returns>
        /// if the local search procedure is successful.
        /// </returns>
        public bool LocalSearchFirstImprovement(int k, Problem problem, Solution<BitArray, string> solution, Metaheuristic<BitArray, string> optimizer)
        {
            if (problem == null)
                throw new ArgumentNullException(string.Format("Parameter '{0}' is null.", nameof(problem)));
            if (problem is not OnesCountMaxProblem)
                throw new ArgumentException(string.Format("Parameter '{0}' have not type 'OnesCountMaxProblem'.", nameof(problem)));
            OnesCountMaxProblem ocProblem = (OnesCountMaxProblem)problem;
            if (solution.Representation == null)
                throw new ArgumentNullException(string.Format("Parameter '{0}' is null.", nameof(solution.Representation)));
            if (optimizer.FinishControl.IsFinished(optimizer.Evaluation, optimizer.Iteration, optimizer.ElapsedSeconds()))
            {
                return false;
            }
            if (k < 1 || k > ocProblem.Dimension)
            {
                return false;
            }
            OnesCountMaxProblemBinaryBitArraySolution startSolution = (OnesCountMaxProblemBinaryBitArraySolution)solution.Clone();
            /// initialize indexes
            var indexes = new ComplexCounterUniformAscending(k, ocProblem.Dimension);
            var in_loop = indexes.Reset();
            while (in_loop)
            {
                /// collect positions for inversion from indexes
                var positions = indexes.CurrentState();
                /// invert and compare, switch and exit if new is better
                for (var i = 0; i < positions.Length; i++)
                {
                    solution.Representation[i] = !solution.Representation[i];
                }
                optimizer.Evaluation += 1;
                if (optimizer.FinishControl.IsFinished(optimizer.Evaluation, optimizer.Iteration, optimizer.ElapsedSeconds()))
                {
                    solution.CopyFrom(startSolution);
                    return false;
                }
                optimizer.WriteOutputValuesIfNeeded("beforeEvaluation", "b_e");
                solution.Evaluate(problem);
                optimizer.WriteOutputValuesIfNeeded("afterEvaluation", "a_e");
                if (optimizer.IsFirstBetter(solution, startSolution, problem) == true)
                {
                    return true;
                }
                for (var i = 0; i < positions.Length; i++)
                {
                    solution.Representation[i] = !solution.Representation[i];
                }
                /// increment indexes and set in_loop accordingly
                in_loop = indexes.Progress();
            }
            solution.CopyFrom(startSolution);
            return false;
        }

        /// <summary>
        /// Strings the rep.
        /// </summary>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="indentation">The indentation.</param>
        /// <param name="indentationSymbol">The indentation symbol.</param>
        /// <param name="groupStart">The group start.</param>
        /// <param name="groupEnd">The group end.</param>
        /// <returns></returns>
        public string StringRep(
            string delimiter,
            int indentation = 0,
            string indentationSymbol = "",
            string groupStart = "{",
            string groupEnd = "}") => "OnesCountMaxProblemBinaryBitArraySolutionVnsSupport";

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => this.StringRep("|");

        /// <summary>
        /// Representation of this instance.
        /// </summary>
        /// <returns></returns>
        public virtual string _repr__() => this.StringRep("\n");

        /// <summary>
        /// Formats the specified spec.
        /// </summary>
        /// <param name="spec">The spec.</param>
        /// <returns></returns>
        public virtual string _format__(string spec) => this.StringRep("|");

    }

}
