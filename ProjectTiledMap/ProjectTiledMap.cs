using Love;
using Love.Awesome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectTiledMap
{
    public class TestTiledMap : Scene
    {
        ClassSequenceAnimation _ani;
        ClassTiledMap _map;
        Love.Point _position;
        Love.Point _offsetMap;
        int _isMove = 0;
        int _speed = 5;

        public override void Load()
        {
            _ani = new ClassSequenceAnimation("role.png", 16, 10, 32, 48);
            _ani.SetOrgin(16, 40);
            _ani.SetFrameSegment(0, 3);
            _ani.Start();
            _position = new Point(1168, 1936);
            _map = new ClassTiledMap();
            _map.Open("Field.tmx", Graphics.GetWidth(), Graphics.GetHeight());
            _offsetMap = _map.Update(ref _position);
        }

        public override void Update(float dt)
        {
            if (Keyboard.IsDown(KeyConstant.Up))
            {
                if(_isMove != 1)
                {
                    _isMove = 1;
                    _ani.SetFrameSegment(12, 15);
                }
                _position.Y = _position.Y - _speed;
                _offsetMap = _map.Update(ref _position);
            }
            else if (Keyboard.IsDown(KeyConstant.Down))
            {
                if (_isMove != 2)
                {
                    _isMove = 2;
                    _ani.SetFrameSegment(0, 3);
                }
                _position.Y = _position.Y + _speed;
                _offsetMap = _map.Update(ref _position);
            }
            else if (Keyboard.IsDown(KeyConstant.Left))
            {
                if (_isMove != 3)
                {
                    _isMove = 3;
                    _ani.SetFrameSegment(4, 7);
                }
                _position.X = _position.X - _speed;
                _offsetMap = _map.Update(ref _position);
            }
            else if (Keyboard.IsDown(KeyConstant.Right))
            {
                if (_isMove != 4)
                {
                    _isMove = 4;
                    _ani.SetFrameSegment(8, 11);
                }
                _position.X = _position.X + _speed;
                _offsetMap = _map.Update(ref _position);
            }
            else
            {
                if(_isMove > 0)
                {
                    _isMove = -1 * _isMove;
                }
            }

            if(_isMove > 0)
            {
                _ani.Stop();
                _ani.Start();
            }
            else
            {
                if(_isMove == -1)
                {
                    _ani.GotoFrame(0);
                }
                else if (_isMove == -2)
                {
                    _ani.GotoFrame(0);
                }
                else if (_isMove == -3)
                {
                    _ani.GotoFrame(0);
                }
                else if (_isMove == -4)
                {
                    _ani.GotoFrame(0);
                }
                _ani.Stop();
            }
            _ani.Update(dt);
           
        }

        public override void Draw()
        {
            _map.Draw();
            _ani.Draw(_position.X + _offsetMap.X, _position.Y + _offsetMap.Y);
        }
    }
}
