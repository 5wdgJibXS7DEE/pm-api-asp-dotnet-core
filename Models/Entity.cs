using System;

namespace ProjectManagement.Models
{
    public class Entity
    {
        /// <summary>
        /// The internal ID is never exposed outside of our application domain.
        /// We use it as a storing identifier that we can change when needed.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The external ID is exposed outside of our application domain.
        /// We use it as a functional identifier.
        /// </summary>
        public Guid ExternalId { get; set; }
    }
}