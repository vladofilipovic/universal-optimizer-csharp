///  
/// The :mod:`~uo.Algorithm.Exact.TotalEnumeration.problemSolutionTeSupport` module describes the class :class:`~uo.Algorithm.Exact.TotalEnumeration.problemSolutionTeSupport.ProblemSolutionTeSupport`.
/// 
namespace UniversalOptimizer.Algorithm.Exact.TotalEnumeration
{

    using UniversalOptimizer.TargetProblem;

    using UniversalOptimizer.TargetSolution;

    using UniversalOptimizer.Algorithm;

    using System;

    public delegate void ProblemSolutionTeSupportResetMethod<R_co, A_co>(TargetProblem problem, TargetSolution<R_co, A_co> solution, Algorithm<R_co, A_co> optimizer) ;
    public delegate void ProblemSolutionTeSupportProgressMethod<R_co, A_co>(TargetProblem problem, TargetSolution<R_co, A_co> solution, Algorithm<R_co, A_co> optimizer) ;
    public delegate bool ProblemSolutionTeSupportCanProgressMethod<R_co, A_co>(TargetProblem problem, TargetSolution<R_co, A_co> solution, Algorithm<R_co, A_co> optimizer) ;
    public delegate long ProblemSolutionTeSupportOverallNumberOfEvaluationsMethod<R_co, A_co>(TargetProblem problem, TargetSolution<R_co, A_co> solution, Algorithm<R_co, A_co> optimizer) ;

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
        public long OverallNumberOfEvaluations(TargetProblem problem, TargetSolution<R_co, A_co> solution, Algorithm<R_co, A_co> optimizer);
    }
}

