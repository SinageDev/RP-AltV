using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARP.DataModels.Static
{
    public enum LocalizeData : uint
    {

    }

    public enum Region : byte
    {
        Ru,
        En,
    }

    public class Localize
    {
        [Key]
        public LocalizeData Id { get; set; }

        public string RuText { get; set; } = "TODO";
        public string EnText { get; set; } = "TODO";
    }
}
