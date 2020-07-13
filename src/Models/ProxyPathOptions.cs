namespace Solidcube.AspNetCore.ProxyHelper.Models
{
    /// <summary>
    /// The forwarded header path options
    /// </summary>
    public class ProxyPathOptions
    {
        /// <summary>
        /// Gets or sets the header key contains the path.
        /// </summary>
        /// <value>
        /// The header key.
        /// </value>
        public string XForwardedHeaderPath { get; set; }
    }
}