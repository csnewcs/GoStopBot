using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;

namespace GoStopBot
{
    public class StartBot
    {
        private readonly string _token;
        private DiscordSocketClient _client;
        private readonly MessageHandler _messageHandler;

        public StartBot()
        {
            JObject config = new JObject();
            try
            {
                config = JObject.Parse(File.ReadAllText("config.json"));
            }
            catch
            {
                config = makeConfig();
            }
            _token = config["token"].ToString();
            _messageHandler = new MessageHandler();
        }
        private JObject makeConfig()
        {
            JObject config = new JObject();
            Console.WriteLine("초기 설정을 시작합니다.\n봇의 토큰을 입력해 주세요.");
            config["token"] = Console.ReadLine();
            Console.WriteLine("초기 설정이 완료되었습니다.");
            File.WriteAllText("config.json", config.ToString());
            return config;
        }
        public async Task StartBotAsync()
        {
            _client = new DiscordSocketClient();
            _client.Log += log;
            _client.Ready += ready;
            _client.Connected += connected;
            _client.MessageReceived += _messageHandler.MessageRecieved;
            _client.InteractionCreated += _messageHandler.InteractionCreated;
            _client.ApplicationCommandCreated += slashCommandCreated;

            await _client.LoginAsync(TokenType.Bot, _token);
            await _client.StartAsync();
            
            await Task.Delay(-1);
        }


        private Task connected()
        {
            return Task.CompletedTask;
        }
        private async Task ready()
        {
            // Console.WriteLine("ready 들어옴");
            var testGuildId = File.ReadAllLines("TestGuild.txt")[0];

            var guild = _client.GetGuild(ulong.Parse(testGuildId));
            
            SlashCommandBuilder pingCommand = new SlashCommandBuilder().WithName("ping").WithDescription("Just ping");
            SlashCommandBuilder startCommand = new SlashCommandBuilder().WithName("start").WithDescription("고스톱을 시작합니다.");
            await guild.CreateApplicationCommandAsync(pingCommand.Build());
            await guild.CreateApplicationCommandAsync(startCommand.Build());
            // await _client.CreateGlobalApplicationCommandAsync(pingCommand.Build());

            // Console.WriteLine("SlashCommandBuilder 생성 완료");
            //var make = await 
            Console.WriteLine("커맨드 생성됨");
            // await log(new LogMessage(LogSeverity.Info, "", $"{make.Name} is made"));
            // return Task.CompletedTask;
        }
        private async Task slashCommandCreated(SocketApplicationCommand cmd)
        {
            Console.WriteLine(cmd.Name);
        }
        private Task log(LogMessage msg)
        {
            DateTime now = DateTime.Now;
            switch (msg.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{now.Year}-{now.Month}-{now.Day} {now.Hour}:{now.Minute}:{now.Second} [ERR] {msg.Message}\n{msg.Exception?.ToString()}");
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{now.Year}-{now.Month}-{now.Day} {now.Hour}:{now.Minute}:{now.Second} [WARN] {msg.Message}\n{msg.Exception?.ToString()}");
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"{now.Year}-{now.Month}-{now.Day} {now.Hour}:{now.Minute}:{now.Second} [INFO] {msg.Message}");
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"{now.Year}-{now.Month}-{now.Day} {now.Hour}:{now.Minute}:{now.Second} [{msg.Severity}] {msg.Message}");
                    break;
            }
            return Task.CompletedTask;
        }
    }
}