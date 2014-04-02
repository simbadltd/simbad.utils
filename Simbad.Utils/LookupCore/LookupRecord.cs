using System.Runtime.Serialization;

namespace Simbad.Utils.LookupCore
{
    [DataContract]
    public class LookupRecord<TName, TValue>
    {
        [DataMember]
        public TName Name { get; set; }

        [DataMember]
        public TValue Value { get; set; }

        #region ctor

        public LookupRecord()
        {
        }

        public LookupRecord(TName name, TValue value)
        {
            Name = name;
            Value = value;
        }

        #endregion
    }
}