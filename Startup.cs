using Microsoft.Owin;
using Owin;

using SignalR.Controllers;
using SignalR.ChatUserStorage;

[assembly: OwinStartup(typeof(SignalR.Startup))]

namespace SignalR
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ChatUserStore.Initialize();                    
        }
    }
}
