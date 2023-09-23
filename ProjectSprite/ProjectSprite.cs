using Love;
using Love.Awesome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace TestProjectCore
{
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    public enum RoleStaus
    {
        Move,
        Idle
    }

    public struct Role
    {
        public ClassSequenceAnimation ani;
        public RectangleF box;
        public float x;
        public float y;
        public float speed;
        public Direction direction;
        public RoleStaus status;
        public float lastX;
        public float lastY;
    }

    /// <summary>
    /// 泡泡堂
    /// </summary>
    public class TestSprite : Scene
    {
        Role _role;
        ClassImage _imageBkg;
        Canvas _canvas;

        ClassImage _imageHouse;
        RectangleF _rectHouse;


        ClassImage _imageBox;
        RectangleF _rectBox;

        
        public override void Load()
        {
            var imageDataAll = Image.NewImageData("bubble/thing.png");

            
            //平铺模式
            _imageBkg = new ClassImage("bubble/bg.png");

            var imageDataHouse = Image.NewImageData(50, 60);
            imageDataHouse.Paste(imageDataAll, 0, 0, 0, 255, 200, 320);
            var imageHouse = Graphics.NewImage(imageDataHouse);
            _imageHouse = new ClassImage(imageHouse);

            var imageDataBox = Image.NewImageData(50, 50);
            imageDataBox.Paste(imageDataAll, 0, 0, 0, 185, 200, 320);
            var imageBox = Graphics.NewImage(imageDataBox);
            _imageBox = new ClassImage(imageBox);

            _canvas = Graphics.NewCanvas(Graphics.GetWidth(), Graphics.GetHeight());
            _canvas.SetWrap(WrapMode.Repeat, WrapMode.Repeat);


            _role = new Role
            {
                ani = new ClassSequenceAnimation("bubble/bnb.png", 12, 8, 60, 60),
                x = 300,
                y = 300,
                box = new RectangleF(310, 305, 40, 50),
                speed = 3,
                direction = Direction.Right,
                status = RoleStaus.Idle
            };
            _role.ani.Start();


            _rectHouse = new RectangleF(255, 400, 40, 60);
            _rectBox = new RectangleF(205, 100, 40, 50);
        }

        private void RoleUpdate(float dt)
        {
            _role.ani.Update(dt);
            // 移动判断  往左是减横向  往右是加横向  往上是减纵向 往下是加纵向
            if (Keyboard.IsDown(KeyConstant.Right))
            {
                _role.direction = Direction.Right;
                _role.status = RoleStaus.Move;
                _role.x += _role.speed;
                _role.box.X += _role.speed;
            }
            else if (Keyboard.IsDown(KeyConstant.Left))
            {
                _role.direction = Direction.Left;
                _role.status = RoleStaus.Move;
                _role.x -= _role.speed;
                _role.box.X -= _role.speed;
            }
            else if (Keyboard.IsDown(KeyConstant.Up))
            {
                _role.direction = Direction.Up;
                _role.status = RoleStaus.Move;
                _role.y -= _role.speed;
                _role.box.Y -= _role.speed;
            }
            else if (Keyboard.IsDown(KeyConstant.Down))
            {
                _role.direction = Direction.Down;
                _role.status = RoleStaus.Move;
                _role.y += _role.speed;
                _role.box.Y += _role.speed;
            }
            //静止让图片显示最后按下的方向
            //注意：动画帧是从0开始播放的 这里载入了12副图 0是第一张  11是最后一张
            if (_role.status == RoleStaus.Idle)
            {
                if (_role.direction == Direction.Down)
                {
                    _role.ani.GotoFrame(0);
                }
                else if (_role.direction == Direction.Up)
                {
                    _role.ani.GotoFrame(0);
                }
                else if (_role.direction == Direction.Left)
                {
                    _role.ani.GotoFrame(0);
                }
                else if (_role.direction == Direction.Right)
                {
                    _role.ani.GotoFrame(0);
                }

            }
            else if (_role.status == RoleStaus.Move)
            {
                //置帧段 0开始  2结束 播放 0 1 2 三幅图
                if (_role.direction == Direction.Down)
                {
                    _role.ani.SetFrameSegment(0, 2);
                }
                else if (_role.direction == Direction.Up)
                {
                    _role.ani.SetFrameSegment(3, 5);
                }
                else if (_role.direction == Direction.Left)
                {
                    _role.ani.SetFrameSegment(6, 8);
                }
                else if (_role.direction == Direction.Right)
                {
                    _role.ani.SetFrameSegment(9, 11);
                }
            }
            if (!Keyboard.IsDown(KeyConstant.Down) && !Keyboard.IsDown(KeyConstant.Up) && !Keyboard.IsDown(KeyConstant.Left) && !Keyboard.IsDown(KeyConstant.Right))
            {
                _role.status = RoleStaus.Idle;
            }
        }

        public override void Update(float dt)
        {
            _role.lastX = _role.x;
            _role.lastY = _role.y;
            RoleUpdate(dt);
            _role.box.X = _role.x + 10;
            _role.box.Y = _role.y + 5;
            if(_role.box.IntersectsWith(_rectHouse) || _role.box.IntersectsWith(_rectBox)) 
            {
                _role.x = _role.lastX;
                _role.y = _role.lastY;
            }
        }

        public override void Draw()
        {
            _imageBkg.DrawQuad(Graphics.NewQuad(0, 0, Graphics.GetWidth(), Graphics.GetHeight(), 32, 32), 0, 0);
            _role.ani.Draw(_role.x, _role.y);
            _imageHouse.Draw(250, 400);
            _imageBox.Draw(200, 100);
            Graphics.Rectangle(DrawMode.Line, _rectHouse);
            Graphics.Rectangle(DrawMode.Line, _rectBox);
            Graphics.Rectangle(DrawMode.Line, _role.box);

        }
    }
}
