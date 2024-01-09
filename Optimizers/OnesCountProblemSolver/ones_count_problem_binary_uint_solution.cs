namespace SingleObjective.Teaching.OnesCountProblem
{

    using UniversalOptimizer.TargetProblem;
    using UniversalOptimizer.TargetSolution;

    using System;
    using System.Collections;
    using System.Linq;
    using System.Security.Cryptography;
    using UniversalOptimizer.utils;

    /// <summary>
    /// Class for solving OnesCountProblem, with binary int representation.
    /// </summary>
    /// <seealso cref="UniversalOptimizer.TargetSolution.TargetSolution&lt;System.UInt32, System.String&gt;" />
    public class OnesCountProblemBinaryUIntSolution: TargetSolution<uint,string>
    {
        private uint _representation;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnesCountProblemBinaryUIntSolution"/> class.
        /// </summary>
        /// <param name="randomSeed">The random seed.</param>
        /// <param name="evaluationCacheIsUsed">if set to <c>true</c> [evaluation cache is used].</param>
        /// <param name="evaluationCacheMaxSize">Maximum size of the evaluation cache.</param>
        /// <param name="distanceCalculationCacheIsUsed">if set to <c>true</c> [distance calculation cache is used].</param>
        /// <param name="distanceCalculationCacheMaxSize">Maximum size of the distance calculation cache.</param>
        public OnesCountProblemBinaryUIntSolution(
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
        /// Makes to be feasible helper.
        /// </summary>
        /// <param name="problem">The problem.</param>
        /// <returns></returns>
        private void MakeToBeFeasibleHelper(OnesCountProblem problem)
        {
            uint mask = 0xFFFFFFFF;
            mask <<= 32 - problem.Dimension;
            mask >>= 32 - problem.Dimension;
            this._representation &= mask;
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
        public override void InitRandom(TargetProblem problem)
        {
            if (problem == null)
                throw new ArgumentNullException(string.Format("Parameter '{0}' is null.", nameof(problem)));
            if (problem is not OnesCountProblem)
                throw new ArgumentException(string.Format("Parameter '{0}' have not type 'OnesCountProblem'.", nameof(problem)));
            OnesCountProblem specificProblem = (OnesCountProblem)problem;
            if (specificProblem.Dimension <= 0)
            {
                throw new ArgumentException("Problem dimension should be positive!");
            }
            if (specificProblem.Dimension >= 32)
            {
                throw new ArgumentException("Problem dimension should be less than 32!");
            }
            _representation = (uint) RandomNumberGenerator.GetInt32( (int)(Math.Pow(2, specificProblem.Dimension) - 1));
            MakeToBeFeasibleHelper(specificProblem);
        }

        /// <summary>
        /// Initialization of the solution, by setting its native representation.
        /// </summary>
        /// <param name="representation">The representation.</param>
        /// <param name="problem">The problem.</param>
        /// <returns></returns>
        public override void InitFrom(uint representation, TargetProblem problem) => this._representation = representation;

        /// <summary>
        /// Calculates the quality directly.
        /// </summary>
        /// <param name="representation">The representation.</param>
        /// <param name="problem">The problem.</param>
        /// <returns></returns>
        public override QualityOfSolution CalculateQualityDirectly(uint representation, TargetProblem problem)
        {
            var ones_count = representation.CountOnes();
            return new QualityOfSolution(ones_count, ones_count, true);
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
            var s = delimiter;
            for(int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            s += groupStart;
            s += base.StringRep(delimiter, indentation, indentationSymbol, "", "");
            s += delimiter;
            s += delimiter;
            for(int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            s += "StringRepresentation()=" + this.StringRepresentation();
            s += delimiter;
            for(int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            s += groupEnd;
            return s;
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

