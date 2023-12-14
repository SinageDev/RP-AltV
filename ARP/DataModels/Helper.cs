using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ARP.DataModels;

public class Helper
{
    [Key]
    public uint Id { get; set; }
    public uint OwnerId { get; set; }
    public byte Level { get; set; }

    [NotMapped]
    public bool DlEnabled { get; set; }
}