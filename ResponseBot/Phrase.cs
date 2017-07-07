﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Adminthulhu {
    public class Phrase {

        public string inputText;
        public ulong user;
        public int chance;
        public string response;
        public ulong channel;

        public bool CheckAndRespond(SocketMessage e) {

            string message = e.Content;
            ulong sender = e.Author.Id;
            ulong locChannel = e.Channel.Id;

            if (message.Length < inputText.Length)
                return false;

            if (inputText == "" || message.Substring (0, inputText.Length).ToUpper () == inputText.ToUpper ()) {
                if (user == 0 || sender == user) {
                    if (channel == 0 || locChannel == channel) {

                        Random random = new Random ();
                        if (random.Next (100) < chance || chance == 100) {

                            e.Channel.SendMessageAsync (response);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public Phrase(string _input, ulong _user, int _chance, string _response, ulong _channel) {
            inputText = _input;
            user = _user;
            chance = _chance;
            response = _response;
            channel = _channel;
        }
    }
}
