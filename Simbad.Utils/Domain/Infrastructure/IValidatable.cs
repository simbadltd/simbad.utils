using System.Collections.Generic;

namespace Simbad.Utils.Domain.Infrastructure
{
    /// <summary>
    /// IValidatable interface.
    /// </summary>
    public interface IValidatable
    {
        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        bool IsValid { get; }

        /// <summary>
        /// Gets the validation errors.
        /// </summary>
        /// <value>
        /// The validation errors.
        /// </value>
        List<ValidationError> ValidationErrors { get; }
    }
}
