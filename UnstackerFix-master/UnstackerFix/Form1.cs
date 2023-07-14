using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnstackerFix
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Enabled = false;
            button5.Enabled = false;
            //Кнопки увеличения маркера
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            //Кнопки уменьшения маркера
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;
            //Расширения маркера
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            //Проверка выбран ли путь для сохранения папки
            if (File.Exists(@".\FolderPach.txt"))
            {
                label4.Text = "Папка для сохранения выбрана";
                label4.ForeColor = Color.Gray;
                textBox1.Enabled = true;
                patch = File.ReadAllText(@".\FolderPach.txt").Trim();
                
            }

        }
        string patch;
        Rectangle rect;
        string[] pach;
        int X ;
        int Y ;
        int W ;
        int H ;
        private void panel1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                label1.Text = "Отпустите мышь";
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void panel1_DragLeave(object sender, EventArgs e)
        {
            label1.Text = "Перетащите сюда картинку";
        }

        private void panel1_DragDrop(object sender, DragEventArgs e)
        {
            pach = (string[])e.Data.GetData(DataFormats.FileDrop);
            pictureBox1.Image = Image.FromFile(pach[0]);
            panel1.Visible = false;
            
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                pictureBox2.Visible = true;

                PictureBox pb = pictureBox1 as PictureBox;

                Bitmap bmp1 = pb.Image as Bitmap;
                Bitmap bmp2 = bmp1.Clone(rect, bmp1.PixelFormat);
                pictureBox2.Image = bmp2;

                //Выводим координаты маркера
                label10.Text = rect.X.ToString();
                label11.Text = rect.Y.ToString();
                label12.Text = rect.Width.ToString();
                label13.Text = rect.Height.ToString();

                //Кнопки увеличения маркера
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;
                //Кнопки уменьшения маркера
                button6.Enabled = true;
                button7.Enabled = true;
                button8.Enabled = true;
                button9.Enabled = true;
                //Расширения маркера
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                textBox5.Enabled = true;
            }
            catch (Exception)
            {

            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                Graphics g = pictureBox1.CreateGraphics();
                g.DrawImage(pictureBox1.Image, 0, 0);

                Point kor = e.Location;
                X = kor.X;
                Y = kor.Y;
                W = 15;
                H = 15;

                Pen pero = new Pen(Color.Red, 1);
                rect = new Rectangle(X - 7, Y - 7, W, H);
                g.DrawRectangle(pero, rect);
            }
            catch (Exception)
            {

            }
        }

        //Плюс в лево
        private void button1_Click(object sender, EventArgs e)
        {
            Graphics g = pictureBox1.CreateGraphics();
            g.DrawImage(pictureBox1.Image, 0, 0);

            int X = rect.X;
            int Y = rect.Y;
            int W = rect.Width;
            int H = rect.Height;

            Pen pero = new Pen(Color.Red, 1);
            rect = new Rectangle(X-1, Y, W+1, H);
            g.DrawRectangle(pero, rect);

            PictureBox pb = pictureBox1 as PictureBox;

            Bitmap bmp1 = pb.Image as Bitmap;
            Bitmap bmp2 = bmp1.Clone(rect, bmp1.PixelFormat);
            pictureBox2.Image = bmp2;

            //Выводим координаты маркера
            label10.Text = rect.X.ToString();
            label11.Text = rect.Y.ToString();
            label12.Text = rect.Width.ToString();
            label13.Text = rect.Height.ToString();
        }
        //Плюс в право
        private void button2_Click(object sender, EventArgs e)
        {
            Graphics g = pictureBox1.CreateGraphics();
            g.DrawImage(pictureBox1.Image, 0, 0);

            int X = rect.X;
            int Y = rect.Y;
            int W = rect.Width;
            int H = rect.Height;

            Pen pero = new Pen(Color.Red, 1);
            rect = new Rectangle(X, Y, W + 1, H);
            g.DrawRectangle(pero, rect);

            PictureBox pb = pictureBox1 as PictureBox;

            Bitmap bmp1 = pb.Image as Bitmap;
            Bitmap bmp2 = bmp1.Clone(rect, bmp1.PixelFormat);
            pictureBox2.Image = bmp2;

            //Выводим координаты маркера
            label10.Text = rect.X.ToString();
            label11.Text = rect.Y.ToString();
            label12.Text = rect.Width.ToString();
            label13.Text = rect.Height.ToString();
        }
        //Плюс в верх
        private void button3_Click(object sender, EventArgs e)
        {
            Graphics g = pictureBox1.CreateGraphics();
            g.DrawImage(pictureBox1.Image, 0, 0);

            int X = rect.X;
            int Y = rect.Y;
            int W = rect.Width;
            int H = rect.Height;

            Pen pero = new Pen(Color.Red, 1);
            rect = new Rectangle(X, Y-1, W, H+1);
            g.DrawRectangle(pero, rect);

            PictureBox pb = pictureBox1 as PictureBox;

            Bitmap bmp1 = pb.Image as Bitmap;
            Bitmap bmp2 = bmp1.Clone(rect, bmp1.PixelFormat);
            pictureBox2.Image = bmp2;

            //Выводим координаты маркера
            label10.Text = rect.X.ToString();
            label11.Text = rect.Y.ToString();
            label12.Text = rect.Width.ToString();
            label13.Text = rect.Height.ToString();
        }
        //Плюс в низ
        private void button4_Click(object sender, EventArgs e)
        {
            Graphics g = pictureBox1.CreateGraphics();
            g.DrawImage(pictureBox1.Image, 0, 0);

            int X = rect.X;
            int Y = rect.Y;
            int W = rect.Width;
            int H = rect.Height;

            Pen pero = new Pen(Color.Red, 1);
            rect = new Rectangle(X, Y, W, H+1);
            g.DrawRectangle(pero, rect);

            PictureBox pb = pictureBox1 as PictureBox;

            Bitmap bmp1 = pb.Image as Bitmap;
            Bitmap bmp2 = bmp1.Clone(rect, bmp1.PixelFormat);
            pictureBox2.Image = bmp2;

            //Выводим координаты маркера
            label10.Text = rect.X.ToString();
            label11.Text = rect.Y.ToString();
            label12.Text = rect.Width.ToString();
            label13.Text = rect.Height.ToString();
        }
        //Минус с лева
        private void button6_Click(object sender, EventArgs e)
        {
            Graphics g = pictureBox1.CreateGraphics();
            g.DrawImage(pictureBox1.Image, 0, 0);

            int X = rect.X;
            int Y = rect.Y;
            int W = rect.Width;
            int H = rect.Height;

            Pen pero = new Pen(Color.Red, 1);
            rect = new Rectangle(X+1, Y, W-1, H);
            g.DrawRectangle(pero, rect);

            PictureBox pb = pictureBox1 as PictureBox;

            Bitmap bmp1 = pb.Image as Bitmap;
            Bitmap bmp2 = bmp1.Clone(rect, bmp1.PixelFormat);
            pictureBox2.Image = bmp2;

            //Выводим координаты маркера
            label10.Text = rect.X.ToString();
            label11.Text = rect.Y.ToString();
            label12.Text = rect.Width.ToString();
            label13.Text = rect.Height.ToString();
        }
        //Минус с права
        private void button7_Click(object sender, EventArgs e)
        {
            Graphics g = pictureBox1.CreateGraphics();
            g.DrawImage(pictureBox1.Image, 0, 0);

            int X = rect.X;
            int Y = rect.Y;
            int W = rect.Width;
            int H = rect.Height;

            Pen pero = new Pen(Color.Red, 1);
            rect = new Rectangle(X, Y, W-1, H);
            g.DrawRectangle(pero, rect);

            PictureBox pb = pictureBox1 as PictureBox;

            Bitmap bmp1 = pb.Image as Bitmap;
            Bitmap bmp2 = bmp1.Clone(rect, bmp1.PixelFormat);
            pictureBox2.Image = bmp2;

            //Выводим координаты маркера
            label10.Text = rect.X.ToString();
            label11.Text = rect.Y.ToString();
            label12.Text = rect.Width.ToString();
            label13.Text = rect.Height.ToString();
        }
        //Минус с верху
        private void button8_Click(object sender, EventArgs e)
        {
            Graphics g = pictureBox1.CreateGraphics();
            g.DrawImage(pictureBox1.Image, 0, 0);

            int X = rect.X;
            int Y = rect.Y;
            int W = rect.Width;
            int H = rect.Height;

            Pen pero = new Pen(Color.Red, 1);
            rect = new Rectangle(X, Y+1, W, H-1);
            g.DrawRectangle(pero, rect);

            PictureBox pb = pictureBox1 as PictureBox;

            Bitmap bmp1 = pb.Image as Bitmap;
            Bitmap bmp2 = bmp1.Clone(rect, bmp1.PixelFormat);
            pictureBox2.Image = bmp2;

            //Выводим координаты маркера
            label10.Text = rect.X.ToString();
            label11.Text = rect.Y.ToString();
            label12.Text = rect.Width.ToString();
            label13.Text = rect.Height.ToString();
        }
        //Минус с низу
        private void button9_Click(object sender, EventArgs e)
        {
            Graphics g = pictureBox1.CreateGraphics();
            g.DrawImage(pictureBox1.Image, 0, 0);

            int X = rect.X;
            int Y = rect.Y;
            int W = rect.Width;
            int H = rect.Height;

            Pen pero = new Pen(Color.Red, 1);
            rect = new Rectangle(X, Y, W, H-1);
            g.DrawRectangle(pero, rect);

            PictureBox pb = pictureBox1 as PictureBox;

            Bitmap bmp1 = pb.Image as Bitmap;
            Bitmap bmp2 = bmp1.Clone(rect, bmp1.PixelFormat);
            pictureBox2.Image = bmp2;

            //Выводим координаты маркера
            label10.Text = rect.X.ToString();
            label11.Text = rect.Y.ToString();
            label12.Text = rect.Width.ToString();
            label13.Text = rect.Height.ToString();
        }

        //Проверка при вводе имени папки на дубли
        List<string> spisokPapok = new List<string>();
        

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
                button5.Enabled = true;
            else
                button5.Enabled = false;

            //читаем файлы по порядку и копируем данные в список RezFilStata
           
            //Получить имена файлов 
            var fileName = Directory.GetDirectories(patch);

            int schet = 0;

            foreach (char sim in fileName[0])
            {
                if (sim == '\\')
                {
                    schet++;
                }
            }
            
            //char[] charCount = (char[])fileName[0];

            foreach (var item in fileName)
            {
                string nameFolder = item.Split('\\')[schet];
                spisokPapok.Add(nameFolder);
            }
           
            //Проверка ввода на совподение
            string text = "MESSAGE_FROM_FIFA_TEAM_";
            text += textBox1.Text;

            foreach (var item in spisokPapok)
            {
                if (text == item)
                {
                    pictureBox3.Visible = true;
                    pictureBox4.Visible = false;
                    break;
                }
                else
                {
                    pictureBox4.Visible = true;
                    pictureBox3.Visible = false;
                }
            }
        }

        //Создаём папку с файлами
        private void button5_Click(object sender, EventArgs e)
        {
            //Создаём саму папку
            Directory.CreateDirectory(patch + @"\MESSAGE_FROM_FIFA_TEAM_" + textBox1.Text);

            //Переносим в неё картинку и маркер
            pictureBox1.Image.Save(patch + @"\MESSAGE_FROM_FIFA_TEAM_" + textBox1.Text + @"\MESSAGE_FROM_FIFA_TEAM_" + textBox1.Text + @"_stack.png");
            pictureBox2.Image.Save(patch + @"\MESSAGE_FROM_FIFA_TEAM_" + textBox1.Text + @"\MESSAGE_FROM_FIFA_TEAM_" + textBox1.Text + @".png");

            //Создаём json документ с координатами
            string filePach = patch + @"\MESSAGE_FROM_FIFA_TEAM_" + textBox1.Text + @"\actions.json";
            string nameFolder = "MESSAGE_FROM_FIFA_TEAM_" + textBox1.Text;
            string fullPach = patch + @"\MESSAGE_FROM_FIFA_TEAM_" + textBox1.Text + @"\";

            //Считаем координаты
            int X = rect.X - Convert.ToInt32(textBox2.Text.Replace("-", ""));
            int Y = rect.Y - Convert.ToInt32(textBox3.Text.Replace("-", ""));
            int W = rect.Width + Convert.ToInt32(textBox4.Text);
            int H = rect.Height + Convert.ToInt32(textBox5.Text);

            //пишим в фаил
            using (FileStream fs = new FileStream(filePach, FileMode.OpenOrCreate))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(@"{");
                    sw.WriteLine(@" ""marker"": {");
                    sw.WriteLine(@"    ""type"": ""image"",");
                    sw.WriteLine(@"    ""body"": {");
                    sw.WriteLine($@"    ""markerId"": ""{nameFolder}"",");
                    sw.WriteLine(@"      ""accuracy"": 90,");
                    sw.WriteLine(@"      ""searchArea"": {");
                    sw.WriteLine($@"        ""x"": {X},");
                    sw.WriteLine($@"        ""y"": {Y},");
                    sw.WriteLine($@"        ""width"": {W},");
                    sw.WriteLine($@"        ""height"": {H}");
                    sw.WriteLine(@"      }");
                    sw.WriteLine(@"    }");
                    sw.WriteLine(@"  },");
                    sw.WriteLine($@"  ""action"": [");
                    sw.WriteLine(@"    {");
                    sw.WriteLine($@"      ""button"": ""cross"",");
                    sw.WriteLine($@"      ""action"": ""press"",");
                    sw.WriteLine($@"      ""args"": ""6""");
                    sw.WriteLine(@"    }");
                    sw.WriteLine(@"  ]");
                    sw.WriteLine(@"}");
                }
            //Открываем созданную папку
           
            System.Diagnostics.Process.Start(Path.GetDirectoryName(fullPach));  // Открываем папку с файлом

            //Копируем название папки в буфер
            Clipboard.SetText(nameFolder.ToString());    
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //Чистим картинку и флаг
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            //Выводим ДрагэндДроп
            panel1.Visible = true;
            label1.Text = "Перетащите сюда картинку";
            //Отключаем кнопку создать папку
            button5.Enabled = false;
            //Кнопки увеличения маркера
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            //Кнопки уменьшения маркера
            button6.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
            button9.Enabled = false;
            //Расширения маркера
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            //Чистим текст бокс имени папки
            textBox1.Text = "";
            //Чистим координаты
            label10.Text = "0";
            label11.Text = "0";
            label12.Text = "0";
            label13.Text = "0";
            //Скрывае крестик и галку
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
        }
        //Выбор папки для сохранения
        string folderPach = "";
        private void button13_Click(object sender, EventArgs e)
        {
            //Открывваем выбор пути к папке
            
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                folderPach = folderBrowserDialog1.SelectedPath;
                patch = folderPach;
            }
            //Создаём файлик с путём к папке
            using (FileStream fs = new FileStream(@".\FolderPach.txt", FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(folderPach);   
            }

            if(File.Exists(@".\FolderPach.txt"))
            {
                label4.Text = "Папка для сохранения выбрана";
                label4.ForeColor = Color.Gray;
                textBox1.Enabled = true;
            }
        }   
    }
}
