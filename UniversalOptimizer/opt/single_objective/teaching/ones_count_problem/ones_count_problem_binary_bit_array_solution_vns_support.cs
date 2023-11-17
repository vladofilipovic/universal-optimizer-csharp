///  
/// ..  _py_ones_count_problem_bit_array_solution_vns_support:
/// 
/// The :mod:`~opt.single_objective.teaching.ones_count_problem.ones_count_problem_binary_bit_array_solution_vns_support` 
/// contains class :class:`~opt.single_objective.teaching.ones_count_problem.ones_count_problem_binary_bit_array_solution_vns_support.OnesCountProblemBinaryBitArraySolutionVnsSupport`, 
/// that represents supporting parts of the `VNS` algorithm, where solution of the :ref:`Problem_Max_Ones` have `BitArray` 
/// representation.
/// 
namespace single_objective.teaching.ones_count_problem {
    
    using sys;
    
    using Path = pathlib.Path;
    
    using deepcopy = copy.deepcopy;
    
    using choice = random.choice;
    
    using random = random.random;
    
    using Bits = bitstring.Bits;
    
    using BitArray = bitstring.BitArray;
    
    using BitStream = bitstring.BitStream;
    
    using pack = bitstring.pack;
    
    using logger = uo.utils.logger.logger;
    
    using ComplexCounterUniformAscending = uo.utils.complex_counter_uniform_distinct.ComplexCounterUniformAscending;
    
    using QualityOfSolution = uo.TargetSolution.TargetSolution.QualityOfSolution;
    
    using Algorithm = uo.Algorithm.algorithm.Algorithm;
    
    using ProblemSolutionVnsSupport = uo.Algorithm.metaheuristic.variable_neighborhood_search.problem_solution_vns_support.ProblemSolutionVnsSupport;
    
    using OnesCountProblem = opt.single_objective.teaching.ones_count_problem.ones_count_problem.OnesCountProblem;
    
    using OnesCountProblemBinaryBitArraySolution = opt.single_objective.teaching.ones_count_problem.ones_count_problem_binary_bit_array_solution.OnesCountProblemBinaryBitArraySolution;
    
    using System.Collections.Generic;
    
    using System;
    
    using System.Linq;
    
    public static class ones_count_problem_binary_bit_array_solution_vns_support {
        
        public static object directory = Path(_file__).resolve();
        
        static ones_count_problem_binary_bit_array_solution_vns_support() {
            sys.path.append(directory);
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent.parent);
        }
        
        public class OnesCountProblemBinaryBitArraySolutionVnsSupport
            : ProblemSolutionVnsSupport[BitArraystr] {
            
            public OnesCountProblemBinaryBitArraySolutionVnsSupport() {
                return;
            }
            
            /// 
            ///         Internal copy of the `OnesCountProblemBinaryBitArraySolutionVnsSupport`
            /// 
            ///         :return: new `OnesCountProblemBinaryBitArraySolutionVnsSupport` instance with the same properties
            ///         return type `OnesCountProblemBinaryBitArraySolutionVnsSupport`
            ///         
            public virtual void _copy__() {
                var sol = deepcopy(this);
                return sol;
            }
            
            /// 
            ///         Copy the `OnesCountProblemBinaryBitArraySolutionVnsSupport` instance
            /// 
            ///         :return: new `OnesCountProblemBinaryBitArraySolutionVnsSupport` instance with the same properties
            ///         return type `OnesCountProblemBinaryBitArraySolutionVnsSupport`
            ///         
            public virtual void copy() {
                return _copy__();
            }
            
            /// 
            ///         Random shaking of k parts such that new solution code does not differ more than k from all solution codes 
            ///         inside shakingPoints 
            /// 
            ///         :param int k: int parameter for VNS
            ///         :param `OnesCountProblem` problem: problem that is solved
            ///         :param `OnesCountProblemBinaryBitArraySolution` solution: solution used for the problem that is solved
            ///         :param `Algorithm` optimizer: optimizer that is executed
            ///         :return: if randomization is successful
            ///         return type bool
            ///         
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
                    var repr = BitArray(solution.representation.tobytes());
                    foreach (var pos in positions) {
                        repr[pos] = !repr[pos];
                    }
                    solution.representation = repr;
                    var all_ok = true;
                    if (solution.representation.count(value: 1) > problem.dimension) {
                        all_ok = false;
                    }
                    if (all_ok) {
                        break;
                    }
                }
                if (tries < limit) {
                    optimizer.evaluation += 1;
                    if (optimizer.finish_control.evaluations_max > 0 && optimizer.evaluation > optimizer.finish_control.evaluations_max) {
                        return false;
                    }
                    optimizer.write_outputValues_if_needed("before_evaluation", "b_e");
                    solution.evaluate(problem);
                    optimizer.write_outputValues_if_needed("after_evaluation", "a_e");
                    optimizer.write_outputValues_if_needed("after_step_in_iteration", "shaking");
                    return true;
                } else {
                    return false;
                }
            }
            
            /// 
            ///         Executes "best improvement" variant of the local search procedure 
            ///         
            ///         :param int k: int parameter for VNS
            ///         :param `OnesCountProblem` problem: problem that is solved
            ///         :param `OnesCountProblemBinaryBitArraySolution` solution: solution used for the problem that is solved
            ///         :param `Algorithm` optimizer: optimizer that is executed
            ///         :return: result of the local search procedure 
            ///         return type OnesCountProblemBinaryBitArraySolution
            ///         
            public virtual object local_search_best_improvement(int k, object problem, object solution, object optimizer) {
                if (optimizer.finish_control.evaluations_max > 0 && optimizer.evaluation > optimizer.finish_control.evaluations_max) {
                    return solution;
                }
                if (k < 1 || k > problem.dimension) {
                    return solution;
                }
                object best_rep = null;
                var best_triplet = QualityOfSolution(solution.objectiveValue, solution.fitnessValue, solution.isFeasible);
                /// initialize indexes
                var indexes = ComplexCounterUniformAscending(k, problem.dimension);
                var in_loop = indexes.reset();
                while (in_loop) {
                    /// collect positions for inversion from indexes
                    var positions = indexes.current_state();
                    /// invert and compare, switch of new is better
                    solution.representation.invert(positions);
                    optimizer.evaluation += 1;
                    if (optimizer.finish_control.evaluations_max > 0 && optimizer.evaluation > optimizer.finish_control.evaluations_max) {
                        return solution;
                    }
                    optimizer.write_outputValues_if_needed("before_evaluation", "b_e");
                    var new_triplet = solution.CalculateQuality(problem);
                    optimizer.write_outputValues_if_needed("after_evaluation", "a_e");
                    if (new_triplet.fitnessValue > best_triplet.fitnessValue) {
                        best_triplet = new_triplet;
                        best_rep = BitArray(bin: solution.representation.bin);
                    }
                    solution.representation.invert(positions);
                    /// increment indexes and set in_loop according to the state
                    in_loop = indexes.progress();
                }
                if (best_rep is not null) {
                    solution.representation = best_rep;
                    solution.objectiveValue = best_triplet.objectiveValue;
                    solution.fitnessValue = best_triplet.fitnessValue;
                    solution.isFeasible = best_triplet.isFeasible;
                    return solution;
                }
                return solution;
            }
            
            /// 
            ///         Executes "first improvement" variant of the local search procedure 
            ///         
            ///         :param int k: int parameter for VNS
            ///         :param `OnesCountProblem` problem: problem that is solved
            ///         :param `OnesCountProblemBinaryBitArraySolution` solution: solution used for the problem that is solved
            ///         :param `Algorithm` optimizer: optimizer that is executed
            ///         :return: result of the local search procedure 
            ///         return type OnesCountProblemBinaryBitArraySolution
            ///         
            public virtual object local_search_first_improvement(int k, object problem, object solution, object optimizer) {
                if (optimizer.finish_control.evaluations_max > 0 && optimizer.evaluation > optimizer.finish_control.evaluations_max) {
                    return solution;
                }
                if (k < 1 || k > problem.dimension) {
                    return solution;
                }
                var best_fv = solution.fitnessValue;
                /// initialize indexes
                var indexes = ComplexCounterUniformAscending(k, problem.dimension);
                var in_loop = indexes.reset();
                while (in_loop) {
                    /// collect positions for inversion from indexes
                    var positions = indexes.current_state();
                    /// invert and compare, switch and exit if new is better
                    solution.representation.invert(positions);
                    optimizer.evaluation += 1;
                    if (optimizer.finish_control.evaluations_max > 0 && optimizer.evaluation > optimizer.finish_control.evaluations_max) {
                        return solution;
                    }
                    optimizer.write_outputValues_if_needed("before_evaluation", "b_e");
                    var new_triplet = solution.CalculateQuality(problem);
                    optimizer.write_outputValues_if_needed("after_evaluation", "a_e");
                    if (new_triplet.fitnessValue > best_fv) {
                        solution.objectiveValue = new_triplet.objectiveValue;
                        solution.fitnessValue = new_triplet.fitnessValue;
                        solution.isFeasible = new_triplet.isFeasible;
                        return solution;
                    }
                    solution.representation.invert(positions);
                    /// increment indexes and set in_loop accordingly
                    in_loop = indexes.progress();
                }
                return solution;
            }
            
            /// 
            ///         String representation of the vns support structure
            /// 
            ///         :param delimiter: delimiter between fields
            ///         :type delimiter: str
            ///         :param indentation: level of indentation
            ///         :type indentation: int, optional, default value 0
            ///         :param indentationSymbol: indentation symbol
            ///         :type indentationSymbol: str, optional, default value ''
            ///         :param groupStart: group start string 
            ///         :type groupStart: str, optional, default value '{'
            ///         :param groupEnd: group end string 
            ///         :type groupEnd: str, optional, default value '}'
            ///         :return: string representation of vns support instance
            ///         return type str
            ///         
            public virtual string StringRep(
                string delimiter,
                int indentation = 0,
                string indentationSymbol = "",
                string groupStart = "{",
                string groupEnd = "}") {
                return "OnesCountProblemBinaryBitArraySolutionVnsSupport";
            }
            
            /// 
            ///         String representation of the vns support instance
            /// 
            ///         :return: string representation of the vns support instance
            ///         return type str
            ///         
            public override string ToString() {
                return this.StringRep("|");
            }
            
            /// 
            ///         Representation of the vns support instance
            /// 
            ///         :return: string representation of the vns support instance
            ///         return type str
            ///         
            public virtual string _repr__() {
                return this.StringRep("\n");
            }
            
            /// 
            ///         Formatted the vns support instance
            /// 
            ///         :param str spec: format specification
            ///         :return: formatted vns support instance
            ///         return type str
            ///         
            public virtual string _format__(string spec) {
                return this.StringRep("|");
            }
        }
    }
}
