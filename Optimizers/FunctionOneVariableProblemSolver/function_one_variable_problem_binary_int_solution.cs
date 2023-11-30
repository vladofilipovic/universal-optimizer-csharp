namespace opt.SingleObjective.Teaching
{
    using uo.TargetProblem;


    using FunctionOneVariableProblem = Teaching.FunctionOneVariableProblem.FunctionOneVariableProblem.FunctionOneVariableProblem;

    using System.Collections.Generic;

    using System;

    using System.Linq;

    public static class FunctionOneVariableProblemBinaryIntSolution
    {

        public static object directory = Path(_file__).resolve();

        static FunctionOneVariableProblemBinaryIntSolution()
        {
            sys.path.append(directory);
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent.parent);
        }

        public class FunctionOneVariableProblemBinaryIntSolution
            : TargetSolution[intfloat]
        {

            private object _domain_from;

            private object _domain_to;

            private object _numberOfIntervals;

            public object representation;

            public FunctionOneVariableProblemBinaryIntSolution(
                double domain_from,
                double domain_to,
                int numberOfIntervals,
                int randomSeed = null,
                bool evaluationCacheIsUsed = false,
                int evaluationCacheMaxSize = 0,
                bool distanceCalculationCacheIsUsed = false,
                int distanceCalculationCacheMaxSize = 0)
                : base(randomSeed: randomSeed, fitnessValue: null, objectiveValue: null, isFeasible: false, evaluationCacheIsUsed: evaluationCacheIsUsed, evaluationCacheMaxSize: evaluationCacheMaxSize, distanceCalculationCacheIsUsed: distanceCalculationCacheIsUsed, distanceCalculationCacheMaxSize: distanceCalculationCacheMaxSize)
            {
                _domain_from = domain_from;
                _domain_to = domain_to;
                _numberOfIntervals = numberOfIntervals;
            }

            public virtual void _copy__()
            {
                var sol = base._copy__();
                sol.domain_from = domain_from;
                sol.domain_to = domain_to;
                sol.numberOfIntervals = numberOfIntervals;
                return sol;
            }

            public virtual object copy()
            {
                return _copy__();
            }

            public virtual object copy_to(object destination)
            {
                destination = _copy__();
            }

            public object domain_from
            {
                get
                {
                    return _domain_from;
                }
                set
                {
                    _domain_from = value;
                }
            }

            public object domain_to
            {
                get
                {
                    return _domain_to;
                }
                set
                {
                    _domain_to = value;
                }
            }

            public object numberOfIntervals
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

            public virtual void _make_to_beFeasible_helper__(object problem)
            {
                if (representation > numberOfIntervals)
                {
                    representation = numberOfIntervals;
                }
            }

            public virtual double argument(int representation)
            {
                return domain_from + representation * (domain_to - domain_from) / numberOfIntervals;
            }

            public virtual object InitRandom(object problem)
            {
                representation = randint(0, numberOfIntervals);
                _make_to_beFeasible_helper__(problem);
            }

            public virtual object InitFrom(int representation, object problem)
            {
                this.representation = representation;
            }

            public virtual object CalculateQualityDirectly(int representation, object problem)
            {
                var arg = argument(representation);
                var res = this.ReflectionGetPropertyValue(problem.Expression);
                return QualityOfSolution(res, res, true);
            }

            public virtual int NativeRepresentation(string representationStr)
            {
                var ret = Convert.ToInt32(representationStr, 2);
                return ret;
            }

            public static double RepresentationDistanceDirectly(object solution_code_1, string solution_code_2)
            {
                var rep_1 = this.NativeRepresentation(solution_code_1);
                var rep_2 = this.NativeRepresentation(solution_code_2);
                var result = (rep_1 ^ rep_2).count(true);
                return result;
            }

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
                s += base.stringRep(delimiter, indentation, indentationSymbol, "", "");
                s += delimiter;
                s += delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0))
                {
                    s += indentationSymbol;
                }
                s += "stringRepresentation()=" + this.stringRepresentation().ToString();
                s += delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0))
                {
                    s += indentationSymbol;
                }
                s += groupEnd;
                return s;
            }

            public override string ToString()
            {
                return StringRep("\n", 0, "   ", "{", "}");
            }

            public virtual string _repr__()
            {
                return StringRep("\n", 0, "   ", "{", "}");
            }

            public virtual string _format__(string spec)
            {
                return StringRep("\n", 0, "   ", "{", "}");
            }
        }
    }
}
