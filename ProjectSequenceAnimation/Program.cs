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
                WindowTitle = "序列动画",
                WindowCentered = true
            });
            Boot.Run(new TestSequenceAnimation());
        }
    }
}
