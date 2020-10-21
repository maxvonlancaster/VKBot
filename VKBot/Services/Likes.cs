using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using VkNet.Exception;
using VkNet.Utils;

namespace VKBot.Services
{
    public class Likes
    {
        private long _mineId = 537259490;
        private int _wait = 300;
        // implement service for liking all the posts by all the friends 
        public void LikeProfPicAll(VkApi api) 
        {
            int count = 550;
            List<string> friendPhotos = api.Friends.Get(new VkNet.Model.RequestParams.FriendsGetParams
            {
                UserId = _mineId,
                Fields = ProfileFields.PhotoId
            })
                .Select(i => i.PhotoId)
                .Where(p => p != null)
                .Distinct()
                .Skip(550)
                .ToList();

            Dictionary<long, long> friendPhotosSplit = new Dictionary<long, long>();
            foreach (var photo in friendPhotos) 
            {
                friendPhotosSplit.Add(long.Parse(photo.Split("_")[0]), long.Parse(photo.Split("_")[1]));
            }

            foreach (var elem in friendPhotosSplit) 
            {
                try
                {
                    api.Likes.Add(new VkNet.Model.RequestParams.LikesAddParams
                    {
                        Type = LikeObjectType.Photo,
                        OwnerId = elem.Key,
                        ItemId = elem.Value
                    });
                    count++;
                    Console.WriteLine("Sent like for {0}, number: {1}", elem.Key, count);
                    if (count % 30 == 0)
                    {
                        Thread.Sleep(1000 * _wait);
                    }
                }
                catch (VkApiMethodInvokeException ex) 
                {
                    Console.WriteLine("Cannot send request for {0}: {1}", elem.Key, ex.Message);
                }
            }
        }
    }
}
