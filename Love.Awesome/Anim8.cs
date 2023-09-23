using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Love.Awesome
{
    // ProjectURL => @"https://github.com/kikito/anim8";
    // Author => "kikito";
    // Version => "anim8 v2.3.1";

    /// <summary>
    /// 原始时间轴帧点数据。
    /// </summary>
    public struct RawPointWithTime
    {
        /// <summary>
        /// 位于精灵图的位置。
        /// </summary>
        public Point Point;

        /// <summary>
        /// 维持时间。
        /// </summary>
        public float Duration;

        /// <summary>
        /// 创建时间轴帧点。
        /// </summary>
        /// <param name="x">精灵图的 x 下标，从 1 开始。</param>
        /// <param name="y">精灵图的 y 下标，从 1 开始。</param>
        /// <param name="duration">维持时间。</param>
        public RawPointWithTime(int x, int y, float duration)
        {
            Point = new Point(x, y);
            Duration = duration;
        }


    }

    /// <summary>
    /// 原始节点数据列表对象。
    /// </summary>
    public class RawPointWithTimeList : IEnumerable<RawPointWithTime>
    {
        /// <summary>
        /// 精灵图解析时的读取顺序，默认行优先。
        /// </summary>
        public enum OrientationOrder
        {
            /// <summary>
            /// 行优先。
            /// </summary>
            HorizontalFirst,

            /// <summary>
            /// 列优先。
            /// </summary>
            VerticalFirst
        }

        private List<RawPointWithTime> _list = new List<RawPointWithTime>();


        private bool IsInRange(int bound1, int bound2, int value)
        {
            var max = Mathf.Max(bound1, bound2);
            var min = Mathf.Min(bound1, bound1);
            if (value > max) return false;
            if (value < min) return false;
            return true;
        }

        /// <summary>
        /// 添加区域精灵图。
        /// </summary>
        /// <param name="xfrom">行精灵截取开始索引</param>
        /// <param name="xto">行精灵截取结束索引</param>
        /// <param name="yfrom">列精灵截取开始索引</param>
        /// <param name="yto">列精灵截取结束索引</param>
        /// <param name="duration">持续时间</param>
        /// <param name="orientation">优先顺序类型</param>
        /// <returns></returns>
        public RawPointWithTimeList AddRange(int xfrom, int xto, int yfrom, int yto, float duration, OrientationOrder orientation = OrientationOrder.HorizontalFirst)
        {
            var istep = 1;
            var jstep = 1;

            if (yto - yfrom < 0) istep = -1;
            if (xto - xfrom < 0) jstep = -1;

            switch (orientation)
            {
                case OrientationOrder.HorizontalFirst:
                    {
                        for (int i = yfrom; IsInRange(yfrom, yto, i); i += istep)
                        {
                            for (int j = xfrom; IsInRange(xfrom, xto, j); j += jstep)
                            {
                                Add(j, i, duration);
                            }
                        }
                    }
                    break;
                case OrientationOrder.VerticalFirst:
                    {
                        for (int j = xfrom; IsInRange(xfrom, xto, j); j += jstep)
                        {
                            for (int i = yfrom; IsInRange(yfrom, yto, i); i += istep)
                            {
                                Add(j, i, duration);
                            }

                        }

                    }
                    break;
                default:
                    break;
            }

            return this;
        }

        /// <summary>
        /// 添加指定行的连续格点
        /// </summary>
        /// <param name="xfrom">起始列</param>
        /// <param name="xto">截止列</param>
        /// <param name="y">第几行</param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public RawPointWithTimeList AddRow(int xfrom, int xto, int y, float duration)
        {
            return AddRange(xfrom, xto, y, y, duration);
        }

        /// <summary>
        /// 添加指定列的连续格点
        /// </summary>
        /// <param name="x"></param>
        /// <param name="yfrom"></param>
        /// <param name="yto"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public RawPointWithTimeList AddColumn(int x, int yfrom, int yto, float duration)
        {
            return AddRange(x, x, yfrom, yto, duration, OrientationOrder.VerticalFirst);
        }

        public RawPointWithTimeList Add(int x, int y, float duration)
        {
            _list.Add(new RawPointWithTime(x, y, duration));
            return this;
        }

        public IEnumerator<RawPointWithTime> GetEnumerator()
        {
            return ((IEnumerable<RawPointWithTime>)_list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_list).GetEnumerator();
        }
    }


    /// <summary>
    /// 网格。
    /// </summary>
    public struct Grid
    {
        private Dictionary<Point, Quad> _keyFrames;

        /// <summary>
        /// 帧宽度。
        /// </summary>
        public int FrameWidth { get; private set; }

        /// <summary>
        /// 帧高度。
        /// </summary>
        public int FrameHeight { get; private set; }

        /// <summary>
        /// 图片宽度。
        /// </summary>
        public int ImageWidth { get; private set; }

        /// <summary>
        /// 图片高度。
        /// </summary>
        public int ImageHeight { get; private set; }

        /// <summary>
        /// 图片裁剪的左偏移量。
        /// </summary>
        public int Left { get; private set; }

        /// <summary>
        /// 图片裁剪的上偏移量，
        /// </summary>
        public int Top { get; private set; }

        /// <summary>
        /// 线框大小。
        /// </summary>
        public int Border { get; private set; }


        /// <summary>
        /// 创建网格对象，网格对象是用来制定精灵表最小帧的尺寸的窗口。
        /// </summary>
        /// <param name="frameWidth">帧宽度。</param>
        /// <param name="frameHeight">帧高度。</param>
        /// <param name="imageWidth">图片宽度。</param>
        /// <param name="imageHeight">图片高度。</param>
        /// <param name="left">横向偏移量。如果您的精灵图不太规整，您就可以用到这个。</param>
        /// <param name="top">纵向偏移量。如果您的精灵图不太规整，您就可以用到这个。</param>
        /// <param name="border">边框线宽，如果您的精灵图存在边框，您可以使用这个减掉。</param>
        public Grid(int frameWidth, int frameHeight, int imageWidth, int imageHeight, int left = 0, int top = 0, int border = 0)
        {
            _keyFrames = new Dictionary<Point, Quad>();
            ImageWidth = imageWidth;
            ImageHeight = imageHeight;
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
            Left = left;
            Top = top;
            Border = border;
        }

        private Love.Quad CreateFrame(int x, int y)
        {
            var point = new Point(x, y);
            if (_keyFrames.ContainsKey(point)) return _keyFrames[point];

            var frameWidth = this.FrameWidth;
            var frameHeight = this.FrameHeight;
            var frameLeft = this.Left + (x - 1) * frameWidth + x * this.Border;
            var frameTop = this.Top + (y - 1) * frameHeight + y * this.Border;

            return Love.Graphics.NewQuad(frameLeft, frameTop, frameWidth, FrameHeight, ImageWidth, ImageHeight);
        }


        /// <summary>
        /// <para>
        /// 创建关键帧序列，推荐使用 new <see cref="RawPointWithTimeList"/>() 创建原始节点数据集合，并将对象传入其中。</para>
        /// <para>
        /// 示例如下：
        /// <code>
        /// var grid = new Anim8NET.Grid(64, 64, image.GetWidth(), image.GetHeight()); // image 为传入的图片对象
        /// var rawPoints = new RawPointWithTimeList()
        ///     .AddRange(1, 10, 1, 4, 0.1f)
        ///     .AddRow(1, 3, 5, 0.1f);
        /// grid.CreateFrames(rawPoints);
        /// </code>
        /// </para>
        /// </summary>
        /// <param name="pointWithTimes"></param>
        /// <returns></returns>
        public IEnumerable<KeyFrame> CreateFrames(IEnumerable<RawPointWithTime> pointWithTimes)
        {
            var frames = new List<KeyFrame>();
            foreach (var point in pointWithTimes)
            {
                frames.Add(new KeyFrame()
                {
                    Duration = point.Duration,
                    Frame = CreateFrame(point.Point.X, point.Point.Y)
                });
            }
            return frames;
        }

    }


    /// <summary>
    /// 您不能通过 new 创建该类型实例，您需要使用<see cref="Grid.CreateFrames(IEnumerable{RawPointWithTime})"/>创建关键帧列表。
    /// </summary>
    public class KeyFrame
    {

        /// <summary>
        /// 关键帧的裁剪信息。
        /// </summary>
        public Love.Quad Frame { get; internal set; }

        /// <summary>
        /// 当前帧点的维持时间。
        /// </summary>
        public float Duration { get; internal set; }


        internal float Interval;
        internal List<KeyFrame> Parent;


        /// <summary>
        /// 获得在集合中的下标。
        /// </summary>
        /// <returns></returns>
        public int GetIndex()
        {
            if (Parent is null) return -1;
            return Parent.IndexOf(this);
        }

        internal KeyFrame()
        {
        }
    }

    /// <summary>
    /// 精灵动画对象。
    /// 用法如下：
    /// <code>
    /// var grid = new Grid(72, 67, image.GetWidth(), image.GetHeight());
    /// 
    /// var rawPoints = new RawPointWidthTimeList()
    ///     .AddRange(1, 10, 1, 4, 0.1f)
    ///     .AddRow(1, 3, 5, 0.1f);
    ///
    /// var keyFrames = grid.CreateFrames(rawPoints);
    ///
    /// animation = new SpriteAnimation(keyFrames);
    /// </code>
    /// </summary>
    public class SpriteAnimation
    {
        /// <summary>
        /// 动画状态。
        /// </summary>
        public enum AnimationStatus
        {
            /// <summary>
            /// 播放。
            /// </summary>
            Playing,

            /// <summary>
            /// 暂停。
            /// </summary>
            Pause
        }

        //Frame[] Frames;
        //float[] Durations;
        //float[] Intervals;
        private List<KeyFrame> KeyFrames;

        private List<KeyFrame> OldKeyFrames;

        /// <summary>
        /// 总时长，这是由传入的 keyFrames 的 Duration 求和得到的，您不能更改。
        /// </summary>
        public float TotalDuration => _totalDuration;
        private float _totalDuration;

        /// <summary>
        /// 每次循环可触发的事件，在<see cref="Update(float)"/> 更新 <see cref="CurrentKeyFrame"/> 前触发，您可以当成一个外挂的 Update 事件。
        /// </summary>
        public event EventHandler<float> OnLoop;

        /// <summary>
        /// 时间进度，表示时间，并不是百分比，原版叫做 timer, 这边为了统一叫做 Progress 进度，如果您想要百分比请参看 <see cref="Rate"/>。
        /// </summary>
        public float Progress { get; private set; }

        /// <summary>
        /// 进度百分比，如果您想要查看绝对时间，请参看 <see cref=" Progress"/>。
        /// </summary>
        public float Rate => Progress / TotalDuration;

        /// <summary>
        /// 当前帧信息。在 <see cref="Update(float)"/> 执行后更新。
        /// </summary>
        public KeyFrame CurrentKeyFrame { get; private set; }

        public int CurrentFrameIndex { get; private set; }

        public List<KeyFrame> KeyFramesList => OldKeyFrames;

        /// <summary>
        /// 动画的播放状态。
        /// </summary>
        public AnimationStatus Status { get; private set; }

        /// <summary>
        /// 横向反转。
        /// </summary>
        public bool FlippedH { get; set; }


        /// <summary>
        /// 纵向反转。
        /// </summary>
        public bool FlippedV { get; set; }

        /// <summary>
        /// 创建精灵动画。用法：animation = new SpriteAnimation(keyFrames) keyFrames 来自 Grid 实例的方法 Grid.CreateFrames()。
        /// </summary>
        /// <param name="keyFrames">帧列表信息。</param>
        public SpriteAnimation(IEnumerable<KeyFrame> keyFrames)
        {
            KeyFrames = new List<KeyFrame>(keyFrames);

            foreach (var item in KeyFrames)
            {
                item.Parent = KeyFrames;
            }
            OldKeyFrames = KeyFrames;
            ParseIntervals(keyFrames, out _totalDuration);
        }



        /// <summary>
        /// 创建精灵动画。用法：animation = new SpriteAnimation(grid, rawKeyFrames) 这里推荐使用 <see cref="RawPointWithTimeList"/> 创建。
        /// 示例：
        /// <code>
        /// animation = new SpriteAnimation(
        ///     grid: new Grid(72, 67, image.GetWidth(), image.GetHeight()),
        ///     rawPointWithTimeList: new RawPointWithTimeList()
        ///         .AddRange(1, 10, 1, 4, 0.1f)
        ///         .AddRow(1, 3, 5, 0.1f));
        /// </code>
        /// </summary>
        /// <param name="grid">单独格点对象。</param>
        /// <param name="rawPointWithTimeList">推荐传入 <see cref="RawPointWithTimeList"/> 对象。您可以使用 
        /// <code> new RawPointWithTimeList().AddXXX(...)</code>
        /// 
        /// 具体地，您可以这样写：
        /// <code>
        /// animation = new SpriteAnimation(
        ///     grid: grid,
        ///     rawPointWithTimeList: new RawPointWithTimeList()
        ///         .AddRange(1, 10, 1, 4, 0.1f)
        ///         .AddRow(1, 3, 5, 0.1f));
        /// </code>
        /// 
        /// </param>
        public SpriteAnimation(Grid grid, IEnumerable<RawPointWithTime> rawPointWithTimeList) : this(grid.CreateFrames(rawPointWithTimeList))
        {

        }


        public void SetFrameSegment(int start, int end)
        {
            List<KeyFrame> newKeyFrames = new List<KeyFrame>();
            for(int i = start; i < end; i++)
            {
                newKeyFrames.Add(OldKeyFrames[i]);
            }
            KeyFrames = newKeyFrames;
            ParseIntervals(KeyFrames, out _totalDuration);
        }

        private KeyFrame SeekKeyFrame(float timer)
        {
            if (KeyFrames.Count <= 0) return null;
            if (KeyFrames.Count == 1) return KeyFrames[0];

            int low = 0, high = KeyFrames.Count - 1;
            int i = high;
            while (low <= high)
            {
                i = (low + high) / 2;

                if (timer >= KeyFrames[Mathf.Min(i + 1, high)].Interval) low = i + 1;
                else if (timer < KeyFrames[i].Interval) high = i - 1;
                else return KeyFrames[i];

            }
            
            return KeyFrames[i];
        }

        private void ParseIntervals(IEnumerable<KeyFrame> keyFrames, out float totalDuration)
        {
            var sumTime = 0f;
            foreach (var keyFrame in keyFrames)
            {
                keyFrame.Interval = sumTime;
                sumTime += keyFrame.Duration;
            }
            totalDuration = sumTime;
        }

        /// <summary>
        /// 更新。
        /// </summary>
        /// <param name="dt">时间差。</param>
        public void Update(float dt)
        {
            if (Status == AnimationStatus.Pause) return;
            Progress += dt;
            var loops = Mathf.FloorToInt(Progress / TotalDuration);


            if (loops != 0)
            {
                Progress -= TotalDuration * loops;
                OnLoop?.Invoke(this, dt);
            }

            CurrentKeyFrame = SeekKeyFrame(Progress);
            CurrentFrameIndex = CurrentKeyFrame.GetIndex();
        }


        /// <summary>
        /// 暂停当前动画。
        /// </summary>
        public void Pause()
        {
            Status = AnimationStatus.Pause;
        }


        /// <summary>
        /// 直接跳转到指定 id 的帧。
        /// </summary>
        /// <param name="frameIndex"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void GotoFrame(int frameIndex)
        {
            if (frameIndex < 0 || KeyFrames.Count <= frameIndex) throw new ArgumentOutOfRangeException("下标超出数组范围！");
            var targetFrame = KeyFrames[frameIndex];
            CurrentFrameIndex = frameIndex;
            Progress = targetFrame.Interval;
        }

        /// <summary>
        /// 跳转至结尾并暂停。
        /// </summary>
        public void PauseAtEnd()
        {
            CurrentKeyFrame = KeyFrames[KeyFrames.Count - 1];
            Progress = TotalDuration;
            CurrentFrameIndex = KeyFrames.Count - 1;
            Pause();
        }

        /// <summary>
        /// 水平翻转。
        /// </summary>
        public void FlipH()
        {
            FlippedH = !FlippedH;
        }

        /// <summary>
        /// 垂直翻转。
        /// </summary>
        public void FlipV()
        {
            FlippedV = !FlippedV;
        }

        /// <summary>
        /// 在起点处暂停。
        /// </summary>
        public void PauseAtStart()
        {
            CurrentFrameIndex = 0;
            CurrentKeyFrame = KeyFrames[0];
            Progress = 0;
            Pause();
        }

        /// <summary>
        /// 播放。
        /// </summary>
        public void Resume()
        {
            Status = AnimationStatus.Playing;
            CurrentKeyFrame = SeekKeyFrame(0);
            CurrentFrameIndex = CurrentKeyFrame.GetIndex();
        }

        /// <summary>
        /// 绘制。
        /// </summary>
        /// <param name="image">精灵表图。</param>
        /// <param name="x">横向偏移</param>
        /// <param name="y">纵向偏移</param>
        /// <param name="r">角度</param>
        /// <param name="sx">横向缩放系数</param>
        /// <param name="sy">纵向缩放系数</param>
        /// <param name="ox">横向偏移值，如果动画被翻转，宽度或高度会被适当减去</param>
        /// <param name="oy">纵向偏移值，如果动画被翻转，宽度或高度会被适当减去</param>
        /// <param name="kx">横向剪切系数，随翻转状态而变化</param>
        /// <param name="ky">纵向剪切系数，随翻转状态而变化</param>
        public void Draw(Love.Image image, float x = 0, float y = 0, float r = 0, float sx = 1, float sy = 1, float ox = 0, float oy = 0, float kx = 0, float ky = 0)
        {
            var frameInfo = GetFrameInfo(x, y, r, sx, sy, ox, oy, kx, ky);
            Graphics.Draw(frameInfo.frame, image, frameInfo.x, frameInfo.y, frameInfo.r, frameInfo.sx, frameInfo.sy, frameInfo.ox, frameInfo.oy, frameInfo.kx, frameInfo.ky);
        }


        public void DrawDebug(Love.Image image, float x = 0, float y = 0, float r = 0, float sx = 1, float sy = 1, float ox = 0, float oy = 0, float kx = 0, float ky = 0)
        {
            var frameInfo = GetFrameInfo(x, y, r, sx, sy, ox, oy, kx, ky);
            Graphics.Reset();
            Graphics.Draw(frameInfo.frame, image, frameInfo.x, frameInfo.y, frameInfo.r, frameInfo.sx, frameInfo.sy, frameInfo.ox, frameInfo.oy, frameInfo.kx, frameInfo.ky);
            var index = CurrentKeyFrame.GetIndex();
            Graphics.Print(index.ToString(), frameInfo.x, frameInfo.y, 0, frameInfo.sx, frameInfo.sy);
            Graphics.SetLineWidth(2);
            Graphics.SetColor(Color.Aqua);
            Graphics.Rectangle(DrawMode.Line, frameInfo.x, frameInfo.y, frameInfo.w * frameInfo.sx, frameInfo.h * frameInfo.sy);
            Graphics.SetColor(Color.Yellow);
            Graphics.Rectangle(DrawMode.Fill, frameInfo.x, frameInfo.y + (frameInfo.h - 10) * frameInfo.sy, frameInfo.sx * frameInfo.w * Rate, frameInfo.sy * 10);
        }

        public void DrawDebug(float x = 0, float y = 0, float r = 0, float sx = 1, float sy = 1, float ox = 0, float oy = 0, float kx = 0, float ky = 0)
        {
            var frameInfo = GetFrameInfo(x, y, r, sx, sy, ox, oy, kx, ky);
            var index = CurrentKeyFrame.GetIndex();
            Graphics.Print(index.ToString(), frameInfo.x, frameInfo.y, 0, frameInfo.sx, frameInfo.sy);
            Graphics.SetLineWidth(2);
            Graphics.SetColor(Color.Aqua);
            Graphics.Rectangle(DrawMode.Line, frameInfo.x, frameInfo.y, frameInfo.w * frameInfo.sx, frameInfo.h * frameInfo.sy);
            Graphics.SetColor(Color.Yellow);
            Graphics.Rectangle(DrawMode.Fill, frameInfo.x, frameInfo.y + (frameInfo.h - 10) * frameInfo.sy, frameInfo.sx * frameInfo.w * Rate, frameInfo.sy * 10);
        }

        private struct FrameInfo
        {
            public Love.Quad frame;
            public float x, y, r, sx, sy, ox, oy, kx, ky;
            public float w, h;
            public FrameInfo(Love.Quad frame, float x = 0, float y = 0, float r = 0, float sx = 1, float sy = 1, float ox = 0, float oy = 0, float kx = 0, float ky = 0)
            {
                this.frame = frame;
                this.x = x;
                this.y = y;
                this.r = r;
                this.sx = sx;
                this.sy = sy;
                this.ox = ox;
                this.oy = oy;
                this.kx = kx;
                this.ky = ky;
                this.w = 0;
                this.h = 0;
            }
        }

        private FrameInfo GetFrameInfo(float x = 0, float y = 0, float r = 0, float sx = 1, float sy = 1, float ox = 0, float oy = 0, float kx = 0, float ky = 0)
        {
            var frame = CurrentKeyFrame;
            var viewPort = frame.Frame.GetViewport();
            var w = viewPort.Width;
            var h = viewPort.Height;
            if (FlippedH || FlippedV)
            {

                if (FlippedH)
                {
                    sx = sx * -1;
                    ox = w - ox;
                    kx = kx * -1;
                    ky = ky * -1;
                }
                if (FlippedV)
                {
                    sy = sy * -1;
                    oy = h - oy;
                    kx = kx * -1;
                    ky = ky * -1;
                }

            }
            return new FrameInfo(frame.Frame, x, y, r, sx, sy, ox, oy, kx, ky) { w = w, h = h };
        }

        /// <summary>
        /// 置渲染色
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            Graphics.SetColor(color);
        }
    }
}