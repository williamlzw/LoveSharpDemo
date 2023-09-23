using Love;
using Love.Awesome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProjectCore
{
    public class Ball
    {
        public ClassImage image;
        public float x;
        public float y;
        /// <summary>
        /// 横方向速度
        /// </summary>
        public float speedX;
        /// <summary>
        /// 纵方向速度
        /// </summary>
        public float speedY;
        /// <summary>
        /// 横方向加速度
        /// </summary>
        public float accelerationX;
        /// <summary>
        /// 纵方向加速度
        /// </summary>
        public float accelerationY;
        /// <summary>
        /// 横方向摩擦力
        /// </summary>
        public float frictionX;
        /// <summary>
        /// 旋转度
        /// </summary>
        public float angle;
    }

    public class TestUniformMotion : Scene
    {
        Ball _ball;
        Love.Source _soundEffects1;

        public override void Load()
        {
            _soundEffects1 = Audio.NewSource("ball/ball.wav", SourceType.Static);
            _ball = new Ball
            {
                image = new ClassImage("ball/BALL.png"),
                x = 20,
                y = 20,
                speedX = 10,
                speedY = 10,
                angle = 0f
            };
            _ball.image.SetOrgin(_ball.image.Width / 2, _ball.image.Height / 2);
        }

        public override void Update(float dt)
        {
            _ball.x += _ball.speedX;
            _ball.y += _ball.speedY;
            _ball.angle += _ball.speedX / 10;

            // 这里要注意重启不然快速撞击两次 只会播放一次
            // 判断是不让小球飞出屏幕范围
            if (_ball.y > Graphics.GetHeight() - 20 || _ball.y < 20)
            {
                Audio.Stop(_soundEffects1);
                Audio.Play(_soundEffects1);
                _ball.speedY = -_ball.speedY;
            }

            if (_ball.x > Graphics.GetWidth() - 20 || _ball.x < 20)
            {
                Audio.Stop(_soundEffects1);
                Audio.Play(_soundEffects1);
                _ball.speedX = -_ball.speedX;
            }
        }

        public override void Draw()
        {
            Graphics.Clear(Color.White);
            _ball.image.DrawEx(_ball.x, _ball.y, 1, 1, _ball.angle);
        }
    }

    public class TestVariableMotion : Scene
    {
        Ball _ball;
        public override void Load()
        {
            _ball = new Ball
            {
                image = new ClassImage("ball/BALL.png"),
                x = 0,
                y = 240,
                speedX = 6,
                frictionX = 0.03f,
                angle = 0f
            };
            _ball.image.SetOrgin(_ball.image.Width / 2, _ball.image.Height / 2);
        }

        public override void Update(float dt)
        {
            // 这里大于0代表小球移动中 当移动中每帧 就减少一点速度
            //默认是6移动/0.03=200帧 一秒60帧左右3.3秒左右就停
            if (_ball.speedX > 0)
            {
                _ball.speedX -= _ball.frictionX;
                _ball.x += _ball.speedX;
                _ball.angle += _ball.speedX / 10;
            }
        }

        public override void Draw()
        {
            Graphics.Clear(Color.White);
            _ball.image.DrawEx(_ball.x, _ball.y, 1, 1, _ball.angle);
        }
    }

    public class TestAccelerationMotion : Scene
    {
        Ball _ball;
        Love.Source _soundEffects1;
        float _gravitySpeed = 10;

        public override void Load()
        {
            _soundEffects1 = Audio.NewSource("ball/ball.wav", SourceType.Static);
            _ball = new Ball
            {
                image = new ClassImage("ball/BALL.png"),
                x = 20,
                y = 20,
                accelerationX = -0.5f,
                accelerationY = 2f,
                speedX = 8,
                speedY = 1,
                angle = 0f
            };
            _ball.image.SetOrgin(_ball.image.Width / 2, _ball.image.Height / 2);
        }

        public override void Update(float dt)
        {
            //小球【下降】判断
            if (_ball.speedY >= 0)
            {
                _ball.speedY += _gravitySpeed;
                _ball.y += _ball.speedY;
                //小球到地面后做出的事件判断
                if (_ball.y >= Graphics.GetHeight() - 20)
                {
                    _ball.speedY = -_ball.speedY;
                    _ball.y = Graphics.GetHeight() - 20;
                    //小球加速行为
                    _ball.speedY += _ball.accelerationY;
                    _ball.speedX += _ball.accelerationX;
                    if (_ball.speedX <= 0)
                    {
                        _ball.speedX = 0;
                    }
                    else
                    {
                        Audio.Stop(_soundEffects1);
                        Audio.Play(_soundEffects1);
                    }
                }
            }
            else
            {
                //小球【弹起】判断
                _ball.speedY += _gravitySpeed;
                if (_ball.speedY >= 0)
                {
                    _ball.speedY = 0;
                }
                else
                {
                    _ball.y += _ball.speedY;
                }
            }

            //这里是小球横向移动
            _ball.x += _ball.speedX;
            _ball.angle += _ball.speedX / 10;
        }

        public override void Draw()
        {
            Graphics.Clear(Color.White);
            _ball.image.DrawEx(_ball.x, _ball.y, 1, 1, _ball.angle);
        }
    }
}
