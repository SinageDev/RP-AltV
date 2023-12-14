using ARP.Managers;

namespace ARP.Core.Singleton;

public static class GlobalInstances
{
    public static LocalizeText LocalizeText => LocalizeText.Instance;
}