namespace UniversalOptimizer.TargetSolution
{
    using Serilog.Debugging;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UniversalOptimizer.TargetProblem;

    /// <summary>
    /// Quality of the solution - encompasses objective value, fitness and feasibility  
    /// </summary>
    public class QualityOfSolution
    {
        public double? ObjectiveValue { get; set; } = null;
        public IEnumerable<double>? ObjectiveValues { get; set; } = null;
        public double? FitnessValue { get; set; } = null;
        public IEnumerable<double>? FitnessValues { get; set; } = null;
        public bool? IsFeasible { get; set; } = null;

        public QualityOfSolution(double? objectiveValue = null,
            IEnumerable<double>? objectiveValues = null,
            double? fitnessValue = null,
            IEnumerable<double>? fitnessValues = null,
            bool? isFeasible = null
            )
        {
            ObjectiveValue = objectiveValue;
            ObjectiveValues = objectiveValues;
            FitnessValue = fitnessValue;    
            FitnessValues = fitnessValues;
            IsFeasible = isFeasible;
        }

        public static bool? IsFirstFitnessBetter(QualityOfSolution qos1, QualityOfSolution qos2, bool isMinimization)
        {
            double? fit1 = qos1.FitnessValue;
            double? fit2 = qos2.FitnessValue;
            if (fit1 is null)
            {
                if (fit2 is not null)
                    return false;
                else
                    return null;
            }
            else
            {
                if (fit2 is null)
                    return true;
                if ((isMinimization && fit1 < fit2) || (!isMinimization && fit1 > fit2))
                    return true;
                if (fit1 == fit2)
                    return null;
                return false;
            }
        }
    }
}
