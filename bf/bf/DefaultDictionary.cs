using System.Collections.Generic;

namespace bf
{
    /// <summary>
    /// An analogous of python's defaultdict.
    /// Getting a key that does not exist creates a new object of the given type.
    /// </summary>
    public class DefaultDictionary<Tkey, TValue> : Dictionary<Tkey, TValue> where TValue: new()
    {
        public new TValue this[Tkey key]
        {
            get
            {
                try
                {
                    return base[key];
                }
                catch (KeyNotFoundException)
                {
                    TValue retval = new TValue();
                    base[key] = retval;
                    return retval;
                }
            }

            set { base[key] = value; }
        }
    }
}
