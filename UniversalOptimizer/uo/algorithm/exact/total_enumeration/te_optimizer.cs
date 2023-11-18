///  
/// The :mod:`~uo.Algorithm.exact.total_enumeration` module describes the class :class:`~uo.Algorithm.exact.total_enumeration.TotalEnumeration`.
/// 
namespace uo.Algorithm.exact.total_enumeration {
    
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
    
    using TargetProblem = uo.TargetProblem.TargetProblem.TargetProblem;
    
    using TargetSolution = uo.TargetSolution.TargetSolution.TargetSolution;
    
    using OutputControl = uo.Algorithm.OutputControl.OutputControl;
    
    using Algorithm = uo.Algorithm.algorithm.Algorithm;
    
    using ProblemSolutionTeSupport = uo.Algorithm.exact.total_enumeration.problem_solution_te_support.ProblemSolutionTeSupport;
    
    using System;
    
    using System.Linq;
    
    public static class te_optimizer {
        
        public static object directory = Path(_file__).resolve();
        
        static te_optimizer() {
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent);
        }
        
        /// 
        ///     Instance of the class :class:`~uo.Algorithm.exact.total_enumerations.TotalEnumerationConstructorParameters` represents constructor parameters for total enumeration algorithm.
        ///     
        public class TeOptimizerConstructionParameters {
            
            public object initial_solution;
            
            public object OutputControl;
            
            public object problem_solution_te_support;
            
            public object TargetProblem;
            
            public object OutputControl = null;
            
            public object TargetProblem = null;
            
            public object initial_solution = null;
            
            public object problem_solution_te_support = null;
        }
        
        /// 
        ///     This class represent total enumeration algorithm
        ///     
        public class TeOptimizer
            : Algorithm {
            
            private object _can_progress_method;
            
            private object _currentSolution;
            
            private object _iteration;
            
            private object _problem_solution_te_support;
            
            private object _progress_method;
            
            private object _reset_method;
            
            public int evaluation;
            
            public datetime executionEnded;
            
            public datetime executionStarted;
            
            public int iteration;
            
            public TeOptimizer(object OutputControl, object TargetProblem, object initial_solution, object problem_solution_te_support)
                : base(OutputControl: OutputControl, TargetProblem: TargetProblem) {
                /// total enumeration support
                if (problem_solution_te_support is not null) {
                    if (problem_solution_te_support is ProblemSolutionTeSupport) {
                        _problem_solution_te_support = problem_solution_te_support.copy();
                        _reset_method = _problem_solution_te_support.reset;
                        _progress_method = _problem_solution_te_support.progress;
                        _can_progress_method = _problem_solution_te_support.can_progress;
                    } else {
                        _problem_solution_te_support = problem_solution_te_support;
                        _reset_method = problem_solution_te_support.reset;
                        _progress_method = problem_solution_te_support.progress;
                        _can_progress_method = problem_solution_te_support.can_progress;
                    }
                } else {
                    _problem_solution_te_support = null;
                    _reset_method = null;
                    _progress_method = null;
                    _can_progress_method = null;
                }
                /// current solution
                _currentSolution = initial_solution;
                _iteration = null;
            }
            
            /// 
            /// Additional constructor, that creates new instance of class :class:`~uo.Algorithm.exact.te_optimizer.TeOptimizer`. 
            /// 
            /// :param `TeOptimizerConstructionParameters` construction_tuple: tuple with all constructor parameters
            /// 
            [classmethod]
            public static void from_construction_tuple(object cls, object construction_tuple) {
                return cls(construction_tuple.OutputControl, construction_tuple.TargetProblem, construction_tuple.initial_solution, construction_tuple.problem_solution_te_support);
            }
            
            /// 
            /// Internal copy of the current total enumeration algorithm
            /// 
            /// :return: new `TotalEnumeration` instance with the same properties
            /// return type `TotalEnumeration`
            /// 
            public virtual void _copy__() {
                var tot = deepcopy(this);
                return tot;
            }
            
            /// 
            /// Copy the current total enumeration algorithm
            /// 
            /// :return: new `TotalEnumeration` instance with the same properties
            /// return type `TotalEnumeration`
            /// 
            public virtual void copy() {
                return _copy__();
            }
            
            /// 
            /// Property getter for the current solution used during VNS execution
            /// 
            /// :return: instance of the :class:`uo.TargetSolution.TargetSolution` class subtype -- current solution of the problem 
            /// return type :class:`TargetSolution`        
            /// 
            /// 
            /// Property setter for the current solution used during VNS execution
            /// 
            /// :param value: the current solution
            /// :type value: :class:`TargetSolution`
            /// 
            public object currentSolution {
                get {
                    return _currentSolution;
                }
                set {
                    _currentSolution = value;
                }
            }
            
            /// 
            /// Property getter for the current iteration during TE execution
            /// 
            /// :return: current iteration number 
            /// return type int       
            /// 
            /// 
            /// Property setter for the current iteration during TE execution
            /// 
            /// :param value: the current iteration
            /// :type value: int
            /// 
            public object iteration {
                get {
                    return _iteration;
                }
                set {
                    _iteration = value;
                }
            }
            
            /// 
            /// Initialization of the total enumeration algorithm
            /// 
            public virtual void init() {
                _reset_method(this.TargetProblem, this.currentSolution, this);
                this.WriteOutputValuesIfNeeded("beforeEvaluation", "b_e");
                this.evaluation += 1;
                this.currentSolution.evaluate(this.TargetProblem);
                this.WriteOutputValuesIfNeeded("afterEvaluation", "a_e");
                this.CopyToBestSolution(this.currentSolution);
                this.iteration = 1;
            }
            
            public virtual object optimize() {
                this.executionStarted = datetime.now();
                this.init();
                logger.debug("Overall number of evaluations: {}".format(_problem_solution_te_support.overall_number_of_evaluations(this.TargetProblem, this.currentSolution, this)));
                this.writeOutputHeadersIfNeeded();
                this.WriteOutputValuesIfNeeded("beforeAlgorithm", "b_a");
                while (true) {
                    this.WriteOutputValuesIfNeeded("beforeIteration", "b_i");
                    this.iteration += 1;
                    _progress_method(this.TargetProblem, this.currentSolution, this);
                    var new_is_better = this.is_first_solution_better(this.currentSolution, this.bestSolution);
                    if (new_is_better) {
                        this.CopyToBestSolution(this.currentSolution);
                    }
                    this.WriteOutputValuesIfNeeded("afterIteration", "a_i");
                    if (!_can_progress_method(this.TargetProblem, this.currentSolution, this)) {
                        break;
                    }
                }
                this.executionEnded = datetime.now();
                this.WriteOutputValuesIfNeeded("afterAlgorithm", "a_a");
            }
            
            /// 
            /// String representation of the 'TotalEnumeration' instance
            /// 
            /// :param delimiter: delimiter between fields
            /// :type delimiter: str
            /// :param indentation: level of indentation
            /// :type indentation: int, optional, default value 0
            /// :param indentationSymbol: indentation symbol
            /// :type indentationSymbol: str, optional, default value ''
            /// :param groupStart: group start string 
            /// :type groupStart: str, optional, default value '{'
            /// :param groupEnd: group end string 
            /// :type groupEnd: str, optional, default value '}'
            /// :return: string representation of instance that controls output
            /// return type str
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
                s += groupStart;
                s = base.stringRep(delimiter, indentation, indentationSymbol, "", "");
                s += delimiter;
                s += "currentSolution=" + this.currentSolution.stringRep(delimiter, indentation + 1, indentationSymbol, groupStart, groupEnd) + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += groupEnd;
                return s;
            }
            
            /// 
            /// String representation of the 'TotalEnumeration' instance
            /// 
            /// :return: string representation of the 'TotalEnumeration' instance
            /// return type str
            /// 
            public override string ToString() {
                return this.StringRep("|");
            }
            
            /// 
            /// Representation of the 'TotalEnumeration' instance
            /// 
            /// :return: string representation of the 'TotalEnumeration' instance
            /// return type str
            /// 
            public virtual string _repr__() {
                return this.StringRep("\n");
            }
            
            /// 
            /// Formatted 'TotalEnumeration' instance
            /// 
            /// :param str spec: format specification
            /// :return: formatted 'TotalEnumeration' instance
            /// return type str
            /// 
            public virtual string _format__(string spec) {
                return this.StringRep("|");
            }
        }
    }
}
