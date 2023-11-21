namespace utils
{

    using System;
    using System.Collections;
    using System.Linq;

    /// <summary>
    /// This class describes complex counter with uniform values, that counts full.
    /// </summary>
    public class ComplexCounterBitArrayFull : ICloneable
    {

        private BitArray _counters;
        private int _number_of_counters;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexCounterBitArrayFull"/> class.
        /// </summary>
        /// <param name="number_of_counters">The number of counters.</param>
        public ComplexCounterBitArrayFull(int number_of_counters)
        {
            _number_of_counters = number_of_counters;
            _counters = new BitArray(number_of_counters);
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public object Clone()
        {
            ComplexCounterBitArrayFull cl = new ComplexCounterBitArrayFull(0);
            cl._number_of_counters = _number_of_counters;
            cl._counters = (BitArray) _counters.Clone();
            return cl;
        }

        /// <summary>
        /// Returns current state of the complex counter.
        /// </summary>
        /// <returns>The current state of the complex counter.</returns>
        public BitArray CurrentState()
        {
            return _counters;
        }

        /// 
        /// Resets the complex counter to its initial position.
        /// 
        /// :return: if progress is possible after resetting
        /// return type bool
        /// 
        public bool reset()
        {
            _counters.Set(0, false);
            return _number_of_counters > 0;
        }

        /// 
        /// Make the progress to the complex counter. At the same time, determine if complex counter can progress.
        /// 
        /// :return: if progress is successful
        /// return type bool
        /// 
        public virtual bool progress()
        {
            if (_counters.All(true))
            {
                return false;
            }
            var ind_not_max = _counters.find("0b0")[0];
            _counters[ind_not_max] = true;
            _counters.set(false, Enumerable.Range(0, ind_not_max - 0));
            return true;
        }

        public virtual bool can_progress()
        {
            return !_counters.all(true);
        }

        /// testing the developed class
        //public static void main() {
        //    var cc = new ComplexCounterBitArrayFull(6);
        //    var can_progress = cc.reset();
        //    foreach (var i in Enumerable.Range(1, 100 - 1)) {
        //        Console.WriteLine(cc.current_state().bin);
        //        can_progress = cc.progress();
        //    }
        //}

        static complex_counter_bit_array_full()
        {
            if (_name__ == "__main__")
            {
            }
        }
    }
}
