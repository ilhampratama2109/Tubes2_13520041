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
        private List<List<int>> listOfSolution;
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
            this.listOfSolution = new List<List<int>>;
            this.idxqueue = new Queue<int>();
        }

        // ALGORITMA proses BFS
        public void processBFS()
        {
            // Root dikunjungi
            this.visited[0] = true;
            // Node diwarnai merah
            this.searchPath.Add(0);

            // Mencatat adjacent child dari root
            List<int> adjchild = returnAdjacentChildNodes(listOfNode[0]);

            // Tandai semua child telah dikunjungi
            // Masukkan ke dalam queue
            int i = 0;
            while (i < adjchild.Count && !keepGoingCheck())
            {
                string ct = childNode[adjchild[i]];
                int ctidx = childIdxInLON(ct);

                this.searchPath.Add(ctidx);

                if (ctidx < this.listOfNode.Count){
                    // Masukkan child ke dalam antrian
                    this.idxqueue.Enqueue(ctidx);
                    this.visited[ctidx] = true;

                    // Pengecekan apakah child yang baru saja dikunjungi
                    // merupakan target
                    if (this.fc.getFileToFind() == this.listOfNode[ctidx]){
                        // Node dimasukkan ke path
                        this.searchPath.Add(ctidx);
                        this.answerExist = true;
                    } 
                    else{
                        i++;
                    }
                }
                else{
                    i++;
                }
            }

            while(this.idxqueue.Count > 0 && !this.answerExist)
            {
                int childTarget = this.idxqueue.Dequeue();
                List<int> adjchild2 = returnAdjacentChildNodes(listOfNode[childTarget]);
                int j = 0;
                while (j < adjchild2.Count && !keepGoingCheck()){
                    string ct2 = childNode[adjchild2[j]];
                    int ct2idx = childIdxInLON(ct2);
                    
                    this.searchPath.Add(ct2idx);
                    bool nodeIsVisited = this.visited[ct2idx];

                    if (ct2idx < this.listOfNode.Count && !nodeIsVisited){
                        // Masukkan child ke dalam antrian
                        this.idxqueue.Enqueue(ct2idx);
                        this.visited[ct2idx] = true;

                        // Pengecekan apakah child yang baru saja dikunjungi
                        // merupakan target
                        if (this.fc.getFileToFind() == this.listOfNode[ct2idx]){
                            // Node dimasukkan ke path
                            this.searchPath.Add(ct2idx);
                            this.answerExist = true;

                            // Memasukkan path idx ke idxsolution
                            this.isSolution[ct2idx] = true;

                            if (this.fc.getFindAll()){
                                this.trackAllPath(ct2idx);
                            }
                            else{
                                this.trackOnePath(ct2idx);
                            }
                            // Warnai semua index pada idxsolution
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
            if (!(this.listOfNode[childindex] == "root")){
                this.idxsolution.Add(childindex);
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
                        this.isSolution[parentInLONidx] = true;
                        this.trackAllPath(parentInLONidx);
                    }
                }
            }
            else if (this.listOfNode[childindex] == "root"){
                // Masukkan root ke idxsolution
                this.idxsolution.Add(childindex);

                // Masukkan idxsolution pertama ke listOfSolution
                this.listOfSolution.Add(this.idxsolution);

                // Menghapus elemen idxsolution dari belakang
                // Penghapusan dilakukan sampai indeks ke- 1
                int deleteidx = this.idxsolution.Count - 1;
                bool deletesuccess = true;
                while(deleteidx > 0 && deletesuccess){
                    deletesuccess = this.idxsolution.Remove(this.idxsolution[deleteidx]);
                    deleteidx--;
                }

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
            }
        }

        // Mencari path index dari solusi (khusus getFindAll == false)
        private void trackOnePath(int childindex){
            if (!(childindex == 0)){
                this.idxsolution.Add(childindex);
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
                    this.isSolution[parentInLONidx] = true;
                    this.trackOnePath(parentInLONidx);
                }
            }
            else
            {
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

        private async void visualize()
        {
            // int j = 0;
            for (int i = 0; i < this.searchPath.Count; i++)
            {
                await PutTaskDelay();
                this.form1.SuspendLayout();
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