using Discord.Commands;
using Google.Apis.Customsearch.v1;
using Google.Apis.Services;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class SearchModule : ModuleBase
{
    //api key
    static string apiKey = "AIzaSyC4znIVRxU53mYW5zBxZb48d0f6ccf4yJI";
    //creates search service
    CustomsearchService svc = new Google.Apis.Customsearch.v1.CustomsearchService(new BaseClientService.Initializer { ApiKey = apiKey });
    //Custom searching engine id
    string cx = "017085880482068702817:mkkqkl2yybo";

    [Command("image"), Summary("Sends a random image from google")]
    public async Task Image([Remainder, Summary("What to search for")] string query = "error")
    {
        Random ran = new Random();
        int index = 0;
        Regex rgx = new Regex(@"(-\d{1,3})");
        string temp = query;

        if (temp.Contains(" "))
            temp = temp.Substring(0, query.IndexOf(' '));

        if (rgx.IsMatch(temp))
        {
            index = System.Convert.ToInt32(temp.Substring(1));
            query = query.Substring(query.IndexOf(' ')).Replace("_", " ");
        }

        var listRequest = svc.Cse.List(query);

        //turns off safe mode if request is located in a chat with "nsfw" in title
        if (Context.Channel.ToString().Contains("NSFW"))
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

        await ReplyAsync("[-" + index + " " + query + "]");
        await Context.Channel.SendFileAsync(ms,".jpg"); //send image as jpeg
    }

    [Command("gif"), Summary("Sends a random image from google")]
    public async Task Gif([Remainder, Summary("What to search for")] string query = "error")
    {
        Random ran = new Random();
        int index = 0;
        Regex rgx = new Regex(@"(-\d{1,3})");
        string temp = query;

        if (temp.Contains(" "))
            temp = temp.Substring(0, query.IndexOf(' '));

        if (rgx.IsMatch(temp))
        {
            index = System.Convert.ToInt32(temp.Substring(1));
            query = query.Substring(query.IndexOf(' ')).Replace("_", " ");
        }

        var listRequest = svc.Cse.List(query);

        //turns off safe mode if request is located in a chat with "nsfw" in title
        if (Context.Channel.ToString().Contains("NSFW"))
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

        await ReplyAsync("[-" + index + " " + query + "]");
        await Context.Channel.SendFileAsync(ms, ".gif"); //send image as jpeg
    }
}

