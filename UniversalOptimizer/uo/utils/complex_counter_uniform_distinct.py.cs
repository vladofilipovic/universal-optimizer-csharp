namespace utils {
    
    using Path = pathlib.Path;
    
    using sys;
    
    using deepcopy = copy.deepcopy;
    
    using datetime = datetime.datetime;
    
    using logger = uo.utils.logger.logger;
    
    using System.Collections.Generic;
    
    using System;
    
    using System.Linq;
    
    public static class complex_counter_uniform_distinct {
        
        public static object directory = Path(@__file__).resolve();
        
        static complex_counter_uniform_distinct() {
            sys.path.append(directory.parent);
            main();
        }
        
        // 
        //     This class describes complex counter with uniform values, that counts only ascending data 
        //     
        public class ComplexCounterUniformAscending {
            
            public object @__counter_size;
            
            public List<int> @__counters;
            
            public object @__number_of_counters;
            
            public ComplexCounterUniformAscending(int number_of_counters, int counter_size) {
                this.@__number_of_counters = number_of_counters;
                this.@__counter_size = counter_size;
                this.@__counters = new List<int> {
                    0
                } * number_of_counters;
            }
            
            // 
            //         Internal copy of the current complex counter
            // 
            //         :return:  new `ComplexCounterUniformAscending` instance with the same properties
            //         :rtype: :class:`uo.utils.ComplexCounterUniformAscending`
            //         
            public virtual void @__copy__() {
                var ccud = deepcopy(this);
                return ccud;
            }
            
            // 
            //         Copy the current complex counter
            // 
            //         :return:  new `ComplexCounterUniformAscending` instance with the same properties
            //         :rtype: :class:`uo.utils.ComplexCounterUniformAscending`
            //         
            public virtual void copy() {
                return this.@__copy__();
            }
            
            // 
            //         Returns current state of the complex counter
            // 
            //         :return: current state of the complex counter
            //         :rtype: list[int]
            //         
            public virtual object current_state() {
                return this.@__counters;
            }
            
            // 
            //         Resets the complex counter to its initial position.
            // 
            //         :return: if progress is possible after resetting
            //         :rtype: bool
            //         
            public virtual bool reset() {
                foreach (var i in Enumerable.Range(0, this.@__number_of_counters)) {
                    this.@__counters[i] = i;
                }
                return this.@__number_of_counters * this.@__counter_size > 0;
            }
            
            // 
            //         Make the progress to the complex counter. At the same time, determine if complex counter can progress.
            // 
            //         :return: if progress is successful
            //         :rtype: bool
            //         
            public virtual bool progress() {
                var finish = true;
                foreach (var i in Enumerable.Range(0, this.@__number_of_counters - 0)) {
                    if (this.@__counters[i] < this.@__counter_size - 1) {
                        finish = false;
                    }
                }
                if (finish) {
                    return false;
                }
                var ind_not_max = this.@__number_of_counters - 1;
                foreach (var i in Enumerable.Range(0, Convert.ToInt32(Math.Ceiling(Convert.ToDouble(-1 - ind_not_max) / -1))).Select(_x_1 => ind_not_max + _x_1 * -1)) {
                    if (this.@__counters[i] < this.@__counter_size - 1) {
                        ind_not_max = i;
                        break;
                    }
                }
                this.@__counters[ind_not_max] += 1;
                foreach (var i in Enumerable.Range(ind_not_max + 1, this.@__number_of_counters - (ind_not_max + 1))) {
                    this.@__counters[i] = 0;
                }
                return true;
            }
        }
        
        // testing the developed class
        public static void main() {
            var cc = new ComplexCounterUniformAscending(4, 6);
            var can_progress = cc.reset();
            foreach (var i in Enumerable.Range(1, 1400 - 1)) {
                Console.WriteLine(cc.current_state());
                can_progress = cc.progress();
            }
        }
        
        static complex_counter_uniform_distinct() {
            if (@__name__ == "__main__") {
            }
        }
    }
}
