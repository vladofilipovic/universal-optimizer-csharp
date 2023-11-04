//  
// The :mod:`~uo.target_solution.target_solution_void_object_str` module describes the class :class:`~uo.target_solution.TargetSolutionVoidObjectStr`.
// 
namespace target_solution {
    
    using Path = pathlib.Path;
    
    using sys;
    
    using deepcopy = copy.deepcopy;
    
    using logger = uo.utils.logger.logger;
    
    using TargetProblem = uo.target_problem.target_problem.TargetProblem;
    
    using TargetSolution = uo.target_solution.target_solution.TargetSolution;
    
    using QualityOfSolution = uo.target_solution.target_solution.QualityOfSolution;
    
    using Optimizer = uo.algorithm.optimizer.Optimizer;
    
    using OutputControl = uo.algorithm.output_control.OutputControl;
    
    public static class target_solution_void_object_str {
        
        public static object directory = Path(@__file__).resolve();
        
        static target_solution_void_object_str() {
            sys.path.append(directory.parent);
        }
        
        public class TargetSolutionVoidObjectStr
            : TargetSolution[objectstr] {
            
            public object representation;
            
            public TargetSolutionVoidObjectStr(string name)
                : base(random_seed: null, fitness_value: 0, objective_value: 0, is_feasible: true, evaluation_cache_is_used: false, evaluation_cache_max_size: 0, distance_calculation_cache_is_used: false, distance_calculation_cache_max_size: 0) {
            }
            
            public virtual void @__copy__() {
                var pr = deepcopy(this);
                return pr;
            }
            
            public virtual object copy() {
                return this.@__copy__();
            }
            
            public virtual object copy_to(object destination) {
                destination = copy(this);
            }
            
            public virtual string argument(object representation) {
                return representation.ToString();
            }
            
            public virtual object init_random(object problem) {
                this.representation = null;
                return;
            }
            
            public virtual object init_from(object representation, object problem) {
                this.representation = representation;
            }
            
            public virtual object native_representation(string representation_str) {
                return representation_str;
            }
            
            public virtual object calculate_quality_directly(object representation, object problem) {
                return QualityOfSolution(0, 0, true);
            }
            
            public static double representation_distance_directly(object solution_code_1, string solution_code_2) {
                return 0;
            }
            
            public virtual string string_representation() {
                return this.ToString();
            }
            
            public override string ToString() {
                return this.ToString();
            }
            
            public virtual string @__repr__() {
                return this.@__repr__();
            }
            
            public virtual string @__format__(string spec) {
                return this.@__format__();
            }
        }
    }
}
