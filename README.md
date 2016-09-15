# signalr-chat
A chat implementation of SignalR.
##Technologies
.NET MVC, JavaScript 
##Libs
jQuery, SignalR, Bootstrap, Moment + Livemin

##Features
Users a stored in a static thread-safe dictionary (ConcurrentDictionary). Whenever you login, you'll get a random pair. Basicly, it works the same way as ChatRoulette. Server-client connection implemented with SignalR. See scripts/chat.js, server endpoint is Controllers/MainHub.cs.

![alt tag](http://i.imgur.com/L8VOrF5.jpg)

