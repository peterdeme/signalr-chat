using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalR.Controllers;


namespace SignalR.ChatUserModel
{

   public class ChatUser
    {
        public ChatUser(string username, bool isoccupied, string connectionid)
        {
            this.UserName = username;
            this.IsOccupied = isoccupied;
            this.ConnectionID = connectionid;
        }
        public string UserName { get; set; }
        public bool IsOccupied { get; set; }
        public string ConnectionID { get; set;}
        public ChatUser Pair { get; set; }
    }

    public static class ChatUserStore
    {
        static ConcurrentDictionary<string, ChatUser> chatUserCache;         

        public static void Initialize()
        {
            chatUserCache = new ConcurrentDictionary<string, ChatUser>();           
        }

        public static void AddToCache(ChatUser user)
        {
            if (!chatUserCache.Keys.Where(x => x == user.ConnectionID).Any())
                chatUserCache.TryAdd(user.ConnectionID, user);
        }

        public static void AddToCache(string connectionId)
        {
            if (!chatUserCache.Keys.Where(x => x == connectionId).Any())
            {
                chatUserCache.TryAdd(connectionId, new ChatUser(GetRandomName(), false, connectionId));
            }
        }

        public static ConcurrentDictionary<string, ChatUser> GetAllUsers()
        {
            return chatUserCache;
        }

        public static ChatUser GetFreeUserAndPairWithCurrent(string connectionId)
        {
            var freeUser = chatUserCache.Values.Where(x => x.Pair == null && x.ConnectionID != connectionId).First();
            if (freeUser != null)
            {
                var currentUser = GetCurrentUser(connectionId);
                Logger.Log("Paired current user: " + currentUser.ConnectionID + " with a free user:" + freeUser.ConnectionID);
                currentUser.Pair = freeUser;
                freeUser.Pair = currentUser;
                return freeUser;
            }
            return null;
        }

        public static bool SetUserNameForCurrentSession(string username, string connectionId)
        {
            var currentUser = GetCurrentUser(connectionId);
            if (currentUser != null)
            {
                currentUser.UserName = username;
                return true;
            }
            return false;
                
        }

        public static ChatUser GetCurrentUser(string connectionid)
        {
            return chatUserCache.Values.Where(x => x.ConnectionID == connectionid).FirstOrDefault();
        }

        public static void RemoveFromCache(string connectionid)
        {
            ChatUser removedUser;
           // chatUserCache.Remove
            chatUserCache.TryRemove(connectionid, out removedUser);
            removedUser = null; //dispose
        }

        private static string GetRandomName()
        {
            return "User_" + new Random().Next(1000, 9999);
        }
    }
}