namespace UniversalOptimizer.TargetSolution
{

    using UniversalOptimizer.TargetProblem;
    using UniversalOptimizer.TargetSolution;

    public class TargetSolutionVoidObjectStr : TargetSolution<object, string>, ICloneable
    {

        private object _representation;

        public TargetSolutionVoidObjectStr(string name)
            : base(name, randomSeed: null, fitnessValue: 0, fitnessValues: new List<double>(), objectiveValue: 0, objectiveValues: new List<double>(), isFeasible: true, evaluationCacheIsUsed: false, evaluationCacheMaxSize: 0, distanceCalculationCacheIsUsed: false, distanceCalculationCacheMaxSize: 0)
        {
        }

        public override string Argument(object representation)
        {
            return representation.ToString();
        }

        public override void InitRandom(TargetProblem problem)
        {
            _representation = null;
        }

        public override void InitFrom(object representation, TargetProblem problem)
        {
            this._representation = representation;
        }

        public override object NativeRepresentation(string representationStr)
        {
            return representationStr;
        }

        public override QualityOfSolution CalculateQualityDirectly(object representation, TargetProblem problem)
        {
            return new QualityOfSolution()
            {
                FitnessValue = 0.0,
                ObjectiveValue = 0.0,
                IsFeasible = true
            };
        }

        public static double RepresentationDistanceDirectly(object solution_code_1, string solution_code_2)
        {
            return 0;
        }

        public new string StringRepresentation()
        {
            return ToString();
        }


        public override double RepresentationDistanceDirectly(object representation_1, object representation_2)
        {
            throw new NotImplementedException();
        }

    }

}
