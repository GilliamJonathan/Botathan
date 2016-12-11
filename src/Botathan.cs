using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.IO;
using Google.Apis.Customsearch.v1;
using Google.Apis.Customsearch.v1.Data;
using Google.Apis.Services;


class Botathan
{
    static void Main(string[] args)
    {
        new Botathan().Start();
    }
    private Random ran;
    private string apiKey = "AIzaSyC4znIVRxU53mYW5zBxZb48d0f6ccf4yJI";
    private string cx = "017085880482068702817:mkkqkl2yybo"; //Custom search engine id
    private string query = "error"; //Default search
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
            x.PrefixChar = '$';
            x.AllowMentionPrefix = false; //doesn't require @ mention for commands
            x.HelpMode = HelpMode.Private; //doesn't send help message in chat
        });

        CreateCommands();
        ChatLogger();

        _client.MessageReceived += async (s, e) =>
        {
            if (!e.Message.IsAuthor)
            {
                if (e.Message.RawText.ToUpper().Replace(" ", "").Replace("S", "").Equals("DICK"))
                    await e.Channel.SendMessage("Stop trying to summon Jake");
            }

        };
        
        _client.ExecuteAndWait(async () =>
        {
            //dev
            //await _client.Connect("MjU3MTk3MDQ0MTQ4NjAwODMy.Cy3M8g.yCb63W0qf6LxzzDq_0dwTBqq4Y4", TokenType.Bot);

            //release
            await _client.Connect("MjU1NDU1MTMxMDgyOTQ4NjA4.Cyeybw.0dR4xHZf2mQ6_fOPz0x4MxV4dLM", TokenType.Bot);
        });
    }

    public void CreateCommands()
    {
        //creates search service
        var svc = new Google.Apis.Customsearch.v1.CustomsearchService(new BaseClientService.Initializer { ApiKey = apiKey });
        
        //creates command service
        var cService = _client.GetService<CommandService>();

        cService.CreateCommand("Ping")
        .Alias(new String[] { "ping" })
        .Description("Pings the bot")
        .Do(async e =>
        {
            await e.Channel.SendMessage("pong");
        });

        cService.CreateCommand("Image Search")
        .Alias(new String[] { "search", "send", "searchfor" })
        .Description("Sends a photo")
        .Parameter("search", ParameterType.Unparsed)
        .Do(async e =>
        {
            query = e.GetArg("search").Replace("_", " "); //formats search to be engine friendly

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

            listRequest.Start = ran.Next(0, 100); //picks a random image from the top 100
            var search = listRequest.Execute();

            WebClient wc = new WebClient(); //creaes webclient
            byte[] bytes = wc.DownloadData(search.Items[0].Link); //gets the iomage from the enginesearch
            MemoryStream ms = new MemoryStream(bytes); //converts image into a memoryStorage

            await e.Channel.SendFile(".jpg", ms); //send image as jpeg
        });

        cService.CreateCommand("Gif Search")
        .Alias(new String[] { "gif", "sendgif", "searchforgif" })
        .Description("Sends a gif")
        .Parameter("search", ParameterType.Unparsed)
        .Do(async e =>
        {
            query = e.GetArg("search").Replace("_", " "); //formats search to be engine friendly

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

            listRequest.Start = ran.Next(0, 100); //picks a random image from the top 100
            var search = listRequest.Execute();

            WebClient wc = new WebClient(); //creaes webclient
            byte[] bytes = wc.DownloadData(search.Items[0].Link); //gets the iomage from the enginesearch
            MemoryStream ms = new MemoryStream(bytes); //converts image into a memoryStorage

            await e.Channel.SendFile(".gif", ms); //send image as gif
                
        });
    }

    public void Log(object sender, LogMessageEventArgs e)
    {
        Console.WriteLine($"[{e.Severity}] [{e.Source}] {e.Message}");
    }

    public void Log(Server s, String message)
    {

        String path;
        path = @"Desktop/Botathan/logs" + s.ToString() + @"\" + DateTime.Now.Year.ToString() + @"\";
        //path = @"G:\Botathan\logs\" + s.ToString() + @"\" + DateTime.Now.Year.ToString() + @"\";
        Console.WriteLine(message);
        Directory.CreateDirectory(path);
        using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(path + DateTime.Now.ToString("MM-dd-yyyy") + @".txt", true))
        {
            file.WriteLine(message);
        }
        
    }

    public void ChatLogger()
    {
        _client.UserJoined += (s, e) =>
        {
            Log(e.Server, $"**[{DateTime.Now.ToString("h:mm:ss tt")}][{e.User.Name}] Joined the server");
        };

        _client.UserLeft += (s, e) =>
        {
            Log(e.Server, $"**[{DateTime.Now.ToString("h:mm:ss tt")}][{e.User.Name}] Left the server");
        };

        _client.UserBanned += (s, e) =>
        {
            Log(e.Server, $"**[{DateTime.Now.ToString("h:mm:ss tt")}][{e.User.Name}] Was banned from the server");
        };

        _client.UserUnbanned += (s, e) =>
        {
            Log(e.Server, $"**[{DateTime.Now.ToString("h:mm:ss tt")}][{e.User.Name}] Was unbanned from the server");
        };

        _client.MessageReceived += (s, e) =>
        {
            Log(e.Server, $"[{DateTime.Now.ToString("h:mm:ss tt")}][{e.Channel}] {e.User.Name}:{e.Message.Text}");
        };

        //Console.WriteLine($"[{e.Severity}] [{e.Source}] {e.Message}");
    }
}
