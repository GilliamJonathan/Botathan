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
using System.Text.RegularExpressions;

class Botathan
{
    static void Main(string[] args)
    {
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
            x.PrefixChar = '$';
            x.AllowMentionPrefix = false; //don't require @ mention for commands
            x.HelpMode = HelpMode.Private; //don't send help message in chat
        });
        
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

        //creates command service
        var cService = _client.GetService<CommandService>();

    } 

    