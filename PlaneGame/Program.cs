using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System;

namespace Myplanegame
{
    public partial class Program
    {
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GameForm());

        }
    }
}
