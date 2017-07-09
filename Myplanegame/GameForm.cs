using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Media;
using System.IO; 
using System.Runtime.InteropServices;


namespace Myplanegame
{
    public partial class GameForm : Form     //继承类
    {
        private const int PLANE_OFFSET = 2;       //设置每次定时器触发时图片发生偏移的速度
        
        private int pix_x = 0;
        private int pix_y = 0;         //背景图片移动起始的坐标
        int shot_y = 10;
        int blood_y = 50;
        private Image[] bgrounds;        //设置多张背景图片，每次运行程序随机产生背景图片
        int Index = 0;                           //背景图片索引
        Image headimage = Resource.imgHeadSheep;     //角色头像图片
        Image bm = Resource.bomb4;          //爆炸效果图片
        Image shotImg = Resource.shotgun;
        Image bloodImg = Resource.bloodbox;

        bool isDropGun = false;      //是否产生shotgun的标志
        bool Isdropbox = false;       //是否产生bloodbox的标志

        private void Form1_Load(object sender, EventArgs e)//窗体加载事件
        {
            InitBackground();              //初始化背景
        }

        public GameForm()
        {
            InitializeComponent();
            this.Size = new Size(420, 630);//让窗体与图片一样大
            //this.DoubleBuffered = true;
        }
         ///<summary>
        /// 初始化背景，随机生成背景图片
        /// </summary>
        public void InitBackground()
        {
            bgrounds = new Image[4];
            Random rd = new Random();
            Index = rd.Next(0, 4);//产生0-3的随机数，表示不同背景

            bgrounds[0] = Resource.background1;//从资源获取图片
            bgrounds[1] = Resource.background2;
            bgrounds[2] = Resource.background3;
            bgrounds[3] = Resource.background4;
        }
        
        /// <summary>
        /// 建立背景移动函数
        /// </summary>
        /// <param name="e">图形对象</param>
        public void BackMove(Graphics e)//通过定时位置让图片发生偏移，防止有空白
        {
            e = this.CreateGraphics();
            pix_y += PLANE_OFFSET;
            if (pix_y > 630)
            {
                pix_y = 0;
            }
        }

        /// <summary>
        /// 随机产生shotgun
        /// </summary>
        public void ProduceShotGun()
        {
            Rectangle sgRect = new Rectangle(new Random().Next(0, 350), shot_y, shotImg.Width, shotImg.Height);      //包住shotgun的Rectangle
            Rectangle mpRect = new Rectangle(MyPlane.x, MyPlane.y, MyPlane.myPlaneImg.Width, MyPlane.myPlaneImg.Height);    //包住myplane的Rectangle

            if (new Random().Next(0, 100) == 2)     //随机产生shotgun
            {
                isDropGun = true;
            }
            if (isDropGun && !MyPlane.isGetGun)
            {
                if (sgRect.IntersectsWith(mpRect))
                {
                    MyPlane.isGetGun = true;
                    shot_y = -100;
                }
            }
            shot_y += 5;
            if (shot_y > 950)
            {
                shot_y = -100;
            }
        }
        /// <summary>
        /// 随机产生bloodbox
        /// </summary>
        public void ProduceBlood()
        {
            Rectangle mpRect = new Rectangle(MyPlane.x, MyPlane.y, MyPlane.myPlaneImg.Width, MyPlane.myPlaneImg.Height);    //包住myplane的Rectangle
            Rectangle bbRect = new Rectangle(new Random().Next(0, 390), blood_y, bloodImg.Width, bloodImg.Height);     //包住bloodbox的Rectangle

            if (new Random().Next(0, 100) == 0)        //随机产生bloodbox
            {
                Isdropbox = true;
            }
            if (Isdropbox && !MyPlane.isGetBlood)
            {
                if (bbRect.IntersectsWith(mpRect))
                {
                    MyPlane.isGetBlood = true;
                    blood_y = -100;
                }
            }
        }

        /// <summary>
        /// 绘制游戏界面
        /// </summary>
        /// <param name="g"></param>
        private void DrawGame(Graphics g)                    //绘制界面上所有图像，避免闪烁
        {
            this.BackMove(g);               
            g.DrawImage(bgrounds[Index], pix_x, pix_y, 420, 630);           
            g.DrawImage(bgrounds[Index], pix_x, pix_y - 630, 420, 630);       //绘制背景

            g.DrawImage(headimage, 10, 10);                                            //绘制角色头像
            g.DrawRectangle(new Pen(Color.Black), new Rectangle(10, 100, 100, 10));     //绘制血条矩形
            g.FillRectangle(Brushes.Red, 10, 101, MyPlane.health, 9);                   //填充血条矩形

            g.DrawRectangle(new Pen(Color.Blue), new Rectangle(10, 120, 100, 10));
            g.FillRectangle(Brushes.Green, 11, 121, MyPlane.score, 9);
            g.DrawString("玩家：xjc", new Font("宋体", 9, FontStyle.Bold), Brushes.Yellow, new Point(10, 140));      //显示玩家
            g.DrawString("得分：" + MyPlane.score, new Font("宋体", 9, FontStyle.Bold), Brushes.Yellow, new Point(10, 160));      //显示分数

            MyPlane.MyPlaneShow(g);
            MyPlane.MyPlaneMove();

            MyBullet.ProduceMybul();
            MyBullet.MoveMybul(g);
            MyBullet.IsHitEnemy(g);

            Fighter.ProduceFighter();
            Fighter.FighterMove(g);

            EnemyBullet.ProduceEnbul();
            EnemyBullet.MoveEnbul(g);
            EnemyBullet.HitPlane(g);

            this.ProduceShotGun();
            this.ProduceBlood();

            if (isDropGun && !MyPlane.isGetGun)          //判断是否产生shotgun,并绘制
            {
                g.DrawImage(shotImg, 200, shot_y);
            }
            if (MyPlane.isGetGun)
            {
                g.DrawImage(shotImg, new Point(0, -500));
            }

            if (Isdropbox && !MyPlane.isGetBlood)         //判断是否产生bloodbox,并绘制
            {
                g.DrawImage(bloodImg, 350, blood_y);
            }
            if (MyPlane.isGetBlood)
            {
                g.DrawImage(bloodImg, new Point(0, -500));
            }
        }

        protected override void OnPaint(PaintEventArgs e) //先将图像绘制到Bitmap图片中，再加载这个图片，以减少图像闪烁
        {
            Bitmap bufferBmp = new Bitmap(this.ClientRectangle.Width-1, this.ClientRectangle.Height-1);
            Graphics g = Graphics.FromImage((System.Drawing.Image)bufferBmp);
            this.DrawGame(g);

            //将要绘制的内容先绘制到g上
            e.Graphics.DrawImage(bufferBmp, 0, 0);
            g.Dispose();
            base.OnPaint(e);
        }

        /// <summary>
        /// 设置定时器1事件
        /// </summary>
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Invalidate();      //使当前窗口无效，系统自动调用OnPaint()函数重绘
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            MyPlane.Keydown(e.KeyCode);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        { 
            MyPlane.Keyup(e.KeyCode);
            MyPlane.myPlaneImg = Resource.plane;
        }

        /// <summary>
        /// 设置定时器2事件
        /// </summary>
        private void timer2_Tick(object sender, EventArgs e)
        {
            Graphics g = this.CreateGraphics();
            for (int j = 0; j < Fighter.fighters.Count; j++)
            {
                if (Fighter.fighters[j].flag)
                {
                    g.DrawImage(bm, Fighter.fighters[j].GetLoc());
                    SoundPlayer music = new SoundPlayer(Resource.BOMB21);
                    music.Play(); 
                    Fighter.fighters.Remove(Fighter.fighters[j]);
                }
            }
        }
    }
}
