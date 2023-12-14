namespace ARP.Core.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class CommandAttribute : Attribute
{
    public string Name { get; set; }

    public CommandAttribute(string defaultName)
    {
        Name = defaultName;
    }
    public string[]? Arguments { get; init; }
    public bool IsMessage { get; set; }
}