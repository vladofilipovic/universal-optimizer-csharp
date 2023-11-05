
namespace uo.TargetSolution
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using uo.TargetProblem;
    using static uo.TargetSolution.DistanceCalculationCacheControlStatistics;

    public struct QualityOfSolution
    {
        public double ObjectiveValue { get; set; }
        public IEnumerable<double> ObjectiveValues { get; set; }
        public double FitnessValue { get; set; }
        public IEnumerable<double> FitnessValues { get; set; }
        public bool IsFeasible { get; set; }
    };

    public abstract class TargetSolution<R_co, A_co>
    {

        private bool _distanceCalculationCacheIsUsed;
        private int _distanceCalculationCacheMaxSize;

        private bool _evaluationCacheIsUsed;
        private int _evaluationCacheMaxSize;

        private string _name;

        private double _fitnessValue;
        private IEnumerable<double> _fitnessValues;
        private bool _isFeasible;
        private double _objectiveValue;
        private IEnumerable<double> _objectiveValues;

        private int _randomSeed;
        private Random _randomGenerator;

        private R_co _representation = default;

        public static EvaluationCacheControlStatistics EvaluationCacheCS = new EvaluationCacheControlStatistics(false, 42);

        public static DistanceCalculationCacheControlStatistics<R_co> RepresentationDistanceCacheCS = new DistanceCalculationCacheControlStatistics<R_co>(false, 42);

        public TargetSolution(
            string name,
            int randomSeed,
            double fitnessValue,
            List<double> fitnessValues,
            double objectiveValue,
            List<double> objectiveValues,
            bool isFeasible,
            bool evaluationCacheIsUsed,
            int evaluationCacheMaxSize,
            bool distanceCalculationCacheIsUsed,
            int distanceCalculationCacheMaxSize)
        {
            _name = name;
            if (randomSeed != 0)
            {
                _randomSeed = randomSeed;
            }
            else
            {
                _randomSeed = (int)DateTime.UtcNow.Ticks;
            }
            _randomGenerator = new Random(randomSeed);
            _fitnessValue = fitnessValue;
            _fitnessValues = fitnessValues;
            _objectiveValue = objectiveValue;
            _objectiveValues = objectiveValues;
            _isFeasible = isFeasible;
            _evaluationCacheIsUsed = evaluationCacheIsUsed;
            _evaluationCacheMaxSize = evaluationCacheMaxSize;
            EvaluationCacheCS = new EvaluationCacheControlStatistics(_evaluationCacheIsUsed, _evaluationCacheMaxSize);
            _distanceCalculationCacheIsUsed = distanceCalculationCacheIsUsed;
            _distanceCalculationCacheMaxSize = distanceCalculationCacheMaxSize;
            RepresentationDistanceCacheCS = new DistanceCalculationCacheControlStatistics<R_co>(distanceCalculationCacheIsUsed, distanceCalculationCacheMaxSize);
        }

        /// 
        ///         Property getter for the name of the target solution
        /// 
        ///         :return: name of the target solution instance 
        ///         :rtype: str
        ///         
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /// 
        ///         Property getter and setter for fitness value of the target solution
        /// 
        ///         
        public double FitnessValue
        {
            get
            {
                return _fitnessValue;
            }
            set
            {
                _fitnessValue = value;
            }
        }

        /// 
        ///         Property getter and setter for fitness values of the target solution
        /// 
        ///         
        public IEnumerable<double> FitnessValues
        {
            get
            {
                return _fitnessValues;
            }
            set
            {
                _fitnessValues = value;
            }
        }

        /// 
        ///         Property getter and setter for objective value of the target solution
        ///         
        public double ObjectiveValue
        {
            get
            {
                return _objectiveValue;
            }
            set
            {
                _objectiveValue = value;
            }
        }

        /// 
        ///         Property getter and setter for objective values of the target solution
        ///         
        public IEnumerable<double> ObjectiveValues
        {
            get
            {
                return _objectiveValues;
            }
            set
            {
                _objectiveValues = value;
            }
        }

        /// 
        ///         Property getter and setter for feasibility of the target solution
        /// 
        public bool IsFeasible
        {
            get
            {
                return _isFeasible;
            }
            set
            {
                _isFeasible = value;
            }
        }

        /// 
        ///         Property getter and setter for representation of the target solution
        /// 
        public R_co Representation
        {
            get
            {
                return Representation;
            }
            set
            {
                Representation = value;
            }
        }

        /// 
        ///         Argument of the target solution
        /// 
        ///         :param representation: internal representation of the solution
        ///         :type representation: R_co
        ///         :return: argument of the solution 
        ///         :rtype: A_co
        ///         
        public abstract A_co Argument(R_co representation);

        /// 
        ///         String representation of the target solution
        /// 
        ///         :param representation: internal representation of the solution
        ///         :type representation: R_co
        ///         :return: string representation of the solution 
        ///         :rtype: str
        ///         
        public virtual string StringRepresentation()
        {
            return Argument(Representation).ToString();
        }

        /// 
        ///         Random initialization of the solution
        /// 
        ///         :param problem: problem which is solved by solution
        ///         :type problem: `TargetProblem`
        ///         
        public abstract void InitRandom(TargetProblem problem);

        /// 
        ///         Obtain native representation from solution code of the `Solution` instance
        /// 
        ///         :param str representationStr: solution's representation as string (e.g. solution code)
        ///         :return: solution's native representation 
        ///         :rtype: R_co
        ///         
        public abstract R_co NativeRepresentation(string representationStr);


        /// 
        ///         Initialization of the solution, by setting its native representation 
        /// 
        ///         :param R_co representation: representation that will be ste to solution
        ///         :param `TargetProblem` problem: problem which is solved by solution
        ///         
        public abstract void InitFrom(R_co representation, TargetProblem problem);

        /// 
        ///         Quality calculation of the target solution
        /// 
        ///         :param R_co representation: native representation of the solution for which objective value, fitness and feasibility are calculated
        ///         :param TargetProblem problem: problem that is solved
        ///         :return: objective value, fitness value and feasibility of the solution instance 
        ///         :rtype: `QualityOfSolution`
        ///         
        public abstract QualityOfSolution CalculateQualityDirectly(object representation, object problem);

        /// 
        ///         Calculate fitness, objective and feasibility of the solution, with optional cache consultation
        /// 
        ///         :param TargetProblem TargetProblem: problem that is solved
        ///         :return: objective value, fitness value and feasibility of the solution instance 
        ///         :rtype: `QualityOfSolution`
        ///         
        public virtual QualityOfSolution CalculateQuality(TargetProblem targetProblem)
        {
            QualityOfSolution qos;
            var eccs = EvaluationCacheCS;
            if (eccs.IsCaching)
            {
                eccs.IncrementCacheRequestCount();
                string rep = this.StringRepresentation();
                if (eccs.Cache.ContainsKey(rep))
                {
                    eccs.IncrementCacheHitCount();
                    return eccs.Cache[rep];
                }
                qos = this.CalculateQualityDirectly(this.Representation, targetProblem);
                if (eccs.Cache.Count >= eccs.MaxCacheSize)
                {
                    /// removing random
                    var code = eccs.Cache.ElementAt(_randomGenerator.Next(0, eccs.Cache.Count)).Key;
                    eccs.Cache.Remove(code);
                }
                eccs.Cache[rep] = qos;
                return qos;
            }
            else
            {
                qos = this.CalculateQualityDirectly(this.Representation, targetProblem);
                return qos;
            }
        }

        /// 
        ///         Evaluate current target solution
        /// 
        ///         :param TargetProblem TargetProblem: problem that is solved
        ///         
        public virtual void Evaluate(TargetProblem targetProblem)
        {
            QualityOfSolution qos = this.CalculateQuality(targetProblem);
            this.ObjectiveValue = qos.ObjectiveValue;
            this.FitnessValue = qos.FitnessValue;
            this.IsFeasible = qos.IsFeasible;
        }

        /// 
        ///         Directly calculate distance between two solutions determined by its native representations
        /// 
        ///         :param `R_co` representation_1: native representation for the first solution
        ///         :param `R_co` representation_2: native representation for the second solution
        ///         :return: distance 
        ///         :rtype: float
        ///         
        public abstract double RepresentationDistanceDirectly(R_co representation_1, R_co representation_2);
   
        /// 
        ///         Calculate distance between two native representations, with optional cache consultation
        /// 
        ///         :param `R_co` representation_1: native representation for the first solution
        ///         :param `R_co` representation_2: native representation for the second solution
        ///         :return: distance 
        ///         :rtype: float
        ///         
        public virtual double RepresentationDistance(R_co representation_1, R_co representation_2)
        {
            double ret;
            var rdcs = RepresentationDistanceCacheCS;
            if (rdcs.IsCaching)
            {
                rdcs.IncrementCacheRequestCount();
                var pair = (representation_1, representation_2);
                if (rdcs.Cache.ContainsKey(pair))
                {
                    rdcs.IncrementCacheHitCount();
                    return rdcs.Cache[pair];
                }
                ret = this.RepresentationDistanceDirectly(representation_1, representation_2);
                if (rdcs.Cache.Count >= rdcs.MaxCacheSize)
                {
                    /// removing random
                    var code = rdcs.Cache.ElementAt(_randomGenerator.Next(0, rdcs.Cache.Count)).Key;
                    rdcs.Cache.Remove(code);
                }
                rdcs.Cache[pair] = ret;
                return ret;
            }
            else
            {
                ret = this.RepresentationDistanceDirectly(representation_1, representation_2);
                return ret;
            }
        }

        /// 
        ///         String representation of the target solution instance
        /// 
        ///         :param delimiter: delimiter between fields
        ///         :type delimiter: str
        ///         :param indentation: level of indentation
        ///         :type indentation: int, optional, default value 0
        ///         :param indentationSymbol: indentation symbol
        ///         :type indentationSymbol: str, optional, default value ''
        ///         :param groupStart: group start string 
        ///         :type groupStart: str, optional, default value '{'
        ///         :param groupEnd: group end string 
        ///         :type groupEnd: str, optional, default value '}'
        ///         :return: string representation of instance that controls output
        ///         :rtype: str
        ///         
        public virtual string StringRep(
            string delimiter,
            int indentation = 0,
            string indentationSymbol = "",
            string groupStart = "{",
            string groupEnd = "}")
        {
            var s = delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += groupStart + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "name=" + this.Name + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "fitnessValue=" + this.FitnessValue.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "fitnessValues=" + this.FitnessValues.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "objectiveValue=" + this.ObjectiveValue.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "objectiveValues=" + this.ObjectiveValues.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "isFeasible=" + this.IsFeasible.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "representation()=" + this.Representation.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "evaluationCacheCS(static)=" + EvaluationCacheCS.StringRep(delimiter, indentation + 1, indentationSymbol, "{", "}");
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "_RepresentationDistanceCacheCS(static)=" + RepresentationDistanceCacheCS.StringRep(delimiter, indentation + 1, indentationSymbol, "{", "}") + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += groupEnd;
            return s;
        }

        /// 
        ///         String representation of the target solution instance
        /// 
        ///         :return: string representation of the target solution instance
        ///         :rtype: str
        ///         
        public override string ToString()
        {
            return this.StringRep("|");
        }

    }
}
