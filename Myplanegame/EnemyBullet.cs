using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Myplanegame
{
    public class EnemyBullet
    {
        private int x;
        private int y;
        private int distance;  
        
        static List<EnemyBullet> enbullist = new List<EnemyBullet>();//敌机子弹列表
        Image enbul=Resource.en_bul01;
        Point eloc;
        double k;     //斜率
        static Rectangle EnbulRect;

        //敌机子弹当前位置
        public Point Eloc
        {
            get { return eloc; }
            set { eloc = value; }
        }
       
        public EnemyBullet(int ex,int ey,int dis,int player_x,int player_y)
        {
            x = ex;
            y = ey;
            distance = dis;
            k = (1.0 * (player_x - x) / (1.0 * (player_y - y)));
            Eloc = new Point(x, y);
        }

        /// <summary>
        /// 产生敌方子弹
        /// </summary>
        public static void ProduceEnbul()
        {
            for (int i = 0; i < Fighter.fighters.Count; i++)
            {
                if (new Random().Next(10, 20) == 10)
                {
                    EnemyBullet enbul = new EnemyBullet(Fighter.fighters[i].GetLoc().X + 15, Fighter.fighters[i].GetLoc().Y + 30, new Random().Next(10, 25), MyPlane.x, MyPlane.y);
                    enbullist.Add(enbul);
                }
            }                
        }

        /// <summary>
        /// 显示敌方子弹
        /// </summary>
        /// <param name="e"></param>
        public void ShowEnbul(Graphics e)
        {
            e.DrawImage(enbul,Eloc);
        }
        
        public void Move()
        {
            eloc.Y += distance;
            eloc.X += (int)(distance * k);
        }

        /// <summary>
        /// 敌方子弹移动
        /// </summary>
        /// <param name="e"></param>
        public static void MoveEnbul(Graphics e)
        {
            for (int i = 0; i < enbullist.Count; i++)
            {
                enbullist[i].ShowEnbul(e);
                enbullist[i].Move();
                if (enbullist[i].Eloc.Y > 700 || enbullist[i].Eloc.X < 0 || enbullist[i].Eloc.X > 420)
                {
                    enbullist.Remove(enbullist[i]);
                }
            }
         
        }
        /// <summary>
        /// 我方飞机碰撞检测方法
        /// </summary>
        public static void HitPlane(Graphics g)
        {
            for (int i = 0; i < enbullist.Count; i++)
            {
                EnbulRect = new Rectangle(enbullist[i].Eloc.X, enbullist[i].Eloc.Y, 6, 6);
                Rectangle MyplaneRect = new Rectangle(MyPlane.x,MyPlane.y, 40, 50);
                if (EnbulRect.IntersectsWith(MyplaneRect)) //我方飞机被敌方子弹击中
                {
                    enbullist.Remove(enbullist[i]);
                    MyPlane.health -= 1;
                    if (MyPlane.score > 0)
                    {
                        MyPlane.score -= 1;
                    }
                }
            }
        }
    }
}
