// Система непересекающихся множеств. Алгоритмы сжатия пути


namespace ConsoleApp4
{
    using System;

    public class UnionFind
    {
        private int[] parent;
        private int[] rank;

        public UnionFind(int size)
        {
            parent = new int[size];
            rank = new int[size];

            for (int i = 0; i < size; i++)
            {
                parent[i] = i;
                rank[i] = 1;
            }
        }

        public int Find(int x)
        {
            if (parent[x] != x)
            {
                // Сжатие пути
                parent[x] = Find(parent[x]);
            }
            return parent[x];
        }

        public int PathSplit(int x)
        {
            while (parent[x] != x)
            {
                x = parent[x];
                parent[x] = parent[parent[x]];
            }
            return x;
        }

        public int PathHalve(int x)
        {
            while (parent[x] != x)
            {
                parent[x] = parent[parent[x]];
                x = parent[x];
            }
            return x;
        }

        public void Union(int x, int y)
        {
            int rootX = PathSplit(x); //Find(x)
            int rootY = PathHalve(y); //Find(y)

            if (rootX == rootY)
                return;

            // Объединяем деревья по рангу
            if (rank[rootX] > rank[rootY])
            {
                parent[rootY] = rootX;
            }
            else if (rank[rootX] < rank[rootY])
            {
                parent[rootY] = rootX;
            }
            else
            {
                parent[rootY] = rootX;
                rank[rootX]++;
            }
        }

        public bool Connected(int x, int y)
        {
            return Find(x) == Find(y);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            UnionFind uf = new UnionFind(10);
            uf.Union(1, 2);
            uf.Union(2, 3);
            Console.WriteLine(uf.Connected(1, 3));  // Вывод: True
            Console.WriteLine(uf.Connected(1, 4));  // Вывод: False
            uf.Union(3, 4);
            Console.WriteLine(uf.Connected(1, 4));  // Вывод: True
        }
    }
}
