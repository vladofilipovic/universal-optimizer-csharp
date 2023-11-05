namespace utils {
    
    using Path = pathlib.Path;
    
    using sys;
    
    using deepcopy = copy.deepcopy;
    
    using datetime = datetime.datetime;
    
    using logger = uo.utils.logger.logger;
    
    using System.Collections.Generic;
    
    using System;
    
    using System.Linq;
    
    public static class complex_counter_uniform_full {
        
        public static object directory = Path(_file__).resolve();
        
        static complex_counter_uniform_full() {
            sys.path.append(directory.parent);
            main();
        }
        
        /// 
        ///     This class describes complex counter with uniform values, that counts full 
        ///     
        public class ComplexCounterUniformFull {
            
            private object _counter_size;
            
            public List<int> _counters;
            
            private object _number_of_counters;
            
            public ComplexCounterUniformFull(int number_of_counters, int counter_size) {
                _number_of_counters = number_of_counters;
                _counter_size = counter_size;
                _counters = new List<int> {
                    0
                } * number_of_counters;
            }
            
            /// 
            ///         Internal copy of the current complex counter
            /// 
            ///         :return:  new `ComplexCounterUniformFull` instance with the same properties
            ///         :rtype: :class:`uo.utils.ComplexCounterUniformFull`
            ///         
            public virtual void _copy__() {
                var cc = deepcopy(this);
                return cc;
            }
            
            /// 
            ///         Copy the current complex counter
            /// 
            ///         :return:  new `ComplexCounterUniformFull` instance with the same properties
            ///         :rtype: :class:`uo.utils.ComplexCounterUniformFull`
            ///         
            public virtual void copy() {
                return _copy__();
            }
            
            /// 
            ///         Returns current state of the complex counter
            /// 
            ///         :return: current state of the complex counter
            ///         :rtype: list[int]
            ///         
            public virtual object current_state() {
                return _counters;
            }
            
            /// 
            ///         Resets the complex counter to its initial position.
            /// 
            ///         :return: if progress is possible after resetting
            ///         :rtype: bool
            ///         
            public virtual bool reset() {
                foreach (var i in Enumerable.Range(0, _number_of_counters)) {
                    _counters[i] = 0;
                }
                return _number_of_counters * _counter_size > 0;
            }
            
            /// 
            ///         Make the progress to the complex counter. At the same time, determine if complex counter can progress.
            /// 
            ///         :return: if progress is successful
            ///         :rtype: bool
            ///         
            public virtual bool progress() {
                var finish = true;
                foreach (var i in Enumerable.Range(0, _number_of_counters - 0)) {
                    if (_counters[i] < _counter_size - 1) {
                        finish = false;
                    }
                }
                if (finish) {
                    return false;
                }
                var ind_not_max = _number_of_counters - 1;
                foreach (var i in Enumerable.Range(0, Convert.ToInt32(Math.Ceiling(Convert.ToDouble(-1 - ind_not_max) / -1))).Select(_x_1 => ind_not_max + _x_1 * -1)) {
                    if (_counters[i] < _counter_size - 1) {
                        ind_not_max = i;
                        break;
                    }
                }
                _counters[ind_not_max] += 1;
                foreach (var i in Enumerable.Range(ind_not_max + 1, _number_of_counters - (ind_not_max + 1))) {
                    _counters[i] = 0;
                }
                return true;
            }
        }
        
        /// testing the developed class
        public static void main() {
            var cc = new ComplexCounterUniformFull(4, 6);
            var can_progress = cc.reset();
            foreach (var i in Enumerable.Range(1, 1400 - 1)) {
                Console.WriteLine(cc.current_state());
                can_progress = cc.progress();
            }
        }
        
        static complex_counter_uniform_full() {
            if (_name__ == "__main__") {
            }
        }
    }
}
