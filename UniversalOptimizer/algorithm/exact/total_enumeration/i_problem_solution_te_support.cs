///  
/// The :mod:`~uo.Algorithm.Exact.TotalEnumeration.problemSolution_te_support` module describes the class :class:`~uo.Algorithm.Exact.TotalEnumeration.problemSolution_te_support.ProblemSolutionTeSupport`.
/// 
namespace UniversalOptimizer.Algorithm.Exact.TotalEnumeration
{

    using UniversalOptimizer.TargetProblem;

    using UniversalOptimizer.TargetSolution;

    using UniversalOptimizer.Algorithm;

    using System;
    public interface IProblemSolutionTeSupport<R_co, A_co>
    {

        /// <summary>
        /// Resets internal counter of the total enumerator, so process will start over. Internal 
        /// state of the solution will be set to reflect reset operation.
        /// </summary>
        /// <param name="problem">The problem that is solved.</param>
        /// <param name="solution">The solution used for the problem that is solved.</param>
        /// <param name="optimizer">The optimizer that is executed.</param>
        public void Reset(TargetProblem problem, TargetSolution<R_co, A_co> solution, Algorithm<R_co, A_co> optimizer);

        /// <summary>
        /// Progress internal counter of the total enumerator, so next configuration will be taken 
        /// into consideration. 
        /// Internal state of the solution will be set to reflect progress operation.
        /// </summary>
        /// <param name="problem">The problem that is solved.</param>
        /// <param name="solution">The solution used for the problem that is solved.</param>
        /// <param name="optimizer">The optimizer that is executed.</param>
        public void Progress(TargetProblem problem, TargetSolution<R_co, A_co> solution, Algorithm<R_co, A_co> optimizer);

        /// <summary>
        /// Check if total enumeration process is not at end.
        /// </summary>
        /// <param name="problem">The problem that is solved.</param>
        /// <param name="solution">The solution used for the problem that is solved.</param>
        /// <param name="optimizer">The optimizer that is executed.</param>
        /// <returns>
        ///   <c>true</c> if total enumeration process is not at end; otherwise, <c>false</c>.
        /// </returns>
        public bool CanProgress(TargetProblem problem, TargetSolution<R_co, A_co> solution, Algorithm<R_co, A_co> optimizer);

        /// <summary>
        /// Returns overall number of evaluations required for finishing total enumeration process.
        /// </summary>
        /// <param name="problem">The problem that is solved.</param>
        /// <param name="solution">The solution used for the problem that is solved.</param>
        /// <param name="optimizer">The optimizer that is executed.</param>
        /// <returns></returns>
        public int OverallNumberOfEvaluations(TargetProblem problem, TargetSolution<R_co, A_co> solution, Algorithm<R_co, A_co> optimizer);
    }
}

