namespace DailymotionSDK.Models.Requests
{
    /// <summary>
    /// Class AuthRequest.
    /// </summary>
    public class AuthRequest
    {
        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>The client identifier.</value>
        public string? ClientId { get; set; }
        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        /// <value>The client secret.</value>
        public string? ClientSecret { get; set; }
        /// <summary>
        /// The grant type is read-only and is set to "client_credentials" for this request.
        /// </summary>
        public readonly string GrantType = "client_credentials";
        /// <summary>
        /// Gets or sets the scope.
        /// </summary>
        /// <value>The scope.</value>
        public OAuthScope[]? Scope { get; set; }
    }
}