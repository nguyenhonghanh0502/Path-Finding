 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathFinding;

public class MapGenerate : MonoBehaviour
{
    [SerializeField] GridData data;
    [SerializeField] Transform cellMapParent;

    private int[,] _grid;
    private Vector2Int _start;
    public Vector2Int GetStart => _start;
    private Vector2Int _goal;
    public Vector2Int GetGoal => _goal;
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

            Node start = new Node(_start.x, _start.y);
            Node goal = new Node(_goal.x, _goal.y);

            _path = AStarPathFinding.AStar(_grid, start, goal);
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
        _start = data.GetStart(_grid);
        _goal = data.GetGoal(_grid, _start);
        _cells = data.FactoryGridMap(_grid, cellMapParent);
    }
}
