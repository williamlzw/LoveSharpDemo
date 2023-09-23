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
                WindowWidth = 1200,
                WindowHeight = 800,
                WindowTitle = "测试IMGUI",
                WindowCentered = true
            });
            Boot.Run(new TestImGui());
        }
    }
}
