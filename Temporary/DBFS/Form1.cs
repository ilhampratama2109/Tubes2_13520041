using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;

namespace DBFS
{
    public partial class Form1 : Form
    {
        private string startingDirectory;
        private string fileToFind;
        private int algorithm;
        private bool findAll;
        private string chosenPath;
        public Stopwatch stopwatch;

        public Form1()
        {
            InitializeComponent();
        }

        /* STARTING FOLDER INSERTION BUTTON */
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                /* Menerima input folder */
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    this.startingDirectory = fbd.SelectedPath;
                    textBox1.Text = this.startingDirectory;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /* FILENAME TO FIND INSERTION BOX */
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            this.fileToFind = textBox2.Text;
        }

        /* FIND ALL OCURRENCE BUTTON */
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.findAll = true;
        }

        /* ALGORITHM CHOOSE BUTTON */
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.algorithm = 1;
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.algorithm = 2;
        }

        /* START SEARCHING PROCESS */
        private void button2_Click(object sender, EventArgs e)
        {
            if (this.startingDirectory != null && this.fileToFind != null)
            {
                stopwatch = new Stopwatch();
                stopwatch.Start();
                FolderCrawler fc = new FolderCrawler(this.startingDirectory, this.fileToFind, this.algorithm, this.findAll, this);
                fc.Search(gViewer1);
                ComboBox comboBox1 = new ComboBox();
            }
        }

        /* HYPERLINK CHOOSE COMBOBOX */
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.chosenPath = comboBox1.SelectedItem.ToString();
        }

        /* OPEN HYPERLINK BUTTON */
        private void button3_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(this.chosenPath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = this.chosenPath,
                    FileName = "explorer.exe"
                };

                Process.Start(startInfo);
            }
        }

        /* COPY HYPERLINK BUTTOB */
        private void button4_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.chosenPath);
        }

        /* DRAW GRAPH TO VIEWER */
        public void draw(Graph graph)
        {
            this.gViewer1.Graph = graph;
            this.Controls.Add(gViewer1);
        }

        /* ADD HYPERLINK */
        public void addComboBoxElmt(String path)
        {
            this.comboBox1.Items.Add(path);
        }

        /* PRINT TIME TAKEN FOR SEARCHING (EXCLUDE VISUALIZATION) */
        public void writeTimeElapsed()
        {
            this.textBox4.Text = stopwatch.Elapsed.ToString(@"ss\.ff");
        }
    }
}