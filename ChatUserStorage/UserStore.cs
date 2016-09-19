using System;
using System.Collections.Concurrent;
using System.Linq;
using SignalR.Controllers;


namespace SignalR.ChatUserStorage
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
            var freeUser = chatUserCache.Values.First(x => x.Pair == null && x.ConnectionID != connectionId);
            if (freeUser != null)
            {
                var currentUser = GetCurrentUser(connectionId);               
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
            chatUserCache.TryRemove(connectionid, out removedUser);          
        }

        private static string GetRandomName()
        {
            return "User_" + new Random().Next(1000, 9999);
        }
    }
}