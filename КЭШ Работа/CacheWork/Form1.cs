using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace CacheWork
{
    public partial class Form1 : Form
    {
        Controller controller;
        int AdresI, AdresJ, AdresK;
        int CountPages,
            CountLines,
            CountElements;
        int Value;
        int[] str;

        Stopwatch Time = new Stopwatch();
        public Form1()
        {
            InitializeComponent();
            CountPages = Convert.ToInt32(textBox_Page.Text);
            CountLines = Convert.ToInt32(textBox_String.Text);
            CountElements = Convert.ToInt32(textBox_Items.Text);
            controller = new Controller(CountPages, CountLines, CountElements, "MainMemory");
            label_WhereFrom.Text = "Ни один элемент пока не был загружен";
            label_WhereFrom.ForeColor = Color.Black;
            button_Create.Enabled = false;
            WriteToTextBox_OP();
            WriteToTextBox_Cache();
            textBox_OP.ReadOnly = true;
            textBox_Cache.ReadOnly = true;
        }

        private void button_Change_Click(object sender, EventArgs e)
        {
            try
            {
                AdresI = Convert.ToInt32(textBox_PageSearch.Text);
                AdresJ = Convert.ToInt32(textBox_StringSearch.Text);
                AdresK = Convert.ToInt32(textBox_ItemSearch.Text);
                Value = Convert.ToInt32(textBox_ItemChange.Text);
            }
            catch
            {
                MessageBox.Show("Данные для поиска введены некорректно!");
                return;
            }

            Time.Start();
            str = controller.SearchLine(AdresI, AdresJ);
            Time.Stop();

            if (controller.IsCache)
            {
                label_WhereFrom.Text = "Элемент загружен из Кэша";
                label_WhereFrom.ForeColor = Color.Red;
            }
            else
            {
                label_WhereFrom.Text = "Элемент загружен из Оп";
                label_WhereFrom.ForeColor = Color.Blue;
            }

            str[AdresK] = Value; // сохраняем нужный элемент
            controller.SetLineOnCache(str, CountElements, AdresJ);
            label_ItemFrom.Text = Value.ToString();
            label_StringFrom.Text = " ";
            for (int i = 0; i < CountElements; i++)
            {
                label_StringFrom.Text += str[i].ToString() + " ";
            }

            WriteToTextBox_Cache();
            WriteToTextBox_OP();
            label_TimeFrom.Text = Time.Elapsed.ToString();
            Time.Reset();
        }

        private void button_Search_Click(object sender, EventArgs e)
        {
            try
            {
                AdresI = Convert.ToInt32(textBox_PageSearch.Text);
                AdresJ = Convert.ToInt32(textBox_StringSearch.Text);
                AdresK = Convert.ToInt32(textBox_ItemSearch.Text);
            } catch
            {
                MessageBox.Show("Данные для поиска введены некорректно!");
                return;
            }

            Time.Start();
            str = controller.SearchLine(AdresI, AdresJ);
            Time.Stop();

            if (controller.IsCache)
            {
                label_WhereFrom.Text = "Элемент загружен из Кэша";
                label_WhereFrom.ForeColor = Color.Red;
            } else
            {
                label_WhereFrom.Text = "Элемент загружен из Оп";
                label_WhereFrom.ForeColor = Color.Blue;
            }

            Value = str[AdresK]; // загружаем нужный элемент
            label_ItemFrom.Text = Value.ToString();
            label_StringFrom.Text = " ";
            for (int i = 0; i < CountElements; i++)
            {
                label_StringFrom.Text += str[i].ToString() + " ";
            }
            
            WriteToTextBox_Cache();
            WriteToTextBox_OP();
            label_TimeFrom.Text = Time.Elapsed.ToString();
            Time.Reset();
        }

        private void button_Create_Click(object sender, EventArgs e)
        {
            WriteToTextBox_OP();
        }

        private void WriteToTextBox_OP()
        {
            textBox_OP.Text = "";
            for (int i = 0; i < CountPages; i++)
            {
                for (int j = 0; j < CountLines; j++)
                {
                    for (int k = 0; k < CountElements; k++)
                    {
                        textBox_OP.Text += (controller[i, j, k].ToString() + " ");
                    }
                    textBox_OP.Text += Environment.NewLine;
                }
                textBox_OP.Text += Environment.NewLine;
            }
            textBox_OP.Text += Environment.NewLine;
        }

        private void WriteToTextBox_Cache()
        {
            textBox_Cache.Text = "";
            for (int i = 0; i < CountLines; i++)
            {
                textBox_Cache.Text += controller[i] + "    ";
                for (int j = 0; j < CountElements; j++)
                {
                    textBox_Cache.Text += controller[i, j].ToString() + " ";
                }
                textBox_Cache.Text += Environment.NewLine;
            }
            textBox_Cache.Text += Environment.NewLine;
        }
    }
}
