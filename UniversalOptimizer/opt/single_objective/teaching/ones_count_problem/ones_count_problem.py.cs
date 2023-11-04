//  
// ..  _py_ones_count_problem:
// 
// The :mod:`~opt.single_objective.teaching.ones_count_problem.ones_count_problem` contains class :class:`~opt.single_objective.teaching.ones_count_problem.ones_count_problem.OnesCountProblem`, that represents :ref:`Problem_Max_Ones`.
// 
namespace single_objective.teaching.ones_count_problem {
    
    using sys;
    
    using Path = pathlib.Path;
    
    using deepcopy = copy.deepcopy;
    
    using TargetProblem = uo.target_problem.target_problem.TargetProblem;
    
    using logger = uo.utils.logger.logger;
    
    using System;
    
    using System.Linq;
    
    public static class ones_count_problem {
        
        public static object directory = Path(@__file__).resolve();
        
        static ones_count_problem() {
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent);
            sys.path.append(directory.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent.parent);
        }
        
        public class OnesCountProblem
            : TargetProblem {
            
            public object @__dimension;
            
            public OnesCountProblem(int dim = null)
                : base(is_minimization: false) {
                this.@__dimension = dim;
            }
            
            // 
            //         Additional constructor. Create new `OnesCountProblem` instance when dimension is specified
            // 
            //         :param int dimension: dimension of the problem
            //         
            [classmethod]
            public static void from_dimension(object cls, int dimension) {
                return cls(dim: dimension);
            }
            
            // 
            //         Static function that read problem data from file
            // 
            //         :param str file_path: path of the file with problem data
            //         :param str data_format: data format of the file
            // 
            //         :return: all data that describe problem
            //         :rtype: int
            //         
            [classmethod]
            public static int @__load_from_file__(object cls, string file_path, string data_format) {
                logger.debug("Load parameters: file path=" + file_path.ToString() + ", data format representation=" + data_format);
                if (data_format == "txt") {
                    var input_file = open(file_path, "r");
                    var text_line = input_file.readline().strip();
                    // skip comments
                    while (text_line.startswith("//") || text_line.startswith(";")) {
                        text_line = input_file.readline();
                    }
                    var dimension = Convert.ToInt32(text_line.split()[0]);
                    return dimension;
                } else {
                    throw new ValueError("Value for data format \'{} \' is not supported".format(data_format));
                }
            }
            
            // 
            //         Additional constructor. Create new `OnesCountProblem` instance when input file and input format are specified
            // 
            //         :param str input_file_path: path of the input file with problem data
            //         :param str input_format: format of the input
            //         
            [classmethod]
            public static void from_input_file(object cls, string input_file_path, string input_format) {
                var dimension = OnesCountProblem.@__load_from_file__(input_file_path, input_format);
                return cls(dim: dimension);
            }
            
            // 
            //         Internal copy of the `OnesCountProblem` problem
            // 
            //         :return: new `OnesCountProblem` instance with the same properties
            //         :rtype: `OnesCountProblem`
            //         
            public virtual void @__copy__() {
                var pr = deepcopy(this);
                return pr;
            }
            
            // 
            //         Copy the `OnesCountProblem` problem
            // 
            //         :return: new `OnesCountProblem` instance with the same properties
            //         :rtype: OnesCountProblem
            //         
            public virtual void copy() {
                return this.@__copy__();
            }
            
            // 
            //         Property getter for dimension of the target problem
            // 
            //         :return: dimension of the target problem instance 
            //         :rtype: int
            //         
            public object dimension {
                get {
                    return this.@__dimension;
                }
            }
            
            // 
            //         String representation of the `MaxOneProblem` instance
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
                s += base.string_rep(delimiter, indentation, indentation_symbol, "", "");
                s += delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "dimension=" + this.dimension.ToString() + delimiter;
                s += group_end;
                return s;
            }
            
            // 
            //         String representation of the max ones problem structure
            // 
            //         :return: string representation of the max ones problem structure
            //         :rtype: str
            //         
            public override string ToString() {
                return this.string_rep("|", 0, "", "{", "}");
            }
            
            // 
            //         Representation of the max ones problem instance
            //         :return: str -- string representation of the max ones problem instance
            //         
            public virtual string @__repr__() {
                return this.string_rep("\n", 0, "   ", "{", "}");
            }
            
            // 
            //         Formatted the max ones problem instance
            //         :param spec: str -- format specification
            //         :return: str -- formatted max ones problem instance
            //         
            public virtual string @__format__(string spec) {
                return this.string_rep("|");
            }
        }
    }
}
