using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Love;

namespace Love.Awesome
{
    public class ClassTiledMap
    {
        private int m_totalRow;
        private int m_totalColumn;
        private int m_blockRow;
        private int m_blockColumn;
        private int m_blockWidth;
        private int m_blockHeight;
        private int m_renderRow;
        private int m_renderColumn;
        private int m_renderRowIndex;
        private int m_renderColumnIndex;
        private int[,] m_blockList;
        private bool[,] m_obstacleList;
        private Love.ImageData m_image;
        private Love.Size m_screenSize;
        private Love.Size m_mapSize;
        private Love.Point m_offsetMap;
        private Love.Point m_offsetBlock;
        Love.Point staticPosition;

        public void Open(string path, int screenWidth, int screenHeight)
        {
            XDocument xDoc = XDocument.Load(path);
            var xMap = xDoc.Element("map");
            m_totalRow = (int)xMap.Attribute("height");
            m_totalColumn = (int) xMap.Attribute("width");
            m_blockWidth = (int)xMap.Attribute("tilewidth");
            m_blockHeight = (int)xMap.Attribute("tileheight");
            m_screenSize = new Love.Size(screenWidth, screenHeight);
            m_mapSize = new Love.Size(m_totalRow * m_blockHeight, m_totalColumn * m_blockWidth);
            m_blockList = new int[m_totalRow, m_totalColumn];
            m_obstacleList = new bool[m_totalRow, m_totalColumn];
            bool isObstacle = false;
            m_renderRow = m_screenSize.Height / m_blockHeight + 1;
            if (m_screenSize.Height % m_blockHeight !=0)
            {
                m_renderRow += 1;
            }
            m_renderColumn = m_screenSize.Width / m_blockWidth + 1;
            if (m_screenSize.Width % m_blockWidth != 0)
            {
                m_renderColumn += 1;
            }
            
            string imageRoot = System.Environment.CurrentDirectory;
            foreach (var index in xMap.Elements())
            {
                if(index.Name == "properties")
                {

                }
                else if (index.Name == "tileset")
                {
                    m_blockRow = (int)index.Element("image").Attribute("width") / m_blockHeight;
                    m_blockColumn = (int)index.Element("image").Attribute("height") / m_blockWidth;
                    var imagePath = imageRoot + "/" + index.Element("image").Attribute("source").Value;
                    m_image = Image.NewImageData(imagePath);
                }
                else if (index.Name == "layer")
                {
                    isObstacle = index.Attribute("name").Value == "forbid";
                    var blockStr = index.Element("data").Value;
                    var stringArr = blockStr.Split(',');
                    if(stringArr.Length != m_totalColumn * m_totalRow) 
                    {
                        return;
                    }
                    if(isObstacle)
                    {
                        for(int rowIndex = 0; rowIndex < m_totalRow; rowIndex++)
                        {
                            for(int colIndex = 0; colIndex < m_totalColumn; colIndex++)
                            {
                                m_obstacleList[rowIndex, colIndex] = stringArr[rowIndex * m_totalColumn + colIndex] != "0";
                            }
                        }
                    }
                    else
                    {
                        for (int rowIndex = 0; rowIndex < m_totalRow; rowIndex++)
                        {
                            for (int colIndex = 0; colIndex < m_totalColumn; colIndex++)
                            {
                                m_blockList[rowIndex, colIndex] = int.Parse(stringArr[rowIndex * m_totalColumn + colIndex]) - 1;
                            }
                        }
                    }
                }
            }
        }

        private static Love.Point CalculateViewOffset(Love.Point player, Love.Size screenSize, Love.Size mapSize)
        {
            int num = screenSize.Width / 2;
            int num2 = screenSize.Height / 2;
            int num3 = player.X - num;
            int num4 = player.Y - num2;
            if (num3 < 0)
            {
                num3 = 0;
            }
            else if (num3 > (int)(mapSize.Width - screenSize.Width))
            {
                num3 = mapSize.Width - screenSize.Width;
            }

            if (num4 < 0)
            {
                num4 = 0;
            }
            else if (num4 > (int)(mapSize.Height - screenSize.Height))
            {
                num4 = mapSize.Height - screenSize.Height;
            }
            return new Love.Point(0 - (int)num3, 0 - (int)num4);
        }

        public Love.Point Update(ref Love.Point position)
        {
            var rowIndex = position.Y / m_blockHeight;
            if( position.Y % m_blockHeight != 0)
            {
                rowIndex += 1;
            }
            var colIndex = position.X / m_blockWidth;
            if (position.X % m_blockWidth != 0)
            {
                colIndex += 1;
            }
            
            if (m_obstacleList[rowIndex - 1, colIndex - 1])
            {
                position = staticPosition;
                return CalculateViewOffset(position, m_screenSize, m_mapSize);
            }
            
            staticPosition = position;
            m_offsetMap = CalculateViewOffset(position, m_screenSize, m_mapSize);
            m_renderColumnIndex = m_offsetMap.X / - m_blockWidth + 1;
            if(m_renderColumnIndex > m_totalColumn - m_renderColumn + 1)
            {
                m_renderColumnIndex = m_totalColumn - m_renderColumn + 1;
                m_offsetBlock.X = m_offsetMap.X % m_blockWidth - m_blockWidth;
            }
            else
            {
                m_offsetBlock.X = m_offsetMap.X % m_blockWidth;
            }

            m_renderRowIndex = m_offsetMap.Y / -m_blockHeight + 1;
            if (m_renderRowIndex > m_totalRow - m_renderRow + 1)
            {
                m_renderRowIndex = m_totalRow - m_renderRow + 1;
                m_offsetBlock.Y = m_offsetMap.Y % m_blockHeight - m_blockHeight;
            }
            else
            {
                m_offsetBlock.Y = m_offsetMap.Y % m_blockHeight;
            }
            return m_offsetMap;
        }

        public void Draw()
        {
            for(int rowIndex = 0; rowIndex < m_renderRow; rowIndex++)
            {
                for(int colIndex = 0; colIndex < m_renderColumn; colIndex++)
                {
                    var imageData = Image.NewImageData(m_blockWidth, m_blockHeight);
                    var index = m_blockList[rowIndex + m_renderRowIndex - 1, colIndex + m_renderColumnIndex - 1];
                    imageData.Paste(m_image, 0, 0, index % m_blockColumn * m_blockWidth, index / m_blockColumn * m_blockHeight, m_blockWidth, m_blockHeight);
                    var image = Graphics.NewImage(imageData);
                    Graphics.Draw(image, colIndex * m_blockWidth + m_offsetBlock.X, rowIndex * m_blockHeight + m_offsetBlock.Y);
                    imageData.Dispose();
                    image.Dispose();
                }
            }
        }
    }
}
