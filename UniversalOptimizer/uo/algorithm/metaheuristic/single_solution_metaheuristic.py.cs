//  
// The :mod:`~uo.algorithm.metaheuristic.single_solution_metaheuristic` module describes the class :class:`~uo.algorithm.metaheuristic.single_solution_metaheuristic.SingleSolutionMetaheuristic`.
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
    
    using AdditionalStatisticsControl = uo.algorithm.metaheuristic.additional_statistics_control.AdditionalStatisticsControl;
    
    using Metaheuristic = uo.algorithm.metaheuristic.metaheuristic.Metaheuristic;
    
    using System;
    
    using System.Linq;
    
    public static class single_solution_metaheuristic {
        
        public static object directory = Path(@__file__).resolve();
        
        static single_solution_metaheuristic() {
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent);
        }
        
        // 
        //     This class represent single solution metaheuristic
        //     
        public class SingleSolutionMetaheuristic
            : Metaheuristic, ABCMeta {
            
            public object @__current_solution;
            
            [abstractmethod]
            public SingleSolutionMetaheuristic(
                string name,
                object finish_control,
                int random_seed,
                object additional_statistics_control,
                object output_control,
                object target_problem,
                object initial_solution)
                : base(finish_control: finish_control, random_seed: random_seed, additional_statistics_control: additional_statistics_control, output_control: output_control, target_problem: target_problem) {
                if (initial_solution is not null) {
                    if (initial_solution is TargetSolution) {
                        this.@__current_solution = initial_solution.copy();
                    } else {
                        this.@__current_solution = initial_solution;
                    }
                } else {
                    this.@__current_solution = null;
                }
            }
            
            // 
            //         Internal copy of the current single solution metaheuristic
            // 
            //         :return: new `SingleSolutionMetaheuristic` instance with the same properties
            //         :rtype: `SingleSolutionMetaheuristic`
            //         
            [abstractmethod]
            public virtual void @__copy__() {
                var met = deepcopy(this);
                return met;
            }
            
            // 
            //         Copy the current single solution metaheuristic
            //         
            //         :return: new `SingleSolutionMetaheuristic` instance with the same properties
            //         :rtype: `SingleSolutionMetaheuristic`
            //         
            [abstractmethod]
            public virtual void copy() {
                return this.@__copy__();
            }
            
            // 
            //         Property getter for the current solution used during single solution metaheuristic execution
            // 
            //         :return: instance of the :class:`uo.target_solution.TargetSolution` class subtype -- current solution of the problem 
            //         :rtype: :class:`TargetSolution`        
            //         
            // 
            //         Property setter for the current solution used during single solution metaheuristic execution
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
            //         String representation of the SingleSolutionMetaheuristic instance
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
                s += "current_solution=" + this.current_solution.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += group_end;
                return s;
            }
            
            // 
            //         String representation of the `SingleSolutionMetaheuristic` instance
            //         
            //         :return: string representation of the `SingleSolutionMetaheuristic` instance
            //         :rtype: str
            //         
            [abstractmethod]
            public override string ToString() {
                var s = this.string_rep("|");
                return s;
            }
            
            // 
            //         String representation of the `SingleSolutionMetaheuristic` instance
            //         
            //         :return: string representation of the `SingleSolutionMetaheuristic` instance
            //         :rtype: str
            //         
            [abstractmethod]
            public virtual string @__repr__() {
                var s = this.string_rep("\n");
                return s;
            }
            
            // 
            //         Formatted the `SingleSolutionMetaheuristic` instance
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
