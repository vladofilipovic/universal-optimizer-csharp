namespace utils {
    
    using Path = pathlib.Path;
    
    using sys;
    
    using deepcopy = copy.deepcopy;
    
    using datetime = datetime.datetime;
    
    using BitArray = bitstring.BitArray;
    
    using logger = uo.utils.logger.logger;
    
    using System;
    
    using System.Linq;
    
    public static class complex_counter_bit_array_full {
        
        public static object directory = Path(@__file__).resolve();
        
        static complex_counter_bit_array_full() {
            sys.path.append(directory.parent);
            main();
        }
        
        // 
        //     This class describes complex counter with uniform values, that counts full 
        //     
        public class ComplexCounterBitArrayFull {
            
            public object @__counters;
            
            public object @__number_of_counters;
            
            public ComplexCounterBitArrayFull(int number_of_counters) {
                this.@__number_of_counters = number_of_counters;
                this.@__counters = BitArray(number_of_counters);
            }
            
            // 
            //         Internal copy of the current complex counter
            // 
            //         :return:  new `ComplexCounterBitArrayFull` instance with the same properties
            //         :rtype: :class:`uo.utils.ComplexCounterBitArrayFull`
            //         
            public virtual ComplexCounterBitArrayFull @__copy__() {
                var cc = new ComplexCounterBitArrayFull(this.@__number_of_counters);
                cc.@__counters = BitArray(bin: this.@__counters.bin);
                return cc;
            }
            
            // 
            //         Copy the current complex counter
            // 
            //         :return:  new `ComplexCounterBitArrayFull` instance with the same properties
            //         :rtype: :class:`uo.utils.ComplexCounterBitArrayFull`
            //         
            public virtual void copy() {
                return this.@__copy__();
            }
            
            // 
            //         Returns current state of the complex counter
            // 
            //         :return: current state of the complex counter
            //         :rtype: BitArray
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
                this.@__counters.set(false);
                return this.@__number_of_counters > 0;
            }
            
            // 
            //         Make the progress to the complex counter. At the same time, determine if complex counter can progress.
            // 
            //         :return: if progress is successful
            //         :rtype: bool
            //         
            public virtual bool progress() {
                if (this.@__counters.all(true)) {
                    return false;
                }
                var ind_not_max = this.@__counters.find("0b0")[0];
                this.@__counters[ind_not_max] = true;
                this.@__counters.set(false, Enumerable.Range(0, ind_not_max - 0));
                return true;
            }
            
            public virtual bool can_progress() {
                return !this.@__counters.all(true);
            }
        }
        
        // testing the developed class
        public static void main() {
            var cc = new ComplexCounterBitArrayFull(6);
            var can_progress = cc.reset();
            foreach (var i in Enumerable.Range(1, 100 - 1)) {
                Console.WriteLine(cc.current_state().bin);
                can_progress = cc.progress();
            }
        }
        
        static complex_counter_bit_array_full() {
            if (@__name__ == "__main__") {
            }
        }
    }
}
