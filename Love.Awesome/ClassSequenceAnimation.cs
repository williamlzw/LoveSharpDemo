
namespace Love.Awesome
{
    public enum DirectionLR
    {
        Left,
        Right
    }

    /// <summary>
    /// 序列动画类
    /// </summary>
    public class ClassSequenceAnimation
    {
        Image _spriteSheetImage;
        SpriteAnimation _spriteAnimation;
        private float _ox = 0;
        private float _oy = 0;
        private DirectionLR _direction = DirectionLR.Left;

        /// <summary>
        /// 序列动画类
        /// </summary>
        /// <param name="image">图像</param>
        /// <param name="frames">帧数</param>
        /// <param name="frameRate">帧率 1秒刷新次数</param>
        /// <param name="frameWidth">帧宽度</param>
        /// <param name="frameHeight">帧高度</param>
        /// <param name="offsetLeft">图像偏移横坐标</param>
        /// <param name="offsetTop">图像偏移纵坐标</param>
        public ClassSequenceAnimation(Image image, int frames, float frameRate, int frameWidth, 
            int frameHeight) 
        {
            _spriteSheetImage = image;
            _spriteSheetImage.SetFilter(FilterMode.Nearest, FilterMode.Nearest);
            int cols = _spriteSheetImage.GetWidth() / frameWidth;
            int rows = _spriteSheetImage.GetHeight() / frameHeight;
            int imageWidth = _spriteSheetImage.GetWidth();
            int imageHeight = _spriteSheetImage.GetHeight();
            float duration = 1f / frameRate;
            int indexCol = frames / cols;
            int indexRow = frames % cols;
            // 10 /4 = 2, 10%3 =2
            // 3 /4 = 0, 3%3 = 3
            //4 /4 =1, 4%3=0
            // 8/4=2,8%3=0
            //Console.WriteLine($"{cols},{rows},{indexCol},{indexRow}");
            if (indexCol == 0)
            {
                _spriteAnimation = new SpriteAnimation(
               grid: new Grid(frameWidth, frameHeight, imageWidth, imageHeight),
               rawPointWithTimeList: new RawPointWithTimeList()
                   .AddRow(1, indexRow, 1, duration)
                   );
            }
            else
            {
                if(indexRow == 0)
                {
                    _spriteAnimation = new SpriteAnimation(
               grid: new Grid(frameWidth, frameHeight, imageWidth, imageHeight),
               rawPointWithTimeList: new RawPointWithTimeList()
                   .AddRange(1, cols, 1, indexCol, duration));
                }
                else
                {
                    _spriteAnimation = new SpriteAnimation(
               grid: new Grid(frameWidth, frameHeight, imageWidth, imageHeight),
               rawPointWithTimeList: new RawPointWithTimeList()
                   .AddRange(1, cols, 1, indexCol, duration)
                   .AddRow(1, indexRow, rows, duration)
                   );
                }
            }
        }

        public ClassSequenceAnimation(string imagePath, int frames, float frameRate, int frameWidth,
            int frameHeight)
        {
            _spriteSheetImage = Graphics.NewImage(imagePath);
            _spriteSheetImage.SetFilter(FilterMode.Nearest, FilterMode.Nearest);
            int cols = _spriteSheetImage.GetWidth() / frameWidth;
            int rows = _spriteSheetImage.GetHeight() / frameHeight;
            int imageWidth = _spriteSheetImage.GetWidth();
            int imageHeight = _spriteSheetImage.GetHeight();
            float duration = 1f / frameRate;
            int indexCol = frames / cols;
            int indexRow = frames % cols;
            if (indexCol == 0)
            {
                _spriteAnimation = new SpriteAnimation(
               grid: new Grid(frameWidth, frameHeight, imageWidth, imageHeight),
               rawPointWithTimeList: new RawPointWithTimeList()
                   .AddRow(1, indexRow, 1, duration)
                   );
            }
            else
            {
                if (indexRow == 0)
                {
                    _spriteAnimation = new SpriteAnimation(
               grid: new Grid(frameWidth, frameHeight, imageWidth, imageHeight),
               rawPointWithTimeList: new RawPointWithTimeList()
                   .AddRange(1, cols, 1, indexCol, duration));
                }
                else
                {
                    _spriteAnimation = new SpriteAnimation(
               grid: new Grid(frameWidth, frameHeight, imageWidth, imageHeight),
               rawPointWithTimeList: new RawPointWithTimeList()
                   .AddRange(1, cols, 1, indexCol, duration)
                   .AddRow(1, indexRow, rows, duration)
                   );
                }
            }
        }

        public void Update(float dt)
        {
            if(_spriteAnimation.Status == SpriteAnimation.AnimationStatus.Playing)
            {
                _spriteAnimation.Update(dt);
            }
            
        }

        /// <summary>
        /// 置热点
        /// </summary>
        /// <param name="ox"></param>
        /// <param name="oy"></param>
        public void SetOrgin(float ox, float oy)
        {
            _ox = ox;
            _oy = oy;
        }

        public void SetLeft()
        {
            if (_direction == DirectionLR.Right)
            {
                _direction = DirectionLR.Left;
                FlipH();
            }
        }

        public void SetRight()
        {
            if (_direction == DirectionLR.Left)
            {
                _direction = DirectionLR.Right;
                FlipH();
            }
        }

        /// <summary>
        /// 在起点处暂停
        /// </summary>
        public void PauseAtStart()
        {
            _spriteAnimation.PauseAtStart();
        }

        /// <summary>
        /// 跳转至结尾并暂停。
        /// </summary>
        public void PauseAtEnd()
        {
            _spriteAnimation.PauseAtEnd();
        }

        /// <summary>
        /// 置帧号
        /// </summary>
        /// <param name="frameIndex"></param>
        public void GotoFrame(int frameIndex)
        {
            _spriteAnimation.GotoFrame(frameIndex);
        }

        /// <summary>
        /// 取帧列表
        /// </summary>
        /// <returns></returns>
        public List<KeyFrame> GetKeyFramesList()
        {
            return _spriteAnimation.KeyFramesList;
        }

        /// <summary>
        /// 置帧段
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void SetFrameSegment(int start, int end)
        {
            _spriteAnimation.SetFrameSegment(start, end);
        }

        /// <summary>
        /// 水平翻转
        /// </summary>
        public void FlipH()
        {
            _spriteAnimation.FlipH();
        }

        /// <summary>
        /// 垂直翻转
        /// </summary>
        public void FlipV()
        {
            _spriteAnimation.FlipV();
        }

        /// <summary>
        /// 播放
        /// </summary>
        public void Start()
        {
            _spriteAnimation.Resume();
            
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            _spriteAnimation.Pause();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">屏幕横坐标</param>
        /// <param name="y">屏幕纵坐标</param>
        /// <param name="sx">横向缩放系数</param>
        /// <param name="sy">纵向缩放系数</param>
        /// <param name="angle">旋转度</param>
        public void Draw(float x = 0, float y = 0, float sx = 1, float sy = 1, float angle = 0)
        {
            _spriteAnimation.Draw(_spriteSheetImage, x, y, angle, sx, sy, _ox, _oy);
        }

        /// <summary>
        /// 取帧号
        /// </summary>
        /// <returns></returns>
        public int GetFrameIndex()
        {
            return _spriteAnimation.CurrentFrameIndex;
        }

        /// <summary>
        /// 置渲染色
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            _spriteAnimation.SetColor(color);
        }
    }

}
