﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TelephoneDictWind
{
    public partial class Form1 : Form
    {
        Dictionary<string, string> dict;
        OperationWithDict myTelDic = new OperationWithDict();
        string path = "";
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Закрыть?", "Message", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                e.Cancel = true;
            else
                e.Cancel = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox3.Enabled = true;
            textBox3.Text = dict[listBox1.SelectedValue.ToString()];
        }

        private void button3_Click(object sender, EventArgs e)
        {

            string valueDict = textBox2.Text;
            string keyDict = maskedTextBox1.Text;
            Dictionary<string, string> tmp = new Dictionary<string, string>();
            tmp.Add(keyDict, valueDict);
            myTelDic.TelephoneDirectory = tmp;

            //if (!dic.TryGetValue(find, out rez))
            //{
            //    rez = "ничего не найдено";
            //}
            //textBox2.Text = rez;
            if (string.IsNullOrEmpty(path))
            {
                label4.Text = "Имя файла отсутствует";
            }
            else
            {
                myTelDic.Save(myTelDic.TelephoneDirectory, path, false);
                label4.Text = "Файл успешно записан.\nПодтвердите адрес для обновления списка номеров в окне.";
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            path = textBox1.Text;

            if (string.IsNullOrEmpty(path))
            {
                label4.Text = "Недопустимое имя файла. Попробуйте еще раз.";
            }
            else
            {
                label4.Text = "Принято имя файла.";
                textBox2.Enabled = true;
                maskedTextBox1.Enabled = true;
                if (!(File.Exists(path)))
                {
                    label4.Text += "\nДанный файл не был записан ранее,\n невозможно его прочитать";
                }
                else
                {
                    myTelDic.Read(path);
                    dict = myTelDic.TelephoneDirectory;
                    listBox1.DataSource = dict.Keys.ToList();
                    button4.Enabled = true;

                }
                button2.Enabled = true;
                button3.Enabled = true;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string valueDict = textBox2.Text;
            string keyDict = maskedTextBox1.Text;
            Dictionary<string, string> tmp = new Dictionary<string, string>();
            tmp.Add(keyDict, valueDict);
            myTelDic.TelephoneDirectory = tmp;

            //if (!dic.TryGetValue(find, out rez))
            //{
            //    rez = "ничего не найдено";
            //}
            //textBox2.Text = rez;
            if (string.IsNullOrEmpty(path))
            {
                label4.Text = "Имя файла не указано";

            }
            else
            {
                myTelDic.Save(myTelDic.TelephoneDirectory, path, true);
                label4.Text = "Файл успешно дополнен.\nПодтвердите адрес для обновления списка номеров в окне.";

            }
        }

    }
}