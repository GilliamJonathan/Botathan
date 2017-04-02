using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.Threading.Tasks;
using System.Reflection;
using System;
using System.IO;
using System.Threading;

class Botathan
{
    static void Main(string[] args) => new Botathan().Start().GetAwaiter().GetResult();

    private CommandService commands;
    private DiscordSocketClient client;
    private DependencyMap map;

    private char cmd;

    public async Task Start()
    {

        client = new DiscordSocketClient(new DiscordSocketConfig
        {
            LogLevel = Discord.LogSeverity.Verbose
        });

        commands = new CommandService();
        map = new DependencyMap();

        Thread t = new Thread(new ThreadStart(ConsoleCommand));
        t.IsBackground = true;
        t.Start();

        string token = "MjU1NDU1MTMxMDgyOTQ4NjA4.Cyeybw.0dR4xHZf2mQ6_fOPz0x4MxV4dLM";
        cmd = '$';

        if (true)
        {
            token = "Mjk4MTY1NTI0MDc0ODU2NDQ5.C8LczA.VZ0Lkw1-nzYdqrQz7IOUqYmOvwY";
            cmd = '/';
        }

        client.MessageReceived += async (message) =>
        {
            Log((((IGuildUser)message.Author).Guild.ToString()), $"[{DateTime.Now.ToString("h:mm:ss tt")}][{message.Channel.ToString()}] {message.Author.ToString()}:{message.Content.ToString()}");
            string text = message.Content.ToUpper().Replace(" ", "");
            if (text.Contains("RIPBOTATHAN"))
                await message.Channel.SendMessageAsync("Stop making fun of me, I'm doing the best I can :cry:");
            if (text.Contains("BOTATHAN2.0"))
                await message.Channel.SendMessageAsync("why are you talking about _him_");
            if (text.Contains("ILIKECAT"))
                await message.Channel.SendMessageAsync("Meow");
            if (text.Contains("BOTATHANSUCK"))
                await message.Channel.SendMessageAsync("You wish I could suck :wink:");
            if (text.Contains("YOUCANGIVEMEHEAD"))
                await message.Channel.SendMessageAsync("OwO");
        };

        client.Log += (message) =>
        {
            Console.WriteLine(message);
            return Task.CompletedTask;
        };

        await InstallCommands();

        await client.LoginAsync(TokenType.Bot, token);
        await client.StartAsync();

        await Task.Delay(-1);
    }

    public async Task InstallCommands()
    {
        // Hook the MessageReceived Event into our Command Handler
        client.MessageReceived += HandleCommand;
        // Discover all of the commands in this assembly and load them.
        await commands.AddModulesAsync(Assembly.GetEntryAssembly());
    }
    public async Task HandleCommand(SocketMessage messageParam)
    {
        // Don't process the command if it was a System Message
        var message = messageParam as SocketUserMessage;
        if (message == null) return;
        // Create a number to track where the prefix ends and the command begins
        int argPos = 0;
        // Determine if the message is a command, based on if it starts with cmd or a mention prefix
        if (!(message.HasCharPrefix(cmd, ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos))) return;
        // Create a Command Context
        var context = new CommandContext(client, message);
        // Execute the command. (result does not indicate a return value,
        // rather an object stating if the command executed succesfully)
        var result = await commands.ExecuteAsync(context, argPos, map);
        if (!result.IsSuccess)
            await context.Channel.SendMessageAsync(result.ErrorReason);
    }

    public void Log(string server, string message)
    {
        string path = @"logs/" + server + @"/" + DateTime.Now.Year.ToString() + @"/";
        if(cmd == '/')
        {
            path = @"C:\Users\gilli\Desktop\Botathan\src\logs\" + server + @"\" + DateTime.Now.Year.ToString() + @"\";
        }

        Console.WriteLine(message);
        Directory.CreateDirectory(path);
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(path + DateTime.Now.ToString("MM-dd-yyyy") + @".txt", true))
        {
            file.WriteLine(message);
        }
    }

    public async void ConsoleCommand()
    {
        while (true)
        {
                string input = Console.ReadLine();
                if (input == "stop")
                {
                    await client.StopAsync();
                    Environment.Exit(1);
                }
                else if (input.IndexOf("say") == 0)
                {
                    string[] chopedin = input.Split(null);
                    var guilds = client.Guilds;
                    foreach(var guild in guilds)
                    {
                        if(guild.ToString() == chopedin[1].Replace("_"," "))
                        {
                            var channels = guild.Channels;
                            foreach(var channel in channels)
                            {
                                if (channel.ToString() == chopedin[2])
                                {
                                    IMessageChannel IMC = (IMessageChannel)channel;
                                    await IMC.SendMessageAsync(input.Substring(input.IndexOf(chopedin[3])));
                                    goto end;
                                }
                            }
                            foreach (IMessageChannel IMC in channels)
                            {
                                await IMC.SendMessageAsync(input.Substring(input.IndexOf(chopedin[2])));
                                goto end;
                            }
                            goto end;
                        }
                    }
                }
                
        end:;
        }
    }

}
