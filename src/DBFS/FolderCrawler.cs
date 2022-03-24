using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;

namespace DBFS
{
    class FolderCrawler
    {
        /* ***** ATRIBUT ***** */
        // Starting Directory, Filename, Algorithm Choice, Form1
        private string startingDirectory;
        private string fileToFind;
        private int algorithm; // 1: BFS; 2: DFS
        private bool findAll;
        private Form1 form1;

        /* Graph Nodes */
        private List<string> listOfNode;
        private List<string> parentNode;
        private List<string> childNode;

        /* Graph */
        private Graph graph;

        /* ***** METHOD ***** */
        /* User-Defined Constructor */
        public FolderCrawler(string startingDirectory, string fileToFind, int algorithm, bool findAll, Form1 form1)
        {
            this.startingDirectory = startingDirectory;
            this.fileToFind = fileToFind;
            this.algorithm = algorithm;
            this.findAll = findAll;
            this.form1 = form1;

            /* Graph Node */
            listOfNode = new List<string>();
            parentNode = new List<string>();
            childNode = new List<string>();
            this.ConstructGraphNodes(this.startingDirectory);

            /* Graph */
            graph = new Graph("graph");
            this.ConstructGraph();
        }

        /* Graph Nodes Constructor */
        public void ConstructGraphNodes(string directory)
        {
            /* KAMUS LOKAL */
            string[] files;
            string[] subDirectories;

            /* ALGORITMA */
            files = Directory.GetFiles(directory);
            subDirectories = Directory.GetDirectories(directory);
            this.listOfNode.Add(Path.GetFileName(directory));

            foreach (string file in files)
            {
                this.parentNode.Add(Path.GetFileName(directory));
                this.childNode.Add(Path.GetFileName(file));
                this.listOfNode.Add(Path.GetFileName(file));
            }

            foreach (string subDirectory in subDirectories)
            {
                this.parentNode.Add(Path.GetFileName(directory));
                this.childNode.Add(Path.GetFileName(subDirectory));
                ConstructGraphNodes(subDirectory);
            }
        }

        /* Graph Constructor */
        public void ConstructGraph()
        {
            /* KAMUS LOKAL */

            /* ALGORITMA */
            for (int i = 0; i < this.parentNode.Count; i++)
            {
                graph.AddEdge(parentNode[i], childNode[i]);
            }
        }

        /* Get & Set */
        public string getStartingDirectory()
        {
            return this.startingDirectory;
        }

        public string getFileToFind()
        {
            return this.fileToFind;
        }

        public List<string> getListOfNode()
        {
            return this.listOfNode;
        }

        public List<string> getParentNode()
        {
            return this.parentNode;
        }

        public List<string> getChildNode()
        {
            return this.childNode;
        }

        public bool getFindAll()
        {
            return this.findAll;
        }

        public Form1 getForm()
        {
            return this.form1;
        }

        public async Task updateGraphAsync(string node, int colorCode)
        {
            if (colorCode == 0)
            {
                await PutTaskDelay();
                this.form1.SuspendLayout();
                graph.FindNode(node).Attr.Color = Color.Green;
                this.form1.ResumeLayout();
                this.form1.draw(graph);
            } 
            else
            {
                await PutTaskDelay();
                this.form1.SuspendLayout();
                graph.FindNode(node).Attr.Color = Color.Red;
                this.form1.ResumeLayout();
                this.form1.draw(graph);
            }
        }

        async Task PutTaskDelay()
        {
            await Task.Delay(2000);
        }


        /* *** KASUS SEARCHER DIPISAH *** */
        /* Search */
        public void Search()
        {
            form1.draw(graph);

            if (this.algorithm == 1)
            {
                // BFS
                BFS bfs = new BFS(this.form1, this, this.graph);
                bfs.processBFS();
            }
            else
            {
                // DFS
                DFS dfs = new DFS(this.form1, this, this.graph);
                dfs.processDFS();
            }
        }

        
    }
}
