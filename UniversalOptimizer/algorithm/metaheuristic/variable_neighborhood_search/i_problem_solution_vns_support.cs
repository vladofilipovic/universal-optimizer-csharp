
namespace UniversalOptimizer.Algorithm.Metaheuristic.VariableNeighborhoodSearch
{


    using UniversalOptimizer.TargetProblem;

    using UniversalOptimizer.TargetSolution;

    using UniversalOptimizer.Algorithm;

    using System;

    public delegate bool ProblemSolutionVnsSupportShakingMethod<R_co, A_co>(int k,
            TargetProblem problem,
            TargetSolution<R_co, A_co> solution,
            Metaheuristic<R_co, A_co> optimizer);
    public delegate bool ProblemSolutionVnsSupportLocalSearchMethod<R_co, A_co>(int k,
        TargetProblem problem,
        TargetSolution<R_co, A_co> solution,
        Metaheuristic<R_co, A_co> optimizer);

    public interface IProblemSolutionVnsSupport<R_co, A_co>
    {

        /// <summary>
        /// Random VNS shaking of several parts such that new solution code does not differ more 
        /// than supplied from all solution codes inside collection.
        /// </summary>
        /// <param name="k">The k parameter for VNS.</param>
        /// <param name="problem">The problem that is solved.</param>
        /// <param name="solution">The solution used for the problem that is solved.</param>
        /// <param name="optimizer">The optimizer that is executed.</param>
        /// <param name="solutionRepresentations">The solution representations that should be shaken.</param>
        /// <returns>if shaking is successful</returns>
        public bool Shaking(
            int k,
            TargetProblem problem,
            TargetSolution<R_co, A_co> solution,
            Metaheuristic<R_co, A_co> optimizer);

        /// <summary>
        /// Executes "best improvement" variant of the local search procedure.
        /// </summary>
        /// <param name="k">The k parameter for VNS.</param>
        /// <param name="problem">The problem that is solved.</param>
        /// <param name="solution">The solution used for the problem that is solved.</param>
        /// <param name="optimizer">The optimizer that is executed.</param>
        /// <returns>if the local search procedure is successful.</returns>
        public bool LocalSearchBestImprovement(int k, TargetProblem problem,
            TargetSolution<R_co, A_co> solution,
            Metaheuristic<R_co, A_co> optimizer);

        /// <summary>
        /// Executes "first improvement" variant of the local search procedure.
        /// </summary>
        /// <param name="k">The k parameter for VNS.</param>
        /// <param name="problem">The problem that is solved.</param>
        /// <param name="solution">The solution used for the problem that is solved.</param>
        /// <param name="optimizer">The optimizer that is executed.</param>
        /// <returns>if the local search procedure is successful.</returns>
        public bool LocalSearchFirstImprovement(int k, TargetProblem problem,
            TargetSolution<R_co, A_co> solution,
            Metaheuristic<R_co, A_co> optimizer);
    }
}

