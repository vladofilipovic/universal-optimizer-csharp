namespace single_objective.teaching.function_one_variable_problem {
    
    using sys;
    
    using Path = pathlib.Path;
    
    using deepcopy = copy.deepcopy;
    
    using choice = random.choice;
    
    using randint = random.randint;
    
    using logger = uo.utils.logger.logger;
    
    using ComplexCounterUniformAscending = uo.utils.complex_counter_uniform_distinct.ComplexCounterUniformAscending;
    
    using QualityOfSolution = uo.target_solution.target_solution.QualityOfSolution;
    
    using Algorithm = uo.algorithm.algorithm.Algorithm;
    
    using ProblemSolutionVnsSupport = uo.algorithm.metaheuristic.variable_neighborhood_search.problem_solution_vns_support.ProblemSolutionVnsSupport;
    
    using FunctionOneVariableProblem = opt.single_objective.teaching.function_one_variable_problem.function_one_variable_problem.FunctionOneVariableProblem;
    
    using FunctionOneVariableProblemBinaryIntSolution = opt.single_objective.teaching.function_one_variable_problem.function_one_variable_problem_binary_int_solution.FunctionOneVariableProblemBinaryIntSolution;
    
    using System.Collections.Generic;
    
    using System;
    
    using System.Linq;
    
    public static class function_one_variable_problem_binary_int_solution_vns_support {
        
        public static object directory = Path(@__file__).resolve();
        
        static function_one_variable_problem_binary_int_solution_vns_support() {
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent);
            sys.path.append(directory.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent.parent);
        }
        
        public class FunctionOneVariableProblemBinaryIntSolutionVnsSupport
            : ProblemSolutionVnsSupport[intfloat] {
            
            public FunctionOneVariableProblemBinaryIntSolutionVnsSupport() {
                return;
            }
            
            public virtual void @__copy__() {
                var sup = deepcopy(this);
                return sup;
            }
            
            public virtual object copy() {
                return this.@__copy__();
            }
            
            public virtual bool shaking(int k, object problem, object solution, object optimizer) {
                if (optimizer.finish_control.evaluations_max > 0 && optimizer.evaluation > optimizer.finish_control.evaluations_max) {
                    return false;
                }
                var tries = 0;
                var limit = 10000;
                var representation_length = 32;
                while (tries < limit) {
                    var positions = new List<object>();
                    foreach (var i in Enumerable.Range(0, k - 0)) {
                        positions.append(choice(Enumerable.Range(0, representation_length)));
                    }
                    var mask = 0;
                    foreach (var p in positions) {
                        mask |= 1 << p;
                    }
                    solution.representation ^= mask;
                    var all_ok = true;
                    if (solution.representation.bit_count() > representation_length) {
                        all_ok = false;
                    }
                    if (all_ok) {
                        break;
                    }
                }
                if (tries < limit) {
                    optimizer.evaluation += 1;
                    if (optimizer.finish_control.evaluations_max > 0 && optimizer.evaluation > optimizer.finish_control.evaluations_max) {
                        return solution;
                    }
                    optimizer.write_output_values_if_needed("before_evaluation", "b_e");
                    solution.evaluate(problem);
                    optimizer.write_output_values_if_needed("after_evaluation", "a_e");
                    return true;
                } else {
                    return false;
                }
            }
            
            public virtual object local_search_best_improvement(int k, object problem, object solution, object optimizer) {
                var representation_length = 32;
                if (optimizer.finish_control.evaluations_max > 0 && optimizer.evaluation > optimizer.finish_control.evaluations_max) {
                    return solution;
                }
                if (k < 1 || k > representation_length) {
                    return solution;
                }
                object best_rep = null;
                var best_triplet = QualityOfSolution(solution.objective_value, solution.fitness_value, solution.is_feasible);
                // initialize indexes
                var indexes = ComplexCounterUniformAscending(k, representation_length);
                var in_loop = indexes.reset();
                while (in_loop) {
                    // collect positions for inversion from indexes
                    var positions = indexes.current_state();
                    // invert and compare, switch of new is better
                    var mask = 0;
                    foreach (var i in positions) {
                        mask |= 1 << i;
                    }
                    solution.representation ^= mask;
                    optimizer.evaluation += 1;
                    if (optimizer.finish_control.evaluations_max > 0 && optimizer.evaluation > optimizer.finish_control.evaluations_max) {
                        return solution;
                    }
                    optimizer.write_output_values_if_needed("before_evaluation", "b_e");
                    var new_triplet = solution.calculate_quality(problem);
                    optimizer.write_output_values_if_needed("after_evaluation", "a_e");
                    if (new_triplet.fitness_value > best_triplet.fitness_value) {
                        best_triplet = new_triplet;
                        best_rep = solution.representation;
                    }
                    solution.representation ^= mask;
                    // increment indexes and set in_loop accordingly
                    in_loop = indexes.progress();
                }
                if (best_rep is not null) {
                    solution.representation = best_rep;
                    solution.objective_value = best_triplet.objective_value;
                    solution.fitness_value = best_triplet.fitness_value;
                    solution.is_feasible = best_triplet.is_feasible;
                    return solution;
                }
                return solution;
            }
            
            public virtual object local_search_first_improvement(int k, object problem, object solution, object optimizer) {
                var representation_length = 32;
                if (optimizer.finish_control.evaluations_max > 0 && optimizer.evaluation > optimizer.finish_control.evaluations_max) {
                    return solution;
                }
                if (k < 1 || k > representation_length) {
                    return solution;
                }
                var best_fv = solution.fitness_value;
                // initialize indexes
                var indexes = ComplexCounterUniformAscending(k, representation_length);
                var in_loop = indexes.reset();
                while (in_loop) {
                    // collect positions for inversion from indexes
                    var positions = indexes.current_state();
                    // invert and compare, switch and exit if new is better
                    var mask = 0;
                    foreach (var i in positions) {
                        mask |= 1 << i;
                    }
                    solution.representation ^= mask;
                    optimizer.evaluation += 1;
                    if (optimizer.finish_control.evaluations_max > 0 && optimizer.evaluation > optimizer.finish_control.evaluations_max) {
                        return solution;
                    }
                    optimizer.write_output_values_if_needed("before_evaluation", "b_e");
                    var new_triplet = solution.calculate_quality(problem);
                    optimizer.write_output_values_if_needed("after_evaluation", "a_e");
                    if (new_triplet.fitness_value > best_fv) {
                        solution.fitness_value = new_triplet.fitness_value;
                        solution.objective_value = new_triplet.objective_value;
                        solution.is_feasible = new_triplet.is_feasible;
                        return solution;
                    }
                    solution.representation ^= mask;
                    // increment indexes and set in_loop accordingly
                    in_loop = indexes.progress();
                }
                return solution;
            }
            
            public virtual string string_rep(
                string delimiter,
                int indentation = 0,
                string indentation_symbol = "",
                string group_start = "{",
                string group_end = "}") {
                return "FunctionOneVariableProblemBinaryIntSolutionVnsSupport";
            }
            
            public override string ToString() {
                return this.string_rep("|");
            }
            
            public virtual string @__repr__() {
                return this.string_rep("\n");
            }
            
            public virtual string @__format__(string spec) {
                return this.string_rep("|");
            }
        }
    }
}
