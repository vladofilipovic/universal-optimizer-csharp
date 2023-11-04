//  
// The :mod:`~uo.target_solution.target_solution` module describes the class :class:`~uo.target_solution.TargetSolution`.
// 
namespace target_solution {
    
    using Path = pathlib.Path;
    
    using sys;
    
    using deepcopy = copy.deepcopy;
    
    using randrange = random.randrange;
    
    using choice = random.choice;
    
    using ABCMeta = abc.ABCMeta;
    
    using abstractmethod = abc.abstractmethod;
    
    using NamedTuple = typing.NamedTuple;
    
    using TypeVar = typing.TypeVar;
    
    using Generic = typing.Generic;
    
    using Generic = typing.Generic;
    
    using Optional = typing.Optional;
    
    using TargetProblem = uo.target_problem.target_problem.TargetProblem;
    
    using EvaluationCacheControlStatistics = uo.target_solution.evaluation_cache_control_statistics.EvaluationCacheControlStatistics;
    
    using DistanceCalculationCacheControlStatistics = uo.target_solution.distance_calculation_cache_control_statistics.DistanceCalculationCacheControlStatistics;
    
    using System;
    
    using System.Collections.Generic;
    
    using System.Linq;
    
    public static class target_solution {
        
        public static object directory = Path(@__file__).resolve();
        
        static target_solution() {
            sys.path.append(directory.parent);
        }
        
        public static object QualityOfSolution = NamedTuple("QualityOfSolution", new List<Tuple<string, Func<object>>> {
            ("objective_value", @float | list[@float]),
            ("fitness_value", @float | list[@float]),
            ("is_feasible", @bool)
        });
        
        public static object R_co = TypeVar("R_co", covariant: true);
        
        public static object A_co = TypeVar("A_co", covariant: true);
        
        public class TargetSolution
            : Generic[R_coA_co], ABCMeta {
            
            public object @__distance_calculation_cache_is_used;
            
            public object @__distance_calculation_cache_max_size;
            
            public object @__evaluation_cache_is_used;
            
            public object @__evaluation_cache_max_size;
            
            public object @__fitness_value;
            
            public object @__is_feasible;
            
            public object @__name;
            
            public object @__objective_value;
            
            public object @__random_seed;
            
            public object @__representation;
            
            public object evaluation_cache_cs;
            
            public object fitness_value;
            
            public object is_feasible;
            
            public object objective_value;
            
            public object representation_distance_cache_cs;
            
            [abstractmethod]
            public TargetSolution(
                string name,
                int random_seed,
                object fitness_value,
                object objective_value,
                bool is_feasible,
                bool evaluation_cache_is_used,
                int evaluation_cache_max_size,
                bool distance_calculation_cache_is_used,
                int distance_calculation_cache_max_size) {
                this.@__name = name;
                if (random_seed is not null && random_seed is int && random_seed != 0) {
                    this.@__random_seed = random_seed;
                } else {
                    this.@__random_seed = randrange(sys.maxsize);
                }
                this.@__fitness_value = fitness_value;
                this.@__objective_value = objective_value;
                this.@__is_feasible = is_feasible;
                this.@__representation = null;
                this.@__evaluation_cache_is_used = evaluation_cache_is_used;
                this.@__evaluation_cache_max_size = evaluation_cache_max_size;
                //class/static variable evaluation_cache_cs
                if (!hasattr(TargetSolution, "evaluation_cache_cs")) {
                    TargetSolution.evaluation_cache_cs = EvaluationCacheControlStatistics(this.@__evaluation_cache_is_used, this.@__evaluation_cache_max_size);
                }
                this.@__distance_calculation_cache_is_used = distance_calculation_cache_is_used;
                this.@__distance_calculation_cache_max_size = distance_calculation_cache_max_size;
                //class/static variable representation_distance_cache_cs
                if (!hasattr(TargetSolution, "representation_distance_cache_cs")) {
                    TargetSolution.representation_distance_cache_cs = DistanceCalculationCacheControlStatistics[R_co](this.@__distance_calculation_cache_is_used, this.@__distance_calculation_cache_max_size);
                }
            }
            
            // 
            //         Internal copy of the current target solution
            // 
            //         :return:  new :class:`uo.target_solution.TargetSolution` instance with the same properties
            //         :rtype: TargetSolution
            //         
            [abstractmethod]
            public virtual void @__copy__() {
                var ts = deepcopy(this);
                return ts;
            }
            
            // 
            //         Copy the current target solution
            // 
            //         :return: new :class:`uo.target_solution.TargetSolution` instance with the same properties
            //         :rtype: TargetSolution
            //         
            [abstractmethod]
            public virtual void copy() {
                return this.@__copy__();
            }
            
            // 
            //         Copy the current target solution to the already existing destination target solution
            // 
            //         :param destination: destination target solution
            //         :type destination: :class:`uo.target_solution.TargetSolution`
            //         
            [abstractmethod]
            public virtual object copy_to(object destination) {
                destination = copy(this);
            }
            
            // 
            //         Property getter for the name of the target solution
            // 
            //         :return: name of the target solution instance 
            //         :rtype: str
            //         
            public object name {
                get {
                    return this.@__name;
                }
            }
            
            // 
            //         Property getter for fitness value of the target solution
            // 
            //         :return: fitness value of the target solution instance 
            //         :rtype: float
            //         
            // 
            //         Property setter for fitness value of the target solution
            // 
            //         :param value: value of the `fitness` to be set
            //         :type value: float
            //         
            public object fitness_value {
                get {
                    return this.@__fitness_value;
                }
                set {
                    this.@__fitness_value = value;
                }
            }
            
            // 
            //         Property getter for objective value of the target solution
            // 
            //         :return: objective value of the target solution instance 
            //         :rtype: float
            //         
            // 
            //         Property setter for objective value of the target solution
            // 
            //         :param value: value of the `objective_value` to be set
            //         :type value: float
            //         
            public object objective_value {
                get {
                    return this.@__objective_value;
                }
                set {
                    this.@__objective_value = value;
                }
            }
            
            // 
            //         Property getter for feasibility of the target solution
            // 
            //         :return: feasibility of the target solution instance 
            //         :rtype: bool
            //         
            // 
            //         Property setter for feasibility of the target solution
            // 
            //         :param value: value to be set for the `is_feasible`
            //         :type value: bool
            //         
            public object is_feasible {
                get {
                    return this.@__is_feasible;
                }
                set {
                    this.@__is_feasible = value;
                }
            }
            
            // 
            //         Property getter for representation of the target solution
            // 
            //         :return: representation of the target solution instance 
            //         :rtype: R_co
            //         
            // 
            //         Property setter for representation of the target solution
            // 
            //         :param value: value to be set for the representation of the solution
            //         :type value: R_co
            //         
            public object representation {
                get {
                    return this.@__representation;
                }
                set {
                    this.@__representation = value;
                }
            }
            
            // 
            //         Argument of the target solution
            // 
            //         :param representation: internal representation of the solution
            //         :type representation: R_co
            //         :return: argument of the solution 
            //         :rtype: A_co
            //         
            [abstractmethod]
            public virtual object argument(object representation) {
                throw new NotImplementedException();
            }
            
            // 
            //         String representation of the target solution
            // 
            //         :param representation: internal representation of the solution
            //         :type representation: R_co
            //         :return: string representation of the solution 
            //         :rtype: str
            //         
            public virtual string string_representation() {
                return this.argument(this.representation).ToString();
            }
            
            // 
            //         Random initialization of the solution
            // 
            //         :param problem: problem which is solved by solution
            //         :type problem: `TargetProblem`
            //         
            [abstractmethod]
            public virtual object init_random(object problem) {
                throw new NotImplementedException();
            }
            
            // 
            //         Obtain native representation from solution code of the `Solution` instance
            // 
            //         :param str representation_str: solution's representation as string (e.g. solution code)
            //         :return: solution's native representation 
            //         :rtype: R_co
            //         
            [abstractmethod]
            public virtual object native_representation(string representation_str) {
                throw new NotImplementedException();
            }
            
            // 
            //         Random initialization of the solution
            // 
            //         :param `TargetProblem` problem: problem which is solved by solution
            //         
            [abstractmethod]
            public virtual object init_random(object problem) {
                throw new NotImplementedException();
            }
            
            // 
            //         Initialization of the solution, by setting its native representation 
            // 
            //         :param R_co representation: representation that will be ste to solution
            //         :param `TargetProblem` problem: problem which is solved by solution
            //         
            [abstractmethod]
            public virtual object init_from(object representation, object problem) {
                throw new NotImplementedException();
            }
            
            // 
            //         Fitness calculation of the target solution
            // 
            //         :param R_co representation: native representation of the solution for which objective value, fitness and feasibility are calculated
            //         :param TargetProblem problem: problem that is solved
            //         :return: objective value, fitness value and feasibility of the solution instance 
            //         :rtype: `QualityOfSolution`
            //         
            [abstractmethod]
            public virtual object calculate_quality_directly(object representation, object problem) {
                throw new NotImplementedException();
            }
            
            // 
            //         Calculate fitness, objective and feasibility of the solution, with optional cache consultation
            // 
            //         :param TargetProblem target_problem: problem that is solved
            //         :return: objective value, fitness value and feasibility of the solution instance 
            //         :rtype: `QualityOfSolution`
            //         
            public virtual object calculate_quality(object target_problem) {
                object triplet;
                var eccs = TargetSolution.evaluation_cache_cs;
                if (eccs.is_caching) {
                    eccs.increment_cache_request_count();
                    var rep = this.string_representation();
                    if (eccs.cache.Contains(rep)) {
                        eccs.increment_cache_hit_count();
                        return eccs.cache[rep];
                    }
                    triplet = this.calculate_quality_directly(this.representation, target_problem);
                    if (eccs.cache.Count >= eccs.max_cache_size) {
                        // removing random
                        var code = random.choice(eccs.cache.keys());
                        eccs.cache.Remove(code);
                    }
                    eccs.cache[rep] = triplet;
                    return triplet;
                } else {
                    triplet = this.calculate_quality_directly(this.representation, target_problem);
                    return triplet;
                }
            }
            
            // 
            //         Evaluate current target solution
            // 
            //         :param TargetProblem target_problem: problem that is solved
            //         
            public virtual object evaluate(object target_problem) {
                var triplet = this.calculate_quality(target_problem);
                this.objective_value = triplet.objective_value;
                this.fitness_value = triplet.fitness_value;
                this.is_feasible = triplet.is_feasible;
            }
            
            // 
            //         Directly calculate distance between two solutions determined by its native representations
            // 
            //         :param `R_co` representation_1: native representation for the first solution
            //         :param `R_co` representation_2: native representation for the second solution
            //         :return: distance 
            //         :rtype: float
            //         
            [abstractmethod]
            public virtual double representation_distance_directly(object representation_1, object representation_2) {
                throw new NotImplementedException();
            }
            
            // 
            //         Calculate distance between two native representations, with optional cache consultation
            // 
            //         :param `R_co` representation_1: native representation for the first solution
            //         :param `R_co` representation_2: native representation for the second solution
            //         :return: distance 
            //         :rtype: float
            //         
            public virtual double representation_distance(object representation_1, object representation_2) {
                object ret;
                var rdcs = TargetSolution.representation_distance_cache_cs;
                if (rdcs.is_caching) {
                    rdcs.increment_cache_request_count();
                    var pair = (representation_1, representation_2);
                    if (rdcs.cache.Contains(pair)) {
                        rdcs.increment_cache_hit_count();
                        return rdcs.cache[pair];
                    }
                    ret = this.representation_distance_directly(representation_1, representation_2);
                    if (rdcs.cache.Count >= rdcs.max_cache_size) {
                        // removing random
                        var code = random.choice(rdcs.cache.keys());
                        rdcs.cache.Remove(code);
                    }
                    rdcs.cache[pair] = ret;
                    return ret;
                } else {
                    ret = this.representation_distance_directly(representation_1, representation_2);
                    return ret;
                }
            }
            
            // 
            //         String representation of the target solution instance
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
                string delimiter,
                int indentation = 0,
                string indentation_symbol = "",
                string group_start = "{",
                string group_end = "}") {
                var s = delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += group_start + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "name=" + this.name + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "fitness_value=" + this.fitness_value.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "objective_value=" + this.objective_value.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "is_feasible=" + this.is_feasible.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "representation()=" + this.representation.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "evaluation_cache_cs=" + this.evaluation_cache_cs.string_rep(delimiter, indentation + 1, indentation_symbol, "{", "}");
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "__representation_distance_cache_cs(static)=" + TargetSolution.representation_distance_cache_cs.string_rep(delimiter, indentation + 1, indentation_symbol, "{", "}") + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += group_end;
                return s;
            }
            
            // 
            //         String representation of the target solution instance
            // 
            //         :return: string representation of the target solution instance
            //         :rtype: str
            //         
            [abstractmethod]
            public override string ToString() {
                return this.string_repr("|");
            }
            
            // 
            //         Representation of the target solution instance
            // 
            //         :return: string representation of the target solution instance
            //         :rtype: str
            //         
            [abstractmethod]
            public virtual string @__repr__() {
                return this.string_rep("\n");
            }
            
            // 
            //         Formatted the target solution instance
            // 
            //         :param spec: str -- format specification
            //         :return: formatted target solution instance
            //         :rtype: str
            //         
            [abstractmethod]
            public virtual string @__format__(string spec) {
                return this.string_rep("|");
            }
        }
    }
}
