namespace SingleObjective.Teaching.OnesCountProblem
{

    using UniversalOptimizer.Problem;
    using UniversalOptimizer.Solution;

    using System;
    using System.Collections;
    using System.Linq;
    using System.Security.Cryptography;
    using UniversalOptimizer.utils;
    using System.Text;

    /// <summary>
    /// Class for solving OnesCountMaxProblem, with binary int representation.
    /// </summary>
    /// <seealso cref="UniversalOptimizer.Solution.Solution&lt;System.UInt32, System.String&gt;" />
    public class OnesCountMaxProblemBinaryUintSolution: Solution<uint,string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnesCountMaxProblemBinaryUintSolution"/> class.
        /// </summary>
        /// <param name="randomSeed">The random seed.</param>
        /// <param name="evaluationCacheIsUsed">if set to <c>true</c> [evaluation cache is used].</param>
        /// <param name="evaluationCacheMaxSize">Maximum size of the evaluation cache.</param>
        /// <param name="distanceCalculationCacheIsUsed">if set to <c>true</c> [distance calculation cache is used].</param>
        /// <param name="distanceCalculationCacheMaxSize">Maximum size of the distance calculation cache.</param>
        public OnesCountMaxProblemBinaryUintSolution(
            int? randomSeed = null,
            bool evaluationCacheIsUsed = false,
            int evaluationCacheMaxSize = 0,
            bool distanceCalculationCacheIsUsed = false,
            int distanceCalculationCacheMaxSize = 0)
            : base(randomSeed: randomSeed, 
                  fitnessValue: double.NaN, fitnessValues: [], objectiveValue: double.NaN, objectiveValues: [], 
                  isFeasible: null, 
                  evaluationCacheIsUsed: evaluationCacheIsUsed, evaluationCacheMaxSize: evaluationCacheMaxSize, 
                  distanceCalculationCacheIsUsed: distanceCalculationCacheIsUsed, distanceCalculationCacheMaxSize: distanceCalculationCacheMaxSize)
        {
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public override object Clone()
        {
            OnesCountMaxProblemBinaryUintSolution cl = new(randomSeed: RandomSeed);
            cl.FitnessValue = FitnessValue;
            cl.FitnessValues = FitnessValues;
            cl.ObjectiveValue = ObjectiveValue;
            cl.ObjectiveValues = ObjectiveValues;
            cl.IsFeasible = IsFeasible;
            cl.Representation = Representation;
            return cl;
        }

        /// <summary>
        /// Obtains the feasible representation.
        /// </summary>
        /// <param name="problem">The problem.</param>
        /// <returns></returns>
        public override uint ObtainFeasibleRepresentation(Problem problem)
        {
            if (problem is not OnesCountMaxProblem)
                throw new ArgumentException(string.Format("Specified problem should have type 'OnesCountMaxProblem'"));
            OnesCountMaxProblem ocProblem = (OnesCountMaxProblem)problem;
            uint mask = 0xFFFFFFFF;
            mask <<= 8 * sizeof(uint) - ocProblem.Dimension;
            mask >>= 8 * sizeof(uint) - ocProblem.Dimension;
            return Representation & mask;
        }

        /// <summary>
        /// Arguments the specified representation.
        /// </summary>
        /// <param name="representation">The representation.</param>
        /// <returns></returns>
        public override string Argument(uint representation)=> Convert.ToString(representation);

        /// <summary>
        /// Random initialization of the solution.
        /// </summary>
        /// <param name="problem">The problem that is solved by solution.</param>
        /// <returns></returns>
        /// <exception cref="ValueError">
        public override void InitRandom(Problem problem)
        {
            if (problem == null)
                throw new ArgumentNullException(string.Format("Parameter '{0}' is null.", nameof(problem)));
            if (problem is not OnesCountMaxProblem)
                throw new ArgumentException(string.Format("Parameter '{0}' have not type 'OnesCountMaxProblem'.", nameof(problem)));
            OnesCountMaxProblem specificProblem = (OnesCountMaxProblem)problem;
            if (specificProblem.Dimension <= 0)
            {
                throw new ArgumentException("Problem dimension should be positive!");
            }
            if (specificProblem.Dimension >= 8 * sizeof(uint))
            {
                throw new ArgumentException("Problem dimension should be less than 32!");
            }
            Representation = (uint) RandomNumberGenerator.GetInt32(int.MaxValue);
            Representation = ObtainFeasibleRepresentation(specificProblem);
        }

        /// <summary>
        /// Initialization of the solution, by setting its native representation.
        /// </summary>
        /// <param name="representation">The representation.</param>
        /// <param name="problem">The problem.</param>
        /// <returns></returns>
        public override void InitFrom(uint representation, Problem problem) => this.Representation = representation;

        /// <summary>
        /// Calculates the quality directly.
        /// </summary>
        /// <param name="representation">The representation.</param>
        /// <param name="problem">The problem.</param>
        /// <returns></returns>
        public override QualityOfSolution CalculateQualityDirectly(uint representation, Problem problem)
        {
            var ones_count = representation.CountOnes();
            return new QualityOfSolution(objectiveValue: ones_count, fitnessValue: ones_count, isFeasible: true);
        }

        /// <summary>
        /// Obtain native representation from solution code of this instance.
        /// </summary>
        /// <param name="representationStr">The representation string.</param>
        /// <returns>
        /// R_co
        /// </returns>
        public override uint NativeRepresentation(string representationStr)
        {
            var ret = Convert.ToUInt32(representationStr, 2);
            return ret;
        }

        /// <summary>
        /// Directly calculate distance between two solutions determined by its native representations.
        /// </summary>
        /// <param name="rep_1"></param>
        /// <param name="rep_2"></param>
        /// <returns>
        /// The distance.
        /// </returns>
        public override double RepresentationDistanceDirectly(uint rep_1, uint rep_2)
        {
            return ( rep_1 ^ rep_2 ).CountOnes();
        }

        /// <summary>
        /// Strings the rep.
        /// </summary>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="indentation">The indentation.</param>
        /// <param name="indentationSymbol">The indentation symbol.</param>
        /// <param name="groupStart">The group start.</param>
        /// <param name="groupEnd">The group end.</param>
        /// <returns></returns>
        public new string StringRep(
            string delimiter = "\n",
            int indentation = 0,
            string indentationSymbol = "   ",
            string groupStart = "{",
            string groupEnd = "}")
        {
            StringBuilder s = new StringBuilder(delimiter);
            for(int i=0; i<indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append(groupStart);
            s.Append(base.StringRep(delimiter, indentation, indentationSymbol, "", ""));
            s.Append(delimiter);
            for(int i=0; i<indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append("StringRepresentation()=" + this.StringRepresentation());
            s.Append(delimiter);
            for(int i=0; i<indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append(groupEnd);
            return s.ToString();
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => this.StringRep("\n", 0, "   ", "{", "}");

        /// <summary>
        /// Representation of the solution instance.
        /// </summary>
        /// <returns></returns>
        public virtual string _repr__() => this.StringRep("\n", 0, "   ", "{", "}");

        /// <summary>
        /// Formats the specified spec.
        /// </summary>
        /// <param name="spec">The spec.</param>
        /// <returns></returns>
        public virtual string _format__(string spec) => this.StringRep("\n", 0, "   ", "{", "}");

    }
}

