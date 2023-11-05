///  
/// The :mod:`~uo.Algorithm.metaheuristic.metaheuristic` module describes the class :class:`~uo.Algorithm.metaheuristic.metaheuristic.Metaheuristic`.
/// 
namespace uo.Algorithm.Metaheuristic {
    
    
    using uo.TargetProblem;
    
    using uo.TargetSolution;
       
    using uo.Algorithm;
        
    using System;
    
    using System.Linq;
    

        
        /// 
        ///     This class represent metaheuristic
        ///     
        public abstract class Metaheuristic: Algorithm {
            
            public AdditionalStatisticsControl _additional_statistics_control;
            
            public FinishControl _finish_control;
            
            private object RandomSeed;
            
            public datetime execution_ended;
            
            public datetime execution_started;
            
            [abstractmethod]
            public Metaheuristic(
                string name,
                object finish_control,
                int randomSeed,
                object additional_statistics_control,
                object OutputControl,
                object TargetProblem)
                : base(OutputControl: OutputControl, TargetProblem: TargetProblem) {
                _finish_control = finish_control;
                if (randomSeed is not null && randomSeed is int && randomSeed != 0) {
                    RandomSeed = randomSeed;
                } else {
                    RandomSeed = randrange(sys.maxsize);
                }
                _additional_statistics_control = additional_statistics_control;
            }
            
            /// 
            ///         Internal copy of the current metaheuristic
            /// 
            ///         :return: new `Metaheuristic` instance with the same properties
            ///         :rtype: `Metaheuristic`
            ///         
            [abstractmethod]
            public virtual void _copy__() {
                var met = deepcopy(this);
                return met;
            }
            
            /// 
            ///         Copy the current metaheuristic
            ///         
            ///         :return: new `Metaheuristic` instance with the same properties
            ///         :rtype: `Metaheuristic`
            ///         
            [abstractmethod]
            public virtual void copy() {
                return _copy__();
            }
            
            /// 
            ///         Property getter for the structure that controls finish criteria for metaheuristic execution
            ///         
            ///         :return: structure that controls finish criteria for metaheuristic execution 
            ///         :rtype: `FinishControl`
            ///         
            public object finish_control {
                get {
                    return _finish_control;
                }
            }
            
            /// 
            ///         Property getter for the random seed used during metaheuristic execution
            ///         
            ///         :return: random seed 
            ///         :rtype: int
            ///         
            public object randomSeed {
                get {
                    return RandomSeed;
                }
            }
            
            /// 
            ///         Property getter for the structure that controls keeping of the statistic during metaheuristic execution
            ///         
            ///         :return: structure that controls that controls keeping of the statistic during metaheuristic execution 
            ///         :rtype: `AdditionalStatisticsControl`
            ///         
            public object additional_statistics_control {
                get {
                    return _additional_statistics_control;
                }
            }
            
            /// 
            ///         One iteration within main loop of the metaheuristic algorithm
            ///         
            [abstractmethod]
            public virtual object main_loop_iteration() {
                throw new NotImplementedException();
            }
            
            /// 
            ///         Calculate time elapsed during execution of the metaheuristic algorithm 
            ///         
            ///         :return: elapsed time (in seconds)
            ///         :rtype: float
            ///         
            public virtual double elapsed_seconds() {
                var delta = datetime.now() - this.execution_started;
                return delta.total_seconds();
            }
            
            /// 
            ///         Main loop of the metaheuristic algorithm
            ///         
            public virtual object main_loop() {
                while (!this.finish_control.is_finished(this.evaluation, this.iteration, this.elapsed_seconds())) {
                    this.write_outputValues_if_needed("before_iteration", "b_i");
                    this.main_loop_iteration();
                    this.write_outputValues_if_needed("after_iteration", "a_i");
                    logger.debug("Iteration: " + this.iteration.ToString() + ", Evaluations: " + this.evaluation.ToString() + ", Best solution objective: " + this.best_solution.objectiveValue.ToString() + ", Best solution fitness: " + this.best_solution.fitnessValue.ToString() + ", Best solution: " + this.best_solution.stringRepresentation().ToString());
                }
            }
            
            /// 
            ///         Executing optimization by the metaheuristic algorithm
            ///         
            public virtual object optimize() {
                this.execution_started = datetime.now();
                this.init();
                this.write_output_headers_if_needed();
                this.write_outputValues_if_needed("before_algorithm", "b_a");
                this.main_loop();
                this.execution_ended = datetime.now();
                this.write_outputValues_if_needed("after_algorithm", "a_a");
            }
            
            /// 
            ///         String representation of the Metaheuristic instance
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
                s += groupStart;
                s = base.stringRep(delimiter, indentation, indentationSymbol, "", "");
                s += delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += "randomSeed=" + this.randomSeed.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += "finish_control=" + this.finish_control.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += "additional_statistics_control=" + this.additional_statistics_control.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += "_iteration=" + _iteration.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += "_iteration_best_found=" + _iteration_best_found.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += "_second_when_best_obtained=" + _second_when_best_obtained.ToString() + delimiter;
                if (this.execution_ended is not null && this.execution_started is not null) {
                    foreach (var i in Enumerable.Range(0, indentation - 0)) {
                        s += indentationSymbol;
                    }
                    s += "execution time=" + (this.execution_ended - this.execution_started).total_seconds().ToString() + delimiter;
                }
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += groupEnd;
                return s;
            }
            
            /// 
            ///         String representation of the `Metaheuristic` instance
            ///         
            ///         :return: string representation of the `Metaheuristic` instance
            ///         :rtype: str
            ///         
            [abstractmethod]
            public override string ToString() {
                var s = this.stringRep("|");
                return s;
            }
            
            /// 
            ///         String representation of the `Metaheuristic` instance
            ///         
            ///         :return: string representation of the `Metaheuristic` instance
            ///         :rtype: str
            ///         
            [abstractmethod]
            public virtual string _repr__() {
                var s = this.stringRep("\n");
                return s;
            }
            
            /// 
            ///         Formatted the `Metaheuristic` instance
            ///         
            ///         :param str spec: format specification
            ///         :return: formatted `Metaheuristic` instance
            ///         :rtype: str
            ///         
            [abstractmethod]
            public virtual string _format__(string spec) {
                return this.StringRep("|");
            }
        }
    }
}
