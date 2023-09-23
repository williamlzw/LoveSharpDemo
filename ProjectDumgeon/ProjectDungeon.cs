using Love;
using Love.Awesome;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProjectCore
{
    public enum Direction2D
    {
        Left,
        Right
    }

    public enum RoleStaus2D
    {
        Walk,
        Run,
        Static
    }

    public class BackGroundInfo
    {
        public ClassImage imageBack;
        public ClassImage imageSecond;
        public ClassImage imageFloor;
        public ClassImage image1;
        public ClassImage image2;
        public ClassImage image3;
        public ClassImage image4;
        public ClassImage image5;
        public ClassImage image6;
    }
     
    public class Role2D
    {
        public float x;
        public float y;
        public Direction2D direction;
        public RoleStaus2D status;
        public float speed;
        public ClassSequenceAnimation aniWalk;
        public ClassSequenceAnimation aniRun;
        public ClassSequenceAnimation aniStatic;
        public int lastDown;
    }

    /// <summary>
    /// 横板卷轴地图
    /// </summary>
    public class TestDungeon : Scene
    {
        Role2D _role;
        BackGroundInfo _background;
        Love.Size _size;
        float _leftUpTime;
        float _rightUpTime;
        Love.Vector2 _pointOffset;
        int _width;
        int _height;

        public override void Load()
        {
            //屏幕高宽设置为600,480
            _width = Graphics.GetWidth();
            _height = Graphics.GetHeight();
            _pointOffset = new Vector2(0, 0);
            _size = new Size(1600, 480);
            _background = new BackGroundInfo()
            {
                imageBack = new ClassImage("dungeon/back.png"),
                imageSecond = new ClassImage("dungeon/second.png"),
                imageFloor = new ClassImage("dungeon/floor.png"),
                image1 = new ClassImage("dungeon/8.png"),
                image2 = new ClassImage("dungeon/9.png"),
                image3 = new ClassImage("dungeon/10.png"),
                image4 = new ClassImage("dungeon/11.png"),
                image5 = new ClassImage("dungeon/12.png"),
                image6 = new ClassImage("dungeon/13.png"),
            };
            _role = new Role2D()
            {
                aniStatic = new ClassSequenceAnimation("dungeon/static.png", 3, 8, 80, 130),
                aniWalk = new ClassSequenceAnimation("dungeon/walk.png", 8, 8, 80, 130),
                aniRun = new ClassSequenceAnimation("dungeon/run.png", 8, 8, 90, 130),

            };
            _role.aniStatic.SetOrgin(40, 65);
            _role.aniWalk.SetOrgin(40, 65);
            _role.aniRun.SetOrgin(45, 65);
            _role.aniStatic.Start();
            _role.aniWalk.Start();
            _role.aniRun.Start();
            _role.x = 200;
            _role.y = 300;
            _role.status = RoleStaus2D.Static;
            _role.direction = Direction2D.Left;
            _role.aniStatic.SetLeft();
            _role.aniWalk.SetLeft();
            _role.aniRun.SetLeft();
            _role.speed = 2;
            _role.lastDown = 0;
        }


        public override void Update(float dt)
        {
            _role.aniStatic.Update(dt);
            _role.aniRun.Update(dt);
            _role.aniWalk.Update(dt);
            //这里记录最后按键 注意：一般记录按键 要放在最上 这样才好判断
            if (Keyboard.IsPressed(KeyConstant.Left))
            {
                _role.lastDown = 0;
                _role.aniStatic.SetLeft();
            }
            else if (Keyboard.IsPressed(KeyConstant.Right))
            {
                _role.lastDown = 1;
                _role.aniStatic.SetRight();
            }
            //这里是负责  更新左键弹起时间和右键弹起时间的  不更新的话 无法判断连续按键
            if (Keyboard.IsReleased(KeyConstant.Left))
            {
                if (Keyboard.IsDown(KeyConstant.Right))
                {
                    _role.lastDown = 1;
                }
                else
                {
                    _leftUpTime = Love.Timer.GetTime();
                    _role.status = RoleStaus2D.Static;
                }
            }
            else if (Keyboard.IsReleased(KeyConstant.Right))
            {
                if (Keyboard.IsDown(KeyConstant.Left))
                {
                    _role.lastDown = 0;
                }
                else
                {
                    _rightUpTime = Love.Timer.GetTime();
                    _role.status = RoleStaus2D.Static;
                }
            }

            //这里防止角色跑出屏幕左边边缘
            if (Keyboard.IsDown(KeyConstant.Left) && _role.lastDown == 0 && _role.x > 35)
            {
                _role.direction = Direction2D.Left;
                //因为上面已经更新了按键弹起事件 所以 运行时间-弹起时间 =连续按键的间隔
                if (Love.Timer.GetTime() - _leftUpTime < 0.1)
                {
                    _role.aniRun.SetLeft();
                    _leftUpTime = Love.Timer.GetTime();
                    _role.status = RoleStaus2D.Run;
                    _role.x -= _role.speed * 1.5f;
                }
                else
                {
                    _role.status = RoleStaus2D.Walk;
                    _role.x -= _role.speed;
                    _role.aniWalk.SetLeft();

                }
            }
            else if (Keyboard.IsDown(KeyConstant.Right) && _role.lastDown == 1 && _role.x < _size.Width - 35)
            {
                _role.direction = Direction2D.Right;
                //因为上面已经更新了按键弹起事件 所以 运行时间-弹起时间 =连续按键的间隔
                if (Love.Timer.GetTime() - _rightUpTime < 0.1)
                {
                    _role.aniRun.SetRight();
                    _rightUpTime = Love.Timer.GetTime();
                    _role.status = RoleStaus2D.Run;
                    _role.x += _role.speed * 1.5f;
                }
                else
                {
                    _role.status = RoleStaus2D.Walk;
                    _role.x += _role.speed;
                    _role.aniWalk.SetRight();
                }
            }

            //这里判断上移动 跑步状态上是行走状态的1.1倍    225是角色Y到了225就无法移动
            if (Keyboard.IsDown(KeyConstant.Up) && _role.y > 225)
            {
                if (_role.status == RoleStaus2D.Run)
                {
                    _role.y -= _role.speed * 1.1f;
                }
                else if (_role.status == RoleStaus2D.Walk)
                {
                    _role.y -= _role.speed;
                }
                else
                {
                    _role.status = RoleStaus2D.Walk;
                }
            }

            if (Keyboard.IsDown(KeyConstant.Down) && _role.y < 415)
            {
                if (_role.status == RoleStaus2D.Run)
                {
                    _role.y += _role.speed * 1.1f;
                }
                else if (_role.status == RoleStaus2D.Walk)
                {
                    _role.y += _role.speed;
                }
                else
                {
                    _role.status = RoleStaus2D.Walk;
                }
            }

            //判断什么按键不按 进入原地状态
            if (!Keyboard.IsDown(KeyConstant.Down) && !Keyboard.IsDown(KeyConstant.Up) && !Keyboard.IsDown(KeyConstant.Left) && !Keyboard.IsDown(KeyConstant.Right))
            {
                _role.status = RoleStaus2D.Static;
            }

            _pointOffset = Graphics.CalculateViewOffset(_role.x, _role.y, 640, 480, 1600, 480);

        }

        private void DrawRole()
        {
            if(_role.direction == Direction2D.Left)
            {
                if(_role.status == RoleStaus2D.Static)
                {
                   
                    //_role.aniStatic.Draw(_role.x + _pointOffset.X - 5, _role.y + 12 + _pointOffset.Y, 0.8f, 0.8f, -0.2f);
                    _role.aniStatic.Draw(_role.x + _pointOffset.X, _role.y + _pointOffset.Y);
                }
                else if (_role.status == RoleStaus2D.Walk)
                {
                   
                    _role.aniWalk.Draw(_role.x + _pointOffset.X, _role.y + _pointOffset.Y);
                }
                else if (_role.status == RoleStaus2D.Run)
                {
                    _role.aniRun.Draw(_role.x + _pointOffset.X, _role.y + _pointOffset.Y);
                }
            }
            else
            {
                if (_role.status == RoleStaus2D.Static)
                {
                    
                    //_role.aniStatic.Draw(_role.x + _pointOffset.X - 5, _role.y + 12 + _pointOffset.Y, 0.8f, 0.8f, -0.2f);
                    _role.aniStatic.Draw(_role.x + _pointOffset.X, _role.y + _pointOffset.Y);
                }
                else if (_role.status == RoleStaus2D.Walk)
                {
                  
                    _role.aniWalk.Draw(_role.x + _pointOffset.X, _role.y + _pointOffset.Y);
                }
                else if (_role.status == RoleStaus2D.Run)
                {
                   
                    _role.aniRun.Draw(_role.x + _pointOffset.X, _role.y + _pointOffset.Y);
                }
            }
        }

        public override void Draw()
        {
            //0.1是放慢的意思 正常地图偏移都是*1 想让偏移越慢就越小  地图底层背景是正常地图偏移慢10倍
            foreach (var index in Enumerable.Range(0, 4))
            {
                _background.imageBack.DrawStretch(index * _width + _pointOffset.X * 0.1f, _pointOffset.Y, index * _width + _pointOffset.X * 0.1f + _width, _pointOffset.Y + 295);
            }
            foreach (var index in Enumerable.Range(0, 4))
            {
                _background.imageSecond.DrawStretch(index * _width + _pointOffset.X * 0.4f, _pointOffset.Y, index * _width + _pointOffset.X * 0.4f + _width, _pointOffset.Y + 300);
            }
            foreach (var index in Enumerable.Range(0, 10))
            {
                _background.imageFloor.DrawStretch(index * 224 + _pointOffset.X * 1f, 250, index * 224 + _pointOffset.X * 1f + _width, _height);
            }

            _background.image1.Draw(80 + _pointOffset.X, -30);
            _background.image2.Draw(600 + _pointOffset.X, 80);
            _background.image3.Draw(1000 + _pointOffset.X, 80);
            _background.image4.Draw(350 + _pointOffset.X, 230);
            _background.image5.Draw(700 + _pointOffset.X, 220);
            _background.image6.Draw(1300 + _pointOffset.X, 220);

            DrawRole();

            _background.image3.Draw(400 + _pointOffset.X, 310);
        }

        
    }
}
