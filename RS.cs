using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RS_Method
{
    class RS
    {
        int pix_in_grp = 4;
        //int[] mask = { 1, 0, 0, 1 };

        int widht;
        int height;
        int grp_in_row;
        int amont_group;

        int[,] group_counters = new int[3, 12];
        public RS() { }
        public Color[,] GetGroup(Bitmap img)
        {
            widht = img.Width;
            height = img.Height;
            grp_in_row = widht / pix_in_grp;
            amont_group = grp_in_row * height;

            int end = pix_in_grp * grp_in_row;

            Color[,] G = new Color[amont_group, 4];
            int k = 0;
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < end; i += 4)
                {
                    G[k, 0] = img.GetPixel(i, j);
                    G[k, 1] = img.GetPixel(i + 1, j);
                    G[k, 2] = img.GetPixel(i + 2, j);
                    G[k, 3] = img.GetPixel(i + 3, j);
                    k += 1;
                }
            }
            return G;
        }
        public Color[,] GetMaskGroup(Color[,] G, int[] mask)
        {
            Color[,] mG = new Color[amont_group, 4];
            for (int k = 0; k < amont_group; k++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (mask[i] == 1)
                    {
                        int r = Flip(G[k, i].R);
                        int g = Flip(G[k, i].G);
                        int b = Flip(G[k, i].B);
                        mG[k, i] = Color.FromArgb(r, g, b);
                    }
                    else if (mask[i] == -1)
                    {
                        int r = InvertFlip(G[k, i].R);
                        if (r == 256) r = 0;
                        else if (r == -1) r = 255;
                        int g = InvertFlip(G[k, i].G);
                        if (g == 256) g = 0;
                        else if (g == -1) g = 255;
                        int b = InvertFlip(G[k, i].B);
                        if (b == 256) b = 0;
                        else if (b == -1) b = 255;
                        mG[k, i] = Color.FromArgb(r, g, b);
                    }
                    else mG[k, i] = Color.FromArgb(G[k, i].R, G[k, i].G, G[k, i].B);
                }
            }
            return mG;
        }
        public void RS_Test(Color[,] G, Color[,] mG, int ot)
        {
            for (int k = 0; k < amont_group; k++)
            {
                int[,] f_mG = new int[1, 3];
                int[,] f_G = new int[1, 3];
                for (int i = 0; i < 3; i++)
                {
                    f_mG[0, 0] = f_mG[0, 0] + Math.Abs(mG[k, i].R - mG[k, i + 1].R);
                    f_mG[0, 1] = f_mG[0, 1] + Math.Abs(mG[k, i].G - mG[k, i + 1].G);
                    f_mG[0, 2] = f_mG[0, 2] + Math.Abs(mG[k, i].B - mG[k, i + 1].B);
                    f_G[0, 0] = f_G[0, 0] + Math.Abs(G[k, i].R - G[k, i + 1].R);
                    f_G[0, 1] = f_G[0, 1] + Math.Abs(G[k, i].G - G[k, i + 1].G);
                    f_G[0, 2] = f_G[0, 2] + Math.Abs(G[k, i].B - G[k, i + 1].B);
                }
                if (f_mG[0, 0] == f_G[0, 0])
                {
                    group_counters[0, ot + 2] = group_counters[0, ot + 2] + 1;
                }
                else
                {
                    if (f_mG[0, 0] > f_G[0, 0])
                        group_counters[0, ot] = group_counters[0, ot] + 1;
                    else group_counters[0, ot + 1] = group_counters[0, ot + 1] + 1;
                }

                if (f_mG[0, 1] == f_G[0, 1])
                {
                    group_counters[1, ot + 2] = group_counters[1, ot + 2] + 1;
                }
                else
                {
                    if (f_mG[0, 1] > f_G[0, 1])
                        group_counters[1, ot] = group_counters[1, ot] + 1;
                    else group_counters[1, ot + 1] = group_counters[1, ot + 1] + 1;
                }

                if (f_mG[0, 2] == f_G[0, 2])
                {
                    group_counters[2, ot + 2] = group_counters[2, ot + 2] + 1;
                }
                else
                {
                    if (f_mG[0, 2] > f_G[0, 2])
                        group_counters[2, ot] = group_counters[2, ot] + 1;
                    else group_counters[2, ot + 1] = group_counters[2, ot + 1] + 1;
                }
            }
        }
        public int Flip(int value)
        {
            int a = (byte)value & 0x01;
            if (a == 0) return value + 1;
            else return value - 1;
        }
        public int InvertFlip(int value)
        {
            int a = (byte)value & 0x01;
            if (a == 0) return value - 1;
            else return value + 1;
        }
        public Color[,] LsbFlip(Color[,] G)
        {
            Color[,] flpG = new Color[amont_group, 4];

            for (int k = 0; k < amont_group; k++)
            {
                for (int i = 0; i < 4; i++)
                {
                    int r = G[k, i].R ^ 0x01;
                    int g = G[k, i].G ^ 0x01;
                    int b = G[k, i].B ^ 0x01;
                    flpG[k, i] = Color.FromArgb(r, g, b);
                }
            }
            return flpG;
        }
        public double[] Solve()
        {
            double[] p = new double[3];
            int d0_r = group_counters[0, 0] - group_counters[0, 1];
            int d0_g = group_counters[1, 0] - group_counters[0, 1];
            int d0_b = group_counters[2, 0] - group_counters[0, 1];

            int md0_r = group_counters[0, 3] - group_counters[0, 4];
            int md0_g = group_counters[1, 3] - group_counters[0, 4];
            int md0_b = group_counters[2, 3] - group_counters[0, 4];

            int d1_r = group_counters[0, 6] - group_counters[0, 7];
            int d1_g = group_counters[1, 6] - group_counters[0, 7];
            int d1_b = group_counters[2, 6] - group_counters[0, 7];

            int md1_r = group_counters[0, 9] - group_counters[0, 10];
            int md1_g = group_counters[1, 9] - group_counters[0, 10];
            int md1_b = group_counters[2, 9] - group_counters[0, 10];
            //R
            int a = 2 * (d1_r + d0_r);
            int b = md0_r - md1_r - d1_r - 3 * d0_r;
            int c = d0_r - md0_r;
            int bb;
            if (b < 0) { bb = -1 * b * b; }
            else { bb = b * b; }
            int D = bb - (4 * a * c);

            double X1_r = (-1*b + Math.Sqrt(D)) / (2 * a);
            double X2_r = (-1*b - Math.Sqrt(D)) / (2 * a);
            if (Math.Abs(X1_r) > Math.Abs(X2_r)) p[0] = X2_r / (X2_r - 0.5);
            else p[0] = X1_r / (X1_r - 0.5);
            //G
            a = 2 * (d1_g + d0_g);
            b = md0_g - md1_g - d1_g - 3 * d0_g;
            c = d0_g - md0_g;

            if (b < 0) { bb = -1 * b * b; }
            else { bb = b * b; }
            D = bb - (4 * a * c);

            double X1_g = (-b + Math.Sqrt(D)) / (2 * a);
            double X2_g = (-b - Math.Sqrt(D)) / (2 * a);
            if (Math.Abs(X1_g) > Math.Abs(X2_g)) p[1] = X2_g / (X2_g - 0.5);
            else p[1] = X1_g / (X1_g - 0.5);
            //B
            a = 2 * (d1_b + d0_b);
            b = md0_b - md1_b - d1_b - 3 * d0_b;
            c = d0_b - md0_b;

            if (b < 0) { bb = -1 * b * b; }
            else { bb = b * b; }
            D = bb - (4 * a * c);

            double X1_b = (-b + Math.Sqrt(D)) / (2 * a);
            double X2_b = (-b - Math.Sqrt(D)) / (2 * a);
            if (Math.Abs(X1_b) > Math.Abs(X2_b)) p[2] = X2_b / (X2_b - 0.5);
            else p[2] = X1_b / (X1_b - 0.5);
            return p;
        }
        public void DoRSTEST(Bitmap bimg)
        {
            int[] mask = { 1, 0, 0, 1 };
            int[] minmask = { -1, 0, 0, -1 };

            Color[,] G = GetGroup(bimg);
            Color[,] mG = GetMaskGroup(G, mask);
            RS_Test(G, mG, 0);

            mG = GetMaskGroup(G, minmask);
            RS_Test(G, mG, 3);


            Color[,] flpG = LsbFlip(G);
            Color[,] mflpG = GetMaskGroup(flpG, mask);
            RS_Test(flpG, mflpG, 6);

            mflpG = GetMaskGroup(flpG, minmask);
            RS_Test(flpG, mflpG, 9);

            double[] p = Solve();
            Console.WriteLine($"{p[0]}\n{p[1]}\n{p[2]}");
            Console.ReadLine();
        }
    }
}
