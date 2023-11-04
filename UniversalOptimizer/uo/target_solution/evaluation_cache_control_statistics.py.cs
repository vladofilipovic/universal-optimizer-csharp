//  
// The :mod:`~uo.target_solution.evaluation_cache_control_statistics` module describes the class :class:`~uo.target_solution.EvaluationCacheControlStatistics`.
// 
namespace target_solution {
    
    using Optional = typing.Optional;
    
    using Path = pathlib.Path;
    
    using sys;
    
    using System.Collections.Generic;
    
    using System;
    
    using System.Linq;
    
    public static class evaluation_cache_control_statistics {
        
        public static object directory = Path(@__file__).resolve();
        
        static evaluation_cache_control_statistics() {
            sys.path.append(directory.parent);
        }
        
        // 
        //     Class that represents control statistics for evaluation caching.
        //     
        public class EvaluationCacheControlStatistics {
            
            public object @__cache;
            
            public int @__cache_hit_count;
            
            public int @__cache_request_count;
            
            public object @__is_caching;
            
            public object @__max_cache_size;
            
            public EvaluationCacheControlStatistics(object is_caching = false, object max_cache_size = 0) {
                this.@__is_caching = is_caching;
                this.@__max_cache_size = max_cache_size;
                this.@__cache = new Dictionary<object, object> {
                };
                this.@__cache_hit_count = 0;
                this.@__cache_request_count = 0;
            }
            
            // 
            //         Internal copy of the `EvaluationCacheControlStatistics` instance
            // 
            //         :return: new `EvaluationCacheControlStatistics` instance with the same properties
            //         :rtype: `EvaluationCacheControlStatistics`
            //         
            public virtual void @__copy__() {
                var pr = deepcopy(this);
                return pr;
            }
            
            // 
            //         Copy the `EvaluationCacheControlStatistics` instance
            // 
            //         :return: new `EvaluationCacheControlStatistics` instance with the same properties
            //         :rtype: `EvaluationCacheControlStatistics`
            //         
            public virtual void copy() {
                return this.@__copy__();
            }
            
            // 
            //         Property getter for is_caching 
            // 
            //         :return: if caching is used during evaluation, or not 
            //         :rtype: bool
            //         
            // 
            //         Property setter for is_caching
            // 
            //         :param bool value: value for determining if caching is activated
            //         
            public object is_caching {
                get {
                    return this.@__is_caching;
                }
                set {
                    this.@__is_caching = value;
                }
            }
            
            // 
            //         Property getter for `max_cache_size` 
            // 
            //         :return: maximum size of the cache - if 0 cache is with unlimited size 
            //         :rtype: int
            //         
            public object max_cache_size {
                get {
                    return this.@__max_cache_size;
                }
            }
            
            // 
            //         Property getter for cache 
            //         
            //         :return: cache that is used during evaluation 
            //         :rtype: dict[str]
            //         
            // 
            //         Property setter for cache
            // 
            //         :param dict[str] value: value for cache
            //         
            public object cache {
                get {
                    return this.@__cache;
                }
                set {
                    this.@__cache = value;
                }
            }
            
            // 
            //         Property getter for cache_hit_count 
            // 
            //         :return: number of cache hits during evaluation
            //         :rtype: int
            //         
            public object cache_hit_count {
                get {
                    return this.@__cache_hit_count;
                }
            }
            
            // 
            //         Increments number of cache hits during evaluation 
            //         
            public virtual object increment_cache_hit_count() {
                this.@__cache_hit_count += 1;
            }
            
            // 
            //         Property getter for cache_request_count 
            // 
            //         :return: overall number of evaluations 
            //         :rtype: int
            //         
            public object cache_request_count {
                get {
                    return this.@__cache_request_count;
                }
            }
            
            // 
            //         Increments overall number of evaluations 
            //         
            public virtual object increment_cache_request_count() {
                this.@__cache_request_count += 1;
            }
            
            // 
            //         String representation of the `EvaluationCacheControlStatistics` instance
            // 
            //         :param delimiter: delimiter between fields
            //         :type delimiter: str
            //         :param indentation: level of indentation
            //         :type indentation: int, optional, default value 0
            //         :param indentation_symbol: indentation symbol
            //         :type indentation_symbol: str, optional, default value ''
            //         :param group_start: group start string 
            //         :type group_start: str, optional, default value '{'
            //         :param group_end: group end string 
            //         :type group_end: str, optional, default value '}'
            //         :return: string representation of instance that controls output
            //         :rtype: str
            //         
            public virtual string string_rep(
                string delimiter,
                int indentation = 0,
                string indentation_symbol = "",
                string group_start = "{",
                string group_end = "}") {
                var s = delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += group_start + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "__is_caching=" + this.@__is_caching.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "__cache_hit_count=" + this.@__cache_hit_count.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += "__cache_request_count=" + this.@__cache_request_count.ToString() + delimiter;
                foreach (var i in Enumerable.Range(0, indentation - 0)) {
                    s += indentation_symbol;
                }
                s += group_end;
                return s;
            }
            
            // 
            //         String representation of the `EvaluationCacheControlStatistics` instance
            // 
            //         :return: string representation of the `EvaluationCacheControlStatistics` instance
            //         :rtype: str
            //         
            public override string ToString() {
                return this.string_rep("|", 0, "", "{", "}");
            }
            
            // 
            //         Representation of the `EvaluationCacheControlStatistics` instance
            //         :return: str -- string representation of the `EvaluationCacheControlStatistics` instance
            //         
            public virtual string @__repr__() {
                return this.string_rep("\n", 0, "   ", "{", "}");
            }
            
            // 
            //         Formatted the `EvaluationCacheControlStatistics` instance
            //         :param spec: str -- format specification
            //         :return: str -- formatted `EvaluationCacheControlStatistics` instance
            //         
            public virtual string @__format__(string spec) {
                return this.string_rep("|");
            }
        }
    }
}
