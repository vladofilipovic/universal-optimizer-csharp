namespace single_objective.teaching.function_one_variable_problem {
    
    using sys;
    
    using Path = pathlib.Path;
    
    using deepcopy = copy.deepcopy;
    
    using choice = random.choice;
    
    using random = random.random;
    
    using randint = random.randint;
    
    using TargetProblem = uo.TargetProblem.TargetProblem.TargetProblem;
    
    using QualityOfSolution = uo.TargetSolution.TargetSolution.QualityOfSolution;
    
    using TargetSolution = uo.TargetSolution.TargetSolution.TargetSolution;
    
    using logger = uo.utils.logger.logger;
    
    using FunctionOneVariableProblem = opt.single_objective.teaching.function_one_variable_problem.function_one_variable_problem.FunctionOneVariableProblem;
    
    using System.Collections.Generic;
    
    using System;
    
    using System.Linq;
    
    public static class function_one_variable_problem_binary_int_solution {
        
        public static object directory = Path(_file__).resolve();
        
        static function_one_variable_problem_binary_int_solution() {
            sys.path.append(directory);
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent.parent);
        }
        
        public class FunctionOneVariableProblemBinaryIntSolution
            : TargetSolution[intfloat] {
            
            private object _domain_from;
            
            private object _domain_to;
            
            private object _number_of_intervals;
            
            public object representation;
            
            public FunctionOneVariableProblemBinaryIntSolution(
                double domain_from,
                double domain_to,
                int number_of_intervals,
                int randomSeed = null,
                bool evaluationCacheIsUsed = false,
                int evaluationCacheMaxSize = 0,
                bool distanceCalculationCacheIsUsed = false,
                int distanceCalculationCacheMaxSize = 0)
                : base(randomSeed: randomSeed, fitnessValue: null, objectiveValue: null, isFeasible: false, evaluationCacheIsUsed: evaluationCacheIsUsed, evaluationCacheMaxSize: evaluationCacheMaxSize, distanceCalculationCacheIsUsed: distanceCalculationCacheIsUsed, distanceCalculationCacheMaxSize: distanceCalculationCacheMaxSize) {
                _domain_from = domain_from;
                _domain_to = domain_to;
                _number_of_intervals = number_of_intervals;
            }
            
            public virtual void _copy__() {
                var sol = base._copy__();
                sol.domain_from = this.domain_from;
                sol.domain_to = this.domain_to;
                sol.number_of_intervals = this.number_of_intervals;
                return sol;
            }
            
            public virtual object copy() {
                return _copy__();
            }
            
            public virtual object copy_to(object destination) {
                destination = _copy__();
            }
            
            public object domain_from {
                get {
                    return _domain_from;
                }
                set {
                    _domain_from = value;
                }
            }
            
            public object domain_to {
                get {
                    return _domain_to;
                }
                set {
                    _domain_to = value;
                }
            }
            
            public object number_of_intervals {
                get {
                    return _number_of_intervals;
                }
                set {
                    _number_of_intervals = value;
                }
            }
            
            public virtual void _make_to_beFeasible_helper__(object problem) {
                if (this.representation > this.number_of_intervals) {
                    this.representation = this.number_of_intervals;
                }
            }
            
            public virtual double argument(int representation) {
                return this.domain_from + representation * (this.domain_to - this.domain_from) / this.number_of_intervals;
            }
            
            public virtual object InitRandom(object problem) {
                this.representation = randint(0, this.number_of_intervals);
                _make_to_beFeasible_helper__(problem);
            }
            
            public virtual object InitFrom(int representation, object problem) {
                this.representation = representation;
            }
            
            public virtual object CalculateQualityDirectly(int representation, object problem) {
                var arg = this.argument(representation);
                var res = eval(problem.expression, new Dictionary<object, object> {
                    {
                        "x",
                        arg}});
                return QualityOfSolution(res, res, true);
            }
            
            public virtual int NativeRepresentation(string representationStr) {
                var ret = Convert.ToInt32(representationStr, 2);
                return ret;
            }
            
            public static double RepresentationDistanceDirectly(object solution_code_1, string solution_code_2) {
                var rep_1 = this.NativeRepresentation(solution_code_1);
                var rep_2 = this.NativeRepresentation(solution_code_2);
                var result = (rep_1 ^ rep_2).count(true);
                return result;
            }
            
            public virtual string StringRep(
                string delimiter = "\n",
                int indentation = 0,
                string indentationSymbol = "   ",
                string groupStart = "{",
                string groupEnd = "}") {
                var s = delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += groupStart;
                s += base.stringRep(delimiter, indentation, indentationSymbol, "", "");
                s += delimiter;
                s += delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += "stringRepresentation()=" + this.stringRepresentation().ToString();
                s += delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += groupEnd;
                return s;
            }
            
            public override string ToString() {
                return this.StringRep("\n", 0, "   ", "{", "}");
            }
            
            public virtual string _repr__() {
                return this.StringRep("\n", 0, "   ", "{", "}");
            }
            
            public virtual string _format__(string spec) {
                return this.StringRep("\n", 0, "   ", "{", "}");
            }
        }
    }
}
