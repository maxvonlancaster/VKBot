using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using VkNet;
using VkNet.Exception;
using VkNet.Model.RequestParams;

namespace VKBot.Services
{
    public class PostOnWalls
    {
        private List<long?> _ids = new List<long?>() 
        { 
            -8337923, 
            -46258034, 
            -11270102, 
            -61413825, 
            -148406719, 
            -153162012, 
            -24261502, 
            -131166480, 
            -51445749, 
            -43606620,
            -132909030,
            -34985835
        };
        private List<string> _text = new List<string>();
        private int _requestWait = 6;

        private string _message = "Го в друзья)))";
        public void PostOnWallsForFriends(VkApi api) 
        {
            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
            {
                foreach (var id in _ids)
                {
                    try
                    {
                        var post = api.Wall.Post(new WallPostParams
                        {
                            OwnerId = id,
                            Message = _message
                        });

                        Console.WriteLine(post);
                        Thread.Sleep(1000 * _requestWait);
                    }
                    catch (VkApiMethodInvokeException ex) 
                    {
                        Console.WriteLine("Exception: {0} for group {1}", ex.Message, id);
                    }
                }

                // do something
            }
        }

        public void PostOnWallsElephant(VkApi api)
        {
            foreach (string line in File.ReadAllLines("../../../../scenario2.txt", Encoding.Default))
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

                    try
                    {
                        var post = api.Wall.Post(new WallPostParams
                        {
                            OwnerId = id,
                            Message = message
                        });

                        Console.WriteLine(post);
                        Thread.Sleep(1000 * _requestWait);
                    }
                    catch (VkApiMethodInvokeException ex)
                    {
                        Console.WriteLine("Exception: {0} for group {1}", ex.Message, id);
                    }
                }
            }
        }
    }
}
