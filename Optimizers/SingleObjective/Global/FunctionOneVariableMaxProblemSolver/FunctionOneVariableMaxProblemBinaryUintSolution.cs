namespace SingleObjective.Teaching.FunctionOneVariableProblem
{
    using UniversalOptimizer.utils;
    using UniversalOptimizer.Problem;
    using UniversalOptimizer.Solution;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public class FunctionOneVariableMaxProblemBinaryUintSolution : Solution<uint, double>
    {
        private double _domainFrom;
        private double _domainTo;
        private uint _numberOfIntervals;

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionOneVariableMaxProblemBinaryUintSolution"/> class.
        /// </summary>
        /// <param name="domainFrom">The domain from.</param>
        /// <param name="domainTo">The domain to.</param>
        /// <param name="numberOfIntervals">The number of intervals.</param>
        /// <param name="randomSeed">The random seed.</param>
        /// <param name="evaluationCacheIsUsed">if set to <c>true</c> [evaluation cache is used].</param>
        /// <param name="evaluationCacheMaxSize">Maximum size of the evaluation cache.</param>
        /// <param name="distanceCalculationCacheIsUsed">if set to <c>true</c> [distance calculation cache is used].</param>
        /// <param name="distanceCalculationCacheMaxSize">Maximum size of the distance calculation cache.</param>
        public FunctionOneVariableMaxProblemBinaryUintSolution(
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
            var clone = new FunctionOneVariableMaxProblemBinaryUintSolution(this._domainFrom, this._domainTo,
                this._numberOfIntervals, this.RandomSeed);
            clone.IsFeasible = this.IsFeasible;
            clone.FitnessValue = this.FitnessValue;
            clone.FitnessValues = this.FitnessValues;
            clone.ObjectiveValue = this.ObjectiveValue;
            clone.ObjectiveValues = this.ObjectiveValues;
            clone.Representation = this.Representation;
            return clone;
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
        /// Obtains the feasible representation.
        /// </summary>
        /// <param name="problem">The problem.</param>
        /// <returns></returns>
        public override uint ObtainFeasibleRepresentation(Problem problem)
        {
            if (Representation > _numberOfIntervals)
                return Representation % _numberOfIntervals;
            return Representation;
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
        public override void InitRandom(Problem problem)
        {
            if (problem == null)
                throw new ArgumentNullException(string.Format("Parameter '{0}' is null.", nameof(problem)));
            if (problem is not FunctionOneVariableMaxProblem)
                throw new ArgumentException(string.Format("Parameter '{0}' have not type 'OnesCountProblem'.", nameof(problem)));
            FunctionOneVariableMaxProblem fovProblem = (FunctionOneVariableMaxProblem)problem;
            Representation = (uint) RandomNumberGenerator.GetInt32((int)NumberOfIntervals);
            Representation = ObtainFeasibleRepresentation(fovProblem);
        }

        /// <summary>
        /// Initialization of the solution, by setting its native representation.
        /// </summary>
        /// <param name="representation">The representation.</param>
        /// <param name="problem">The problem.</param>
        public override void InitFrom(uint representation, Problem problem) => Representation = representation;

        /// <summary>
        /// Calculates the quality of solution directly.
        /// </summary>
        /// <param name="representation">The native representation of the solution for which objective value, fitness and feasibility are calculated.</param>
        /// <param name="problem">The problem that is solved.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Problem type is not valid.</exception>
        public override QualityOfSolution CalculateQualityDirectly(uint representation, Problem problem)
        {
            double arg = Argument(representation);
            if (problem is not FunctionOneVariableMaxProblem)
                throw new ArgumentException("Problem type is not valid.");
            FunctionOneVariableMaxProblem f1vp = (FunctionOneVariableMaxProblem)problem;
            double res = double.NaN;
            object? resObj = f1vp.Expression.ReflectionEvaluateFunctionOneVariable(arg);
            if(resObj is not null)
                res = (double)f1vp.Expression.ReflectionEvaluateFunctionOneVariable(arg)!;
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
            return x.CountOnes();
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
            s.Append("stringRepresentation()=" + this.StringRepresentation().ToString());
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
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => StringRep("\n", 0, "   ", "{", "}");

    }
}
