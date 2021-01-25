namespace Application.Authentication.Dtos
{
    /// <summary>
    /// DTO representing a create user
    /// </summary>
    public class CreatedUserDto
    {
        /// <summary>
        /// Name of the created user
        /// </summary>
        public string Username { get; set; } = string.Empty;
    }
}
