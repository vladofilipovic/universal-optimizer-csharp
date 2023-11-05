namespace uo.Algorithm.Metaheuristic {
    
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
    
    using TargetProblem = uo.TargetProblem.TargetProblem.TargetProblem;
    
    using TargetSolution = uo.TargetSolution.TargetSolution.TargetSolution;
    
    using OutputControl = uo.Algorithm.OutputControl.OutputControl;
    
    using FinishControl = uo.Algorithm.metaheuristic.finish_control.FinishControl;
    
    using AdditionalStatisticsControl = uo.Algorithm.metaheuristic.additional_statistics_control.AdditionalStatisticsControl;
    
    using Metaheuristic = uo.Algorithm.metaheuristic.metaheuristic.Metaheuristic;
    
    using System;
    
    using System.Linq;
    
    public static class PopulationBasedMetaheuristic {
        
        public static object directory = Path(_file__).resolve();
        
        static PopulationBasedMetaheuristic() {
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent);
        }
        
        /// 
        ///     This class represent population metaheuristic
        ///     
        public class PopulationBasedMetaheuristic
            : Metaheuristic, ABCMeta {
            
            private object _currentSolution;
            
            private object _currentSolutions;
            
            [abstractmethod]
            public PopulationBasedMetaheuristic(
                string name,
                object finish_control,
                int randomSeed,
                object additional_statistics_control,
                object OutputControl,
                object TargetProblem,
                object initial_solutions)
                : base(finish_control: finish_control, randomSeed: randomSeed, additional_statistics_control: additional_statistics_control, OutputControl: OutputControl, TargetProblem: TargetProblem) {
                if (initial_solutions is not null) {
                    if (initial_solution is list[TargetSolution]) {
                        _currentSolutions = initial_solution.copy();
                    } else {
                        _currentSolution = initial_solution;
                    }
                } else {
                    _currentSolution = null;
                }
            }
            
            /// 
            ///         Internal copy of the current population based metaheuristic
            /// 
            ///         :return: new `PopulationBasedMetaheuristic` instance with the same properties
            ///         :rtype: `PopulationBasedMetaheuristic`
            ///         
            [abstractmethod]
            public virtual object _copy__() {
                var met = deepcopy(this);
                return met;
            }
            
            /// 
            ///         Copy the current population based metaheuristic
            ///         
            ///         :return: new `PopulationBasedMetaheuristic` instance with the same properties
            ///         :rtype: `PopulationBasedMetaheuristic`
            ///         
            [abstractmethod]
            public virtual object copy() {
                return _copy__();
            }
            
            /// 
            ///         Property getter for the current solutions used during population based metaheuristic execution
            /// 
            ///         :return: list of the :class:`uo.TargetSolution.TargetSolution` class subtype -- current solutions of the problem 
            ///         :rtype: list[TargetSolution]        
            ///         
            /// 
            ///         Property setter for the population of current solutions used during population-based metaheuristic execution
            /// 
            ///         :param value: the current solutions
            ///         :type value: list[TargetSolution]
            ///         
            public object currentSolutions {
                get {
                    return _currentSolutions;
                }
                set {
                    _currentSolutions = value;
                }
            }
            
            /// 
            ///         String representation of the SingleSolutionMetaheuristic instance
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
                s += "currentSolutions=" + this.currentSolutions.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += groupEnd;
                return s;
            }
            
            /// 
            ///         String representation of the `SingleSolutionMetaheuristic` instance
            ///         
            ///         :return: string representation of the `SingleSolutionMetaheuristic` instance
            ///         :rtype: str
            ///         
            [abstractmethod]
            public override string ToString() {
                var s = this.stringRep("|");
                return s;
            }
            
            /// 
            ///         String representation of the `SingleSolutionMetaheuristic` instance
            ///         
            ///         :return: string representation of the `SingleSolutionMetaheuristic` instance
            ///         :rtype: str
            ///         
            [abstractmethod]
            public virtual string _repr__() {
                var s = this.stringRep("\n");
                return s;
            }
            
            /// 
            ///         Formatted the `SingleSolutionMetaheuristic` instance
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
