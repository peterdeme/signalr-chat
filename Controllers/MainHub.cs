using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

using SignalR.ChatUserModel;


namespace SignalR.Controllers
{

    public class MainHub : Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.addToPage(name, message);            
        }

        public void SendMessage(string message)
        {           
           var currentuser = ChatUserStore.GetCurrentUser(Context.ConnectionId);
           if (currentuser.Pair != null)
             Clients.Client(currentuser.Pair.ConnectionID).addChatMessage(message);
           else
             Clients.Caller.showError("You have no pair.");
        }

        public void SetUserName(string name)
        {
            var currentUser = ChatUserStore.GetCurrentUser(Context.ConnectionId);
            if (currentUser.UserName == name) return;
            bool successful = ChatUserStore.SetUserNameForCurrentSession(name, Context.ConnectionId);            
            var pair = currentUser.Pair;
            if (pair != null && successful)
            {
                Clients.Client(pair.ConnectionID).partnerNameChanged(name);
            }
        }

        public void GetFreeUser()
        {
            ChatUser freeUser = ChatUserStore.GetFreeUserAndPairWithCurrent(Context.ConnectionId);
            if (freeUser != null)
            {
                Clients.Client(freeUser.ConnectionID).setNewPartner(freeUser.Pair.UserName);
                Clients.Caller.setNewPartner(freeUser.UserName);
            }
            else
                Clients.Caller.setNewPatner("");
        }

        public void AllConnections()
        {
            Clients.All.sendNumber((ChatUserStore.GetAllUsers().ToList().Count+1).ToString());
        }

        public override Task OnConnected()
        {
            AllConnections();
            ChatUserStore.AddToCache(Context.ConnectionId);         
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var disconnectedUser = ChatUserStore.GetCurrentUser(Context.ConnectionId);
            var disconnectedUsersPair = disconnectedUser.Pair;
            if (disconnectedUsersPair != null)
            {
                disconnectedUsersPair.Pair = null;                
                Clients.Client(disconnectedUsersPair.ConnectionID).addPartnerDisconnectedMessage("Your partner " + disconnectedUser.UserName + " has disconnected.");
            }
            ChatUserStore.RemoveFromCache(disconnectedUser.ConnectionID);
            AllConnections();                
            return base.OnDisconnected(stopCalled);
        }
    }
}