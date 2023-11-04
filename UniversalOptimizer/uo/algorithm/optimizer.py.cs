//  
// The :mod:`~uo.algorithm.optimizer` module describes the class :class:`~uo.algorithm.Optimizer`.
// 
namespace algorithm {
    
    using Path = pathlib.Path;
    
    using sys;
    
    using deepcopy = copy.deepcopy;
    
    using datetime = datetime.datetime;
    
    using ABCMeta = abc.ABCMeta;
    
    using abstractmethod = abc.abstractmethod;
    
    using logger = uo.utils.logger.logger;
    
    using OutputControl = uo.algorithm.output_control.OutputControl;
    
    using TargetProblem = uo.target_problem.target_problem.TargetProblem;
    
    using TargetSolution = uo.target_solution.target_solution.TargetSolution;
    
    using System;
    
    using System.Linq;
    
    public static class optimizer {
        
        public static object directory = Path(@__file__).resolve();
        
        static optimizer() {
            sys.path.append(directory.parent);
        }
        
        // 
        //     This class describes Optimizer
        //     
        public class Optimizer
            : ABCMeta {
            
            public object @__best_solution;
            
            public object @__execution_ended;
            
            public object @__execution_started;
            
            public object @__iteration_best_found;
            
            public object @__name;
            
            public object @__output_control;
            
            public object @__second_when_best_obtained;
            
            public object @__target_problem;
            
            [abstractmethod]
            public Optimizer(string name, object output_control, object target_problem) {
                this.@__name = name;
                this.@__output_control = output_control;
                if (target_problem is TargetProblem) {
                    this.@__target_problem = target_problem.copy();
                } else {
                    this.@__target_problem = target_problem;
                }
                this.@__execution_started = null;
                this.@__execution_ended = null;
                this.@__best_solution = null;
            }
            
            // 
            //         Internal copy of the current optimizer
            // 
            //         :return:  new `Optimizer` instance with the same properties
            //         :rtype: :class:`uo.algorithm.Optimizer`
            //         
            [abstractmethod]
            public virtual void @__copy__() {
                var opt = deepcopy(this);
                return opt;
            }
            
            // 
            //         Copy the current optimizer
            // 
            //         :return:  new `Optimizer` instance with the same properties
            //         :rtype: :class:`uo.algorithm.Optimizer`
            //         
            [abstractmethod]
            public virtual void copy() {
                return this.@__copy__();
            }
            
            // 
            //         Property getter for the name of the optimizer
            //         
            //         :return: name of the algorithm instance 
            //         :rtype: str
            //         
            public object name {
                get {
                    return this.@__name;
                }
            }
            
            // 
            //         Property getter for the target problem to be solved
            //         
            //         :return TargetProblem: target problem to be solved 
            //         
            public object target_problem {
                get {
                    return this.@__target_problem;
                }
            }
            
            // 
            //         Property getter for time when execution started
            //         
            //         :return datetime: time when execution started 
            //         
            // 
            //         Property setter for time when execution started
            // 
            //         :param datetime value: time when execution started
            //         
            public object execution_started {
                get {
                    return this.@__execution_started;
                }
                set {
                    this.@__execution_started = value;
                }
            }
            
            // 
            //         Property getter for time when execution ended
            //         
            //         :return datetime: time when execution ended 
            //         
            // 
            //         Property setter for time when execution ended
            //         
            //         :param datetime value: time when execution ended
            //         
            public object execution_ended {
                get {
                    return this.@__execution_ended;
                }
                set {
                    this.@__execution_ended = value;
                }
            }
            
            // 
            //         Property getter for the best solution obtained during metaheuristic execution
            //         
            //         :return: best solution so far 
            //         :rtype: TargetSolution
            //         
            // 
            //         Property setter for the best solution so far
            //         
            //         :param TargetSolution value: best solution so far
            //         
            public object best_solution {
                get {
                    return this.@__best_solution;
                }
                set {
                    this.@__best_solution = value;
                }
            }
            
            // 
            //         Property getter for the output control of the executing algorithm
            //         
            //         :return: output control of the executing algorithm
            //         :rtype: `OutputControl`
            //         
            // 
            //         Property setter for the output control of the executing algorithm
            //         
            //         :param int value: `OutputControl`
            //         
            public object output_control {
                get {
                    return this.@__output_control;
                }
                set {
                    this.@__output_control = value;
                }
            }
            
            // 
            //         Write headers(with field names) to output file, if necessary 
            //         
            public virtual void write_output_headers_if_needed() {
                if (this.output_control.write_to_output) {
                    var output = this.output_control.output_file;
                    var f_hs = this.output_control.fields_headings;
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
            
            // 
            //         Write data(with field values) to output file, if necessary 
            // 
            //         :param str step_name: name of the step when data should be written to output - have to be one of the following values: 'after_algorithm', 'before_algorithm', 'after_iteration', 'before_iteration', 'after_evaluation', 'before_evaluation', 'after_step_in_iteration', 'before_step_in_iteration'
            //         :param str step_name_value: what should be written to the output instead of step_name
            //         
            public virtual object write_output_values_if_needed(string step_name, string step_name_value) {
                object s_data;
                if (this.output_control.write_to_output) {
                    var output = this.output_control.output_file;
                    var should_write = false;
                    if (step_name == "after_algorithm") {
                        should_write = true;
                    } else if (step_name == "before_algorithm") {
                        should_write = this.output_control.write_before_algorithm;
                    } else if (step_name == "after_iteration") {
                        should_write = this.output_control.write_after_iteration;
                    } else if (step_name == "before_iteration") {
                        should_write = this.output_control.write_before_iteration;
                    } else if (step_name == "after_evaluation") {
                        should_write = this.output_control.write_after_evaluation;
                    } else if (step_name == "before_evaluation") {
                        should_write = this.output_control.write_before_evaluation;
                    } else if (step_name == "after_step_in_iteration") {
                        should_write = this.output_control.write_after_step_in_iteration;
                    } else if (step_name == "before_step_in_iteration") {
                        should_write = this.output_control.write_before_step_in_iteration;
                    } else {
                        throw new ValueError("Supplied step name '" + step_name + "' is not valid.");
                    }
                    if (should_write) {
                        var line = "";
                        var fields_def = this.output_control.fields_definitions;
                        foreach (var f_def in fields_def) {
                            if (f_def != "") {
                                try {
                                    var data = eval(f_def);
                                    s_data = data.ToString();
                                    if (s_data == "step_name") {
                                        s_data = step_name_value;
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
            
            // 
            //         Copies function argument to become the best solution within metaheuristic instance and update info about time 
            //         and iteration when the best solution is updated 
            // 
            //         :param TargetSolution solution: solution that is source for coping operation
            //         
            public virtual object copy_to_best_solution(object solution) {
                this.@__best_solution = solution.copy();
                this.@__second_when_best_obtained = (datetime.now() - this.execution_started).total_seconds();
                this.@__iteration_best_found = this.iteration;
            }
            
            // 
            //         String representation of the 'Algorithm' instance
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
                s = group_start;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "name=" + this.name + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "target_problem=" + this.target_problem.string_rep(delimiter, indentation + 1, indentation_symbol, "{", "}") + delimiter;
                s += "__output_control=" + this.@__output_control.string_rep(delimiter, indentation + 1, indentation_symbol, "{", "}") + delimiter;
                s += "execution_started=" + this.execution_started.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "execution_ended=" + this.execution_ended.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "best_solution=" + this.best_solution.string_rep(delimiter, indentation + 1, indentation_symbol, group_start, group_end) + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += group_end;
                return s;
            }
            
            // 
            //         Method for optimization   
            //         
            [abstractmethod]
            public virtual object optimize() {
                throw new NotImplemented();
            }
            
            // 
            //         String representation of the 'Algorithm' instance
            //         
            //         :return: string representation of the 'Algorithm' instance
            //         :rtype: str
            //         
            [abstractmethod]
            public override string ToString() {
                return this.string_rep("|");
            }
            
            // 
            //         Representation of the 'Algorithm' instance
            //         
            //         :return: string representation of the 'Algorithm' instance
            //         :rtype: str
            //         
            [abstractmethod]
            public virtual string @__repr__() {
                return this.string_rep("\n");
            }
            
            // 
            //         Formatted 'Algorithm' instance
            //         
            //         :param str spec: format specification
            //         :return: formatted 'Algorithm' instance
            //         :rtype: str
            //         
            [abstractmethod]
            public virtual string @__format__(string spec) {
                return this.string_rep("|");
            }
        }
    }
}
