using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSpectre.Server.Data
{
    [Table("admin")]
    public class Admin
    {
        [Key][Column("username"] public string Username { get; set; }

        [Column("password")] public string Password { get; set; }
    }
}