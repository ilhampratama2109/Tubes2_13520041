using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;

namespace DBFS
{
    class DFS
    {
        /* ***** ATRIBUT ***** */
        private Form1 form1;
        private FolderCrawler fc;
        private Graph graph;

        /* Graph Nodes */
        private List<string> listOfNode;
        private List<string> parentNode;
        private List<string> childNode;

        /* Algorithm Atribute */
        private bool answerExist;
        private bool[] isSolution;
        private bool[] visited;
        private List<int> idxsolution;

        /* ***** METHOD ***** */
        public DFS(Form1 f1, FolderCrawler fc, Graph g)
        {
            this.form1 = f1;
            this.fc = fc;
            this.graph = g;
            this.listOfNode = fc.getListOfNode();
            this.parentNode = fc.getParentNode();
            this.childNode = fc.getChildNode();

            this.isSolution = new bool[listOfNode.Count];
            this.visited = new bool[listOfNode.Count];
            this.idxsolution = new List<int>();
        }

        // ALGORITMA proses DFS
        public void processDFS()
        {
            // Root dikunjungi
            this.visited[0] = true;
            // Node dimasukkan ke path
            this.idxsolution.Add(0);
            // Node diwarnai merah
            graph.FindNode(listOfNode[0]).Attr.Color = Color.Red;

            // Mencatat adjacent child dari root
            List<int> adjchild = returnAdjacentNodes(listOfNode[0]);
            // for (int i = 0; i < adjchild.Count; i++)
            // {
            //     Console.WriteLine(this.childNode[adjchild[i]]);
            // }

            bool found = false;
            for (int i = 0; i < adjchild.Count; i++)
            {
                string ctarget = childNode[adjchild[i]];
                if (childIdxInLON(ctarget) < this.listOfNode.Count)
                {
                    if (!this.fc.getFindAll() && answerExist)
                    {
                        // do nothing
                    }
                    else
                    {
                        found = recursiveDFS(childIdxInLON(ctarget));
                    }
                }
            }

            if (this.answerExist){
                for (int i = 0; i < listOfNode.Count; i++)
                {
                    if (isSolution[i])
                    {
                        graph.FindNode(listOfNode[i]).Attr.Color = Color.Green;
                    }
                }
            }
            else
            {
                MessageBox.Show("File tidak ditemukan!");
            }
        }

        private bool recursiveDFS(int nodeIdx)
        {   
            // Node urutan nodeIdx dikunjungi
            Console.WriteLine(this.listOfNode[nodeIdx]);
            this.visited[nodeIdx] = true;
            // Node dimasukkan ke path
            this.idxsolution.Add(nodeIdx);
            // Node diwarnai merah
            graph.FindNode(listOfNode[nodeIdx]).Attr.Color = Color.Red;

            // Pengecekan apakah node ini merupakan file target
            if (this.fc.getFileToFind() == this.listOfNode[nodeIdx])
            {
                // Node target ditemukan
                this.answerExist = true;
                // print path
                
                // path solusi dimasukan ke issolution
                foreach (int idx in this.idxsolution)
                {
                    isSolution[idx] = true;
                }
                
                if (this.fc.getFindAll())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            // Apabila node ini bukan target, lanjut kunjungi child
            // Mencatat adjacent child dari root
            List<int> adjchild = returnAdjacentNodes(listOfNode[nodeIdx]);

            // Ada kemungkinan node ini tidak punya child
            // Sehingga diinisialisasi dengan false
            bool found = false;
            for (int i = 0; i < adjchild.Count; i++)
            {
                string ctarget = childNode[adjchild[i]];
                if (childIdxInLON(ctarget) < this.listOfNode.Count)
                {
                    if (!this.fc.getFindAll() && answerExist)
                    {
                        // do nothing
                    }
                    else
                    {
                        found = recursiveDFS(childIdxInLON(ctarget));
                    }
                }
            }

            if (!found)
            {
                // Hapus node dari path pencarian yang valid
                this.idxsolution.Remove(nodeIdx);
            }
            return found;
        }

        // Method untuk mencatat child yang adjacent dengan parent yang dikunjungi
        private List<int> returnAdjacentNodes(string parentnode)
        {
            List<int> adjchild = new List<int>();
            for (int i = 0; i < this.parentNode.Count; i++)
            {
                if (parentNode[i] == parentnode)
                {
                    adjchild.Add(i);
                }
            }


            return adjchild;
        }

        // Method untuk mencari index node childTarget pada listOfNode
        private int childIdxInLON(string childTarget)
        {
            int i = 0;
            bool found = false;
            while (!found)
            {
                if (this.listOfNode[i] == childTarget)
                {
                    found = true;
                } 
                else
                {
                    i++;
                }
            }
            return i;
        }
    }
}

/*
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/* Data di DirectoryProcessor (barangkali perlu)
private string startingDirectory;
public string[] parentNode;
public string[] childNode;
private List<string> listOfNode = new List<string>();
private int index;
private Graph graph;
private Microsoft.Msagl.Drawing.Graph visualGraph;
private Form1 form1;



namespace DBFS{
    class BFS{
        // Atribut kelas BFS
        private List<int> path; // List untuk menyimpan data path yang dilalui
        private DirectoryProcessor dirProcessor; // Deklarasi prosesor direktori
        private bool[] isVisited; // array of boolean untuk menyimpan status dikunjungi sebuah node

        public List<int> startSearch(Graph graph, int startnode, int targetnode){
            isVisited = new bool[graph.CountNode()];
            isVisited[0] = true; // Node pertama sudah dikunjungi
            for (int i = 1; i < graph.CountNode(); i++){
                isVisited[i] = false; // Selain node pertama diisi "belum dikunjungi"
            }
            path = new List<int>();
            path.Add(startnode); // Catat path node pertama
        }
        private bool DFSrecursive(Graph graph, int startnode, int targetnode){
            
            if (startnode == targetnode){
                return true;
            }
            // Implementasi rekurens dengan mencari child yang terhubung
            // Lakukan pemanggilan DFSrecursive pada child tersebut
            boolean targetfound = false;
            for (int i = startnode; i < graph.CountNode(); i++){
                if (FindEdge(startnode, i) && !isVisited[i]){
                    isVisited[i] = true;
                    path.Add(startnode);
                    targetfound = DFSrecursive(graph, i, targetnode);
                    if (!targetfound){
                        path.Remove(i);
                    }
                    
                }
            }
            return targetfound; // JANGAN LUPA DI HAPUS APABILA SUDAH SELESAI
        }
    }
}*/