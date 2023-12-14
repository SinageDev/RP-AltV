using System.Collections.Concurrent;
using ARP.Core.Singleton;
using ARP.DataModels.Static;
using ARP.Utils;

namespace ARP.Managers;

public class LocalizeText : Singleton<LocalizeText>
{
    private ConcurrentDictionary<LocalizeData, Localize> _locales { get; } = new();

    public LocalizeText()
    {
        using StaticContext staticContext = new();
        foreach (Localize locale in staticContext.Strings)
        {
            _locales.TryAdd(locale.Id, locale);
        }
    }

    public string Get(LocalizeData data)
    {
        if (!_locales.TryGetValue(data, out Localize localize)) return "Localize Data is undefined";

        return Server.Locale switch
        {
            Region.En => localize.EnText,
            Region.Ru => localize.RuText,
            _ => "Undefined Region"
        };
    }

    public string Translate(LocalizeData data, params object[] args)
    {
        return string.Format(Get(data), args);
    }
}