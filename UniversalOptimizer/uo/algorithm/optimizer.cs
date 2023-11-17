///  
/// The :mod:`~uo.Algorithm.optimizer` module describes the class :class:`~uo.Algorithm.Optimizer`.
/// 
namespace uo.Algorithm {
    
    using Path = pathlib.Path;
    
    using sys;
    
    using deepcopy = copy.deepcopy;
    
    using datetime = datetime.datetime;
    
    using ABCMeta = abc.ABCMeta;
    
    using abstractmethod = abc.abstractmethod;
    
    using logger = uo.utils.logger.logger;
    
    using OutputControl = uo.Algorithm.OutputControl.OutputControl;
    
    using TargetProblem = uo.TargetProblem.TargetProblem.TargetProblem;
    
    using TargetSolution = uo.TargetSolution.TargetSolution.TargetSolution;
    
    using System;
    
    using System.Linq;
        
        public abstract class Optimizer: {
            
            private object _best_solution;
            
            private object _execution_ended;
            
            private object _execution_started;
            
            private object _iteration_best_found;
            
            private object _name;
            
            private object _OutputControl;
            
            private object _second_when_best_obtained;
            
            private object _TargetProblem;
            
            [abstractmethod]
            public Optimizer(string name, object OutputControl, object TargetProblem) {
                _name = name;
                _OutputControl = OutputControl;
                if (TargetProblem is TargetProblem) {
                    _TargetProblem = TargetProblem.copy();
                } else {
                    _TargetProblem = TargetProblem;
                }
                _execution_started = null;
                _execution_ended = null;
                _best_solution = null;
            }
            
            /// 
            ///         Internal copy of the current optimizer
            /// 
            ///         :return:  new `Optimizer` instance with the same properties
            ///         return type :class:`uo.Algorithm.Optimizer`
            ///         
            [abstractmethod]
            public virtual void _copy__() {
                var opt = deepcopy(this);
                return opt;
            }
            
            /// 
            ///         Copy the current optimizer
            /// 
            ///         :return:  new `Optimizer` instance with the same properties
            ///         return type :class:`uo.Algorithm.Optimizer`
            ///         
            [abstractmethod]
            public virtual void copy() {
                return _copy__();
            }
            
            /// 
            ///         Property getter for the name of the optimizer
            ///         
            ///         :return: name of the algorithm instance 
            ///         return type str
            ///         
            public object name {
                get {
                    return _name;
                }
            }
            
            /// 
            ///         Property getter for the target problem to be solved
            ///         
            ///         :return TargetProblem: target problem to be solved 
            ///         
            public object TargetProblem {
                get {
                    return _TargetProblem;
                }
            }
            
            /// 
            ///         Property getter for time when execution started
            ///         
            ///         :return datetime: time when execution started 
            ///         
            /// 
            ///         Property setter for time when execution started
            /// 
            ///         :param datetime value: time when execution started
            ///         
            public object execution_started {
                get {
                    return _execution_started;
                }
                set {
                    _execution_started = value;
                }
            }
            
            /// 
            ///         Property getter for time when execution ended
            ///         
            ///         :return datetime: time when execution ended 
            ///         
            /// 
            ///         Property setter for time when execution ended
            ///         
            ///         :param datetime value: time when execution ended
            ///         
            public object execution_ended {
                get {
                    return _execution_ended;
                }
                set {
                    _execution_ended = value;
                }
            }
            
            /// 
            ///         Property getter for the best solution obtained during metaheuristic execution
            ///         
            ///         :return: best solution so far 
            ///         return type TargetSolution
            ///         
            /// 
            ///         Property setter for the best solution so far
            ///         
            ///         :param TargetSolution value: best solution so far
            ///         
            public object best_solution {
                get {
                    return _best_solution;
                }
                set {
                    _best_solution = value;
                }
            }
            
            /// 
            ///         Property getter for the output control of the executing algorithm
            ///         
            ///         :return: output control of the executing algorithm
            ///         return type `OutputControl`
            ///         
            /// 
            ///         Property setter for the output control of the executing algorithm
            ///         
            ///         :param int value: `OutputControl`
            ///         
            public object OutputControl {
                get {
                    return _OutputControl;
                }
                set {
                    _OutputControl = value;
                }
            }
            
            /// 
            ///         Write headers(with field names) to output file, if necessary 
            ///         
            public virtual void write_output_headers_if_needed() {
                if (this.OutputControl.write_to_output) {
                    var output = this.OutputControl.output_file;
                    var f_hs = this.OutputControl.fields_headings;
                    var line = "";
                    foreach (var f_h in f_hs) {
                        output.write(f_h);
                        line += f_h;
                        output.write("\t");
                        line += "\t";
                    }
                    output.write("\n");
                    logger.info(line);
                }
            }
            
            /// 
            ///         Write data(with field values) to output file, if necessary 
            /// 
            ///         :param str step_name: name of the step when data should be written to output - have to be one of the following values: 'after_algorithm', 'before_algorithm', 'after_iteration', 'before_iteration', 'after_evaluation', 'before_evaluation', 'after_step_in_iteration', 'before_step_in_iteration'
            ///         :param str step_nameValue: what should be written to the output instead of step_name
            ///         
            public virtual object write_outputValues_if_needed(string step_name, string step_nameValue) {
                object s_data;
                if (this.OutputControl.write_to_output) {
                    var output = this.OutputControl.output_file;
                    var should_write = false;
                    if (step_name == "after_algorithm") {
                        should_write = true;
                    } else if (step_name == "before_algorithm") {
                        should_write = this.OutputControl.write_before_algorithm;
                    } else if (step_name == "after_iteration") {
                        should_write = this.OutputControl.write_after_iteration;
                    } else if (step_name == "before_iteration") {
                        should_write = this.OutputControl.write_before_iteration;
                    } else if (step_name == "after_evaluation") {
                        should_write = this.OutputControl.write_after_evaluation;
                    } else if (step_name == "before_evaluation") {
                        should_write = this.OutputControl.write_before_evaluation;
                    } else if (step_name == "after_step_in_iteration") {
                        should_write = this.OutputControl.write_after_step_in_iteration;
                    } else if (step_name == "before_step_in_iteration") {
                        should_write = this.OutputControl.write_before_step_in_iteration;
                    } else {
                        throw new ValueError("Supplied step name '" + step_name + "' is not valid.");
                    }
                    if (should_write) {
                        var line = "";
                        var fields_def = this.OutputControl.fields_definitions;
                        foreach (var f_def in fields_def) {
                            if (f_def != "") {
                                try {
                                    var data = eval(f_def);
                                    s_data = data.ToString();
                                    if (s_data == "step_name") {
                                        s_data = step_nameValue;
                                    }
                                } catch {
                                    s_data = "XXX";
                                }
                                output.write(s_data + "\t");
                                line += s_data + "\t";
                            }
                        }
                        output.write("\n");
                        logger.info(line);
                    }
                }
            }
            
            /// 
            ///         Copies function argument to become the best solution within metaheuristic instance and update info about time 
            ///         and iteration when the best solution is updated 
            /// 
            ///         :param TargetSolution solution: solution that is source for coping operation
            ///         
            public virtual object copy_to_best_solution(object solution) {
                _best_solution = solution.copy();
                _second_when_best_obtained = (datetime.now() - this.execution_started).total_seconds();
                _iteration_best_found = this.iteration;
            }
            
            /// 
            ///         String representation of the 'Algorithm' instance
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
                s = groupStart;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += "name=" + this.name + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += "TargetProblem=" + this.TargetProblem.stringRep(delimiter, indentation + 1, indentationSymbol, "{", "}") + delimiter;
                s += "_OutputControl=" + _OutputControl.stringRep(delimiter, indentation + 1, indentationSymbol, "{", "}") + delimiter;
                s += "execution_started=" + this.execution_started.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += "execution_ended=" + this.execution_ended.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += "best_solution=" + this.best_solution.stringRep(delimiter, indentation + 1, indentationSymbol, groupStart, groupEnd) + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += groupEnd;
                return s;
            }
            
            /// 
            ///         Method for optimization   
            ///         
            [abstractmethod]
            public virtual object optimize() {
                throw new NotImplemented();
            }
            
            /// 
            ///         String representation of the 'Algorithm' instance
            ///         
            ///         :return: string representation of the 'Algorithm' instance
            ///         return type str
            ///         
            [abstractmethod]
            public override string ToString() {
                return this.StringRep("|");
            }
            
            /// 
            ///         Representation of the 'Algorithm' instance
            ///         
            ///         :return: string representation of the 'Algorithm' instance
            ///         return type str
            ///         
            [abstractmethod]
            public virtual string _repr__() {
                return this.StringRep("\n");
            }
            
            /// 
            ///         Formatted 'Algorithm' instance
            ///         
            ///         :param str spec: format specification
            ///         :return: formatted 'Algorithm' instance
            ///         return type str
            ///         
            [abstractmethod]
            public virtual string _format__(string spec) {
                return this.StringRep("|");
            }
        }
    }
}
