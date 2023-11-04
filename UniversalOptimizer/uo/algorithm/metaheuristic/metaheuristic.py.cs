//  
// The :mod:`~uo.algorithm.metaheuristic.metaheuristic` module describes the class :class:`~uo.algorithm.metaheuristic.metaheuristic.Metaheuristic`.
// 
namespace algorithm.metaheuristic {
    
    using Path = pathlib.Path;
    
    using sys;
    
    using random = random.random;
    
    using randrange = random.randrange;
    
    using deepcopy = copy.deepcopy;
    
    using datetime = datetime.datetime;
    
    using TextIOWrapper = io.TextIOWrapper;
    
    using BitArray = bitstring.BitArray;
    
    using ABCMeta = abc.ABCMeta;
    
    using abstractmethod = abc.abstractmethod;
    
    using TypeVar = typing.TypeVar;
    
    using Generic = typing.Generic;
    
    using Generic = typing.Generic;
    
    using logger = uo.utils.logger.logger;
    
    using TargetProblem = uo.target_problem.target_problem.TargetProblem;
    
    using TargetSolution = uo.target_solution.target_solution.TargetSolution;
    
    using OutputControl = uo.algorithm.output_control.OutputControl;
    
    using FinishControl = uo.algorithm.metaheuristic.finish_control.FinishControl;
    
    using Algorithm = uo.algorithm.algorithm.Algorithm;
    
    using AdditionalStatisticsControl = uo.algorithm.metaheuristic.additional_statistics_control.AdditionalStatisticsControl;
    
    using System;
    
    using System.Linq;
    
    public static class metaheuristic {
        
        public static object directory = Path(@__file__).resolve();
        
        static metaheuristic() {
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent);
        }
        
        // 
        //     This class represent metaheuristic
        //     
        public class Metaheuristic
            : Algorithm, ABCMeta {
            
            public object @__additional_statistics_control;
            
            public object @__finish_control;
            
            public object @__random_seed;
            
            public datetime execution_ended;
            
            public datetime execution_started;
            
            [abstractmethod]
            public Metaheuristic(
                string name,
                object finish_control,
                int random_seed,
                object additional_statistics_control,
                object output_control,
                object target_problem)
                : base(output_control: output_control, target_problem: target_problem) {
                this.@__finish_control = finish_control;
                if (random_seed is not null && random_seed is int && random_seed != 0) {
                    this.@__random_seed = random_seed;
                } else {
                    this.@__random_seed = randrange(sys.maxsize);
                }
                this.@__additional_statistics_control = additional_statistics_control;
            }
            
            // 
            //         Internal copy of the current metaheuristic
            // 
            //         :return: new `Metaheuristic` instance with the same properties
            //         :rtype: `Metaheuristic`
            //         
            [abstractmethod]
            public virtual void @__copy__() {
                var met = deepcopy(this);
                return met;
            }
            
            // 
            //         Copy the current metaheuristic
            //         
            //         :return: new `Metaheuristic` instance with the same properties
            //         :rtype: `Metaheuristic`
            //         
            [abstractmethod]
            public virtual void copy() {
                return this.@__copy__();
            }
            
            // 
            //         Property getter for the structure that controls finish criteria for metaheuristic execution
            //         
            //         :return: structure that controls finish criteria for metaheuristic execution 
            //         :rtype: `FinishControl`
            //         
            public object finish_control {
                get {
                    return this.@__finish_control;
                }
            }
            
            // 
            //         Property getter for the random seed used during metaheuristic execution
            //         
            //         :return: random seed 
            //         :rtype: int
            //         
            public object random_seed {
                get {
                    return this.@__random_seed;
                }
            }
            
            // 
            //         Property getter for the structure that controls keeping of the statistic during metaheuristic execution
            //         
            //         :return: structure that controls that controls keeping of the statistic during metaheuristic execution 
            //         :rtype: `AdditionalStatisticsControl`
            //         
            public object additional_statistics_control {
                get {
                    return this.@__additional_statistics_control;
                }
            }
            
            // 
            //         One iteration within main loop of the metaheuristic algorithm
            //         
            [abstractmethod]
            public virtual object main_loop_iteration() {
                throw new NotImplementedException();
            }
            
            // 
            //         Calculate time elapsed during execution of the metaheuristic algorithm 
            //         
            //         :return: elapsed time (in seconds)
            //         :rtype: float
            //         
            public virtual double elapsed_seconds() {
                var delta = datetime.now() - this.execution_started;
                return delta.total_seconds();
            }
            
            // 
            //         Main loop of the metaheuristic algorithm
            //         
            public virtual object main_loop() {
                while (!this.finish_control.is_finished(this.evaluation, this.iteration, this.elapsed_seconds())) {
                    this.write_output_values_if_needed("before_iteration", "b_i");
                    this.main_loop_iteration();
                    this.write_output_values_if_needed("after_iteration", "a_i");
                    logger.debug("Iteration: " + this.iteration.ToString() + ", Evaluations: " + this.evaluation.ToString() + ", Best solution objective: " + this.best_solution.objective_value.ToString() + ", Best solution fitness: " + this.best_solution.fitness_value.ToString() + ", Best solution: " + this.best_solution.string_representation().ToString());
                }
            }
            
            // 
            //         Executing optimization by the metaheuristic algorithm
            //         
            public virtual object optimize() {
                this.execution_started = datetime.now();
                this.init();
                this.write_output_headers_if_needed();
                this.write_output_values_if_needed("before_algorithm", "b_a");
                this.main_loop();
                this.execution_ended = datetime.now();
                this.write_output_values_if_needed("after_algorithm", "a_a");
            }
            
            // 
            //         String representation of the Metaheuristic instance
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
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "random_seed=" + this.random_seed.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "finish_control=" + this.finish_control.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "additional_statistics_control=" + this.additional_statistics_control.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "__iteration=" + this.@__iteration.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "__iteration_best_found=" + this.@__iteration_best_found.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "__second_when_best_obtained=" + this.@__second_when_best_obtained.ToString() + delimiter;
                if (this.execution_ended is not null && this.execution_started is not null) {
                    foreach (var i in Enumerable.Range(0, indentation - 0)) {
                        s += indentation_symbol;
                    }
                    s += "execution time=" + (this.execution_ended - this.execution_started).total_seconds().ToString() + delimiter;
                }
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += group_end;
                return s;
            }
            
            // 
            //         String representation of the `Metaheuristic` instance
            //         
            //         :return: string representation of the `Metaheuristic` instance
            //         :rtype: str
            //         
            [abstractmethod]
            public override string ToString() {
                var s = this.string_rep("|");
                return s;
            }
            
            // 
            //         String representation of the `Metaheuristic` instance
            //         
            //         :return: string representation of the `Metaheuristic` instance
            //         :rtype: str
            //         
            [abstractmethod]
            public virtual string @__repr__() {
                var s = this.string_rep("\n");
                return s;
            }
            
            // 
            //         Formatted the `Metaheuristic` instance
            //         
            //         :param str spec: format specification
            //         :return: formatted `Metaheuristic` instance
            //         :rtype: str
            //         
            [abstractmethod]
            public virtual string @__format__(string spec) {
                return this.string_rep("|");
            }
        }
    }
}
