
namespace UniversalOptimizer.Algorithm.Metaheuristic.VariableNeighborhoodSearch
{

    using UniversalOptimizer.Algorithm;

    using UniversalOptimizer.TargetProblem;

    using UniversalOptimizer.TargetSolution;

    using UniversalOptimizer.Algorithm.Metaheuristic.VariableNeighborhoodSearch;

    using System.Collections.Generic;

    using System;

    using System.Linq;


    /// <summary>
    /// Instance of this class represents constructor parameters for VNS algorithm.
    /// </summary>
    public class VnsOptimizerConstructionParameters<R_co, A_co>
    {
        public required AdditionalStatisticsControl AdditionalStatisticsControl { get; set; }
        public required FinishControl FinishControl { get; set; }
        public required TargetSolution<R_co, A_co> InitialSolution { get; set; }
        public int KMax { get; set; }
        public int KMin { get; set; }
        public required string LocalSearchType { get; set; }
        public required OutputControl OutputControl { get; set; }
        public required IProblemSolutionVnsSupport<R_co,A_co> ProblemSolutionVnsSupport { get; set; }
        public int RandomSeed { get; set; }
        public required TargetProblem TargetProblem { get; set; }   
      }

    /// <summary>
    /// Instance of this class encapsulate Variable Neighborhood Search optimization algorithm.
    /// </summary>
    /// <seealso cref="UniversalOptimizer.Algorithm.Metaheuristic.SingleSolutionMetaheuristic" />
    public class VnsOptimizer<R_co, A_co> : SingleSolutionMetaheuristic<R_co, A_co>
    {

        private object _implemented_local_searches; // dict(string, delegate)

        private int _k_current;

        private object _k_max;

        private object _k_min;

        private object _local_search_type;

        private object _ls_method;

        private object _problem_solution_vns_support;

        private object _shaking_method;

        public object currentSolution;

        public int iteration;

        public VnsOptimizer<R_co,A_co>(
            object finishControl,
            int randomSeed,
            object additionalStatisticsControl,
            object outputControl,
            object targetProblem,
            object initial_solution,
            object problem_solution_vns_support,
            int k_min,
            int k_max,
            string local_search_type)
            : base("VnsOptimizer", finishControl: finishControl, randomSeed: randomSeed, additionalStatisticsControl: additionalStatisticsControl, outputControl: outputControl, targetProblem: targetProblem, initial_solution: initial_solution)
        {
            _local_search_type = local_search_type;
            if (problem_solution_vns_support is not null)
            {
                if (problem_solution_vns_support is ProblemSolutionVnsSupport)
                {
                    _problem_solution_vns_support = problem_solution_vns_support;
                    _implemented_local_searches = new Dictionary<object, object> {
                            {
                                "LocalSearchBestImprovement",
                                _problem_solution_vns_support.LocalSearchBestImprovement},
                            {
                                "LocalSearchFirstImprovement",
                                _problem_solution_vns_support.LocalSearchFirstImprovement}};
                    if (!_implemented_local_searches.Contains(_local_search_type))
                    {
                        throw new ValueError("Value \'{}\' for VNS local_search_type is not supported".format(_local_search_type));
                    }
                    _ls_method = _implemented_local_searches[_local_search_type];
                    _shaking_method = _problem_solution_vns_support.shaking;
                }
                else
                {
                    _problem_solution_vns_support = problem_solution_vns_support;
                    _implemented_local_searches = null;
                    _ls_method = null;
                    _shaking_method = null;
                }
            }
            else
            {
                _problem_solution_vns_support = null;
                _implemented_local_searches = null;
                _ls_method = null;
                _shaking_method = null;
            }
            _k_min = k_min;
            _k_max = k_max;
            /// current value of the vns parameter k
            _k_current = null;
        }

        /// 
        /// Additional constructor, that creates new instance of class :class:`~uo.Algorithm.Metaheuristic.VariableNeighborhoodSearch.VnsOptimizer`. 
        /// 
        /// :param `VnsOptimizerConstructionParameters` construction_tuple: tuple with all constructor parameters
        /// 
        [classmethod]
        public static void from_construction_tuple(object cls, object construction_tuple)
        {
            return cls(construction_tuple.finishControl, construction_tuple.randomSeed, construction_tuple.additionalStatisticsControl, construction_tuple.OutputControl, construction_tuple.TargetProblem, construction_tuple.initial_solution, construction_tuple.problem_solution_vns_support, construction_tuple.k_min, construction_tuple.k_max, construction_tuple.local_search_type);
        }

        /// 
        /// Internal copy of the current instance of class :class:`~uo.Algorithm.Metaheuristic.VariableNeighborhoodSearch.VnsOptimizer`
        /// 
        /// :return: new instance of class :class:`~uo.Algorithm.Metaheuristic.VariableNeighborhoodSearch.VnsOptimizer` with the same properties
        /// return type :class:`uo.Algorithm.Metaheuristic.VariableNeighborhoodSearch.VnsOptimizer`        
        /// 
        public virtual void _copy__()
        {
            var vns_opt = deepcopy(this);
            return vns_opt;
        }

        /// 
        /// Copy the current instance of class :class:`~uo.Algorithm.Metaheuristic.VariableNeighborhoodSearch.VnsOptimizer`
        /// 
        /// :return: new instance of class :class:`~uo.Algorithm.Metaheuristic.VariableNeighborhoodSearch.VnsOptimizer` with the same properties
        /// return type :class:`uo.Algorithm.Metaheuristic.VariableNeighborhoodSearch.VnsOptimizer`        
        /// 
        public virtual void copy()
        {
            return _copy__();
        }

        /// 
        /// Property getter for the `k_min` parameter for VNS
        /// 
        /// :return: `k_min` parameter for VNS 
        /// return type int
        /// 
        public object k_min
        {
            get
            {
                return _k_min;
            }
        }

        /// 
        /// Property getter for the `k_max` parameter for VNS
        /// 
        /// :return: k_max parameter for VNS 
        /// return type int
        /// 
        public object k_max
        {
            get
            {
                return _k_max;
            }
        }

        /// 
        /// Initialization of the VNS algorithm
        /// 
        public override void Init()
        {
            _k_current = k_min;
            currentSolution.InitRandom(this.TargetProblem);
            currentSolution.evaluate(this.TargetProblem);
            this.CopyToBestSolution(currentSolution);
        }

        /// 
        /// One iteration within main loop of the VNS algorithm
        /// 
        public virtual object MainLoopIteration()
        {
            this.WriteOutputValuesIfNeeded("beforeStepInIteration", "shaking");
            if (!_shaking_method(_k_current, this.TargetProblem, currentSolution, this))
            {
                this.WriteOutputValuesIfNeeded("afterStepInIteration", "shaking");
                return false;
            }
            this.WriteOutputValuesIfNeeded("afterStepInIteration", "shaking");
            iteration += 1;
            while (_k_current <= _k_max)
            {
                this.WriteOutputValuesIfNeeded("beforeStepInIteration", "ls");
                currentSolution = _ls_method(_k_current, this.TargetProblem, currentSolution, this);
                this.WriteOutputValuesIfNeeded("afterStepInIteration", "ls");
                /// update auxiliary structure that keeps all solution codes
                this.additionalStatisticsControl.AddToAllSolutionCodesIfRequired(currentSolution.stringRepresentation());
                this.additionalStatisticsControl.AddToMoreLocalOptimaIfRequired(currentSolution.stringRepresentation(), currentSolution.fitnessValue, this.bestSolution.stringRepresentation());
                var new_is_better = this.IsFirstSolutionBetter(currentSolution, this.bestSolution);
                var make_move = new_is_better;
                if (new_is_better is null)
                {
                    if (currentSolution.stringRepresentation() == this.bestSolution.stringRepresentation())
                    {
                        make_move = false;
                    }
                    else
                    {
                        logger.debug("VnsOptimizer::MainLoopIteration: Same solution quality, generating random true with probability 0.5");
                        make_move = random() < 0.5;
                    }
                }
                if (make_move)
                {
                    this.CopyToBestSolution(currentSolution);
                    _k_current = k_min;
                }
                else
                {
                    _k_current += 1;
                }
            }
        }

        /// 
        /// String representation of the `VnsOptimizer` instance
        /// 
        /// :param delimiter: delimiter between fields
        /// :type delimiter: str
        /// :param indentation: level of indentation
        /// :type indentation: int, optional, default value 0
        /// :param indentationSymbol: indentation symbol
        /// :type indentationSymbol: str, optional, default value ''
        /// :param groupStart: group start string 
        /// :type groupStart: str, optional, default value '{'
        /// :param groupEnd: group end string 
        /// :type groupEnd: str, optional, default value '}'
        /// :return: string representation of instance that controls output
        /// return type str
        /// 
        public new string StringRep(
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
            s += groupStart;
            s = base.stringRep(delimiter, indentation, indentationSymbol, "", "");
            s += delimiter;
            s += "currentSolution=" + currentSolution.stringRep(delimiter, indentation + 1, indentationSymbol, groupStart, groupEnd) + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "k_min=" + k_min.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "k_max=" + k_max.ToString() + delimiter;
            s += delimiter;
            s += "_problem_solution_vns_support=" + _problem_solution_vns_support.stringRep(delimiter, indentation + 1, indentationSymbol, groupStart, groupEnd) + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "_maxLocalOptima=" + _maxLocalOptima.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "_local_search_type=" + _local_search_type.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += groupEnd;
            return s;
        }

        /// 
        /// String representation of the `VnsOptimizer` instance
        /// 
        /// :return: string representation of the `VnsOptimizer` instance
        /// return type str
        /// 
        public override string ToString()
        {
            var s = this.stringRep("|");
            return s;
        }

        /// 
        /// String representation of the `VnsOptimizer` instance
        /// 
        /// :return: string representation of the `VnsOptimizer` instance
        /// return type str
        /// 
        public virtual string _repr__()
        {
            var s = this.stringRep("\n");
            return s;
        }

        /// 
        /// Formatted the VnsOptimizer instance
        /// 
        /// :param spec: str -- format specification 
        /// :return: formatted `VnsOptimizer` instance
        /// return type str
        /// 
        public virtual string _format__(string spec)
        {
            return StringRep("\n", 0, "   ", "{", "}");
        }
    }
}

