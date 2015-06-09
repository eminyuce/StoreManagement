using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManagement.Data.EmailHelper
{
    /// <summary>
    /// Represents an email account
    /// </summary>
    public class EmailAccount
    {
        /// <summary>
        /// Gets or sets an email address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets an email display name
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets an email host
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets an email port
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets an email user name
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets an email password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value that controls whether the SmtpClient uses Secure Sockets Layer (SSL) to encrypt the connection
        /// </summary>
        public bool EnableSsl { get; set; }

        /// <summary>
        /// Gets or sets a value that controls whether the default system credentials of the application are sent with requests.
        /// </summary>
        public bool UseDefaultCredentials { get; set; }

        /// <summary>
        /// Gets a friendly email account name
        /// </summary>
        public string FriendlyName
        {
            get
            {
                if (!String.IsNullOrWhiteSpace(this.DisplayName))
                    return this.Email + " (" + this.DisplayName + ")";
                return this.Email;
            }
        }


        public static EmailAccount GetAdminEmailAccount()
        {
            var emailAccount = new EmailAccount();
            emailAccount.Username = ProjectAppSettings.GetWebConfigString("AdminEmailAccount_Username");
            emailAccount.Email = ProjectAppSettings.GetWebConfigString("AdminEmailAccount_Email");
            emailAccount.DisplayName = ProjectAppSettings.GetWebConfigString("AdminEmailAccount_DisplayName");
            emailAccount.Password = ProjectAppSettings.GetWebConfigString("AdminEmailAccount_Password");
            emailAccount.Host = ProjectAppSettings.GetWebConfigString("AdminEmailAccount_Host");
            emailAccount.Port = ProjectAppSettings.GetWebConfigInt("AdminEmailAccount_Port");
            emailAccount.EnableSsl = ProjectAppSettings.GetWebConfigBool("AdminEmailAccount_EnableSsl");
            emailAccount.UseDefaultCredentials = ProjectAppSettings.GetWebConfigBool("AdminEmailAccount_UseDefaultCredentials");

            return emailAccount;
        }
    }
}
