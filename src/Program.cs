using System;

namespace GoStopBot
{
    class Program
    {
        static void Main(string[] args)
        {
            new StartBot().StartBotAsync().GetAwaiter().GetResult();
        }
    }
}
