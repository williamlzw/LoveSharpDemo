using Love.Awesome;
using Love;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProjectCore
{
    /// <summary>
    /// 测试序列动画
    /// </summary>
    public class TestSequenceAnimation : Scene
    {
        ClassSequenceAnimation _sequenceAnimation;
        float _scaleX;
        float _scaleY;

        public override void Load()
        {
            var spriteSheetImage = Graphics.NewImage("animation.png");
            _sequenceAnimation = new ClassSequenceAnimation(spriteSheetImage, 10, 8, 496, 361);
            _scaleX = (float)Graphics.GetWidth() / 496;
            _scaleY = (float)Graphics.GetHeight() / 361;
        }

        public override void Update(float dt)
        {
            _sequenceAnimation.Update(dt);
        }

        public override void Draw()
        {
            _sequenceAnimation.Draw(0, 0, _scaleX, _scaleY);
        }
    }
}
