///  
/// The :mod:`~uo.Algorithm.Metaheuristic.variable_neighborhood_search.problem_solution_vns_support` module describes the class :class:`~uo.Algorithm.Metaheuristic.variable_neighborhood_search.problem_solution_vns_support.ProblemSolutionVnsSupport`.
/// 
namespace UniversalOptimizer.Algorithm.Metaheuristic.variable_neighborhood_search
{

    using Path = pathlib.Path;

    using sys;

    using ABCMeta = abc.ABCMeta;

    using abstractmethod = abc.abstractmethod;

    using TypeVar = typing.TypeVar;

    using Generic = typing.Generic;

    using TargetProblem = uo.TargetProblem.TargetProblem.TargetProblem;

    using TargetSolution = uo.TargetSolution.TargetSolution.TargetSolution;

    using Algorithm = uo.Algorithm.algorithm.Algorithm;

    using System;

    public static class problem_solution_vns_support
    {

        public static object directory = Path(_file__).resolve();

        static problem_solution_vns_support()
        {
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent);
            sys.path.append(directory.parent.parent.parent);
        }

        public static object R_co = TypeVar("R_co", covariant: true);

        public static object A_co = TypeVar("A_co", covariant: true);

        public class ProblemSolutionVnsSupport
            : Generic[R_coA_co], ABCMeta
        {

            /// 
            /// Random VNS shaking of several parts such that new solution code does not differ more than supplied from all 
            /// solution codes inside collection
            /// 
            /// :param int k: int parameter for VNS
            /// :param `TargetProblem` problem: problem that is solved
            /// :param `TargetSolution[R_co,A_co]` solution: solution used for the problem that is solved
            /// :param `Algorithm` optimizer: optimizer that is executed
            /// :param `list[R_co]` solutionRepresentations: solution representations that should be shaken
            /// :return: if shaking is successful
            /// return type bool
            /// 
            [abstractmethod]
            public virtual bool shaking(
                int k,
                object problem,
                object solution,
                object optimizer,
                object solutionRepresentations)
            {
                throw new NotImplementedException();
            }

            /// 
            /// Executes "best improvement" variant of the local search procedure 
            /// 
            /// :param int k: int parameter for VNS
            /// :param `TargetProblem` problem: problem that is solved
            /// :param `TargetSolution` solution: solution used for the problem that is solved
            /// :param `Algorithm` optimizer: optimizer that is executed
            /// :return: result of the local search procedure 
            /// return type TargetSolution
            /// 
            [abstractmethod]
            public virtual object local_search_best_improvement(int k, object problem, object solution, object optimizer)
            {
                throw new NotImplementedException();
            }

            /// 
            /// Executes "first improvement" variant of the local search procedure 
            /// 
            /// :param int k: int parameter for VNS
            /// :param `TargetProblem` problem: problem that is solved
            /// :param `TargetSolution` solution: solution used for the problem that is solved
            /// :param `Algorithm` optimizer: optimizer that is executed
            /// :return: result of the local search procedure 
            /// return type TargetSolution
            /// 
            [abstractmethod]
            public virtual object local_search_first_improvement(int k, object problem, object solution, object optimizer)
            {
                throw new NotImplementedException();
            }
        }
    }
}
