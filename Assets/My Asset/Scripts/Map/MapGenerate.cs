 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathFinding;

public class MapGenerate : MonoBehaviour
{
    [SerializeField] GridData data;
    [SerializeField] Transform cellMapParent;

    [SerializeField]
    private Vector2Int startIndex;

    [SerializeField]
    private Vector2Int goalIndex;

    private int[,] _grid;
    private Dictionary<Vector2Int, Cell> _cells = new();
    private Vector2Int _size;
    private List<Node> _path = new();

    private void Start()
    {
        GenerateMap();
    }

    private void Update()
    {
        //Find path
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetPathColor();

            Node start = new Node(startIndex.x, startIndex.y);
            Node goal = new Node(goalIndex.x, goalIndex.y);

            _path = AStarPathFinding.AStar(_grid, start, goal);
            if (_path == null)
            {
                Debug.Log("Cant Find Path");
                return;
            }
            UpdatePathColor();
        }
    }

    private void UpdatePathColor()
    {
        foreach (Node node in _path)
        {
            Vector2Int index = new Vector2Int(node.X, node.Y);
            if (_cells.ContainsKey(index))
            {
                _cells[index].UpdateState(CellState.Path);
            }
        }
    }

    private void ResetPathColor()
    {
        if (_path == null) return;
        foreach (Node node in _path)
        {
            Vector2Int index = new Vector2Int(node.X, node.Y);
            if (_cells.ContainsKey(index))
            {
                _cells[index].UpdateState(CellState.Empty);
            }
        }
    }

    public void GenerateMap()
    {
        _size = data.LoadSize();
        _grid = data.GenerateGrid((_size.x, _size.y));
        startIndex = data.GetStart(_grid);
        goalIndex = data.GetGoal(_grid, startIndex);
        _cells = data.FactoryGridMap(_grid, cellMapParent);
    }
}
