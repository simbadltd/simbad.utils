using System;

namespace Simbad.Utils.Domain.Infrastructure
{
    public class CacheInvalidatedPartiallyEventArgs: EventArgs
    {
        public int InvalidItemId { get; private set; }

        public CacheInvalidatedPartiallyEventArgs(int invalidItemId)
        {
            InvalidItemId = invalidItemId;
        }
    }
}