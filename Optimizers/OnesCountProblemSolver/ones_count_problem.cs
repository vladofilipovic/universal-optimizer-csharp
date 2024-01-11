///  
/// ..  _py_ones_count_problem:
/// 
/// The :mod:`~opt.SingleObjective.Teaching.OnesCountProblem.ones_count_problem` contains class :class:`~opt.SingleObjective.Teaching.OnesCountProblem.ones_count_problem.OnesCountProblem`, that represents :ref:`Problem_Max_Ones`.
/// 
namespace SingleObjective.Teaching.OnesCountProblem
{
    using Serilog;
    using System;
    using System.Linq;

    using UniversalOptimizer.TargetProblem;

    public class OnesCountProblem : TargetProblem
    {

        private int _dimension;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnesCountProblem"/> class.
        /// </summary>
        /// <param name="dimension">The dimension.</param>
        /// <param name="isMinimization">if set to <c>true</c> [is minimization].</param>
        public OnesCountProblem(int dimension, bool isMinimization) : base("OnesCountProblem", isMinimization)
        {
            _dimension = dimension;
        }

        /// <summary>
        /// Loads from file helper.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="dataFormat">The data format.</param>
        /// <returns></returns>
        /// <exception cref="ValueError">Value for data format \'{} \' is not supported".format(dataFormat)</exception>
        public static int LoadFromFileHelper(string filePath, string dataFormat)
        {
            Log.Debug(string.Format("Load parameters: file path= {0}, data format representation={1}.", filePath, dataFormat));
            if (dataFormat == "txt")
            {
                StreamReader inputFile = new(filePath);
                string? textLine = inputFile.ReadLine()?.Trim();
                /// skip comments
                while (textLine!.StartsWith("///") || textLine!.StartsWith(";"))
                {
                    textLine = inputFile.ReadLine()?.Trim();
                }
                var dimension = Convert.ToInt32(textLine.Split()[0]);
                return dimension;
            }
            else
            {
                throw new FormatException("Value for data format is not supported: " + dataFormat);
            }
        }

        /// <summary>
        /// From the input file.
        /// </summary>
        /// <param name="inputFilePath">The input file path.</param>
        /// <param name="inputFormat">The input format.</param>
        /// <param name="isMinimization">if set to <c>true</c> [is minimization].</param>
        /// <returns></returns>
        public static OnesCountProblem FromInputFile(string inputFilePath, string inputFormat, bool isMinimization)
        {
            var dimension = OnesCountProblem.LoadFromFileHelper(inputFilePath, inputFormat);
            return new OnesCountProblem(dimension: dimension, isMinimization: isMinimization);
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override object Clone()
        {
            return new OnesCountProblem(this.Dimension, this.IsMinimization == true);
        }

        /// <summary>
        /// Gets the dimension.
        /// </summary>
        /// <value>
        /// The dimension.
        /// </value>
        public int Dimension
        {
            get
            {
                return _dimension;
            }
        }

        /// <summary>
        /// Returns string representation of the problem.
        /// </summary>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="indentation">The indentation.</param>
        /// <param name="indentationSymbol">The indentation symbol.</param>
        /// <param name="groupStart">The group start.</param>
        /// <param name="groupEnd">The group end.</param>
        /// <returns>
        /// string representation
        /// </returns>
        public new string StringRep(
            string delimiter,
            int indentation = 0,
            string indentationSymbol = "",
            string groupStart = "{",
            string groupEnd = "}")
        {
            var s = delimiter;
            for(int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            s += groupStart;
            s += base.StringRep(delimiter, indentation, indentationSymbol, "", "");
            s += delimiter;
            for(int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            s += "dimension=" + this.Dimension.ToString() + delimiter;
            s += groupEnd;
            return s;
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => this.StringRep("|", 0, "", "{", "}");


        /// <summary>
        /// Representation of this instance.
        /// </summary>
        /// <returns></returns>
        public virtual string _repr__() => this.StringRep("\n", 0, "   ", "{", "}");

        /// <summary>
        /// Formats the specified spec.
        /// </summary>
        /// <param name="spec">The spec.</param>
        /// <returns></returns>
        public virtual string _format__(string spec) => this.StringRep("|");

    }
}

