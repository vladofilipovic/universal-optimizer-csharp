//  
// The :mod:`~uo.target_problem.target_problem` module describes the class :class:`~uo.target_problem.TargetProblem`.
// 
namespace target_problem {
        
    using System;
    
    using System.Linq;
           
    public abstract class TargetProblem: ICloneable
    {


        public bool IsMinimization { get; init; }
            
        public string Name { get; init; }

        public TargetProblem(string name, bool isMinimization) {
            Name = name;
            IsMinimization = isMinimization;
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public virtual string stringRep(
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

        public override string ToString()
        {
            return stringRep("|");
        }
    }
}
