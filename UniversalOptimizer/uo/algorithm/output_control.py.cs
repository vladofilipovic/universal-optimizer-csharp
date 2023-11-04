//  
// The :mod:`~uo.algorithm.output_control` module describes the class :class:`~uo.algorithm.OutputControl`.
// 
namespace algorithm {
    
    using Path = pathlib.Path;
    
    using sys;
    
    using TextIOWrapper = io.TextIOWrapper;
    
    using System.Collections.Generic;
    
    using System;
    
    using System.Linq;
    
    public static class output_control {
        
        public static object directory = Path(@__file__).resolve();
        
        static output_control() {
            sys.path.append(directory.parent);
        }
        
        // 
        //     This class determine where the output generated during execution of the 
        //     :class:`uo.algorithm.Algorithm` instance will be written 
        //     
        public class OutputControl {
            
            public List<string> @__fields_definitions;
            
            public List<string> @__fields_headings;
            
            public object @__output_file;
            
            public bool @__write_after_algorithm;
            
            public bool @__write_after_evaluation;
            
            public bool @__write_after_iteration;
            
            public bool @__write_after_step_in_iteration;
            
            public bool @__write_before_algorithm;
            
            public bool @__write_before_evaluation;
            
            public bool @__write_before_iteration;
            
            public bool @__write_before_step_in_iteration;
            
            public object @__write_to_output;
            
            public OutputControl(bool write_to_output = false, object output_file = null, string fields = "iteration, evaluation, \"step_name\", best_solution.argument(), best_solution.fitness_value, best_solution.objective_value, best_solution.is_feasible", string moments = "after_algorithm") {
                this.@__write_to_output = write_to_output;
                this.@__output_file = output_file;
                this.@__fields_headings = new List<string> {
                    "iteration",
                    "evaluation",
                    "step_name",
                    "best_solution_string_representation",
                    "best_solution_fitness_value",
                    "best_solution_objective_value",
                    "best_solution_is_feasible"
                };
                this.@__fields_definitions = new List<string> {
                    "self.iteration",
                    "self.evaluation",
                    "\"step_name\"",
                    "self.best_solution.string_representation()",
                    "self.best_solution.fitness_value",
                    "self.best_solution.objective_value",
                    "self.best_solution.is_feasible"
                };
                this.@__determine_fields_helper__(fields);
                this.@__determine_moments_helper__(moments);
            }
            
            // 
            //         Helper function that determines fields header list anf field definition lists of the control instance
            // 
            //         :param str fields: comma-separated list of fields for output - basically fields of the optimizer object 
            //         (e.g. `best_solution.fitness_value`, `iteration`, `evaluation`, `seconds_max` etc.) and last word in specific 
            //         field should he header od the csv column
            //         
            public virtual void @__determine_fields_helper__(string fields) {
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
            
            // 
            //         Helper function that determines moments when value of fields will be written to output
            // 
            //         :param str moments: comma-separated list of moments for output - contains following elements:
            //         `before_algorithm`, `after_algorithm`, `before_iteration`, `after_iteration`, 
            //         `before_evaluation`, `after_evaluation`, `before_step_in_iteration`, `after_step_in_iteration`
            //         
            public virtual void @__determine_moments_helper__(string moments) {
                this.@__write_before_algorithm = false;
                this.@__write_before_iteration = false;
                this.@__write_after_iteration = false;
                this.@__write_before_evaluation = false;
                this.@__write_after_evaluation = false;
                this.@__write_before_step_in_iteration = false;
                this.@__write_after_step_in_iteration = false;
                this.@__write_after_algorithm = true;
                var mom = moments.split(",");
                foreach (var mo in mom) {
                    var m = mo.strip();
                    if (m == "") {
                        continue;
                    }
                    if (m == "before_algorithm") {
                        this.@__write_before_algorithm = true;
                    } else if (m == "after_algorithm") {
                        this.@__write_after_algorithm = true;
                    } else if (m == "before_iteration") {
                        this.@__write_before_iteration = true;
                    } else if (m == "after_iteration") {
                        this.@__write_after_iteration = true;
                    } else if (m == "before_evaluation") {
                        this.@__write_before_evaluation = true;
                    } else if (m == "after_evaluation") {
                        this.@__write_after_evaluation = true;
                    } else if (m == "before_step_in_iteration") {
                        this.@__write_before_step_in_iteration = true;
                    } else if (m == "after_step_in_iteration") {
                        this.@__write_after_step_in_iteration = true;
                    } else {
                        throw new ValueError("Invalid value for moment {}. Should be one of:{}.".format(m, "before_algorithm, after_algorithm, before_iteration, after_iteration," + "before_evaluation`, after_evaluation, before_step_in_iteration, after_step_in_iteration"));
                    }
                }
            }
            
            // 
            //         Property getter for determining if write to output 
            // 
            //         :return: if write to output during algorithm execution, or not 
            //         :rtype: bool
            //         
            public object write_to_output {
                get {
                    return this.@__write_to_output;
                }
            }
            
            // 
            //         Property getter for output file 
            // 
            //         :return: output file to which algorithm will write
            //         :rtype: `TextIOWrapper`
            //         
            // 
            //         Property setter for the output file
            //         
            public object output_file {
                get {
                    return this.@__output_file;
                }
                set {
                    this.@__output_file = value;
                }
            }
            
            // 
            //         Property getter for `fields_headings` property 
            // 
            //         :return: list of fields headings for output
            //         :rtype: list[str]
            //         
            public object fields_headings {
                get {
                    return this.@__fields_headings;
                }
            }
            
            // 
            //         Property getter for `fields_definitions` property 
            // 
            //         :return: list of fields definitions to be evaluated during output
            //         :rtype: list[str]
            //         
            public object fields_definitions {
                get {
                    return this.@__fields_definitions;
                }
            }
            
            // 
            //         Property getter for `fields_definitions` property 
            // 
            //         :return: comma-separated string with list of fields for output
            //         :rtype: str
            //         
            // 
            //         Property setter for the fields property
            //         
            public object fields {
                get {
                    return this.@__fields_definitions.join(", ").replace("self.", "");
                }
                set {
                    this.@__determine_fields_helper__(value);
                }
            }
            
            // 
            //         Property getter for moments property 
            // 
            //         :return: comma-separated list of moments for output
            //         :rtype: str
            //         
            // 
            //         Property setter for the moments property
            //         
            public object moments {
                get {
                    var ret = "after_algorithm, ";
                    if (this.@__write_before_algorithm) {
                        ret += "before_algorithm, ";
                    }
                    if (this.@__write_before_iteration) {
                        ret += "before_iteration, ";
                    }
                    if (this.@__write_after_iteration) {
                        ret += "after_iteration, ";
                    }
                    if (this.@__write_before_evaluation) {
                        ret += "before_evaluation, ";
                    }
                    if (this.@__write_after_evaluation) {
                        ret += "after_evaluation, ";
                    }
                    if (this.@__write_before_step_in_iteration) {
                        ret += "before_step_in_iteration, ";
                    }
                    if (this.@__write_after_step_in_iteration) {
                        ret += "after_step_in_iteration, ";
                    }
                    ret = ret[0: - 2:];
                    return ret;
                }
                set {
                    this.@__determine_moments_helper__(value);
                }
            }
            
            // 
            //         Property getter for property `write_before_algorithm`
            // 
            //         :return: should write to the output prior to algorithm execution
            //         :rtype: bool
            //         
            public object write_before_algorithm {
                get {
                    return this.@__write_before_algorithm;
                }
            }
            
            // 
            //         Property getter for property `write_after_algorithm`
            // 
            //         :return: should write to the output after algorithm execution
            //         :rtype: bool
            //         
            public object write_after_algorithm {
                get {
                    return this.@__write_after_algorithm;
                }
            }
            
            // 
            //         Property getter for property `write_before_iteration`
            // 
            //         :return: should write to the output prior to algorithm iteration
            //         :rtype: bool
            //         
            public object write_before_iteration {
                get {
                    return this.@__write_before_iteration;
                }
            }
            
            // 
            //         Property getter for property `write_after_iteration`
            // 
            //         :return: should write to the output after algorithm iteration
            //         :rtype: bool
            //         
            public object write_after_iteration {
                get {
                    return this.@__write_after_iteration;
                }
            }
            
            // 
            //         Property getter for property `write_before_evaluation`
            // 
            //         :return: should write to the output prior to evaluation
            //         :rtype: bool
            //         
            public object write_before_evaluation {
                get {
                    return this.@__write_before_evaluation;
                }
            }
            
            // 
            //         Property getter for property `write_after_evaluation`
            // 
            //         :return: should write to the output after evaluation
            //         :rtype: bool
            //         
            public object write_after_evaluation {
                get {
                    return this.@__write_after_evaluation;
                }
            }
            
            // 
            //         Property getter for property `write_before_step_in_iteration`
            // 
            //         :return: should write to the output prior to step in iteration
            //         :rtype: bool
            //         
            public object write_before_step_in_iteration {
                get {
                    return this.@__write_before_step_in_iteration;
                }
            }
            
            // 
            //         Property getter for property `write_after_step_in_iteration`
            // 
            //         :return: should write to the output after step in iteration
            //         :rtype: bool
            //         
            public object write_after_step_in_iteration {
                get {
                    return this.@__write_after_step_in_iteration;
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
                s += "write_to_output=" + this.write_to_output.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "output_file=" + this.output_file.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "fields_headings=" + this.fields_headings.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "fields_definitions=" + this.fields_definitions.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "moments=" + this.moments.ToString() + delimiter;
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
