namespace UniversalOptimizer.Solution
{
    using Serilog.Debugging;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UniversalOptimizer.Problem;

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
    }
}
