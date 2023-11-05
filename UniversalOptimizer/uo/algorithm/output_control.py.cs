///  
/// The :mod:`~uo.Algorithm.OutputControl` module describes the class :class:`~uo.Algorithm.OutputControl`.
/// 
namespace uo.Algorithm {
    
    using Path = pathlib.Path;
    
    using sys;
    
    using TextIOWrapper = io.TextIOWrapper;
    
    using System.Collections.Generic;
    
    using System;
    
    using System.Linq;
    
       
        /// 
        ///     This class determine where the output generated during execution of the 
        ///     :class:`uo.Algorithm.Algorithm` instance will be written 
        ///     
        public class OutputControl {
            
            public List<string> _fields_definitions;
            
            public List<string> _fields_headings;
            
            private object _output_file;
            
            public bool _write_after_algorithm;
            
            public bool _write_after_evaluation;
            
            public bool _write_after_iteration;
            
            public bool _write_after_step_in_iteration;
            
            public bool _write_before_algorithm;
            
            public bool _write_before_evaluation;
            
            public bool _write_before_iteration;
            
            public bool _write_before_step_in_iteration;
            
            private object _write_to_output;
            
            public OutputControl(bool write_to_output = false, object output_file = null, string fields = "iteration, evaluation, \"step_name\", best_solution.argument(), best_solution.fitnessValue, best_solution.objectiveValue, best_solution.isFeasible", string moments = "after_algorithm") {
                _write_to_output = write_to_output;
                _output_file = output_file;
                _fields_headings = new List<string> {
                    "iteration",
                    "evaluation",
                    "step_name",
                    "best_solutionStringRepresentation",
                    "best_solution_fitnessValue",
                    "best_solution_objectiveValue",
                    "best_solution_isFeasible"
                };
                _fields_definitions = new List<string> {
                    "self.iteration",
                    "self.evaluation",
                    "\"step_name\"",
                    "self.best_solution.stringRepresentation()",
                    "self.best_solution.fitnessValue",
                    "self.best_solution.objectiveValue",
                    "self.best_solution.isFeasible"
                };
                _determine_fields_helper__(fields);
                _determine_moments_helper__(moments);
            }
            
            /// 
            ///         Helper function that determines fields header list anf field definition lists of the control instance
            /// 
            ///         :param str fields: comma-separated list of fields for output - basically fields of the optimizer object 
            ///         (e.g. `best_solution.fitnessValue`, `iteration`, `evaluation`, `seconds_max` etc.) and last word in specific 
            ///         field should he header od the csv column
            ///         
            public virtual void _determine_fields_helper__(string fields) {
                var fields_head = fields.replace(".", "_").replace(" ", "").replace("()", "").split(",");
                foreach (var f_h in fields_head) {
                    if (f_h != "") {
                        if (!this.fields_headings.Contains(f_h)) {
                            this.fields_headings.append(f_h);
                        }
                    }
                }
                var fields_def = fields.replace(" ", "").split(",");
                foreach (var f_def in fields_def) {
                    if (f_def != "") {
                        if (f_def[0] != "'" && f_def[0] != "\"") {
                            var f_def = "self." + f_def;
                        }
                        if (!this.fields_definitions.Contains(f_def)) {
                            this.fields_definitions.append(f_def);
                        }
                    }
                }
            }
            
            /// 
            ///         Helper function that determines moments when value of fields will be written to output
            /// 
            ///         :param str moments: comma-separated list of moments for output - contains following elements:
            ///         `before_algorithm`, `after_algorithm`, `before_iteration`, `after_iteration`, 
            ///         `before_evaluation`, `after_evaluation`, `before_step_in_iteration`, `after_step_in_iteration`
            ///         
            public virtual void _determine_moments_helper__(string moments) {
                _write_before_algorithm = false;
                _write_before_iteration = false;
                _write_after_iteration = false;
                _write_before_evaluation = false;
                _write_after_evaluation = false;
                _write_before_step_in_iteration = false;
                _write_after_step_in_iteration = false;
                _write_after_algorithm = true;
                var mom = moments.split(",");
                foreach (var mo in mom) {
                    var m = mo.strip();
                    if (m == "") {
                        continue;
                    }
                    if (m == "before_algorithm") {
                        _write_before_algorithm = true;
                    } else if (m == "after_algorithm") {
                        _write_after_algorithm = true;
                    } else if (m == "before_iteration") {
                        _write_before_iteration = true;
                    } else if (m == "after_iteration") {
                        _write_after_iteration = true;
                    } else if (m == "before_evaluation") {
                        _write_before_evaluation = true;
                    } else if (m == "after_evaluation") {
                        _write_after_evaluation = true;
                    } else if (m == "before_step_in_iteration") {
                        _write_before_step_in_iteration = true;
                    } else if (m == "after_step_in_iteration") {
                        _write_after_step_in_iteration = true;
                    } else {
                        throw new ValueError("Invalid value for moment {}. Should be one of:{}.".format(m, "before_algorithm, after_algorithm, before_iteration, after_iteration," + "before_evaluation`, after_evaluation, before_step_in_iteration, after_step_in_iteration"));
                    }
                }
            }
            
            /// 
            ///         Property getter for determining if write to output 
            /// 
            ///         :return: if write to output during algorithm execution, or not 
            ///         :rtype: bool
            ///         
            public object write_to_output {
                get {
                    return _write_to_output;
                }
            }
            
            /// 
            ///         Property getter for output file 
            /// 
            ///         :return: output file to which algorithm will write
            ///         :rtype: `TextIOWrapper`
            ///         
            /// 
            ///         Property setter for the output file
            ///         
            public object output_file {
                get {
                    return _output_file;
                }
                set {
                    _output_file = value;
                }
            }
            
            /// 
            ///         Property getter for `fields_headings` property 
            /// 
            ///         :return: list of fields headings for output
            ///         :rtype: list[str]
            ///         
            public object fields_headings {
                get {
                    return _fields_headings;
                }
            }
            
            /// 
            ///         Property getter for `fields_definitions` property 
            /// 
            ///         :return: list of fields definitions to be evaluated during output
            ///         :rtype: list[str]
            ///         
            public object fields_definitions {
                get {
                    return _fields_definitions;
                }
            }
            
            /// 
            ///         Property getter for `fields_definitions` property 
            /// 
            ///         :return: comma-separated string with list of fields for output
            ///         :rtype: str
            ///         
            /// 
            ///         Property setter for the fields property
            ///         
            public object fields {
                get {
                    return _fields_definitions.join(", ").replace("self.", "");
                }
                set {
                    _determine_fields_helper__(value);
                }
            }
            
            /// 
            ///         Property getter for moments property 
            /// 
            ///         :return: comma-separated list of moments for output
            ///         :rtype: str
            ///         
            /// 
            ///         Property setter for the moments property
            ///         
            public object moments {
                get {
                    var ret = "after_algorithm, ";
                    if (_write_before_algorithm) {
                        ret += "before_algorithm, ";
                    }
                    if (_write_before_iteration) {
                        ret += "before_iteration, ";
                    }
                    if (_write_after_iteration) {
                        ret += "after_iteration, ";
                    }
                    if (_write_before_evaluation) {
                        ret += "before_evaluation, ";
                    }
                    if (_write_after_evaluation) {
                        ret += "after_evaluation, ";
                    }
                    if (_write_before_step_in_iteration) {
                        ret += "before_step_in_iteration, ";
                    }
                    if (_write_after_step_in_iteration) {
                        ret += "after_step_in_iteration, ";
                    }
                    ret = ret[0: - 2:];
                    return ret;
                }
                set {
                    _determine_moments_helper__(value);
                }
            }
            
            /// 
            ///         Property getter for property `write_before_algorithm`
            /// 
            ///         :return: should write to the output prior to algorithm execution
            ///         :rtype: bool
            ///         
            public object write_before_algorithm {
                get {
                    return _write_before_algorithm;
                }
            }
            
            /// 
            ///         Property getter for property `write_after_algorithm`
            /// 
            ///         :return: should write to the output after algorithm execution
            ///         :rtype: bool
            ///         
            public object write_after_algorithm {
                get {
                    return _write_after_algorithm;
                }
            }
            
            /// 
            ///         Property getter for property `write_before_iteration`
            /// 
            ///         :return: should write to the output prior to algorithm iteration
            ///         :rtype: bool
            ///         
            public object write_before_iteration {
                get {
                    return _write_before_iteration;
                }
            }
            
            /// 
            ///         Property getter for property `write_after_iteration`
            /// 
            ///         :return: should write to the output after algorithm iteration
            ///         :rtype: bool
            ///         
            public object write_after_iteration {
                get {
                    return _write_after_iteration;
                }
            }
            
            /// 
            ///         Property getter for property `write_before_evaluation`
            /// 
            ///         :return: should write to the output prior to evaluation
            ///         :rtype: bool
            ///         
            public object write_before_evaluation {
                get {
                    return _write_before_evaluation;
                }
            }
            
            /// 
            ///         Property getter for property `write_after_evaluation`
            /// 
            ///         :return: should write to the output after evaluation
            ///         :rtype: bool
            ///         
            public object write_after_evaluation {
                get {
                    return _write_after_evaluation;
                }
            }
            
            /// 
            ///         Property getter for property `write_before_step_in_iteration`
            /// 
            ///         :return: should write to the output prior to step in iteration
            ///         :rtype: bool
            ///         
            public object write_before_step_in_iteration {
                get {
                    return _write_before_step_in_iteration;
                }
            }
            
            /// 
            ///         Property getter for property `write_after_step_in_iteration`
            /// 
            ///         :return: should write to the output after step in iteration
            ///         :rtype: bool
            ///         
            public object write_after_step_in_iteration {
                get {
                    return _write_after_step_in_iteration;
                }
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
            ///         :rtype: str
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
                s += "write_to_output=" + this.write_to_output.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += "output_file=" + this.output_file.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += "fields_headings=" + this.fields_headings.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += "fields_definitions=" + this.fields_definitions.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += "moments=" + this.moments.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += groupEnd;
                return s;
            }
            
            /// 
            ///         String representation of the cache control and statistics structure
            /// 
            ///         :return: string representation of the cache control and statistics structure
            ///         :rtype: str
            ///         
            public override string ToString() {
                return this.StringRep("|");
            }
            
            /// 
            ///         Representation of the cache control and statistics structure
            /// 
            ///         :return: string representation of cache control and statistics structure
            ///         :rtype: str
            ///         
            public virtual string _repr__() {
                return this.StringRep("\n");
            }
            
            /// 
            ///         Formatted the cache control and statistics structure
            /// 
            ///         :param str spec: format specification
            ///         :return: formatted cache control and statistics structure
            ///         :rtype: str
            ///         
            public virtual string _format__(string spec) {
                return this.StringRep("|");
            }
        }
    }
}
