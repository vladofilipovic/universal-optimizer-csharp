//  
// The :mod:`~uo.algorithm.algorithm` module describes the class :class:`~uo.algorithm.Algorithm`.
// 
namespace algorithm {
    
    using Path = pathlib.Path;
    
    using sys;
    
    using deepcopy = copy.deepcopy;
    
    using datetime = datetime.datetime;
    
    using ABCMeta = abc.ABCMeta;
    
    using abstractmethod = abc.abstractmethod;
    
    using logger = uo.utils.logger.logger;
    
    using OutputControl = uo.algorithm.output_control.OutputControl;
    
    using TargetProblem = uo.target_problem.target_problem.TargetProblem;
    
    using TargetSolution = uo.target_solution.target_solution.TargetSolution;
    
    using Optimizer = uo.algorithm.optimizer.Optimizer;
    
    using System;
    
    using System.Linq;
    
    public static class algorithm {
        
        public static object directory = Path(@__file__).resolve();
        
        static algorithm() {
            sys.path.append(directory.parent);
        }
        
        // 
        //     This class describes Algorithm
        //     
        public class Algorithm
            : Optimizer, ABCMeta {
            
            public object @__evaluation;
            
            public object @__iteration;
            
            public int @__iteration_best_found;
            
            public double @__second_when_best_obtained;
            
            [abstractmethod]
            public Algorithm(string name, object output_control, object target_problem)
                : base(output_control: output_control, target_problem: target_problem) {
                this.@__evaluation = 0;
                this.@__iteration = 0;
                this.@__iteration_best_found = 0;
                this.@__second_when_best_obtained = 0.0;
            }
            
            // 
            //         Internal copy of the current algorithm
            // 
            //         :return:  new `Algorithm` instance with the same properties
            //         :rtype: :class:`uo.algorithm.Algorithm`
            //         
            [abstractmethod]
            public virtual void @__copy__() {
                var alg = deepcopy(this);
                return alg;
            }
            
            // 
            //         Copy the current algorithm
            // 
            //         :return:  new `Algorithm` instance with the same properties
            //         :rtype: :class:`uo.algorithm.Algorithm`
            //         
            [abstractmethod]
            public virtual void copy() {
                return this.@__copy__();
            }
            
            // 
            //         Property getter for current number of evaluations during algorithm execution
            //         
            //         :return: current number of evaluations 
            //         :rtype: int
            //         
            // 
            //         Property setter for current number of evaluations
            //         
            public object evaluation {
                get {
                    return this.@__evaluation;
                }
                set {
                    this.@__evaluation = value;
                }
            }
            
            // 
            //         Property getter for the iteration of metaheuristic execution
            //         
            //         :return: iteration
            //         :rtype: int
            //         
            // 
            //         Property setter the iteration of metaheuristic execution
            //         
            //         :param int value: iteration
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
            //         Initialization of the algorithm
            //         
            [abstractmethod]
            public virtual object init() {
                throw new NotImplementedException();
            }
            
            // 
            //         Checks if first solution is better than the second one
            // 
            //         :param TargetSolution sol1: first solution
            //         :param TargetSolution sol2: second solution
            //         :return: `True` if first solution is better, `False` if first solution is worse, `None` if fitnesses of both 
            //                 solutions are equal
            //         :rtype: bool
            //         
            public virtual bool is_first_solution_better(object sol1, object sol2) {
                object fit2;
                object fit1;
                if (this.target_problem is null) {
                    throw new ValueError("Target problem have to be defined within metaheuristic.");
                }
                if (this.target_problem.is_minimization is null) {
                    throw new ValueError("Information if minimization or maximization is set within metaheuristic target problemhave to be defined.");
                }
                var is_minimization = this.target_problem.is_minimization;
                if (sol1 is null) {
                    fit1 = null;
                } else {
                    fit1 = sol1.calculate_quality(this.target_problem).fitness_value;
                }
                if (sol2 is null) {
                    fit2 = null;
                } else {
                    fit2 = sol2.calculate_quality(this.target_problem).fitness_value;
                }
                // with fitness is better than without fitness
                if (fit1 is null) {
                    if (fit2 is not null) {
                        return false;
                    } else {
                        return null;
                    }
                } else if (fit2 is null) {
                    return true;
                }
                // if better, return true
                if (is_minimization && fit1 < fit2 || !is_minimization && fit1 > fit2) {
                    return true;
                }
                // if same fitness, return None
                if (fit1 == fit2) {
                    return null;
                }
                // otherwise, return false
                return false;
            }
            
            // 
            //         String representation of the 'Algorithm' instance
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
                s = group_start;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "name=" + this.name + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "target_problem=" + this.target_problem.string_rep(delimiter, indentation + 1, indentation_symbol, "{", "}") + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "__output_control=" + this.@__output_control.string_rep(delimiter, indentation + 1, indentation_symbol, "{", "}") + delimiter;
                s += "__evaluation=" + this.@__evaluation.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "execution_started=" + this.execution_started.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "execution_ended=" + this.execution_ended.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += group_end;
                return s;
            }
            
            // 
            //         String representation of the 'Algorithm' instance
            //         
            //         :return: string representation of the 'Algorithm' instance
            //         :rtype: str
            //         
            [abstractmethod]
            public override string ToString() {
                return this.string_rep("|");
            }
            
            // 
            //         Representation of the 'Algorithm' instance
            //         
            //         :return: string representation of the 'Algorithm' instance
            //         :rtype: str
            //         
            [abstractmethod]
            public virtual string @__repr__() {
                return this.string_rep("\n");
            }
            
            // 
            //         Formatted 'Algorithm' instance
            //         
            //         :param str spec: format specification
            //         :return: formatted 'Algorithm' instance
            //         :rtype: str
            //         
            [abstractmethod]
            public virtual string @__format__(string spec) {
                return this.string_rep("|");
            }
        }
    }
}
