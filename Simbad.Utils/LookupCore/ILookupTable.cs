using System.Collections.Generic;

namespace Simbad.Utils.LookupCore
{
    public interface ILookupTable<TName, TValue>: IList<LookupRecord<TName, TValue>>
    {
        TValue GetValueByName(TName name);
    }
}