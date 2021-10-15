using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
// using Discord

namespace GoStopBot.Commands
{
    public class StartGoStop
    {
        static Dictionary<ulong, List<ulong>> joining = new Dictionary<ulong, List<ulong>>();
        static public async Task StartGoStopCommand(SocketSlashCommand slashCommand)
        {
            var buttonBuilder = new ComponentBuilder().WithButton("참가하시려면 누르세요!", $"JOINGOSTOP_{slashCommand.Channel.Id}");
            await slashCommand.RespondAsync($"곧 게임이 시작됩니다. 아래의 버튼을 눌러 참가하세요!\n참가자: {slashCommand.User.Mention}", component: buttonBuilder.Build());
            joining.Add(slashCommand.Channel.Id, new List<ulong>() { slashCommand.User.Id });
            await Task.Delay(10000);
            await slashCommand.ModifyOriginalResponseAsync(m => {m.Components = new ComponentBuilder().WithButton("신청이 끝났어요!", "null", ButtonStyle.Danger).Build();});
        }
        static public async Task JoinGoStopCommand(SocketMessageComponent component)
        {
            if(!joining[component.Channel.Id].Contains(component.User.Id)) joining[component.Channel.Id].Add(component.User.Id);
            string mentions = $"<@{joining[component.Channel.Id][0]}>";
            for(int i = 1; i < joining[component.Channel.Id].Count; i++)
            {
                mentions += $", <@{joining[component.Channel.Id][i]}>";
            }
            await component.UpdateAsync(m => { m.Content = $"곧 게임이 시작됩니다. 아래의 버튼을 눌러 참가하세요!\n참가자: {mentions}"; });
            // await component.
        }
    }
}