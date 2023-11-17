
namespace uo.Algorithm {
    
    
    using uo.Algorithm;
    
    using uo.TargetProblem;
    
    using uo.TargetSolution;
    
    
    using System;
    
    using System.Linq;

    /// <summary>
    /// This class describes Algorithm
    /// </summary>
    /// <seealso cref="uo.Algorithm.Optimizer" />
    public abstract class Algorithm: Optimizer {
            
            private object _evaluation;
            
            private object _iteration;
            
            private int _iterationBestFound;
            
            public double _timeWhenBestObtained;
            
            public Algorithm(string name, object OutputControl, object TargetProblem)
                : base(OutputControl: OutputControl, TargetProblem: TargetProblem) {
                _evaluation = 0;
                _iteration = 0;
                _iterationBestFound = 0;
                _timeWhenBestObtained = 0.0;
            }
            
            /// 
            /// Internal copy of the current algorithm
            /// 
            /// :return:  new `Algorithm` instance with the same properties
            /// return type :class:`uo.Algorithm.Algorithm`
            /// 
            [abstractmethod]
            public virtual void _copy__() {
                var alg = deepcopy(this);
                return alg;
            }
            
            /// 
            /// Copy the current algorithm
            /// 
            /// :return:  new `Algorithm` instance with the same properties
            /// return type :class:`uo.Algorithm.Algorithm`
            /// 
            [abstractmethod]
            public virtual void copy() {
                return _copy__();
            }
            
            /// 
            /// Property getter for current number of evaluations during algorithm execution
            /// 
            /// :return: current number of evaluations 
            /// return type int
            /// 
            /// 
            /// Property setter for current number of evaluations
            /// 
            public object evaluation {
                get {
                    return _evaluation;
                }
                set {
                    _evaluation = value;
                }
            }
            
            /// 
            /// Property getter for the iteration of metaheuristic execution
            /// 
            /// :return: iteration
            /// return type int
            /// 
            /// 
            /// Property setter the iteration of metaheuristic execution
            /// 
            /// :param int value: iteration
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
            /// Initialization of the algorithm
            /// 
            [abstractmethod]
            public virtual object init() {
                throw new NotImplementedException();
            }
            
            /// 
            /// Checks if first solution is better than the second one
            /// 
            /// :param TargetSolution sol1: first solution
            /// :param TargetSolution sol2: second solution
            /// :return: `True` if first solution is better, `False` if first solution is worse, `None` if fitnesses of both 
            ///         solutions are equal
            /// return type bool
            /// 
            public virtual bool is_first_solution_better(object sol1, object sol2) {
                object fit2;
                object fit1;
                if (this.TargetProblem is null) {
                    throw new ValueError("Target problem have to be defined within metaheuristic.");
                }
                if (this.TargetProblem.is_minimization is null) {
                    throw new ValueError("Information if minimization or maximization is set within metaheuristic target problemhave to be defined.");
                }
                var is_minimization = this.TargetProblem.is_minimization;
                if (sol1 is null) {
                    fit1 = null;
                } else {
                    fit1 = sol1.CalculateQuality(this.TargetProblem).fitnessValue;
                }
                if (sol2 is null) {
                    fit2 = null;
                } else {
                    fit2 = sol2.CalculateQuality(this.TargetProblem).fitnessValue;
                }
                /// with fitness is better than without fitness
                if (fit1 is null) {
                    if (fit2 is not null) {
                        return false;
                    } else {
                        return null;
                    }
                } else if (fit2 is null) {
                    return true;
                }
                /// if better, return true
                if (is_minimization && fit1 < fit2 || !is_minimization && fit1 > fit2) {
                    return true;
                }
                /// if same fitness, return None
                if (fit1 == fit2) {
                    return null;
                }
                /// otherwise, return false
                return false;
            }
            
            /// 
            /// String representation of the 'Algorithm' instance
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
                s = groupStart;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += "name=" + this.name + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += "TargetProblem=" + this.TargetProblem.stringRep(delimiter, indentation + 1, indentationSymbol, "{", "}") + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += "_OutputControl=" + _OutputControl.stringRep(delimiter, indentation + 1, indentationSymbol, "{", "}") + delimiter;
                s += "_evaluation=" + _evaluation.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += "executionStarted=" + this.executionStarted.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += "executionEnded=" + this.executionEnded.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += groupEnd;
                return s;
            }
            
            /// 
            /// String representation of the 'Algorithm' instance
            /// 
            /// :return: string representation of the 'Algorithm' instance
            /// return type str
            /// 
            [abstractmethod]
            public override string ToString() {
                return this.StringRep("|");
            }
            
            /// 
            /// Representation of the 'Algorithm' instance
            /// 
            /// :return: string representation of the 'Algorithm' instance
            /// return type str
            /// 
            [abstractmethod]
            public virtual string _repr__() {
                return this.StringRep("\n");
            }
            
            /// 
            /// Formatted 'Algorithm' instance
            /// 
            /// :param str spec: format specification
            /// :return: formatted 'Algorithm' instance
            /// return type str
            /// 
            [abstractmethod]
            public virtual string _format__(string spec) {
                return this.StringRep("|");
            }
        }
    }
}
