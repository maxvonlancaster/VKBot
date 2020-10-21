using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using VkNet;
using VkNet.Model.RequestParams;

namespace VKBot.Services
{
    public class SendMessageToConf
    {
        private List<long> _ids = new List<long>() { 56, 52, 51, 50, 49, 59, 62, 61, 53};
        private List<string> _text = new List<string>();
        public void PostElephantToConf(VkApi api)
        {
            foreach (string line in File.ReadAllLines("../../../../scenario.txt")) 
            {
                _text.Add(line);
            }

            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            {
                foreach (var id in _ids)
                {
                    Random random = new Random();
                    int messageNumber = random.Next(0, _text.Count);
                    string message = _text[messageNumber];

                    var post = api.Messages.Send(new MessagesSendParams { 
                        ChatId = id,
                        Message = message,
                        RandomId = messageNumber
                    });

                    Console.WriteLine(post);
                    Thread.Sleep(1000 * 3);
                }

                // do something
            }
        }
    }
}
