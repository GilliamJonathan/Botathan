
public class GeneralModule
{
	cService.CreateCommand("ping")
        .Alias(new String[] { "ping" })
        .Description("Pings the bot")
        .Do(async e =>
        {
            await e.Channel.SendMessage("pong");
        });

        cService.CreateCommand("pastebin")
        .Alias(new String[] { "pastebin", "badcode" , “pastabin” })
        .Description("Sends the pastebin link")
        .Do(async e =>
        {
            await e.Channel.SendMessage("Nobody wants to see your bad code in chat, send a link via pastebin");
            await e.Channel.SendMessage("http://pastebin.com/");
        });

	cService.CreateCommand(“roulette”)
        .Alias(new String[] { “roulette”, “rt” })
        .Description(“Play a game a roulette with yourself“)
        .Do(async e =>
        {
            await e.Channel.SendMessage(“_You place the revolver to your head_”);
	    System.Threading.Thread.Sleep(1500);

	    if(ran.Next(1, 6)==1)
		await e.Channel.SendMessage(“**BANG**”);
	    else
   	    	await e.Channel.SendMessage(“_click_”);
	    
        });

	cService.CreateCommand(“roll”)
        .Alias(new String[] { “roll”, “r” })
        .Description(“Roll a dice“)
	.Parameter(“index”, ParameterType.Unparsed)
        .Do(async e =>
        {
	    int index = 6;
	    if (e.GetArg("reason”)!=“”)
		index = int32.parse(e.GetArg("reason”));

            await e.Channel.SendMessage(“Rolling a dice..”);
	    System.Threading.Thread.Sleep(1000);
            await e.Channel.SendMessage(“It was a “ + ran.Next(1, index));
        });

	cService.CreateCommand(“flip”)
        .Alias(new String[] { “coin”, “flip”, “c”, “f” })
        .Description("Sends the pastebin link")
        .Do(async e =>
        {
            await e.Channel.SendMessage(“Flipping a coin..”);
	    System.Threading.Thread.Sleep(1000);
	    if (ran.Next(1, 2)==1)
            	await e.Channel.SendMessage(“Heads“);
	    else
		await e.Channel.SendMessage(“Tails”);
        });

}