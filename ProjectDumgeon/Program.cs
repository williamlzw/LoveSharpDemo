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
                WindowTitle = "横版卷轴地图",
                WindowCentered = true
            });
            Boot.Run(new TestDungeon());
        }
    }
}
