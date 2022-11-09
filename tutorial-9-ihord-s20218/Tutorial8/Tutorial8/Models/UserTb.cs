using System;

namespace Tutorial9.Models
{
    public class UserTb
    {
        public int IdUserTb { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public byte[] Salt { get; set; }
        public string RefreshedToken { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
