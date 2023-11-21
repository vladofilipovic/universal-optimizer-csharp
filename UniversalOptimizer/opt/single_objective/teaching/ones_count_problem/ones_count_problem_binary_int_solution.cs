///  
/// .. _py_ones_count_problem_int_solution:
/// 
/// The :mod:`~opt.single_objective.teaching.ones_count_problem.ones_count_problemBinaryIntSolution` contains class :class:`~opt.single_objective.teaching.ones_count_problem.ones_count_problemBinaryIntSolution.OnesCountProblemBinaryIntSolution`, that represents solution of the :ref:`Problem_Max_Ones`, where `int` representation of the problem has been used.
/// 
namespace single_objective.teaching.ones_count_problem {
    
    using sys;
    
    using Path = pathlib.Path;
    
    using deepcopy = copy.deepcopy;
    
    using choice = random.choice;
    
    using randint = random.randint;
    
    using TargetProblem = uo.TargetProblem.TargetProblem.TargetProblem;
    
    using QualityOfSolution = uo.TargetSolution.TargetSolution.QualityOfSolution;
    
    using TargetSolution = uo.TargetSolution.TargetSolution.TargetSolution;
    
    using logger = uo.utils.logger.logger;
    
    using System;
    
    using System.Linq;
    
    public static class ones_count_problemBinaryIntSolution {
        
        public static object directory = Path(_file__).resolve();
        
        static ones_count_problemBinaryIntSolution() {
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent);
            sys.path.append(directory.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent.parent);
        }
        
        public class OnesCountProblemBinaryIntSolution
            : TargetSolution[intstr] {
            
            public object representation;
            
            public OnesCountProblemBinaryIntSolution(
                int randomSeed = null,
                bool evaluationCacheIsUsed = false,
                int evaluationCacheMaxSize = 0,
                bool distanceCalculationCacheIsUsed = false,
                int distanceCalculationCacheMaxSize = 0)
                : base(randomSeed: randomSeed, fitnessValue: null, objectiveValue: null, isFeasible: false, evaluationCacheIsUsed: evaluationCacheIsUsed, evaluationCacheMaxSize: evaluationCacheMaxSize, distanceCalculationCacheIsUsed: distanceCalculationCacheIsUsed, distanceCalculationCacheMaxSize: distanceCalculationCacheMaxSize) {
            }
            
            /// 
            /// Internal copy of the `OnesCountProblemBinaryIntSolution`
            /// 
            /// :return: new `OnesCountProblemBinaryIntSolution` instance with the same properties
            /// return type OnesCountProblemBinaryIntSolution
            /// 
            public virtual void _copy__() {
                var sol = deepcopy(this);
                return sol;
            }
            
            /// 
            /// Copy the `OnesCountProblemBinaryIntSolution`
            /// 
            /// :return: new `OnesCountProblemBinaryIntSolution` instance with the same properties
            /// return type `OnesCountProblemBinaryIntSolution`
            /// 
            public virtual void copy() {
                return _copy__();
            }
            
            /// 
            /// Copy the `OnesCountProblemBinaryIntSolution` to the already existing destination `OnesCountProblemBinaryIntSolution`
            /// 
            /// :param `OnesCountProblemBinaryIntSolution` destination: destination `OnesCountProblemBinaryIntSolution`
            /// 
            public virtual object copy_to(object destination) {
                destination = _copy__();
            }
            
            /// 
            /// Helper function that modifies representation to be feasible
            /// 
            /// :param `TargetProblem` problem: problem which is solved by solution
            /// 
            public virtual void _make_to_beFeasible_helper__(object problem) {
                var mask = ~0;
                mask <<= 32 - problem.dimension;
                mask = mask % 0x100000000 >> 32 - problem.dimension;
                this.representation &= mask;
            }
            
            /// 
            /// Argument of the target solution for specific problem
            /// 
            /// :param representation: internal representation of the solution
            /// :type representation: int
            /// :return: solution representation as string
            /// return type str 
            /// 
            public virtual string argument(int representation) {
                return bin(representation);
            }
            
            /// 
            /// Random initialization of the solution
            /// 
            /// :param `TargetProblem` problem: problem which is solved by solution
            /// 
            public virtual object InitRandom(object problem) {
                if (problem.dimension is null) {
                    throw new ValueError("Problem dimension should not be None!");
                }
                if (problem.dimension <= 0) {
                    throw new ValueError("Problem dimension should be positive!");
                }
                if (problem.dimension >= 32) {
                    throw new ValueError("Problem dimension should be less than 32!");
                }
                this.representation = randint(0, 2 ^ problem.dimension - 1);
                _make_to_beFeasible_helper__(problem);
            }
            
            /// 
            /// Initialization of the solution, by setting its native representation 
            /// 
            /// :param int representation: representation that will be ste to solution
            /// :param `TargetProblem` problem: problem which is solved by solution
            /// 
            public virtual object InitFrom(int representation, object problem) {
                this.representation = representation;
            }
            
            /// 
            /// Fitness calculation of the max ones binary int solution
            /// 
            /// :param int representation: native representation of the solution whose fitness, objective and feasibility is calculated
            /// :param TargetProblem problem: problem that is solved
            /// :return: objective value, fitness value and feasibility of the solution instance  
            /// return type `QualityOfSolution`
            /// 
            public virtual object CalculateQualityDirectly(int representation, object problem) {
                var ones_count = representation.bit_count();
                return QualityOfSolution(ones_count, ones_count, true);
            }
            
            /// 
            /// Obtain `int` representation from string representation of the integer binary solution of the Max Ones problem 
            /// 
            /// :param str representationStr: solution's representation as string
            /// :return: solution's representation as int
            /// return type int
            /// 
            public virtual int NativeRepresentation(string representationStr) {
                var ret = Convert.ToInt32(representationStr, 2);
                return ret;
            }
            
            /// 
            /// Calculating distance between two solutions determined by its code
            /// 
            /// :param str solution_code_1: solution code for the first solution
            /// :param str solution_code_2: solution code for the second solution
            /// :return: distance between two solutions represented by its code
            /// return type float
            /// 
            public static double RepresentationDistanceDirectly(object solution_code_1, string solution_code_2) {
                var rep_1 = this.NativeRepresentation(solution_code_1);
                var rep_2 = this.NativeRepresentation(solution_code_2);
                var result = (rep_1 ^ rep_2).count(true);
                return result;
            }
            
            /// 
            /// String representation of the solution instance
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
                s += "stringRepresentation()=" + this.stringRepresentation();
                s += delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += groupEnd;
                return s;
            }
            
            /// 
            /// String representation of the solution instance
            /// 
            /// :return: string representation of the solution instance
            /// return type str
            /// 
            public override string ToString() {
                return this.StringRep("\n", 0, "   ", "{", "}");
            }
            
            /// 
            /// Representation of the solution instance
            /// 
            /// :return: string representation of the solution instance
            /// return type str
            /// 
            public virtual string _repr__() {
                return this.StringRep("\n", 0, "   ", "{", "}");
            }
            
            /// 
            /// Formatted the solution instance
            /// 
            /// :param str spec: format specification
            /// :return: formatted solution instance
            /// return type str
            /// 
            public virtual string _format__(string spec) {
                return this.StringRep("\n", 0, "   ", "{", "}");
            }
        }
    }
}
