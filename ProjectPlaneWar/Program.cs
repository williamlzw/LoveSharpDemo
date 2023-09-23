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
                WindowHeight = 800,
                WindowTitle = "飞机大战",
                WindowCentered = true
            });
            Boot.Run(new TestPlaneWar());
        }
    }
}
