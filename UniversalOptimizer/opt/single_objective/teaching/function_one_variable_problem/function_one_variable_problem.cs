namespace UniversalOptimizer.opt.single_objective.teaching
{

    using sys;

    using Path = pathlib.Path;

    using deepcopy = copy.deepcopy;

    using NamedTuple = typing.NamedTuple;

    using logger = uo.utils.logger.logger;

    using TargetProblem = uo.TargetProblem.TargetProblem.TargetProblem;

    using System;

    using System.Collections.Generic;

    using System.Linq;

    public static class function_one_variable_problem
    {

        public static object directory = Path(_file__).resolve();

        static function_one_variable_problem()
        {
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent);
            sys.path.append(directory.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent.parent);
        }

        public static object FunctionOneVariableProblemElements = NamedTuple("FunctionOneVariableProblemElements", new List<Tuple<string, Func<object>>> {
            ("expression", str),
            ("domain_low", @float),
            ("domain_high", @float)
        });

        public class FunctionOneVariableProblem
            : TargetProblem
        {

            private object _domain_high;

            private object _domain_low;

            private object _expression;

            public FunctionOneVariableProblem(string expression, double domain_low, double domain_high)
                : base(is_minimization: false)
            {
                _expression = expression;
                _domain_low = domain_low;
                _domain_high = domain_high;
            }

            [classmethod]
            public static int _load_from_file__(object cls, string file_path, string data_format)
            {
                logger.debug("Load parameters: file path=" + file_path.ToString() + ", data format representation=" + data_format);
                if (data_format == "txt")
                {
                    var input_file = open(file_path, "r");
                    var text_line = input_file.readline().strip();
                    /// skip comments
                    while (text_line.startswith("///") || text_line.startswith(";"))
                    {
                        text_line = input_file.readline();
                    }
                    var data = text_line.split();
                    if (data.Count >= 3)
                    {
                        return FunctionOneVariableProblemElements(data[0], Convert.ToDouble(data[1]), Convert.ToDouble(data[2]));
                    }
                    else
                    {
                        throw new ValueError("Invalid line \'{}\' - not enough data".format(data));
                    }
                }
                else
                {
                    throw new ValueError("Value for data format \'{}\' is not supported".format(data_format));
                }
            }

            /// 
            /// Additional constructor. Create new `FunctionOneVariableProblem` instance when input file and input format are specified
            /// 
            /// :param str input_file_path: path of the input file with problem data
            /// :param str input_format: format of the input
            /// 
            [classmethod]
            public static void from_input_file(object cls, string input_file_path, string input_format)
            {
                var @params = FunctionOneVariableProblem._load_from_file__(input_file_path, input_format);
                return cls(expression: @params.expression, domain_low: @params.domain_low, domain_high: @params.domain_high);
            }

            public virtual void _copy__()
            {
                var pr = deepcopy(this);
                return pr;
            }

            public virtual object copy()
            {
                return _copy__();
            }

            public object expression
            {
                get
                {
                    return _expression;
                }
            }

            public object domain_low
            {
                get
                {
                    return _domain_low;
                }
            }

            public object domain_high
            {
                get
                {
                    return _domain_high;
                }
            }

            public object number_of_intervals
            {
                get
                {
                    return _number_of_intervals;
                }
            }

            public virtual string StringRep(
                string delimiter,
                int indentation = 0,
                string indentationSymbol = "",
                string groupStart = "{",
                string groupEnd = "}")
            {
                var s = delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0))
                {
                    s += indentationSymbol;
                }
                s += groupStart;
                s += base.stringRep(delimiter, indentation, indentationSymbol, "", "");
                s += delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0))
                {
                    s += indentationSymbol;
                }
                s += "expression=" + expression;
                s += delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0))
                {
                    s += indentationSymbol;
                }
                s += "domain_low=" + domain_low.ToString();
                s += delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0))
                {
                    s += indentationSymbol;
                }
                s += "domain_high=" + domain_high.ToString();
                s += groupEnd;
                return s;
            }

            public override string ToString()
            {
                return StringRep("|", 0, "", "{", "}");
            }

            public virtual string _repr__()
            {
                return StringRep("\n", 0, "   ", "{", "}");
            }

            public virtual string _format__(string spec)
            {
                return StringRep("|");
            }
        }
    }
}
