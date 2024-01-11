
namespace UniversalOptimizer.TargetSolution
{
    using Serilog.Debugging;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UniversalOptimizer.TargetProblem;

    /// <summary>
    /// Class that abstracts target solution. 
    /// </summary>
    /// <typeparam name="R_co">The type for the solution representation.</typeparam>
    /// <typeparam name="A_co">The type for the solution arguments.</typeparam>
    public abstract class TargetSolution<R_co, A_co> : ICloneable 
    {
        private double _fitnessValue;
        private IEnumerable<double> _fitnessValues;
        private double _objectiveValue;
        private IEnumerable<double> _objectiveValues;
        private bool? _isFeasible;
        private int _randomSeed;
        private Random _randomGenerator;
        private R_co? _representation;

        public static EvaluationCacheControlStatistics? EvaluationCacheCS = null;

        public static DistanceCalculationCacheControlStatistics<R_co>? RepresentationDistanceCacheCS = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="TargetSolution{R_co, A_co}"/> class.
        /// </summary>
        /// <param name="randomSeed">The random seed.</param>
        /// <param name="fitnessValue">The fitness value.</param>
        /// <param name="fitnessValues">The fitness values.</param>
        /// <param name="objectiveValue">The objective value.</param>
        /// <param name="objectiveValues">The objective values.</param>
        /// <param name="isFeasible">The is feasible.</param>
        /// <param name="evaluationCacheIsUsed">if set to <c>true</c> [evaluation cache is used].</param>
        /// <param name="evaluationCacheMaxSize">Maximum size of the evaluation cache.</param>
        /// <param name="distanceCalculationCacheIsUsed">if set to <c>true</c> [distance calculation cache is used].</param>
        /// <param name="distanceCalculationCacheMaxSize">Maximum size of the distance calculation cache.</param>
        public TargetSolution(
            int? randomSeed,
            double fitnessValue,
            List<double> fitnessValues,
            double objectiveValue,
            List<double> objectiveValues,
            bool? isFeasible,
            bool evaluationCacheIsUsed,
            int evaluationCacheMaxSize,
            bool distanceCalculationCacheIsUsed,
            int distanceCalculationCacheMaxSize)
        {
            if (randomSeed is not null && randomSeed != 0)
            {
                _randomSeed = randomSeed.GetValueOrDefault();
            }
            else
            {
                _randomSeed = (int)DateTime.UtcNow.Ticks;
            }
            _randomGenerator = new Random(_randomSeed);
            _fitnessValue = fitnessValue;
            _fitnessValues = fitnessValues;
            _objectiveValue = objectiveValue;
            _objectiveValues = objectiveValues;
            _isFeasible = isFeasible;
            _representation = default;
            if(EvaluationCacheCS is null)
                EvaluationCacheCS = new EvaluationCacheControlStatistics(evaluationCacheIsUsed, evaluationCacheMaxSize);
            if (RepresentationDistanceCacheCS is null)
                RepresentationDistanceCacheCS = new DistanceCalculationCacheControlStatistics<R_co>(distanceCalculationCacheIsUsed, distanceCalculationCacheMaxSize);
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual object Clone() => throw new NotImplementedException();

        /// <summary>
        /// Property getter for random seed of the target solution.
        /// </summary>
        /// <value>
        /// The random seed.
        /// </value>
        /// 
        public int RandomSeed
        {
            get
            {
                return _randomSeed;
            }
        }

        /// <summary>
        /// Property getter and setter for fitness value of the target solution.
        /// </summary>
        /// <value>
        /// The fitness value.
        /// </value>
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

        /// <summary>
        /// Property getter and setter for fitness values of the target solution.
        /// </summary>
        /// <value>
        /// The fitness values.
        /// </value>
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

        /// <summary>
        /// Property getter and setter for objective value of the target solution.
        /// </summary>
        /// <value>
        /// The objective value.
        /// </value>
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

        /// <summary>
        /// Property getter and setter for objective values of the target solution.
        /// </summary>
        /// <value>
        /// The objective values.
        /// </value>
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

        /// <summary>
        /// Property getter and setter for feasibility of the target solution.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is feasible; otherwise, <c>false</c>.
        /// </value>
        /// 
        public bool? IsFeasible
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

        /// <summary>
        /// Gets the quality single.
        /// </summary>
        /// <value>
        /// The quality single.
        /// </value>
        public QualityOfSolution QualitySingle        
        {
            get
            {
                return new QualityOfSolution(objectiveValue: _objectiveValue, fitnessValue: _fitnessValue, isFeasible: _isFeasible);
            }
        }

        /// <summary>
        /// Gets the quality multi.
        /// </summary>
        /// <value>
        /// The quality multi.
        /// </value>
        public QualityOfSolution QualityMulti
        {
            get
            {
                return new QualityOfSolution(objectiveValues: _objectiveValues, fitnessValues: _fitnessValues, isFeasible: _isFeasible);
            }
        }

        /// <summary>
        /// Property getter and setter for representation of the target solution.
        /// </summary>
        /// <value>
        /// The representation.
        /// </value>
        /// 
        public R_co? Representation
        {
            get
            {
                return _representation;
            }
            set
            {
                _representation = value;
            }
        }

        /// <summary>
        /// Obtains the feasible representation.
        /// </summary>
        /// <param name="problem">The problem.</param>
        /// <returns></returns>
        public virtual R_co ObtainFeasibleRepresentation(TargetProblem problem)
        {
            if(Representation is null)
                throw new ArgumentNullException(nameof(Representation));
            return Representation;
        }

        public virtual void CopyFrom(TargetSolution<R_co, A_co> original)
        {
            this._fitnessValue = original._fitnessValue;
            this._fitnessValues = original._fitnessValues;
            this._isFeasible= original._isFeasible;
            this._objectiveValue = original._objectiveValue;
            this._objectiveValues = original._objectiveValues;
            this._randomSeed = original._randomSeed;
            this._randomGenerator = original._randomGenerator;
            this._representation = original._representation;
        }

        /// <summary>
        /// Arguments of the target solution
        /// </summary>
        /// <param name='representation'> internal representation of the solution
        /// :type representation: R_co
        /// </param>
        /// <returns> arguments of the solution 
        /// return type A_co
        /// </returns>        
        public abstract A_co Argument(R_co? representation);

        /// <summary>
        /// String representation of the target solution.
        /// </summary>
        /// <returns></returns>
        /// 
        public string StringRepresentation() => Argument(Representation)!.ToString() ?? "Invalid representation";

        /// <summary>
        /// Random initialization of the solution.
        /// </summary>
        /// <param name="problem">The problem that is solved by solution.</param>
        /// 
        public abstract void InitRandom(TargetProblem problem);

        /// <summary>
        /// Obtain native representation from solution code of this instance.
        /// </summary>
        /// <param name="representationStr">The representation string.</param>
        /// <returns>R_co</returns>
        /// 
        public abstract R_co NativeRepresentation(string representationStr);


        /// <summary>
        /// Initialization of the solution, by setting its native representation.
        /// </summary>
        /// <param name="representation">The representation.</param>
        /// <param name="problem">The problem.</param>
        public abstract void InitFrom(R_co representation, TargetProblem problem);

        /// <summary>
        /// Calculates the quality of solution directly.
        /// </summary>
        /// <param name="representation">The native representation of the solution for which objective value, fitness and feasibility are calculated.</param>
        /// <param name="problem">The problem that is solved.</param>
        /// <returns></returns>
        public abstract QualityOfSolution CalculateQualityDirectly(R_co? representation, TargetProblem problem);

        /// <summary>
        /// Calculate fitness, objective and feasibility of the solution, with optional cache consultation.
        /// </summary>
        /// <param name="targetProblem">The target problem that is solved.</param>
        /// <returns>Objective value(s), fitness value(s) and feasibility of the solution instance</returns>
        public virtual QualityOfSolution CalculateQuality(TargetProblem targetProblem)
        {
            QualityOfSolution qos;
            var eccs = EvaluationCacheCS;
            if (eccs is not null && eccs.IsCaching)
            {
                eccs.IncrementCacheRequestCount();
                string rep = StringRepresentation();
                if (eccs.Cache.ContainsKey(rep))
                {
                    eccs.IncrementCacheHitCount();
                    return eccs.Cache[rep];
                }
                qos = CalculateQualityDirectly(Representation, targetProblem);
                if (eccs.Cache.Count >= eccs.MaxCacheSize)
                {
                    /// removing random
                    var code = eccs.Cache.ElementAt(_randomGenerator.Next(0, eccs.Cache.Count)).Key;
                    _ = eccs.Cache.Remove(code);
                }
                eccs.Cache[rep] = qos;
                return qos;
            }
            else
            {
                qos = CalculateQualityDirectly(Representation, targetProblem);
                return qos;
            }
        }

        /// <summary>
        /// Evaluates the specified target problem with this solution instance.
        /// </summary>
        /// <param name="targetProblem">The target problem that is solved.</param>
        public virtual void Evaluate(TargetProblem targetProblem)
        {
            QualityOfSolution qos = CalculateQuality(targetProblem);
            ObjectiveValue = qos.ObjectiveValue?? double.NaN;
            FitnessValue = qos.FitnessValue ?? double.NaN;
            IsFeasible = qos.IsFeasible;
        }

        /// <summary>
        /// Directly calculate distance between two solutions determined by its native representations.
        /// </summary>
        /// <param name="representation_1">The native representation for the first solution.</param>
        /// <param name="representation_2">The native representation for the second solution.</param>
        /// <returns>The distance.</returns>
        public abstract double RepresentationDistanceDirectly(R_co representation_1, R_co representation_2);

        /// <summary>
        /// Calculate distance between two native representations, with optional cache consultation.
        /// </summary>
        /// <param name="representation_1">The native representation for the first solution.</param>
        /// <param name="representation_2">The native representation for the second solution.</param>
        /// <returns>The distance.</returns>
        public virtual double RepresentationDistance(R_co representation_1, R_co representation_2)
        {
            double ret;
            var rdcs = RepresentationDistanceCacheCS;
            if (rdcs is not null && rdcs.IsCaching)
            {
                rdcs.IncrementCacheRequestCount();
                var pair = (representation_1, representation_2);
                if (rdcs.Cache.ContainsKey(pair))
                {
                    rdcs.IncrementCacheHitCount();
                    return rdcs.Cache[pair];
                }
                ret = RepresentationDistanceDirectly(representation_1, representation_2);
                if (rdcs.Cache.Count >= rdcs.MaxCacheSize)
                {
                    /// removing random
                    var code = rdcs.Cache.ElementAt(_randomGenerator.Next(0, rdcs.Cache.Count)).Key;
                    _ = rdcs.Cache.Remove(code);
                }
                rdcs.Cache[pair] = ret;
                return ret;
            }
            else
            {
                ret = RepresentationDistanceDirectly(representation_1, representation_2);
                return ret;
            }
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
        public string StringRep(
            string delimiter,
            int indentation = 0,
            string indentationSymbol = "",
            string groupStart = "{",
            string groupEnd = "}")
        {
            var s = delimiter;
            for(int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            s += groupStart + delimiter;
            for(int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            s += "fitnessValue=" + FitnessValue.ToString() + delimiter;
            for(int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            s += "fitnessValues=" + FitnessValues.ToString() + delimiter;
            for(int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            s += "objectiveValue=" + ObjectiveValue.ToString() + delimiter;
            for(int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            s += "objectiveValues=" + ObjectiveValues.ToString() + delimiter;
            for(int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            s += "isFeasible=" + IsFeasible.ToString() + delimiter;
            for(int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            s += "representation()=" + Representation!.ToString() + delimiter;
            for(int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            if(EvaluationCacheCS is not null)
                s += "EvaluationCacheCS(static)=" + EvaluationCacheCS.StringRep(delimiter, indentation + 1, indentationSymbol, "{", "}");
            else
                s += "EvaluationCacheCS(static)=null" + delimiter;
            for (int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            if (RepresentationDistanceCacheCS is not null)
                s += "RepresentationDistanceCacheCS(static)=" + RepresentationDistanceCacheCS.StringRep(delimiter, indentation + 1, indentationSymbol, "{", "}") + delimiter;
            else
                s += "RepresentationDistanceCacheCS(static)=null" + delimiter;
            for (int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            s += groupEnd;
            return s;
        }

        /// <summary>
        /// String representation of the target solution instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString() => StringRep("|");
    }
}
