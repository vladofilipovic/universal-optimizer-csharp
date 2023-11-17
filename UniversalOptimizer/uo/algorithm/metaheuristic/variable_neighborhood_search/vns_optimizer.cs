///  
/// ..  _py_vns_optimizer:
/// 
/// The :mod:`~uo.Algorithm.metaheuristic.variable_neighborhood_search` contains class :class:`~.algorithm.metaheuristic.variable_neighborhood_search.VnsOptimizer`, that represents implements algorithm :ref:`VNS<Algorithm_Variable_Neighborhood_Search>`.
/// 
namespace uo.Algorithm.Metaheuristic.variable_neighborhood_search {
    
    using Path = pathlib.Path;
    
    using sys;
    
    using deepcopy = copy.deepcopy;
    
    using choice = random.choice;
    
    using random = random.random;
    
    using Bits = bitstring.Bits;
    
    using BitArray = bitstring.BitArray;
    
    using BitStream = bitstring.BitStream;
    
    using pack = bitstring.pack;
    
    using TypeVar = typing.TypeVar;
    
    using Generic = typing.Generic;
    
    using Generic = typing.Generic;
    
    using NamedTuple = typing.NamedTuple;
    
    using dataclass = dataclasses.dataclass;
    
    using logger = uo.utils.logger.logger;
    
    using TargetProblem = uo.TargetProblem.TargetProblem.TargetProblem;
    
    using TargetSolution = uo.TargetSolution.TargetSolution.TargetSolution;
    
    using OutputControl = uo.Algorithm.OutputControl.OutputControl;
    
    using FinishControl = uo.Algorithm.metaheuristic.finish_control.FinishControl;
    
    using AdditionalStatisticsControl = uo.Algorithm.metaheuristic.additional_statistics_control.AdditionalStatisticsControl;
    
    using SingleSolutionMetaheuristic = uo.Algorithm.metaheuristic.SingleSolutionMetaheuristic.SingleSolutionMetaheuristic;
    
    using ProblemSolutionVnsSupport = uo.Algorithm.metaheuristic.variable_neighborhood_search.problem_solution_vns_support.ProblemSolutionVnsSupport;
    
    using System.Collections.Generic;
    
    using System;
    
    using System.Linq;
    
    public static class vns_optimizer {
        
        public static object directory = Path(_file__).resolve();
        
        static vns_optimizer() {
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent);
            sys.path.append(directory.parent.parent.parent);
        }
        
        /// 
        ///         Instance of the class :class:`~uo.Algorithm.metaheuristic.variable_neighborhood_search_constructor_parameters.
        ///         VnsOptimizerConstructionParameters` represents constructor parameters for VNS algorithm.
        ///         
        public class VnsOptimizerConstructionParameters {
            
            public object additional_statistics_control;
            
            public object finish_control;
            
            public object initial_solution;
            
            public object k_max;
            
            public object k_min;
            
            public object local_search_type;
            
            public object OutputControl;
            
            public object problem_solution_vns_support;
            
            public object randomSeed;
            
            public object TargetProblem;
            
            public object finish_control = null;
            
            public object OutputControl = null;
            
            public object TargetProblem = null;
            
            public object initial_solution = null;
            
            public object problem_solution_vns_support = null;
            
            public object randomSeed = null;
            
            public object additional_statistics_control = null;
            
            public object k_min = null;
            
            public object k_max = null;
            
            public object local_search_type = null;
        }
        
        /// 
        ///     Instance of the class :class:`~uo.Algorithm.metaheuristic.variable_neighborhood_search.VnsOptimizer` encapsulate 
        ///     :ref:`Algorithm_Variable_Neighborhood_Search` optimization algorithm.
        ///     
        public class VnsOptimizer
            : SingleSolutionMetaheuristic {
            
            private object _implemented_local_searches;
            
            private int _k_current;
            
            private object _k_max;
            
            private object _k_min;
            
            private object _local_search_type;
            
            private object _ls_method;
            
            private object _problem_solution_vns_support;
            
            private object _shaking_method;
            
            public object currentSolution;
            
            public int iteration;
            
            public VnsOptimizer(
                object finish_control,
                int randomSeed,
                object additional_statistics_control,
                object OutputControl,
                object TargetProblem,
                object initial_solution,
                object problem_solution_vns_support,
                int k_min,
                int k_max,
                string local_search_type)
                : base(finish_control: finish_control, randomSeed: randomSeed, additional_statistics_control: additional_statistics_control, OutputControl: OutputControl, TargetProblem: TargetProblem, initial_solution: initial_solution) {
                _local_search_type = local_search_type;
                if (problem_solution_vns_support is not null) {
                    if (problem_solution_vns_support is ProblemSolutionVnsSupport) {
                        _problem_solution_vns_support = problem_solution_vns_support;
                        _implemented_local_searches = new Dictionary<object, object> {
                            {
                                "local_search_best_improvement",
                                _problem_solution_vns_support.local_search_best_improvement},
                            {
                                "local_search_first_improvement",
                                _problem_solution_vns_support.local_search_first_improvement}};
                        if (!_implemented_local_searches.Contains(_local_search_type)) {
                            throw new ValueError("Value \'{}\' for VNS local_search_type is not supported".format(_local_search_type));
                        }
                        _ls_method = _implemented_local_searches[_local_search_type];
                        _shaking_method = _problem_solution_vns_support.shaking;
                    } else {
                        _problem_solution_vns_support = problem_solution_vns_support;
                        _implemented_local_searches = null;
                        _ls_method = null;
                        _shaking_method = null;
                    }
                } else {
                    _problem_solution_vns_support = null;
                    _implemented_local_searches = null;
                    _ls_method = null;
                    _shaking_method = null;
                }
                _k_min = k_min;
                _k_max = k_max;
                /// current value of the vns parameter k
                _k_current = null;
            }
            
            /// 
            ///         Additional constructor, that creates new instance of class :class:`~uo.Algorithm.metaheuristic.variable_neighborhood_search.VnsOptimizer`. 
            /// 
            ///         :param `VnsOptimizerConstructionParameters` construction_tuple: tuple with all constructor parameters
            ///         
            [classmethod]
            public static void from_construction_tuple(object cls, object construction_tuple) {
                return cls(construction_tuple.finish_control, construction_tuple.randomSeed, construction_tuple.additional_statistics_control, construction_tuple.OutputControl, construction_tuple.TargetProblem, construction_tuple.initial_solution, construction_tuple.problem_solution_vns_support, construction_tuple.k_min, construction_tuple.k_max, construction_tuple.local_search_type);
            }
            
            /// 
            ///         Internal copy of the current instance of class :class:`~uo.Algorithm.metaheuristic.variable_neighborhood_search.VnsOptimizer`
            /// 
            ///         :return: new instance of class :class:`~uo.Algorithm.metaheuristic.variable_neighborhood_search.VnsOptimizer` with the same properties
            ///         return type :class:`uo.Algorithm.metaheuristic.variable_neighborhood_search.VnsOptimizer`        
            ///         
            public virtual void _copy__() {
                var vns_opt = deepcopy(this);
                return vns_opt;
            }
            
            /// 
            ///         Copy the current instance of class :class:`~uo.Algorithm.metaheuristic.variable_neighborhood_search.VnsOptimizer`
            /// 
            ///         :return: new instance of class :class:`~uo.Algorithm.metaheuristic.variable_neighborhood_search.VnsOptimizer` with the same properties
            ///         return type :class:`uo.Algorithm.metaheuristic.variable_neighborhood_search.VnsOptimizer`        
            ///         
            public virtual void copy() {
                return _copy__();
            }
            
            /// 
            ///         Property getter for the `k_min` parameter for VNS
            /// 
            ///         :return: `k_min` parameter for VNS 
            ///         return type int
            ///         
            public object k_min {
                get {
                    return _k_min;
                }
            }
            
            /// 
            ///         Property getter for the `k_max` parameter for VNS
            /// 
            ///         :return: k_max parameter for VNS 
            ///         return type int
            ///         
            public object k_max {
                get {
                    return _k_max;
                }
            }
            
            /// 
            ///         Initialization of the VNS algorithm
            ///         
            public virtual object init() {
                _k_current = this.k_min;
                this.currentSolution.InitRandom(this.TargetProblem);
                this.currentSolution.evaluate(this.TargetProblem);
                this.copy_to_best_solution(this.currentSolution);
            }
            
            /// 
            ///         One iteration within main loop of the VNS algorithm
            ///         
            public virtual object main_loop_iteration() {
                this.write_outputValues_if_needed("before_step_in_iteration", "shaking");
                if (!_shaking_method(_k_current, this.TargetProblem, this.currentSolution, this)) {
                    this.write_outputValues_if_needed("after_step_in_iteration", "shaking");
                    return false;
                }
                this.write_outputValues_if_needed("after_step_in_iteration", "shaking");
                this.iteration += 1;
                while (_k_current <= _k_max) {
                    this.write_outputValues_if_needed("before_step_in_iteration", "ls");
                    this.currentSolution = _ls_method(_k_current, this.TargetProblem, this.currentSolution, this);
                    this.write_outputValues_if_needed("after_step_in_iteration", "ls");
                    /// update auxiliary structure that keeps all solution codes
                    this.additional_statistics_control.add_to_all_solution_codes_if_required(this.currentSolution.stringRepresentation());
                    this.additional_statistics_control.add_to_more_local_optima_if_required(this.currentSolution.stringRepresentation(), this.currentSolution.fitnessValue, this.best_solution.stringRepresentation());
                    var new_is_better = this.is_first_solution_better(this.currentSolution, this.best_solution);
                    var make_move = new_is_better;
                    if (new_is_better is null) {
                        if (this.currentSolution.stringRepresentation() == this.best_solution.stringRepresentation()) {
                            make_move = false;
                        } else {
                            logger.debug("VnsOptimizer::main_loop_iteration: Same solution quality, generating random true with probability 0.5");
                            make_move = random() < 0.5;
                        }
                    }
                    if (make_move) {
                        this.copy_to_best_solution(this.currentSolution);
                        _k_current = this.k_min;
                    } else {
                        _k_current += 1;
                    }
                }
            }
            
            /// 
            ///         String representation of the `VnsOptimizer` instance
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
                s += groupStart;
                s = base.stringRep(delimiter, indentation, indentationSymbol, "", "");
                s += delimiter;
                s += "currentSolution=" + this.currentSolution.stringRep(delimiter, indentation + 1, indentationSymbol, groupStart, groupEnd) + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += "k_min=" + this.k_min.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += "k_max=" + this.k_max.ToString() + delimiter;
                s += delimiter;
                s += "_problem_solution_vns_support=" + _problem_solution_vns_support.stringRep(delimiter, indentation + 1, indentationSymbol, groupStart, groupEnd) + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += "_max_local_optima=" + _max_local_optima.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += "_local_search_type=" + _local_search_type.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += groupEnd;
                return s;
            }
            
            /// 
            ///         String representation of the `VnsOptimizer` instance
            /// 
            ///         :return: string representation of the `VnsOptimizer` instance
            ///         return type str
            ///         
            public override string ToString() {
                var s = this.stringRep("|");
                return s;
            }
            
            /// 
            ///         String representation of the `VnsOptimizer` instance
            /// 
            ///         :return: string representation of the `VnsOptimizer` instance
            ///         return type str
            ///         
            public virtual string _repr__() {
                var s = this.stringRep("\n");
                return s;
            }
            
            /// 
            ///         Formatted the VnsOptimizer instance
            /// 
            ///         :param spec: str -- format specification 
            ///         :return: formatted `VnsOptimizer` instance
            ///         return type str
            ///         
            public virtual string _format__(string spec) {
                return this.StringRep("\n", 0, "   ", "{", "}");
            }
        }
    }
}
