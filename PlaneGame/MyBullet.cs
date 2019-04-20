using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Media;

namespace Myplanegame
{
    public class MyBullet
    {
        private int x;//子弹横坐标
        private int y;//子弹纵坐标
        private const int BULLET_OFFSET = 18;//移动速度
        public int Angle;//子弹角度
        private Image bulImg;//定义子弹图片
        private const double PI = Math.PI;
        public static List<MyBullet> mybulList = new List<MyBullet>();//子弹对象集合

        static Bitmap bm = new Bitmap(Resource.bomb4);//爆炸图片
        public bool isHit = false;//碰撞的标志
        public MyBullet()
        { 
        }

         public MyBullet(int bx, int by, int angle)
        {
            x = bx;
            y = by;
            Angle = angle;
            switch (Angle)
            {
                case 0:
                    bulImg = Resource.bul02;
                    y -= 17;
                    break;
                case 30:
                    bulImg = Resource.bul02_30;
                    x += 12;
                    y -= 12;
                    break;
                case 60:
                    bulImg = Resource.bul02_60;
                    x += 2;
                    y -= 17;
                    break;
                case 120:
                    bulImg = Resource.bul02_120;
                    x -= 35;
                    y -= 12;
                    break;
                case 150:
                    bulImg = Resource.bul02_150;
                    x -= 20;
                    y -= 12;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 通过按键盘J键来产生我方子弹
        /// </summary>
        public static void ProduceMybul()
        {
            if (!MyPlane.isGameOver && MyPlane.IsKeyDown(Keys.J))
            {
                mybulList.Add(new MyBullet(MyPlane.x + 13, MyPlane.y - 10, 0));
                if (MyPlane.isGetGun)
                {
                    mybulList.Add(new MyBullet(MyPlane.x + 13, MyPlane.y - 8, 60));
                    mybulList.Add(new MyBullet(MyPlane.x + 7, MyPlane.y - 8, 30));
                    mybulList.Add(new MyBullet(MyPlane.x + 30, MyPlane.y - 12, 120));
                    mybulList.Add(new MyBullet(MyPlane.x, MyPlane.y - 7, 150));
                }
            }
        }

        /// <summary>
        /// 显示我方子弹
        /// </summary>
        /// <param name="e"></param>
        public void ShowMybul(Graphics e)
        {
            e.DrawImage(bulImg, new Point(x, y));
        }
       
        /// <summary>
        /// 我方子弹移动
        /// </summary>
        /// <param name="g"></param>
        public static void MoveMybul(Graphics g)
        {
            for(int i = 0;i < mybulList.Count; i++)
            {
                mybulList[i].ShowMybul(g);
                switch (mybulList[i].Angle)
                {
                    case 0:
                        mybulList[i].y -= BULLET_OFFSET;
                        break;
                    case 60:
                        mybulList[i].x += (int)(BULLET_OFFSET / 2);
                        mybulList[i].y -= (int)(BULLET_OFFSET * Math.Cos(PI / 6));
                        break;
                    case 30:
                        mybulList[i].x += (int)(BULLET_OFFSET * Math.Cos(PI / 6));
                        mybulList[i].y -= (int)(BULLET_OFFSET / 2);
                        break;
                    case 120:
                        mybulList[i].x -= (int)(BULLET_OFFSET / 2);
                        mybulList[i].y -= (int)(BULLET_OFFSET * Math.Cos(PI / 6));
                        break;
                    case 150:
                        mybulList[i].x -= (int)(BULLET_OFFSET * Math.Cos(PI / 6));
                        mybulList[i].y -= (int)(BULLET_OFFSET / 2);
                        break;
                }
                if (mybulList[i].y < 0 || mybulList[i].x > 415 || mybulList[i].x < 0)
                {
                    mybulList.Remove(mybulList[i]);
                }
            }
        }
        
        /// <summary>
        /// 敌机碰撞检测方法
        /// </summary>
         public static void IsHitEnemy(Graphics g)
        {
            Rectangle myPlaneRect = new Rectangle(MyPlane.x, MyPlane.y, MyPlane.myPlaneImg.Width, MyPlane.myPlaneImg.Height);    //包住myplane的Rectangle

            //g.DrawRectangle(new Pen(Color.Red), myPlaneRect);
            for(int i = 0; i < mybulList.Count; i++)
              for (int j = 0; j < Fighter.fighters.Count; j++)
              { 
                  Rectangle mybulRect = new Rectangle(mybulList[i].x, mybulList[i].y, 8, 10);
                  Rectangle fighterRect = new Rectangle(Fighter.fighters[j]._x, Fighter.fighters[j]._y, 65, 45);
                  //g.DrawRectangle(new Pen(Color.Black), fighterRect);
                  if (mybulRect.IntersectsWith(fighterRect))   //我方子弹击中敌机，敌机爆炸
                  {
                     mybulList.Remove(mybulList[i]);
                     Fighter.fighters[j].flag = true;
                     if (MyPlane.score < 100)
                     {
                         MyPlane.score += 1;
                     }
                  }
                  else if (myPlaneRect.IntersectsWith(fighterRect))  //我方飞机撞上敌机，敌机爆炸
                  {
                      Fighter.fighters[j].flag = true;
                      if (MyPlane.score < 100)
                      {
                          MyPlane.score += 1;
                      }
                  }
             }

        }
    }
}