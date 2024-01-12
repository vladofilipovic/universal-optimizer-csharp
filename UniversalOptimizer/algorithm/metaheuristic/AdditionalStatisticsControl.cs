namespace UniversalOptimizer.Algorithm.Metaheuristic
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
        private HashSet<string> _allSolutionCodes;
        private Dictionary<string, double> _moreLocalOptima;
        private Random _random;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdditionalStatisticsControl"/> class.
        /// </summary>
        /// <param name="keep">The keep.</param>
        /// <param name="maxLocalOptima">The maximum local optima.</param>
        public AdditionalStatisticsControl(string keep = "", int maxLocalOptima = 10)
        {
            _allSolutionCodes = new HashSet<string>();
            _moreLocalOptima = new Dictionary<string, double>();
            _canBeKept = new List<string> {
                    "allSolutionCode",
                    "moreLocalOptima"
                };
            _maxLocalOptima = maxLocalOptima;
            _random = new Random();
            DetermineKeepHelper(keep);
        }

        /// <summary>
        /// Helper function that determines which criteria should be checked during.
        /// </summary>
        /// <param name="keep">The keep - comma-separated list of values that should be kept
        /// (currently keep contains strings `allSolutionCode`, `moreLocalOptima`).</param>
        /// <exception cref="ValueError">Invalid value for keep '{}'. Should be one of:{}.".format(k, "allSolutionCode, moreLocalOptima")</exception> 
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
                if (k == "allSolutionCode")
                {
                    _keepAllSolutionCodes = true;
                }
                else if (k == "moreLocalOptima")
                {
                    _keepMoreLocalOptima = true;
                }
                else
                {
                    throw new ArgumentException("Invalid value for keep '" + k + "'. Should be one of: allSolutionCode, moreLocalOptima");
                }
            }
            _allSolutionCodes = new HashSet<string>();
            _moreLocalOptima = new Dictionary<string, double>();
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

        /// <summary>
        /// Property getter for keep property - comma-separated list of values to be kept.
        /// </summary>
        /// <value>
        /// The keep.
        /// </value>
        public string Keep
        {
            get
            {
                var ret = "";
                if (_keepAllSolutionCodes)
                {
                    ret += "allSolutionCode, ";
                }
                if (_keepMoreLocalOptima)
                {
                    ret += "moreLocalOptima, ";
                }
                ret = ret[..^2];
                return ret;
            }
            set
            {
                DetermineKeepHelper(value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether [keep all solution codes].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [keep all solution codes]; otherwise, <c>false</c>.
        /// </value>
        public bool KeepAllSolutionCodes
        {
            get
            {
                return _keepAllSolutionCodes;
            }
        }

        /// <summary>
        /// Gets a value indicating whether [keep more local optima].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [keep more local optima]; otherwise, <c>false</c>.
        /// </value>
        public bool KeepMoreLocalOptima
        {
            get
            {
                return _keepMoreLocalOptima;
            }
        }

        public HashSet<string> AllSolutionCodes
        {
            get
            {
                return _allSolutionCodes;
            }
        }

        public Dictionary<string, double> MoreLocalOptima
        {
            get
            {
                return _moreLocalOptima;
            }
        }

        /// <summary>
        /// Adds to all solution codes if required.
        /// </summary>
        /// <param name="representation">The solution representation to be inserted into all 
        /// solution code.</param>
        public virtual void AddToAllSolutionCodesIfRequired(string representation)
        {
            if (KeepAllSolutionCodes)
            {
                _ = _allSolutionCodes!.Add(representation);
            }
        }

        /// <summary>
        /// Add solution to the local optima structure.
        /// </summary>
        /// <param name="solutionToAddRep">String representation of the solution to be added to 
        /// local optima structure.</param>
        /// <param name="solutionToAddFitness">The fitness value of the solution to be added to 
        /// local optima structure.</param>
        /// <param name="bestSolutionRep">The string representation of the best solution so far.
        /// </param>
        /// <returns>If adding is successful e.g. currentSolution is new element in the structure 
        /// </returns>
        public virtual bool AddToMoreLocalOptimaIfRequired(string solutionToAddRep, double? solutionToAddFitness, string bestSolutionRep)
        {
            if (!solutionToAddFitness.HasValue)
            {
                return false;
            }
            if (!KeepMoreLocalOptima)
            {
                return false;
            }
            if (_moreLocalOptima.ContainsKey(solutionToAddRep))
            {
                return false;
            }
            if (_moreLocalOptima.Count >= _maxLocalOptima)
            {
                /// removing random, just taking care not to remove the best ones
                while (true)
                {
                    var code = _moreLocalOptima.Keys.
                                ToArray()[_random.Next(_moreLocalOptima.Keys.Count)];
                    if (code != bestSolutionRep)
                    {
                        _ = _moreLocalOptima.Remove(code);
                        break;
                    }
                }
            }
            _moreLocalOptima[solutionToAddRep] = (double)solutionToAddFitness;
            return true;
        }

        /// <summary>
        /// String representation of the additional statistic control instance.
        /// </summary>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="indentation">The indentation.</param>
        /// <param name="indentationSymbol">The indentation symbol.</param>
        /// <param name="groupStart">The group start.</param>
        /// <param name="groupEnd">The group end.</param>
        /// <returns>The string representation.</returns>
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
            s += "keep=" + Keep.ToString() + delimiter;
            for(int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            if (KeepAllSolutionCodes)
            {
                for(int i=0; i<indentation; i++)
                {
                    s += indentationSymbol;
                }
                s += "all solution codes=" + _allSolutionCodes.Count.ToString() + delimiter;
            }
            for(int i=0; i<indentation; i++)
            {
                s += indentationSymbol;
            }
            s += groupEnd;
            return s;
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString() => StringRep("|");

    }
}