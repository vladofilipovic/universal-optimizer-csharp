namespace uo.TargetSolution
{

    using uo.TargetProblem;
    using uo.TargetSolution;
    using uo.Algorithm;


    public class TargetSolutionVoidObjectStr: TargetSolution<object, string>
    {

        public object representation;

        public TargetSolutionVoidObjectStr(string name)
            : base(name, randomSeed: null, fitnessValue: 0, fitnessValues: new List<double>(), objectiveValue: 0, objectiveValues: new List<double>(), isFeasible: true, evaluationCacheIsUsed: false, evaluationCacheMaxSize: 0, distanceCalculationCacheIsUsed: false, distanceCalculationCacheMaxSize: 0)
        {
        }

        public virtual void _copy__()
        {
            var pr = deepcopy(this);
            return pr;
        }

        public virtual object copy()
        {
            return _copy__();
        }

        public virtual object copy_to(object destination)
        {
            destination = copy(this);
        }

        public virtual string argument(object representation)
        {
            return representation.ToString();
        }

        public virtual object InitRandom(object problem)
        {
            this.representation = null;
            return;
        }

        public virtual object InitFrom(object representation, object problem)
        {
            this.representation = representation;
        }

        public virtual object NativeRepresentation(string representationStr)
        {
            return representationStr;
        }

        public virtual object CalculateQualityDirectly(object representation, object problem)
        {
            return QualityOfSolution(0, 0, true);
        }

        public static double RepresentationDistanceDirectly(object solution_code_1, string solution_code_2)
        {
            return 0;
        }

        public virtual string StringRepresentation()
        {
            return this.ToString();
        }

        public override string ToString()
        {
            return this.ToString();
        }

        public virtual string _repr__()
        {
            return _repr__();
        }

        public virtual string _format__(string spec)
        {
            return _format__();
        }

        public override string Argument(object representation)
        {
            throw new NotImplementedException();
        }

        public override void InitRandom(TargetProblem problem)
        {
            throw new NotImplementedException();
        }

        public override object NativeRepresentation(string representationStr)
        {
            throw new NotImplementedException();
        }

        public override void InitFrom(object representation, TargetProblem problem)
        {
            throw new NotImplementedException();
        }

        public override QualityOfSolution CalculateQualityDirectly(object representation, object problem)
        {
            throw new NotImplementedException();
        }

        public override double RepresentationDistanceDirectly(object representation_1, object representation_2)
        {
            throw new NotImplementedException();
        }
    }
}
}
