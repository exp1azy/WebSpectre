using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSpectre.Client.Data
{
    [Table("Admin")]
    public class User
    {
        [Key] public string Username { get; set; }

        public string Password { get; set; }
    }
}