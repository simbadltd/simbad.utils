using System.Runtime.Serialization;

namespace Simbad.Utils.Domain.Infrastructure
{
    /// <summary>
    /// Validation error.
    /// </summary>
    [DataContract]
    public class ValidationError
    {
        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        [DataMember]
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationError"/> class.
        /// </summary>
        /// <param name="propertyName">
        /// Name of the property.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>

        public ValidationError(string propertyName, string message)
        {
            PropertyName = propertyName;
            Message = message;
        }
    }
}