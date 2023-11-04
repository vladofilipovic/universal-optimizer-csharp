//  
// The :mod:`~uo.algorithm.metaheuristic.additional_statistics_control` module describes the class :class:`~uo.algorithm.metaheuristic.AdditionalStatisticsControl`.
// 
namespace algorithm.metaheuristic {
    
    using Path = pathlib.Path;
    
    using sys;
    
    using System.Collections.Generic;
    
    using System;
    
    using System.Linq;
    
    public static class additional_statistics_control {
        
        public static object directory = Path(@__file__).resolve();
        
        static additional_statistics_control() {
            sys.path.append(directory.parent);
        }
        
        // 
        //     This class determine additional statistics that should be kept during execution of the 
        //     :class:`uo.algorithm.metaheuristic.Metaheuristic` 
        //     
        public class AdditionalStatisticsControl {
            
            public List<string> @__can_be_kept;
            
            public bool @__keep_all_solution_codes;
            
            public bool @__keep_more_local_optima;
            
            public object @__max_local_optima;
            
            public object all_solution_codes;
            
            public Dictionary<object, object> more_local_optima;
            
            public AdditionalStatisticsControl(string keep = "", int max_local_optima = 10) {
                this.@__can_be_kept = new List<string> {
                    "all_solution_code",
                    "more_local_optima"
                };
                this.@__max_local_optima = max_local_optima;
                this.@__determine_keep_helper__(keep);
            }
            
            // 
            //         Helper function that determines which criteria should be checked during
            // 
            //         :param str keep: comma-separated list of values that should be kept 
            //         (currently keep contains strings `all_solution_code`, `more_local_optima`) 
            //         
            public virtual void @__determine_keep_helper__(string keep) {
                this.@__keep_all_solution_codes = false;
                this.@__keep_more_local_optima = false;
                var kep = keep.split("&");
                foreach (var ke in kep) {
                    var k = ke.strip();
                    if (k == "" || k == "None") {
                        continue;
                    }
                    if (k == "all_solution_code") {
                        this.@__keep_all_solution_codes = true;
                    } else if (k == "more_local_optima") {
                        this.@__keep_more_local_optima = true;
                    } else {
                        throw new ValueError("Invalid value for keep '{}'. Should be one of:{}.".format(k, "all_solution_code, more_local_optima"));
                    }
                }
                if (this.@__keep_all_solution_codes) {
                    //class/static variable all_solution_codes
                    if (!hasattr(AdditionalStatisticsControl, "all_solution_codes")) {
                        AdditionalStatisticsControl.all_solution_codes = new HashSet<object>();
                    }
                }
                if (this.keep_more_local_optima) {
                    // values of the local optima foreach element calculated 
                    if (!hasattr(AdditionalStatisticsControl, "all_solution_codes")) {
                        AdditionalStatisticsControl.more_local_optima = new Dictionary<object, object> {
                        };
                    }
                }
            }
            
            // 
            //         Property getter for maximum number of local optima that will be kept
            // 
            //         :return: maximum number of local optima that will be kept
            //         :rtype: int
            //         
            public object max_local_optima {
                get {
                    return this.@__max_local_optima;
                }
            }
            
            // 
            //         Property getter for keep property 
            // 
            //         :return: comma-separated list of values vo be kept
            //         :rtype: str
            //         
            // 
            //         Property setter for the keep property 
            //         
            public object keep {
                get {
                    var ret = "";
                    if (this.@__keep_all_solution_codes) {
                        ret += "all_solution_code, ";
                    }
                    if (this.@__keep_more_local_optima) {
                        ret += "more_local_optima, ";
                    }
                    ret = ret[0: - 2:];
                    return ret;
                }
                set {
                    this.@__determine_keep_helper__(value);
                }
            }
            
            // 
            //         Property getter for property if all solution codes to be kept
            // 
            //         :return: if all solution codes to be kept
            //         :rtype: bool
            //         
            public object keep_all_solution_codes {
                get {
                    return this.@__keep_all_solution_codes;
                }
            }
            
            // 
            //         Property getter for decision if more local optima should be kept
            // 
            //         :return: if more local optima should be kept
            //         :rtype: bool
            //         
            public object keep_more_local_optima {
                get {
                    return this.@__keep_more_local_optima;
                }
            }
            
            // 
            //         Filling all solution code, if necessary 
            // 
            //         :param representation: solution representation to be inserted into all solution code
            //         :type representation: str
            //         :rtype: None
            //         
            public virtual object add_to_all_solution_codes_if_required(string representation) {
                if (this.keep_all_solution_codes) {
                    AdditionalStatisticsControl.all_solution_codes.add(representation);
                }
            }
            
            // 
            //         Add solution to the local optima structure 
            // 
            //         :param str solution_to_add_rep: string representation of the solution to be added to local optima structure
            //         :param float solution_to_add_fitness: fitness value of the solution to be added to local optima structure
            //         :param str best_solution_rep: string representation of the best solution so far
            //         :return:  if adding is successful e.g. current_solution is new element in the structure
            //         :rtype: bool
            //         
            public virtual bool add_to_more_local_optima_if_required(string solution_to_add_rep, object solution_to_add_fitness, string best_solution_rep) {
                if (!this.keep_more_local_optima) {
                    return false;
                }
                if (AdditionalStatisticsControl.more_local_optima.Contains(solution_to_add_rep)) {
                    return false;
                }
                if (AdditionalStatisticsControl.more_local_optima.Count >= this.@__max_local_optima) {
                    // removing random, just taking care not to remove the best ones
                    while (true) {
                        var code = random.choice(AdditionalStatisticsControl.more_local_optima.keys());
                        if (code != best_solution_rep) {
                            AdditionalStatisticsControl.more_local_optima.Remove(code);
                            break;
                        }
                    }
                }
                AdditionalStatisticsControl.more_local_optima[solution_to_add_rep] = solution_to_add_fitness;
                return true;
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
                s += "keep=" + this.keep.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "use_cache_for_distance_calculation=" + this.use_cache_for_distance_calculation.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                if (this.keep_all_solution_codes) {
                    foreach (var i in Enumerable.Range(0, indentation - 0)) {
                        s += indentation_symbol;
                    }
                    s += "all solution codes=" + AdditionalStatisticsControl.all_solution_codes.Count.ToString() + delimiter;
                }
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += group_end;
                return s;
            }
            
            // 
            //         String representation of the cache control and statistics structure
            // 
            //         :return: string representation of the cache control and statistics structure
            //         :rtype: str
            //         
            public override string ToString() {
                return this.string_rep("|");
            }
            
            // 
            //         Representation of the cache control and statistics structure
            // 
            //         :return: string representation of cache control and statistics structure
            //         :rtype: str
            //         
            public virtual string @__repr__() {
                return this.string_rep("\n");
            }
            
            // 
            //         Formatted the cache control and statistics structure
            // 
            //         :param str spec: format specification
            //         :return: formatted cache control and statistics structure
            //         :rtype: str
            //         
            public virtual string @__format__(string spec) {
                return this.string_rep("|");
            }
        }
    }
}
