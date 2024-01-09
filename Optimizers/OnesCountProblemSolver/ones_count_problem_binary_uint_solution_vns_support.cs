
namespace SingleObjective.Teaching.OnesCountProblem
{

    using UniversalOptimizer.utils;
    using UniversalOptimizer.TargetProblem;
    using UniversalOptimizer.TargetSolution;
    using UniversalOptimizer.Algorithm;
    using UniversalOptimizer.Algorithm.Exact.TotalEnumeration;
    using UniversalOptimizer.Algorithm.Metaheuristic;
    using UniversalOptimizer.Algorithm.Metaheuristic.VariableNeighborhoodSearch;

    using SingleObjective.Teaching.OnesCountProblem;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;

    public class OnesCountProblemBinaryUIntSolutionVnsSupport: IProblemSolutionVnsSupport<uint,string>
    {

        public OnesCountProblemBinaryUIntSolutionVnsSupport()
        {
        }

 
        /// 
        /// Random VNS shaking of k parts such that new solution code does not differ more than k from all solution codes 
        /// inside shakingPoints 
        /// 
        /// :param int k: int parameter for VNS
        /// :param `OnesCountProblem` problem: problem that is solved
        /// :param `OnesCountProblemBinaryUIntSolution` solution: solution used for the problem that is solved
        /// :param `Algorithm` optimizer: optimizer that is executed
        /// :return: if shaking is successful
        /// return type bool
        /// 
        public bool Shaking(int k, TargetProblem problem, TargetSolution<uint, string> solution, Metaheuristic<uint, string> optimizer)
        {
            if (problem == null)
                throw new ArgumentNullException(string.Format("Parameter '{0}' is null.", nameof(problem)));
            if (problem is not OnesCountProblem)
                throw new ArgumentException(string.Format("Parameter '{0}' have not type 'OnesCountProblem'.", nameof(problem)));
            OnesCountProblem ocProblem = (OnesCountProblem)problem;
            if (optimizer.FinishControl.CheckEvaluations && optimizer.Evaluation > optimizer.FinishControl.EvaluationsMax)
            {
                return false;
            }
            var tries = 0;
            var limit = 10000;
            while (tries < limit)
            {
                var positions = new List<int>();
                foreach (var i in Enumerable.Range(0, k - 0))
                {
                    positions.Append(RandomNumberGenerator.GetInt32(ocProblem.Dimension));
                }
                uint mask = 0;
                foreach (var p in positions)
                {
                    mask |= (uint)(1 << p);
                }
                solution.Representation ^= mask;
                var all_ok = true;
                if (solution.Representation.CountOnes() > ocProblem.Dimension)
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

        /// 
        /// Executes "best improvement" variant of the local search procedure 
        /// 
        /// :param int k: int parameter for VNS
        /// :param `OnesCountProblem` problem: problem that is solved
        /// :param `OnesCountProblemBinaryUIntSolution` solution: solution used for the problem that is solved
        /// :param `Algorithm` optimizer: optimizer that is executed
        /// :return: result of the local search procedure 
        /// return type OnesCountProblemBinaryUIntSolution
        /// 
        public bool LocalSearchBestImprovement(int k, TargetProblem problem, TargetSolution<uint, string> solution, Metaheuristic<uint, string> optimizer)
        {
            if (problem == null)
                throw new ArgumentNullException(string.Format("Parameter '{0}' is null.", nameof(problem)));
            if (problem is not OnesCountProblem)
                throw new ArgumentException(string.Format("Parameter '{0}' have not type 'OnesCountProblem'.", nameof(problem)));
            OnesCountProblem ocProblem = (OnesCountProblem)problem;
            if (optimizer.FinishControl.CheckEvaluations && optimizer.Evaluation > optimizer.FinishControl.EvaluationsMax)
            {
                return false;
            }
            if (k < 1 || k > ocProblem.Dimension)
            {
                return false;
            }
            OnesCountProblemBinaryUIntSolution startSolution = (OnesCountProblemBinaryUIntSolution)solution.Clone(); 
            uint? bestRep = null;
            var bestTuple = new QualityOfSolution(solution.ObjectiveValue, solution.FitnessValue, solution.IsFeasible);
            /// initialize indexes
            var indexes = new ComplexCounterUniformAscending(k, ocProblem.Dimension);
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
                solution.Representation = (uint) bestRep;
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
        public bool LocalSearchFirstImprovement(int k, TargetProblem problem, TargetSolution<uint, string> solution,       Metaheuristic<uint, string> optimizer)
        {
            if (problem == null)
                throw new ArgumentNullException(string.Format("Parameter '{0}' is null.", nameof(problem)));
            if (problem is not OnesCountProblem)
                throw new ArgumentException(string.Format("Parameter '{0}' have not type 'OnesCountProblem'.", nameof(problem)));
            OnesCountProblem ocProblem = (OnesCountProblem)problem;
            if (optimizer.FinishControl.CheckEvaluations && optimizer.Evaluation > optimizer.FinishControl.EvaluationsMax)
            {
                return false;
            }
            if (k < 1 || k > ocProblem.Dimension)
            {
                return false;
            }
            OnesCountProblemBinaryUIntSolution startSolution = (OnesCountProblemBinaryUIntSolution)solution.Clone();
            var bestTuple = new QualityOfSolution(solution.ObjectiveValue, solution.FitnessValue, solution.IsFeasible);
            /// initialize indexes
            var indexes = new ComplexCounterUniformAscending(k, ocProblem.Dimension);
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
            return true;
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
            string groupEnd = "}") => "OnesCountProblemBinaryUIntSolutionVnsSupport";

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => this.StringRep("|");

    }
}

