using Microsoft.Owin;
using Owin;

using SignalR.Controllers;
using SignalR.ChatUserModel;

[assembly: OwinStartup(typeof(SignalR.Startup))]

namespace SignalR
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Logger.Log("starting app");
            app.MapSignalR();
            ChatUserStore.Initialize();                    
        }
    }
}
