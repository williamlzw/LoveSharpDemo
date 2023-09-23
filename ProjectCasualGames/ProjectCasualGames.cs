using System;
using Love;
using Love.Awesome;

namespace TestProjectCore
{
    /// <summary>
    /// 休闲游戏背景
    /// </summary>
    public class TestCasualGames : Scene
    {
        ClassImage _imageBack;
        ClassImage _imageSurface;
        ClassImage _imageShip;

        ClassImage _imageSailboat;
        ClassImage _imageLeaves1;
        ClassImage _imageLeaves2;
        ClassImage _imageLeaves3;

        float _surfacePosition = 0f;
        float _surfaceOffset = 0.4f;
        float _shipRadian = 0f;
        float _shipOffset = 0.002f;
        float _sailboatPosition = 0f;
        float _sailboatOffset = 0.2f;
        float _leavesRadian1 = 0f;
        float _leavesOffset1 = 0.002f;
        float _leavesRadian2 = 0f;
        float _leavesOffset2 = 0.002f;
        float _leavesRadian3 = 0f;
        float _leavesOffset3 = -0.0015f;

        public override void Load()
        {
            _imageBack = new ClassImage("casual/GameBack.png");
            _imageSurface = new ClassImage("casual/GameBackAni_00.png");

            var imageDataAll = Image.NewImageData("casual/GameBackAni_04.png");
            var imageDataShip = Image.NewImageData(80, 50);
            imageDataShip.Paste(imageDataAll, 0, 0, 40, 0, 200, 50);
            var imageShip = Graphics.NewImage(imageDataShip);
            _imageShip = new ClassImage(imageShip);

            var imageDataSailboat = Image.NewImageData(40, 50);
            imageDataSailboat.Paste(imageDataAll, 0, 0, 0, 0, 200, 50);
            var imageSailboat = Graphics.NewImage(imageDataSailboat);
            _imageSailboat = new ClassImage(imageSailboat);

            _imageLeaves1 = new ClassImage("casual/GameBackAni_01.png");
            _imageLeaves2 = new ClassImage("casual/GameBackAni_02.png");
            _imageLeaves3 = new ClassImage("casual/GameBackAni_03.png");
            _imageShip.SetOrgin(40, 25);
            _imageLeaves1.SetOrgin(43, 0);
            _imageLeaves2.SetOrgin(0, 55);
        }

        public override void Update(float dt)
        {
            _surfacePosition += _surfaceOffset;
            if (_surfacePosition > 30 || _surfacePosition < 0)
            {
                _surfaceOffset = -_surfaceOffset;
            }

            _shipRadian += _shipOffset;
            if (_shipRadian > 0.1f || _shipRadian <= -0.1f)
            {
                _shipOffset = -_shipOffset;
            }

            _sailboatPosition += _sailboatOffset;
            if (_sailboatPosition > 200 || _sailboatPosition < 0)
            {
                _sailboatOffset = -_sailboatOffset;
            }

            _leavesRadian1 += _leavesOffset1;
            if (_leavesRadian1 >= 0.2f || _leavesRadian1 <= -0.2f)
            {
                _leavesOffset1 = -_leavesOffset1;
            }

            _leavesRadian2 += _leavesOffset2;
            if (_leavesRadian2 >= 0.1f || _leavesRadian2 <= -0.05f)
            {
                _leavesOffset2 = -_leavesOffset2;
            }

            _leavesRadian3 += _leavesOffset3;
            if (_leavesRadian3 >= 0f || _leavesRadian3 <= -0.2f)
            {
                _leavesOffset3 = -_leavesOffset3;
            }
        }

        public override void Draw()
        {
            _imageSurface.DrawStretch(_surfacePosition, 0, Graphics.GetWidth() + _surfacePosition, Graphics.GetHeight());
            _imageBack.DrawStretch(0, 0, Graphics.GetWidth(), Graphics.GetHeight());
            _imageLeaves1.DrawEx(675 + 43, 5, 1, 1, _leavesRadian1);
            _imageLeaves2.DrawEx(0, 50, 1, 1, _leavesRadian2);
            _imageLeaves3.DrawEx(0, 100, 1, 1, _leavesRadian3);
            _imageShip.DrawEx(350, 400, 1, 1, _shipRadian);
            _imageSailboat.Draw(300 + _sailboatPosition, 330);
        }
    }
}
