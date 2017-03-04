public class LogModule{

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
           	if(e.Server != null)
               		Log(e.Server, $"[{DateTime.Now.ToString("h:mm:ss tt")}][{e.Channel}] {e.User.Name}:{e.Message.Text}");
        };

	public void Log(object sender, LogMessageEventArgs e)
	{
		Console.WriteLine($"[{e.Severity}] [{e.Source}] {e.Message}");
	}

	public void Log(Server s, String message)
	{
        	String path = @"logs/" + s.ToString() + @“/“ + DateTime.Now.Year.ToString() + @“/“;
        	//path = @"H:\Botathan\logs\" + s.ToString() + @"\" + DateTime.Now.Year.ToString() + @"\";
        	Console.WriteLine(message);
        	Directory.CreateDirectory(path);
        	using (System.IO.StreamWriter file = new System.IO.StreamWriter(path + DateTime.Now.ToString("MM-dd-yyyy") + @".txt", true))
        	{
        	file.WriteLine(message);
        	}
    	}
}

