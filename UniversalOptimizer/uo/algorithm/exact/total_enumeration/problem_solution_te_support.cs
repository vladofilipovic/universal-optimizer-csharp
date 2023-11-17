///  
/// The :mod:`~uo.Algorithm.exact.total_enumeration.problem_solution_te_support` module describes the class :class:`~uo.Algorithm.exact.total_enumeration.problem_solution_te_support.ProblemSolutionTeSupport`.
/// 
namespace uo.Algorithm.exact.total_enumeration {
    
    using Path = pathlib.Path;
    
    using sys;
    
    using ABCMeta = abc.ABCMeta;
    
    using abstractmethod = abc.abstractmethod;
    
    using TypeVar = typing.TypeVar;
    
    using Generic = typing.Generic;
    
    using Generic = typing.Generic;
    
    using TargetProblem = uo.TargetProblem.TargetProblem.TargetProblem;
    
    using TargetSolution = uo.TargetSolution.TargetSolution.TargetSolution;
    
    using Algorithm = uo.Algorithm.algorithm.Algorithm;
    
    using System;
    
    public static class problem_solution_te_support {
        
        public static object directory = Path(_file__).resolve();
        
        static problem_solution_te_support() {
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent);
            sys.path.append(directory.parent.parent.parent);
        }
        
        public static object R_co = TypeVar("R_co", covariant: true);
        
        public static object A_co = TypeVar("A_co", covariant: true);
        
        public class ProblemSolutionTeSupport
            : Generic[R_coA_co], ABCMeta {
            
            /// 
            ///         Resets internal counter of the total enumerator, so process will start over. Internal state of the solution 
            ///         will be set to reflect reset operation. 
            /// 
            ///         :param `TargetProblem` problem: problem that is solved
            ///         :param `TargetSolution[R_co,A_co]` solution: solution used for the problem that is solved
            ///         :param `Algorithm` optimizer: optimizer that is executed
            ///         
            [abstractmethod]
            public virtual object reset(object problem, object solution, object optimizer) {
                throw new NotImplementedException();
            }
            
            /// 
            ///         Progress internal counter of the total enumerator, so next configuration will be taken into consideration. 
            ///         Internal state of the solution will be set to reflect progress operation.  
            /// 
            ///         :param `TargetProblem` problem: problem that is solved
            ///         :param `TargetSolution[R_co,A_co]` solution: solution used for the problem that is solved
            ///         :param `Algorithm` optimizer: optimizer that is executed
            ///         
            [abstractmethod]
            public virtual object progress(object problem, object solution, object optimizer) {
                throw new NotImplementedException();
            }
            
            /// 
            ///         Check if total enumeration process is not at end.  
            /// 
            ///         :param `TargetProblem` problem: problem that is solved
            ///         :param `TargetSolution[R_co,A_co]` solution: solution used for the problem that is solved
            ///         :param `Algorithm` optimizer: optimizer that is executed
            ///         :return: indicator if total enumeration process is not at end 
            ///         return type bool
            ///         
            [abstractmethod]
            public virtual bool can_progress(object problem, object solution, object optimizer) {
                throw new NotImplementedException();
            }
            
            /// 
            ///         Returns overall number of evaluations required for finishing total enumeration process.  
            /// 
            ///         :param `TargetProblem` problem: problem that is solved
            ///         :param `TargetSolution[R_co,A_co]` solution: solution used for the problem that is solved
            ///         :param `Algorithm` optimizer: optimizer that is executed
            ///         :return: overall number of evaluations required for finishing total enumeration process
            ///         return type int
            ///         
            [abstractmethod]
            public virtual int overall_number_of_evaluations(object problem, object solution, object optimizer) {
                throw new NotImplementedException();
            }
        }
    }
}
