namespace Infrastructure.Identity
{
    /// <summary>
    /// Enum describing the names of the roles used in this application
    /// </summary>
    /// <remarks>
    /// The user role is the default one and is not referenced here nor somewhere else. A user is defined as someone
    /// with no additional role
    /// </remarks>
    public enum ApplicationRoles
    {
        /// <summary>
        /// Represent an administrator, with higher privileges than a normal user
        /// </summary>
        Administrator,
    }
}
