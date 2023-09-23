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
                WindowWidth = 600,
                WindowHeight = 480,
                WindowTitle = "回合制横版战斗",
                WindowCentered = true
            });
            Boot.Run(new TestTurnbase());
        }
    }
}
