namespace UniversalOptimizer.Problem
{

    using System;

    using System.Linq;
    using System.Text;

    /// <summary>
    /// Class that abstracts target problem, which should be solved.
    /// </summary>
    /// <remarks>
    /// Create new Problem instance
    /// </remarks>
    /// <param name="name">name of the target problem</param>
    /// <param name="isMinimization">should minimum or maximum be determined</param>
    public abstract class Problem(string name, bool? isMinimization=null, bool? isMultiObjective=null) : ICloneable
    {
        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        object ICloneable.Clone() => throw new NotImplementedException();

        /// <summary>
        /// Gets a value indicating whether this instance is minimization.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is minimization; otherwise, <c>false</c>.
        /// </value>
        public bool? IsMinimization { get; init; } = isMinimization;

        public bool? IsMultiObjective { get; init; } = isMultiObjective;

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; init; } = name;

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public virtual object Clone() => throw new NotImplementedException();

        /// <summary>
        /// Returns string representation of the problem.
        /// </summary>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="indentation">The indentation.</param>
        /// <param name="indentationSymbol">The indentation symbol.</param>
        /// <param name="groupStart">The group start.</param>
        /// <param name="groupEnd">The group end.</param>
        /// <returns> string representation </returns>
        public string StringRep(
            string delimiter,
            int indentation = 0,
            string indentationSymbol = "",
            string groupStart = "{",
            string groupEnd = "}")
        {
            StringBuilder s = new StringBuilder(delimiter);
            for(int i=0; i<indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append(groupStart + delimiter);
            for(int i=0; i<indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append("Name=" + Name + delimiter);
            for(int i=0; i<indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append("IsMinimization=" + IsMinimization.ToString());
            for(int i=0; i<indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append(groupEnd);
            return s.ToString();
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns> string representation </returns>
        public override string ToString() => StringRep("|");

    }
}
