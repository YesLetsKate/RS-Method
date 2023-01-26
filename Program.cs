using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RS_Method
{
    class Program
    {
        static void Main(string[] args)
        {
            //OpenFileDialog openDialog = new OpenFileDialog();
            //openDialog.Filter = "Файлы изображений|*.bmp";
            //if (openDialog.ShowDialog() != DialogResult.OK) return;
            string path = @"C:\Users\Yesle\Desktop\_-_\УПД\УПД-5\Тех часть\RS-Method1\pics\R0vvnl9HVnwменьше.bmp";
            Image img = Image.FromFile(path);
            Bitmap bImg = new Bitmap(img);

            RS rs = new RS();
            rs.DoRSTEST(bImg);
        }
    }
}
