namespace SingleObjective.Teaching.FunctionOneVariableProblem
{
    using UniversalOptimizer.TargetProblem;

    using UniversalOptimizer.TargetSolution;

    using System.Collections.Generic;

    using System;

    using System.Linq;
    using UniversalOptimizer.utils;



    public class FunctionOneVariableProblemBinaryIntSolution : TargetSolution<int, double>
    {

        private double _domainFrom;

        private double _domainTo;

        private int _numberOfIntervals;

        private int _representation;

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionOneVariableProblemBinaryIntSolution"/> class.
        /// </summary>
        /// <param name="domainFrom">The domain from.</param>
        /// <param name="domainTo">The domain to.</param>
        /// <param name="numberOfIntervals">The number of intervals.</param>
        /// <param name="randomSeed">The random seed.</param>
        /// <param name="evaluationCacheIsUsed">if set to <c>true</c> [evaluation cache is used].</param>
        /// <param name="evaluationCacheMaxSize">Maximum size of the evaluation cache.</param>
        /// <param name="distanceCalculationCacheIsUsed">if set to <c>true</c> [distance calculation cache is used].</param>
        /// <param name="distanceCalculationCacheMaxSize">Maximum size of the distance calculation cache.</param>
        public FunctionOneVariableProblemBinaryIntSolution(
            double domainFrom,
            double domainTo,
            int numberOfIntervals,
            int randomSeed,
            bool evaluationCacheIsUsed = false,
            int evaluationCacheMaxSize = 0,
            bool distanceCalculationCacheIsUsed = false,
            int distanceCalculationCacheMaxSize = 0)
            : base("FunctionOneVariableProblemBinaryIntSolution", randomSeed: randomSeed, fitnessValue: Double.NegativeInfinity, fitnessValues: new List<double>(), objectiveValue: double.NegativeInfinity, objectiveValues: new List<double>(), isFeasible: false, evaluationCacheIsUsed: evaluationCacheIsUsed, evaluationCacheMaxSize: evaluationCacheMaxSize, distanceCalculationCacheIsUsed: distanceCalculationCacheIsUsed, distanceCalculationCacheMaxSize: distanceCalculationCacheMaxSize)
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
        public object Clone()
        {
            throw new NotImplementedException();
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
        public int NumberOfIntervals
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
        private void MakeToBeFeasibleHelper(object problem)
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
        public override double Argument(int representation)
        {
            return DomainFrom + representation * (DomainTo - DomainFrom) / NumberOfIntervals;
        }


        /// <summary>
        /// Random initialization of the solution.
        /// </summary>
        /// <param name="problem"></param>
        public override void InitRandom(TargetProblem problem)
        {
            _representation = (new Random()).Next(NumberOfIntervals);
            MakeToBeFeasibleHelper(problem);
        }

        /// <summary>
        /// Initialization of the solution, by setting its native representation.
        /// </summary>
        /// <param name="representation">The representation.</param>
        /// <param name="problem">The problem.</param>
        public override void InitFrom(int representation, TargetProblem problem)
        {
            _representation = representation;
        }

        /// <summary>
        /// Calculates the quality of solution directly.
        /// </summary>
        /// <param name="representation">The native representation of the solution for which objective value, fitness and feasibility are calculated.</param>
        /// <param name="problem">The problem that is solved.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Problem type is not valid.</exception>
        public override QualityOfSolution CalculateQualityDirectly(int representation, TargetProblem problem)
        {
            var arg = Argument(representation);
            if (problem is not FunctionOneVariableProblem)
                throw new Exception("Problem type is not valid.");
            FunctionOneVariableProblem f1vp = (FunctionOneVariableProblem)problem;
            double res = (double)f1vp.Expression.ReflectionEvaluateExpression();
            return new QualityOfSolution()
            {
                FitnessValue = res,
                FitnessValues = null,
                ObjectiveValue = res,
                ObjectiveValues = null,
                IsFeasible = true
            };
        }

        /// <summary>
        /// Obtain native representation from solution code of this instance.
        /// </summary>
        /// <param name="representationStr">The representation string.</param>
        /// <returns>
        /// int
        /// </returns>
        public override int NativeRepresentation(string representationStr)
        {
            var ret = Convert.ToInt32(representationStr, 2);
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
        public override double RepresentationDistanceDirectly(int representation_1, int representation_2)
        {
            int x = representation_1 ^ representation_2;
            return x;
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
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += groupStart;
            s += base.StringRep(delimiter, indentation, indentationSymbol, "", "");
            s += delimiter;
            s += delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "stringRepresentation()=" + this.StringRepresentation().ToString();
            s += delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
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
        public override string ToString()
        {
            return StringRep("\n", 0, "   ", "{", "}");
        }

    }
}
