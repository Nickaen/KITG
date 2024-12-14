using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        int vertices = 6; // количество вершин в графе
        Graph graph = new Graph(vertices);

        // Пример графа (можно изменять по необходимости)
        graph.AddEdge(0, 1, 16);
        graph.AddEdge(0, 2, 13);
        graph.AddEdge(1, 2, 10);
        graph.AddEdge(1, 3, 12);
        graph.AddEdge(2, 1, 4);
        graph.AddEdge(2, 4, 14);
        graph.AddEdge(3, 2, 9);
        graph.AddEdge(3, 5, 20);
        graph.AddEdge(4, 3, 7);
        graph.AddEdge(4, 5, 4);

        int maxFlow = graph.FordFulkerson(0, 5); // 0 - источник, 5 - сток
        Console.WriteLine("Максимальный поток: " + maxFlow);
    }
}

class Graph
{
    private int Vertices; // Количество вершин
    private int[,] Capacity; // Массив ёмкости
    private int[,] Flow; // Массив текущего потока

    public Graph(int vertices)
    {
        Vertices = vertices;
        Capacity = new int[vertices, vertices];
        Flow = new int[vertices, vertices];
    }

    public void AddEdge(int from, int to, int capacity)
    {
        Capacity[from, to] = capacity;
    }

    public int FordFulkerson(int source, int sink)
    {
        int maxFlow = 0;
        int[] parent = new int[Vertices];

        while (BFS(source, sink, parent))
        {
            // Найти максимальную ёмкость пути
            int pathFlow = int.MaxValue;
            for (int v = sink; v != source; v = parent[v])
            {
                int u = parent[v];
                pathFlow = Math.Min(pathFlow, Capacity[u, v] - Flow[u, v]);
            }

            // Обновляем потоки по графу
            for (int v = sink; v != source; v = parent[v])
            {
                int u = parent[v];
                Flow[u, v] += pathFlow;
                Flow[v, u] -= pathFlow; // Обратный поток
            }

            maxFlow += pathFlow;
        }

        return maxFlow;
    }

    private bool BFS(int source, int sink, int[] parent)
    {
        bool[] visited = new bool[Vertices];
        Queue<int> queue = new Queue<int>();
        queue.Enqueue(source);
        visited[source] = true;
        parent[source] = -1;

        while (queue.Count > 0)
        {
            int u = queue.Dequeue();

            for (int v = 0; v < Vertices; v++)
            {
                if (!visited[v] && Capacity[u, v] > Flow[u, v]) // Если есть возможность увеличить поток
                {
                    queue.Enqueue(v);
                    visited[v] = true;
                    parent[v] = u;

                    if (v == sink) return true; // Если дошли до стока
                }
            }
        }

        return false; // Не найден увеличивающий путь
    }
}
