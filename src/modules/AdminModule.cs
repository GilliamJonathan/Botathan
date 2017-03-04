	
public class AdminModule
{
	cService.CreateCommand("purge")
        .Alias(new String[] { "purge", "remove" })
        .Description("Clears the chat for the given amount")
        .AddCheck((cm, u, ch) => u.ServerPermissions.Administrator)
        .Parameter("amount", ParameterType.Required)
        .Do(async (e) =>
        {
            int amountToDelete = 0;
            if (e.GetArg("amount").ToLower().Equals("all"))
                amountToDelete = 100;
            else
                int.TryParse(e.GetArg("amount"), out amountToDelete);

            Message[] messagesToDelete;
            messagesToDelete = await e.Channel.DownloadMessages(amountToDelete+1);

            await e.Channel.DeleteMessages(messagesToDelete);
            await e.Channel.SendMessage($"Deleted {amountToDelete} messages, consider your secret safe");
        });

        cService.CreateCommand("kick")
        .Alias(new String[] { "kick" })
        .Description("Kicks a player")
        .AddCheck((cm, u, ch) => u.ServerPermissions.KickMembers)
        .Parameter("player", ParameterType.Required)
        .Parameter("reason", ParameterType.Unparsed)
        .Do(async e =>
        {
            if (e.Channel.FindUsers(e.GetArg("player")).ToArray().Length > 0)
            {
                await e.Channel.FindUsers(e.GetArg("player")).ToArray()[0].Kick();
                await e.Channel.SendMessage(e.GetArg("player") + " was kicked for " + e.GetArg("reason"));
            }
            else
                await e.Channel.SendMessage("Unable to kick player");
        });

        cService.CreateCommand("ban")
        .Alias(new String[] { "ban" })
        .Description("Bans a player")
        .AddCheck((cm, u, ch) => u.ServerPermissions.BanMembers)
        .Parameter("player", ParameterType.Required)
        .Parameter("reason", ParameterType.Unparsed)
        .Do(async e =>
        {
            if (e.Channel.FindUsers(e.GetArg("player")).ToArray().Length > 0)
            {
                await e.Server.Ban(e.Channel.FindUsers(e.GetArg("player")).ToArray()[0],0);
                await e.Channel.SendMessage(e.GetArg("player") + " was banned for " + e.GetArg("reason"));
            }
            else
                await e.Channel.SendMessage("Unable to ban player");

        });

        cService.CreateCommand("unban")
        .Alias(new String[] { "unban" })
        .Description("Unbans a player")
        .AddCheck((cm, u, ch) => u.ServerPermissions.BanMembers)
        .Parameter("player", ParameterType.Required)
        .Do(async e =>
        {
            if (e.Channel.FindUsers(e.GetArg("player")).ToArray().Length > 0)
            {
                await e.Server.Unban(e.Channel.FindUsers(e.GetArg("player")).ToArray()[0], 0);
                await e.Channel.SendMessage(e.GetArg("player") + " was unabnned");
            }
            else
                await e.Channel.SendMessage("Unable to unban player");

        });

}