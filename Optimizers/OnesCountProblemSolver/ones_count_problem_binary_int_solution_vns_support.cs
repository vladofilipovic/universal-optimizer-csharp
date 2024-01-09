
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
            OnesCountProblemBinaryUIntSolution startSolution = (OnesCountProblemBinaryUIntSolution)solution.Clone(); object bestRep = null;
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
                in_loop = indexes.progress();
            }
            if (bestRep is not null)
            {
                solution.Representation = bestRep;
                solution.ObjectiveValue = bestTuple.objectiveValue;
                solution.FitnessValue = bestTuple.fitnessValue;
                solution.IsFeasible = bestTuple.isFeasible;
                return true;
            }
            solution.CopyFrom(startSolution);
            return false;
        }

        /// 
        /// Executes "first improvement" variant of the local search procedure 
        /// 
        /// :param int k: int parameter for VNS
        /// :param `OnesCountProblem` problem: problem that is solved
        /// :param `OnesCountProblemBinaryUIntSolution` solution: solution used for the problem that is solved
        /// :param `Algorithm` optimizer: optimizer that is executed
        /// :return: result of the local search procedure 
        /// return type OnesCountProblemBinaryUIntSolution
        /// 
        public bool LocalSearchFirstImprovement(int k, TargetProblem problem, TargetSolution<uint, string> solution,       Metaheuristic<uint, string> optimizer)
        {
            if (optimizer.FinishControl.CheckEvaluations && optimizer.Evaluation > optimizer.FinishControl.EvaluationsMax)
            {
                return false;
            }
            if (k < 1 || k > problem.dimension)
            {
                return false;
            }
            OnesCountProblemBinaryBitArraySolution startSolution = (OnesCountProblemBinaryBitArraySolution)solution.Clone();
            var bestTuple = QualityOfSolution(solution.ObjectiveValue, solution.FitnessValue, solution.IsFeasible);
            /// initialize indexes
            var indexes = ComplexCounterUniformAscending(k, problem.dimension);
            var in_loop = indexes.reset();
            while (in_loop)
            {
                /// collect positions for inversion from indexes
                var positions = indexes.current_state();
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
                    solution.CopyFrom(startSolution);
                    return false;
                }
                optimizer.WriteOutputValuesIfNeeded("beforeEvaluation", "b_e");
                var newTuple = solution.CalculateQuality(problem);
                optimizer.WriteOutputValuesIfNeeded("afterEvaluation", "a_e");
                if (QualityOfSolution.IsFirstFitnessBetter(newTuple, bestTuple, problem.IsMinimization) == true)
                {
                    solution.FitnessValue = newTuple.FitnessValue;
                    solution.ObjectiveValue = newTuple.ObjectiveValue;
                    solution.IsFeasible = newTuple.IsFeasible;
                    return true;
                }
                solution.Representation ^= mask;
                /// increment indexes and set in_loop accordingly
                in_loop = indexes.progress();
            }
            solution.CopyFrom(startSolution);
            return true;
        }

        /// 
        /// String representation of the vns support instance
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
        /// :return: string representation of vns support instance
        /// return type str
        /// 
        public new string StringRep(
            string delimiter,
            int indentation = 0,
            string indentationSymbol = "",
            string groupStart = "{",
            string groupEnd = "}") => "OnesCountProblemBinaryUIntSolutionVnsSupport";

        /// 
        /// String representation of the vns support instance
        /// 
        /// :return: string representation of the vns support instance
        /// return type str
        /// 
        public override string ToString() => this.StringRep("|");

        /// 
        /// Representation of the vns support instance
        /// 
        /// :return: string representation of the vns support instance
        /// return type str
        /// 
        public virtual string _repr__() => this.StringRep("\n");

        /// 
        /// Formatted the vns support instance
        /// 
        /// :param str spec: format specification
        /// :return: formatted vns support instance
        /// return type str
        /// 
        public virtual string _format__(string spec) => this.StringRep("|");

    }
}

