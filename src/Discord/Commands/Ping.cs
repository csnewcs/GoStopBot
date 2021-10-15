using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace GoStopBot.Commands
{
    public class Ping
    {
        static public async Task PingCommand(SocketSlashCommand slashCommand)
        {
            DateTime now = DateTime.Now;
            await slashCommand.RespondAsync($"지연 시간: {(now - slashCommand.CreatedAt).TotalMilliseconds}ms");
        }   
    }
}