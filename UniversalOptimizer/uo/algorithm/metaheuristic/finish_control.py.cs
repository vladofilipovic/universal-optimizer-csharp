//  
// The :mod:`~uo.algorithm.metaheuristic.finish_control` module describes the class :class:`~uo.algorithm.metaheuristic.FinishControl`.
// 
namespace algorithm.metaheuristic {
    
    using Path = pathlib.Path;
    
    using sys;
    
    using System.Collections.Generic;
    
    using System;
    
    using System.Linq;
    
    public static class finish_control {
        
        public static object directory = Path(@__file__).resolve();
        
        static finish_control() {
            sys.path.append(directory.parent);
        }
        
        // 
        //     This class determine finishing criteria and status during execution of the 
        //     :class:`uo.algorithm.metaheuristic.Metaheuristic` 
        //     
        public class FinishControl {
            
            public bool @__check_evaluations;
            
            public bool @__check_iterations;
            
            public bool @__check_seconds;
            
            public object @__evaluations_max;
            
            public List<string> @__implemented_criteria;
            
            public object @__iterations_max;
            
            public object @__seconds_max;
            
            public FinishControl(string criteria = "evaluations & seconds & iterations", int evaluations_max = 0, int iterations_max = 0, double seconds_max = 0) {
                this.@__implemented_criteria = new List<string> {
                    "evaluations_max",
                    "iterations_max",
                    "seconds_max"
                };
                this.@__evaluations_max = evaluations_max;
                this.@__iterations_max = iterations_max;
                this.@__seconds_max = seconds_max;
                this.@__determine_criteria_helper__(criteria);
            }
            
            // 
            //         Helper function that determines which criteria should be checked during
            // 
            //         :param str criteria: list of finish criteria, separated with sign `&` 
            //         (currently finish criteria contains strings `evaluations`, `iterations`, `seconds`) 
            //         
            public virtual void @__determine_criteria_helper__(string criteria) {
                this.@__check_evaluations = false;
                this.@__check_iterations = false;
                this.@__check_seconds = false;
                var crit = criteria.split("&");
                foreach (var cr in crit) {
                    var c = cr.strip();
                    if (c == "") {
                        continue;
                    }
                    if (c == "evaluations") {
                        if (this.@__evaluations_max > 0) {
                            this.@__check_evaluations = true;
                        }
                    } else if (c == "iterations") {
                        if (this.@__iterations_max > 0) {
                            this.@__check_iterations = true;
                        }
                    } else if (c == "seconds") {
                        if (this.@__seconds_max > 0) {
                            this.@__check_seconds = true;
                        }
                    } else {
                        throw new ValueError("Invalid value for criteria '{}'. Should be one of:{}.".format(c, "evaluations, iterations, seconds"));
                    }
                }
            }
            
            // 
            //         Property getter for maximum number of evaluations 
            // 
            //         :return: maximum number of evaluations 
            //         :rtype: int
            //         
            public object evaluations_max {
                get {
                    return this.@__evaluations_max;
                }
            }
            
            // 
            //         Property getter for maximum number of iterations 
            // 
            //         :return: maximum number of iterations 
            //         :rtype: int
            //         
            public object iterations_max {
                get {
                    return this.@__iterations_max;
                }
            }
            
            // 
            //         Property getter for maximum number of seconds for metaheuristic execution 
            // 
            //         :return: maximum number of seconds 
            //         :rtype: float
            //         
            public object seconds_max {
                get {
                    return this.@__seconds_max;
                }
            }
            
            // 
            //         Property getter for finish criteria property 
            // 
            //         :return: list of finish criteria separated with `&`
            //         :rtype: str
            //         
            // 
            //         Property setter for the finish criteria property
            //         
            public object criteria {
                get {
                    var ret = "";
                    if (this.@__check_evaluations) {
                        ret += "evaluations & ";
                    }
                    if (this.@__check_iterations) {
                        ret += "iterations & ";
                    }
                    if (this.@__check_seconds) {
                        ret += "seconds & ";
                    }
                    ret = ret[0: - 2:];
                    return ret;
                }
                set {
                    this.@__determine_criteria_helper__(value);
                }
            }
            
            // 
            //         Property getter for property `check_evaluations`
            // 
            //         :return: if number of evaluations is within finish criteria
            //         :rtype: bool
            //         
            public object check_evaluations {
                get {
                    return this.@__check_evaluations;
                }
            }
            
            // 
            //         Property getter for property `check_iterations`
            // 
            //         :return: if number of iterations is within finish criteria
            //         :rtype: bool
            //         
            public object check_iterations {
                get {
                    return this.@__check_iterations;
                }
            }
            
            // 
            //         Property getter for property `check_seconds`
            // 
            //         :return: if elapsed time (in seconds) is within finish criteria
            //         :rtype: bool
            //         
            public object check_seconds {
                get {
                    return this.@__check_seconds;
                }
            }
            
            // 
            //         Check if execution of metaheuristic is finished, according to specified criteria
            // 
            //         :param int evaluation: number of evaluations for metaheuristic execution
            //         :param int iteration: number of iterations for metaheuristic execution
            //         :param float elapsed_seconds: elapsed time (in seconds) for metaheuristic execution
            //         :return: if execution is finished
            //         :rtype: bool
            //         
            public virtual object is_finished(int evaluation, int iteration, double elapsed_seconds) {
                return this.check_evaluations && evaluation >= this.evaluations_max || this.check_iterations && iteration >= this.iterations_max || this.check_seconds && elapsed_seconds >= this.seconds_max;
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
                s += "criteria=" + this.criteria.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "evaluations_max=" + this.evaluations_max.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "iterations_max=" + this.iterations_max.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "seconds_max=" + this.seconds_max.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += group_end;
                return s;
            }
            
            // 
            //         String representation of the finish control 
            // 
            //         :return: string representation of the finish control 
            //         :rtype: str
            //         
            public override string ToString() {
                return this.string_rep("|");
            }
            
            // 
            //         Representation of the finish control 
            // 
            //         :return: string representation of finish control 
            //         :rtype: str
            //         
            public virtual string @__repr__() {
                return this.string_rep("\n");
            }
            
            // 
            //         Formatted the finish control 
            // 
            //         :param str spec: format specification
            //         :return: formatted finish control structure
            //         :rtype: str
            //         
            public virtual string @__format__(string spec) {
                return this.string_rep("|");
            }
        }
    }
}
