using UniversalOptimizer.Algorithm;

///  
/// ..  _py_vns_optimizer:
/// 
/// The :mod:`~uo.Algorithm.Metaheuristic.VariableNeighborhoodSearch` contains class :class:`~.algorithm.metaheuristic.variable_neighborhood_search.VnsOptimizer`, that represents implements algorithm :ref:`VNS<Algorithm_Variable_Neighborhood_Search>`.
/// 
namespace UniversalOptimizer.Algorithm.Metaheuristic.VariableNeighborhoodSearch
{

    using Path = pathlib.Path;

    using sys;

    using deepcopy = copy.deepcopy;

    using choice = random.choice;

    using random = random.random;

    using Bits = bitstring.Bits;

    using BitArray = bitstring.BitArray;

    using BitStream = bitstring.BitStream;

    using pack = bitstring.pack;

    using TypeVar = typing.TypeVar;

    using Generic = typing.Generic;

    using Generic = typing.Generic;

    using NamedTuple = typing.NamedTuple;

    using dataclass = dataclasses.dataclass;

    using logger = uo.utils.logger.logger;

    using TargetProblem = uo.TargetProblem.TargetProblem.TargetProblem;

    using TargetSolution = uo.TargetSolution.TargetSolution.TargetSolution;

    using OutputControl = OutputControl.OutputControl;

    using FinishControl = uo.Algorithm.Metaheuristic.finishControl.FinishControl;

    using AdditionalStatisticsControl = uo.Algorithm.Metaheuristic.additionalStatisticsControl.AdditionalStatisticsControl;

    using SingleSolutionMetaheuristic = uo.Algorithm.Metaheuristic.SingleSolutionMetaheuristic.SingleSolutionMetaheuristic;

    using ProblemSolutionVnsSupport = uo.Algorithm.Metaheuristic.VariableNeighborhoodSearch.problem_solution_vns_support.ProblemSolutionVnsSupport;

    using System.Collections.Generic;

    using System;

    using System.Linq;

    public static class vns_optimizer
    {

        public static object directory = Path(_file__).resolve();

        static vns_optimizer()
        {
            sys.path.append(directory.parent);
            sys.path.append(directory.parent.parent);
            sys.path.append(directory.parent.parent.parent);
        }

        /// 
        /// Instance of the class :class:`~uo.Algorithm.Metaheuristic.VariableNeighborhoodSearch_constructor_parameters.
        /// VnsOptimizerConstructionParameters` represents constructor parameters for VNS algorithm.
        /// 
        public class VnsOptimizerConstructionParameters
        {

            public object additionalStatisticsControl;

            public object finishControl;

            public object initial_solution;

            public object k_max;

            public object k_min;

            public object local_search_type;

            public object OutputControl;

            public object problem_solution_vns_support;

            public object randomSeed;

            public object TargetProblem;

            public object finishControl = null;

            public object OutputControl = null;

            public object TargetProblem = null;

            public object initial_solution = null;

            public object problem_solution_vns_support = null;

            public object randomSeed = null;

            public object additionalStatisticsControl = null;

            public object k_min = null;

            public object k_max = null;

            public object local_search_type = null;
        }

        /// 
        ///     Instance of the class :class:`~uo.Algorithm.Metaheuristic.VariableNeighborhoodSearch.VnsOptimizer` encapsulate 
        ///     :ref:`Algorithm_Variable_Neighborhood_Search` optimization algorithm.
        ///     
        public class VnsOptimizer
            : SingleSolutionMetaheuristic
        {

            private object _implemented_local_searches;

            private int _k_current;

            private object _k_max;

            private object _k_min;

            private object _local_search_type;

            private object _ls_method;

            private object _problem_solution_vns_support;

            private object _shaking_method;

            public object currentSolution;

            public int iteration;

            public VnsOptimizer(
                object finishControl,
                int randomSeed,
                object additionalStatisticsControl,
                object OutputControl,
                object TargetProblem,
                object initial_solution,
                object problem_solution_vns_support,
                int k_min,
                int k_max,
                string local_search_type)
                : base(finishControl: finishControl, randomSeed: randomSeed, additionalStatisticsControl: additionalStatisticsControl, OutputControl: OutputControl, TargetProblem: TargetProblem, initial_solution: initial_solution)
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
            public virtual object init()
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
}
