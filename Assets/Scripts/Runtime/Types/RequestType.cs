namespace Runtime.Types
{
    /// <summary>
    /// Request type for elements, equal to CRUD operations.
    /// </summary>
    public enum RequestType
    {
        /// <summary>
        /// Create new entity.
        /// </summary>
        Create = 0,

        /// <summary>
        /// Read or select entity.
        /// </summary>
        Select = 1,

        /// <summary>
        /// Update or Edit entity.
        /// </summary>
        Edit = 2,

        /// <summary>
        /// Delete entity.
        /// </summary>
        Delete = 3,
    }
}