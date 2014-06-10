using System;
using System.Collections.Generic;

namespace Simbad.Utils.LookupCore
{
    [Serializable]
    public class LookupTable<TName, TValue> : List<LookupRecord<TName, TValue>>, ILookupTable<TName, TValue>
    {
        public TValue GetValueByName(TName name)
        {
            foreach (var lookupRecord in this)
            {
                if (name.Equals(lookupRecord.Name))
                {
                    return lookupRecord.Value;
                }
            }

            return default(TValue);
        }
    }
}