using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

namespace DBFS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                /* Deklarasi & Alokasi List of String: Menyimpan nama file dan folder */
                List<string> AllFiles = new List<string>();

                /* Menerima input folder */
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string startingDir = fbd.SelectedPath;
                    textBox1.Text = startingDir; // Mencetak path folder di textBox1

                    // Menjalankan ParsePath: Mencari folder & file dari  
                    ParsePath(startingDir);

                    // 
                    void ParsePath(string path)
                    {
                        string[] SubDirs = Directory.GetDirectories(path);  // Membuka directory
                        AllFiles.AddRange(Directory.GetFiles(path));        // Membaca file
                        AllFiles.AddRange(SubDirs);                         // Membaca folder
                        foreach (string subdir in SubDirs)                  // Membuka setiap folder
                        {
                            ParsePath(subdir);
                        }
                    }
                }

                /* Mencetak isi AllFiles: File/Folder yang ditemukan */
                foreach (string file in AllFiles)
                {
                    string filename = Path.GetFileName(file);
                    textBox2.Text = textBox2.Text + "\r\n" + filename;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
    }
}