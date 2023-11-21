///  
/// The :mod:`~uo.Algorithm.metaheuristic.SingleSolutionMetaheuristic` module describes the class :class:`~uo.Algorithm.metaheuristic.SingleSolutionMetaheuristic.SingleSolutionMetaheuristic`.
/// 
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
    
    public static class SingleSolutionMetaheuristic {
        
        public static object directory = Path(_file__).resolve();
        
        static SingleSolutionMetaheuristic() {
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent);
        }
        
        /// 
        ///     This class represent single solution metaheuristic
        ///     
        public class SingleSolutionMetaheuristic
            : Metaheuristic, ABCMeta {
            
            private object _currentSolution;
            
            [abstractmethod]
            public SingleSolutionMetaheuristic(
                string name,
                object finish_control,
                int randomSeed,
                object additional_statistics_control,
                object OutputControl,
                object TargetProblem,
                object initial_solution)
                : base(finish_control: finish_control, randomSeed: randomSeed, additional_statistics_control: additional_statistics_control, OutputControl: OutputControl, TargetProblem: TargetProblem) {
                if (initial_solution is not null) {
                    if (initial_solution is TargetSolution) {
                        _currentSolution = initial_solution.copy();
                    } else {
                        _currentSolution = initial_solution;
                    }
                } else {
                    _currentSolution = null;
                }
            }
            
            /// 
            /// Internal copy of the current single solution metaheuristic
            /// 
            /// :return: new `SingleSolutionMetaheuristic` instance with the same properties
            /// return type `SingleSolutionMetaheuristic`
            /// 
            [abstractmethod]
            public virtual void _copy__() {
                var met = deepcopy(this);
                return met;
            }
            
            /// 
            /// Copy the current single solution metaheuristic
            /// 
            /// :return: new `SingleSolutionMetaheuristic` instance with the same properties
            /// return type `SingleSolutionMetaheuristic`
            /// 
            [abstractmethod]
            public virtual void copy() {
                return _copy__();
            }
            
            /// 
            /// Property getter for the current solution used during single solution metaheuristic execution
            /// 
            /// :return: instance of the :class:`uo.TargetSolution.TargetSolution` class subtype -- current solution of the problem 
            /// return type :class:`TargetSolution`        
            /// 
            /// 
            /// Property setter for the current solution used during single solution metaheuristic execution
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
            /// String representation of the SingleSolutionMetaheuristic instance
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
            public new string StringRep(
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
                s += "currentSolution=" + this.currentSolution.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += groupEnd;
                return s;
            }
            
            /// 
            /// String representation of the `SingleSolutionMetaheuristic` instance
            /// 
            /// :return: string representation of the `SingleSolutionMetaheuristic` instance
            /// return type str
            /// 
            [abstractmethod]
            public override string ToString() {
                var s = this.stringRep("|");
                return s;
            }
            
            /// 
            /// String representation of the `SingleSolutionMetaheuristic` instance
            /// 
            /// :return: string representation of the `SingleSolutionMetaheuristic` instance
            /// return type str
            /// 
            [abstractmethod]
            public virtual string _repr__() {
                var s = this.stringRep("\n");
                return s;
            }
            
            /// 
            /// Formatted the `SingleSolutionMetaheuristic` instance
            /// 
            /// :param str spec: format specification
            /// :return: formatted `Metaheuristic` instance
            /// return type str
            /// 
            [abstractmethod]
            public virtual string _format__(string spec) {
                return this.StringRep("|");
            }
        }
    }
}