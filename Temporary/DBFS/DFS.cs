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
*/


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
}