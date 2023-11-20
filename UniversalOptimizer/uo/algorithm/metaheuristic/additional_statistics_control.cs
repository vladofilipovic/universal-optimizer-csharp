namespace uo.Algorithm.Metaheuristic
{

    using System.Collections.Generic;
    using System;
    using System.Linq;
    using static System.Runtime.InteropServices.JavaScript.JSType;


    /// <summary>
    /// This class determine additional statistics that should be kept during execution of the
    /// metaheuristics instance 
    /// </summary>
    /// 
    public class AdditionalStatisticsControl
    {
        public List<string> _canBeKept;
        public bool _keepAllSolutionCodes;
        public bool _keepMoreLocalOptima;
        private int _maxLocalOptima;
        public static HashSet<string>? AllSolutionCodes;
        public static Dictionary<string, float>? MoreLocalOptima;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdditionalStatisticsControl"/> class.
        /// </summary>
        /// <param name="keep">The keep.</param>
        /// <param name="maxLocalOptima">The maximum local optima.</param>
        public AdditionalStatisticsControl(string keep = "", int maxLocalOptima = 10)
        {
            _canBeKept = new List<string> {
                    "all_solution_code",
                    "moreLocalOptima"
                };
            _maxLocalOptima = maxLocalOptima;
            DetermineKeepHelper(keep);
        }

        /// <summary>
        /// Helper function that determines which criteria should be checked during.
        /// </summary>
        /// <param name="keep">The keep - comma-separated list of values that should be kept
        /// (currently keep contains strings `all_solution_code`, `moreLocalOptima`).</param>
        /// <exception cref="ValueError">Invalid value for keep '{}'. Should be one of:{}.".format(k, "all_solution_code, moreLocalOptima")</exception> 
        private void DetermineKeepHelper(string keep)
        {
            _keepAllSolutionCodes = false;
            _keepMoreLocalOptima = false;
            var kep = keep.Split("&");
            foreach (var ke in kep)
            {
                var k = ke.Trim();
                if (k == "" || k == "None")
                {
                    continue;
                }
                if (k == "all_solution_code")
                {
                    _keepAllSolutionCodes = true;
                }
                else if (k == "moreLocalOptima")
                {
                    _keepMoreLocalOptima = true;
                }
                else
                {
                    throw new Exception("Invalid value for keep '" + k + "'. Should be one of: all_solution_code, moreLocalOptima");
                }
            }
            if (_keepAllSolutionCodes)
            {
                AllSolutionCodes = new HashSet<string>();
            }
            if (_keepMoreLocalOptima)
            {
                MoreLocalOptima = new Dictionary<string, float>();
            }
        }

        /// <summary>
        /// Property getter for maximum number of local optima that will be kept.
        /// </summary>
        /// <value>
        /// The maximum number of local optima that will be kept.
        /// </value>
        public int MaxLocalOptima
        {
            get
            {
                return _maxLocalOptima;
            }
        }

        /// 
        /// Property getter for keep property 
        /// 
        /// :return: comma-separated list of values vo be kept
        /// return type str
        /// 
        /// 
        /// Property setter for the keep property 
        /// 
        public object keep
        {
            get
            {
                var ret = "";
                if (_keepAllSolutionCodes)
                {
                    ret += "all_solution_code, ";
                }
                if (_keepMoreLocalOptima)
                {
                    ret += "moreLocalOptima, ";
                }
                ret = ret[0: -2:];
                return ret;
            }
            set
            {
                DetermineKeepHelper(value);
            }
        }

        /// 
        /// Property getter for property if all solution codes to be kept
        /// 
        /// :return: if all solution codes to be kept
        /// return type bool
        /// 
        public object keepAllSolutionCodes
        {
            get
            {
                return _keepAllSolutionCodes;
            }
        }

        /// 
        /// Property getter for decision if more local optima should be kept
        /// 
        /// :return: if more local optima should be kept
        /// return type bool
        /// 
        public object keepMoreLocalOptima
        {
            get
            {
                return _keepMoreLocalOptima;
            }
        }

        /// 
        /// Filling all solution code, if necessary 
        /// 
        /// :param representation: solution representation to be inserted into all solution code
        /// :type representation: str
        /// return type None
        /// 
        public virtual object add_to_allSolutionCodes_if_required(string representation)
        {
            if (this.keepAllSolutionCodes)
            {
                AdditionalStatisticsControl.allSolutionCodes.add(representation);
            }
        }

        /// 
        /// Add solution to the local optima structure 
        /// 
        /// :param str solution_to_add_rep: string representation of the solution to be added to local optima structure
        /// :param float solution_to_add_fitness: fitness value of the solution to be added to local optima structure
        /// :param str bestSolution_rep: string representation of the best solution so far
        /// :return:  if adding is successful e.g. currentSolution is new element in the structure
        /// return type bool
        /// 
        public virtual bool add_to_moreLocalOptima_if_required(string solution_to_add_rep, object solution_to_add_fitness, string bestSolution_rep)
        {
            if (!this.keepMoreLocalOptima)
            {
                return false;
            }
            if (AdditionalStatisticsControl.moreLocalOptima.Contains(solution_to_add_rep))
            {
                return false;
            }
            if (AdditionalStatisticsControl.moreLocalOptima.Count >= _maxLocalOptima)
            {
                /// removing random, just taking care not to remove the best ones
                while (true)
                {
                    var code = random.choice(AdditionalStatisticsControl.moreLocalOptima.keys());
                    if (code != bestSolution_rep)
                    {
                        AdditionalStatisticsControl.moreLocalOptima.Remove(code);
                        break;
                    }
                }
            }
            AdditionalStatisticsControl.moreLocalOptima[solution_to_add_rep] = solution_to_add_fitness;
            return true;
        }

        /// 
        /// String representation of the target solution instance
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
            s += "keep=" + this.keep.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "use_cache_forDistance_calculation=" + this.use_cache_forDistance_calculation.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            if (this.keepAllSolutionCodes)
            {
                foreach (var i in Enumerable.Range(0, indentation - 0))
                {
                    s += indentationSymbol;
                }
                s += "all solution codes=" + AdditionalStatisticsControl.allSolutionCodes.Count.ToString() + delimiter;
            }
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += groupEnd;
            return s;
        }

        /// 
        /// String representation of the cache control and statistics structure
        /// 
        /// :return: string representation of the cache control and statistics structure
        /// return type str
        /// 
        public override string ToString()
        {
            return this.StringRep("|");
        }

        /// 
        /// Representation of the cache control and statistics structure
        /// 
        /// :return: string representation of cache control and statistics structure
        /// return type str
        /// 
        public virtual string _repr__()
        {
            return this.StringRep("\n");
        }

        /// 
        /// Formatted the cache control and statistics structure
        /// 
        /// :param str spec: format specification
        /// :return: formatted cache control and statistics structure
        /// return type str
        /// 
        public virtual string _format__(string spec)
        {
            return this.StringRep("|");
        }
    }
}
}
