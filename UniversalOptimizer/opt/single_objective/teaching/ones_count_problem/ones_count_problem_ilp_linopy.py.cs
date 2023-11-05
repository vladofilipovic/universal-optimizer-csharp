namespace single_objective.teaching.ones_count_problem {
    
    using sys;
    
    using Path = pathlib.Path;
    
    using dataclass = dataclasses.dataclass;
    
    using deepcopy = copy.deepcopy;
    
    using datetime = datetime.datetime;
    
    using xr = xarray;
    
    using Model = linopy.Model;
    
    using logger = uo.utils.logger.logger;
    
    using TargetProblem = uo.TargetProblem.TargetProblem.TargetProblem;
    
    using TargetSolution = uo.TargetSolution.TargetSolution.TargetSolution;
    
    using TargetSolutionVoidObjectStr = uo.TargetSolution.TargetSolutionVoidObjectStr.TargetSolutionVoidObjectStr;
    
    using QualityOfSolution = uo.TargetSolution.TargetSolution.QualityOfSolution;
    
    using Optimizer = uo.Algorithm.optimizer.Optimizer;
    
    using OutputControl = uo.Algorithm.OutputControl.OutputControl;
    
    using OnesCountProblem = opt.single_objective.teaching.ones_count_problem.ones_count_problem.OnesCountProblem;
    
    using System.Collections.Generic;
    
    using System;
    
    using System.Linq;
    
    public static class ones_count_problem_ilp_linopy {
        
        public static object directory = Path(_file__).resolve();
        
        static ones_count_problem_ilp_linopy() {
            sys.path.append(directory);
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent);
            sys.path.append(directory.parent.parent.parent);
        }
        
        /// 
        ///         Instance of the class :class:`OnesCountProblemIntegerLinearProgrammingSolverConstructionParameters` represents constructor parameters for max ones problem ILP solver.
        ///         
        public class OnesCountProblemIntegerLinearProgrammingSolverConstructionParameters {
            
            public object OutputControl;
            
            public object TargetProblem;
            
            public object OutputControl = null;
            
            public object TargetProblem = null;
        }
        
        public class OnesCountProblemIntegerLinearProgrammingSolution
            : TargetSolutionVoidObjectStr {
            
            private object _sol;
            
            public OnesCountProblemIntegerLinearProgrammingSolution(object sol) {
                _sol = sol;
            }
            
            public virtual string StringRepresentation() {
                return _sol.ToString();
            }
        }
        
        public class OnesCountProblemIntegerLinearProgrammingSolver
            : Optimizer {
            
            private object _model;
            
            public OnesCountProblemIntegerLinearProgrammingSolution best_solution;
            
            public int evaluation;
            
            public datetime execution_ended;
            
            public datetime execution_started;
            
            public int iteration;
            
            public OnesCountProblemIntegerLinearProgrammingSolver(object OutputControl, object problem)
                : base(OutputControl: OutputControl, TargetProblem: problem) {
                _model = Model();
            }
            
            /// 
            ///         Additional constructor. Create new `OnesCountProblemIntegerLinearProgrammingSolver` instance from construction parameters
            /// 
            ///         :param `OnesCountProblemIntegerLinearProgrammingSolverConstructionParameters` construction_params: parameters for construction 
            ///         
            [classmethod]
            public static void from_construction_tuple(object cls, object construction_params = null) {
                return cls(construction_tuple.OutputControl, construction_tuple.TargetProblem);
            }
            
            /// 
            ///         Internal copy of the current algorithm
            /// 
            ///         :return:  new `OnesCountProblemIntegerLinearProgrammingSolver` instance with the same properties
            ///         :rtype: :class:`OnesCountProblemIntegerLinearProgrammingSolver`
            ///         
            public virtual void _copy__() {
                var alg = deepcopy(this);
                return alg;
            }
            
            /// 
            ///         Copy the current algorithm
            /// 
            ///         :return:  new `OnesCountProblemIntegerLinearProgrammingSolver` instance with the same properties
            ///         :rtype: :class:``OnesCountProblemIntegerLinearProgrammingSolver``
            ///         
            public virtual void copy() {
                return _copy__();
            }
            
            /// 
            ///         Property getter for the ILP model
            ///         
            ///         :return: model of the problem 
            ///         :rtype: `Model`
            ///         
            public object model {
                get {
                    return _model;
                }
            }
            
            /// 
            ///         Uses ILP model in order to solve OnesCountProblem
            ///         
            public virtual object optimize() {
                this.iteration = -1;
                this.evaluation = -1;
                this.execution_started = datetime.now();
                var l = new List<object>();
                foreach (var i in Enumerable.Range(0, this.TargetProblem.dimension)) {
                    l.append(0);
                }
                var coords = xr.DataArray(l);
                var x = this.model.add_variables(binary: true, coords: new List<object> {
                    coords
                }, name: "x");
                ///logger.debug(self.model.variables)
                if (this.TargetProblem.is_minimization) {
                    this.model.add_objective(x.sum(), sense: "min");
                } else {
                    this.model.add_objective(x.sum(), sense: "max");
                }
                this.model.solve();
                this.execution_ended = datetime.now();
                this.write_outputValues_if_needed("after_algorithm", "a_a");
                this.best_solution = new OnesCountProblemIntegerLinearProgrammingSolution(this.model.solution.x);
                ///logger.debug(self.model.solution.x)
            }
            
            /// 
            ///         String representation of the 'OnesCountProblemIntegerLinearProgrammingSolver' instance
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
                s += groupEnd;
                return s;
            }
            
            /// 
            ///         String representation of the 'OnesCountProblemIntegerLinearProgrammingSolver' instance
            ///         
            ///         :return: string representation of the 'OnesCountProblemIntegerLinearProgrammingSolver' instance
            ///         :rtype: str
            ///         
            public override string ToString() {
                return this.StringRep("|");
            }
            
            /// 
            ///         Representation of the 'OnesCountProblemIntegerLinearProgrammingSolver' instance
            ///         
            ///         :return: string representation of the 'OnesCountProblemIntegerLinearProgrammingSolver' instance
            ///         :rtype: str
            ///         
            public virtual string _repr__() {
                return this.StringRep("\n");
            }
            
            /// 
            ///         Formatted 'OnesCountProblemIntegerLinearProgrammingSolver' instance
            ///         
            ///         :param str spec: format specification
            ///         :return: formatted 'OnesCountProblemIntegerLinearProgrammingSolver' instance
            ///         :rtype: str
            ///         
            public virtual string _format__(string spec) {
                return this.StringRep("|");
            }
        }
    }
}