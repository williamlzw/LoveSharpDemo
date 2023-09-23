using Love;
using Love.Awesome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProjectCore
{
    public class PhysicalInfo
    {
        public float speedX;
        public float speedY;
        public float speedGravity;
        public float angle;
        public float rotationValue;
        public float fallPointX;
        public float fallPointY;
    }

    public class CaskFragment
    {
        public ClassImage image;
        public PhysicalInfo info;
    }

    /// <summary>
    /// 爆炸效果
    /// </summary>
    public class TestCrush : Scene
    {
        List<CaskFragment> _fragment;
        /// <summary>
        /// 爆炸位置
        /// </summary>
        float _explosionPosition = 300;
        ClassFont _font;

        public override void Load()
        {
            _font = new ClassFont("HYC1GJM.ttf", 20);
            _fragment = new List<CaskFragment>();
            foreach(var index in Enumerable.Range(0, 12))
            {
                _fragment.Add(new CaskFragment
                {
                    image = new ClassImage($"crush/{index + 1}.png"),
                });
                _fragment[index].image.SetOrgin(_fragment[index].image.Width / 2, _fragment[index].image.Height / 2);
                _fragment[index].info = new PhysicalInfo();
            }
            
        }

        public override void Update(float dt)
        {
            if(Keyboard.IsPressed(KeyConstant.Space))
            {
                var rt = new Random();
                foreach(var index in _fragment)
                {
                    //这里-5和5的意思是往左和往右的位置
                    index.info.speedX = rt.Next(-5, 5);
                    //这里简单说下 两个值都为负 就是先往上升
                    index.info.speedY = rt.Next(-12, -8);
                    index.info.speedGravity = (float)rt.Next(5, 9) / 10;
                    index.info.rotationValue = (float)rt.Next(-3, 3) / 10;
                    index.info.fallPointX = 400;
                    index.info.fallPointY = 250;
                }
            }

            foreach (var index in _fragment)
            {
                // 木桶碎片【下降】判断
                if (index.info.speedY >=0)
                {
                    index.info.speedY += index.info.speedGravity;
                    index.info.fallPointY += index.info.speedY;
                    //这里是判断是否到达边界 爆炸位置 就是抵达的边界
                    if(index.info.fallPointY >= _explosionPosition)
                    {
                        index.info.fallPointY = _explosionPosition;
                        if(index.info.speedX !=0)
                        {
                            //这里是停止碎片继续移动
                            index.info.speedX = 0;
                            index.info.speedY = 0;
                            index.info.speedGravity = 0;
                            index.info.angle += index.info.rotationValue;
                        }
                    }
                    else
                    {
                        //这里是旋转功能
                        index.info.angle += index.info.rotationValue;
                    }
                }
                else
                {
                    //木桶碎片【弹起】判断
                    //由于 木桶碎片纵方向速度 初始值是负数 所以会先判断这里
                    index.info.speedY += index.info.speedGravity;
                    if(index.info.speedY >= 0)
                    {
                        index.info.speedY = 0;
                    }
                    else
                    {
                        index.info.fallPointY += index.info.speedY;
                        index.info.angle += index.info.rotationValue;
                    }
                }
                //木桶碎片横移动 初始值是负数的向左 初始值是正数的向右
                index.info.fallPointX += index.info.speedX;
            }
        }

        public override void Draw()
        {
            Graphics.Clear(Color.Gray);
            foreach (var index in _fragment)
            {
                if(index.info.fallPointX>0)
                {
                    index.image.DrawEx(index.info.fallPointX, index.info.fallPointY, 1, 1, index.info.angle);
                }
            }
            Graphics.Line(0, _explosionPosition, 800, _explosionPosition);
            _font.Draw("按下空格键,演示效果", 10, 10, new Vector4(255, 0, 0, 255));//画文本
        }
    }
}
