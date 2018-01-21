namespace Strata.API.Security {
    public class Token {

        /// <summary>
        /// The encoded token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Number of seconds token is valid for
        /// </summary>
        public double ExpiresIn { get; set; }
    }
}
