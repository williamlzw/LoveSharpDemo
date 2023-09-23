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
    public enum StatusTurnBase
    {
        Idle,
        Run,
        Attack,
        Backspace
    }

    public class RoleTurnBase
    {
        public float x;
        public float y;
        public StatusTurnBase status;
        public float damage;
        public ClassSequenceAnimation aniIdle;
        public ClassSequenceAnimation aniRun;
        public ClassSequenceAnimation aniAttack;
        public ClassSequenceAnimation aniBackspace;
        public ClassSequenceAnimation aniEffect;
    }
    /// <summary>
    /// 回合制横板战斗
    /// </summary>
    public class TestTurnbase : Scene
    {
        ClassImage _imageBack;
        ClassImage _imageMonster;
        ClassSequenceAnimation _aniWater;
        RoleTurnBase _role;
        float _gradualValue = 0;
        float _lastdt;

        public override void Load()
        {
            var imageDataAll = Image.NewImageData("turnbase/res.png");

            var imageDataBack = Image.NewImageData(640, 480);
            imageDataBack.Paste(imageDataAll, 0, 0, 1441, 170, 2145, 1770);
            var imageBack = Graphics.NewImage(imageDataBack);
            _imageBack = new ClassImage(imageBack);

            var imageDataMonster = Image.NewImageData(100, 96);
            imageDataMonster.Paste(imageDataAll, 0, 0, 710, 155, 2145, 1770);
            var imageMonster = Graphics.NewImage(imageDataMonster);
            _imageMonster = new ClassImage(imageMonster);

            var imageDataWater = Image.NewImageData(2000, 720);
            imageDataWater.Paste(imageDataAll, 0, 0, 0, 1050, 2145, 1770);
            var imageWater = Graphics.NewImage(imageDataWater);
            _aniWater = new ClassSequenceAnimation(imageWater, 30, 15, 400, 120);

            var imageDataIdle = Image.NewImageData(74 * 9, 102);
            imageDataIdle.Paste(imageDataAll, 0, 0, 0, 200, 2145, 1770);
            var imageIdle = Graphics.NewImage(imageDataIdle);

            var imageDataRun = Image.NewImageData(123 * 8, 90);
            imageDataRun.Paste(imageDataAll, 0, 0, 0, 310, 2145, 1770);
            var imageRun = Graphics.NewImage(imageDataRun);

            var imageDataAttack = Image.NewImageData(128 * 10, 110);
            imageDataAttack.Paste(imageDataAll, 0, 0, 0, 400, 2145, 1770);
            var imageAttack = Graphics.NewImage(imageDataAttack);

            var imageDataBackspace = Image.NewImageData(165 * 8, 144);
            imageDataBackspace.Paste(imageDataAll, 0, 0, 0, 0, 2145, 1770);
            var imageBackspace = Graphics.NewImage(imageDataBackspace);

            var imageDataEffect = Image.NewImageData(192 * 6, 192);
            imageDataEffect.Paste(imageDataAll, 0, 0, 0, 510, 2145, 1770);
            var imageEffect = Graphics.NewImage(imageDataEffect);
            _role = new RoleTurnBase
            {
                aniIdle = new ClassSequenceAnimation(imageIdle, 9, 10, 74, 102),
                aniRun = new ClassSequenceAnimation(imageRun, 8, 10, 123, 90),
                aniAttack = new ClassSequenceAnimation(imageAttack, 10, 10, 128, 110),
                aniBackspace = new ClassSequenceAnimation(imageBackspace, 8, 13, 165, 144),
                aniEffect = new ClassSequenceAnimation(imageEffect, 6, 10, 192, 192),
                status = StatusTurnBase.Idle,
                x = 500,
                y = 250
            };

            _aniWater.Start();
            _role.aniIdle.Start();
            _role.aniRun.Start();
            _role.aniAttack.Start();
            _role.aniBackspace.Start();
            _role.aniEffect.Start();
     
        }

        public override void Update(float dt)
        {
            _lastdt = dt;
            _aniWater.Update(dt);
            
            if (_role.status == StatusTurnBase.Idle)
            {
                _role.aniIdle.Update(dt);
            }
            else if (_role.status == StatusTurnBase.Run)
            {
                _role.aniRun.Update(dt);
                //这里100是攻击的横位置 横坐标到了100 就停止前进 从右到左是减
                if (_role.x > 100)
                {
                    _role.x = _role.x - 5;
                }
                else
                {
                    _role.aniRun.GotoFrame(0);
                    _role.status = StatusTurnBase.Attack;
                }
            }
            else if (_role.status == StatusTurnBase.Attack)
            {
                _role.aniAttack.Update(dt);
                //这里采用了求余数 当前帧是：1，3，5，7，9  #PG_混合_AlphaAdd  0，2，4，6，8 初始化混合  为了做出一闪一闪效果
                if (_role.aniAttack.GetFrameIndex() % 2 == 0)
                {
                    _imageMonster.SetBlendMode(BlendMode.Alpha, BlendAlphaMode.Multiply);
                }
                else
                {
                    _imageMonster.SetBlendMode(BlendMode.Add, BlendAlphaMode.Multiply);
                }
                _role.aniEffect.Update(dt);
                if (_role.aniAttack.GetFrameIndex() == 9)
                {
                    _role.aniAttack.GotoFrame(0);
                    _imageMonster.SetBlendMode(BlendMode.Alpha, BlendAlphaMode.Multiply);
                    _role.aniEffect.Start();
                    _role.damage = new Random().Next(100, 500);
                    _role.status = StatusTurnBase.Backspace;
                }
            }
            else if (_role.status == StatusTurnBase.Backspace)
            {
                _role.aniBackspace.Update(dt);
                if (_role.x < 500)
                {
                    _role.x = _role.x + 10;
                }
                else
                {
                    _role.aniBackspace.GotoFrame(0);
                    _role.status = StatusTurnBase.Idle;
                }
            }
            if(Keyboard.IsPressed(KeyConstant.Space) && _role.status == StatusTurnBase.Idle)
            {
               
                _role.status = StatusTurnBase.Run;
               
            }
        }

        public override void Draw()
        {
            
            Graphics.Clear(Color.White);
            Graphics.SetColor(new Color(255, 255, 255, 255));
            _aniWater.Draw(0, 150, 2, 1);
            _imageBack.Draw();

            _imageMonster.SetColor(Color.White);
            _imageMonster.Draw(50, 250);
            _imageMonster.SetColor(new Love.Color(255, 255, 255, 50));
            _imageMonster.DrawEx(50, 250 + 150, 1, -1);

            if (_role.status == StatusTurnBase.Idle)
            {
                _role.aniIdle.SetColor(Color.White);
                _role.aniIdle.Draw(_role.x, _role.y, 1, 1);
                //倒影效果
                _role.aniIdle.SetColor(new Love.Color(255, 255, 255, 50));//半透明
                _role.aniIdle.Draw(_role.x, _role.y + 200, 1, -1);//-1倒影
                
            }
            else if(_role.status == StatusTurnBase.Run)
            {
              
                _role.aniRun.SetColor(Color.White);
                _role.aniRun.Draw(_role.x, _role.y, 1, 1);
                //倒影效果
                _role.aniRun.SetColor(new Love.Color(255, 255, 255, 50));//半透明
                _role.aniRun.Draw(_role.x, _role.y + 180, 1, -1);//-1倒影
            }
            else if (_role.status == StatusTurnBase.Attack)
            {
                _role.aniAttack.SetColor(Color.White);
                _role.aniAttack.Draw(_role.x, _role.y - 15, 1, 1);
                //倒影效果
                _role.aniAttack.SetColor(new Love.Color(255, 255, 255, 50));//半透明
                _role.aniAttack.Draw(_role.x, _role.y + 200, 1, -1);//-1倒影
                _role.aniEffect.Draw(_role.x - 70, _role.y - 70);
            }
            else if (_role.status == StatusTurnBase.Backspace)
            {
                _role.aniBackspace.SetColor(Color.White);
                _role.aniBackspace.Draw(_role.x, _role.y - 45, 1, 1);
                //倒影效果
                _role.aniBackspace.SetColor(new Love.Color(255, 255, 255, 50));//半透明
                _role.aniBackspace.Draw(_role.x, _role.y + 245, 1, -1);//-1倒影
            }

            if(_role.damage !=0)
            {

            }
        }
    }
}
