using Love;
using System;
using Love.Awesome;
using ProjectTiledMap;

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
                WindowTitle = "测试TiledMap",
                WindowCentered = true
            });
            Boot.Run(new TestTiledMap());
        }
    }
}
