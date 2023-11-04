//  
// ..  _py_ones_count_problem_bit_array_solution:
// 
// The :mod:`~opt.single_objective.teaching.ones_count_problem.ones_count_problem_binary_bit_array_solution` contains class :class:`~opt.single_objective.teaching.ones_count_problem.ones_count_problem_binary_bit_array_solution.OnesCountProblemBinaryBitArraySolution`, that represents solution of the :ref:`Problem_Max_Ones`, where `BitArray` representation of the problem has been used.
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
    
    using TargetProblem = uo.target_problem.target_problem.TargetProblem;
    
    using QualityOfSolution = uo.target_solution.target_solution.QualityOfSolution;
    
    using TargetSolution = uo.target_solution.target_solution.TargetSolution;
    
    using logger = uo.utils.logger.logger;
    
    using System;
    
    using System.Linq;
    
    public static class ones_count_problem_binary_bit_array_solution {
        
        public static object directory = Path(@__file__).resolve();
        
        static ones_count_problem_binary_bit_array_solution() {
            sys.path.append(directory);
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent.parent);
        }
        
        public class OnesCountProblemBinaryBitArraySolution
            : TargetSolution[BitArraystr] {
            
            public object representation;
            
            public OnesCountProblemBinaryBitArraySolution(
                int random_seed = null,
                bool evaluation_cache_is_used = false,
                int evaluation_cache_max_size = 0,
                bool distance_calculation_cache_is_used = false,
                int distance_calculation_cache_max_size = 0)
                : base(random_seed: random_seed, fitness_value: null, objective_value: null, is_feasible: false, evaluation_cache_is_used: evaluation_cache_is_used, evaluation_cache_max_size: evaluation_cache_max_size, distance_calculation_cache_is_used: distance_calculation_cache_is_used, distance_calculation_cache_max_size: distance_calculation_cache_max_size) {
            }
            
            // 
            //         Internal copy of the `OnesCountProblemBinaryBitArraySolution`
            // 
            //         :return: new `OnesCountProblemBinaryBitArraySolution` instance with the same properties
            //         :rtype: OnesCountProblemBinaryBitArraySolution
            //         
            public virtual void @__copy__() {
                var sol = base.@__copy__();
                if (this.representation is not null) {
                    sol.representation = BitArray(bin: this.representation.bin);
                } else {
                    sol.representation = null;
                }
                return sol;
            }
            
            // 
            //         Copy the `OnesCountProblemBinaryBitArraySolution`
            //         
            //         :return: new `OnesCountProblemBinaryBitArraySolution` instance with the same properties
            //         :rtype: `OnesCountProblemBinaryBitArraySolution`
            //         
            public virtual void copy() {
                return this.@__copy__();
            }
            
            // 
            //         Copy the `OnesCountProblemBinaryBitArraySolution` to the already existing destination `OnesCountProblemBinaryBitArraySolution`
            // 
            //         :param `OnesCountProblemBinaryBitArraySolution` destination: destination `OnesCountProblemBinaryBitArraySolution`
            //         
            public virtual object copy_to(object destination) {
                destination = this.@__copy__();
            }
            
            // 
            //         Argument of the target solution
            // 
            //         :param representation: internal representation of the solution
            //         :type representation: `BitArray`
            //         :return: solution code
            //         :rtype: str 
            //         
            public virtual string argument(object representation) {
                return representation.bin;
            }
            
            // 
            //         Random initialization of the solution
            // 
            //         :param `TargetProblem` problem: problem which is solved by solution
            //         
            public virtual object init_random(object problem) {
                //logger.debug('Solution: ' + str(self))
                this.representation = BitArray(problem.dimension);
                foreach (var i in Enumerable.Range(0, problem.dimension)) {
                    if (random() > 0.5) {
                        this.representation[i] = true;
                    }
                }
            }
            
            // 
            //         Initialization of the solution, by setting its native representation 
            // 
            //         :param BitArray representation: representation that will be ste to solution
            //         :param `TargetProblem` problem: problem which is solved by solution
            //         
            public virtual object init_from(object representation, object problem) {
                this.representation = BitArray(bin: representation.bin);
            }
            
            // 
            //         Fitness calculation of the max ones binary BitArray solution
            // 
            //         :param BitArray representation: native representation of solution whose fitness is calculated
            //         :param TargetProblem problem: problem that is solved
            //         :return: objective value, fitness value and feasibility of the solution instance  
            //         :rtype: `QualityOfSolution`
            //         
            public virtual object calculate_quality_directly(object representation, object problem) {
                var ones_count = representation.count(true);
                return QualityOfSolution(ones_count, ones_count, true);
            }
            
            // 
            //         Obtain `BitArray` representation from string representation of the BitArray binary solution of the Max Ones problem 
            // 
            //         :param str representation_str: solution's representation as string
            //         :return: solution's representation as BitArray
            //         :rtype: `BitArray`
            //         
            public virtual object native_representation(string representation_str) {
                var ret = BitArray(bin: representation_str);
                return ret;
            }
            
            // 
            //         Calculating distance between two solutions determined by its code
            // 
            //         :param str solution_code_1: solution code for the first solution
            //         :param str solution_code_2: solution code for the second solution
            //         :return: distance between two solutions represented by its code
            //         :rtype: float
            //         
            public static double representation_distance_directly(object solution_code_1, string solution_code_2) {
                var rep_1 = this.native_representation(solution_code_1);
                var rep_2 = this.native_representation(solution_code_2);
                var result = (rep_1 ^ rep_2).count(true);
                return result;
            }
            
            // 
            //         String representation of the solution instance
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
            //         :return: string representation of instance that controls output
            //         :rtype: str
            //         
            public virtual string string_rep(
                string delimiter = "\n",
                int indentation = 0,
                string indentation_symbol = "   ",
                string group_start = "{",
                string group_end = "}") {
                var s = delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += group_start;
                s += base.string_rep(delimiter, indentation, indentation_symbol, "", "");
                s += delimiter;
                s += delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "string_representation()=" + this.string_representation().ToString();
                s += delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += group_end;
                return s;
            }
            
            // 
            //         String representation of the solution instance
            // 
            //         :return: string representation of the solution instance
            //         :rtype: str
            //         
            public override string ToString() {
                return this.string_rep("\n", 0, "   ", "{", "}");
            }
            
            // 
            //         Representation of the solution instance
            // 
            //         :return: string representation of the solution instance
            //         :rtype: str
            //         
            public virtual string @__repr__() {
                return this.string_rep("\n", 0, "   ", "{", "}");
            }
            
            // 
            //         Formatted the solution instance
            // 
            //         :param str spec: format specification
            //         :return: formatted solution instance
            //         :rtype: str
            //         
            public virtual string @__format__(string spec) {
                return this.string_rep("\n", 0, "   ", "{", "}");
            }
        }
    }
}
