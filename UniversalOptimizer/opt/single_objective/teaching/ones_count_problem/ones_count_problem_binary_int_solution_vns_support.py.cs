//  
// .. _py_ones_count_problem_int_solution_vns_support:
// 
// The :mod:`~opt.single_objective.teaching.ones_count_problem.ones_count_problem_binary_int_solution_vns_support` contains 
// class :class:`~opt.single_objective.teaching.ones_count_problem.ones_count_problem_binary_int_solution_vns_support.OnesCountProblemBinaryIntSolutionVnsSupport`, 
// that represents solution of the :ref:`Problem_Max_Ones`, where `int` representation of the problem has been used.
// 
namespace single_objective.teaching.ones_count_problem {
    
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
    
    using OnesCountProblem = opt.single_objective.teaching.ones_count_problem.ones_count_problem.OnesCountProblem;
    
    using OnesCountProblemBinaryIntSolution = opt.single_objective.teaching.ones_count_problem.ones_count_problem_binary_int_solution.OnesCountProblemBinaryIntSolution;
    
    using System.Collections.Generic;
    
    using System;
    
    using System.Linq;
    
    public static class ones_count_problem_binary_int_solution_vns_support {
        
        public static object directory = Path(@__file__).resolve();
        
        static ones_count_problem_binary_int_solution_vns_support() {
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent);
            sys.path.append(directory.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent.parent);
        }
        
        public class OnesCountProblemBinaryIntSolutionVnsSupport
            : ProblemSolutionVnsSupport[intstr] {
            
            public OnesCountProblemBinaryIntSolutionVnsSupport() {
                return;
            }
            
            // 
            //         Internal copy of the `OnesCountProblemBinaryIntSolutionVnsSupport`
            // 
            //         :return: new `OnesCountProblemBinaryIntSolutionVnsSupport` instance with the same properties
            //         :rtype: OnesCountProblemBinaryIntSolutionVnsSupport
            //         
            public virtual void @__copy__() {
                var sup = deepcopy(this);
                return sup;
            }
            
            // 
            //         Copy the `OnesCountProblemBinaryIntSolutionVnsSupport`
            //         
            //         :return: new `OnesCountProblemBinaryIntSolutionVnsSupport` instance with the same properties
            //         :rtype: `OnesCountProblemBinaryIntSolutionVnsSupport`
            //         
            public virtual void copy() {
                return this.@__copy__();
            }
            
            // 
            //         Random VNS shaking of k parts such that new solution code does not differ more than k from all solution codes 
            //         inside shakingPoints 
            // 
            //         :param int k: int parameter for VNS
            //         :param `OnesCountProblem` problem: problem that is solved
            //         :param `OnesCountProblemBinaryIntSolution` solution: solution used for the problem that is solved
            //         :param `Algorithm` optimizer: optimizer that is executed
            //         :return: if shaking is successful
            //         :rtype: bool
            //         
            public virtual bool shaking(int k, object problem, object solution, object optimizer) {
                if (optimizer.finish_control.evaluations_max > 0 && optimizer.evaluation > optimizer.finish_control.evaluations_max) {
                    return false;
                }
                var tries = 0;
                var limit = 10000;
                while (tries < limit) {
                    var positions = new List<object>();
                    foreach (var i in Enumerable.Range(0, k - 0)) {
                        positions.append(choice(Enumerable.Range(0, problem.dimension)));
                    }
                    var mask = 0;
                    foreach (var p in positions) {
                        mask |= 1 << p;
                    }
                    solution.representation ^= mask;
                    var all_ok = true;
                    if (solution.representation.bit_count() > problem.dimension) {
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
            
            // 
            //         Executes "best improvement" variant of the local search procedure 
            //         
            //         :param int k: int parameter for VNS
            //         :param `OnesCountProblem` problem: problem that is solved
            //         :param `OnesCountProblemBinaryIntSolution` solution: solution used for the problem that is solved
            //         :param `Algorithm` optimizer: optimizer that is executed
            //         :return: result of the local search procedure 
            //         :rtype: OnesCountProblemBinaryIntSolution
            //         
            public virtual object local_search_best_improvement(int k, object problem, object solution, object optimizer) {
                if (optimizer.finish_control.evaluations_max > 0 && optimizer.evaluation > optimizer.finish_control.evaluations_max) {
                    return solution;
                }
                if (k < 1 || k > problem.dimension) {
                    return solution;
                }
                object best_rep = null;
                var best_triplet = QualityOfSolution(solution.objective_value, solution.fitness_value, solution.is_feasible);
                // initialize indexes
                var indexes = ComplexCounterUniformAscending(k, problem.dimension);
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
            
            // 
            //         Executes "first improvement" variant of the local search procedure 
            //         
            //         :param int k: int parameter for VNS
            //         :param `OnesCountProblem` problem: problem that is solved
            //         :param `OnesCountProblemBinaryIntSolution` solution: solution used for the problem that is solved
            //         :param `Algorithm` optimizer: optimizer that is executed
            //         :return: result of the local search procedure 
            //         :rtype: OnesCountProblemBinaryIntSolution
            //         
            public virtual object local_search_first_improvement(int k, object problem, object solution, object optimizer) {
                if (optimizer.finish_control.evaluations_max > 0 && optimizer.evaluation > optimizer.finish_control.evaluations_max) {
                    return solution;
                }
                if (k < 1 || k > problem.dimension) {
                    return solution;
                }
                var best_fv = solution.fitness_value;
                // initialize indexes
                var indexes = ComplexCounterUniformAscending(k, problem.dimension);
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
            
            // 
            //         String representation of the vns support instance
            // 
            //         :param delimiter: delimiter between fields
            //         :type delimiter: str
            //         :param indentation: level of indentation
            //         :type indentation: int, optional, default value 0
            //         :param indentation_symbol: indentation symbol
            //         :type indentation_symbol: str, optional, default value ''
            //         :param group_start: group start string 
            //         :type group_start: str, optional, default value '{'
            //         :param group_end: group end string 
            //         :type group_end: str, optional, default value '}'
            //         :return: string representation of vns support instance
            //         :rtype: str
            //         
            public virtual string string_rep(
                string delimiter,
                int indentation = 0,
                string indentation_symbol = "",
                string group_start = "{",
                string group_end = "}") {
                return "OnesCountProblemBinaryIntSolutionVnsSupport";
            }
            
            // 
            //         String representation of the vns support instance
            // 
            //         :return: string representation of the vns support instance
            //         :rtype: str
            //         
            public override string ToString() {
                return this.string_rep("|");
            }
            
            // 
            //         Representation of the vns support instance
            // 
            //         :return: string representation of the vns support instance
            //         :rtype: str
            //         
            public virtual string @__repr__() {
                return this.string_rep("\n");
            }
            
            // 
            //         Formatted the vns support instance
            // 
            //         :param str spec: format specification
            //         :return: formatted vns support instance
            //         :rtype: str
            //         
            public virtual string @__format__(string spec) {
                return this.string_rep("|");
            }
        }
    }
}
