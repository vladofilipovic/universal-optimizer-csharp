namespace uo.TargetSolution {
        
    using uo.TargetProblem;
    
    using uo.TargetSolution;
       
    using Optimizer = uo.Algorithm.optimizer.Optimizer;
    
    using OutputControl = uo.Algorithm.OutputControl.OutputControl;
    
    public static class TargetSolutionVoidObjectStr {
        
        public static object directory = Path(_file__).resolve();
        
        static TargetSolutionVoidObjectStr() {
            sys.path.append(directory.parent);
        }
        
        public class TargetSolutionVoidObjectStr
            : TargetSolution[objectstr] {
            
            public object representation;
            
            public TargetSolutionVoidObjectStr(string name)
                : base(randomSeed: null, fitnessValue: 0, objectiveValue: 0, isFeasible: true, evaluationCacheIsUsed: false, evaluationCacheMaxSize: 0, distanceCalculationCacheIsUsed: false, distanceCalculationCacheMaxSize: 0) {
            }
            
            public virtual void _copy__() {
                var pr = deepcopy(this);
                return pr;
            }
            
            public virtual object copy() {
                return _copy__();
            }
            
            public virtual object copy_to(object destination) {
                destination = copy(this);
            }
            
            public virtual string argument(object representation) {
                return representation.ToString();
            }
            
            public virtual object InitRandom(object problem) {
                this.representation = null;
                return;
            }
            
            public virtual object InitFrom(object representation, object problem) {
                this.representation = representation;
            }
            
            public virtual object NativeRepresentation(string representationStr) {
                return representationStr;
            }
            
            public virtual object CalculateQualityDirectly(object representation, object problem) {
                return QualityOfSolution(0, 0, true);
            }
            
            public static double RepresentationDistanceDirectly(object solution_code_1, string solution_code_2) {
                return 0;
            }
            
            public virtual string StringRepresentation() {
                return this.ToString();
            }
            
            public override string ToString() {
                return this.ToString();
            }
            
            public virtual string _repr__() {
                return _repr__();
            }
            
            public virtual string _format__(string spec) {
                return _format__();
            }
        }
    }
}
