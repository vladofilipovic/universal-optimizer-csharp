///  
/// ..  _py_ones_count_problem_bit_array_solution:
/// 
/// The :mod:`~opt.single_objective.teaching.ones_count_problem.ones_count_problem_binary_bit_array_solution` contains class :class:`~opt.single_objective.teaching.ones_count_problem.ones_count_problem_binary_bit_array_solution.OnesCountProblemBinaryBitArraySolution`, that represents solution of the :ref:`Problem_Max_Ones`, where `BitArray` representation of the problem has been used.
/// 
namespace single_objective.teaching.ones_count_problem {
    
    using sys;
    
    using Path = pathlib.Path;
    
    using deepcopy = copy.deepcopy;
    
    using choice = random.choice;
    
    using random = random.random;
    
    using Bits = bitstring.Bits;
    
    using BitArray = bitstring.BitArray;
    
    using BitStream = bitstring.BitStream;
    
    using pack = bitstring.pack;
    
    using TargetProblem = uo.TargetProblem.TargetProblem.TargetProblem;
    
    using QualityOfSolution = uo.TargetSolution.TargetSolution.QualityOfSolution;
    
    using TargetSolution = uo.TargetSolution.TargetSolution.TargetSolution;
    
    using logger = uo.utils.logger.logger;
    
    using System;
    
    using System.Linq;
    
    public static class ones_count_problem_binary_bit_array_solution {
        
        public static object directory = Path(_file__).resolve();
        
        static ones_count_problem_binary_bit_array_solution() {
            sys.path.append(directory);
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent.parent);
        }
        
        public class OnesCountProblemBinaryBitArraySolution
            : TargetSolution[BitArraystr] {
            
            public object representation;
            
            public OnesCountProblemBinaryBitArraySolution(
                int randomSeed = null,
                bool evaluationCacheIsUsed = false,
                int evaluationCacheMaxSize = 0,
                bool distanceCalculationCacheIsUsed = false,
                int distanceCalculationCacheMaxSize = 0)
                : base(randomSeed: randomSeed, fitnessValue: null, objectiveValue: null, isFeasible: false, evaluationCacheIsUsed: evaluationCacheIsUsed, evaluationCacheMaxSize: evaluationCacheMaxSize, distanceCalculationCacheIsUsed: distanceCalculationCacheIsUsed, distanceCalculationCacheMaxSize: distanceCalculationCacheMaxSize) {
            }
            
            /// 
            /// Internal copy of the `OnesCountProblemBinaryBitArraySolution`
            /// 
            /// :return: new `OnesCountProblemBinaryBitArraySolution` instance with the same properties
            /// return type OnesCountProblemBinaryBitArraySolution
            /// 
            public virtual void _copy__() {
                var sol = base._copy__();
                if (this.representation is not null) {
                    sol.representation = BitArray(bin: this.representation.bin);
                } else {
                    sol.representation = null;
                }
                return sol;
            }
            
            /// 
            /// Copy the `OnesCountProblemBinaryBitArraySolution`
            /// 
            /// :return: new `OnesCountProblemBinaryBitArraySolution` instance with the same properties
            /// return type `OnesCountProblemBinaryBitArraySolution`
            /// 
            public virtual void copy() {
                return _copy__();
            }
            
            /// 
            /// Copy the `OnesCountProblemBinaryBitArraySolution` to the already existing destination `OnesCountProblemBinaryBitArraySolution`
            /// 
            /// :param `OnesCountProblemBinaryBitArraySolution` destination: destination `OnesCountProblemBinaryBitArraySolution`
            /// 
            public virtual object copy_to(object destination) {
                destination = _copy__();
            }
            
            /// 
            /// Argument of the target solution
            /// 
            /// :param representation: internal representation of the solution
            /// :type representation: `BitArray`
            /// :return: solution code
            /// return type str 
            /// 
            public virtual string argument(object representation) {
                return representation.bin;
            }
            
            /// 
            /// Random initialization of the solution
            /// 
            /// :param `TargetProblem` problem: problem which is solved by solution
            /// 
            public virtual object InitRandom(object problem) {
                ///logger.debug('Solution: ' + str(self))
                this.representation = BitArray(problem.dimension);
                foreach (var i in Enumerable.Range(0, problem.dimension)) {
                    if (random() > 0.5) {
                        this.representation[i] = true;
                    }
                }
            }
            
            /// 
            /// Initialization of the solution, by setting its native representation 
            /// 
            /// :param BitArray representation: representation that will be ste to solution
            /// :param `TargetProblem` problem: problem which is solved by solution
            /// 
            public virtual object InitFrom(object representation, object problem) {
                this.representation = BitArray(bin: representation.bin);
            }
            
            /// 
            /// Fitness calculation of the max ones binary BitArray solution
            /// 
            /// :param BitArray representation: native representation of solution whose fitness is calculated
            /// :param TargetProblem problem: problem that is solved
            /// :return: objective value, fitness value and feasibility of the solution instance  
            /// return type `QualityOfSolution`
            /// 
            public virtual object CalculateQualityDirectly(object representation, object problem) {
                var ones_count = representation.count(true);
                return QualityOfSolution(ones_count, ones_count, true);
            }
            
            /// 
            /// Obtain `BitArray` representation from string representation of the BitArray binary solution of the Max Ones problem 
            /// 
            /// :param str representationStr: solution's representation as string
            /// :return: solution's representation as BitArray
            /// return type `BitArray`
            /// 
            public virtual object NativeRepresentation(string representationStr) {
                var ret = BitArray(bin: representationStr);
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
