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

    public class FunctionOneVariableProblemBinaryUIntSolutionVnsSupport : IProblemSolutionVnsSupport<uint, double>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionOneVariableProblemBinaryUIntSolutionVnsSupport"/> class.
        /// </summary>
        public FunctionOneVariableProblemBinaryUIntSolutionVnsSupport()
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
        public bool Shaking(int k, TargetProblem problem, TargetSolution<uint, double> solution, Metaheuristic<uint, double> optimizer)
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
                    _ = positions.Append((new Random()).Next(representationLength));
                }
                uint mask = 0;
                foreach (var p in positions)
                {
                    mask |= (uint)(1 << p);
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
        public bool LocalSearchBestImprovement(int k, TargetProblem problem, TargetSolution<uint, double> solution, Metaheuristic<uint, double> optimizer)
        {
            int representationLength = 32;
            if (optimizer.FinishControl.CheckEvaluations && optimizer.Evaluation > optimizer.FinishControl.EvaluationsMax)
            {
                return false;
            }
            if (k < 1 || k > representationLength)
            {
                return false;
            }
            FunctionOneVariableProblemBinaryUIntSolution startSolution = (FunctionOneVariableProblemBinaryUIntSolution)solution.Clone();
            uint? bestRep = null;
            var bestTuple = new QualityOfSolution(objectiveValue: solution.ObjectiveValue,
                    fitnessValue: solution.FitnessValue,
                    isFeasible: solution.IsFeasible ?? false);
            /// initialize indexes
            var indexes = new ComplexCounterUniformAscending(k, representationLength);
            var in_loop = indexes.Reset();
            while (in_loop)
            {
                /// collect positions for inversion from indexes
                var positions = indexes.CurrentState();
                /// invert and compare, switch of new is better
                uint mask = 0;
                foreach (var i in positions)
                {
                    mask |= (uint)(1 << i);
                }
                solution.Representation ^= mask;
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
                    bestRep = solution.Representation;
                }
                solution.Representation ^= mask;
                /// increment indexes and set in_loop accordingly
                in_loop = indexes.Progress();
            }
            if (bestRep is not null)
            {
                solution.Representation = (uint)bestRep;
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
        /// Solution - result of the local search procedure.
        /// </returns>
        public bool LocalSearchFirstImprovement(int k, TargetProblem problem, TargetSolution<uint, double> solution, Metaheuristic<uint, double> optimizer)
        {
            var representationLength = 32;
            if (optimizer.FinishControl.CheckEvaluations && optimizer.Evaluation > optimizer.FinishControl.EvaluationsMax)
            {
                return false;
            }
            if (k < 1 || k > representationLength)
            {
                return false;
            }
            FunctionOneVariableProblemBinaryUIntSolution startSolution = (FunctionOneVariableProblemBinaryUIntSolution)solution.Clone();
            var bestTuple = new QualityOfSolution(objectiveValue: solution.ObjectiveValue,
                fitnessValue: solution.FitnessValue,
                isFeasible: solution.IsFeasible ?? false);
            /// initialize indexes
            var indexes = new ComplexCounterUniformAscending(k, representationLength);
            var in_loop = indexes.Reset();
            while (in_loop)
            {
                /// collect positions for inversion from indexes
                var positions = indexes.CurrentState();
                /// invert and compare, switch and exit if new is better
                uint mask = 0;
                foreach (var i in positions)
                {
                    mask |= (uint)(1 << i);
                }
                solution.Representation ^= mask;
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
                    solution.FitnessValue = newTuple.FitnessValue ?? double.NaN;
                    solution.ObjectiveValue = newTuple.ObjectiveValue ?? double.NaN;
                    solution.IsFeasible = newTuple.IsFeasible;
                    return true;
                }
                solution.Representation ^= mask;
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
            string groupEnd = "}") => "FunctionOneVariableProblemBinaryUIntSolutionVnsSupport";

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => StringRep("|");

    }
}

