using System.Collections.Generic;

namespace Simbad.Utils.Domain.Infrastructure
{
    /// <summary>
    /// Validation errors.
    /// </summary>
    public class ValidationErrors
    {
        /// <summary>
        /// The _errors
        /// </summary>
        private readonly List<ValidationError> _errors;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationErrors" /> class.
        /// </summary>
        public ValidationErrors()
        {
            _errors = new List<ValidationError>();
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public IList<ValidationError> Items
        {
            get { return _errors; }
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="errors">
        /// The errors.
        /// </param>
        public void AddRange(IList<ValidationError> errors)
        {
            _errors.AddRange(errors);
        }

        /// <summary>
        /// Clears the items.
        /// </summary>
        internal void Clear()
        {
            _errors.Clear();
        }
    }
}
