using Love;
using Love.Awesome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProjectCore
{
    public class PlaneProp
    {
        public Love.Point point;
        public float speed;
        public int blood;
        public int level;
        public Love.Rectangle rect;
        public int bulletInterval;
    }

    public class EnemyProp
    {
        public Love.Point point;
        public float speed;
        public int blood;
        public Love.Rectangle rect;
        public Love.Rectangle rectBoss;
    }

    public class BulletProp
    {
        public Love.Point point;
        public int speed;
        public float angle;
        public int damage;
        public Love.Rectangle rect;
    }

    public class CloudProp
    {
        public Love.Point point;
        public int speed;
        public float angle;
    }

    public class AudioGame
    {
        public Love.Source audioBkg;
        public Love.Source audioAttack;
        public Love.Source audioHit;
        public Love.Source[] audioBlast = new Love.Source[6];
        public Love.Source audioDeath;
        public Love.Source audioUpLevel;
    }

    public class Plane
    {
        private ClassImage plane;
        private float attackInterval = 0;

        public Plane(ImageData planeData)
        {
            plane = new ClassImage(planeData);
            TestPlaneWar._planeProp.rect = new Rectangle(0, 0, 40, 40);
        }

        public void Draw()
        {
            plane.DrawEx(TestPlaneWar._planeProp.point.X - 65, TestPlaneWar._planeProp.point.Y - 50, 1, 1);
            TestPlaneWar._planeProp.rect = new Rectangle(TestPlaneWar._planeProp.point.X - 20, TestPlaneWar._planeProp.point.Y - 30, 40, 40);
            attackInterval += 1;
            if(attackInterval == TestPlaneWar._planeProp.bulletInterval)
            {
                attackInterval = 0;
                Fire(TestPlaneWar._planeProp.point);
            }
        }

        private void Fire(Love.Point bulletPoint)
        {
            var rt = new Random();
            BulletProp index = new BulletProp
            {
                point = bulletPoint,
                damage = rt.Next(8, 12),
                rect = new Rectangle(bulletPoint.X, bulletPoint.Y, 10, 10),
                speed = 18
            };
            TestPlaneWar._bulletProp.Add(index);
        }
    }

    public class EnemyPlane
    {
        private ClassImage _plane;
        private Love.Point[] _pointBlast = new Point[6];
        private ClassSequenceAnimation[] _aniBlast = new ClassSequenceAnimation[6];
        

        public EnemyPlane(ImageData planeData, ImageData blastData, Love.Source source)
        {
            _plane = new ClassImage(planeData);
            var imageBlast = Graphics.NewImage(blastData);
            for(int i = 0; i < 6; i++)
            {
                RandomEnemy(i);
                _aniBlast[i] = new ClassSequenceAnimation(imageBlast, 16, 20, 64, 64);
                _aniBlast[i].Start();
                _pointBlast[i].X = 0;
                _pointBlast[i].Y = 0;
            }
            for (int i = 0; i < 6; i++)
            {
                TestPlaneWar._audio.audioBlast[i] = source ;
            }
        }

        private void RandomEnemy(int index)
        {
            var rt = new Random();
            TestPlaneWar._enemyProp[index].point = new Point
            {
                X = rt.Next(20, 698),
                Y = -150
            };
            TestPlaneWar._enemyProp[index].rect = new Rectangle(0, 0, 50, 50);
            TestPlaneWar._enemyProp[index].speed = rt.NextSingle() * 3.4f + 1.5f;
        }

        private void PlaneBlast()
        {
            for (int i = 0; i < 6; i++)
            {
                if (_pointBlast[i].Y !=0)
                {
                    if(TestPlaneWar._audio.audioBlast[i].IsPlaying() == false)
                    {
                        TestPlaneWar._audio.audioBlast[i].Play();
                    }
                    if (_aniBlast[i].GetFrameIndex() >= 10)
                    {
                        _aniBlast[i].GotoFrame(0);
                        _pointBlast[i].Y = 0;
                    }
                    else
                    {
                        _aniBlast[i].Update(TestPlaneWar._lastDt);
                        _aniBlast[i].Draw(_pointBlast[i].X, _pointBlast[i].Y);
                    }
                }
            }
        }

        public void Draw()
        {
            for (int i = 0; i < 6; i++)
            {
                TestPlaneWar._enemyProp[i].point.Y += (int)TestPlaneWar._enemyProp[i].speed;
                if(TestPlaneWar._enemyProp[i].point.Y > 800)
                {
                    RandomEnemy(i);
                }
                _plane.Draw(TestPlaneWar._enemyProp[i].point.X, TestPlaneWar._enemyProp[i].point.Y);
                TestPlaneWar._enemyProp[i].rect = new Rectangle(TestPlaneWar._enemyProp[i].point.X + 25, TestPlaneWar._enemyProp[i].point.Y + 20, 50, 50);
                if(TestPlaneWar._enemyProp[i].rect.IntersectsWith(TestPlaneWar._planeProp.rect))
                {
                    _pointBlast[i] = TestPlaneWar._enemyProp[i].point;
                    RandomEnemy(i);
                }
                for(int j = 0; j < TestPlaneWar._bulletProp.Count; j++)
                {
                    if (TestPlaneWar._enemyProp[i].rect.IntersectsWith(TestPlaneWar._bulletProp[j].rect))
                    {
                        _pointBlast[i] = TestPlaneWar._enemyProp[i].point;
                        RandomEnemy(i);
                        TestPlaneWar._bulletProp.RemoveAt(j);
                        break;
                    }
                }
            }
            PlaneBlast();
        }
    }

    public class WindowSize
    {
        public int width;
        public int height;
    }

    /// <summary>
    /// 子弹类
    /// </summary>
    public class Bullet
    {
        private ClassImage _bullet;
        public Bullet(ImageData image)
        {
            _bullet = new ClassImage(image);
        }

        public void Draw()
        {
            for(int i = 0; i<TestPlaneWar._bulletProp.Count; i++)
            {
                TestPlaneWar._bulletProp[i].point.Y -= TestPlaneWar._bulletProp[i].speed;
                _bullet.DrawEx(TestPlaneWar._bulletProp[i].point.X - 10, TestPlaneWar._bulletProp[i].point.Y - 80, 0.8f, 0.8f);
                TestPlaneWar._bulletProp[i].rect = new Rectangle(TestPlaneWar._bulletProp[i].point.X + 5, TestPlaneWar._bulletProp[i].point.Y + 3, 10, 10);
                if(TestPlaneWar._bulletProp[i].point.Y < -10 || TestPlaneWar._bulletProp[i].point.X < -10 || TestPlaneWar._bulletProp[i].point.X > 810)
                {
                    TestPlaneWar._bulletProp.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public class ScenePlane
    {
        private ClassImage _cloud;
        private ClassImage _bkg;
        public ScenePlane(ImageData cloudData, ImageData bkgData, Love.Source source)
        {
            _cloud = new ClassImage(cloudData);
            _bkg = new ClassImage(bkgData);
            TestPlaneWar._audio.audioBkg = source;
            TestPlaneWar._audio.audioBkg.SetLooping(true);
            TestPlaneWar._audio.audioBkg.Play();
        }

        public void Draw(int type, Love.Point point, int bkgY, float speed)
        {
            if(type == 0)
            {
                _bkg.DrawQuad(Graphics.NewQuad(0, bkgY, TestPlaneWar._windowSize.width, TestPlaneWar._windowSize.height, 800, 800));

            }
            else if(type == 1)
            {
                if(speed>=6)
                {
                    _cloud.DrawEx(point.X, point.Y, 0.7f, 0.7f);
                }
                else
                {
                    _cloud.Draw(point.X, point.Y);
                }
            }
        }
    }


    public class TestPlaneWar : Scene
    {
        ClassZipResource _zip;

        Plane _plane;
        ScenePlane _scene;
        CloudProp _cloudProp;
        EnemyPlane _enemy;
        Bullet _bullet;
        int _bkgY;


        public static float _lastDt;
        public static PlaneProp _planeProp = new PlaneProp();
        public static EnemyProp[] _enemyProp = new EnemyProp[6];
        public static List<BulletProp> _bulletProp = new List<BulletProp>();
        public static AudioGame _audio = new AudioGame();
        public static WindowSize _windowSize;

        public override void Load()
        {
            _zip = new ClassZipResource("planewar\\dat.zip", true, "123456");
            _windowSize = new WindowSize
            {
                width = 800,
                height = 800
            };
            _plane = new Plane(_zip.ReadData("plane.png"));
            _planeProp = new PlaneProp
            {
                point = new Point
                {
                    X = 800 / 2 - 65,
                    Y = 800 - 110
                }
            };

            _scene = new ScenePlane(_zip.ReadData("cloud.png"), _zip.ReadData("bkg.png"), Audio.NewSource("planewar\\wav\\bkg.wav", SourceType.Static));
            var rt = new Random();
            _cloudProp = new CloudProp
            {
                point = new Point
                {
                    X = rt.Next(-50, 800 - 250),
                    Y = -200
                },
                speed = (int)(rt.NextSingle() * 6.5f + 1.5f)
            };

            for(int i = 0; i<6;i++)
            {
                _enemyProp[i] = new EnemyProp();
            }
            _enemy = new EnemyPlane(_zip.ReadData("enemy.png"), _zip.ReadData("boom3.png"), Audio.NewSource("planewar\\wav\\blast.wav", SourceType.Static));
            _bullet = new Bullet(_zip.ReadData("bullet.png"));
        }

        public override void Update(float dt)
        {
            _lastDt = dt;
            UpdateBkg();
            UpdateCloud();
            UpdatePlane();
        }

        private void UpdateBkg()
        {
            _bkgY -= 1;
        }

        private void UpdateCloud()
        {
            _cloudProp.point.Y += _cloudProp.speed;
             
            if(_cloudProp.point.Y > 1300)
            {
                var rt = new Random();
                _cloudProp.point.X = rt.Next(-50, 800 - 250);
                _cloudProp.speed = rt.Next(4, 8);
                if(_cloudProp.speed >=6)
                {
                    _cloudProp.point.Y = -300;
                }
                else
                {
                    _cloudProp.point.Y = -100;
                }
            }

        }

        private void UpdatePlane()
        {
            _planeProp.level = 1;
            _planeProp.bulletInterval = 8;
            var pos = Mouse.GetPosition();
            _planeProp.point = new Point((int)pos.X, (int)pos.Y);
            
        }

        public override void Draw()
        {
            _scene.Draw(0, new Point(0, 0), _bkgY, 0);
            _enemy.Draw();
            _plane.Draw();
            _bullet.Draw();
            _scene.Draw(1, _cloudProp.point, 0, _cloudProp.speed);
        }
    }
}
