///  
/// The :mod:`~uo.Algorithm.metaheuristic.finish_control` module describes the class :class:`~uo.Algorithm.metaheuristic.FinishControl`.
/// 
namespace uo.Algorithm.Metaheuristic {
    
        
        /// 
        ///     This class determine finishing criteria and status during execution of the 
        ///     :class:`uo.Algorithm.metaheuristic.Metaheuristic` 
        ///     
        public class FinishControl {
            
            public bool _check_evaluations;
            
            public bool _check_iterations;
            
            public bool _check_seconds;
            
            private object _evaluations_max;
            
            public List<string> _implemented_criteria;
            
            private object _iterations_max;
            
            private object _seconds_max;
            
            public FinishControl(string criteria = "evaluations & seconds & iterations", int evaluations_max = 0, int iterations_max = 0, double seconds_max = 0) {
                _implemented_criteria = new List<string> {
                    "evaluations_max",
                    "iterations_max",
                    "seconds_max"
                };
                _evaluations_max = evaluations_max;
                _iterations_max = iterations_max;
                _seconds_max = seconds_max;
                _determine_criteria_helper__(criteria);
            }
            
            /// 
            ///         Helper function that determines which criteria should be checked during
            /// 
            ///         :param str criteria: list of finish criteria, separated with sign `&` 
            ///         (currently finish criteria contains strings `evaluations`, `iterations`, `seconds`) 
            ///         
            public virtual void _determine_criteria_helper__(string criteria) {
                _check_evaluations = false;
                _check_iterations = false;
                _check_seconds = false;
                var crit = criteria.split("&");
                foreach (var cr in crit) {
                    var c = cr.strip();
                    if (c == "") {
                        continue;
                    }
                    if (c == "evaluations") {
                        if (_evaluations_max > 0) {
                            _check_evaluations = true;
                        }
                    } else if (c == "iterations") {
                        if (_iterations_max > 0) {
                            _check_iterations = true;
                        }
                    } else if (c == "seconds") {
                        if (_seconds_max > 0) {
                            _check_seconds = true;
                        }
                    } else {
                        throw new ValueError("Invalid value for criteria '{}'. Should be one of:{}.".format(c, "evaluations, iterations, seconds"));
                    }
                }
            }
            
            /// 
            ///         Property getter for maximum number of evaluations 
            /// 
            ///         :return: maximum number of evaluations 
            ///         return type int
            ///         
            public object evaluations_max {
                get {
                    return _evaluations_max;
                }
            }
            
            /// 
            ///         Property getter for maximum number of iterations 
            /// 
            ///         :return: maximum number of iterations 
            ///         return type int
            ///         
            public object iterations_max {
                get {
                    return _iterations_max;
                }
            }
            
            /// 
            ///         Property getter for maximum number of seconds for metaheuristic execution 
            /// 
            ///         :return: maximum number of seconds 
            ///         return type float
            ///         
            public object seconds_max {
                get {
                    return _seconds_max;
                }
            }
            
            /// 
            ///         Property getter for finish criteria property 
            /// 
            ///         :return: list of finish criteria separated with `&`
            ///         return type str
            ///         
            /// 
            ///         Property setter for the finish criteria property
            ///         
            public object criteria {
                get {
                    var ret = "";
                    if (_check_evaluations) {
                        ret += "evaluations & ";
                    }
                    if (_check_iterations) {
                        ret += "iterations & ";
                    }
                    if (_check_seconds) {
                        ret += "seconds & ";
                    }
                    ret = ret[0: - 2:];
                    return ret;
                }
                set {
                    _determine_criteria_helper__(value);
                }
            }
            
            /// 
            ///         Property getter for property `check_evaluations`
            /// 
            ///         :return: if number of evaluations is within finish criteria
            ///         return type bool
            ///         
            public object check_evaluations {
                get {
                    return _check_evaluations;
                }
            }
            
            /// 
            ///         Property getter for property `check_iterations`
            /// 
            ///         :return: if number of iterations is within finish criteria
            ///         return type bool
            ///         
            public object check_iterations {
                get {
                    return _check_iterations;
                }
            }
            
            /// 
            ///         Property getter for property `check_seconds`
            /// 
            ///         :return: if elapsed time (in seconds) is within finish criteria
            ///         return type bool
            ///         
            public object check_seconds {
                get {
                    return _check_seconds;
                }
            }
            
            /// 
            ///         Check if execution of metaheuristic is finished, according to specified criteria
            /// 
            ///         :param int evaluation: number of evaluations for metaheuristic execution
            ///         :param int iteration: number of iterations for metaheuristic execution
            ///         :param float elapsed_seconds: elapsed time (in seconds) for metaheuristic execution
            ///         :return: if execution is finished
            ///         return type bool
            ///         
            public virtual object is_finished(int evaluation, int iteration, double elapsed_seconds) {
                return this.check_evaluations && evaluation >= this.evaluations_max || this.check_iterations && iteration >= this.iterations_max || this.check_seconds && elapsed_seconds >= this.seconds_max;
            }
            
            /// 
            ///         String representation of the target solution instance
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
            ///         :return: string representation of instance that controls output
            ///         return type str
            ///         
            public virtual string StringRep(
                string delimiter,
                int indentation = 0,
                string indentationSymbol = "",
                string groupStart = "{",
                string groupEnd = "}") {
                var s = delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += groupStart + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += "criteria=" + this.criteria.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += "evaluations_max=" + this.evaluations_max.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += "iterations_max=" + this.iterations_max.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += "seconds_max=" + this.seconds_max.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += groupEnd;
                return s;
            }
            
            /// 
            ///         String representation of the finish control 
            /// 
            ///         :return: string representation of the finish control 
            ///         return type str
            ///         
            public override string ToString() {
                return this.StringRep("|");
            }
            
            /// 
            ///         Representation of the finish control 
            /// 
            ///         :return: string representation of finish control 
            ///         return type str
            ///         
            public virtual string _repr__() {
                return this.StringRep("\n");
            }
            
            /// 
            ///         Formatted the finish control 
            /// 
            ///         :param str spec: format specification
            ///         :return: formatted finish control structure
            ///         return type str
            ///         
            public virtual string _format__(string spec) {
                return this.StringRep("|");
            }
        }
    }
}
