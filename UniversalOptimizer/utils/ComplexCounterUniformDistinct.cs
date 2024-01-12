namespace UniversalOptimizer.utils
{

    public class ComplexCounterUniformAscending: ICloneable
    {

        private int _counter_size;

        private int[] _counters;

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
            _counters = new int[number_of_counters];
            for (int i = 0; i < _counters.Length; i++)
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
        public virtual object Clone() => throw new NotImplementedException();


        /// <summary>
        /// Returns current state of the complex counter.
        /// </summary>
        /// <returns>Current state of the complex counter.</returns>
        public virtual int[] CurrentState() => _counters;

        /// <summary>
        /// Resets the complex counter to its initial position.
        /// </summary>
        /// <returns>if progress is possible after resetting.</returns>
        public virtual bool Reset()
        {
            for (int i = 0; i < _counters.Length; i++)
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
            for (int i = 0; i < _counters.Length; i++)
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
            int ind_not_max = _number_of_counters - 1;
            for (int i = ind_not_max; i >= -0; i--)
            {
                if (_counters[i] < _counter_size - 1)
                {
                    ind_not_max = i;
                break;
                }
            }
            _counters[ind_not_max] += 1;
            for (int i = ind_not_max + 1; i < _number_of_counters; i++)
                _counters[i] = 0;
            return true;
        }
    }

}
