using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PractTask2
{
    public partial class Form1 : Form
    {
        private Tree fract = new Tree(new PointF(180, 250), new PointF(320, 250));

        public class Tree
        {
            public class Node
            {
                public PointF dot1;
                public PointF dot2;
                public int level;
                public Node left;
                public Node right;
            }

            private List<Color> colors = new List<Color>();
            private int maxLevel;
            private Node root;

            public Tree(PointF dot1, PointF dot2)
            {
                Node tempN = new Node();
                root = tempN;
                maxLevel = 0;
                root.dot1 = dot1;
                root.dot2 = dot2;
                Random rand = new Random();
                int R = rand.Next(0, 255);
                int G = rand.Next(0, 255);
                int B = rand.Next(0, 255);
                Color temp = Color.FromArgb(R, G, B);
                colors.Add(temp);
                root.level = 0;
            }

            public void drawLayer(int drawLevel, PaintEventArgs e)
            {
                if (drawLevel > maxLevel)
                {
                    throw new ArgumentOutOfRangeException("level", "This level doesn't exists");
                }

                drawLayer(root, drawLevel, e);
            }

            private void drawLayer(Node parent, int drawLevel, PaintEventArgs e)
            {
                if (drawLevel == 0)
                {
                    Pen pen = new Pen(colors[parent.level]);
                    e.Graphics.DrawLine(pen, parent.dot1, parent.dot2);
                }
                else
                {
                    drawLayer(parent.left, drawLevel - 1, e);
                    drawLayer(parent.right, drawLevel - 1, e);
                }
            }

            public void buffMax(int newMax)
            {
                if (newMax > maxLevel)
                {
                    int toGo = maxLevel;

                    Random rand = new Random();
                    for (int i = maxLevel; i < newMax; ++i)
                    {
                        int R = rand.Next(0, 255);
                        int G = rand.Next(0, 255);
                        int B = rand.Next(0, 255);
                        Color levelColor = Color.FromArgb(R, G, B);
                        colors.Add(levelColor);
                    }
                    maxLevel = newMax;
                    buffMax(root, newMax - 1, toGo);
                }
            }

            private void buffMax(Node parent, int newMax, int toGo)
            {
                if (toGo == 0)
                {
                    Node leftTemp = new Node();
                    Node rightTemp = new Node();

                    float newX = (parent.dot1.X + parent.dot2.X) / 2 - (parent.dot2.Y - parent.dot1.Y) / 2;
                    float newY = (parent.dot1.Y + parent.dot2.Y) / 2 + (parent.dot2.X - parent.dot1.X) / 2;

                    leftTemp.dot1 = parent.dot1;
                    leftTemp.dot2.X = newX;
                    leftTemp.dot2.Y = newY;

                    rightTemp.dot1.X = newX;
                    rightTemp.dot1.Y = newY;
                    rightTemp.dot2 = parent.dot2;

                    leftTemp.level = maxLevel - newMax;
                    rightTemp.level = maxLevel - newMax;
                    parent.left = leftTemp;
                    parent.right = rightTemp;
                }

                if (newMax > 0)
                {
                    --newMax;
                    if (toGo > 0)
                        --toGo;
                    buffMax(parent.left, newMax, toGo);
                    buffMax(parent.right, newMax, toGo);
                }
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void numericUpDownLevel_ValueChanged(object sender, EventArgs e)
        {
            fract.buffMax((int)numericUpDownLevel.Value - 1);
            numericUpDownExtra.Maximum = numericUpDownLevel.Value - 1;
            pictureBox.Refresh();
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(0, pictureBox.Height);
            e.Graphics.ScaleTransform(1, -1);
            fract.drawLayer((int)numericUpDownLevel.Value - 1, e);
            if (numericUpDownExtra.Value > 0)
                for (int i = 0; i < numericUpDownExtra.Value; ++i)
                    fract.drawLayer((int)numericUpDownLevel.Value - 2 - i, e);
        }

        private void numericUpDownExtra_ValueChanged(object sender, EventArgs e)
        {
            pictureBox.Refresh();
        }
    }
}