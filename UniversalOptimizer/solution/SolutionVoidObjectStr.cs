namespace UniversalOptimizer.Solution
{

    using UniversalOptimizer.Problem;
    using UniversalOptimizer.Solution;

    public class SolutionVoidObjectStr : Solution<object, string>, ICloneable
    {

        private object? _representation;

        public SolutionVoidObjectStr()
            : base(randomSeed: null, fitnessValue: 0, fitnessValues: new List<double>(), objectiveValue: 0, objectiveValues: new List<double>(), isFeasible: true, evaluationCacheIsUsed: false, evaluationCacheMaxSize: 0, distanceCalculationCacheIsUsed: false, distanceCalculationCacheMaxSize: 0)
        {
        }

        public override string Argument(object? representation) => representation!.ToString() ?? "";

        public override void InitRandom(Problem problem) => _representation = default;

        public override void InitFrom(object representation, Problem problem) => this._representation = representation;

        public override object NativeRepresentation(string representationStr) => representationStr;

        public override QualityOfSolution CalculateQualityDirectly(object? representation, Problem problem) => new QualityOfSolution(fitnessValue: 0.0, objectiveValue: 0.0, isFeasible: true);

        public static double RepresentationDistanceDirectly(object solution_code_1, string solution_code_2) => 0;

        public new string StringRepresentation() => ToString();


        public override double RepresentationDistanceDirectly(object representation_1, object representation_2) => throw new NotImplementedException();

    }

}
