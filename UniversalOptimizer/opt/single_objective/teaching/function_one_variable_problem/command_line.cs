///  
/// The :mod:`~opt.single_objective.teaching.function_one_variable_problem.command_line` module is used for obtaining execution parameters for execution of the optimizers for max ones problem.
/// 
namespace single_objective.teaching.function_one_variable_problem {
    
    using sys;
    
    using Path = pathlib.Path;
    
    using os;
    
    using logging;
    
    using dt = datetime;
    
    using ArgumentParser = argparse.ArgumentParser;
    
    using System.Collections.Generic;
    
    public static class command_line {
        
        public static object directory = Path(_file__).resolve();
        
        static command_line() {
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent);
            sys.path.append(directory.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent);
            sys.path.append(directory.parent.parent.parent.parent.parent);
        }
        
        public static Dictionary<string, object> default_parameters_cl = new Dictionary<object, object> {
            {
                "algorithm",
                "variable_neighborhood_search"},
            {
                "optimization_type",
                "maximization"},
            {
                "writeToOutputFile",
                true},
            {
                "outputFilePath",
                "opt/single_objective/teaching/ones_count_problem/outputs/dimension_77.csv"},
            {
                "outputFileNameAppendTimeStamp",
                false},
            {
                "outputFields",
                "iteration, evaluation, bestSolution.fitnessValue, bestSolution.argument()"},
            {
                "outputMoments",
                "afterAlgorithm, afterEvaluation"},
            {
                "inputFilePath",
                "opt/single_objective/teaching/ones_count_problem/inputs/dimension_77.txt"},
            {
                "inputFormat",
                "txt"},
            {
                "finishCriteria",
                "evaluations & seconds"},
            {
                "finishEvaluationsMax",
                300},
            {
                "finishIterationsMax",
                0},
            {
                "finishSecondsMax",
                0},
            {
                "randomSeed",
                0},
            {
                "solutionEvaluationCacheIsUsed",
                false},
            {
                "solutionEvaluationCacheMaxSize",
                0},
            {
                "solutionDistanceCalculationCacheIsUsed",
                false},
            {
                "solutionDistanceCalculationCacheMaxSize",
                0},
            {
                "additionalStatisticsKeep",
                "None"},
            {
                "additionalStatisticsMaxLocalOptima",
                7},
            {
                "kMin",
                1},
            {
                "kMax",
                3},
            {
                "localSearchType",
                "local_search_best_improvement"},
            {
                "solutionType",
                ""},
            {
                "solutionNumberOfIntervals",
                1000}};
        
        ///  The `parse_arguments` function parses execution parameters for execution of the optimizers for max 
        ///         ones problem.
        /// 
        public static void parse_arguments() {
            var parser = ArgumentParser();
            var subparsers = parser.add_subparsers(dest: "algorithm");
            var parser_vns = subparsers.add_parser("variable_neighborhood_search", help: "Execute VNS metaheuristic for ones_count_problem.");
            parser_vns.add_argument("optimization_type", help: "Decide if minimization or maximization will be executed.", nargs: "?", choices: ("minimization", "maximization"));
            parser_vns.add_argument("--writeToOutputFile", type: @bool, @default: true, help: "Should results of metaheuristic execution be written to output file.");
            parser_vns.add_argument("--outputFilePath", type: str, @default: "output/out.txt", help: "File path of the output file. File path '' means that it is within 'outputs' folder.");
            parser_vns.add_argument("--outputFileNameAppendTimeStamp", type: @bool, @default: false, help: "Should timestamp be automatically added to the name of the output file.");
            parser_vns.add_argument("--outputFields", type: str, @default: "iteration, evaluation, self.bestSolution.argument()", help: "Comma-separated list of fields whose values will be outputted during algorithm execution. Fields 'iteration, evaluation' means that current iterations and current evaluation will be outputted.");
            parser_vns.add_argument("--outputMoments", type: str, @default: "afterAlgorithm, afterIteration", help: "Comma-separated list of moments when values will be outputted during algorithm execution. List contains of following elements: 'beforeAlgorithm', 'afterAlgorithm', 'beforeIteration', 'afterIteration', 'beforeEvaluation', 'afterEvaluation', 'beforeStepInIteration', 'afterStepInIteration'Moments 'afterAlgorithm' means that result will be outputted after algorithm.");
            parser_vns.add_argument("--inputFilePath", type: str, @default: "inputs/ones_count_problem/dim_25.txt", help: "Input file path for the instance of the problem. ");
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
                "local_search_best_improvement",
                "local_search_first_improvement"
            }, @default: "local_search_best_improvement", help: "VNS parameter that determines local search type.");
            parser_vns.add_argument("--solutionType", type: str, choices: new List<string> {
                "int"
            }, @default: "int", help: "VNS parameter that determines solution (representation) type.");
            parser_vns.add_argument("--solutionNumberOfIntervals", type: @int, @default: 1000, help: "Numbers of intervals within domain used for solution representation.");
            parser_vns.add_argument("--log", @default: "warning", help: "Provide logging level. Example --log debug', default='warning'");
            var parser_idle = subparsers.add_parser("idle", help: "Execute idle algorithm for ones_count_problem.");
            return parser.parse_args();
        }
    }
}
