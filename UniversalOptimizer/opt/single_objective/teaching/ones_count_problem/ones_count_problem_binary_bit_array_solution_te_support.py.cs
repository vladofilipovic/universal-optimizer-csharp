//  
// ..  _py_ones_count_problem_bit_array_solution_te_support:
// 
// The :mod:`~opt.single_objective.teaching.ones_count_problem.ones_count_problem_binary_bit_array_solution_te_support` 
// contains class :class:`~opt.single_objective.teaching.ones_count_problem.ones_count_problem_binary_bit_array_solution_te_support.OnesCountProblemBinaryBitArraySolutionTeSupport`, 
// that represents supporting parts of the `Total enumeration` algorithm, where solution of the :ref:`Problem_Max_Ones` have `BitArray` 
// representation.
// 
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
    
    using ComplexCounterBitArrayFull = uo.utils.complex_counter_bit_array_full.ComplexCounterBitArrayFull;
    
    using logger = uo.utils.logger.logger;
    
    using ComplexCounterUniformAscending = uo.utils.complex_counter_uniform_distinct.ComplexCounterUniformAscending;
    
    using QualityOfSolution = uo.target_solution.target_solution.QualityOfSolution;
    
    using Algorithm = uo.algorithm.algorithm.Algorithm;
    
    using ProblemSolutionTeSupport = uo.algorithm.exact.total_enumeration.problem_solution_te_support.ProblemSolutionTeSupport;
    
    using OnesCountProblem = opt.single_objective.teaching.ones_count_problem.ones_count_problem.OnesCountProblem;
    
    using OnesCountProblemBinaryBitArraySolution = opt.single_objective.teaching.ones_count_problem.ones_count_problem_binary_bit_array_solution.OnesCountProblemBinaryBitArraySolution;
    
    public static class ones_count_problem_binary_bit_array_solution_te_support {
        
        public static object directory = Path(@__file__).resolve();
        
        static ones_count_problem_binary_bit_array_solution_te_support() {
            sys.path.append(directory);
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent.parent);
        }
        
        public class OnesCountProblemBinaryBitArraySolutionTeSupport
            : ProblemSolutionTeSupport[BitArraystr] {
            
            public object @__bit_array_counter;
            
            public OnesCountProblemBinaryBitArraySolutionTeSupport() {
                this.@__bit_array_counter = null;
            }
            
            // 
            //         Internal copy of the `OnesCountProblemBinaryBitArraySolutionTeSupport`
            // 
            //         :return: new `OnesCountProblemBinaryBitArraySolutionTeSupport` instance with the same properties
            //         :rtype: `OnesCountProblemBinaryBitArraySolutionTeSupport`
            //         
            public virtual void @__copy__() {
                var sol = deepcopy(this);
                return sol;
            }
            
            // 
            //         Copy the `OnesCountProblemBinaryBitArraySolutionTeSupport` instance
            // 
            //         :return: new `OnesCountProblemBinaryBitArraySolutionTeSupport` instance with the same properties
            //         :rtype: `OnesCountProblemBinaryBitArraySolutionTeSupport`
            //         
            public virtual void copy() {
                return this.@__copy__();
            }
            
            // 
            //         Resets internal counter of the total enumerator, so process will start over. Internal state of the solution 
            //         will be set to reflect reset operation. 
            // 
            //         :param `OnesCountProblem` problem: problem that is solved
            //         :param `OnesCountProblemBinaryBitArraySolution` solution: solution used for the problem that is solved
            //         :param `Algorithm` optimizer: optimizer that is executed
            //         
            public virtual object reset(object problem, object solution, object optimizer) {
                this.@__bit_array_counter = ComplexCounterBitArrayFull(problem.dimension);
                this.@__bit_array_counter.reset();
                solution.init_from(this.@__bit_array_counter.current_state(), problem);
                optimizer.write_output_values_if_needed("before_evaluation", "b_e");
                optimizer.evaluation += 1;
                solution.evaluate(problem);
                optimizer.write_output_values_if_needed("after_evaluation", "a_e");
            }
            
            // 
            //         Progress internal counter of the total enumerator, so next configuration will be taken into consideration. 
            //         Internal state of the solution will be set to reflect progress operation.  
            // 
            //         :param `OnesCountProblem` problem: problem that is solved
            //         :param `OnesCountProblemBinaryBitArraySolution` solution: solution used for the problem that is solved
            //         :param `Algorithm` optimizer: optimizer that is executed
            //         
            public virtual object progress(object problem, object solution, object optimizer) {
                this.@__bit_array_counter.progress();
                solution.init_from(this.@__bit_array_counter.current_state(), problem);
                optimizer.write_output_values_if_needed("before_evaluation", "b_e");
                optimizer.evaluation += 1;
                solution.evaluate(problem);
                optimizer.write_output_values_if_needed("after_evaluation", "a_e");
            }
            
            // 
            //         Check if total enumeration process is not at end.  
            // 
            //         :param `OnesCountProblem` problem: problem that is solved
            //         :param `OnesCountProblemBinaryBitArraySolution` solution: solution used for the problem that is solved
            //         :param `Algorithm` optimizer: optimizer that is executed
            //         :return: indicator if total enumeration process is not at end 
            //         :rtype: bool
            //         
            public virtual bool can_progress(object problem, object solution, object optimizer) {
                return this.@__bit_array_counter.can_progress();
            }
            
            // 
            //         Returns overall number of evaluations required for finishing total enumeration process.  
            // 
            //         :param `OnesCountProblem` problem: problem that is solved
            //         :param `OnesCountProblemBinaryBitArraySolution` solution: solution used for the problem that is solved
            //         :param `Algorithm` optimizer: optimizer that is executed
            //         :return: overall number of evaluations required for finishing total enumeration process
            //         :rtype: int
            //         
            public virtual int overall_number_of_evaluations(object problem, object solution, object optimizer) {
                return pow(2, problem.dimension);
            }
            
            // 
            //         String representation of the te support structure
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
                return "OnesCountProblemBinaryBitArraySolutionTeSupport";
            }
            
            // 
            //         String representation of the te support instance
            // 
            //         :return: string representation of the te support instance
            //         :rtype: str
            //         
            public override string ToString() {
                return this.string_rep("|");
            }
            
            // 
            //         Representation of the te support instance
            // 
            //         :return: string representation of the te support instance
            //         :rtype: str
            //         
            public virtual string @__repr__() {
                return this.string_rep("\n");
            }
            
            // 
            //         Formatted the te support instance
            // 
            //         :param str spec: format specification
            //         :return: formatted te support instance
            //         :rtype: str
            //         
            public virtual string @__format__(string spec) {
                return this.string_rep("|");
            }
        }
    }
}
