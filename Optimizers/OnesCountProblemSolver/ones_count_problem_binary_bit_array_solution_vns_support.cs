
namespace SingleObjective.Teaching.OnesCountProblem
{
    using UniversalOptimizer.utils;
    using UniversalOptimizer.TargetSolution;
    using UniversalOptimizer.Algorithm;
    using UniversalOptimizer.Algorithm.Metaheuristic.VariableNeighborhoodSearch;
    using SingleObjective.Teaching.OnesCountProblem;

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UniversalOptimizer.TargetProblem;
    using UniversalOptimizer.Algorithm.Metaheuristic;

    public class OnesCountProblemBinaryBitArraySolutionVnsSupport: IProblemSolutionVnsSupport<BitArray, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnesCountProblemBinaryBitArraySolutionVnsSupport"/> class.
        /// </summary>
        public OnesCountProblemBinaryBitArraySolutionVnsSupport()
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
        public bool Shaking(int k, TargetProblem problem, TargetSolution<BitArray, string> solution, Metaheuristic<BitArray, string> optimizer)
        {
            if (problem == null)
                throw new ArgumentNullException(string.Format("Parameter '{0}' is null.", nameof(problem)));
            if (problem is not OnesCountProblem)
                throw new ArgumentException(string.Format("Parameter '{0}' have not type 'OnesCountProblem'.", nameof(problem)));
            OnesCountProblem ocProblem = (OnesCountProblem)problem;
            if (solution.Representation == null)
                throw new ArgumentNullException(string.Format("Parameter '{0}' is null.", nameof(solution.Representation)));
            if (optimizer.FinishControl.CheckEvaluations && optimizer.Evaluation > optimizer.FinishControl.EvaluationsMax)
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
                    positions.Append(optimizer.RandomGenerator.Next(0, ocProblem.Dimension));
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
                if (optimizer.FinishControl.CheckEvaluations && optimizer.Evaluation > optimizer.FinishControl.EvaluationsMax)
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
        public bool LocalSearchBestImprovement(int k, TargetProblem problem, TargetSolution<BitArray, string> solution, Metaheuristic<BitArray, string> optimizer)
        {
            if (problem == null)
                throw new ArgumentNullException(string.Format("Parameter '{0}' is null.", nameof(problem)));
            if (problem is not OnesCountProblem)
                throw new ArgumentException(string.Format("Parameter '{0}' have not type 'OnesCountProblem'.", nameof(problem)));
            OnesCountProblem ocProblem = (OnesCountProblem)problem;
            if (solution.Representation == null)
                throw new ArgumentNullException(string.Format("Parameter '{0}' is null.", nameof(solution.Representation)));
            if (optimizer.FinishControl.CheckEvaluations && optimizer.Evaluation > optimizer.FinishControl.EvaluationsMax)
            {
                return false;
            }
            if (k < 1 || k > ocProblem.Dimension)
            {
                return false;
            }
            OnesCountProblemBinaryBitArraySolution startSolution = (OnesCountProblemBinaryBitArraySolution)solution.Clone();
            BitArray? bestRep = null;
            var bestTuple = new QualityOfSolution(solution.ObjectiveValue, solution.FitnessValue, solution.IsFeasible);
            /// initialize indexes
            var indexes = new ComplexCounterUniformAscending(k, ocProblem.Dimension);
            var in_loop = indexes.Reset();
            while (in_loop)
            {
                /// collect positions for inversion from indexes
                var positions = indexes.CurrentState();
                /// invert and compare, switch of new is better
                for ( var i = 0; i<positions.Count; i++ )
                {
                    solution.Representation[i] = !solution.Representation[i];
                }
                optimizer.Evaluation += 1;
                if (optimizer.FinishControl.CheckEvaluations && optimizer.Evaluation > optimizer.FinishControl.EvaluationsMax)
                {
                    solution.CopyFrom(startSolution);
                    return false;
                }
                optimizer.WriteOutputValuesIfNeeded("beforeEvaluation", "b_e");
                var newTuple = solution.CalculateQuality(problem);
                optimizer.WriteOutputValuesIfNeeded("afterEvaluation", "a_e");
                if (QualityOfSolution.IsFirstFitnessBetter(newTuple, bestTuple, problem.IsMinimization) == true)
                {
                    bestTuple = newTuple;
                    bestRep = new BitArray(solution.Representation);
                }
                for (var i = 0; i < positions.Count; i++)
                {
                    solution.Representation[i] = !solution.Representation[i];
                }
                /// increment indexes and set in_loop according to the state
                in_loop = indexes.Progress();
            }
            if (bestRep is not null)
            {
                solution.Representation = bestRep;
                solution.ObjectiveValue = bestTuple.ObjectiveValue ?? double.NaN;
                solution.FitnessValue = bestTuple.FitnessValue ?? double.NaN;
                solution.IsFeasible = bestTuple.IsFeasible;
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
        public bool LocalSearchFirstImprovement(int k, TargetProblem problem, TargetSolution<BitArray, string> solution, Metaheuristic<BitArray, string> optimizer)
        {
            if (problem == null)
                throw new ArgumentNullException(string.Format("Parameter '{0}' is null.", nameof(problem)));
            if (problem is not OnesCountProblem)
                throw new ArgumentException(string.Format("Parameter '{0}' have not type 'OnesCountProblem'.", nameof(problem)));
            OnesCountProblem ocProblem = (OnesCountProblem)problem;
            if (solution.Representation == null)
                throw new ArgumentNullException(string.Format("Parameter '{0}' is null.", nameof(solution.Representation)));
            if (optimizer.FinishControl.CheckEvaluations && optimizer.Evaluation > optimizer.FinishControl.EvaluationsMax)
            {
                return false;
            }
            if (k < 1 || k > ocProblem.Dimension)
            {
                return false;
            }
            OnesCountProblemBinaryBitArraySolution startSolution = (OnesCountProblemBinaryBitArraySolution)solution.Clone();
            var bestTuple = new QualityOfSolution(solution.ObjectiveValue, solution.FitnessValue, solution.IsFeasible);
            /// initialize indexes
            var indexes = new ComplexCounterUniformAscending(k, ocProblem.Dimension);
            var in_loop = indexes.Reset();
            while (in_loop)
            {
                /// collect positions for inversion from indexes
                var positions = indexes.CurrentState();
                /// invert and compare, switch and exit if new is better
                for (var i = 0; i < positions.Count; i++)
                {
                    solution.Representation[i] = !solution.Representation[i];
                }
                optimizer.Evaluation += 1;
                if (optimizer.FinishControl.CheckEvaluations && optimizer.Evaluation > optimizer.FinishControl.EvaluationsMax)
                {
                    solution.CopyFrom(startSolution);
                    return false;
                }
                optimizer.WriteOutputValuesIfNeeded("beforeEvaluation", "b_e");
                var newTuple = solution.CalculateQuality(problem);
                optimizer.WriteOutputValuesIfNeeded("afterEvaluation", "a_e");
                if (QualityOfSolution.IsFirstFitnessBetter(newTuple, bestTuple, problem.IsMinimization) == true)
                {
                    solution.ObjectiveValue = newTuple.ObjectiveValue ?? double.NaN;
                    solution.FitnessValue = newTuple.FitnessValue ?? double.NaN;
                    solution.IsFeasible = newTuple.IsFeasible;
                    return true;
                }
                for (var i = 0; i < positions.Count; i++)
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
            string groupEnd = "}") => "OnesCountProblemBinaryBitArraySolutionVnsSupport";

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
