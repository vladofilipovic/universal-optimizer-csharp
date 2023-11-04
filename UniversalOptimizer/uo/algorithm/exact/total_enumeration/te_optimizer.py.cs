//  
// The :mod:`~uo.algorithm.exact.total_enumeration` module describes the class :class:`~uo.algorithm.exact.total_enumeration.TotalEnumeration`.
// 
namespace algorithm.exact.total_enumeration {
    
    using Path = pathlib.Path;
    
    using sys;
    
    using random = random.random;
    
    using randrange = random.randrange;
    
    using deepcopy = copy.deepcopy;
    
    using datetime = datetime.datetime;
    
    using TextIOWrapper = io.TextIOWrapper;
    
    using BitArray = bitstring.BitArray;
    
    using TypeVar = typing.TypeVar;
    
    using Generic = typing.Generic;
    
    using Generic = typing.Generic;
    
    using NamedTuple = typing.NamedTuple;
    
    using dataclass = dataclasses.dataclass;
    
    using logger = uo.utils.logger.logger;
    
    using TargetProblem = uo.target_problem.target_problem.TargetProblem;
    
    using TargetSolution = uo.target_solution.target_solution.TargetSolution;
    
    using OutputControl = uo.algorithm.output_control.OutputControl;
    
    using Algorithm = uo.algorithm.algorithm.Algorithm;
    
    using ProblemSolutionTeSupport = uo.algorithm.exact.total_enumeration.problem_solution_te_support.ProblemSolutionTeSupport;
    
    using System;
    
    using System.Linq;
    
    public static class te_optimizer {
        
        public static object directory = Path(@__file__).resolve();
        
        static te_optimizer() {
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent);
        }
        
        // 
        //     Instance of the class :class:`~uo.algorithm.exact.total_enumerations.TotalEnumerationConstructorParameters` represents constructor parameters for total enumeration algorithm.
        //     
        public class TeOptimizerConstructionParameters {
            
            public object initial_solution;
            
            public object output_control;
            
            public object problem_solution_te_support;
            
            public object target_problem;
            
            public object output_control = null;
            
            public object target_problem = null;
            
            public object initial_solution = null;
            
            public object problem_solution_te_support = null;
        }
        
        // 
        //     This class represent total enumeration algorithm
        //     
        public class TeOptimizer
            : Algorithm {
            
            public object @__can_progress_method;
            
            public object @__current_solution;
            
            public object @__iteration;
            
            public object @__problem_solution_te_support;
            
            public object @__progress_method;
            
            public object @__reset_method;
            
            public int evaluation;
            
            public datetime execution_ended;
            
            public datetime execution_started;
            
            public int iteration;
            
            public TeOptimizer(object output_control, object target_problem, object initial_solution, object problem_solution_te_support)
                : base(output_control: output_control, target_problem: target_problem) {
                // total enumeration support
                if (problem_solution_te_support is not null) {
                    if (problem_solution_te_support is ProblemSolutionTeSupport) {
                        this.@__problem_solution_te_support = problem_solution_te_support.copy();
                        this.@__reset_method = this.@__problem_solution_te_support.reset;
                        this.@__progress_method = this.@__problem_solution_te_support.progress;
                        this.@__can_progress_method = this.@__problem_solution_te_support.can_progress;
                    } else {
                        this.@__problem_solution_te_support = problem_solution_te_support;
                        this.@__reset_method = problem_solution_te_support.reset;
                        this.@__progress_method = problem_solution_te_support.progress;
                        this.@__can_progress_method = problem_solution_te_support.can_progress;
                    }
                } else {
                    this.@__problem_solution_te_support = null;
                    this.@__reset_method = null;
                    this.@__progress_method = null;
                    this.@__can_progress_method = null;
                }
                // current solution
                this.@__current_solution = initial_solution;
                this.@__iteration = null;
            }
            
            // 
            //         Additional constructor, that creates new instance of class :class:`~uo.algorithm.exact.te_optimizer.TeOptimizer`. 
            // 
            //         :param `TeOptimizerConstructionParameters` construction_tuple: tuple with all constructor parameters
            //         
            [classmethod]
            public static void from_construction_tuple(object cls, object construction_tuple) {
                return cls(construction_tuple.output_control, construction_tuple.target_problem, construction_tuple.initial_solution, construction_tuple.problem_solution_te_support);
            }
            
            // 
            //         Internal copy of the current total enumeration algorithm
            // 
            //         :return: new `TotalEnumeration` instance with the same properties
            //         :rtype: `TotalEnumeration`
            //         
            public virtual void @__copy__() {
                var tot = deepcopy(this);
                return tot;
            }
            
            // 
            //         Copy the current total enumeration algorithm
            //         
            //         :return: new `TotalEnumeration` instance with the same properties
            //         :rtype: `TotalEnumeration`
            //         
            public virtual void copy() {
                return this.@__copy__();
            }
            
            // 
            //         Property getter for the current solution used during VNS execution
            // 
            //         :return: instance of the :class:`uo.target_solution.TargetSolution` class subtype -- current solution of the problem 
            //         :rtype: :class:`TargetSolution`        
            //         
            // 
            //         Property setter for the current solution used during VNS execution
            // 
            //         :param value: the current solution
            //         :type value: :class:`TargetSolution`
            //         
            public object current_solution {
                get {
                    return this.@__current_solution;
                }
                set {
                    this.@__current_solution = value;
                }
            }
            
            // 
            //         Property getter for the current iteration during TE execution
            // 
            //         :return: current iteration number 
            //         :rtype: int       
            //         
            // 
            //         Property setter for the current iteration during TE execution
            // 
            //         :param value: the current iteration
            //         :type value: int
            //         
            public object iteration {
                get {
                    return this.@__iteration;
                }
                set {
                    this.@__iteration = value;
                }
            }
            
            // 
            //         Initialization of the total enumeration algorithm
            //         
            public virtual void init() {
                this.@__reset_method(this.target_problem, this.current_solution, this);
                this.write_output_values_if_needed("before_evaluation", "b_e");
                this.evaluation += 1;
                this.current_solution.evaluate(this.target_problem);
                this.write_output_values_if_needed("after_evaluation", "a_e");
                this.copy_to_best_solution(this.current_solution);
                this.iteration = 1;
            }
            
            public virtual object optimize() {
                this.execution_started = datetime.now();
                this.init();
                logger.debug("Overall number of evaluations: {}".format(this.@__problem_solution_te_support.overall_number_of_evaluations(this.target_problem, this.current_solution, this)));
                this.write_output_headers_if_needed();
                this.write_output_values_if_needed("before_algorithm", "b_a");
                while (true) {
                    this.write_output_values_if_needed("before_iteration", "b_i");
                    this.iteration += 1;
                    this.@__progress_method(this.target_problem, this.current_solution, this);
                    var new_is_better = this.is_first_solution_better(this.current_solution, this.best_solution);
                    if (new_is_better) {
                        this.copy_to_best_solution(this.current_solution);
                    }
                    this.write_output_values_if_needed("after_iteration", "a_i");
                    if (!this.@__can_progress_method(this.target_problem, this.current_solution, this)) {
                        break;
                    }
                }
                this.execution_ended = datetime.now();
                this.write_output_values_if_needed("after_algorithm", "a_a");
            }
            
            // 
            //         String representation of the 'TotalEnumeration' instance
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
                s += group_start;
                s = base.string_rep(delimiter, indentation, indentation_symbol, "", "");
                s += delimiter;
                s += "current_solution=" + this.current_solution.string_rep(delimiter, indentation + 1, indentation_symbol, group_start, group_end) + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += group_end;
                return s;
            }
            
            // 
            //         String representation of the 'TotalEnumeration' instance
            //         
            //         :return: string representation of the 'TotalEnumeration' instance
            //         :rtype: str
            //         
            public override string ToString() {
                return this.string_rep("|");
            }
            
            // 
            //         Representation of the 'TotalEnumeration' instance
            //         
            //         :return: string representation of the 'TotalEnumeration' instance
            //         :rtype: str
            //         
            public virtual string @__repr__() {
                return this.string_rep("\n");
            }
            
            // 
            //         Formatted 'TotalEnumeration' instance
            //         
            //         :param str spec: format specification
            //         :return: formatted 'TotalEnumeration' instance
            //         :rtype: str
            //         
            public virtual string @__format__(string spec) {
                return this.string_rep("|");
            }
        }
    }
}
