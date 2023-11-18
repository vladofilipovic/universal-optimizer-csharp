
namespace uo.Algorithm
{

    using uo.TargetProblem;

    using uo.TargetSolution;

    using System;

    using System.Linq;

    public abstract class Optimizer<R_co, A_co>
    {
        private TargetSolution<R_co, A_co>? _bestSolution;
        private DateTime _executionEnded;
        private DateTime _executionStarted;
        private readonly string _name;
        private OutputControl _outputControl;
        private TargetProblem _targetProblem;
        private double _timeWhenBestObtained;

        public Optimizer(string name, OutputControl outputControl, TargetProblem targetProblem)
        {
            _name = name;
            _outputControl = outputControl;
            _targetProblem = targetProblem;
            _timeWhenBestObtained = 0.0;
        }

        /// <summary>
        /// Property getter for the name of the optimizer.
        /// </summary>
        /// <value>
        /// The name of the optimizer instance.
        /// </value>
        public string Name
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
        public TargetProblem TargetProblem
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
        public TargetSolution<R_co, A_co>? BestSolution
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

        /// <summary>
        /// Write data(with field values) to output file, if necessary.
        /// </summary>
        /// <param name="stepName">Name  of the step when data should be written to output 
        /// - have to be one of the following values: 'afterAlgorithm', 'beforeAlgorithm', 
        /// 'afterIteration', 'beforeIteration', 'afterEvaluation', 'beforeEvaluation', 
        /// 'afterStepInIteration', 'beforeStepInIteration'.</param>
        /// <param name="stepNameValue">The step name value - what should be written to the output 
        /// instead of stepName.</param>
        /// <returns></returns>
        /// <exception cref="ValueError">Supplied step name '" + stepName + "' is not valid.</exception>
        public void WriteOutputValuesIfNeeded(string stepName, string stepNameValue)
        {
            string s_data;
            if (this.OutputControl.WriteToOutput)
            {
                var output = this.OutputControl.OutputFile;
                var should_write = false;
                if (stepName == "afterAlgorithm")
                {
                    should_write = true;
                }
                else if (stepName == "beforeAlgorithm")
                {
                    should_write = this.OutputControl.WriteBeforeAlgorithm;
                }
                else if (stepName == "afterIteration")
                {
                    should_write = this.OutputControl.WriteAfterIteration;
                }
                else if (stepName == "beforeIteration")
                {
                    should_write = this.OutputControl.WriteBeforeIteration;
                }
                else if (stepName == "afterEvaluation")
                {
                    should_write = this.OutputControl.WriteAfterEvaluation;
                }
                else if (stepName == "beforeEvaluation")
                {
                    should_write = this.OutputControl.WriteBeforeEvaluation;
                }
                else if (stepName == "afterStepInIteration")
                {
                    should_write = this.OutputControl.WriteAfterStepInIteration;
                }
                else if (stepName == "beforeStepInIteration")
                {
                    should_write = this.OutputControl.WriteBeforeStepInIteration;
                }
                else
                {
                    throw new Exception("Supplied step name '" + stepName + "' is not valid.");
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
                                var data = "TODO"; // eval(f_def);
                                s_data = data.ToString();
                                if (s_data == "stepName")
                                {
                                    s_data = stepNameValue;
                                }
                            }
                            catch
                            {
                                s_data = "XXX";
                            }
                            output?.Write(s_data + "\t");
                            line += s_data + "\t";
                        }
                    }
                    output?.Write("\n");
                }
            }
        }

        /// <summary>
        /// Copies function argument to become the best solution within optimizer instance and update
        /// info about time and iteration when the best solution is updated.
        /// </summary>
        /// <param name="solution">The solution that is source for coping operation.</param>
        /// <returns></returns>
        public virtual void CopyToBestSolution(TargetSolution<R_co, A_co> solution)
        {
            _bestSolution = solution.Clone();
            TimeSpan duration = DateTime.Now - this.ExecutionStarted;
            _timeWhenBestObtained = duration.TotalNanoseconds;
        }

        /// 
        /// Method for optimization   
        /// 
        public virtual object Optimize()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// String representation of the optimizer instance.
        /// </summary>
        /// <param name="delimiter">The delimiter between fields.</param>
        /// <param name="indentation">The indentation level.</param>
        /// <param name="indentationSymbol">The indentation symbol.</param>
        /// <param name="groupStart">The group start.</param>
        /// <param name="groupEnd">The group end.</param>
        /// <returns></returns>
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
            s += "name=" + this.Name + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "TargetProblem=" + this.TargetProblem.StringRep(delimiter, indentation + 1, indentationSymbol, "{", "}") + delimiter;
            s += "_OutputControl=" + this.OutputControl.StringRep(delimiter, indentation + 1, indentationSymbol, "{", "}") + delimiter;
            s += "executionStarted=" + this.ExecutionStarted.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "executionEnded=" + this.ExecutionEnded.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "_timeWhenBestObtained=" + _timeWhenBestObtained.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "execution time=" + (this.ExecutionEnded - this.ExecutionStarted).TotalSeconds.ToString() + delimiter;
            s += "bestSolution=" + this.BestSolution?.StringRep(delimiter, indentation + 1, indentationSymbol, groupStart, groupEnd) + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += groupEnd;
            return s;
        }

    }
}
