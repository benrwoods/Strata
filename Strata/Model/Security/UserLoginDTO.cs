using System.ComponentModel.DataAnnotations;

namespace Strata.API.Model.Security {

    public class UserLoginDTO {

        /// <summary>
        /// User login
        /// </summary>
        [Required]
        public string Login { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
