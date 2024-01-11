namespace SingleObjective.Teaching.FunctionOneVariableProblem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.IO;
    using UniversalOptimizer.TargetProblem;
    using System.Text;
    using Serilog;

    public class FunctionOneVariableProblemElements
    {
        public string Expression { get; set; } = "";
        public double DomainLow { get; set; } = 0;
        public double DomainHigh { get; set; } = 0;
    }

    public class FunctionOneVariableProblem : TargetProblem
    {
        private double _domainHigh;
        private double _domainLow;
        private string _expression;

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionOneVariableProblem"/> class.
        /// </summary>
        /// <param name="isMinimization">if set to <c>true</c> [is minimization].</param>
        /// <param name="expression">The expression.</param>
        /// <param name="domainLow">The domain low.</param>
        /// <param name="domainHigh">The domain high.</param>
        public FunctionOneVariableProblem(bool isMinimization, string expression, double domainLow, double domainHigh)
            : base("FunctionOneVariableProblem", isMinimization)
        {
            _expression = expression;
            _domainLow = domainLow;
            _domainHigh = domainHigh;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionOneVariableProblem"/> class.
        /// </summary>
        /// <param name="isMinimization">if set to <c>true</c> [is minimization].</param>
        /// <param name="fovpe">The elements of the Function One Variable Problem.</param>
        public FunctionOneVariableProblem(bool isMinimization, FunctionOneVariableProblemElements fovpe) : this(isMinimization, fovpe.Expression, fovpe.DomainLow, fovpe.DomainHigh)
        {
        }

        private static FunctionOneVariableProblemElements LoadFromFileHelper(string filePath, string dataFormat)
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
                var data = textLine.Split();
                if (data.Length >= 3)
                {
                    return new FunctionOneVariableProblemElements()
                    {
                        Expression = data[0],
                        DomainLow = Convert.ToDouble(data[1]),
                        DomainHigh = Convert.ToDouble(data[2])
                    };
                }
                else
                {
                    throw new FormatException("Invalid line - not enough data: " + data);
                }
            }
            else
            {
                throw new FormatException("Value for data format is not supported: " + dataFormat);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionOneVariableProblem"/> class.
        /// </summary>
        /// <param name="isMinimization">if set to <c>true</c> [is minimization].</param>
        /// <param name="inputFilePath">The input file path.</param>
        /// <param name="inputFormat">The input format.</param>
        public FunctionOneVariableProblem(bool isMinimization, string inputFilePath, string inputFormat) :
            this(isMinimization, LoadFromFileHelper(inputFilePath, inputFormat))
        {
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public override object Clone()
        {
            FunctionOneVariableProblem cl = new(
                IsMinimization == true, Expression, DomainLow, DomainHigh);
            return cl;
        }

        /// <summary>
        /// Gets the expression.
        /// </summary>
        /// <value>
        /// The expression.
        /// </value>
        public string Expression
        {
            get
            {
                return _expression;
            }
        }

        /// <summary>
        /// Gets the domain low.
        /// </summary>
        /// <value>
        /// The domain low.
        /// </value>
        public double DomainLow
        {
            get
            {
                return _domainLow;
            }
        }

        /// <summary>
        /// Gets the domain high.
        /// </summary>
        /// <value>
        /// The domain high.
        /// </value>
        public double DomainHigh
        {
            get
            {
                return _domainHigh;
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
            StringBuilder sb = new(delimiter);
            for(int i=0; i<indentation; i++)
            {
                _ = sb.Append(indentationSymbol);
            }
            _ = sb.Append(groupStart);
            _ = sb.Append(base.StringRep(delimiter, indentation, indentationSymbol, "", ""));
            _ = sb.Append(delimiter);
            for(int i=0; i<indentation; i++)
            {
                _ = sb.Append(indentationSymbol);
            }
            _ = sb.Append("expression=" + Expression);
            _ = sb.Append(delimiter);
            for(int i=0; i<indentation; i++)
            {
                _ = sb.Append(indentationSymbol);
            }
            _ = sb.Append("domainLow=" + DomainLow.ToString());
            _ = sb.Append(delimiter);
            for(int i=0; i<indentation; i++)
            {
                _ = sb.Append(indentationSymbol);
            }
            _ = sb.Append("domainHigh=" + DomainHigh.ToString());
            _ = sb.Append(groupEnd);
            return sb.ToString();
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => StringRep("|", 0, "", "{", "}");

    }
}

