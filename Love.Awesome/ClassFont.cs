using Love;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Love.Awesome
{
    public class ClassFont
    {
        private Love.Font _font;

        public ClassFont(string fileName, int fontSize)
        {
            _font = Graphics.NewFont(fileName, fontSize);
        }

        public ClassFont(int fontSize)
        {
            _font = Graphics.NewFont(fontSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str">文本</param>
        /// <param name="x">屏幕横坐标</param>
        /// <param name="y">屏幕纵坐标</param>
        /// <param name="strColor">文本颜色</param>
        public void Draw(string str, float x = 0, float y = 0, Vector4 strColor = default(Vector4))
        {
            var clArr = new ColoredStringArray(ColoredString.Create(str, strColor));
            var text = Graphics.NewText(_font, clArr);
            Graphics.Draw(text, x, y);
        }
    }
}
