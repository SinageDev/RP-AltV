using AltV.Net;
using AltV.Net.Async;
using ARP.Commands;
using ARP.Core.Attributes;
using ARP.DataModels.Static;
using ARP.Factories;
using ARP.Managers;
using ARP.Utils;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text;

namespace ARP.Core;

public class ChatHandler : IScript
{
    public const string BackgroundStyle =
      "border-radius:0.4vh;margin-right:0.3vw;font-size:1.25vh;padding: 0.1vh 0.3vw;font-weight:500;";

    public const string RedBackground =
      "background: radial-gradient(59.46% 59.46% at 50% 15.54%, rgba(255, 255, 255, 0.2) 0%, rgba(255, 255, 255, 0) 100%), linear-gradient(203.88deg, #831C1C 15.34%, #761616 36.08%, #7B1717 68.89%, #881B1B 84.96%);";

    private static readonly ConcurrentDictionary<string, CommandInfo> Commands = new();

    [AsyncClientEvent("Chat:Command")]
    public Task OnPlayerCommand(EPlayer player, string command, params string[] args)
    {
        if (player.Character == null || player.Account == null) return Task.CompletedTask;
        if (!Commands.TryGetValue(command, out CommandInfo cmdInfo))
        {
            player.SendChatMessage($"Команда /{command} не найдена!");
            return Task.CompletedTask;
        }

        if (cmdInfo.Access == CommandAccessType.Admin && (player.Admin == null || player.Admin.Level < cmdInfo.AccessLevel)) return Task.CompletedTask;

        if (cmdInfo.IsMessage && player.Character.Mute != null)
        {
            player.SendMuteMessage();
            return Task.CompletedTask;
        }

        List<object> arguments = args.Cast<object>().ToList();

        MethodInfo method = cmdInfo.Method;
        ParameterInfo[] parameters = method.GetParameters().Where(x => x.ParameterType != typeof(EPlayer)).ToArray();

        if (parameters.Count(x => !x.IsOptional) > arguments.Count)
        {
            player.SendChatMessage(BuildCommandError(command));
            return Task.CompletedTask;
        }

        try
        {
            for (var i = 0; i < parameters.Length; i++)
            {
                if (i >= arguments.Count)
                {
                    arguments.Add(parameters[i].DefaultValue ?? Type.Missing);
                    continue;
                }
                Type type = parameters[i].ParameterType;
                if (type.IsArray)
                {
                    List<object> newList = arguments.GetRange(i, arguments.Count - i);
                    arguments.RemoveRange(i, arguments.Count - i);
                    Type elementType = type.GetElementType() ?? typeof(object);
                    List<object> convertArray = newList.ConvertAll(x => Convert.ChangeType(x, elementType));
                    if (elementType == typeof(string)) arguments.Add(convertArray.Cast<string>().ToArray());
                    else if (elementType == typeof(float)) arguments.Add(convertArray.Cast<float>().ToArray());
                    break;
                }

                arguments[i] = Convert.ChangeType(arguments[i], type);
            }

            arguments.Insert(0, player);
            method.Invoke(null, arguments.ToArray());
        }
        catch (Exception e)
        {
            Alt.LogFast(e.Message);
            player.SendChatMessage(BuildCommandError(command));

            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }


    [AsyncClientEvent("Chat:Message")]
    public Task OnPlayerChatMessage(EPlayer player, string message)
    {
        if (player.Character is null || player.Account is null) return Task.CompletedTask;
        if (player.Character.Mute != null)
        {
            player.SendMuteMessage();
            return Task.CompletedTask;
        }

        DateTime date = Server.GetDateTime();
        foreach (EPlayer target in Server.GetPlayersInRange(player.Position, 15))
        {
            target.SendChatMessage(
              new CustomString(new StyleText($"[{date:HH:mm}] ", "opacity: 0.7"),
                new StyleText(player.Character!.GetNameForId(target.Character!.Id), "font-weight:700"),
                new StyleText($" #{player.Character!.Id}:", "opacity:0.7")),
              PackMessage(message));
        }

        return Task.CompletedTask;
    }

    public static void SendChatAdminNotify(object header)
    {
        foreach (EPlayer player in Pool.GetPlayers().Where(x => x.Character is { IsAdminNotifyOn: true }))
        {

            player.SendChatMessage(header);
        }
    }
    public static void SendChatAdminNotify(object header, object message)
    {
        foreach (EPlayer player in Pool.GetPlayers().Where(x => x.Character is { IsAdminNotifyOn: true }))
            player.SendChatMessage(header, message);
    }

    private static readonly string[][] SecureXss =
    [
        ["<", "&lt;"],
        [">", "&gt;"],
        ["'", "&#39;"],
        ["\"", "&#34;"]
    ];

    public static string PackMessage(string[] message)
    {
        return SecureXss.Aggregate(string.Join(' ', message), (current, secure) => current.Replace(secure[0], secure[1]));
    }

    public static string PackMessage(string message)
    {
        return SecureXss.Aggregate(message, (current, secure) => current.Replace(secure[0], secure[1]));
    }

    private static string BuildCommandError(string command)
    {
        CommandInfo cmdInfo = Commands.GetValueOrDefault(command)!;
        ParameterInfo[] parameters = cmdInfo.Method.GetParameters().Where(x => x.ParameterType != typeof(EPlayer)).ToArray();
        StringBuilder result = new($"Используйте /{command} ");
        if (cmdInfo.Arguments != null)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                string parameterName = cmdInfo.Arguments.Length <= i ? parameters[i].Name : cmdInfo.Arguments[i];
                result.Append(parameters[i].IsOptional ? $"{{{parameterName}}}" : $"[{parameterName}]").Append(' ');
            }
        }
        else
        {
            foreach (ParameterInfo parameter in parameters)
            {
                result.Append(parameter.IsOptional ? $"{{{parameter.Name}}}" : $"[{parameter.Name}]").Append(' ');
            }
        }
        return result.ToString();
    }

    public static void SendAdminMessage(object header)
    {
        foreach (EPlayer admin in Pool.GetAdmins()) admin.SendChatMessage(header);
    }

    public static void SendAdminMessage(object header, object text)
    {
        foreach (EPlayer admin in Pool.GetAdmins()) admin.SendChatMessage(header, text);
    }

    public static List<string> GetAdminCommands(uint level)
    {
        return Commands.Where(x => x.Value.Access == CommandAccessType.Admin && x.Value.AccessLevel <= level).Select(x => $"{x.Key} - {x.Value.Description}").ToList();
    }

    public static void InitCommand()
    {
        Type? types = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(x => x == typeof(PlayerCommands));
        if (types == null)
        {
            Alt.LogError("Команд не найдено!");
            return;
        }

        MethodInfo[] methods = types.GetMethods();
        using StaticContext staticContext = new();
        foreach (MethodInfo method in methods)
        {
            CommandAttribute? commandAttr = method.GetCustomAttribute<CommandAttribute>();
            if (commandAttr == null) continue;
            CommandInfo? commandInfo = staticContext.Commands.FirstOrDefault(x => x.Name == commandAttr.Name);
            if (commandInfo == null)
            {
                commandInfo = new CommandInfo
                {
                    Name = commandAttr.Name,
                    IsMessage = commandAttr.IsMessage,
                    Arguments = commandAttr.Arguments,
                    Method = method,
                };
                staticContext.Commands.Add(commandInfo);
            }
            else
            {
                commandInfo.IsMessage = commandAttr.IsMessage;
                commandInfo.Method = method;
                commandInfo.Arguments = commandAttr.Arguments;
            }
            Commands.TryAdd(commandAttr.Name, commandInfo);
            foreach (string alias in commandInfo.Aliases) Commands.TryAdd(alias, commandInfo);
        }
        staticContext.SaveChanges();
    }
}