using System.Text;
using AltV.Net;
using System.Text.RegularExpressions;
using System.Drawing;

namespace ARP.Managers;

public class CustomString
{
    private readonly StringBuilder _stringBuilder = new();
    private readonly StringBuilder _styles = new();

    public CustomString(params object[] styleTexts)
    {
        foreach (var style in styleTexts)
        {
            switch (style)
            {
                case string:
                    _stringBuilder.Append(style);
                    break;
                case StyleText styleText:
                    _stringBuilder.Append(styleText);
                    break;
            }
        }
    }

    public CustomString SetOpacity(float opacity)
    {
        _styles.Append($"opacity:{opacity};");
        return this;
    }

    public CustomString SetColor(byte r, byte g, byte b)
    {
        _styles.Append($"color:rgb({r}, {g}, {b});");
        return this;
    }

    public CustomString SetColor(byte r, byte g, byte b, byte a)
    {
        _styles.Append($"color:rgba({r}, {g}, {b}, {a});");
        return this;
    }

    public CustomString SetColor(string color)
    {
        _styles.Append($"color:{color};");
        return this;
    }

    public CustomString SetFontWeight(ushort weight)
    {
        if(weight is < 100 or > 900)
        {
            Alt.LogError($"[{nameof(CustomString)}]: {nameof(SetFontWeight)} value is out of range");
            return this;
        }
        _styles.Append($"font-weight:{weight};");
        return this;
    }

    public override string ToString()
    {
        _stringBuilder.Insert(0, $"<span style=\"{_styles}\">");
        _stringBuilder.Append("</span>");
        return _stringBuilder.ToString();
    }
}

public class StyleText
{
    private StringBuilder _styleBuilder { get; } = new();
    public StyleText(string text, string styles)
    {
        _styleBuilder.Append(text);
        Regex.Replace(styles, @"\s+", "");
        _styleBuilder.Insert(0, $"<span style=\"{styles}\">");
        _styleBuilder.Append("</span>");
    }

    public override string ToString()
    {
        return _styleBuilder.ToString();
    }
}