using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
namespace Myplanegame
{
    public class MyPlane
    {
        public static int x = 180;
        public static int y = 530;//坐标
        public static int health = 100; //血量
        
        private const int PLANE_OFFSET = 12;//移动速度
        
        public static Image myPlaneImg=Resource.plane;//我方飞机图片
        static List<Keys> keys = new List<Keys>();//键盘键列表，用于控制飞机移动
        static Image gameOver = Resource.gameover;

        public static bool isGetGun = false;//是否得到shotgun的标志
        public static bool isGetBlood = false;//是否得到bloodbox的标志
        public static bool isGameOver = false; //游戏是否结束的标志
        public static int score = 0;      //得分

        /// <summary>
        /// 显示我方飞机
        /// </summary>
        /// <param name="g"></param>
        public static void MyPlaneShow(Graphics g)
        {
            if (health > 0)
            {
                g.DrawImage(myPlaneImg, x, y);
            }
            else if (health <= 0 || score <= 0)
            {
                isGameOver = true;
                g.DrawImage(myPlaneImg, 0, -300);
                g.DrawImage(gameOver, 10, 260);
            }
            else if (isGetBlood && health <= 90)
            {
                health += 10;
            }
        }

        /// <summary>
        /// 松开键盘键
        /// </summary>
        /// <param name="key"></param>
        public static void Keyup(Keys key)
        {
            keys.Remove(key);
        }

        /// <summary>
        /// 按下键盘键
        /// </summary>
        /// <param name="key"></param>
        public static void Keydown(Keys key)
        {
            if (!keys.Contains(key))
            {
                keys.Add(key);
            }
        }
        
        /// <summary>
        /// 判断按键是否被按下
        /// </summary>
        /// <param name="key"></param>
        /// <returns>是则返回true  不是则返回false</returns>
        public static bool IsKeyDown(Keys key)
        {
            return keys.Contains(key);
        }
        
        /// <summary>
        /// 用键盘控制我方飞机移动
        /// </summary>
        public static void MyPlaneMove()
        {
            if(isGameOver)
            {
                return;
            }
            if (IsKeyDown(Keys.A))
            {
                myPlaneImg = Resource.planeLeft;
                if (x < 5)
                    x = 5;
                x -= PLANE_OFFSET;
            }
            if (IsKeyDown(Keys.D))
            {
                myPlaneImg = Resource.planeRight;
                if (x > 370)
                    x = 370;
                x += PLANE_OFFSET;
            }
            if (IsKeyDown(Keys.W))
            {
                if (y < 5)
                    y = 5;
                y -= PLANE_OFFSET;
            }
            if (IsKeyDown(Keys.S))
            {
                if (y > 530)
                    y = 530;
                y += PLANE_OFFSET;
            }
        }
    }
}
