using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ARP.DataModels;

public class Character
{
    /// <summary>Уникальный идентификатор персонажа</summary>
    [Key]
    public uint Id { get; set; }

    /// <summary>Уникальный идентификатор аккаунта владельца</summary>
    public uint OwnerId { get; set; }

    /// <summary>Уникальное имя персонажа</summary>
    public string Name { get; set; } = null!;

    /// <summary>Уровень</summary>
    public ushort Level { get; set; } = 1;

    /// <summary>Опыт</summary>
    public uint Exp { get; set; } = 0;

    /// <summary>Сумма денег на руках</summary>
    public ulong Money { get; set; } = 0;

    /// <summary>Пол персонажа. false - Мужской, true - Женский.</summary>
    public bool Gender { get; set; }

    /// <summary>Дата регистрации</summary>
    public DateTime RegDate { get; set; }

    /// <summary>Дата последнего входа</summary>
    public DateTime? LastDate { get; set; }

    /// <summary>IP при регистрации</summary>
    public string RegIp { get; set; } = null!;

    /// <summary>IP при последнем входе</summary>
    public string? LastIp { get; set; }

    public byte? Fraction { get; set; }
    public ushort[]? Business { get; set; }
    public ushort[]? Homes { get; set; }

    //[Column(TypeName = "jsonb")]
    //public CharacterInventory Inventory { get; set; } = new();

    public bool IsAdminNotifyOn { get; set; } = true;

    [Column(TypeName = "jsonb")]
    public MuteInfo? Mute { get; set; }

    public List<uint> Friends { get; set; } = new();

    /// <summary>Уникальный идентификатор аккаунта владельца</summary>
    [Column(TypeName = "jsonb")]
    public PedCustomization Customization { get; set; } = null!;

    /// <summary>Предупреждения персонажа. 3 - бан</summary>
    public byte Warn { get; set; } = 0;

    public string GetName() => Name.Replace('_', ' ');

    public string GetNameForId(uint id) => Friends.Contains(id) || Id == id ? GetName() : "Незнакомец";
}

public class CustomHeadBlend
{
    public uint Mather { get; set; }
    public uint Father { get; set; }
    public float Shape { get; set; }
    public float Skin { get; set; }
}

public class CustomHeadOverlay
{
    public byte Index { get; set; }
    public byte FirstColor { get; set; }
    public byte SecondColor { get; set; }
    public float Opacity { get; set; }
}

public class PedCustomization
{
    public CustomHeadBlend HeadBlendData { get; set; } = null!;
    public Dictionary<byte, float> FaceFeatures { get; set; } = null!;
    public Dictionary<byte, CustomHeadOverlay> HeadOverlays { get; set; } = null!;
    public byte[] Hair { get; set; } = null!;
    public byte EyeColor { get; set; }
}

public class MuteInfo
{
    public DateTime Date { get; set; }
    public string Reason { get; set; }
    public uint AdminId { get; set; }
    public ushort Minute { get; set; }
}