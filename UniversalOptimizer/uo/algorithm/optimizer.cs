
namespace uo.Algorithm
{

    using uo.TargetProblem;

    using uo.TargetSolution;

    using System;

    using System.Linq;

    public abstract class Optimizer<R_co, A_co>
    {

        private TargetSolution<R_co, A_co> _bestSolution;

        private DateTime _executionEnded;

        private DateTime _executionStarted;

        private TargetSolution<R_co, A_co> _iterationBestFound;

        private string _name;

        private static OutputControl _outputControl;

        private DateTime _timeWhenBestObtained;

        private TargetProblem _targetProblem;

        public Optimizer(string name, OutputControl outputControl, TargetProblem targetProblem)
        {
            _name = name;
            _outputControl = outputControl;
            _targetProblem = targetProblem;
        }

        /// <summary>
        /// Property getter for the name of the optimizer.
        /// </summary>
        /// <value>
        /// The name of the optimizer instance.
        /// </value>
        public object Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// Property getter for the target problem to be solved.
        /// </summary>
        /// <value>
        /// The target problem to be solved.
        /// </value>
        public object TargetProblem
        {
            get
            {
                return _targetProblem;
            }
        }

        /// <summary>
        /// Property getter and setter for time when execution started.
        /// </summary>
        /// <value>
        /// The time when execution started.
        /// </value>
        public DateTime ExecutionStarted
        {
            get
            {
                return _executionStarted;
            }
            set
            {
                _executionStarted = value;
            }
        }

        /// <summary>
        /// Property getter and setter for time when execution ended.
        /// </summary>
        /// <value>
        /// The time when execution ended.
        /// </value>
        public DateTime ExecutionEnded
        {
            get
            {
                return _executionEnded;
            }
            set
            {
                _executionEnded = value;
            }
        }

        /// <summary>
        /// Property getter for the best solution obtained during optimizer execution.
        /// </summary>
        /// <value>
        /// The best solution so far.
        /// </value>
         public TargetSolution<R_co,A_co> BestSolution
        {
            get
            {
                return _bestSolution;
            }
            set
            {
                _bestSolution = value;
            }
        }

        /// <summary>
        /// Property getter and setter for the output control of the executing optimizer.
        /// </summary>
        /// <value>
        /// The output control of the executing optimizer.
        /// </value>
        public OutputControl OutputControl
        {
            get
            {
                return _outputControl;
            }
            set
            {
                _outputControl = value;
            }
        }

        /// <summary>
        /// Write headers(with field names) to output file, if needed.
        /// </summary>
        public virtual void WriteOutputHeadersIfNeeded()
        {
            if (this.OutputControl.WriteToOutput)
            {
                StreamWriter? output = this.OutputControl.OutputFile;
                if (output is null)
                    return;
                var f_hs = this.OutputControl.FieldsHeadings;
                var line = "";
                foreach (var f_h in f_hs)
                {
                    output.Write(f_h);
                    line += f_h;
                    output.Write("\t");
                    line += "\t";
                }
                output.Write("\n");
            }
        }

        /// 
        /// Write data(with field values) to output file, if necessary 
        /// 
        /// :param str step_name: name of the step when data should be written to output - have to be one of the following values: 'afterAlgorithm', 'beforeAlgorithm', 'afterIteration', 'beforeIteration', 'afterEvaluation', 'beforeEvaluation', 'afterStepInIteration', 'beforeStepInIteration'
        /// :param str step_nameValue: what should be written to the output instead of step_name
        /// 
        public virtual object write_outputValues_if_needed(string step_name, string step_nameValue)
        {
            object s_data;
            if (this.OutputControl.WriteToOutput)
            {
                var output = this.OutputControl.OutputFile;
                var should_write = false;
                if (step_name == "afterAlgorithm")
                {
                    should_write = true;
                }
                else if (step_name == "beforeAlgorithm")
                {
                    should_write = this.OutputControl.WriteBeforeAlgorithm;
                }
                else if (step_name == "afterIteration")
                {
                    should_write = this.OutputControl.WriteAfterIteration;
                }
                else if (step_name == "beforeIteration")
                {
                    should_write = this.OutputControl.WriteBeforeIteration;
                }
                else if (step_name == "afterEvaluation")
                {
                    should_write = this.OutputControl.WriteAfterEvaluation;
                }
                else if (step_name == "beforeEvaluation")
                {
                    should_write = this.OutputControl.WriteBeforeEvaluation;
                }
                else if (step_name == "afterStepInIteration")
                {
                    should_write = this.OutputControl.WriteAfterStepInIteration;
                }
                else if (step_name == "beforeStepInIteration")
                {
                    should_write = this.OutputControl.WriteBeforeStepInIteration;
                }
                else
                {
                    throw new ValueError("Supplied step name '" + step_name + "' is not valid.");
                }
                if (should_write)
                {
                    var line = "";
                    var fields_def = this.OutputControl.FieldsDefinitions;
                    foreach (var f_def in fields_def)
                    {
                        if (f_def != "")
                        {
                            try
                            {
                                var data = eval(f_def);
                                s_data = data.ToString();
                                if (s_data == "step_name")
                                {
                                    s_data = step_nameValue;
                                }
                            }
                            catch
                            {
                                s_data = "XXX";
                            }
                            output.write(s_data + "\t");
                            line += s_data + "\t";
                        }
                    }
                    output.write("\n");
                    logger.info(line);
                }
            }
        }

        /// 
        /// Copies function argument to become the best solution within metaheuristic instance and update info about time 
        /// and iteration when the best solution is updated 
        /// 
        /// :param TargetSolution solution: solution that is source for coping operation
        /// 
        public virtual object copy_to_bestSolution(object solution)
        {
            _bestSolution = solution.copy();
            _timeWhenBestObtained = (datetime.now() - this.executionStarted).total_seconds();
            _iterationBestFound = this.iteration;
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
            string groupEnd = "}")
        {
            var s = delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s = groupStart;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "name=" + this.name + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "TargetProblem=" + this.TargetProblem.stringRep(delimiter, indentation + 1, indentationSymbol, "{", "}") + delimiter;
            s += "_OutputControl=" + _OutputControl.stringRep(delimiter, indentation + 1, indentationSymbol, "{", "}") + delimiter;
            s += "executionStarted=" + this.executionStarted.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "executionEnded=" + this.executionEnded.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "bestSolution=" + this.bestSolution.stringRep(delimiter, indentation + 1, indentationSymbol, groupStart, groupEnd) + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += groupEnd;
            return s;
        }

        /// 
        /// Method for optimization   
        /// 
        [abstractmethod]
        public virtual object optimize()
        {
            throw new NotImplemented();
        }

        /// 
        /// String representation of the 'Algorithm' instance
        /// 
        /// :return: string representation of the 'Algorithm' instance
        /// return type str
        /// 
        [abstractmethod]
        public override string ToString()
        {
            return this.StringRep("|");
        }

        /// 
        /// Representation of the 'Algorithm' instance
        /// 
        /// :return: string representation of the 'Algorithm' instance
        /// return type str
        /// 
        [abstractmethod]
        public virtual string _repr__()
        {
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
        public virtual string _format__(string spec)
        {
            return this.StringRep("|");
        }
    }
}
}
