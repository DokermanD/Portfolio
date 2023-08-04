﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnstackerFix.MarkerScript;

namespace UnstackerFix
{
    public partial class Form1 : Form
    {
        //Выбор папки для сохранения
        private string folderPatch = "";

        private string patch;
        private string[] patchImage;
        private Rectangle rect;

        //Проверка при вводе имени папки на дубли
        private readonly List<string> spisokPapok = new List<string>();
        private int X, Y, W, H;
        
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
            if (File.Exists(@".\FolderPatch.txt"))
            {
                label4.Text = "Папка для сохранения выбрана";
                label4.ForeColor = Color.Gray;
                textBox1.Enabled = true;
                patch = File.ReadAllText(@".\FolderPatch.txt").Trim();
            }
        }

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
            patchImage = (string[])e.Data.GetData(DataFormats.FileDrop);
            pictureBox1.Image = Image.FromFile(patchImage[0]);
            panel1.Visible = false;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                pictureBox2.Visible = true;

                var pb = pictureBox1;

                var bmp1 = pb.Image as Bitmap;
                var bmp2 = bmp1.Clone(rect, bmp1.PixelFormat);
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
                var g = pictureBox1.CreateGraphics();
                g.DrawImage(pictureBox1.Image, 0, 0);

                var kor = e.Location;
                X = kor.X;
                Y = kor.Y;
                W = 15;
                H = 15;

                var pero = new Pen(Color.Red, 1);
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
            var g = pictureBox1.CreateGraphics();
            g.DrawImage(pictureBox1.Image, 0, 0);

            var X = rect.X;
            var Y = rect.Y;
            var W = rect.Width;
            var H = rect.Height;

            var pero = new Pen(Color.Red, 1);
            rect = new Rectangle(X - 1, Y, W + 1, H);
            g.DrawRectangle(pero, rect);

            var pb = pictureBox1;

            var bmp1 = pb.Image as Bitmap;
            var bmp2 = bmp1.Clone(rect, bmp1.PixelFormat);
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
            var g = pictureBox1.CreateGraphics();
            g.DrawImage(pictureBox1.Image, 0, 0);

            var X = rect.X;
            var Y = rect.Y;
            var W = rect.Width;
            var H = rect.Height;

            var pero = new Pen(Color.Red, 1);
            rect = new Rectangle(X, Y, W + 1, H);
            g.DrawRectangle(pero, rect);

            var pb = pictureBox1;

            var bmp1 = pb.Image as Bitmap;
            var bmp2 = bmp1.Clone(rect, bmp1.PixelFormat);
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
            var g = pictureBox1.CreateGraphics();
            g.DrawImage(pictureBox1.Image, 0, 0);

            var X = rect.X;
            var Y = rect.Y;
            var W = rect.Width;
            var H = rect.Height;

            var pero = new Pen(Color.Red, 1);
            rect = new Rectangle(X, Y - 1, W, H + 1);
            g.DrawRectangle(pero, rect);

            var pb = pictureBox1;

            var bmp1 = pb.Image as Bitmap;
            var bmp2 = bmp1.Clone(rect, bmp1.PixelFormat);
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
            var g = pictureBox1.CreateGraphics();
            g.DrawImage(pictureBox1.Image, 0, 0);

            var X = rect.X;
            var Y = rect.Y;
            var W = rect.Width;
            var H = rect.Height;

            var pero = new Pen(Color.Red, 1);
            rect = new Rectangle(X, Y, W, H + 1);
            g.DrawRectangle(pero, rect);

            var pb = pictureBox1;

            var bmp1 = pb.Image as Bitmap;
            var bmp2 = bmp1.Clone(rect, bmp1.PixelFormat);
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
            var g = pictureBox1.CreateGraphics();
            g.DrawImage(pictureBox1.Image, 0, 0);

            var X = rect.X;
            var Y = rect.Y;
            var W = rect.Width;
            var H = rect.Height;

            var pero = new Pen(Color.Red, 1);
            rect = new Rectangle(X + 1, Y, W - 1, H);
            g.DrawRectangle(pero, rect);

            var pb = pictureBox1;

            var bmp1 = pb.Image as Bitmap;
            var bmp2 = bmp1.Clone(rect, bmp1.PixelFormat);
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
            var g = pictureBox1.CreateGraphics();
            g.DrawImage(pictureBox1.Image, 0, 0);

            var X = rect.X;
            var Y = rect.Y;
            var W = rect.Width;
            var H = rect.Height;

            var pero = new Pen(Color.Red, 1);
            rect = new Rectangle(X, Y, W - 1, H);
            g.DrawRectangle(pero, rect);

            var pb = pictureBox1;

            var bmp1 = pb.Image as Bitmap;
            var bmp2 = bmp1.Clone(rect, bmp1.PixelFormat);
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
            var g = pictureBox1.CreateGraphics();
            g.DrawImage(pictureBox1.Image, 0, 0);

            var X = rect.X;
            var Y = rect.Y;
            var W = rect.Width;
            var H = rect.Height;

            var pero = new Pen(Color.Red, 1);
            rect = new Rectangle(X, Y + 1, W, H - 1);
            g.DrawRectangle(pero, rect);

            var pb = pictureBox1;

            var bmp1 = pb.Image as Bitmap;
            var bmp2 = bmp1.Clone(rect, bmp1.PixelFormat);
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
            var g = pictureBox1.CreateGraphics();
            g.DrawImage(pictureBox1.Image, 0, 0);

            var X = rect.X;
            var Y = rect.Y;
            var W = rect.Width;
            var H = rect.Height;

            var pero = new Pen(Color.Red, 1);
            rect = new Rectangle(X, Y, W, H - 1);
            g.DrawRectangle(pero, rect);

            var pb = pictureBox1;

            var bmp1 = pb.Image as Bitmap;
            var bmp2 = bmp1.Clone(rect, bmp1.PixelFormat);
            pictureBox2.Image = bmp2;

            //Выводим координаты маркера
            label10.Text = rect.X.ToString();
            label11.Text = rect.Y.ToString();
            label12.Text = rect.Width.ToString();
            label13.Text = rect.Height.ToString();
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
                button5.Enabled = true;
            else
                button5.Enabled = false;

            //читаем файлы по порядку и копируем данные в список RezFilStata

            //Получить имена файлов 
            var fileName = Directory.GetDirectories(patch);

            if (spisokPapok.Count == 0)
            {
                foreach (var item in fileName)
                {
                    var nameFolder = item.Split('\\').Last();
                    spisokPapok.Add(nameFolder);
                }
            }


            //Проверка ввода на совпадение
            var text = "MESSAGE_FROM_FIFA_TEAM_";
            text += textBox1.Text;

            foreach (var item in spisokPapok)
            {
                if (text == item)
                {
                    pictureBox3.Visible = true;
                    pictureBox4.Visible = false;
                    break;
                }

                pictureBox4.Visible = true;
                pictureBox3.Visible = false;
            }
        }


        PictureBox _resetImage = new PictureBox();
        /// <summary>
        /// Проверка маркера для Script
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button15_Click(object sender, EventArgs e)
        {
            if (_resetImage.Image != null) pictureBox1.Image = _resetImage.Image;
            
            var userAnswer = openFileDialog1.ShowDialog();
            if (userAnswer != DialogResult.OK) return;
            var inputImage = new Image<Bgr, byte>(openFileDialog1.FileName);

            pictureBox6.Image = inputImage.ToBitmap();
            var marker = new SearchMarker(pictureBox1.Image, pictureBox6.Image);
            _resetImage.Image = pictureBox1.Image;
            pictureBox1.Image = marker.MarkerSearchMethod() ?? pictureBox1.Image;
        }

        //Создаем папку с файлами для Skripta
        private void button14_Click(object sender, EventArgs e)
        {
            //Создаём саму папку
            var path = Path.Combine(patch, textBox6.Text);
            Directory.CreateDirectory(path);

            //Переносим в неё картинку и маркер
            pictureBox1.Image.Save(Path.Combine(path, textBox6.Text + @"_stack.png"));
            pictureBox2.Image.Save(Path.Combine(path, textBox6.Text + @".png"));

            //Создаем json документ с координатами
            var filePach = Path.Combine(path, textBox6.Text + ".json");
            var nameFolder = textBox6.Text;
            var fullPach = path + @"\";

            //Считаем координаты
            var X = rect.X - Convert.ToInt32(textBox2.Text.Replace("-", ""));
            var Y = rect.Y - Convert.ToInt32(textBox3.Text.Replace("-", ""));
            var W = rect.Width + Convert.ToInt32(textBox4.Text);
            var H = rect.Height + Convert.ToInt32(textBox5.Text);

            #region Сереализация и сохранение файла json
            // Сереализация
            var searchAreaSkript = new SearchAreaSkript
            {
                X = X,
                Y = Y,
                Width = W,
                Height = H
            };


            var modelJsonScript = new ModelJsonScript
            {
                MarkerId = textBox6.Text,
                SearchAreaSkript = searchAreaSkript,
                Accuracy = 90
            };


            var result = JsonConvert.SerializeObject(modelJsonScript, Formatting.Indented);

            //Пишем в файл
            using (var fs = new FileStream(filePach, FileMode.OpenOrCreate))
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine(result);
            }

            //Открываем созданную папку
            Process.Start(Path.GetDirectoryName(fullPach)); // Открываем папку с файлом

            //Копируем название папки в буфер
            //Clipboard.SetText(nameFolder);
            //Запускаем FileZilla
            //Process.Start(@"C:\Program Files\FileZilla FTP Client\filezilla.exe");
            #endregion
        }

        //Создаем папку с файлами для анстакера
        private void button5_Click(object sender, EventArgs e)
        {
            //Создаём саму папку
            var path = Path.Combine(patch, "MESSAGE_FROM_FIFA_TEAM_" + textBox1.Text);
            Directory.CreateDirectory(path);

            //Переносим в неё картинку и маркер
            pictureBox1.Image.Save(Path.Combine(path, "MESSAGE_FROM_FIFA_TEAM_", textBox1.Text + @"_stack.png"));
            pictureBox2.Image.Save(Path.Combine(path, "MESSAGE_FROM_FIFA_TEAM_", textBox1.Text + @".png"));

            //Создаем json документ с координатами
            var filePach = Path.Combine(path, "actions.json");
            var nameFolder = "MESSAGE_FROM_FIFA_TEAM_" + textBox1.Text;
            var fullPach = path + @"\";

            //Считаем координаты
            var X = rect.X - Convert.ToInt32(textBox2.Text.Replace("-", ""));
            var Y = rect.Y - Convert.ToInt32(textBox3.Text.Replace("-", ""));
            var W = rect.Width + Convert.ToInt32(textBox4.Text);
            var H = rect.Height + Convert.ToInt32(textBox5.Text);

            #region Сереализация и сохранение файла json
            // Сереализация
            var actionList = new List<Action>();
            var action = new Action
            {
                Button = "cross",
                Actions = "press",
                Args = "6"
            };
            actionList.Add(action);

            var searchArea = new SearchArea
            {
                X = X,
                Y = Y,
                Width = W,
                Height = H
            };

            var body = new Body
            {
                MarkerId = nameFolder,
                Accuracy = 90,
                SearchArea = searchArea
            };

            var marker = new Marker
            {
                Type = "image",
                Body = body
            };

            var modelJson = new ModelJson
            {
                Marker = marker,
                Action = actionList
            };

            var result = JsonConvert.SerializeObject(modelJson,Formatting.Indented);

            //Пишем в файл
            using (var fs = new FileStream(filePach, FileMode.OpenOrCreate))
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine(result);
            }

            //Открываем созданную папку
            Process.Start(Path.GetDirectoryName(fullPach)); // Открываем папку с файлом

            //Копируем название папки в буфер
            Clipboard.SetText(nameFolder);
            //Запускаем FileZilla
            Process.Start(@"C:\Program Files\FileZilla FTP Client\filezilla.exe");
            #endregion
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
            //Скрываем крестик и галку
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //Открываем выбор пути к папке

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                folderPatch = folderBrowserDialog1.SelectedPath;
                patch = folderPatch;
            }

            //Создаём файлик с путём к папке
            using (var fs = new FileStream(@".\FolderPatch.txt", FileMode.Create))
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine(folderPatch);
            }

            if (File.Exists(@".\FolderPatch.txt"))
            {
                label4.Text = "Папка для сохранения выбрана";
                label4.ForeColor = Color.Gray;
                textBox1.Enabled = true;
            }
        }
    }
}