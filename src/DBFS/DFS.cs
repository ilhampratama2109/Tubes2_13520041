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
        private List<int> searchPath;
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
            this.searchPath = new List<int>();
            this.idxsolution = new List<int>();
        }

        // ALGORITMA proses DFS
        public void processDFS()
        {
            // Root dikunjungi
            this.visited[0] = true;
            // Node dimasukkan ke path
            this.searchPath.Add(0);
            this.idxsolution.Add(0);

            // Mencatat adjacent child dari root
            List<int> adjchild = returnAdjacentNodes(listOfNode[0]);

            bool found = false;
            for (int i = 0; i < adjchild.Count; i++)
            {
                string ctarget = childNode[adjchild[i]];
                int ctidx = childIdxInLON(ctarget);
                if (ctidx < this.listOfNode.Count)
                {
                    if (!this.fc.getFindAll() && answerExist)
                    {
                        // do nothing
                    }
                    else
                    {
                        found = recursiveDFS(ctidx);
                    }
                }
            }
            this.form1.stopwatch.Stop();

            this.form1.writeTimeElapsed();

            visualize();
        }

        private bool recursiveDFS(int nodeIdx)
        {
            // Node urutan nodeIdx dikunjungi
            this.visited[nodeIdx] = true;
            // Node dimasukkan ke path
            this.searchPath.Add(nodeIdx);
            this.idxsolution.Add(nodeIdx);

            // Pengecekan apakah node ini merupakan file target
            if (this.fc.getFileToFind() == this.listOfNode[nodeIdx])
            {
                // Node target ditemukan
                this.answerExist = true;

                String solutionPath = this.fc.getStartingDirectory();
                foreach (int idx in this.idxsolution)
                {
                    if (idx != 0 && this.listOfNode[idx] != this.fc.getFileToFind())
                    {
                        String node = this.listOfNode[idx];
                        solutionPath = solutionPath + "\\" + node;
                    }
                    isSolution[idx] = true;
                }
                this.form1.addComboBoxElmt(solutionPath);

                if (this.fc.getFindAll())
                {
                    this.idxsolution.Remove(nodeIdx);
                    return false;
                }
                else
                {
                    // path solusi dimasukan ke issolution
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
                int ct2idx = childIdxInLON(ctarget);
                if (ct2idx < this.listOfNode.Count)
                {
                    if (!this.fc.getFindAll() && answerExist)
                    {
                        // do nothing
                    }
                    else
                    {
                        found = recursiveDFS(ct2idx);
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

        // Method untuk memvisualisasikan langkah per langkah algoritma
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