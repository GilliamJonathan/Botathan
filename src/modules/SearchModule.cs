
pubic class SearchModule
{
	//api key
	string apiKey = "AIzaSyC4znIVRxU53mYW5zBxZb48d0f6ccf4yJI";
	//creates search service
        var svc = new Google.Apis.Customsearch.v1.CustomsearchService(new BaseClientService.Initializer { ApiKey = apiKey });
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