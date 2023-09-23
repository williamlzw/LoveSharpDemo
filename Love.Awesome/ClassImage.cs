using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Love;

namespace Love.Awesome
{
    public class ClassImage
    {
        private Love.Image _image;
        private float _ox = 0;
        private float _oy = 0;

        public float Width
        {
            get
            {
                return _image.GetWidth();
            }
        }

        public float Height
        {
            get
            {
                return _image.GetHeight();
            }
        }

        /// <summary>
        /// 整图载入
        /// </summary>
        /// <param name="imagePath">图像路径</param>
        public ClassImage(string imagePath, WrapMode wraphoriz_type = WrapMode.Repeat, WrapMode wrapvert_type = WrapMode.Repeat)
        {
            _image = Graphics.NewImage(imagePath);
            _image.SetWrap(wraphoriz_type, wrapvert_type);
        }

        /// <summary>
        /// 整图载入
        /// </summary>
        /// <param name="image"></param>
        public ClassImage(Image image, WrapMode wraphoriz_type = WrapMode.Repeat, WrapMode wrapvert_type = WrapMode.Repeat)
        {
            _image = image;
            _image.SetWrap(wraphoriz_type, wrapvert_type);
        }

        /// <summary>
        /// 整图载入
        /// </summary>
        /// <param name="image"></param>
        public ClassImage(ImageData imageData, WrapMode wraphoriz_type = WrapMode.Repeat, WrapMode wrapvert_type = WrapMode.Repeat)
        {
            _image = Graphics.NewImage(imageData);
            _image.SetWrap(wraphoriz_type, wrapvert_type);
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

        /// <summary>
        /// 置渲染色
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            Graphics.SetColor(color);
        }

        /// <summary>
        /// 置混合模式
        /// </summary>
        /// <param name="blendMode"></param>
        /// <param name="blendAlphaMode"></param>
        public void SetBlendMode(BlendMode blendMode, BlendAlphaMode blendAlphaMode = BlendAlphaMode.Multiply)
        {
            Graphics.SetBlendMode(blendMode, blendAlphaMode);
        }

        /// <summary>
        /// 高级渲染
        /// </summary>
        /// <param name="x">屏幕横坐标</param>
        /// <param name="y">屏幕纵坐标</param>
        /// <param name="sx">横向缩放系数</param>
        /// <param name="sy">纵向缩放系数</param>
        /// <param name="angle">旋转角度</param>
        public void DrawEx(float x = 0, float y = 0, float sx = 1, float sy = 1, float angle = 0)
        {
            Graphics.Draw(_image, x, y, angle, sx, sy, _ox, _oy);
        }

        /// <summary>
        /// 渲染
        /// </summary>
        /// <param name="x">屏幕横坐标</param>
        /// <param name="y">屏幕纵坐标</param>
        public void Draw(float x = 0, float y = 0)
        {
            Graphics.Draw(_image, x, y);
        }

        /// <summary>
        /// 拉伸渲染
        /// </summary>
        /// <param name="left">左边</param>
        /// <param name="top">顶边</param>
        /// <param name="right">右边</param>
        /// <param name="bottom">底边</param>
        public void DrawStretch(float left, float top, float right, float bottom) 
        {
            var scaleX = (float)(right - left) / this.Width;
            var scaleY = (float)(bottom - top) / this.Height;
            Graphics.Draw(_image, left, top, 0, scaleX, scaleY);
        }

        /// <summary>
        /// 重复模式渲染
        /// </summary>
        /// <param name="quad"></param>
        /// <param name="x">屏幕横坐标</param>
        /// <param name="y">屏幕纵坐标</param>
        public void DrawQuad(Quad quad, float x = 0, float y = 0)
        {
            Graphics.Draw(quad, _image, x, y);
        }
    }
}
