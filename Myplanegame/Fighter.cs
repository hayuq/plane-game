using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Myplanegame
{
    public class Fighter
    {
        Image redImg;
        Image greenImg;
        Image yellowImg;
        public Image fighterImg;//敌机图片
        private const int FIGHTER_OFFSET = 4;//敌机图片移动速度
        public int _x = 0;
        public int _y = 0;//敌机图片移动起始的坐标
        public static List<Fighter> fighters = new List<Fighter>();//敌机对象列表
        private int fi;//敌机图片索引
        List<Image> imgList = new List<Image>();//敌机图片列表
        public bool flag = false;//碰撞的标志

        public Fighter()
        {
        }
        public Fighter(int x,int i)
        {
            _x = x;//横坐标
            fi = i;
            redImg = Resource.fighterRed;
            greenImg = Resource.fighterGreen;
            yellowImg = Resource.fighterYellow;
            switch (fi)
            {
                case 0:
                    fighterImg = redImg;break;
                case 1:
                    fighterImg = greenImg;break;
                case 2:
                    fighterImg = yellowImg;break;
                default:
                    break;
            }
            imgList.Add(redImg);
            imgList.Add(greenImg);
            imgList.Add(yellowImg);
        }

        public Point GetLoc()
        {
            Point p = new Point();
            p.X = this._x;
            p.Y = this._y;
            return p;
        }
        /// <summary>
        /// 随机产生敌机
        /// </summary>
        public static void ProduceFighter()
        {
            Random rad = new Random();
            if (rad.Next(18) == 0)
            {
                Fighter f = new Fighter(rad.Next(0, 350), rad.Next(0, 3));
                fighters.Add(f);
            }
        }
     
        /// <summary>
        /// 出现敌机
        /// </summary>
        public void FighterShow(Graphics g)
        {
           g.DrawImage(fighterImg,_x,_y);
        }

        public void fMove()
        {
            _y += FIGHTER_OFFSET;
        }

       /// <summary>
       /// 敌机移动函数
       /// </summary>
        public static void FighterMove(Graphics g)//通过定时位置让图片发生偏移
        {
            for (int i = 0; i < fighters.Count; i++)
            {
                fighters[i].FighterShow(g);
                fighters[i].fMove();
                if (fighters[i]._y > 650)
                {
                    fighters.Remove(fighters[i]);
                }
            }
        }
    }
}
