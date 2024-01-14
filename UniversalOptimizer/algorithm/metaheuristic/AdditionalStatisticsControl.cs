namespace UniversalOptimizer.Algorithm.Metaheuristic
{

    using System.Collections.Generic;
    using System;
    using System.Linq;
    using static System.Runtime.InteropServices.JavaScript.JSType;
    using System.Text;


    /// <summary>
    /// This class determine additional statistics that should be kept during execution of the
    /// metaheuristics instance 
    /// </summary>
    /// 
    public class AdditionalStatisticsControl
    {
        private readonly bool _isActive;
        private bool _keepAllSolutionCodes;
        private bool _keepMoreLocalOptima;

        private HashSet<string> _allSolutionCodes;
        private readonly int _maxLocalOptimaCount;
        private Dictionary<string, double> _moreLocalOptima;

        private readonly Random _random;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdditionalStatisticsControl"/> class.
        /// </summary>
        /// <param name="keep">The keep.</param>
        /// <param name="maxLocalOptima">The maximum local optima.</param>
        public AdditionalStatisticsControl(bool isActive, string keep = "", int maxLocalOptimaCount = 10)
        {
            _isActive = isActive;
            _maxLocalOptimaCount = maxLocalOptimaCount;
            _random = new Random();
            _allSolutionCodes = [];
            _moreLocalOptima = new Dictionary<string, double>();
            DetermineKeepHelper(keep);
        }

        /// <summary>
        /// Helper function that determines which criteria should be checked during.
        /// </summary>
        /// <param name="keep">The keep - comma-separated list of values that should be kept
        /// (currently keep contains strings `allSolutionCode`, `moreLocalOptima`.</param>
        /// <exception cref="ValueError">"Invalid value for keep '" + k + "'. Should be either empty string or 'none', either comma-separated sequence of: 'allSolutionCode', 'moreLocalOptima'."</exception> 
        private void DetermineKeepHelper(string keep)
        {
            _keepAllSolutionCodes = false;
            _keepMoreLocalOptima = false;
            var kep = keep.Split("&");
            foreach (var ke in kep)
            {
                var k = ke.Trim();
                if (k == "" || k=="none")
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
                    throw new ArgumentException("Invalid value for keep '" + k + "'. Should be either empty string or 'none', either comma-separated sequence of: 'allSolutionCode', 'moreLocalOptima'.");
                }
            }
            _allSolutionCodes = [];
            _moreLocalOptima = [];
        }

        /// <summary>
        /// Property getter for indicator if additional statistics is active.
        /// </summary>
        /// <value>
        /// Indicator if additional statistics is active.
        /// </value>
        public bool IsActive
        {
            get
            {
                return _isActive;
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
        /// Gets all solution codes.
        /// </summary>
        /// <value>
        /// All solution codes.
        /// </value>
        public HashSet<string> AllSolutionCodes
        {
            get
            {
                return _allSolutionCodes;
            }
        }


        /// <summary>
        /// Property getter for maximum number of local optima that will be kept.
        /// </summary>
        /// <value>
        /// The maximum number of local optima that will be kept.
        /// </value>
        public int MaxLocalOptimaCount
        {
            get
            {
                return _maxLocalOptimaCount;
            }
        }


        /// <summary>
        /// Gets the more local optima.
        /// </summary>
        /// <value>
        /// The more local optima.
        /// </value>
        public Dictionary<string, double> MoreLocalOptima
        {
            get
            {
                return _moreLocalOptima;
            }
        }


        /// <summary>
        /// Adds to all solution codes.
        /// </summary>
        /// <param name="representation">The solution representation to be inserted into all 
        /// solution code.</param>
        public void AddToAllSolutionCodes(string representation)
        {
            _ = _allSolutionCodes!.Add(representation);
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
        public bool AddToMoreLocalOptima(string solutionToAddRep, double? solutionToAddFitness, string bestSolutionRep)
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
            if (_moreLocalOptima.Count >= _maxLocalOptimaCount)
            {
                /// removing random, just taking care not to remove the best ones
                while (true)
                {
                    var code = _moreLocalOptima.Keys.ToArray()[_random.Next(_moreLocalOptima.Keys.Count)];
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
            StringBuilder s = new StringBuilder(delimiter);
            for(int i=0; i<indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append(groupStart + delimiter);
            for(int i=0; i<indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append("keep=" + Keep.ToString() + delimiter);
            for(int i=0; i<indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            if (KeepAllSolutionCodes)
            {
                for(int i=0; i<indentation; i++)
                {
                    s.Append(indentationSymbol);
                }
                s.Append("all solution codes=" + _allSolutionCodes.Count.ToString() + delimiter);
            }
            for(int i=0; i<indentation; i++)
            {
                s.Append(indentationSymbol);
            }
            s.Append(groupEnd);
            return s.ToString();
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
