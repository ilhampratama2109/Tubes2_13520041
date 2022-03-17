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
        /* ATRIBUT */
        private string startingDirectory;
        public string[] parentNode;
        public string[] childNode;
        private List<string> listOfNode = new List<string>();
        private int index;
        private Graph graph;
        private Microsoft.Msagl.Drawing.Graph visualGraph;
        private Form1 form1;

        /* METHOD */

        /* Constructor: Membuat ... */
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

        /*  */
        public void setupNodes(string path)
        {
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                this.parentNode[index] = path;
                this.childNode[index] = file;
                this.index++;
            }
            string[] SubDirs = Directory.GetDirectories(path);  // Membuka directory
            foreach (string subdir in SubDirs)                  // Membuka setiap folder
            {
                this.parentNode[index] = path;
                this.childNode[index] = subdir;
                this.index++;
                setupNodes(subdir);
            }

            listOfNode = parentNode.ToArray().Union(childNode).Distinct().ToList();
        }

        public void setupGraph(List<string> listOfNode)
        {
            graph = new Graph(listOfNode.Count());
            foreach (string node in listOfNode)
            {
                graph.AddNode(listOfNode.IndexOf(node));
            }
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
