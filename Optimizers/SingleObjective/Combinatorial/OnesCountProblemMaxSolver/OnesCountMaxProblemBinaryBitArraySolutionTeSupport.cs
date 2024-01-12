
namespace SingleObjective.Teaching.OnesCountProblem
{

    using UniversalOptimizer.utils;
    using UniversalOptimizer.TargetSolution;
    using UniversalOptimizer.Algorithm;
    using UniversalOptimizer.Algorithm.Exact.TotalEnumeration;

    using SingleObjective.Teaching.OnesCountProblem;

    using System;
    using System.Collections;
    using UniversalOptimizer.TargetProblem;

    public class OnesCountMaxProblemBinaryBitArraySolutionTeSupport : IProblemSolutionTeSupport<BitArray, string>
    {

        private ComplexCounterBitArrayFull? _bitArrayCounter;

        public OnesCountMaxProblemBinaryBitArraySolutionTeSupport()
        {
            _bitArrayCounter = null;
        }


        /// <summary>
        /// Resets internal counter of the total enumerator, so process will start over. Internal
        /// state of the solution will be set to reflect reset operation.
        /// </summary>
        /// <param name="problem">The problem that is solved.</param>
        /// <param name="solution">The solution used for the problem that is solved.</param>
        /// <param name="optimizer">The optimizer that is executed.</param>
        public void Reset(TargetProblem problem, TargetSolution<BitArray, string> solution, Algorithm<BitArray, string> optimizer)
        {
            if (problem == null)
                throw new ArgumentNullException(string.Format("Parameter '{0}' is null.", nameof(problem)));
            if (problem is not OnesCountMaxProblem)
                throw new ArgumentException(string.Format("Parameter '{0}' have not type 'OnesCountMaxProblem'.", nameof(problem)));
            OnesCountMaxProblem ocProblem = (OnesCountMaxProblem)problem;
            _bitArrayCounter = new ComplexCounterBitArrayFull(ocProblem.Dimension);
            _bitArrayCounter.Reset();
            solution.InitFrom(_bitArrayCounter.CurrentState(), problem);
            optimizer.WriteOutputValuesIfNeeded("beforeEvaluation", "b_e");
            optimizer.Evaluation += 1;
            solution.Evaluate(problem);
            optimizer.WriteOutputValuesIfNeeded("afterEvaluation", "a_e");
        }

        /// <summary>
        /// Progress internal counter of the total enumerator, so next configuration will be taken
        /// into consideration.
        /// Internal state of the solution will be set to reflect progress operation.
        /// </summary>
        /// <param name="problem">The problem that is solved.</param>
        /// <param name="solution">The solution used for the problem that is solved.</param>
        /// <param name="optimizer">The optimizer that is executed.</param>
        public void Progress(TargetProblem problem, TargetSolution<BitArray, string> solution, Algorithm<BitArray, string> optimizer)
        {
            if (_bitArrayCounter == null)
                throw new ArgumentNullException(string.Format("Variable '{0}' is null.", nameof(_bitArrayCounter)));
            _bitArrayCounter.Progress();
            solution.InitFrom(_bitArrayCounter.CurrentState(), problem);
            optimizer.WriteOutputValuesIfNeeded("beforeEvaluation", "b_e");
            optimizer.Evaluation += 1;
            solution.Evaluate(problem);
            optimizer.WriteOutputValuesIfNeeded("afterEvaluation", "a_e");
        }

        /// <summary>
        /// Check if total enumeration process is not at end.
        /// </summary>
        /// <param name="problem">The problem that is solved.</param>
        /// <param name="solution">The solution used for the problem that is solved.</param>
        /// <param name="optimizer">The optimizer that is executed.</param>
        /// <returns>
        /// <c>true</c> if total enumeration process is not at end; otherwise, <c>false</c>.
        /// </returns>
        public bool CanProgress(TargetProblem problem, TargetSolution<BitArray, string> solution, Algorithm<BitArray, string> optimizer)
        {
            if (_bitArrayCounter == null)
                throw new ArgumentNullException(string.Format("Variable '{0}' is null.", nameof(_bitArrayCounter)));
            return _bitArrayCounter.CanProgress();
        }


        /// <summary>
        /// Returns overall number of evaluations required for finishing total enumeration process.
        /// </summary>
        /// <param name="problem">The problem that is solved.</param>
        /// <param name="solution">The solution used for the problem that is solved.</param>
        /// <param name="optimizer">The optimizer that is executed.</param>
        /// <returns></returns>
        public long OverallNumberOfEvaluations(TargetProblem problem, TargetSolution<BitArray, string> solution, Algorithm<BitArray, string> optimizer)
        {
            if (problem == null)
                throw new ArgumentNullException(string.Format("Parameter '{0}' is null.", nameof(problem)));
            if (problem is not OnesCountMaxProblem)
                throw new ArgumentException(string.Format("Parameter '{0}' have not type 'OnesCountMaxProblem'.", nameof(problem)));
            OnesCountMaxProblem ocProblem = (OnesCountMaxProblem)problem;
            int dim = ocProblem.Dimension;
            return (long) Math.Pow(2, dim);
        }
        /// 
        /// String representation of the te support structure
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
        /// :return: string representation of vns support instance
        /// return type str
        /// 
        public  string StringRep(
            string delimiter,
            int indentation = 0,
            string indentationSymbol = "",
            string groupStart = "{",
            string groupEnd = "}") => "OnesCountMaxProblemBinaryBitArraySolutionTeSupport";

        /// 
        /// String representation of the te support instance
        /// 
        /// :return: string representation of the te support instance
        /// return type str
        /// 
        public override string ToString() => this.StringRep("|");

        /// 
        /// Representation of the te support instance
        /// 
        /// :return: string representation of the te support instance
        /// return type str
        /// 
        public virtual string _repr__() => this.StringRep("\n");

        /// 
        /// Formatted the te support instance
        /// 
        /// :param str spec: format specification
        /// :return: formatted te support instance
        /// return type str
        /// 
        public virtual string _format__(string spec) => this.StringRep("|");

    }
}
