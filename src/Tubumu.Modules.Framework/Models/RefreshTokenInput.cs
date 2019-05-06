using System.ComponentModel.DataAnnotations;

namespace Tubumu.Modules.Framework.Models
{
    /// <summary>
    /// RefreshToken Input
    /// </summary>
    public class RefreshTokenInput
    {
        /// <summary>
        /// Token
        /// </summary>
        [Required]
        public string Token { get; set; }

        /// <summary>
        /// RefreshToken
        /// </summary>
        [Required]
        public string RefreshToken { get; set; }
    }
}
