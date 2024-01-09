namespace SingleObjective.Teaching.FunctionOneVariableProblem
{
    using UniversalOptimizer.utils;
    using UniversalOptimizer.TargetProblem;
    using UniversalOptimizer.TargetSolution;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;

    public class FunctionOneVariableProblemBinaryUIntSolution : TargetSolution<uint, double>
    {

        private double _domainFrom;

        private double _domainTo;

        private uint _numberOfIntervals;

        private uint _representation;

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionOneVariableProblemBinaryUIntSolution"/> class.
        /// </summary>
        /// <param name="domainFrom">The domain from.</param>
        /// <param name="domainTo">The domain to.</param>
        /// <param name="numberOfIntervals">The number of intervals.</param>
        /// <param name="randomSeed">The random seed.</param>
        /// <param name="evaluationCacheIsUsed">if set to <c>true</c> [evaluation cache is used].</param>
        /// <param name="evaluationCacheMaxSize">Maximum size of the evaluation cache.</param>
        /// <param name="distanceCalculationCacheIsUsed">if set to <c>true</c> [distance calculation cache is used].</param>
        /// <param name="distanceCalculationCacheMaxSize">Maximum size of the distance calculation cache.</param>
        public FunctionOneVariableProblemBinaryUIntSolution(
            double domainFrom,
            double domainTo,
            uint numberOfIntervals,
            int randomSeed,
            bool evaluationCacheIsUsed = false,
            int evaluationCacheMaxSize = 0,
            bool distanceCalculationCacheIsUsed = false,
            int distanceCalculationCacheMaxSize = 0)
            : base(randomSeed: randomSeed, fitnessValue: Double.NegativeInfinity, fitnessValues: new List<double>(), objectiveValue: double.NegativeInfinity, objectiveValues: new List<double>(), isFeasible: false, evaluationCacheIsUsed: evaluationCacheIsUsed, evaluationCacheMaxSize: evaluationCacheMaxSize, distanceCalculationCacheIsUsed: distanceCalculationCacheIsUsed, distanceCalculationCacheMaxSize: distanceCalculationCacheMaxSize)
        {
            _domainFrom = domainFrom;
            _domainTo = domainTo;
            _numberOfIntervals = numberOfIntervals;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public override object Clone()
        {
            return new FunctionOneVariableProblemBinaryUIntSolution(this._domainFrom, this._domainTo,
                this._numberOfIntervals, this.RandomSeed);
        }
        /// <summary>
        /// Gets or sets the domain from.
        /// </summary>
        /// <value>
        /// The domain from.
        /// </value>
        public double DomainFrom
        {
            get
            {
                return _domainFrom;
            }
            set
            {
                _domainFrom = value;
            }
        }

        /// <summary>
        /// Gets or sets the domain to.
        /// </summary>
        /// <value>
        /// The domain to.
        /// </value>
        public double DomainTo
        {
            get
            {
                return _domainTo;
            }
            set
            {
                _domainTo = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of intervals.
        /// </summary>
        /// <value>
        /// The number of intervals.
        /// </value>
        public uint NumberOfIntervals
        {
            get
            {
                return _numberOfIntervals;
            }
            set
            {
                _numberOfIntervals = value;
            }
        }

        /// <summary>
        /// Makes to be feasible helper.
        /// </summary>
        /// <param name="problem">The problem.</param>
        private void MakeToBeFeasibleHelper(FunctionOneVariableProblem problem)
        {
            if (_representation > _numberOfIntervals)
            {
                _representation = _numberOfIntervals;
            }
        }

        /// <summary>
        /// Arguments of the target solution
        /// </summary>
        /// <param name="representation">internal representation of the solution
        /// :type representation: R_co</param>
        /// <returns>
        /// arguments of the solution
        /// return type A_co
        /// </returns>
        public override double Argument(uint representation) => DomainFrom + representation * (DomainTo - DomainFrom) / NumberOfIntervals;

        /// <summary>
        /// Random initialization of the solution.
        /// </summary>
        /// <param name="problem">The problem that is solved by solution.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        public override void InitRandom(TargetProblem problem)
        {
            if (problem == null)
                throw new ArgumentNullException(string.Format("Parameter '{0}' is null.", nameof(problem)));
            if (problem is not FunctionOneVariableProblem)
                throw new ArgumentException(string.Format("Parameter '{0}' have not type 'OnesCountProblem'.", nameof(problem)));
            FunctionOneVariableProblem fovProblem = (FunctionOneVariableProblem)problem;
            _representation = (uint) RandomNumberGenerator.GetInt32((int)NumberOfIntervals);
            MakeToBeFeasibleHelper(fovProblem);
        }

        /// <summary>
        /// Initialization of the solution, by setting its native representation.
        /// </summary>
        /// <param name="representation">The representation.</param>
        /// <param name="problem">The problem.</param>
        public override void InitFrom(uint representation, TargetProblem problem) => _representation = representation;

        /// <summary>
        /// Calculates the quality of solution directly.
        /// </summary>
        /// <param name="representation">The native representation of the solution for which objective value, fitness and feasibility are calculated.</param>
        /// <param name="problem">The problem that is solved.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Problem type is not valid.</exception>
        public override QualityOfSolution CalculateQualityDirectly(uint representation, TargetProblem problem)
        {
            var arg = Argument(representation);
            if (problem is not FunctionOneVariableProblem)
                throw new ArgumentException("Problem type is not valid.");
            FunctionOneVariableProblem f1vp = (FunctionOneVariableProblem)problem;
            double res = (double)f1vp.Expression.ReflectionEvaluateExpression()!;
            return new QualityOfSolution(
                fitnessValue: res,
                objectiveValue: res,
                isFeasible: true);
        }

        /// <summary>
        /// Obtain native representation from solution code of this instance.
        /// </summary>
        /// <param name="representationStr">The representation string.</param>
        /// <returns>
        /// int
        /// </returns>
        public override uint NativeRepresentation(string representationStr)
        {
            var ret = Convert.ToUInt32(representationStr, 2);
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
        public override double RepresentationDistanceDirectly(uint representation_1, uint representation_2)
        {
            uint x = representation_1 ^ representation_2;
            //return OnesCountProblemBinaryUIntSolution x;
        }

        /// <summary>
        /// String representation of the target solution instance.
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
            s += "stringRepresentation()=" + this.StringRepresentation().ToString();
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
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => StringRep("\n", 0, "   ", "{", "}");

    }
}
