using System;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding
{
    [Serializable]
    public class Node
    {
        public int X, Y;
        public int G, H;
        public int F => G + H;
        public Node Parent;

        public Node(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Node other)
        {
            return other.X == X && other.Y == Y;
        }
    }

    public class AStarPathFinding
    {
        public static int Heuristic(Node start, Node goal)
        {
            return Mathf.Abs(start.X - goal.X) + Mathf.Abs(start.Y - goal.Y);
        }

        public static List<Node> AStar(int[,] grid, Node start, Node goal)
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            var openSet = new List<Node> { start };
            var closedSet = new HashSet<Node>();

            start.G = 0;
            start.H = Heuristic(start, goal);

            while (openSet.Count > 0)
            {
                //Sắp xếp dãy ưu tiên node theo F và G
                openSet.Sort((a, b) =>
                {
                    int compare = a.F.CompareTo(b.F);
                    if (compare == 0) compare = a.H.CompareTo(b.H);
                    return compare;
                });

                Node current = openSet[0];
                openSet.RemoveAt(0);

                //if current node is goal node
                if (current.X == goal.X && current.Y == goal.Y)
                {
                    var path = new List<Node>();
                    while (current != null)
                    {
                        path.Add(current);
                        current = current.Parent;
                    }
                    return path;
                }

                closedSet.Add(current);

                //Xét các hàng xóm ở 4 hướng xung quanh và tính toán G, H cho nó.
                foreach (var dir in new (int dx, int dy)[] { (-1, 0), (0, -1), (0, 1), (1, 0) })
                {
                    int nx = current.X + dir.dx;
                    int ny = current.Y + dir.dy;

                    if (nx < 0 || ny < 0 || nx >= rows || ny >= cols || grid[nx, ny] == 1) continue;

                    Node neighbor = new Node(nx, ny);

                    if (closedSet.Contains(neighbor)) continue; //hàng xóm này đã được duyệt qua.

                    int currentNeighborG = current.G + 1;

                    Node exist = openSet.Find(x => x.X == neighbor.X && x.Y == neighbor.Y);

                    if (exist == null)
                    {
                        neighbor.G = currentNeighborG;
                        neighbor.H = Heuristic(neighbor, goal);
                        neighbor.Parent = current;
                        openSet.Add(neighbor);
                    }
                    else if (currentNeighborG < exist.G)
                    {
                        openSet.Remove(exist);
                        exist.G = currentNeighborG;
                        exist.Parent = current;
                        openSet.Add(exist);
                    }
                }
            }
            return null; //cant find path
        }
    }
}
