namespace Tubumu.Modules.Framework.Authorization
{
    /// <summary>
    /// TubumuClaimTypes
    /// </summary>
    public class TubumuClaimTypes
    {
        /// <summary>
        /// Group
        /// </summary>
        public const string Group = "g"; // 注意不要使用 group，否则会被篡改。

        /// <summary>
        /// Permission
        /// </summary>
        public const string Permission = "p";
    }
}
