using Love.Awesome;
using Love;
using System;

namespace TestProjectCore
{
    /// <summary>
    /// 基本演示,可以nativeaot
    /// </summary>
    public class TestScreen : Scene
    {
        ClassImage _image;
        float _scaleX;
        float _scaleY;

        ClassFont _font;
        ClassSequenceAnimation _sequenceAnimation;
        Vector2 _position;
        bool _keyDown = false;
        bool _keyPressed = false;
        bool _stopPlay = true;
        bool _leftPressed = false;
        bool _rightPressed = false;
        Love.Source _soundEffects1;
        Love.Source _soundEffects2;
        Love.Source _soundEffects3;

        public override void Load()
        {
            _image = new ClassImage("texture.jpg");
            _scaleX = (float)Graphics.GetWidth() / _image.Width;
            _scaleY = (float)Graphics.GetHeight() / _image.Height;
            _font = new ClassFont("HYC1GJM.ttf", 20);
            _sequenceAnimation = new ClassSequenceAnimation("cursor.png", 8, 8, 32, 32);
            _sequenceAnimation.PauseAtStart();
            _soundEffects1 = Audio.NewSource("bkgsound.ogg", SourceType.Static);
            _soundEffects2 = Audio.NewSource("sound1.wav", SourceType.Static);
            _soundEffects3 = Audio.NewSource("sound2.wav", SourceType.Static);
            Mouse.SetVisible(false);
        }

        public override void Update(float dt)
        {
            _position = Mouse.GetPosition();
            _sequenceAnimation.Update(dt);
            _keyDown = Keyboard.IsDown(KeyConstant.Space);
            _keyPressed = Keyboard.IsPressed(KeyConstant.Space);
            _leftPressed = Mouse.IsPressed(MouseButton.LeftButton);
            _rightPressed = Mouse.IsPressed(MouseButton.RightButton);
            if (_keyPressed)
            {
                _stopPlay = !_stopPlay;
            }
        }

        public override void Draw()
        {
            var random = new Random();
            float x = random.NextSingle() * 2 - 1;
            float y = random.NextSingle() * 2 - 1;
            float sx = _scaleX + random.NextSingle() * 0.02f;
            float sy = _scaleY + random.NextSingle() * 0.02f;

            //_image.DrawEx(x, y, sx, sy);//震动效果
            _image.DrawStretch(0, 0, Graphics.GetWidth(), Graphics.GetHeight());

            if (_keyDown)
            {
                Graphics.SetColor(new Color(0, 255, 0, 255));
            }
            else
            {
                Graphics.SetColor(new Color(0, 0, 255, 255));
            }
            Graphics.Rectangle(DrawMode.Fill, new Rectangle(50 + (int)x, 50 + (int)y, 150, 50));

            Graphics.SetColor(new Color(255, 255, 255, 255));//透明
            _font.Draw("空格切换音乐,鼠标左键右键音效", 400 + (int)x, 150 + (int)y, new Vector4(255, 0, 0, 255));//画文本

            _sequenceAnimation.Draw(_position.X, _position.Y);//自定义鼠标指针


            if (_stopPlay)//控制暂停与继续播放
            {
                Audio.Play(_soundEffects1);
            }
            else
            {
                Audio.Pause(_soundEffects1);
            }
            if (_leftPressed)
            {
                Audio.Stop(_soundEffects2);
                Audio.Play(_soundEffects2);
            }
            if (_rightPressed)
            {
                Audio.Stop(_soundEffects3);
                Audio.Play(_soundEffects3);
            }
        }
    }
}
