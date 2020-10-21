using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Exception;

namespace VKBot.Services
{
    public class Friends
    {
        private int _requestWait = 4;
        private int _requestsSent = 0;

        private List<long?> _groupIds = new List<long?>()
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
            -33764742,
            -94946045,
            -59721672,
            -39130136,
            -46258034,
            -47350356,
            -72686157,
            -68486191,
            -74738426
        };

        public void SendFriendRequests(VkApi api)
        {
            var suggestions = api.Friends.GetSuggestions(count: 500).ToList().Distinct().ToList();
            foreach (var friend in suggestions)
            {
                var user = api.Users.Get(new long[] { friend.Id }, VkNet.Enums.Filters.ProfileFields.Counters)
                    .FirstOrDefault();

                if (user.Counters != null /*&& user.Counters.MutualFriends > 20*/
                    && user.Counters.Followers < 40
                    && user.Counters.Friends > 250
                    && user.FriendStatus == VkNet.Enums.FriendStatus.NotFriend)
                {
                    try
                    {
                        api.Friends.Add(user.Id, null, null);
                        Console.WriteLine("Sent request for {0}, number: {1}", user.Id, _requestsSent);
                        _requestsSent++;
                        Thread.Sleep(1000 * _requestWait);
                    }
                    catch (VkApiMethodInvokeException ex)
                    {
                        Console.WriteLine("Cannot send request for {0}: {1}", user.Id, ex.Message);
                    }
                }
            }
        }

        public void FindFriendsInGroups(VkApi api)
        {
            List<long?> suggestUsers = new List<long?>();
            foreach (var id in _groupIds)
            {
                suggestUsers.AddRange(api.Wall.Get(new VkNet.Model.RequestParams.WallGetParams
                {
                    OwnerId = id,
                    Count = 100
                })
                    .WallPosts
                    .Select(i => i.FromId)
                    );
            }
            List<long> usersNotNull = suggestUsers.Where(i => i != null)
                .Select(i => i ?? 0)
                .ToList()
                .Distinct()
                .ToList();

            foreach (var id in usersNotNull)
            {
                if (id > 0)
                {
                    var user = api.Users.Get(new long[] { id }, VkNet.Enums.Filters.ProfileFields.Counters)
                        .FirstOrDefault();

                    if (user.Counters != null /*&& user.Counters.MutualFriends > 20*/
                        && user.Counters.Followers < 40
                        && user.Counters.Friends > 250
                        && user.FriendStatus == VkNet.Enums.FriendStatus.NotFriend)
                    {
                        try
                        {
                            api.Friends.Add(user.Id, null, null);
                            Console.WriteLine("Sent request for {0}, number: {1}", user.Id, _requestsSent);
                            _requestsSent++;
                            Thread.Sleep(1000 * _requestWait);
                        }
                        catch (VkApiMethodInvokeException ex)
                        {
                            Console.WriteLine("Cannot send request for {0}: {1}", user.Id, ex.Message);
                        }
                    }
                }
            }
        }
    }
}
