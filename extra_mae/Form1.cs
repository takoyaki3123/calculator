using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace extra_mae
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Regex reg = new Regex(@"[()]+");
        Bitmap bmp = new Bitmap(1, 1);
        int count = 0, father = 0;
        string treetwo = "";
        Graphics g;
        int num = 0;
        string n;
        private void Button3_Click(object sender, EventArgs e)
        {
            n = richTextBox1.Text;
            bmp = new Bitmap(pictureBox1.Width * Convert.ToInt32((double)(n.Length)/7), pictureBox1.Height *Convert.ToInt32((double)(n.Length) / 7) + 50);
            g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            treetwo = "";
            if (n[0] == '+' | n[0] == '-' | n[0] == '*' | n[0] == '/')//輸入前序
            {
                stack = "";
                buffer = new List<char>();
                findmid(n);
                n = stack;
            }
            else if (n.Last() == '+' | n.Last() == '-' | n.Last() == '*' | n.Last() == '/')//輸入後序
            {
                stack = "";
                string newn = "";
                for (int i = n.Length - 1; i >= 0; i--)
                {
                    newn += n[i];
                }
                buffer = new List<char>();
                findmid(newn);
                newn = "";
                for (int i = stack.Length - 1; i >= 0; i--)
                {
                    if (stack[i] == '(')
                    {
                        newn += ')'.ToString();
                    }else if( stack[i] == ')')
                    {
                        newn += '('.ToString();
                    }
                    else
                    {
                        newn += stack[i];
                    }
                }
                n = newn;
            }
            num = 0;
            have = new List<string>();
            findfirst(n, 1, true);
            n = n.Replace("(", "");
            n = n.Replace(")", "");
            for (int i = n.Length - 1; i >= 0; i--)
            {
                if (n[i] == treetwo[num])
                {
                    g.FillEllipse(new SolidBrush(Color.Black), bmp.Width/2, 20 , 30, 30);
                    g.DrawString(treetwo[num].ToString(), new Font(new FontFamily("新細明體"), 20), new SolidBrush(Color.LightBlue), bmp.Width / 2, 20);
                    num++;
                   // n = n.Remove(i, 1);
                    draw(n.Substring(0, i), 0, bmp.Width / 4,bmp.Height/4, new Point(bmp.Width / 2 - bmp.Width / 4, bmp.Height / 2 - bmp.Height / 4 + 40), new Point(bmp.Width / 2, 35));//左
                    draw(n.Substring(i+1), 1, bmp.Width / 4,bmp.Height/4, new Point(bmp.Width / 2 + bmp.Width / 4, bmp.Height / 2 - bmp.Height / 4 + 40), new Point(bmp.Width / 2, 35));//又
                    pictureBox1.Image = bmp;
                    break;
                }
            }
        }
        List<string> have = new List<string>();
        void draw(string tree,int l,int c,int b,Point point, Point before_point)
        {
            if (tree.Length > 0 & !(have.Contains(tree)))
            {
                for (int i = tree.Length - 1; i >= 0; i--)
                {
                    if (tree[i] == treetwo[num])
                    {
                        if (l == 0)//左邊
                        {
                            g.FillEllipse(new SolidBrush(Color.Black), point.X, point.Y, 30,30);
                            g.DrawString(tree[i].ToString(), new Font(new FontFamily("新細明體"), 20), new SolidBrush(Color.LightBlue), point.X, point.Y);
                        }
                        else
                        {
                            g.FillEllipse(new SolidBrush(Color.Black), point.X, point.Y, 30, 30);
                            g.DrawString(tree[i].ToString(), new Font(new FontFamily("新細明體"), 20), new SolidBrush(Color.LightBlue), point.X, point.Y);

                        }
                        g.DrawLine(new Pen(Color.Black), point.X + 20, point.Y + 10, before_point.X+20, before_point.Y+10);
                        have.Add(tree);
                        //tree = tree.Remove(i, 1);
                        num++;
                        c = c / 2;
                        b = b / 2;
                        draw(tree.Substring(0, i), 0, c,b, new Point(point.X - c, point.Y+b+40), new Point(point.X,point.Y+15));
                        draw(tree.Substring(i+1), 1, c,b, new Point(point.X + c, point.Y+b+40), new Point(point.X, point.Y + 15));
                        break;
                    }
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            richTextBox1.Text = "(1*(2+3)*4)-5*6-7";
        }
        string stack = "";
        List<char> buffer = new List<char>();
        void findfirst(string str, int who,bool draw)
        {
            father = 0;
            int number = 0;
            for (int i = str.Length - 1; i >= 0; i--)
            {
                if (str[i] == '(')
                    count--;
                else if (str[i] == ')')
                    count++;
                if (count == 0)
                {
                    if (str[i] == '+' | str[i] == '-')//+-優先度較低放前面
                    {
                        father = i; number++;
                        break;
                    }
                    else if (str[i] == '*' | str[i] == '/')
                    {
                        father = i > father ? i : father; number++;
                    }
                }
            }
            if (number != 0)
            {
                treetwo = who == 1 ? treetwo + str[father].ToString() : str[father].ToString() + treetwo;
                str = str.Remove(father, 1);
                str = str.Insert(father, " ");
                string[] data = str.Split(' ');
                if (who == 1)//1前旭2後續
                {
                    findfirst(data[0], 1, draw);
                    findfirst(data[1], 1, draw);
                }
                else
                {
                    findfirst(data[1], 2, draw);
                    findfirst(data[0], 2, draw);
                }
            }
            else
            {
                if (reg.IsMatch(str))
                {
                    for (int i = 0; i < str.Length; i++)
                    {
                        if (str[i] == '(')
                        {
                            str = str.Remove(i, 1);
                            break;
                        }
                    }
                    for (int i = str.Length - 1; i >= 0; i--)
                    {
                        if (str[i] == ')')
                        {
                            str = str.Remove(i, 1);
                            break;
                        }
                    }
                    if (who == 1)
                    {
                        findfirst(str, 1, draw);
                    }
                    else
                    {
                        findfirst(str, 2, draw);
                    }
                }
                else
                {
                    treetwo = who == 1 ? treetwo + str[father].ToString() : str[father].ToString() + treetwo;
                }
                return;
            }
        }
        int ans = 0;
        private void Button6_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = "";
            n = richTextBox1.Text;
            if(n[0] == '+' | n[0] == '-'| n[0] == '*'| n[0] == '/')//輸入前序
            {
                stack = n;
                shenshan(stack);
                richTextBox2.Text += "前序"+stack + "\r\n";
                stack = "";
                buffer = new List<char>();
                findmid(n);//轉中序
                treetwo = "";
                n = stack;
                findfirst(n, 2, false);//中轉後
                houshan(treetwo);
                richTextBox2.Text += "後序" + treetwo + "\r\n";
                n = richTextBox1.Text;
                stack = "";
                buffer = new List<char>();
                findmid(n);
                richTextBox2.Text += "中序" + stack;
            }
            else if(n.Last() == '+' | n.Last() == '-' | n.Last() == '*' | n.Last() == '/')//輸入後序
            {
                stack = "";
                string newn = "";
                for (int i = n.Length - 1; i >= 0; i--)//從後往前找 根前序相反
                {
                    newn += n[i];
                }
                buffer = new List<char>();
                findmid(newn);
                newn = "";
                for (int i = stack.Length - 1; i >= 0; i--)
                {
                    newn += stack[i];
                }
                treetwo = "";
                n = newn;
                findfirst(n, 1, false);
                stack = n;
                shenshan(stack);
                richTextBox2.Text += "前序" + stack + "\r\n";
                n = richTextBox1.Text;
                houshan(n);
                richTextBox2.Text += "後序" + n + "\r\n";
                stack = "";
                newn = "";
                for (int i = n.Length - 1; i >= 0; i--)
                {
                    newn += n[i];
                }
                buffer = new List<char>();
                findmid(newn);
                newn = "";
                for (int i = stack.Length - 1; i >= 0; i--)
                {
                    if (stack[i] == '(')
                    {
                        newn += ')'.ToString();
                    }
                    else if (stack[i] == ')')
                    {
                        newn += '('.ToString();
                    }
                    else
                    {
                        newn += stack[i];
                    }
                }
                richTextBox2.Text += "中序"+newn + "\r\n";
                

            }
            else
            {
                treetwo = "";
                findfirst(n, 1, false);
                shenshan(treetwo);
                richTextBox2.Text += "前序"+treetwo + "\r\n";
                treetwo = "";
                findfirst(n, 2, false);
                houshan(treetwo);
                richTextBox2.Text += "後續"+treetwo + "\r\n";
                stack = n;
                richTextBox2.Text += "中序"+n + "\r\n";
            }
        }
        void findmid(string str)
        {
            int buffer_now = 0;
            int count = 0;
            int before_pass_word = 0;
            bool first = false;//找乘號除號或其他
            for(int i = 0; i < str.Length; i++)
            {
                char op = str[i];
                switch (op)
                {
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                        if (stack != "" & buffer.Count > 0)
                        {
                            if ((stack.Last() == '*' | stack.Last() == '/') & (op == '+' | op == '-'))
                            {
                                before_pass_word++;
                                stack += "(";
                            }
                        }
                        buffer.Add(op);
                        buffer_now++;
                        break;
                    default:
                        stack += op.ToString();
                        if (stack != "" & buffer.Count > 0)
                        {
                            if ((buffer.Last() == '*' | buffer.Last() == '/') & before_pass_word!=0)
                            {
                                stack += ")";
                                before_pass_word--;
                            }
                        }
                        if (buffer.Count > 0)
                        {
                            stack += buffer.Last();
                            buffer.Remove(buffer.Last());
                        }
                        buffer_now--;
                        break;
                }
            }
        }
        void houshan(string sta)
        {
            List<char> buffer = new List<char>();
            List<string> str = new List<string>();
            int strlen = 0;
            foreach (char item in sta)
            {
                foreach(string i in str)
                {
                    richTextBox4.Text += i;
                }
                richTextBox4.Text += item.ToString() + "\r\n";
                switch (item)//運算福放最後所以遇到就往前算
                {
                    case '+':
                        ans = Convert.ToInt32(str[strlen - 2]) + Convert.ToInt32(str[strlen - 1]);
                        str.Remove(str.Last());
                        strlen--;
                        str[strlen - 1] = ans.ToString();
                        break;
                    case '-':
                        ans = Convert.ToInt32(str[strlen - 2]) - Convert.ToInt32(str[strlen - 1]);
                        str.Remove(str.Last());
                        strlen--;
                        str[strlen - 1] = ans.ToString();
                        break;
                    case '/':
                        ans = Convert.ToInt32(str[strlen - 2]) / Convert.ToInt32(str[strlen - 1]);
                        str.Remove(str.Last());
                        strlen--;
                        str[strlen - 1] = ans.ToString();
                        break;
                    case '*':
                        ans = Convert.ToInt32(str[strlen - 2]) * Convert.ToInt32(str[strlen - 1]);
                        str.Remove(str.Last());
                        strlen--;
                        str[strlen - 1] = ans.ToString();
                        break;
                    default:
                        str.Add(item.ToString());
                        strlen++;
                        break;
                }
            }
            richTextBox4.Text += ans.ToString();
        }
        void shenshan(string sta)
        {
            List<char> buffer = new List<char>();
            int bufferlen = 0;
            List<string> str = new List<string>();
            int strlen = 0;
            for(int i = 0; i < sta.Length; i++)
            {
                str.Add(sta[i].ToString());
            }
            int count = 0;
            List<string> have = new List<string>();
            for(int i = 0; i < str.Count; i++)
            {
                string check = "";
                if (str.Count == 1)
                {
                    break;
                }
                for (int j = 0; j <= i; j++)
                {
                    check += str[j];
                }
                if (!(have.Contains(check)))//步驟顯示
                {
                    have.Add(check);
                    richTextBox3.Text += have.Last() + "\r\n";
                }
                
                switch (str[i])
                {
                    case "+":
                    case "-":
                    case "*":
                    case "/"://兩個數字連這才能算
                        count = 0;
                        break;
                    default:
                        strlen++;
                        count++;
                        if (count >= 2)
                        {
                            if (str[i-count] == "+")
                            {
                                ans = ans == 0 ? Convert.ToInt32(sta[i - count + 1].ToString()) + Convert.ToInt32(sta[i - count + 2].ToString()) : Convert.ToInt32(str[i - count + 1].ToString()) + Convert.ToInt32(str[i - count + 2].ToString());
                            }
                            if (str[i - count] == "-")
                                ans = ans == 0 ? Convert.ToInt32(sta[i - count + 1].ToString()) - Convert.ToInt32(sta[i - count + 2].ToString()) : Convert.ToInt32(str[i - count + 1].ToString()) - Convert.ToInt32(str[i - count + 2].ToString());
                            if (str[i - count] == "*")
                                ans = ans == 0 ? Convert.ToInt32(sta[i - count + 1].ToString()) * Convert.ToInt32(sta[i - count + 2].ToString()) : Convert.ToInt32(str[i - count + 1].ToString()) * Convert.ToInt32(str[i - count + 2].ToString());
                            if (str[i - count] == "/")
                                ans = ans == 0 ? Convert.ToInt32(sta[i - count + 1].ToString()) / Convert.ToInt32(sta[i - count + 2].ToString()) : Convert.ToInt32(str[i - count + 1].ToString()) / Convert.ToInt32(str[i - count + 2].ToString());
                            strlen--;
                            str.Remove(str[i - count + 1].ToString());
                            str.Remove(str[i - count].ToString());
                            str[i - count] = ans.ToString();
                            count--;
                            i =-1;
                        }
                        break;
                }
            }
            richTextBox3.Text += ans.ToString();
        }
    }
}
