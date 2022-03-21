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
        private GViewer gviewer;

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
            gviewer = new GViewer();
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
            for (int i = 0; i < this.listOfNode.Count; i++)
            {
                graph.AddNode(listOfNode[i]);
            }

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

        public Form1 getForm()
        {
            return this.form1;
        }

        public void setNodeColor(int index, int colorCode)
        {
            if (index >= 0 && index < this.listOfNode.Count())
            {

            }
        }

        /* *** KASUS SEARCHER DIPISAH *** */
        /* Search */
        public void Search(GViewer gviewer)
        {
            form1.draw(graph);

            if (this.algorithm == 1)
            {
                // BFS
                if (this.findAll)
                {
                    // BFS bfs = new BFS(this, this.gviewer, this.graph, gviewer);
                    // bfs.doBFSAll(panel);
                }
                else
                {
                    // BFS bfs = new BFS(this, this.gviewer, this.graph, gviewer);
                    // bfs.doBFS(panel);
                }
            }
            else
            {
                // DFS
                if (this.findAll)
                {
                    // DFS dfs = new DFS(this, this.gviewer, this.graph, gviewer);
                    // dfs.doDFSAllpanel);
                }
                else
                {
                    // DFS dfs = new DFS(this, this.gviewer, this.graph, gviewer);
                    // dfs.doDFS();
                }
            }
        }
    }
}
