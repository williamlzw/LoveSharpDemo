using Love;
using Love.Awesome;
using System;


namespace TestProjectCore
{
    public class Program
    {
        static void Main(string[] args)
        {
            Boot.Init(new BootConfig
            {
                WindowWidth = 800,
                WindowHeight = 600,
                WindowTitle = "休闲游戏背景",
                WindowCentered = true
            });
            Boot.Run(new TestCasualGames());
        }
    }
}
