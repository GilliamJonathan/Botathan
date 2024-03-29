using System;
using System.Linq;
using Discord;
using Discord.Commands;
using System.Net;
using System.IO;
using Google.Apis.Customsearch.v1;
using Google.Apis.Services;
using System.Text.RegularExpressions;

class Botathan
{
    private static string botid = "MjU1NDU1MTMxMDgyOTQ4NjA4.Cyeybw.0dR4xHZf2mQ6_fOPz0x4MxV4dLM";
    private static char botext = '$';

    static void Main(string[] args)
    {
        if (false)
        {
            botid = "MjU3MTk3MDQ0MTQ4NjAwODMy.Cy3M8g.yCb63W0qf6LxzzDq_0dwTBqq4Y4";
            botext = '/';
        }

        new Botathan().Start();
    }

    private Random ran;
    private DiscordClient _client;

    public void Start()
    {
        ran = new Random();

        //creates _client
        _client = new DiscordClient(x =>
        {
            x.AppName = "Botathan";
            x.AppUrl = "https://sites.google.com/site/jonationfigureitout/";
            x.LogLevel = LogSeverity.Info;
            x.LogHandler = Log;
        });

        //sets bot command settings
        _client.UsingCommands(x =>
        {
            x.PrefixChar = botext;
            x.AllowMentionPrefix = false; //don't require @ mention for commands
            x.HelpMode = HelpMode.Private; //don't send help message in chat
        });

        CreateCommands();

        _client.ExecuteAndWait(async () =>
        {
            await _client.Connect(botid, TokenType.Bot);
        });
    }

    private CommandService cService;

    public void CreateCommands()
    {
        //creates command service
        cService = _client.GetService<CommandService>();

        SearchModule();
        LogModule();
        GeneralModule();
        ChattyModule();
        AdminModule();
    }


    public void SearchModule()
    {
        //api key
        string apiKey = "AIzaSyC4znIVRxU53mYW5zBxZb48d0f6ccf4yJI";
        //creates search service
        CustomsearchService svc = new Google.Apis.Customsearch.v1.CustomsearchService(new BaseClientService.Initializer { ApiKey = apiKey });
        //Custom searching engine id
        string cx = "017085880482068702817:mkkqkl2yybo";
        string query = "error";

        cService.CreateCommand("image")
        .Alias(new String[] { "search", "send", "searchfor", "image" })
        .Description("Sends a random photo from google")
        .Parameter("search", ParameterType.Unparsed)
        .Do(async e =>
        {
            int index = 0;
            Regex rgx = new Regex(@"(-\d{1,3})");
            String temp = e.GetArg("search");

            if (temp.Contains(' '))
                temp = temp.Substring(0, e.GetArg("search").IndexOf(' '));

            if (rgx.IsMatch(temp))
            {
                index = System.Convert.ToInt32(temp.Substring(1));
                query = e.GetArg("search").Substring(e.GetArg("search").IndexOf(' ')).Replace("_", " ");
            }
            else
                query = e.GetArg("search"); //formats search to be engine friendly

            var listRequest = svc.Cse.List(query);

            //turns off safe mode if request is located in a chat with "nsfw" in title
            if (e.Channel.Name.ToUpper().Contains("NSFW"))
            {
                System.Console.WriteLine("NSFW");
                listRequest.Safe = CseResource.ListRequest.SafeEnum.Off;
            }
            else
                listRequest.Safe = CseResource.ListRequest.SafeEnum.High;

            //sets the reqest's engine
            listRequest.Cx = cx;

            //searches for images only
            listRequest.SearchType = CseResource.ListRequest.SearchTypeEnum.Image;

            if (index == 0)
                index = ran.Next(1, 100); //picks a random image from the top 100

            Console.WriteLine("radom index: " + index);

            listRequest.Start = index;
            var search = listRequest.Execute();

            WebClient wc = new WebClient(); //creaes webclient
            byte[] bytes = wc.DownloadData(search.Items[0].Link); //gets the iomage from the enginesearch
            MemoryStream ms = new MemoryStream(bytes); //converts image into a memoryStorage

            await e.Channel.SendMessage("[-" + index + " " + query + "]");
            await e.Channel.SendFile(".jpg", ms); //send image as jpeg
        });

        cService.CreateCommand("gif")
        .Alias(new String[] { "gif", "sendgif", "searchforgif" })
        .Description("Sends a gif")
        .Parameter("search", ParameterType.Unparsed)
        .Do(async e =>
        {
            int index = 0;
            Regex rgx = new Regex(@"(-\d{1,3})");
            String temp = e.GetArg("search");

            if (temp.Contains(' '))
                temp = temp.Substring(0, e.GetArg("search").IndexOf(' '));

            if (rgx.IsMatch(temp))
            {
                index = System.Convert.ToInt32(temp.Substring(1));
                query = e.GetArg("search").Substring(e.GetArg("search").IndexOf(' ')).Replace("_", " ");
            }
            else
                query = e.GetArg("search"); //formats search to be engine friendly

            var listRequest = svc.Cse.List(query);

            //turns off safe mode if request is located in a chat with "nsfw" in title
            if (e.Channel.Name.ToUpper().Contains("NSFW"))
            {
                System.Console.WriteLine("NSFW");
                listRequest.Safe = CseResource.ListRequest.SafeEnum.Off;
            }
            else
                listRequest.Safe = CseResource.ListRequest.SafeEnum.High;

            //sets the reqest's engine
            listRequest.Cx = cx;

            //only allows gifs
            listRequest.FileType = "gif";

            //searches for images only
            listRequest.SearchType = CseResource.ListRequest.SearchTypeEnum.Image;

            if (index == 0)
                index = ran.Next(1, 100); //picks a random image from the top 100

            Console.WriteLine("radom index: " + index);

            listRequest.Start = index;
            var search = listRequest.Execute();

            WebClient wc = new WebClient(); //creaes webclient
            byte[] bytes = wc.DownloadData(search.Items[0].Link); //gets the iomage from the enginesearch
            MemoryStream ms = new MemoryStream(bytes); //converts image into a memoryStorage

            await e.Channel.SendMessage("[-" + index + " " + query + "]");
            await e.Channel.SendFile(".gif", ms); //send image as gif
        });
    }

    public void LogModule()
    {
        _client.UserJoined += (s, e) =>
        {
            if (e.Server != null)
                Log(e.Server, $"**[{DateTime.Now.ToString("h:mm:ss tt")}][{e.User.Name}] Joined the server");
        };

        _client.UserLeft += (s, e) =>
        {
            if (e.Server != null)
                Log(e.Server, $"**[{DateTime.Now.ToString("h:mm:ss tt")}][{e.User.Name}] Left the server");
        };

        _client.UserBanned += (s, e) =>
        {
            if (e.Server != null)
                Log(e.Server, $"**[{DateTime.Now.ToString("h:mm:ss tt")}][{e.User.Name}] Was banned from the server");
        };

        _client.UserUnbanned += (s, e) =>
        {
            if (e.Server != null)
                Log(e.Server, $"**[{DateTime.Now.ToString("h:mm:ss tt")}][{e.User.Name}] Was unbanned from the server");
        };

        _client.MessageReceived += (s, e) =>
        {
            if (e.Server != null)
                Log(e.Server, $"[{DateTime.Now.ToString("h:mm:ss tt")}][{e.Channel}] {e.User.Name}:{e.Message.Text}");
        };
    }

    public void Log(object sender, LogMessageEventArgs e)
    {
        Console.WriteLine($"[{e.Severity}] [{e.Source}] {e.Message}");
    }

    public void Log(Server s, string message)
    {
        string path = @"logs/" + s.ToString() + @"/" +DateTime.Now.Year.ToString() + @"/";
        //path = @"C:\Users\gilli\Desktop\Botathan\src\logs\" + s.ToString() + @"\" + DateTime.Now.Year.ToString() + @"\";
        Console.WriteLine(message);
        Directory.CreateDirectory(path);
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(path + DateTime.Now.ToString("MM-dd-yyyy") + @".txt", true))
        {
            file.WriteLine(message);
        }
    }

    public void GeneralModule()
    {
        cService.CreateCommand("ping")
        .Alias(new String[] { "ping" })
        .Description("Pings the bot")
            .Do(async e =>
            {
                await e.Channel.SendMessage("pong");
            });

        cService.CreateCommand("pastebin")
        .Alias(new String[] { "pastebin", "badcode" , "pastebin" })
        .Description("Sends the pastebin link")
        .Do(async e =>
        {
            await e.Channel.SendMessage("Nobody wants to see your bad code in chat, send a link via pastebin");
            await e.Channel.SendMessage("http://pastebin.com/");
        });

	    cService.CreateCommand("roulette")
        .Alias(new String[] { "roulette", "rt" })
        .Description("Play a game a roulette with yourself")
        .Do(async e =>
        {
            await e.Channel.SendMessage("_You place the revolver to your head_");
            System.Threading.Thread.Sleep(1500);

            if (ran.Next(1, 6) == 1)
                await e.Channel.SendMessage("**BANG**");
	        else
   	    	    await e.Channel.SendMessage("_click_");
        });

	    cService.CreateCommand("roll")
        .Alias(new String[] { "roll", "r" })
        .Description("Roll a dice")
	    .Parameter("index", ParameterType.Unparsed)
        .Do(async e =>
        {
            int index = 6;
            if (e.GetArg("index")!="")
                index = Int32.Parse(e.GetArg("index"));


            await e.Channel.SendMessage("Rolling a dice..");
            System.Threading.Thread.Sleep(1000);
            await e.Channel.SendMessage("It was a " + ran.Next(1, index));
        });

	    cService.CreateCommand("flip")
        .Alias(new String[] { "coin", "flip", "c", "f" })
        .Description("Sends the pastebin link")
        .Do(async e =>
        {
            await e.Channel.SendMessage("Flipping a coin..");
            System.Threading.Thread.Sleep(1000);
            if (ran.Next(1, 2) == 1)
                await e.Channel.SendMessage("Head");
	        else
		        await e.Channel.SendMessage("Tails");
        });
}

    public void ChattyModule()
    {
        _client.MessageReceived += async (s, e) =>
        {
            if (!e.Message.IsAuthor)
            {
                String text = e.Message.RawText.ToUpper().Replace(" ", "");
                if (text.Contains("RIPBOTATHAN"))
                    await e.Channel.SendMessage("Stop making fun of me, I'm doing the best I can :cry:");
                if (text.Contains("BOTATHAN2.0"))
                    await e.Channel.SendMessage("why are you talking about _him_");
                if (text.Contains("ILIKECAT"))
                    await e.Channel.SendMessage("Meow");
                if (text.Contains("BOTATHANSUCK"))
                    await e.Channel.SendMessage("You wish I could suck :wink:");
                if (text.Contains("YOUCANGIVEMEHEAD"))
                    await e.Channel.SendMessage("OwO");
            }
        };
    }

    public void AdminModule()
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
                Int32.TryParse(e.GetArg("amount"), out amountToDelete);

            Message[] messagesToDelete;
            messagesToDelete = await e.Channel.DownloadMessages((amountToDelete+1));

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
                await e.Server.Ban(e.Channel.FindUsers(e.GetArg("player")).ToArray()[0], 0);
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
}

    