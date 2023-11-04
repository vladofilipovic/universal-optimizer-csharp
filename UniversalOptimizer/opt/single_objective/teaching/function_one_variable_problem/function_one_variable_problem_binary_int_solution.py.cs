namespace single_objective.teaching.function_one_variable_problem {
    
    using sys;
    
    using Path = pathlib.Path;
    
    using deepcopy = copy.deepcopy;
    
    using choice = random.choice;
    
    using random = random.random;
    
    using randint = random.randint;
    
    using TargetProblem = uo.target_problem.target_problem.TargetProblem;
    
    using QualityOfSolution = uo.target_solution.target_solution.QualityOfSolution;
    
    using TargetSolution = uo.target_solution.target_solution.TargetSolution;
    
    using logger = uo.utils.logger.logger;
    
    using FunctionOneVariableProblem = opt.single_objective.teaching.function_one_variable_problem.function_one_variable_problem.FunctionOneVariableProblem;
    
    using System.Collections.Generic;
    
    using System;
    
    using System.Linq;
    
    public static class function_one_variable_problem_binary_int_solution {
        
        public static object directory = Path(@__file__).resolve();
        
        static function_one_variable_problem_binary_int_solution() {
            sys.path.append(directory);
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent.parent);
        }
        
        public class FunctionOneVariableProblemBinaryIntSolution
            : TargetSolution[intfloat] {
            
            public object @__domain_from;
            
            public object @__domain_to;
            
            public object @__number_of_intervals;
            
            public object representation;
            
            public FunctionOneVariableProblemBinaryIntSolution(
                double domain_from,
                double domain_to,
                int number_of_intervals,
                int random_seed = null,
                bool evaluation_cache_is_used = false,
                int evaluation_cache_max_size = 0,
                bool distance_calculation_cache_is_used = false,
                int distance_calculation_cache_max_size = 0)
                : base(random_seed: random_seed, fitness_value: null, objective_value: null, is_feasible: false, evaluation_cache_is_used: evaluation_cache_is_used, evaluation_cache_max_size: evaluation_cache_max_size, distance_calculation_cache_is_used: distance_calculation_cache_is_used, distance_calculation_cache_max_size: distance_calculation_cache_max_size) {
                this.@__domain_from = domain_from;
                this.@__domain_to = domain_to;
                this.@__number_of_intervals = number_of_intervals;
            }
            
            public virtual void @__copy__() {
                var sol = base.@__copy__();
                sol.domain_from = this.domain_from;
                sol.domain_to = this.domain_to;
                sol.number_of_intervals = this.number_of_intervals;
                return sol;
            }
            
            public virtual object copy() {
                return this.@__copy__();
            }
            
            public virtual object copy_to(object destination) {
                destination = this.@__copy__();
            }
            
            public object domain_from {
                get {
                    return this.@__domain_from;
                }
                set {
                    this.@__domain_from = value;
                }
            }
            
            public object domain_to {
                get {
                    return this.@__domain_to;
                }
                set {
                    this.@__domain_to = value;
                }
            }
            
            public object number_of_intervals {
                get {
                    return this.@__number_of_intervals;
                }
                set {
                    this.@__number_of_intervals = value;
                }
            }
            
            public virtual void @__make_to_be_feasible_helper__(object problem) {
                if (this.representation > this.number_of_intervals) {
                    this.representation = this.number_of_intervals;
                }
            }
            
            public virtual double argument(int representation) {
                return this.domain_from + representation * (this.domain_to - this.domain_from) / this.number_of_intervals;
            }
            
            public virtual object init_random(object problem) {
                this.representation = randint(0, this.number_of_intervals);
                this.@__make_to_be_feasible_helper__(problem);
            }
            
            public virtual object init_from(int representation, object problem) {
                this.representation = representation;
            }
            
            public virtual object calculate_quality_directly(int representation, object problem) {
                var arg = this.argument(representation);
                var res = eval(problem.expression, new Dictionary<object, object> {
                    {
                        "x",
                        arg}});
                return QualityOfSolution(res, res, true);
            }
            
            public virtual int native_representation(string representation_str) {
                var ret = Convert.ToInt32(representation_str, 2);
                return ret;
            }
            
            public static double representation_distance_directly(object solution_code_1, string solution_code_2) {
                var rep_1 = this.native_representation(solution_code_1);
                var rep_2 = this.native_representation(solution_code_2);
                var result = (rep_1 ^ rep_2).count(true);
                return result;
            }
            
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
            
            public override string ToString() {
                return this.string_rep("\n", 0, "   ", "{", "}");
            }
            
            public virtual string @__repr__() {
                return this.string_rep("\n", 0, "   ", "{", "}");
            }
            
            public virtual string @__format__(string spec) {
                return this.string_rep("\n", 0, "   ", "{", "}");
            }
        }
    }
}
