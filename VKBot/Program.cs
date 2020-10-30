using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualBasic.CompilerServices;
using VKBot.Services;
using VkNet;
using VkNet.AudioBypassService.Extensions;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace VKBot
{
    class Program
    {
        // past this into browser
        // https://oauth.vk.com/authorize?client_id=7631190&scope=friends,wall,...&redirect_uri=https://oauth.vk.com/blank.html&response_type=token
        // the result will be in access_token param; insert your client_id instead of mine (duh)
        private static string _accessToken = "3368735b70790b24755d721f2683708f119619e19f37700273da083dc17e149bb35416a6b776fc20f7d94";
        private static int _option;

        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddAudioBypass();

            var api = new VkApi(services);
            api.Authorize(new ApiAuthParams { 
                ApplicationId = 7631190,
                AccessToken = _accessToken,
                Settings = Settings.Messages
            });

            Console.WriteLine("Press 1 to send requests and post on walls, 2 to only post on walls, 3 to put likes");
            string input = Console.ReadLine();
            Int32.TryParse( input, out _option);

            if (_option == 1)
            {
                // Send friend requests to supposed friends and people that post in groups for requests

                Friends service = new Friends();

                Console.WriteLine("Press ESC to stop");
                service.SendFriendRequests(api);
                service.FindFriendsInGroups(api);
            }

            if (_option <= 2)
            {
                // Post on walls for friend requests

                PostOnWalls service2 = new PostOnWalls();
                service2.PostOnWallsForFriends(api);
            }
            if (_option == 3) 
            {
                // Put likes on prof. pics of people in friend list

                Services.Likes service3 = new Services.Likes();
                service3.LikeProfPicAll(api);
            }
        }
    }
}
