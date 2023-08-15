namespace Kirel.Identity.Server.Domain
{
    /// <summary>
    /// Configuration class for users
    /// </summary>
    public class UserConfig
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the user's first name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the user's last name.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets the list of roles associated with the user.
        /// </summary>
        public List<string>? Roles { get; set; } = new List<string>();
    }
}