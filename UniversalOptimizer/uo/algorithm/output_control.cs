
namespace uo.Algorithm
{

    using System.Collections.Generic;

    using System;

    using System.Linq;


    /// <summary>
    /// This class determine where the output generated during execution of the optimizer instance will be written.
    /// </summary>
    /// 
    public class OutputControl
    {
        public List<string> _fieldsDefinitions;
        public List<string> _fieldsHeadings;
        private StreamWriter? _outputFile;
        public bool _writeAfterAlgorithm;
        public bool _writeAfterEvaluation;
        public bool _writeAfterIteration;
        public bool _writeAfterStepInIteration;
        public bool _writeBeforeAlgorithm;
        public bool _writeBeforeEvaluation;
        public bool _writeBeforeIteration;
        public bool _writeBeforeStepInIteration;
        private readonly bool _writeToOutput;

        public OutputControl(bool writeToOutput = false, StreamWriter? outputFile = null, string fields = "iteration, evaluation, \"stepName\", bestSolution.argument(), bestSolution.fitnessValue, bestSolution.objectiveValue, bestSolution.isFeasible", string moments = "afterAlgorithm")
        {
            _writeToOutput = writeToOutput;
            _outputFile = outputFile;
            _fieldsHeadings = new List<string> {
                    "iteration",
                    "evaluation",
                    "stepName",
                    "bestSolutionStringRepresentation",
                    "bestSolution_fitnessValue",
                    "bestSolution_objectiveValue",
                    "bestSolution_isFeasible"
                };
            _fieldsDefinitions = new List<string> {
                    "self.iteration",
                    "self.evaluation",
                    "\"stepName\"",
                    "self.bestSolution.stringRepresentation()",
                    "self.bestSolution.fitnessValue",
                    "self.bestSolution.objectiveValue",
                    "self.bestSolution.isFeasible"
                };
            DetermineFieldsHelper(fields);
            DetermineMomentsHelper(moments);
        }

        /// <summary>
        /// Helper function that determines fields header list and field definition lists of the control instance.
        /// </summary>
        /// <param name="fields">Comma-separated list of fields for output - basically fields of the
        /// optimizer object (e.g. `bestSolution.fitnessValue`, `iteration`, `evaluation`, `secondsMax`
        /// etc.) and last word in specific field should he header of the column in .csv file</param>
        private void DetermineFieldsHelper(string fields)
        {
            var fields_head = fields.Replace(".", "_").Replace(" ", "").Replace("()", "").Split(",");
            foreach (var f_h in fields_head)
            {
                if (f_h != "")
                {
                    if (!this.FieldsHeadings.Contains(f_h))
                    {
                        this.FieldsHeadings.Append(f_h);
                    }
                }
            }
            var fields_def = fields.Replace(" ", "").Split(",");
            foreach (var f_def in fields_def)
            {
                string s = f_def;
                if (s != "")
                {
                    if (s[0] != '\'' && s[0] != '"')
                    {
                        s = "self." + s;
                    }
                    if (!this.FieldsDefinitions.Contains(s))
                    {
                        this.FieldsDefinitions.Append(s);
                    }
                }
            }
        }

        /// <summary>
        /// Helper function that determines moments when value of fields will be written to output.
        /// </summary>
        /// <param name="moments">moments: comma-separated list of moments for output - contains
        /// following elements:
        /// `beforeAlgorithm`, `afterAlgorithm`, `beforeIteration`, `afterIteration`,
        /// `beforeEvaluation`, `afterEvaluation`, `beforeStepInIteration`, `afterStepInIteration`.
        /// </param>
        /// <exception cref="ValueError">Invalid value for moment {}. Should be one of:{}.".format(m, "beforeAlgorithm, afterAlgorithm, beforeIteration, afterIteration," + "beforeEvaluation`, afterEvaluation, beforeStepInIteration, afterStepInIteration")</exception>
        private void DetermineMomentsHelper(string moments)
        {
            _writeBeforeAlgorithm = false;
            _writeBeforeIteration = false;
            _writeAfterIteration = false;
            _writeBeforeEvaluation = false;
            _writeAfterEvaluation = false;
            _writeBeforeStepInIteration = false;
            _writeAfterStepInIteration = false;
            _writeAfterAlgorithm = true;
            var mom = moments.Split(",");
            foreach (var mo in mom)
            {
                var m = mo.Trim();
                if (m == "")
                {
                    continue;
                }
                if (m == "beforeAlgorithm")
                {
                    _writeBeforeAlgorithm = true;
                }
                else if (m == "afterAlgorithm")
                {
                    _writeAfterAlgorithm = true;
                }
                else if (m == "beforeIteration")
                {
                    _writeBeforeIteration = true;
                }
                else if (m == "afterIteration")
                {
                    _writeAfterIteration = true;
                }
                else if (m == "beforeEvaluation")
                {
                    _writeBeforeEvaluation = true;
                }
                else if (m == "afterEvaluation")
                {
                    _writeAfterEvaluation = true;
                }
                else if (m == "beforeStepInIteration")
                {
                    _writeBeforeStepInIteration = true;
                }
                else if (m == "afterStepInIteration")
                {
                    _writeAfterStepInIteration = true;
                }
                else
                {
                    throw new Exception("Invalid value for moment" + m + ". Should be one of: beforeAlgorithm, afterAlgorithm, beforeIteration, afterIteration," + "beforeEvaluation`, afterEvaluation, beforeStepInIteration, afterStepInIteration");
                }
            }
        }

        /// <summary>
        /// Property getter for determining if write to output during algorithm execution, or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [write to output]; otherwise, <c>false</c>.
        /// </value>
        public bool WriteToOutput
        {
            get
            {
                return _writeToOutput;
            }
        }

        /// <summary>
        /// Property getter and setter for output file.
        /// </summary>
        /// <value>
        /// The output file.
        /// </value>
        public StreamWriter? OutputFile
        {
            get
            {
                return _outputFile;
            }
            set
            {
                _outputFile = value;
            }
        }

        /// <summary>
        /// Gets the fields headings.
        /// </summary>
        /// <value>
        /// The fields heading list.
        /// </value>
        public List<string> FieldsHeadings
        {
            get
            {
                return _fieldsHeadings;
            }
        }

        /// <summary>
        /// Gets the fields definitions.
        /// </summary>
        /// <value>
        /// The fields definition list.
        /// </value>
        public List<string> FieldsDefinitions
        {
            get
            {
                return _fieldsDefinitions;
            }
        }

        /// <summary>
        /// Gets or sets the fields - comma-separated string with list of fields for output.
        /// </summary>
        /// <value>
        public string Fields
        {
            get
            {
                return String.Join(", ", _fieldsDefinitions.ToArray()).Replace("self.", "");
            }
            set
            {
                DetermineFieldsHelper(value);
            }
        }

        /// <summary>
        /// Property getter and setter for moments.
        /// </summary>
        /// <value>
        /// The moments - comma-separated list of moments when output is created during execution.
        /// </value>
        public string Moments
        {
            get
            {
                var ret = "afterAlgorithm, ";
                if (_writeBeforeAlgorithm)
                {
                    ret += "beforeAlgorithm, ";
                }
                if (_writeBeforeIteration)
                {
                    ret += "beforeIteration, ";
                }
                if (_writeAfterIteration)
                {
                    ret += "afterIteration, ";
                }
                if (_writeBeforeEvaluation)
                {
                    ret += "beforeEvaluation, ";
                }
                if (_writeAfterEvaluation)
                {
                    ret += "afterEvaluation, ";
                }
                if (_writeBeforeStepInIteration)
                {
                    ret += "beforeStepInIteration, ";
                }
                if (_writeAfterStepInIteration)
                {
                    ret += "afterStepInIteration, ";
                }
                ret = ret.Substring(0,ret.Length-2);
                return ret;
            }
            set
            {
                DetermineMomentsHelper(value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether [write before algorithm].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [write before algorithm]; otherwise, <c>false</c>.
        /// </value>
        public bool WriteBeforeAlgorithm
        {
            get
            {
                return _writeBeforeAlgorithm;
            }
        }

        /// <summary>
        /// Gets the write after algorithm.
        /// </summary>
        /// <value>
        /// The write after algorithm.
        /// </value>
        public bool WriteAfterAlgorithm
        {
            get
            {
                return _writeAfterAlgorithm;
            }
        }

        /// <summary>
        /// Gets the write before iteration.
        /// </summary>
        /// <value>
        /// The write before iteration.
        /// </value>
        public bool WriteBeforeIteration
        {
            get
            {
                return _writeBeforeIteration;
            }
        }

        /// <summary>
        /// Gets the write after iteration.
        /// </summary>
        /// <value>
        /// The write after iteration.
        /// </value>
        public bool WriteAfterIteration
        {
            get
            {
                return _writeAfterIteration;
            }
        }

        /// <summary>
        /// Gets the write before evaluation.
        /// </summary>
        /// <value>
        /// The write before evaluation.
        /// </value>
        public bool WriteBeforeEvaluation
        {
            get
            {
                return _writeBeforeEvaluation;
            }
        }

        /// <summary>
        /// Gets the write after evaluation.
        /// </summary>
        /// <value>
        /// The write after evaluation.
        /// </value>
        public bool WriteAfterEvaluation
        {
            get
            {
                return _writeAfterEvaluation;
            }
        }

        /// <summary>
        /// Gets the write before step in iteration.
        /// </summary>
        /// <value>
        /// The write before step in iteration.
        /// </value>
        public bool WriteBeforeStepInIteration
        {
            get
            {
                return _writeBeforeStepInIteration;
            }
        }

        /// <summary>
        /// Gets the write after step in iteration.
        /// </summary>
        /// <value>
        /// The write after step in iteration.
        /// </value>
        public bool WriteAfterStepInIteration
        {
            get
            {
                return _writeAfterStepInIteration;
            }
        }

        /// <summary>
        /// String representation of the output control instance.
        /// </summary>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="indentation">The indentation.</param>
        /// <param name="indentationSymbol">The indentation symbol.</param>
        /// <param name="groupStart">The group start.</param>
        /// <param name="groupEnd">The group end.</param>
        /// <returns></returns>
        public new string StringRep(
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
            s += groupStart + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "writeToOutput=" + this.WriteToOutput.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += $"outputFile={this.OutputFile}{delimiter}";
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "fieldsHeadings=" + this.FieldsHeadings.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "fieldsDefinitions=" + this.FieldsDefinitions.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "moments=" + this.Moments.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += groupEnd;
            return s;
        }

        /// <summary>
        ///  String representation of the cache control and statistics structure.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
         public override string ToString()
        {
            return this.StringRep("|");
        }

    }
}
