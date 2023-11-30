namespace UniversalOptimizer.algorithm.metaheuristic
{
    /// <summary>
    /// This class determine finishing criteria and status during execution of the metaheuristics
    /// </summary>
    public class FinishControl
    {
        public bool _checkEvaluations;
        public bool _checkIterations;
        public bool _checkSeconds;
        private int _evaluationsMax;
        public List<string> _implemented_criteria;
        private int _iterationsMax;
        private double _secondsMax;

        public FinishControl(string criteria = "evaluations & seconds & iterations", int evaluationsMax = 0, int iterationsMax = 0, double secondsMax = 0)
        {
            _implemented_criteria = new List<string> {
                    "evaluationsMax",
                    "iterationsMax",
                    "secondsMax"
                };
            _evaluationsMax = evaluationsMax;
            _iterationsMax = iterationsMax;
            _secondsMax = secondsMax;
            DetermineCriteriaHelper(criteria);
        }

        /// <summary>
        /// Determines the criteria helper.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <exception cref="ValueError">Invalid value for criteria. Should be one of:
        /// "evaluations, iterations, seconds")</exception>
        private void DetermineCriteriaHelper(string criteria)
        {
            _checkEvaluations = false;
            _checkIterations = false;
            _checkSeconds = false;
            var crit = criteria.Split("&");
            foreach (var cr in crit)
            {
                var c = cr.Trim();
                if (c == "")
                {
                    continue;
                }
                if (c == "evaluations")
                {
                    if (_evaluationsMax > 0)
                    {
                        _checkEvaluations = true;
                    }
                }
                else if (c == "iterations")
                {
                    if (_iterationsMax > 0)
                    {
                        _checkIterations = true;
                    }
                }
                else if (c == "seconds")
                {
                    if (_secondsMax > 0)
                    {
                        _checkSeconds = true;
                    }
                }
                else
                {
                    throw new Exception("Invalid value for criteria. Should be one of: evaluations, iterations, seconds");
                }
            }
        }

        /// <summary>
        /// Property getter for maximum number of evaluations.
        /// </summary>
        /// <value>
        /// The maximum number of evaluations.
        /// </value>
        public int EvaluationsMax
        {
            get
            {
                return _evaluationsMax;
            }
        }

        /// <summary>
        /// Property getter for maximum number of iterations.
        /// </summary>
        /// <value>
        /// The maximum number of iterations.
        /// </value>
        public int IterationsMax
        {
            get
            {
                return _iterationsMax;
            }
        }

        /// <summary>
        /// Property getter for maximum number of seconds for metaheuristics execution.
        /// </summary>
        /// <value>
        /// The maximum number of seconds.
        /// </value>
        public double SecondsMax
        {
            get
            {
                return _secondsMax;
            }
        }

        /// <summary>
        /// Gets or sets the criteria for finish.
        /// </summary>
        /// <value>
        /// The criteria.
        /// </value>
        public string Criteria
        {
            get
            {
                var ret = "";
                if (_checkEvaluations)
                {
                    ret += "evaluations & ";
                }
                if (_checkIterations)
                {
                    ret += "iterations & ";
                }
                if (_checkSeconds)
                {
                    ret += "seconds & ";
                }
                ret = ret[..^2];
                return ret;
            }
            set
            {
                DetermineCriteriaHelper(value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether [check evaluations].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [check evaluations]; otherwise, <c>false</c>.
        /// </value>
        public bool CheckEvaluations
        {
            get
            {
                return _checkEvaluations;
            }
        }

        /// <summary>
        /// Gets a value indicating whether [check iterations].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [check iterations]; otherwise, <c>false</c>.
        /// </value>
        public bool CheckIterations
        {
            get
            {
                return _checkIterations;
            }
        }

        /// <summary>
        /// Gets a value indicating whether [check seconds].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [check seconds]; otherwise, <c>false</c>.
        /// </value>
        public bool CheckSeconds
        {
            get
            {
                return _checkSeconds;
            }
        }

        /// <summary>
        /// Determines whether the specified evaluation is finished.
        /// </summary>
        /// <param name="evaluation">The evaluation.</param>
        /// <param name="iteration">The iteration.</param>
        /// <param name="elapsedSeconds">The elapsed seconds.</param>
        /// <returns>
        ///   <c>true</c> if the specified evaluation is finished; otherwise, <c>false</c>.
        /// </returns>
        public bool IsFinished(int evaluation, int iteration, double elapsedSeconds)
        {
            return CheckEvaluations && evaluation >= EvaluationsMax || CheckIterations && iteration >= IterationsMax ||
              CheckSeconds && elapsedSeconds >= SecondsMax;
        }

        /// <summary>
        /// Strings the rep.
        /// </summary>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="indentation">The indentation.</param>
        /// <param name="indentationSymbol">The indentation symbol.</param>
        /// <param name="groupStart">The group start.</param>
        /// <param name="groupEnd">The group end.</param>
        /// <returns></returns>
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
            s += groupStart + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "criteria=" + Criteria.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "evaluationsMax=" + EvaluationsMax.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "iterationsMax=" + IterationsMax.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "secondsMax=" + SecondsMax.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
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
        public override string ToString()
        {
            return StringRep("|");
        }

    }

}
