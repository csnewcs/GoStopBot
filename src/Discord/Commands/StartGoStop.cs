using System;
using System.Threading.Tasks;
using System.Threading;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
// using Discord

namespace GoStopBot.Commands
{
    public class StartGoStop
    {
        static public async Task StartGoStopCommand(SocketSlashCommand slashCommand)
        {
            var buttonBuilder = new ComponentBuilder().WithButton("참가하시려면 누르세요!", $"JOINGOSTOP_{slashCommand.Channel.Id}");
            await slashCommand.RespondAsync($"곧 게임이 시작됩니다. 아래의 버튼을 눌러 참가하세요!\n참가자: {slashCommand.User.Mention}", component: buttonBuilder.Build());
            await Task.Delay(10000);
            await slashCommand.ModifyOriginalResponseAsync(m => {m.Content = "테스트"; m.Components = new ComponentBuilder().WithButton("신청이 끝났어요!", "null", ButtonStyle.Danger).Build();});
        }   
    }
}