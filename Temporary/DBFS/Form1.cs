using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public Form1()
        {
            InitializeComponent();
        }

        /* Aksi Ketika Menekan Button1 */
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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            this.fileToFind = textBox2.Text;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.findAll = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.algorithm = 1;
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.algorithm = 2;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.startingDirectory != null && this.fileToFind != null)
            {
                FolderCrawler fc = new FolderCrawler(this.startingDirectory, this.fileToFind, this.algorithm, this.findAll, this);
                fc.Search(gViewer1);
            }
        }

        public void draw(Graph graph)
        {
            this.gViewer1.Graph = graph;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}