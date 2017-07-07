using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Discord;
using Discord.WebSocket;
using Adminthulhu;

/*
 * This is a small frankensteination of a bot, with a single feature
 * of the full Adminthulhu bot ripped out and put into an otherwise
 * empty shell. Don't expect good code, since I am incredibly tired
 * right now and can't be arsed to think about performance.
*/

namespace ResponseBot
{
    public class Program {

        public static DiscordSocketClient discordClient;
        public static string dataPath = "";
        public static string gitHubIgnoreType = ".botproperty";

        public List<Phrase> phrases;

        public static void Main(string [ ] args) => new Program ().Start (args).GetAwaiter ().GetResult ();


        public async Task Start(string [ ] args) {

            discordClient = new DiscordSocketClient ();
            dataPath = AppContext.BaseDirectory + "/data/";
            Directory.CreateDirectory (dataPath);

            LoadPhrases ();

            string token = SerializationIO.LoadTextFile (dataPath + "bottoken" + gitHubIgnoreType) [ 0 ];

            discordClient.MessageReceived += (e) => {
                if (e.Content == "!reload" && (e.Author as SocketGuildUser).GuildPermissions.Administrator) {
                    e.Channel.SendMessageAsync ("Reloading phrases from file..");
                    LoadPhrases ();
                    e.Channel.SendMessageAsync ("Reload succesful!");
                }

                FindPhraseAndRespond (e);
                return Task.CompletedTask;
            };

            discordClient.Ready += () => {
                Console.WriteLine ("Bot is ready and running!");
                return Task.CompletedTask;
            };

            Console.WriteLine ("Connecting to Discord..");
            await discordClient.LoginAsync (TokenType.Bot, token);
            await discordClient.StartAsync ();

            await Task.Delay (-1);
        }

        private void LoadPhrases() {
            phrases = SerializationIO.LoadObjectFromFile<List<Phrase>> (dataPath + "phrases.json");
            if (phrases == null)
                phrases = new List<Phrase> ();
        }

        public void FindPhraseAndRespond(SocketMessage e) {
            for (int i = 0; i < phrases.Count; i++) {
                if (phrases [ i ].CheckAndRespond (e))
                    return;
            }
        }
    }
}
