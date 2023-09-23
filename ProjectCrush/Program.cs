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
                WindowWidth = 640,
                WindowHeight = 480,
                WindowTitle = "木桶破碎物理效果",
                WindowCentered = true
            });
            Boot.Run(new TestCrush());
        }
    }
}
