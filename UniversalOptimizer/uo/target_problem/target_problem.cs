namespace uo.TargetProblem {
        
    using System;
    
    using System.Linq;
           
    public abstract class TargetProblem
    {
        /// <summary>
        /// Gets a value indicating whether this instance is minimization.
        /// </summary>
        /// <value>
/        ///   <c>true</c> if this instance is minimization; otherwise, <c>false</c>.
        /// </value>
        public bool IsMinimization { get; init; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; init; }


        /// <summary>
        /// Create new TargetProblem instance
        /// </summary>
        /// <param name="name">name of the target problem</param>
        /// <param name="isMinimization">should minimum or maximum be determined</param>
        public TargetProblem(string name, bool isMinimization) {
            Name = name;
            IsMinimization = isMinimization;
        }

        /// <summary>
        /// Returns string representation of the problem.
        /// </summary>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="indentation">The indentation.</param>
        /// <param name="indentationSymbol">The indentation symbol.</param>
        /// <param name="groupStart">The group start.</param>
        /// <param name="groupEnd">The group end.</param>
        /// <returns> string representation </returns>
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
            s += groupStart + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0)) {
                s += indentationSymbol;
            }
            s += "Name=" + this.Name + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0)) {
                s += indentationSymbol;
            }
            s += "IsMinimization=" + this.IsMinimization.ToString();
            foreach (var i in Enumerable.Range(0, indentation - 0)) {
                s += indentationSymbol;
            }
            s += groupEnd;
            return s;
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns> string representation </returns>
        public override string ToString()
        {
            return StringRep("|");
        }
    }
}
