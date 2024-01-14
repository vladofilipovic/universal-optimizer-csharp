namespace SingleObjective.Teaching.OnesCountProblem
{

    using UniversalOptimizer.TargetProblem;
    using UniversalOptimizer.TargetSolution;

    using System;
    using System.Collections;
    using System.Linq;
    using System.Security.Cryptography;
    using UniversalOptimizer.utils;
    using System.Linq.Expressions;
    using System.Text;

    public class OnesCountMaxProblemBinaryBitArraySolution: TargetSolution<BitArray,string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnesCountMaxProblemBinaryBitArraySolution"/> class.
        /// </summary>
        /// <param name="randomSeed">The random seed.</param>
        /// <param name="evaluationCacheIsUsed">if set to <c>true</c> [evaluation cache is used].</param>
        /// <param name="evaluationCacheMaxSize">Maximum size of the evaluation cache.</param>
        /// <param name="distanceCalculationCacheIsUsed">if set to <c>true</c> [distance calculation cache is used].</param>
        /// <param name="distanceCalculationCacheMaxSize">Maximum size of the distance calculation cache.</param>
        public OnesCountMaxProblemBinaryBitArraySolution(
            int? randomSeed = null,
            bool evaluationCacheIsUsed = false,
            int evaluationCacheMaxSize = 0,
            bool distanceCalculationCacheIsUsed = false,
            int distanceCalculationCacheMaxSize = 0)
            : base(randomSeed: randomSeed, fitnessValue: double.NaN, fitnessValues:[], 
                  objectiveValue: double.NaN, objectiveValues:[], isFeasible: null, 
                  evaluationCacheIsUsed: evaluationCacheIsUsed, evaluationCacheMaxSize: evaluationCacheMaxSize,
                  distanceCalculationCacheIsUsed: distanceCalculationCacheIsUsed, distanceCalculationCacheMaxSize: distanceCalculationCacheMaxSize)
        {
            Representation = new BitArray(0);
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public override object Clone()
        {
            OnesCountMaxProblemBinaryBitArraySolution cl = new(randomSeed: RandomSeed);
            cl.FitnessValue = FitnessValue;
            cl.FitnessValues = FitnessValues;
            cl.ObjectiveValue = ObjectiveValue;
            cl.ObjectiveValues = ObjectiveValues;
            cl.IsFeasible = IsFeasible;
            if (Representation is not null)
                cl.Representation = new BitArray(Representation);
            else
                cl.Representation = null;
            return cl;
        }

        /// <summary>
        /// Arguments the specified representation.
        /// </summary>
        /// <param name="representation">The representation.</param>
        /// <returns></returns>
        public override string Argument(BitArray? representation) => representation!.ToString() ?? "null";

        /// <summary>
        /// Random initialization of the solution.
        /// </summary>
        /// <param name="problem">The problem that is solved by solution.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">string.Format("Parameter '{0}' is null.", nameof(problem))</exception>
        /// <exception cref="ArgumentException">string.Format("Parameter '{0}' have not type 'OnesCountMaxProblem'.", nameof(problem))</exception>
        public override void InitRandom(TargetProblem problem)
        {
            if (problem == null) 
                throw new ArgumentNullException(string.Format("Parameter '{0}' is null.", nameof(problem)));
            if(problem is not OnesCountMaxProblem)
                throw new ArgumentException(string.Format("Parameter '{0}' have not type 'OnesCountMaxProblem'.", nameof(problem)));
            OnesCountMaxProblem ocProblem = (OnesCountMaxProblem) problem;
            int dim = ocProblem.Dimension;
            byte[] values =  RandomNumberGenerator.GetBytes(dim/8);
            Representation = new BitArray(values);
        }

        /// <summary>
        /// Initialization of the solution, by setting its native representation.
        /// </summary>
        /// <param name="representation">The representation.</param>
        /// <param name="problem">The problem.</param>
        /// <returns></returns>
        public override void InitFrom(BitArray representation, TargetProblem problem)
        {
            Representation = new BitArray(representation);
        }

        /// <summary>
        /// Calculates the quality directly.
        /// </summary>
        /// <param name="representation">The representation.</param>
        /// <param name="problem">The problem.</param>
        /// <returns></returns>
        public override QualityOfSolution CalculateQualityDirectly(BitArray? representation, TargetProblem problem)
        {
            if (representation == null)
                throw new ArgumentNullException(string.Format("Parameter '{0}' is null.", nameof(representation)));
            int onesCount =  representation!.CountOnes();
            return new QualityOfSolution(objectiveValue: onesCount, fitnessValue: onesCount, isFeasible: true);
        }

        /// <summary>
        /// Natives the representation.
        /// </summary>
        /// <param name="representationStr">The representation string.</param>
        /// <returns></returns>
        public override BitArray NativeRepresentation(string representationStr)
        {
            int dim = representationStr.Length;
            bool[] values = new bool[dim];
            for (int i = 0; i < dim; i++)
                if (representationStr[i] == '0')
                    values[i] = false;
                else if (representationStr[i] == '0')
                    values[i] = false;
                else throw new ArgumentException(string.Format("Parameter '{0}' contains invalid data {1}.", nameof(representationStr), values[i]));
            var ret = new BitArray(values);
            return ret;
        }

        /// <summary>
        /// Directly calculate distance between two solutions determined by its native representations.
        /// </summary>
        /// <param name="representation_1">The native representation for the first solution.</param>
        /// <param name="representation_2">The native representation for the second solution.</param>
        /// <returns>
        /// The distance.
        /// </returns>
        public override double RepresentationDistanceDirectly(BitArray rep_1, BitArray rep_2)
        {
            rep_1.Xor(rep_2);
            return rep_1.CountOnes();
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
        /// String representation of the solution instance
        public new string StringRep(
            string delimiter = "\n",
            int indentation = 0,
            string indentationSymbol = "   ",
            string groupStart = "{",
            string groupEnd = "}")
        {
            StringBuilder s = new StringBuilder(delimiter);
            foreach (var _ in Enumerable.Range(0, indentation - 0))
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
        /// Representation of this instance.
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

