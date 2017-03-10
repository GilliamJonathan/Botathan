using Discord.Commands;
using System;
using System.Threading.Tasks;

public class GeneralModule : ModuleBase
{
    [Command("ping"), Summary("Pings the bot to check response time")]
    public async Task Ping()
    {
        await ReplyAsync("Pong");
    }

    [Command("pastebin"), Summary("Sends the pastebin link"), Alias("pb")]
    public async Task PasteBin()
    {
        await ReplyAsync("Nobody wants to see your bad code in chat, send a link via pastebin");
        await ReplyAsync("http://pastebin.com/");
    }

    [Command("roulette"), Summary("Play a game a roulette with yourself"), Alias("rt")]
    public async Task Roulette()
    {
        Random ran = new Random();
        await ReplyAsync("_You place the revolver to your head_");
        System.Threading.Thread.Sleep(1500);

        if (ran.Next(1, 7) == 1)
            await ReplyAsync("**BANG**");
        else
            await ReplyAsync("_click_");
    }

    [Command("roll"), Summary("Roll a dice"), Alias("r")]
    public async Task Roll([Summary("Sides of the die")]int n = 6)
    {
        Random ran = new Random();
        await ReplyAsync("It was a " + ran.Next(1, n+1));
    }

    [Command("flip"), Summary("Flip a coin")]
    public async Task Flip()
    {
        Random ran = new Random();
        if(ran.Next(1, 3)==1)
            await ReplyAsync("It was head");
        else
            await ReplyAsync("It was tails");
    }
}