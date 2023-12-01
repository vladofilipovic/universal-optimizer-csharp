namespace UniversalOptimizer.utils
{

    public class ComplexCounterUniformAscending: ICloneable
    {

        private int _counter_size;

        private List<int> _counters;

        private int _number_of_counters;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexCounterUniformAscending"/> class.
        /// </summary>
        /// <param name="number_of_counters">The number of counters.</param>
        /// <param name="counter_size">Size of the counter.</param>
        public ComplexCounterUniformAscending(int number_of_counters, int counter_size)
        {
            _number_of_counters = number_of_counters;
            _counter_size = counter_size;
            _counters = new List<int>(number_of_counters);
            foreach (var i in Enumerable.Range(0, _number_of_counters))
            {
                _counters[i] = i;
            }
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object Clone()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Returns current state of the complex counter.
        /// </summary>
        /// <returns>Current state of the complex counter.</returns>
        public virtual List<int> CurrentState()
        {
            return _counters;
        }

        /// <summary>
        /// Resets the complex counter to its initial position.
        /// </summary>
        /// <returns>if progress is possible after resetting.</returns>
        public virtual bool Reset()
        {
            foreach (var i in Enumerable.Range(0, _number_of_counters))
            {
                _counters[i] = i;
            }
            return _number_of_counters * _counter_size > 0;
        }

        /// <summary>
        /// Make the progress to the complex counter. 
        /// At the same time, determine if complex counter can progress at all.
        /// </summary>
        /// <returns>if progress is successful</returns>
        public virtual bool Progress()
        {
            var finish = true;
            foreach (var i in Enumerable.Range(0, _number_of_counters - 0))
            {
                if (_counters[i] < _counter_size - 1)
                {
                    finish = false;
                }
            }
            if (finish)
            {
                return false;
            }
            var ind_not_max = _number_of_counters - 1;
            foreach (var i in Enumerable.Range(0, Convert.ToInt32(Math.Ceiling(Convert.ToDouble(-1 - ind_not_max) / -1))).Select(_x_1 => ind_not_max + _x_1 * -1))
            {
                if (_counters[i] < _counter_size - 1)
                {
                    ind_not_max = i;
                    break;
                }
            }
            _counters[ind_not_max] += 1;
            foreach (var i in Enumerable.Range(ind_not_max + 1, _number_of_counters - (ind_not_max + 1)))
            {
                _counters[i] = 0;
            }
            return true;
        }
    }

    /// testing the developed class
    //public static void main()
    //{
    //    var cc = new ComplexCounterUniformAscending(4, 6);
    //    var can_progress = cc.reset();
    //    foreach (var i in Enumerable.Range(1, 1400 - 1))
    //    {
    //        Console.WriteLine(cc.current_state());
    //        can_progress = cc.progress();
    //    }
    //}

}
