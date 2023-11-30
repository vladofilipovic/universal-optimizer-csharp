namespace UniversalOptimizer.opt.SingleObjective.Teaching
{

    using sys;

    using Path = pathlib.Path;

    using deepcopy = copy.deepcopy;

    using choice = random.choice;

    using randint = random.randint;

    using logger = uo.utils.logger.logger;

    using ComplexCounterUniformAscending = uo.utils.complex_counter_uniform_distinct.ComplexCounterUniformAscending;

    using QualityOfSolution = uo.TargetSolution.TargetSolution.QualityOfSolution;

    using Algorithm = uo.Algorithm.algorithm.Algorithm;

    using ProblemSolutionVnsSupport = uo.Algorithm.metaheuristic.variable_neighborhood_search.problem_solution_vns_support.ProblemSolutionVnsSupport;

    using FunctionOneVariableProblem = teaching.FunctionOneVariableProblem.FunctionOneVariableProblem.FunctionOneVariableProblem;

    using FunctionOneVariableProblemBinaryIntSolution = teaching.FunctionOneVariableProblem.FunctionOneVariableProblemBinaryIntSolution.FunctionOneVariableProblemBinaryIntSolution;

    using System.Collections.Generic;

    using System;

    using System.Linq;

    public static class FunctionOneVariableProblemBinaryIntSolution_vns_support
    {

        public static object directory = Path(_file__).resolve();

        static FunctionOneVariableProblemBinaryIntSolution_vns_support()
        {
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent);
            sys.path.append(directory.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent.parent);
        }

        public class FunctionOneVariableProblemBinaryIntSolutionVnsSupport
            : ProblemSolutionVnsSupport[intfloat]
        {

            public FunctionOneVariableProblemBinaryIntSolutionVnsSupport()
            {
                return;
            }

            public virtual void _copy__()
            {
                var sup = deepcopy(this);
                return sup;
            }

            public virtual object copy()
            {
                return _copy__();
            }

            public virtual bool shaking(int k, object problem, object solution, object optimizer)
            {
                if (optimizer.finish_control.evaluationsMax > 0 && optimizer.evaluation > optimizer.finish_control.evaluationsMax)
                {
                    return false;
                }
                var tries = 0;
                var limit = 10000;
                var representation_length = 32;
                while (tries < limit)
                {
                    var positions = new List<object>();
                    foreach (var i in Enumerable.Range(0, k - 0))
                    {
                        positions.append(choice(Enumerable.Range(0, representation_length)));
                    }
                    var mask = 0;
                    foreach (var p in positions)
                    {
                        mask |= 1 << p;
                    }
                    solution.representation ^= mask;
                    var all_ok = true;
                    if (solution.representation.bit_count() > representation_length)
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
                    optimizer.evaluation += 1;
                    if (optimizer.finish_control.evaluationsMax > 0 && optimizer.evaluation > optimizer.finish_control.evaluationsMax)
                    {
                        return solution;
                    }
                    optimizer.WriteOutputValuesIfNeeded("beforeEvaluation", "b_e");
                    solution.evaluate(problem);
                    optimizer.WriteOutputValuesIfNeeded("afterEvaluation", "a_e");
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public virtual object local_search_best_improvement(int k, object problem, object solution, object optimizer)
            {
                var representation_length = 32;
                if (optimizer.finish_control.evaluationsMax > 0 && optimizer.evaluation > optimizer.finish_control.evaluationsMax)
                {
                    return solution;
                }
                if (k < 1 || k > representation_length)
                {
                    return solution;
                }
                object best_rep = null;
                var best_triplet = QualityOfSolution(solution.objectiveValue, solution.fitnessValue, solution.isFeasible);
                /// initialize indexes
                var indexes = ComplexCounterUniformAscending(k, representation_length);
                var in_loop = indexes.reset();
                while (in_loop)
                {
                    /// collect positions for inversion from indexes
                    var positions = indexes.current_state();
                    /// invert and compare, switch of new is better
                    var mask = 0;
                    foreach (var i in positions)
                    {
                        mask |= 1 << i;
                    }
                    solution.representation ^= mask;
                    optimizer.evaluation += 1;
                    if (optimizer.finish_control.evaluationsMax > 0 && optimizer.evaluation > optimizer.finish_control.evaluationsMax)
                    {
                        return solution;
                    }
                    optimizer.WriteOutputValuesIfNeeded("beforeEvaluation", "b_e");
                    var new_triplet = solution.CalculateQuality(problem);
                    optimizer.WriteOutputValuesIfNeeded("afterEvaluation", "a_e");
                    if (new_triplet.fitnessValue > best_triplet.fitnessValue)
                    {
                        best_triplet = new_triplet;
                        best_rep = solution.representation;
                    }
                    solution.representation ^= mask;
                    /// increment indexes and set in_loop accordingly
                    in_loop = indexes.progress();
                }
                if (best_rep is not null)
                {
                    solution.representation = best_rep;
                    solution.objectiveValue = best_triplet.objectiveValue;
                    solution.fitnessValue = best_triplet.fitnessValue;
                    solution.isFeasible = best_triplet.isFeasible;
                    return solution;
                }
                return solution;
            }

            public virtual object local_search_first_improvement(int k, object problem, object solution, object optimizer)
            {
                var representation_length = 32;
                if (optimizer.finish_control.evaluationsMax > 0 && optimizer.evaluation > optimizer.finish_control.evaluationsMax)
                {
                    return solution;
                }
                if (k < 1 || k > representation_length)
                {
                    return solution;
                }
                var best_fv = solution.fitnessValue;
                /// initialize indexes
                var indexes = ComplexCounterUniformAscending(k, representation_length);
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
                    solution.representation ^= mask;
                    optimizer.evaluation += 1;
                    if (optimizer.finish_control.evaluationsMax > 0 && optimizer.evaluation > optimizer.finish_control.evaluationsMax)
                    {
                        return solution;
                    }
                    optimizer.WriteOutputValuesIfNeeded("beforeEvaluation", "b_e");
                    var new_triplet = solution.CalculateQuality(problem);
                    optimizer.WriteOutputValuesIfNeeded("afterEvaluation", "a_e");
                    if (new_triplet.fitnessValue > best_fv)
                    {
                        solution.fitnessValue = new_triplet.fitnessValue;
                        solution.objectiveValue = new_triplet.objectiveValue;
                        solution.isFeasible = new_triplet.isFeasible;
                        return solution;
                    }
                    solution.representation ^= mask;
                    /// increment indexes and set in_loop accordingly
                    in_loop = indexes.progress();
                }
                return solution;
            }

            public new string StringRep(
                string delimiter,
                int indentation = 0,
                string indentationSymbol = "",
                string groupStart = "{",
                string groupEnd = "}")
            {
                return "FunctionOneVariableProblemBinaryIntSolutionVnsSupport";
            }

            public override string ToString()
            {
                return StringRep("|");
            }

            public virtual string _repr__()
            {
                return StringRep("\n");
            }

            public virtual string _format__(string spec)
            {
                return StringRep("|");
            }
        }
    }
}
