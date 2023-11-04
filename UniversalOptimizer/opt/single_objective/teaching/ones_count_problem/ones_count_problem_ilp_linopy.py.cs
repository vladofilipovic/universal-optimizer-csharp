namespace single_objective.teaching.ones_count_problem {
    
    using sys;
    
    using Path = pathlib.Path;
    
    using dataclass = dataclasses.dataclass;
    
    using deepcopy = copy.deepcopy;
    
    using datetime = datetime.datetime;
    
    using xr = xarray;
    
    using Model = linopy.Model;
    
    using logger = uo.utils.logger.logger;
    
    using TargetProblem = uo.target_problem.target_problem.TargetProblem;
    
    using TargetSolution = uo.target_solution.target_solution.TargetSolution;
    
    using TargetSolutionVoidObjectStr = uo.target_solution.target_solution_void_object_str.TargetSolutionVoidObjectStr;
    
    using QualityOfSolution = uo.target_solution.target_solution.QualityOfSolution;
    
    using Optimizer = uo.algorithm.optimizer.Optimizer;
    
    using OutputControl = uo.algorithm.output_control.OutputControl;
    
    using OnesCountProblem = opt.single_objective.teaching.ones_count_problem.ones_count_problem.OnesCountProblem;
    
    using System.Collections.Generic;
    
    using System;
    
    using System.Linq;
    
    public static class ones_count_problem_ilp_linopy {
        
        public static object directory = Path(@__file__).resolve();
        
        static ones_count_problem_ilp_linopy() {
            sys.path.append(directory);
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent);
            sys.path.append(directory.parent.parent.parent);
        }
        
        // 
        //         Instance of the class :class:`OnesCountProblemIntegerLinearProgrammingSolverConstructionParameters` represents constructor parameters for max ones problem ILP solver.
        //         
        public class OnesCountProblemIntegerLinearProgrammingSolverConstructionParameters {
            
            public object output_control;
            
            public object target_problem;
            
            public object output_control = null;
            
            public object target_problem = null;
        }
        
        public class OnesCountProblemIntegerLinearProgrammingSolution
            : TargetSolutionVoidObjectStr {
            
            public object @__sol;
            
            public OnesCountProblemIntegerLinearProgrammingSolution(object sol) {
                this.@__sol = sol;
            }
            
            public virtual string string_representation() {
                return this.@__sol.ToString();
            }
        }
        
        public class OnesCountProblemIntegerLinearProgrammingSolver
            : Optimizer {
            
            public object @__model;
            
            public OnesCountProblemIntegerLinearProgrammingSolution best_solution;
            
            public int evaluation;
            
            public datetime execution_ended;
            
            public datetime execution_started;
            
            public int iteration;
            
            public OnesCountProblemIntegerLinearProgrammingSolver(object output_control, object problem)
                : base(output_control: output_control, target_problem: problem) {
                this.@__model = Model();
            }
            
            // 
            //         Additional constructor. Create new `OnesCountProblemIntegerLinearProgrammingSolver` instance from construction parameters
            // 
            //         :param `OnesCountProblemIntegerLinearProgrammingSolverConstructionParameters` construction_params: parameters for construction 
            //         
            [classmethod]
            public static void from_construction_tuple(object cls, object construction_params = null) {
                return cls(construction_tuple.output_control, construction_tuple.target_problem);
            }
            
            // 
            //         Internal copy of the current algorithm
            // 
            //         :return:  new `OnesCountProblemIntegerLinearProgrammingSolver` instance with the same properties
            //         :rtype: :class:`OnesCountProblemIntegerLinearProgrammingSolver`
            //         
            public virtual void @__copy__() {
                var alg = deepcopy(this);
                return alg;
            }
            
            // 
            //         Copy the current algorithm
            // 
            //         :return:  new `OnesCountProblemIntegerLinearProgrammingSolver` instance with the same properties
            //         :rtype: :class:``OnesCountProblemIntegerLinearProgrammingSolver``
            //         
            public virtual void copy() {
                return this.@__copy__();
            }
            
            // 
            //         Property getter for the ILP model
            //         
            //         :return: model of the problem 
            //         :rtype: `Model`
            //         
            public object model {
                get {
                    return this.@__model;
                }
            }
            
            // 
            //         Uses ILP model in order to solve OnesCountProblem
            //         
            public virtual object optimize() {
                this.iteration = -1;
                this.evaluation = -1;
                this.execution_started = datetime.now();
                var l = new List<object>();
                foreach (var i in Enumerable.Range(0, this.target_problem.dimension)) {
                    l.append(0);
                }
                var coords = xr.DataArray(l);
                var x = this.model.add_variables(binary: true, coords: new List<object> {
                    coords
                }, name: "x");
                //logger.debug(self.model.variables)
                if (this.target_problem.is_minimization) {
                    this.model.add_objective(x.sum(), sense: "min");
                } else {
                    this.model.add_objective(x.sum(), sense: "max");
                }
                this.model.solve();
                this.execution_ended = datetime.now();
                this.write_output_values_if_needed("after_algorithm", "a_a");
                this.best_solution = new OnesCountProblemIntegerLinearProgrammingSolution(this.model.solution.x);
                //logger.debug(self.model.solution.x)
            }
            
            // 
            //         String representation of the 'OnesCountProblemIntegerLinearProgrammingSolver' instance
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
                s += group_end;
                return s;
            }
            
            // 
            //         String representation of the 'OnesCountProblemIntegerLinearProgrammingSolver' instance
            //         
            //         :return: string representation of the 'OnesCountProblemIntegerLinearProgrammingSolver' instance
            //         :rtype: str
            //         
            public override string ToString() {
                return this.string_rep("|");
            }
            
            // 
            //         Representation of the 'OnesCountProblemIntegerLinearProgrammingSolver' instance
            //         
            //         :return: string representation of the 'OnesCountProblemIntegerLinearProgrammingSolver' instance
            //         :rtype: str
            //         
            public virtual string @__repr__() {
                return this.string_rep("\n");
            }
            
            // 
            //         Formatted 'OnesCountProblemIntegerLinearProgrammingSolver' instance
            //         
            //         :param str spec: format specification
            //         :return: formatted 'OnesCountProblemIntegerLinearProgrammingSolver' instance
            //         :rtype: str
            //         
            public virtual string @__format__(string spec) {
                return this.string_rep("|");
            }
        }
    }
}
