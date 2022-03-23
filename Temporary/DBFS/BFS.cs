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
    class BFS
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
        private Queue<int> idxqueue;

        /* ***** METHOD ***** */
        public BFS(Form1 f1, FolderCrawler fc, Graph g)
        {
            this.form1 = f1;
            this.fc = fc;
            this.graph = g;
            this.listOfNode = fc.getListOfNode();
            this.parentNode = fc.getParentNode();
            this.childNode = fc.getChildNode();

            this.answerExist = false;
            this.isSolution = new bool[listOfNode.Count];
            this.visited = new bool[listOfNode.Count];
            this.idxsolution = new List<int>();
            this.idxqueue = new Queue<int>();
        }

        // ALGORITMA proses BFS
        public void processBFS()
        {
            // Root dikunjungi
            this.visited[0] = true;
            // Node dimasukkan ke path
            this.idxsolution.Add(0);
            // Node diwarnai merah
            graph.FindNode(listOfNode[0]).Attr.Color = Color.Red;

            // Mencatat adjacent child dari root
            List<int> adjchild = returnAdjacentChildNodes(listOfNode[0]);

            // Tandai semua child telah dikunjungi
            // Masukkan ke dalam queue
            int i = 0;
            while (i < adjchild.Count && !keepGoingCheck())
            {
                string ct = childNode[adjchild[i]];
                int ctidx = childIdxInLON(ct);
                if (ctidx < this.listOfNode.Count){
                    // Masukkan child ke dalam antrian
                    this.idxqueue.Enqueue(ctidx);
                    this.visited[ctidx] = true;

                    // Node dimasukkan ke path
                    this.idxsolution.Add(ctidx);
                    // Node diwarnai merah
                    graph.FindNode(listOfNode[ctidx]).Attr.Color = Color.Red;
                    Console.WriteLine(listOfNode[ctidx]);

                    // Pengecekan apakah child yang baru saja dikunjungi
                    // merupakan target
                    if (this.fc.getFileToFind() == this.listOfNode[ctidx]){
                        this.answerExist = true;

                        // Path target diberi warna hijau
                        // the code goes here...
                    }
                }
                else{
                    i++;
                }
            }

            do
            {
                int childTarget = this.idxqueue.Dequeue();
                List<int> adjchild2 = returnAdjacentChildNodes(listOfNode[childTarget]);
                int j = 0;
                while (j < adjchild2.Count && !keepGoingCheck()){
                    string ct2 = childNode[adjchild2[j]];
                    int ct2idx = childIdxInLON(ct2);
                    bool nodeIsVisited = this.visited[ct2idx];

                    if (ct2idx < this.listOfNode.Count && !nodeIsVisited){
                        // Masukkan child ke dalam antrian
                        this.idxqueue.Enqueue(ct2idx);
                        this.visited[ct2idx] = true;

                        // Node diwarnai merah
                        graph.FindNode(listOfNode[ct2idx]).Attr.Color = Color.Red;
                        Console.WriteLine(listOfNode[ct2idx]);

                        // Pengecekan apakah child yang baru saja dikunjungi
                        // merupakan target
                        if (this.fc.getFileToFind() == this.listOfNode[ct2idx]){
                            // Node dimasukkan ke path
                            this.idxsolution.Add(ct2idx);
                            this.answerExist = true;

                            /* ILUSTRASI UNTUK ALAT BANTU PENJELASAN
                                        root
                                folderA     folderB
                                    - root.txt
                            parentNode[0] = root
                            childNode[0] = folderA
                            parentNode[1] = root
                            childNode[1] = folderB
                            parentNode[2] = folderA
                            childNode[2] = root.txt
                            parentNode[3] = folderB
                            childNode[3] = root.txt */

                            // Memasukkan path idx ke idxsolution
                            if (this.fc.getFindAll()){
                                this.trackAllPath(ct2idx);
                            }
                            else{
                                this.trackOnePath(ct2idx);
                            }
                            // Warnai semua index pada idxsolution
                        }
                    }
                    else{
                        j++;
                    }
                }
            } while(this.idxqueue.Count > 0);

            if (this.answerExist){
                for (int x = 0; x < listOfNode.Count; x++)
                {
                    if (isSolution[x])
                    {
                        graph.FindNode(listOfNode[x]).Attr.Color = Color.Green;
                    }
                }
            }
            else
            {
                MessageBox.Show("File tidak ditemukan!");
            }
        }

        // Mencari path index dari solusi (khusus getFindAll == true)
        private void trackAllPath(int childindex){
            if (!(this.listOfNode[childindex] == "root")){
                List<int> adjparent = returnAdjacentParentNodes(this.listOfNode[childindex]);

                // Semua parent yang terhubung dengan child saat ini
                // akan ditandai sebagai path
                for (int i = 0; i < adjparent.Count; i++)
                {
                    string ptarget = this.parentNode[adjparent[i]];

                    // childIdxInLON sudah menghandle untuk mencari
                    // node parent pada listOfNode, maka digunakan kembali
                    int parentInLONidx = childIdxInLON(ptarget);
                    if (parentInLONidx < this.listOfNode.Count)
                    {
                        this.idxsolution.Add(parentInLONidx);
                        this.trackThePath(parentInLONidx);
                    }
                }
            }
        }

        // Mencari path index dari solusi (khusus getFindAll == false)
        private void trackOnePath(int childindex){
            if (!(childindex == 0)){
                List<int> adjparent = returnAdjacentParentNodes(this.listOfNode[childindex]);

                // Indeks 0 dari adjparent adalah parent yang pertama
                // mengakses child node saat ini.
                // Oleh karena itu, cukup untuk menggunakan adjparent[0]
                string ptarget = this.parentNode[adjparent[0]];

                // childIdxInLON sudah menghandle untuk mencari
                // node parent pada listOfNode, maka digunakan kembali
                int parentInLONidx = childIdxInLON(ptarget);
                if (parentInLONidx < this.listOfNode.Count)
                {
                    this.idxsolution.Add(parentInLONidx);
                    this.trackThePath(parentInLONidx);
                }
            }
        }

        // Method untuk memeriksa apakah masih perlu lanjut atau tidak
        private bool keepGoingCheck(){
            return (!this.fc.getFindAll() && this.answerExist);
        }

        // Method untuk mencatat parent dari sebuah child
        // Digunakan untuk pewarnaan path pada BFS
        private List<int> returnAdjacentParentNodes(string childnode)
        {
            List<int> adjparent = new List<int>();
            for (int i = 0; i < this.childNode.Count; i++)
            {
                if (childNode[i] == childnode)
                {
                    adjparent.Add(i);
                }
            }
            return adjparent; // berisi idx parent
        }

        // Method untuk mencatat child yang adjacent dengan parent yang dikunjungi
        private List<int> returnAdjacentChildNodes(string parentnode)
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