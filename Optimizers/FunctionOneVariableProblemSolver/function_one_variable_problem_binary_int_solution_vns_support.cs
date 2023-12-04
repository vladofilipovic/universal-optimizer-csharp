namespace SingleObjective.Teaching.FunctionOneVariableProblem
{
    using UniversalOptimizer.TargetProblem;

    using UniversalOptimizer.TargetSolution;

    using UniversalOptimizer.Algorithm.Metaheuristic;

    using UniversalOptimizer.Algorithm.Metaheuristic.VariableNeighborhoodSearch;

    using System.Collections.Generic;

    using System;

    using System.Linq;
    using UniversalOptimizer.utils;

    public class FunctionOneVariableProblemBinaryIntSolutionVnsSupport : IProblemSolutionVnsSupport<int, double>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionOneVariableProblemBinaryIntSolutionVnsSupport"/> class.
        /// </summary>
        public FunctionOneVariableProblemBinaryIntSolutionVnsSupport()
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
        /// <param name="solutionRepresentations">The solution representations that should be shaken.</param>
        /// <returns>
        /// if shaking is successful
        /// </returns>
        public bool Shaking(int k, TargetProblem problem, TargetSolution<int, double> solution, Metaheuristic<int, double> optimizer, IEnumerable<int> solutionRepresentations)
        {
            if (optimizer.FinishControl.CheckEvaluations && optimizer.Evaluation > optimizer.FinishControl.EvaluationsMax)
            {
                return false;
            }
            var tries = 0;
            var limit = 10000;
            int representationLength = 32;
            while (tries < limit)
            {
                var positions = new List<int>();
                foreach (var i in Enumerable.Range(0, k - 0))
                {
                    positions.Append((new Random()).Next(representationLength));
                }
                var mask = 0;
                foreach (var p in positions)
                {
                    mask |= 1 << p;
                }
                solution.Representation ^= mask;
                var all_ok = true;
                if (Convert.ToString(solution.Representation, 2).Count(c => c == '1') > representationLength)
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
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Executes "best improvement" variant of the local search procedure.
        /// </summary>
        /// <param name="k">The k parameter for VNS.</param>
        /// <param name="problem">The problem that is solved.</param>
        /// <param name="solution">The solution used for the problem that is solved.</param>
        /// <param name="optimizer">The optimizer that is executed.</param>
        /// <returns>
        /// Solution - result of the local search procedure.
        /// </returns>
        public TargetSolution<int, double> LocalSearchBestImprovement(int k, TargetProblem problem, TargetSolution<int, double> solution, Metaheuristic<int, double> optimizer)
        {
            int representationLength = 32;
            if (optimizer.FinishControl.CheckEvaluations && optimizer.Evaluation > optimizer.FinishControl.EvaluationsMax)
            {
                return solution;
            }
            if (k < 1 || k > representationLength)
            {
                return solution;
            }
            int? bestRep = null;
            var bestTuple = new QualityOfSolution()
            {
                ObjectiveValue = solution.ObjectiveValue,
                FitnessValue = solution.FitnessValue,
                IsFeasible = solution.IsFeasible
            };
            /// initialize indexes
            var indexes = new ComplexCounterUniformAscending(k, representationLength);
            var in_loop = indexes.Reset();
            while (in_loop)
            {
                /// collect positions for inversion from indexes
                var positions = indexes.CurrentState();
                /// invert and compare, switch of new is better
                var mask = 0;
                foreach (var i in positions)
                {
                    mask |= 1 << i;
                }
                solution.Representation ^= mask;
                optimizer.Evaluation += 1;
                if (optimizer.FinishControl.CheckEvaluations && optimizer.Evaluation > optimizer.FinishControl.EvaluationsMax)
                {
                    return solution;
                }
                optimizer.WriteOutputValuesIfNeeded("beforeEvaluation", "b_e");
                var newTuple = solution.CalculateQuality(problem);
                optimizer.WriteOutputValuesIfNeeded("afterEvaluation", "a_e");
                if (newTuple.FitnessValue > bestTuple.FitnessValue)
                {
                    bestTuple = newTuple;
                    bestRep = solution.Representation;
                }
                solution.Representation ^= mask;
                /// increment indexes and set in_loop accordingly
                in_loop = indexes.Progress();
            }
            if (bestRep is not null)
            {
                solution.Representation = (int)bestRep;
                solution.ObjectiveValue = bestTuple.ObjectiveValue;
                solution.FitnessValue = bestTuple.FitnessValue;
                solution.IsFeasible = bestTuple.IsFeasible;
                return solution;
            }
            return solution;
        }

        /// <summary>
        /// Executes "first improvement" variant of the local search procedure.
        /// </summary>
        /// <param name="k">The k parameter for VNS.</param>
        /// <param name="problem">The problem that is solved.</param>
        /// <param name="solution">The solution used for the problem that is solved.</param>
        /// <param name="optimizer">The optimizer that is executed.</param>
        /// <returns>
        /// Solution - result of the local search procedure.
        /// </returns>
        public TargetSolution<int, double> LocalSearchFirstImprovement(int k, TargetProblem problem, TargetSolution<int, double> solution, Metaheuristic<int, double> optimizer)
        {
            var representationLength = 32;
            if (optimizer.FinishControl.CheckEvaluations && optimizer.Evaluation > optimizer.FinishControl.EvaluationsMax)
            {
                return solution;
            }
            if (k < 1 || k > representationLength)
            {
                return solution;
            }
            var best_fv = solution.FitnessValue;
            /// initialize indexes
            var indexes = new ComplexCounterUniformAscending(k, representationLength);
            var in_loop = indexes.Reset();
            while (in_loop)
            {
                /// collect positions for inversion from indexes
                var positions = indexes.CurrentState();
                /// invert and compare, switch and exit if new is better
                var mask = 0;
                foreach (var i in positions)
                {
                    mask |= 1 << i;
                }
                solution.Representation ^= mask;
                optimizer.Evaluation += 1;
                if (optimizer.FinishControl.CheckEvaluations && optimizer.Evaluation > optimizer.FinishControl.EvaluationsMax)
                {
                    return solution;
                }
                optimizer.WriteOutputValuesIfNeeded("beforeEvaluation", "b_e");
                var newTuple = solution.CalculateQuality(problem);
                optimizer.WriteOutputValuesIfNeeded("afterEvaluation", "a_e");
                if (newTuple.FitnessValue > best_fv)
                {
                    solution.FitnessValue = newTuple.FitnessValue;
                    solution.ObjectiveValue = newTuple.ObjectiveValue;
                    solution.IsFeasible = newTuple.IsFeasible;
                    return solution;
                }
                solution.Representation ^= mask;
                /// increment indexes and set in_loop accordingly
                in_loop = indexes.Progress();
            }
            return solution;
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
        public new string StringRep(
            string delimiter,
            int indentation = 0,
            string indentationSymbol = "",
            string groupStart = "{",
            string groupEnd = "}")
        {
            return "FunctionOneVariableProblemBinaryIntSolutionVnsSupport";
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return StringRep("|");
        }
    }
}

