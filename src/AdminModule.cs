using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class AdminModule : ModuleBase
{
    [Command("purge"), Summary("Clears the chat"), RequireUserPermission(ChannelPermission.ManageMessages)]
    public async Task Purge([Remainder, Summary("Number of messages to clear")] string amount)
    {
        int amountToDelete = 0;
        if (amount.ToLower().Equals("all"))
            amountToDelete = 99;
        else
            Int32.TryParse(amount, out amountToDelete);

        amountToDelete++;
        int index = 0;
        var deleteMessages = new List<IMessage>(amountToDelete);
        var messages = Context.Channel.GetMessagesAsync();
        await messages.ForEachAsync(async m =>
        {
            IEnumerable<IMessage> delete = null;
            delete = m;

            foreach (var msg in delete.OrderByDescending(msg => msg.Timestamp))
            {
                if (index >= amountToDelete) { await Context.Channel.DeleteMessagesAsync(deleteMessages); return; }
                deleteMessages.Add(msg);
                index++;
            }
        });

        await ReplyAsync($"Deleted {amountToDelete-1} messages, consider your secret safe");
    }

    [Command("kick"), Summary("Kicks a user"), RequireUserPermission(GuildPermission.KickMembers)]
    public async Task Kick([Summary("User to kick")]IUser user = null, [Remainder, Summary("Optional reason for kick")] string reason = "")
    {
        await ((IGuildUser)user).KickAsync();
        await ReplyAsync(user.Username.ToString() + " was kicked - " + reason);
    }

    [Command("ban"), Summary("Bans a user"), RequireUserPermission(GuildPermission.BanMembers)]
    public async Task Ban([Summary("User to ban")]IUser user = null, int days = 0, [Remainder, Summary("Optional reason for ban")] string reason = "")
    {
        await Context.Guild.AddBanAsync(user, days);
        await ReplyAsync(user.Username.ToString() + " was banned - " + reason);
    }

    [Command("unban"), Summary("Unbans a user"), RequireUserPermission(GuildPermission.BanMembers)]
    public async Task Unban([Remainder, Summary("Username of user to unban")]String user = null)
    {
        IReadOnlyCollection<IBan> bans = await Context.Guild.GetBansAsync();

        foreach(var tempUser in bans)
        {
            if(tempUser.User.Username == user)
                await Context.Guild.RemoveBanAsync(tempUser.User);
        }
        await ReplyAsync(user + " was unbanned");
    }
}