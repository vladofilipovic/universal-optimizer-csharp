///  
/// ..  _py_ones_count_problem:
/// 
/// The :mod:`~opt.single_objective.teaching.ones_count_problem.ones_count_problem` contains class :class:`~opt.single_objective.teaching.ones_count_problem.ones_count_problem.OnesCountProblem`, that represents :ref:`Problem_Max_Ones`.
/// 
namespace single_objective.teaching.ones_count_problem {
    
    using sys;
    
    using Path = pathlib.Path;
    
    using deepcopy = copy.deepcopy;
    
    using TargetProblem = uo.TargetProblem.TargetProblem.TargetProblem;
    
    using logger = uo.utils.logger.logger;
    
    using System;
    
    using System.Linq;
    
    public static class ones_count_problem {
        
        public static object directory = Path(_file__).resolve();
        
        static ones_count_problem() {
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent);
            sys.path.append(directory.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent.parent);
        }
        
        public class OnesCountProblem
            : TargetProblem {
            
            private object _dimension;
            
            public OnesCountProblem(int dim = null)
                : base(is_minimization: false) {
                _dimension = dim;
            }
            
            /// 
            ///         Additional constructor. Create new `OnesCountProblem` instance when dimension is specified
            /// 
            ///         :param int dimension: dimension of the problem
            ///         
            [classmethod]
            public static void from_dimension(object cls, int dimension) {
                return cls(dim: dimension);
            }
            
            /// 
            ///         Static function that read problem data from file
            /// 
            ///         :param str file_path: path of the file with problem data
            ///         :param str data_format: data format of the file
            /// 
            ///         :return: all data that describe problem
            ///         return type int
            ///         
            [classmethod]
            public static int _load_from_file__(object cls, string file_path, string data_format) {
                logger.debug("Load parameters: file path=" + file_path.ToString() + ", data format representation=" + data_format);
                if (data_format == "txt") {
                    var input_file = open(file_path, "r");
                    var text_line = input_file.readline().strip();
                    /// skip comments
                    while (text_line.startswith("///") || text_line.startswith(";")) {
                        text_line = input_file.readline();
                    }
                    var dimension = Convert.ToInt32(text_line.split()[0]);
                    return dimension;
                } else {
                    throw new ValueError("Value for data format \'{} \' is not supported".format(data_format));
                }
            }
            
            /// 
            ///         Additional constructor. Create new `OnesCountProblem` instance when input file and input format are specified
            /// 
            ///         :param str input_file_path: path of the input file with problem data
            ///         :param str input_format: format of the input
            ///         
            [classmethod]
            public static void from_input_file(object cls, string input_file_path, string input_format) {
                var dimension = OnesCountProblem._load_from_file__(input_file_path, input_format);
                return cls(dim: dimension);
            }
            
            /// 
            ///         Internal copy of the `OnesCountProblem` problem
            /// 
            ///         :return: new `OnesCountProblem` instance with the same properties
            ///         return type `OnesCountProblem`
            ///         
            public virtual void _copy__() {
                var pr = deepcopy(this);
                return pr;
            }
            
            /// 
            ///         Copy the `OnesCountProblem` problem
            /// 
            ///         :return: new `OnesCountProblem` instance with the same properties
            ///         return type OnesCountProblem
            ///         
            public virtual void copy() {
                return _copy__();
            }
            
            /// 
            ///         Property getter for dimension of the target problem
            /// 
            ///         :return: dimension of the target problem instance 
            ///         return type int
            ///         
            public object dimension {
                get {
                    return _dimension;
                }
            }
            
            /// 
            ///         String representation of the `MaxOneProblem` instance
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
            ///         return type str
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
                s += base.stringRep(delimiter, indentation, indentationSymbol, "", "");
                s += delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentationSymbol;
                }
                s += "dimension=" + this.dimension.ToString() + delimiter;
                s += groupEnd;
                return s;
            }
            
            /// 
            ///         String representation of the max ones problem structure
            /// 
            ///         :return: string representation of the max ones problem structure
            ///         return type str
            ///         
            public override string ToString() {
                return this.StringRep("|", 0, "", "{", "}");
            }
            
            /// 
            ///         Representation of the max ones problem instance
            ///         :return: str -- string representation of the max ones problem instance
            ///         
            public virtual string _repr__() {
                return this.StringRep("\n", 0, "   ", "{", "}");
            }
            
            /// 
            ///         Formatted the max ones problem instance
            ///         :param spec: str -- format specification
            ///         :return: str -- formatted max ones problem instance
            ///         
            public virtual string _format__(string spec) {
                return this.StringRep("|");
            }
        }
    }
}
