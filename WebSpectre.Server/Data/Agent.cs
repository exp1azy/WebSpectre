using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSpectre.Server.Data
{
    [Table("agent")]
    public class Agent
    {
        [Key][Column("hostname")] public string Hostname { get; set; }

        [Column("url")] public string Url { get; set; }
    }
}
