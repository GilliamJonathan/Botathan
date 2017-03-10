using Discord;
using Discord.Audio;
using Discord.Commands;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

public class VoiceModule : ModuleBase
{

    public IAudioClient audioClient;
    [Command("Soundboard"), Summary("Sends a sound from the soundboard to you current voice channel")]
    public async Task Soundboard(string sound = "help")
    {
        if (sound == "help")
            await ReplyAsync("Please use '!help soundboard' command");

        var channel = ((IGuildUser)Context.User).VoiceChannel;

        if (channel == null)
        {
            await ReplyAsync("User must be in a voice channel.");
            return;
        }
        Console.WriteLine("Connecting to VoiceChannel");
        audioClient = await channel.ConnectAsync();
        Console.WriteLine("Connected to VoiceChannel");
        await SendAsync(audioClient, "C:/Users/gilli/Downloads/2SAD4ME.mp3");
        Console.WriteLine("Done");
    }

    [Command("debug")]
    public async Task Send()
    {
      await Context.ReplyAsync("Unable to debug at this time");
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
        Console.WriteLine("Creating stream...");
        // Create FFmpeg using the previous example
        var ffmpeg = CreateStream(path);
        Console.WriteLine("Creating output...");
        var output = ffmpeg.StandardOutput.BaseStream;
        Console.WriteLine("Setting up output settings for the IAudioClient...");
        var discord = client.CreatePCMStream(AudioApplication.Music,1920);
        Console.WriteLine("Outputing Audio...");
        await output.CopyToAsync(discord);
        Console.WriteLine("Waiting for audio to output...");
        await discord.FlushAsync();
    }
}
