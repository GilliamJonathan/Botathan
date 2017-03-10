using Discord;
using Discord.Audio;
using Discord.Commands;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

public class VoiceModule : ModuleBase
{
    /*
    public IAudioClient audioClient;
    [Command("Soundboard"), Summary("Sends a sound from the soundboard to you current voice channel")]
    public async Task Soundboard(string sound = "help")
    {
        if (sound == "help")
            await ReplyAsync("Airhorn");

        var channel = ((IGuildUser)Context.User).VoiceChannel;

        if (channel == null)
        {
            await ReplyAsync("User must be in a voice channel.");
            return;
        }
        Console.WriteLine("1");
        audioClient = await channel.ConnectAsync();
        Console.WriteLine("2");
        await SendAsync(audioClient, "C:/Users/gilli/Downloads/2SAD4ME.mp3");

    }

    [Command("send")]
    public async Task Send()
    {

        await SendAsync(audioClient, "C:/Users/gilli/Downloads/2SAD4ME.mp3");
    }

    private Process CreateStream(string path)
    {
        var ffmpeg = new ProcessStartInfo
        {
            FileName = "ffmpeg",
            Arguments = $"-i {path} -ac 2 -f s16le -ar 48000 pipe:1",
            UseShellExecute = false,
            RedirectStandardOutput = true,
        };
        return Process.Start(ffmpeg);
    }

    private async Task SendAsync(IAudioClient client, string path)
    {
        Console.WriteLine("3");
        // Create FFmpeg using the previous example
        var ffmpeg = CreateStream(path);
        Console.WriteLine("4");
        var output = ffmpeg.StandardOutput.BaseStream;
        Console.WriteLine("5");
        var discord = client.CreatePCMStream(AudioApplication.Music,1920);
        Console.WriteLine("6");
        await output.CopyToAsync(discord);
        Console.WriteLine("7");
        await discord.FlushAsync();
    }
    */
}