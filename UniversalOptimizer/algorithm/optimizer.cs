namespace UniversalOptimizer.Algorithm
{

    using Problem;

    using Solution;

    using System;

    using System.Linq;
    using UniversalOptimizer.utils;
    using Serilog;
    using System.Text;
    using UniversalOptimizer.Algorithm.Metaheuristic;

    public abstract class Optimizer<R_co, A_co> : ICloneable
    {
        private Solution<R_co, A_co>? _bestSolution;
        private DateTime _executionEnded;
        private DateTime _executionStarted;
        private readonly string _name;
        private OutputControl _outputControl;
        private readonly Problem _problem;
        private double? _timeWhenBestFound;

        /// <summary>
        /// Initializes a new instance of the <see cref="Optimizer{R_co, A_co}"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="outputControl">The output control.</param>
        /// <param name="problem">The target problem.</param>
        protected Optimizer(string name, OutputControl outputControl, Problem problem)
        {
            _name = name;
            _outputControl = outputControl;
            _problem = problem;
            _timeWhenBestFound = null;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual object Clone()
        {
            throw new NotImplementedException();
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
        public Problem Problem
        {
            get
            {
                return _problem;
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
        /// Property getter for time when best is obtained.
        /// </summary>
        /// <value>
        /// The time when execution ended.
        /// </value>
        public double? TimeWhenBestFound
        {
            get
            {
                return _timeWhenBestFound;
            }
            set
            {
                _timeWhenBestFound = value;
            }
        }

        /// <summary>
        /// Property getter for the best solution obtained during optimizer execution.
        /// </summary>
        /// <value>
        /// The best solution so far.
        /// </value>
        public virtual Solution<R_co, A_co>? BestSolution
        {
            get
            {
                return _bestSolution;
            }
            set
            {
                if (value == null)
                {
                    _bestSolution = null;
                    return;
                }
                _bestSolution = value.Clone() as Solution<R_co, A_co>;
                TimeSpan duration = DateTime.UtcNow - ExecutionStarted;
                _timeWhenBestFound = duration.TotalNanoseconds;
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
            if (OutputControl.WriteToOutput)
            {
                StreamWriter? output = OutputControl.OutputFile;
                if (output is null)
                    return;
                var f_hs = OutputControl.FieldsHeadings;
                StringBuilder line = new StringBuilder("");
                foreach (var f_h in f_hs)
                {
                    output.Write(f_h);
                    line.Append(f_h);
                    output.Write('\t');
                    line.Append('\t');
                }
                output.Write("\n");
                Log.Debug(line.ToString());
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
            if (!OutputControl.WriteToOutput)
            {
                return;
            }
            string s_data;
            var output = OutputControl.OutputFile;
            bool should_write = false;
            if (stepName == "afterAlgorithm")
            {
                should_write = true;
            }
            else if (stepName == "beforeAlgorithm")
            {
                should_write = OutputControl.WriteBeforeAlgorithm;
            }
            else if (stepName == "afterIteration")
            {
                should_write = OutputControl.WriteAfterIteration;
            }
            else if (stepName == "beforeIteration")
            {
                should_write = OutputControl.WriteBeforeIteration;
            }
            else if (stepName == "afterEvaluation")
            {
                should_write = OutputControl.WriteAfterEvaluation;
            }
            else if (stepName == "beforeEvaluation")
            {
                should_write = OutputControl.WriteBeforeEvaluation;
            }
            else if (stepName == "afterStepInIteration")
            {
                should_write = OutputControl.WriteAfterStepInIteration;
            }
            else if (stepName == "beforeStepInIteration")
            {
                should_write = OutputControl.WriteBeforeStepInIteration;
            }
            else
            {
                throw new ArgumentException("Supplied step name '" + stepName + "' is not valid.");
            }
            if (should_write)
            {
                StringBuilder line = new StringBuilder("");
                var fields_def = OutputControl.FieldsDefinitions;
                foreach (var f_def in fields_def)
                {
                    if (f_def != "")
                    {
                        try
                        {
                            var data = this.ReflectionGetPropertyValue(f_def);
                            s_data = data!.ToString() ?? "";
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
                        line.Append(s_data + "\t");
                        Log.Debug(line.ToString());
                    }
                }
                output?.Write("\n");
            }
        }

        /// 
        /// Method for optimization   
        /// 
        public virtual Solution<R_co, A_co>? Optimize() => throw new NotImplementedException();

        /// <summary>
        /// String representation of the optimizer instance.
        /// </summary>
        /// <param name="delimiter">The delimiter between fields.</param>
        /// <param name="indentation">The indentation level.</param>
        /// <param name="indentationSymbol">The indentation symbol.</param>
        /// <param name="groupStart">The group start.</param>
        /// <param name="groupEnd">The group end.</param>
        /// <returns></returns>
        public string StringRep(
            string delimiter,
            int indentation = 0,
            string indentationSymbol = "",
            string groupStart = "{",
            string groupEnd = "}")
        {
            StringBuilder s = new StringBuilder(delimiter);
            for (int i = 0; i < indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append(groupStart);
            for (int i = 0; i < indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append("name=" + Name + delimiter);
            for (int i = 0; i < indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append("Problem=" + Problem.StringRep(delimiter, indentation + 1, indentationSymbol, "{", "}") + delimiter);
            s.Append("_OutputControl=" + OutputControl.StringRep(delimiter, indentation + 1, indentationSymbol, "{", "}") + delimiter);
            s.Append("executionStarted=" + ExecutionStarted.ToString() + delimiter);
            for (int i = 0; i < indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append("executionEnded=" + ExecutionEnded.ToString() + delimiter);
            for (int i = 0; i < indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append("_timeWhenBestFound=" + _timeWhenBestFound.ToString() + delimiter);
            for (int i = 0; i < indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append("execution time=" + (ExecutionEnded - ExecutionStarted).TotalSeconds.ToString() + delimiter);
            s.Append("bestSolution=" + BestSolution?.StringRep(delimiter, indentation + 1, indentationSymbol, groupStart, groupEnd) + delimiter);
            for (int i = 0; i < indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append(groupEnd);
            return s.ToString();
        }

    }
}
