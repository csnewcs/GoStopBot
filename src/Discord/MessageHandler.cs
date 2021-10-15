using System;
using System.Threading;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using GoStopBot.Commands;

namespace GoStopBot
{
    public class MessageHandler
    {

        public async Task MessageRecieved(SocketMessage msg)
        {

        }
        public async Task InteractionCreated(SocketInteraction interaction)
        {
            //타입 구별
            switch (interaction.Type)
            {
                case InteractionType.ApplicationCommand:
                    await SlashCommand(interaction as SocketSlashCommand);
                    break;
                case InteractionType.MessageComponent:
                    await ButtonClicked(interaction as SocketMessageComponent);
                    break;
            }
        }
        private async Task SlashCommand(SocketSlashCommand command)
        {
            switch (command.Data.Name)
            {
                case "ping":
                    Thread pingThread = new Thread(async () => {await Ping.PingCommand(command);});
                    pingThread.Start();
                    // await Ping.PingCommand(command);
                    break;
                case "start":
                    Thread startThread = new Thread(async () => {await StartGoStop.StartGoStopCommand(command);});
                    startThread.Start();
                    // await StartGoStop.StartGoStopCommand(command);
                    break;
            }
        }
        private async Task ButtonClicked(SocketMessageComponent component)
        {
            string id = component.Data.CustomId;
            if(id.Contains("JOINGOSTOP"))
            {
                await StartGoStop.JoinGoStopCommand(component);
            }
        }
    }
}