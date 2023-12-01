/// <summary>
/// This module is used for obtaining execution parameters for execution of the optimizers for 
/// max ones problem.
/// </summary>
namespace SingleObjective.Teaching.FunctionOneVariableProblem {

    using CommandLine;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class CommandLineHelper {

        [Verb("variable_neighborhood_search", HelpText = "Execute VNS metaheuristic for 'function_one_variable_problem'.")]
        public class VariableNeighborhoodSearchOptions
        {
            [Option("OptimizationType", Required = true, Default = "minimization", HelpText = "Decide if minimization or maximization will be executed. Possible values: minimization, maximization")]
            public required string OptimizationType { get; set; }

            [Option("--writeToOutputFile", Required = true, Default = true, HelpText = "Should results of metaheuristic execution be written to output file.")]
            public bool WriteToOutputFile { get; set; }

            [Option("--outputFilePath", Required = true, Default = "output/out.txt", HelpText = "File path of the output file. File path '' means that it is within 'outputs' folder.")]
            public required string OutputFilePath { get; set; }

            [Option("--outputFileNameAppendTimeStamp", Required = true, Default = false, HelpText = "Should timestamp be automatically added to the name of the output file.")]
            public bool OutputFileNameAppendTimeStamp { get; set; }

            [Option("--outputFields", Required = true, Default = "iteration, evaluation, self.bestSolution.argument()", HelpText = "Comma-separated list of fields whose values will be outputted during algorithm execution. Fields 'iteration, evaluation' means that current iterations and current evaluation will be outputted.")]
            public required string OutputFields { get; set; }

            [Option("--outputMoments", Required = true, Default = "afterAlgorithm, afterIteration", HelpText = "Comma-separated list of moments when values will be outputted during algorithm execution. List contains of following elements: 'beforeAlgorithm', 'afterAlgorithm', 'beforeIteration', 'afterIteration', 'beforeEvaluation', 'afterEvaluation', 'beforeStepInIteration', 'afterStepInIteration'. Moments 'afterAlgorithm' means that result will be outputted after algorithm.")]
            public required string OutputMoments { get; set; }

            [Option("--inputFilePath", Required = true, Default = "inputs/function_one_variable_problem/dim_25.txt", HelpText = "Input file path for the instance of the problem.")]
            public required string InputFilePath { get; set; }
        }

        [Verb("idle", HelpText = "Execute idle algorithm for the 'function_one_variable_problem'.")]
        public class IdleOptions
        {
        }

            public static void parse_arguments() {
            parser_vns.add_argument("", type: str, @default: "", help: " ");
            parser_vns.add_argument("--inputFormat", type: str, choices: new List<string> {
                "txt",
                "idle"
            }, @default: "txt", help: "Input file format. ");
            parser_vns.add_argument("--finishCriteria", type: str, @default: "evaluations & seconds", help: "Finish criteria - list of fields separated by '&'. Currently, fields can be: 'evaluations', 'iterations', 'seconds'.");
            parser_vns.add_argument("--finishEvaluationsMax", type: @int, @default: 0, help: "Maximum numbers of evaluations during VNS execution. Value 0 means that there is no limit on number of evaluations.");
            parser_vns.add_argument("--finishIterationsMax", type: @int, @default: 0, help: "Maximum numbers of iterations during VNS execution. Value 0 means that there is no limit on number of iterations.");
            parser_vns.add_argument("--finishSecondsMax", type: @int, @default: 0, help: "Maximum time for execution (in seconds).\n Value 0 means that there is no limit on execution time.");
            parser_vns.add_argument("--randomSeed", type: @int, @default: 0, help: "Random seed for the VNS execution. Value 0 means that random seed will be obtained from system timer.");
            parser_vns.add_argument("--solutionEvaluationCacheIsUsed", type: @bool, @default: false, help: "Should caching be used during evaluation.");
            parser_vns.add_argument("--solutionEvaluationCacheMaxSize", type: @int, @default: 0, help: "Maximum cache size for cache used in solutions evaluation. Value 0 means that there is no limit on cache size.");
            parser_vns.add_argument("--solutionDistanceCalculationCacheIsUsed", type: @bool, @default: false, help: "Should caching be used during distance calculations for solution individual.");
            parser_vns.add_argument("--solutionDistanceCalculationCacheMaxSize", type: @int, @default: 0, help: "Maximum cache size for cache used in distance calculations between two solutions. Value 0 means that there is no limit on cache size.");
            parser_vns.add_argument("--additionalStatisticsKeep", type: str, @default: "None", help: "Comma-separated list of statistical data will be calculated and keep during solving. Currently, data within list can be: 'all_solution_code', 'distance_among_solutions'.");
            parser_vns.add_argument("--additionalStatisticsMaxLocalOptima", type: @int, @default: 3, help: "Parameter maximum number of local optima kept during execution.");
            parser_vns.add_argument("--kMin", type: @int, @default: 1, help: "VNS parameter k min.");
            parser_vns.add_argument("--kMax", type: @int, @default: 3, help: "VNS parameter k max.");
            parser_vns.add_argument("--localSearchType", type: str, choices: new List<string> {
                "LocalSearchBestImprovement",
                "LocalSearchFirstImprovement"
            }, @default: "LocalSearchBestImprovement", help: "VNS parameter that determines local search type.");
            parser_vns.add_argument("--solutionType", type: str, choices: new List<string> {
                "int"
            }, @default: "int", help: "VNS parameter that determines solution (representation) type.");
            parser_vns.add_argument("--solutionNumberOfIntervals", type: @int, @default: 1000, help: "Numbers of intervals within domain used for solution representation.");
            parser_vns.add_argument("--log", @default: "warning", help: "Provide logging level. Example --log debug', default='warning'");
            var parser_idle = subparsers.add_parser("idle", help: );
            return parser.parse_args();
        }
    }
}
