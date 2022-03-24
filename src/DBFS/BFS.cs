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
        private List<int> searchPath;
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
            this.searchPath = new List<int>();
            this.idxqueue = new Queue<int>();
        }

        // ALGORITMA proses BFS
        public void processBFS()
        {
            // Root dikunjungi
            this.visited[0] = true;
            this.searchPath.Add(0);

            // Mencatat adjacent child dari root
            List<int> adjchild = returnAdjacentChildNodes(listOfNode[0]);

            // Tandai semua child telah dikunjungi
            // Masukkan ke dalam queue
            int i = 0;
            while (i < adjchild.Count && !stopCheck())
            {
                string ct = childNode[adjchild[i]];
                int ctidx = childIdxInLON(ct);

                this.searchPath.Add(ctidx);

                if (ctidx < this.listOfNode.Count)
                {
                    // Masukkan child ke dalam antrian
                    this.idxqueue.Enqueue(ctidx);
                    this.visited[ctidx] = true;

                    // Pengecekan apakah child yang baru saja dikunjungi
                    // merupakan target
                    if (this.fc.getFileToFind() == this.listOfNode[ctidx])
                    {
                        isSolution[ctidx] = true;
                        this.answerExist = true;
                        if (stopCheck())
                        {
                            this.trackOnePath(ctidx);
                        } 
                        else{
                            this.trackAllPath(ctidx);
                            i++;
                        }
                    } 
                    else{
                        i++;
                    }
                } 
                else{
                    i++;
                }
            }

            while(this.idxqueue.Count > 0 && !stopCheck())
            {
                int childTarget = this.idxqueue.Dequeue();
                List<int> adjchild2 = returnAdjacentChildNodes(listOfNode[childTarget]);
                int j = 0;
                while (j < adjchild2.Count && !stopCheck()){
                    string ct2 = childNode[adjchild2[j]];
                    int ct2idx = childIdxInLON(ct2);
                    
                    bool nodeIsVisited = this.visited[ct2idx];

                    if (ct2idx < this.listOfNode.Count && !nodeIsVisited){
                        this.searchPath.Add(ct2idx);
                        // Masukkan child ke dalam antrian
                        this.idxqueue.Enqueue(ct2idx);
                        this.visited[ct2idx] = true;

                        // Pengecekan apakah child yang baru saja dikunjungi
                        // merupakan target
                        if (this.fc.getFileToFind() == this.listOfNode[ct2idx]){
                            this.answerExist = true;

                            // Memasukkan path idx ke idxsolution
                            if (this.fc.getFindAll()){
                                this.trackAllPath(ct2idx);
                                j++;
                            }
                            else{
                                this.trackOnePath(ct2idx);
                                j++;
                            }
                        }
                        else{
                            j++;
                        }
                    }
                    else{
                        j++;
                    }
                }
            }

            this.form1.stopwatch.Stop();
            this.form1.writeTimeElapsed();
            visualize();
        }

        // Mencari path index dari solusi (khusus getFindAll == true)
        private void trackAllPath(int childindex){
            if (!(childindex == 0)){
                this.idxsolution.Add(childindex);
                this.isSolution[childindex] = true;
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
                        this.trackAllPath(parentInLONidx);
                    }
                }
            }
            else{
                // Masukkan root ke idxsolution
                this.idxsolution.Add(childindex);
                this.isSolution[childindex] = true;

                // Pengembalian path
                string solutionPath = this.fc.getStartingDirectory();
                
                for (int idx = this.idxsolution.Count - 1; idx > 0; idx--)
                {
                    if (idx != this.idxsolution.Count - 1)
                    {
                        String node = this.listOfNode[idxsolution[idx]];
                        solutionPath = solutionPath + "\\" + node;
                    }
                }
                this.form1.addComboBoxElmt(solutionPath);

                // Menghapus elemen idxsolution dari belakang
                // Penghapusan dilakukan sampai indeks ke- 1
                int deleteidx = this.idxsolution.Count - 1;
                bool deletesuccess = true;
                while(deleteidx > 0 && deletesuccess){
                    deletesuccess = this.idxsolution.Remove(this.idxsolution[deleteidx]);
                    deleteidx--;
                }

            }
        }

        // Mencari path index dari solusi (khusus getFindAll == false)
        private void trackOnePath(int childindex){
            if (!(childindex == 0)){
                this.idxsolution.Add(childindex);
                this.isSolution[childindex] = true;
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
                    this.trackOnePath(parentInLONidx);
                }
            }
            else
            {
                this.idxsolution.Add(childindex);
                this.isSolution[childindex] = true;
                string solutionPath = this.fc.getStartingDirectory();
                for (int idx = this.idxsolution.Count - 1; idx > 0; idx--)
                {
                    if (idx != this.idxsolution.Count - 1)
                    {
                        String node = this.listOfNode[idxsolution[idx]];
                        solutionPath = solutionPath + "\\" + node;
                    }
                }
                this.form1.addComboBoxElmt(solutionPath);
            }
        }

        // Method untuk memeriksa apakah masih perlu lanjut atau tidak
        private bool stopCheck(){
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

        // Method untuk memvisualisasikan langkah per langkah algoritma
        private async void visualize()
        {
            // int j = 0;
            for (int i = 0; i < this.searchPath.Count; i++)
            {
                await PutTaskDelay();
                this.form1.SuspendLayout();
                Console.WriteLine(this.listOfNode[searchPath[i]]);
                graph.FindNode(this.listOfNode[searchPath[i]]).Attr.Color = Color.Red;
                this.form1.ResumeLayout();
                this.form1.draw(graph);
            }

            for (int j = 0; j < listOfNode.Count; j++)
            {
                if (isSolution[j])
                {
                    graph.FindNode(this.listOfNode[j]).Attr.Color = Color.Green;
                }
            }

            if (!this.answerExist)
            {
                MessageBox.Show("File tidak ditemukan!");
            }
        }
        async Task PutTaskDelay()
        {
            await Task.Delay(500);
        }
    }
}