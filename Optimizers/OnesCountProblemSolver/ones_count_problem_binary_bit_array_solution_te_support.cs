///  
/// ..  _py_ones_count_problem_bit_array_solution_te_support:
/// 
/// The :mod:`~opt.SingleObjective.Teaching.ones_count_problem.ones_count_problem_binary_bit_array_solution_te_support` 
/// contains class :class:`~opt.SingleObjective.Teaching.ones_count_problem.ones_count_problem_binary_bit_array_solution_te_support.OnesCountProblemBinaryBitArraySolutionTeSupport`, 
/// that represents supporting parts of the `Total enumeration` algorithm, where solution of the :ref:`Problem_Max_Ones` have `BitArray` 
/// representation.
/// 
namespace SingleObjective.Teaching.ones_count_problem {
    
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
    
    using QualityOfSolution = uo.TargetSolution.TargetSolution.QualityOfSolution;
    
    using Algorithm = uo.Algorithm.algorithm.Algorithm;
    
    using ProblemSolutionTeSupport = uo.Algorithm.Exact.TotalEnumeration.problemSolution_te_support.ProblemSolutionTeSupport;
    
    using OnesCountProblem = opt.SingleObjective.Teaching.ones_count_problem.ones_count_problem.OnesCountProblem;
    
    using OnesCountProblemBinaryBitArraySolution = opt.SingleObjective.Teaching.ones_count_problem.ones_count_problem_binary_bit_array_solution.OnesCountProblemBinaryBitArraySolution;
    
    public static class ones_count_problem_binary_bit_array_solution_te_support {
        
        public static object directory = Path(_file__).resolve();
        
        static ones_count_problem_binary_bit_array_solution_te_support() {
            sys.path.append(directory);
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent.parent);
        }
        
        public class OnesCountProblemBinaryBitArraySolutionTeSupport
            : ProblemSolutionTeSupport[BitArraystr] {
            
            private object _bit_array_counter;
            
            public OnesCountProblemBinaryBitArraySolutionTeSupport() {
                _bit_array_counter = null;
            }
            
            /// 
            /// Internal copy of the `OnesCountProblemBinaryBitArraySolutionTeSupport`
            /// 
            /// :return: new `OnesCountProblemBinaryBitArraySolutionTeSupport` instance with the same properties
            /// return type `OnesCountProblemBinaryBitArraySolutionTeSupport`
            /// 
            public virtual void _copy__() {
                var sol = deepcopy(this);
                return sol;
            }
            
            /// 
            /// Copy the `OnesCountProblemBinaryBitArraySolutionTeSupport` instance
            /// 
            /// :return: new `OnesCountProblemBinaryBitArraySolutionTeSupport` instance with the same properties
            /// return type `OnesCountProblemBinaryBitArraySolutionTeSupport`
            /// 
            public virtual void copy() {
                return _copy__();
            }
            
            /// 
            /// Resets internal counter of the total enumerator, so process will start over. Internal state of the solution 
            /// will be set to reflect reset operation. 
            /// 
            /// :param `OnesCountProblem` problem: problem that is solved
            /// :param `OnesCountProblemBinaryBitArraySolution` solution: solution used for the problem that is solved
            /// :param `Algorithm` optimizer: optimizer that is executed
            /// 
            public virtual object reset(object problem, object solution, object optimizer) {
                _bit_array_counter = ComplexCounterBitArrayFull(problem.dimension);
                _bit_array_counter.reset();
                solution.InitFrom(_bit_array_counter.current_state(), problem);
                optimizer.WriteOutputValuesIfNeeded("beforeEvaluation", "b_e");
                optimizer.evaluation += 1;
                solution.evaluate(problem);
                optimizer.WriteOutputValuesIfNeeded("afterEvaluation", "a_e");
            }
            
            /// 
            /// Progress internal counter of the total enumerator, so next configuration will be taken into consideration. 
            /// Internal state of the solution will be set to reflect progress operation.  
            /// 
            /// :param `OnesCountProblem` problem: problem that is solved
            /// :param `OnesCountProblemBinaryBitArraySolution` solution: solution used for the problem that is solved
            /// :param `Algorithm` optimizer: optimizer that is executed
            /// 
            public virtual object progress(object problem, object solution, object optimizer) {
                _bit_array_counter.progress();
                solution.InitFrom(_bit_array_counter.current_state(), problem);
                optimizer.WriteOutputValuesIfNeeded("beforeEvaluation", "b_e");
                optimizer.evaluation += 1;
                solution.evaluate(problem);
                optimizer.WriteOutputValuesIfNeeded("afterEvaluation", "a_e");
            }
            
            /// 
            /// Check if total enumeration process is not at end.  
            /// 
            /// :param `OnesCountProblem` problem: problem that is solved
            /// :param `OnesCountProblemBinaryBitArraySolution` solution: solution used for the problem that is solved
            /// :param `Algorithm` optimizer: optimizer that is executed
            /// :return: indicator if total enumeration process is not at end 
            /// return type bool
            /// 
            public virtual bool can_progress(object problem, object solution, object optimizer) {
                return _bit_array_counter.can_progress();
            }
            
            /// 
            /// Returns overall number of evaluations required for finishing total enumeration process.  
            /// 
            /// :param `OnesCountProblem` problem: problem that is solved
            /// :param `OnesCountProblemBinaryBitArraySolution` solution: solution used for the problem that is solved
            /// :param `Algorithm` optimizer: optimizer that is executed
            /// :return: overall number of evaluations required for finishing total enumeration process
            /// return type int
            /// 
            public virtual int OverallNumberOfEvaluations(object problem, object solution, object optimizer) {
                return pow(2, problem.dimension);
            }
            
            /// 
            /// String representation of the te support structure
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
                string groupEnd = "}") {
                return "OnesCountProblemBinaryBitArraySolutionTeSupport";
            }
            
            /// 
            /// String representation of the te support instance
            /// 
            /// :return: string representation of the te support instance
            /// return type str
            /// 
            public override string ToString() {
                return this.StringRep("|");
            }
            
            /// 
            /// Representation of the te support instance
            /// 
            /// :return: string representation of the te support instance
            /// return type str
            /// 
            public virtual string _repr__() {
                return this.StringRep("\n");
            }
            
            /// 
            /// Formatted the te support instance
            /// 
            /// :param str spec: format specification
            /// :return: formatted te support instance
            /// return type str
            /// 
            public virtual string _format__(string spec) {
                return this.StringRep("|");
            }
        }
    }
}
