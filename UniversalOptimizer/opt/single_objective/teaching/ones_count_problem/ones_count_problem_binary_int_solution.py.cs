//  
// .. _py_ones_count_problem_int_solution:
// 
// The :mod:`~opt.single_objective.teaching.ones_count_problem.ones_count_problem_binary_int_solution` contains class :class:`~opt.single_objective.teaching.ones_count_problem.ones_count_problem_binary_int_solution.OnesCountProblemBinaryIntSolution`, that represents solution of the :ref:`Problem_Max_Ones`, where `int` representation of the problem has been used.
// 
namespace single_objective.teaching.ones_count_problem {
    
    using sys;
    
    using Path = pathlib.Path;
    
    using deepcopy = copy.deepcopy;
    
    using choice = random.choice;
    
    using randint = random.randint;
    
    using TargetProblem = uo.target_problem.target_problem.TargetProblem;
    
    using QualityOfSolution = uo.target_solution.target_solution.QualityOfSolution;
    
    using TargetSolution = uo.target_solution.target_solution.TargetSolution;
    
    using logger = uo.utils.logger.logger;
    
    using System;
    
    using System.Linq;
    
    public static class ones_count_problem_binary_int_solution {
        
        public static object directory = Path(@__file__).resolve();
        
        static ones_count_problem_binary_int_solution() {
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent);
            sys.path.append(directory.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent.parent);
        }
        
        public class OnesCountProblemBinaryIntSolution
            : TargetSolution[intstr] {
            
            public object representation;
            
            public OnesCountProblemBinaryIntSolution(
                int random_seed = null,
                bool evaluation_cache_is_used = false,
                int evaluation_cache_max_size = 0,
                bool distance_calculation_cache_is_used = false,
                int distance_calculation_cache_max_size = 0)
                : base(random_seed: random_seed, fitness_value: null, objective_value: null, is_feasible: false, evaluation_cache_is_used: evaluation_cache_is_used, evaluation_cache_max_size: evaluation_cache_max_size, distance_calculation_cache_is_used: distance_calculation_cache_is_used, distance_calculation_cache_max_size: distance_calculation_cache_max_size) {
            }
            
            // 
            //         Internal copy of the `OnesCountProblemBinaryIntSolution`
            // 
            //         :return: new `OnesCountProblemBinaryIntSolution` instance with the same properties
            //         :rtype: OnesCountProblemBinaryIntSolution
            //         
            public virtual void @__copy__() {
                var sol = deepcopy(this);
                return sol;
            }
            
            // 
            //         Copy the `OnesCountProblemBinaryIntSolution`
            //         
            //         :return: new `OnesCountProblemBinaryIntSolution` instance with the same properties
            //         :rtype: `OnesCountProblemBinaryIntSolution`
            //         
            public virtual void copy() {
                return this.@__copy__();
            }
            
            // 
            //         Copy the `OnesCountProblemBinaryIntSolution` to the already existing destination `OnesCountProblemBinaryIntSolution`
            // 
            //         :param `OnesCountProblemBinaryIntSolution` destination: destination `OnesCountProblemBinaryIntSolution`
            //         
            public virtual object copy_to(object destination) {
                destination = this.@__copy__();
            }
            
            // 
            //         Helper function that modifies representation to be feasible
            // 
            //         :param `TargetProblem` problem: problem which is solved by solution
            //         
            public virtual void @__make_to_be_feasible_helper__(object problem) {
                var mask = ~0;
                mask <<= 32 - problem.dimension;
                mask = mask % 0x100000000 >> 32 - problem.dimension;
                this.representation &= mask;
            }
            
            // 
            //         Argument of the target solution for specific problem
            // 
            //         :param representation: internal representation of the solution
            //         :type representation: int
            //         :return: solution representation as string
            //         :rtype: str 
            //         
            public virtual string argument(int representation) {
                return bin(representation);
            }
            
            // 
            //         Random initialization of the solution
            // 
            //         :param `TargetProblem` problem: problem which is solved by solution
            //         
            public virtual object init_random(object problem) {
                if (problem.dimension is null) {
                    throw new ValueError("Problem dimension should not be None!");
                }
                if (problem.dimension <= 0) {
                    throw new ValueError("Problem dimension should be positive!");
                }
                if (problem.dimension >= 32) {
                    throw new ValueError("Problem dimension should be less than 32!");
                }
                this.representation = randint(0, 2 ^ problem.dimension - 1);
                this.@__make_to_be_feasible_helper__(problem);
            }
            
            // 
            //         Initialization of the solution, by setting its native representation 
            // 
            //         :param int representation: representation that will be ste to solution
            //         :param `TargetProblem` problem: problem which is solved by solution
            //         
            public virtual object init_from(int representation, object problem) {
                this.representation = representation;
            }
            
            // 
            //         Fitness calculation of the max ones binary int solution
            // 
            //         :param int representation: native representation of the solution whose fitness, objective and feasibility is calculated
            //         :param TargetProblem problem: problem that is solved
            //         :return: objective value, fitness value and feasibility of the solution instance  
            //         :rtype: `QualityOfSolution`
            //         
            public virtual object calculate_quality_directly(int representation, object problem) {
                var ones_count = representation.bit_count();
                return QualityOfSolution(ones_count, ones_count, true);
            }
            
            // 
            //         Obtain `int` representation from string representation of the integer binary solution of the Max Ones problem 
            // 
            //         :param str representation_str: solution's representation as string
            //         :return: solution's representation as int
            //         :rtype: int
            //         
            public virtual int native_representation(string representation_str) {
                var ret = Convert.ToInt32(representation_str, 2);
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
                s += "string_representation()=" + this.string_representation();
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
