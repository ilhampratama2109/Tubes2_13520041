using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DBFS
{
    public class DirectoryProcessor
    {
        /* *** ATRIBUT *** */

        // Directory awal: starting folder for file finder
        private string startingDirectory;

        // Form1
        private Form1 form1;

        // Array of parentNode and childNode and its index
        public string[] parentNode;
        public string[] childNode;
        private int index;

        // List of Node
        public List<string> listOfNode = new List<string>();
        
        // Graph and its visualizer
        private Graph graph;
        private Microsoft.Msagl.Drawing.Graph visualGraph;
        

        /* *** METHOD *** */

        /* Constructor: Membuat kelas DirectoryProcessor */
        public DirectoryProcessor(string startingDirectory, int size, Form1 form1)
        {
            this.startingDirectory = startingDirectory;
            this.index = 0;
            this.form1 = form1;
            this.parentNode = new string[size];
            this.childNode = new string[size];
            this.setupNodes(this.startingDirectory);
            this.setupGraph(this.listOfNode);
        }

        /* Setup Nodes: 
         * Mengelola parentNode dan childNode serta 
         * Membuat List of Node */
        public void setupNodes(string path)
        {
            // Memperoleh files yang tersedia pada path
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                this.parentNode[index] = path;
                this.childNode[index] = file;
                this.index++;
            }

            // Memperoleh folder yang tersedia pada path
            string[] SubDirs = Directory.GetDirectories(path);
            foreach (string subdir in SubDirs)
            {
                this.parentNode[index] = path;
                this.childNode[index] = subdir;
                this.index++;

                // Rekursi untuk setiap sub folder/subdirectory
                setupNodes(subdir);
            }

            // Membuat listOfNode
            listOfNode = parentNode.ToArray().Union(childNode).Distinct().ToList();
        }

        /* Setup Graph: 
         * Membuat graph, 
         * Menambahkan node dari list of node, dan 
         * Menambahkan edge untuk setiap pasangan parentNode[i] dan childNode[i] 
         */
        public void setupGraph(List<string> listOfNode)
        {
            // Membuat graph
            graph = new Graph(listOfNode.Count());

            // Menambahkan node pada graph dengan indeks node pada list of node
            foreach (string node in listOfNode)
            {
                graph.AddNode(listOfNode.IndexOf(node));
            }

            // Menambahkan edge pada graph untuk setiap pasangan parentNode[i] dan childNode[i]
            // dengan indeks node pada list of node
            for (int i = 0; i < index; i++)
            {
                graph.AddEdge(listOfNode.IndexOf(childNode[i]), listOfNode.IndexOf(parentNode[i]));
            }
        }



        public Microsoft.Msagl.Drawing.Graph process()
        {
            this.visualGraph = new Microsoft.Msagl.Drawing.Graph("graph");



            for (int i = 0; i < this.index; i++)
            {

                // graph.AddEdge(this.parentNode[i], this.childNode[i]).Attr.Color = Microsoft.Msagl.Drawing.Color.MediumSpringGreen;
                var Edge = visualGraph.AddEdge(this.parentNode[i], this.childNode[i]);

                Edge.Attr.ArrowheadAtTarget = Microsoft.Msagl.Drawing.ArrowStyle.None;
                Edge.Attr.ArrowheadAtSource = Microsoft.Msagl.Drawing.ArrowStyle.None;
                Edge.Attr.Color = Microsoft.Msagl.Drawing.Color.SpringGreen;
            }

            return this.visualGraph;
        }
    }
}