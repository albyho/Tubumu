namespace Tubumu.Modules.Framework.Authorization
{
    public class TokenValidationSettings
    {
        public string ValidIssuer { get; set; }

        public string ValidAudience { get; set; }

        public string IssuerSigningKey { get; set; }

        public bool ValidateLifetime { get; set; }

        public int ClockSkewSeconds { get; set; }

        public int ExpiresSeconds { get; set; }

        public int RefreshTokenExpirationSeconds { get; set; }
    }
}
