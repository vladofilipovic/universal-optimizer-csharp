/// <summary>
/// This module is used for obtaining execution parameters for execution of the optimizers for 
/// function one variable problem.
/// </summary>
namespace SingleObjective.Teaching.FunctionOneVariableProblem {

    using CommandLine;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class CommandLineHelper {

        [Verb("variable_neighborhood_search", HelpText = "Execute VNS metaheuristic for 'function_one_variable_problem'.")]
        public class VariableNeighborhoodSearchOptions
        {
            [Option("optimizationType", Required = true, Default = "minimization", HelpText = "Decide if minimization or maximization will be executed. Possible values: minimization, maximization")]
            public required string OptimizationType { get; set; }

            [Option("writeToOutputFile", Required = true, Default = true, HelpText = "Should results of metaheuristic execution be written to output file.")]
            public bool WriteToOutputFile { get; set; }

            [Option("outputFilePath", Required = true, Default = "output/out.txt", HelpText = "File path of the output file. File path '' means that it is within 'outputs' folder.")]
            public required string OutputFilePath { get; set; }

            [Option("outputFileNameAppendTimeStamp", Required = true, Default = false, HelpText = "Should timestamp be automatically added to the name of the output file.")]
            public bool OutputFileNameAppendTimeStamp { get; set; }

            [Option("outputFields", Required = true, Default = "iteration, evaluation, self.bestSolution.argument()", HelpText = "Comma-separated list of fields whose values will be outputted during algorithm execution. Fields 'iteration, evaluation' means that current iterations and current evaluation will be outputted.")]
            public required string OutputFields { get; set; }

            [Option("outputMoments", Required = true, Default = "afterAlgorithm, afterIteration", HelpText = "Comma-separated list of moments when values will be outputted during algorithm execution. List contains of following elements: 'beforeAlgorithm', 'afterAlgorithm', 'beforeIteration', 'afterIteration', 'beforeEvaluation', 'afterEvaluation', 'beforeStepInIteration', 'afterStepInIteration'. Moments 'afterAlgorithm' means that result will be outputted after algorithm.")]
            public required string OutputMoments { get; set; }

            [Option("inputFilePath", Required = true, Default = "inputs/function_one_variable_problem/dim_25.txt", HelpText = "Input file path for the instance of the problem.")]
            public required string InputFilePath { get; set; }

            [Option("inputFormat", Required = true, Default = "txt", HelpText = "Input file format. Choices: 'txt', 'idle'.")]
            public required string InputFormat { get; set; }

            [Option("finishCriteria", Required = true, Default = "evaluations & seconds", HelpText = "Finish criteria - list of fields separated by '&'. Currently, fields can be: 'evaluations', 'iterations', 'seconds'.")]
            public required string FinishCriteria { get; set; }

            [Option("finishEvaluationsMax", Required = true, Default = 0, HelpText = "Maximum numbers of evaluations during VNS execution. Value 0 means that there is no limit on number of evaluations.")]
            public int FinishEvaluationsMax { get; set; }

            [Option("finishIterationsMax", Required = true, Default = 0, HelpText = "Maximum numbers of iterations during VNS execution. Value 0 means that there is no limit on number of iterations.")]
            public int FinishIterationsMax { get; set; }

            [Option("finishSecondsMax", Required = true, Default = 0, HelpText = "Maximum time for execution (in seconds).\n Value 0 means that there is no limit on execution time.")]
            public int FinishSecondsMax { get; set; }

            [Option("randomSeed", Required = true, Default = 0, HelpText = "Random seed for the VNS execution. Value 0 means that random seed will be obtained from system timer.")]
            public int RandomSeed { get; set; }

            [Option("solutionEvaluationCacheIsUsed", Required = true, Default = false, HelpText = "Should caching be used during evaluation.")]
            public bool SolutionEvaluationCacheIsUsed { get; set; }

            [Option("solutionEvaluationCacheMaxSize", Required = true, Default = 0, HelpText = "Maximum cache size for cache used in solutions evaluation. Value 0 means that there is no limit on cache size.")]
            public int SolutionEvaluationCacheMaxSize { get; set; }

            [Option("solutionDistanceCalculationCacheIsUsed", Required = true, Default = false, HelpText = "Should caching be used during distance calculations for solution individual.")]
            public bool SolutionDistanceCalculationCacheIsUsed { get; set; }

            [Option("solutionDistanceCalculationCacheMaxSize", Required = true, Default = 0, HelpText = "Maximum cache size for cache used in distance calculations between two solutions. Value 0 means that there is no limit on cache size.")]
            public int SolutionDistanceCalculationCacheMaxSize { get; set; }

            [Option("additionalStatisticsIsActive", Required = true, Default = false, HelpText = "Indicator if additional statistics is active, or not.")]
            public required bool AdditionalStatisticsIsActive { get; set; }

            [Option("additionalStatisticsKeep", Required = true, Default = "none", HelpText = "Comma-separated list of statistical data will be calculated and keep during solving. Currently, data within list can be: 'allSolutionCode', 'distance_among_solutions'.")]
            public required string AdditionalStatisticsKeep { get; set; }

            [Option("additionalStatisticsMaxLocalOptimaCount", Required = true, Default = 0, HelpText = "Parameter maximum number of local optima kept during execution.")]
            public int AdditionalStatisticsMaxLocalOptimaCount { get; set; }

            [Option("kMin", Required = true, Default = 1, HelpText = "VNS parameter k min.")]
            public int KMin { get; set; }

            [Option("kMax", Required = true, Default = 3, HelpText = "VNS parameter k max.")]
            public int KMax { get; set; }

            [Option("localSearchType", Required = true, Default = "localSearchBestImprovement", HelpText = "VNS parameter that determines local search type. It can have one of values: 'localSearchBestImprovement', 'localSearchFirstImprovement'.")]
            public required string LocalSearchType { get; set; }

            [Option("solutionType", Required = true, Default = "int", HelpText = "VNS parameter that determines solution (representation) type.")]
            public required string SolutionType { get; set; }

            [Option("solutionNumberOfIntervals", Required = true, Default = 10000, HelpText = "Numbers of intervals within domain used for solution representation.")]
            public uint SolutionNumberOfIntervals { get; set; }
        }

        [Verb("idle", HelpText = "Execute idle algorithm for the 'function_one_variable_problem'.")]
        public class IdleOptions
        {
        }
    }
}
