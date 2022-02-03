using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cipher
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

            richTextBox1.AllowDrop = true;
            richTextBox1.DragDrop += new DragEventHandler(DragDropRichTextBox_DragDrop);
            openFileDialog1.FileName = "";
        }

        private void DragDropRichTextBox_DragDrop(object sender, DragEventArgs e)
        {
            string[] fileNames = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (fileNames != null)
            {
                foreach (string name in fileNames)
                {
                    try
                    {
                        richTextBox1.AppendText(File.ReadAllText(name));
                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show(exp.Message);
                    }
                }
            }
        }
       
        private void openToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.ShowDialog();
                StreamReader reader = new StreamReader(openFileDialog1.FileName);
                richTextBox1.Text = reader.ReadToEnd();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void closeToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to close the program?", "Cipher", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private bool CheckStringForUniqueness(string Text)
        {
            for (int i = 0; i < Text.Length; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (Text[i] == Text[j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        private string StartWorking(string password)
        {
            var polybius = new PolybiusSquare();
            string text = "";
            //text = polybius.PolybiusDecrypt(richTextBox1.Text.ToUpper(), password.ToUpper());

            text = polybius.PolibiusEncrypt(richTextBox1.Text, password);
            return text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                richTextBox2.Text = StartWorking("");
            }
            else
            {
                if (CheckStringForUniqueness(textBox1.Text))
                {
                    richTextBox2.Text = StartWorking(textBox1.Text);
                }
                else
                {
                    MessageBox.Show("The password must only contain unique characters.", "Cipher", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox1.Text = "";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 form3= new Form3();
            form3.Show();
            form3.textBox1.Text = textBox1.Text;
            form3.richTextBox1.Text = richTextBox2.Text;
            Hide();
        }

        private void claerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            richTextBox1.Text = "";
            richTextBox2.Text = "";
        }

        private void findAgainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool allOkay = false;
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    myStream.Close();
                    allOkay = true;
                }
            }
            if (allOkay)
            {
                File.WriteAllText(saveFileDialog1.FileName, "Pasword:\n" + textBox1.Text + "\n\nCode:\n" + richTextBox2.Text);
            }
        }
    }
}
