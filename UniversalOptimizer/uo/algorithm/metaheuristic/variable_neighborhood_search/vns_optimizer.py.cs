//  
// ..  _py_vns_optimizer:
// 
// The :mod:`~uo.algorithm.metaheuristic.variable_neighborhood_search` contains class :class:`~.algorithm.metaheuristic.variable_neighborhood_search.VnsOptimizer`, that represents implements algorithm :ref:`VNS<Algorithm_Variable_Neighborhood_Search>`.
// 
namespace algorithm.metaheuristic.variable_neighborhood_search {
    
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
    
    using TargetProblem = uo.target_problem.target_problem.TargetProblem;
    
    using TargetSolution = uo.target_solution.target_solution.TargetSolution;
    
    using OutputControl = uo.algorithm.output_control.OutputControl;
    
    using FinishControl = uo.algorithm.metaheuristic.finish_control.FinishControl;
    
    using AdditionalStatisticsControl = uo.algorithm.metaheuristic.additional_statistics_control.AdditionalStatisticsControl;
    
    using SingleSolutionMetaheuristic = uo.algorithm.metaheuristic.single_solution_metaheuristic.SingleSolutionMetaheuristic;
    
    using ProblemSolutionVnsSupport = uo.algorithm.metaheuristic.variable_neighborhood_search.problem_solution_vns_support.ProblemSolutionVnsSupport;
    
    using System.Collections.Generic;
    
    using System;
    
    using System.Linq;
    
    public static class vns_optimizer {
        
        public static object directory = Path(@__file__).resolve();
        
        static vns_optimizer() {
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent);
            sys.path.append(directory.parent.parent.parent);
        }
        
        // 
        //         Instance of the class :class:`~uo.algorithm.metaheuristic.variable_neighborhood_search_constructor_parameters.
        //         VnsOptimizerConstructionParameters` represents constructor parameters for VNS algorithm.
        //         
        public class VnsOptimizerConstructionParameters {
            
            public object additional_statistics_control;
            
            public object finish_control;
            
            public object initial_solution;
            
            public object k_max;
            
            public object k_min;
            
            public object local_search_type;
            
            public object output_control;
            
            public object problem_solution_vns_support;
            
            public object random_seed;
            
            public object target_problem;
            
            public object finish_control = null;
            
            public object output_control = null;
            
            public object target_problem = null;
            
            public object initial_solution = null;
            
            public object problem_solution_vns_support = null;
            
            public object random_seed = null;
            
            public object additional_statistics_control = null;
            
            public object k_min = null;
            
            public object k_max = null;
            
            public object local_search_type = null;
        }
        
        // 
        //     Instance of the class :class:`~uo.algorithm.metaheuristic.variable_neighborhood_search.VnsOptimizer` encapsulate 
        //     :ref:`Algorithm_Variable_Neighborhood_Search` optimization algorithm.
        //     
        public class VnsOptimizer
            : SingleSolutionMetaheuristic {
            
            public object @__implemented_local_searches;
            
            public int @__k_current;
            
            public object @__k_max;
            
            public object @__k_min;
            
            public object @__local_search_type;
            
            public object @__ls_method;
            
            public object @__problem_solution_vns_support;
            
            public object @__shaking_method;
            
            public object current_solution;
            
            public int iteration;
            
            public VnsOptimizer(
                object finish_control,
                int random_seed,
                object additional_statistics_control,
                object output_control,
                object target_problem,
                object initial_solution,
                object problem_solution_vns_support,
                int k_min,
                int k_max,
                string local_search_type)
                : base(finish_control: finish_control, random_seed: random_seed, additional_statistics_control: additional_statistics_control, output_control: output_control, target_problem: target_problem, initial_solution: initial_solution) {
                this.@__local_search_type = local_search_type;
                if (problem_solution_vns_support is not null) {
                    if (problem_solution_vns_support is ProblemSolutionVnsSupport) {
                        this.@__problem_solution_vns_support = problem_solution_vns_support;
                        this.@__implemented_local_searches = new Dictionary<object, object> {
                            {
                                "local_search_best_improvement",
                                this.@__problem_solution_vns_support.local_search_best_improvement},
                            {
                                "local_search_first_improvement",
                                this.@__problem_solution_vns_support.local_search_first_improvement}};
                        if (!this.@__implemented_local_searches.Contains(this.@__local_search_type)) {
                            throw new ValueError("Value \'{}\' for VNS local_search_type is not supported".format(this.@__local_search_type));
                        }
                        this.@__ls_method = this.@__implemented_local_searches[this.@__local_search_type];
                        this.@__shaking_method = this.@__problem_solution_vns_support.shaking;
                    } else {
                        this.@__problem_solution_vns_support = problem_solution_vns_support;
                        this.@__implemented_local_searches = null;
                        this.@__ls_method = null;
                        this.@__shaking_method = null;
                    }
                } else {
                    this.@__problem_solution_vns_support = null;
                    this.@__implemented_local_searches = null;
                    this.@__ls_method = null;
                    this.@__shaking_method = null;
                }
                this.@__k_min = k_min;
                this.@__k_max = k_max;
                // current value of the vns parameter k
                this.@__k_current = null;
            }
            
            // 
            //         Additional constructor, that creates new instance of class :class:`~uo.algorithm.metaheuristic.variable_neighborhood_search.VnsOptimizer`. 
            // 
            //         :param `VnsOptimizerConstructionParameters` construction_tuple: tuple with all constructor parameters
            //         
            [classmethod]
            public static void from_construction_tuple(object cls, object construction_tuple) {
                return cls(construction_tuple.finish_control, construction_tuple.random_seed, construction_tuple.additional_statistics_control, construction_tuple.output_control, construction_tuple.target_problem, construction_tuple.initial_solution, construction_tuple.problem_solution_vns_support, construction_tuple.k_min, construction_tuple.k_max, construction_tuple.local_search_type);
            }
            
            // 
            //         Internal copy of the current instance of class :class:`~uo.algorithm.metaheuristic.variable_neighborhood_search.VnsOptimizer`
            // 
            //         :return: new instance of class :class:`~uo.algorithm.metaheuristic.variable_neighborhood_search.VnsOptimizer` with the same properties
            //         :rtype: :class:`uo.algorithm.metaheuristic.variable_neighborhood_search.VnsOptimizer`        
            //         
            public virtual void @__copy__() {
                var vns_opt = deepcopy(this);
                return vns_opt;
            }
            
            // 
            //         Copy the current instance of class :class:`~uo.algorithm.metaheuristic.variable_neighborhood_search.VnsOptimizer`
            // 
            //         :return: new instance of class :class:`~uo.algorithm.metaheuristic.variable_neighborhood_search.VnsOptimizer` with the same properties
            //         :rtype: :class:`uo.algorithm.metaheuristic.variable_neighborhood_search.VnsOptimizer`        
            //         
            public virtual void copy() {
                return this.@__copy__();
            }
            
            // 
            //         Property getter for the `k_min` parameter for VNS
            // 
            //         :return: `k_min` parameter for VNS 
            //         :rtype: int
            //         
            public object k_min {
                get {
                    return this.@__k_min;
                }
            }
            
            // 
            //         Property getter for the `k_max` parameter for VNS
            // 
            //         :return: k_max parameter for VNS 
            //         :rtype: int
            //         
            public object k_max {
                get {
                    return this.@__k_max;
                }
            }
            
            // 
            //         Initialization of the VNS algorithm
            //         
            public virtual object init() {
                this.@__k_current = this.k_min;
                this.current_solution.init_random(this.target_problem);
                this.current_solution.evaluate(this.target_problem);
                this.copy_to_best_solution(this.current_solution);
            }
            
            // 
            //         One iteration within main loop of the VNS algorithm
            //         
            public virtual object main_loop_iteration() {
                this.write_output_values_if_needed("before_step_in_iteration", "shaking");
                if (!this.@__shaking_method(this.@__k_current, this.target_problem, this.current_solution, this)) {
                    this.write_output_values_if_needed("after_step_in_iteration", "shaking");
                    return false;
                }
                this.write_output_values_if_needed("after_step_in_iteration", "shaking");
                this.iteration += 1;
                while (this.@__k_current <= this.@__k_max) {
                    this.write_output_values_if_needed("before_step_in_iteration", "ls");
                    this.current_solution = this.@__ls_method(this.@__k_current, this.target_problem, this.current_solution, this);
                    this.write_output_values_if_needed("after_step_in_iteration", "ls");
                    // update auxiliary structure that keeps all solution codes
                    this.additional_statistics_control.add_to_all_solution_codes_if_required(this.current_solution.string_representation());
                    this.additional_statistics_control.add_to_more_local_optima_if_required(this.current_solution.string_representation(), this.current_solution.fitness_value, this.best_solution.string_representation());
                    var new_is_better = this.is_first_solution_better(this.current_solution, this.best_solution);
                    var make_move = new_is_better;
                    if (new_is_better is null) {
                        if (this.current_solution.string_representation() == this.best_solution.string_representation()) {
                            make_move = false;
                        } else {
                            logger.debug("VnsOptimizer::main_loop_iteration: Same solution quality, generating random true with probability 0.5");
                            make_move = random() < 0.5;
                        }
                    }
                    if (make_move) {
                        this.copy_to_best_solution(this.current_solution);
                        this.@__k_current = this.k_min;
                    } else {
                        this.@__k_current += 1;
                    }
                }
            }
            
            // 
            //         String representation of the `VnsOptimizer` instance
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
                s += "k_min=" + this.k_min.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "k_max=" + this.k_max.ToString() + delimiter;
                s += delimiter;
                s += "__problem_solution_vns_support=" + this.@__problem_solution_vns_support.string_rep(delimiter, indentation + 1, indentation_symbol, group_start, group_end) + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "__max_local_optima=" + this.@__max_local_optima.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "__local_search_type=" + this.@__local_search_type.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += group_end;
                return s;
            }
            
            // 
            //         String representation of the `VnsOptimizer` instance
            // 
            //         :return: string representation of the `VnsOptimizer` instance
            //         :rtype: str
            //         
            public override string ToString() {
                var s = this.string_rep("|");
                return s;
            }
            
            // 
            //         String representation of the `VnsOptimizer` instance
            // 
            //         :return: string representation of the `VnsOptimizer` instance
            //         :rtype: str
            //         
            public virtual string @__repr__() {
                var s = this.string_rep("\n");
                return s;
            }
            
            // 
            //         Formatted the VnsOptimizer instance
            // 
            //         :param spec: str -- format specification 
            //         :return: formatted `VnsOptimizer` instance
            //         :rtype: str
            //         
            public virtual string @__format__(string spec) {
                return this.string_rep("\n", 0, "   ", "{", "}");
            }
        }
    }
}
