using System;
using System.Collections.Generic;
using System.Text;

namespace DBFS
{
    class Graph
    {
        private bool[] nodes;
        private int size = 10;
        private int n_nodes;
        private bool[] adjmatrix;

        private bool Get(int i, int j)
        {
            return adjmatrix[i * n_nodes + j];
        }

        private void Set(int i, int j, bool val)
        {
            adjmatrix[i * n_nodes + j] = val;
        }

        public Graph(int set_size)
        {
            size = set_size;
            nodes = new bool[size];
            adjmatrix = new bool[size * size];
            n_nodes = set_size;
        }

        public void AddNode(int node)
        {
            nodes[node] = true;
        }

        public void RemoveNode(int node)
        {
            nodes[node] = false;
        }

        public bool FindNode(int node)
        {
            return nodes[node];
        }

        public int CountNode()
        {
            return n_nodes;
        }

        public void AddEdge(int node1, int node2)
        {
            this.Set(node1, node2, true);
            this.Set(node2, node1, true);
        }

        public void RemoveEdge(int node1, int node2)
        {
            this.Set(node1, node2, false);
            this.Set(node2, node1, false);
        }

        public bool FindEdge(int node1, int node2)
        {
            return this.Get(node1, node2) || this.Get(node2, node1);
        }
    }
}
