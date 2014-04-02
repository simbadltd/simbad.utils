using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Simbad.Utils.Domain.Infrastructure
{
    /// <summary>
    /// The base class for domain entities.
    /// </summary>
    [DataContract]
    public abstract class EntityBase : IIdentityObject
    {
        public const int UNKNOWN_ID = -2147483648;

        /// <summary>
        /// Identity field
        /// </summary>
        [DataMember]
        public virtual int Id { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance contains in database.
        /// </summary>
        public virtual bool IsNew
        {
            get
            {
                return Id < 1;
            }
        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        protected virtual void Validate()
        {
        }
    }
}
