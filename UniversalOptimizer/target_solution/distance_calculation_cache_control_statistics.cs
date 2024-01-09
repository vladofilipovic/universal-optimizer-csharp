
namespace UniversalOptimizer.TargetSolution
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Class that represents control statistics for solution code distance calculation cache.
    /// </summary>
    /// <typeparam name="E_co">The type of the solution code element.</typeparam>
    public class DistanceCalculationCacheControlStatistics<E_co>
    {

        private Dictionary<(E_co, E_co), double> _cache;
        private int _cacheHitCount;
        private int _cacheRequestCount;
        private bool _isCaching;
        private readonly int _maxCacheSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="DistanceCalculationCacheControlStatistics{E_co}"/> class.
        /// </summary>
        /// <param name="isCaching">if set to <c>true</c> [is caching].</param>
        /// <param name="maxCacheSize">Maximum size of the cache.</param>
        public DistanceCalculationCacheControlStatistics(bool isCaching, int maxCacheSize = 0)
        {
            _isCaching = isCaching;
            _maxCacheSize = maxCacheSize;
            _cache = new Dictionary<(E_co, E_co), double>();
            _cacheHitCount = 0;
            _cacheRequestCount = 0;
        }

        /// <summary>
        /// Property getter and setter for decision if caching is used during calculation of the solution code distances, or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is caching; otherwise, <c>false</c>.
        /// </value>
        public bool IsCaching
        {
            get
            {
                return _isCaching;
            }
            set
            {
                _isCaching = value;
            }
        }

        /// <summary>
        /// Property getter for maximum size of the cache - if 0 cache is with unlimited size.
        /// </summary>
        /// <value>
        /// The maximum size of the cache.
        /// </value>
        public int MaxCacheSize
        {
            get
            {
                return _maxCacheSize;
            }
        }

        ///         
        /// <summary>
        /// Gets or sets the cache.
        /// </summary>
        /// <value>
        /// The cache.
        /// </value>
        public Dictionary<(E_co, E_co), double> Cache
        {
            get
            {
                return _cache;
            }
            set
            {
                _cache = value;
            }
        }

        /// <summary>
        /// Property getter for number of cache hits during calculation of the solution code distances.
        /// </summary>
        /// <value>
        /// The cache hit count.
        /// </value>
        public int CacheHitCount
        {
            get
            {
                return _cacheHitCount;
            }
        }

        /// <summary>
        /// Increments number of cache hits during calculation of the solution code distances.
        /// </summary>
        public virtual void IncrementCacheHitCount() => _cacheHitCount += 1;

        /// <summary>
        /// Property getter for overall number of calculation of the solution code distances.
        /// </summary>
        /// <value>
        /// The cache request count.
        /// </value>
        public object CacheRequestCount
        {
            get
            {
                return _cacheRequestCount;
            }
        }

        /// <summary>
        /// Increments the cache request count. Increments overall number of calculation of the solution code distances.
        /// </summary>
        public virtual void IncrementCacheRequestCount() => _cacheRequestCount += 1;

        /// <summary>
        /// String representation of solution distance calculation cache control statistic.
        /// </summary>
        /// <param name="delimiter"> <see cref="string" /> delimiter between fields.</param>
        /// <param name="indentation"> <see cref="int" /> that represents the level of indentation.</param>
        /// <param name="indentationSymbol">indentation symbol <see cref="string" />.</param>
        /// <param name="groupStart">group start <see cref="string" /> .</param>
        /// <param name="groupEnd">group end <see cref="string" /> .</param>
        /// <returns></returns>
        /// 
        public string StringRep(
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
            s += "_isCaching=" + _isCaching.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "_cacheHitCount=" + _cacheHitCount.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += "_cache_requests_count=" + _cacheRequestCount.ToString() + delimiter;
            foreach (var i in Enumerable.Range(0, indentation - 0))
            {
                s += indentationSymbol;
            }
            s += groupEnd;
            return s;
        }

        /// <summary>
        /// String representation of the cache control and statistics structure.
        /// </summary>
        /// <returns>
        /// <see cref="string" /> that represents this instance.
        /// </returns>
        /// 
        public override string ToString() => StringRep("|");

    }
}

