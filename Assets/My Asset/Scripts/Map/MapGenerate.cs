 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathFinding;

public class MapGenerate : MonoBehaviour
{
    [SerializeField] GridData data;
    [SerializeField] Transform cellMapParent;

    private Vector2Int _startIndex;
    private Vector2Int _goalIndex;

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
            FindPath();
        }
    }

    public void FindPath()
    {
        ResetPathColor();

        Node start = new Node(_startIndex.x, _startIndex.y);
        Node goal = new Node(_goalIndex.x, _goalIndex.y);

        _path = AStarPathFinding.AStar(_grid, start, goal);
        if (_path == null)
        {
            Debug.Log("Cant Find Path");
            return;
        }
        UpdatePathColor();
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
        _startIndex = data.GetStart(_grid);
        _goalIndex = data.GetGoal(_grid, _startIndex);
        _cells = data.FactoryGridMap(_grid, cellMapParent);
    }

    public void UpdateStartIndex(Vector2Int index)
    {
        bool checkObs = index.x >= 0 && index.x < _grid.GetLength(0) && index.y >= 0 && index.x < _grid.GetLength(1) && _grid[index.x, index.y] == 1;
        if (checkObs) return;
        UpdateState(_startIndex, CellState.Empty, 0);

        _startIndex.x = index.x >= 0 && index.x < _size.x && !checkObs ? index.x : _startIndex.x;
        _startIndex.y = index.y >= 0 && index.y < _size.y && !checkObs ? index.y : _startIndex.y;
    }

    public void UpdateGoalIndex(Vector2Int index)
    {
        bool checkObs = index.x >= 0 && index.x < _grid.GetLength(0) && index.y >= 0 && index.x < _grid.GetLength(1) && _grid[index.x, index.y] == 1;
        if (checkObs) return;
        UpdateState(_goalIndex, CellState.Empty, 0);

        _goalIndex.x = index.x >= 0 && index.x < _size.x && !checkObs ? index.x : _goalIndex.x;
        _goalIndex.y = index.y >= 0 && index.y < _size.y && !checkObs ? index.y : _goalIndex.y;
    }

    public void UpdateState(Vector2Int index,  CellState state, int indexState)
    {
        if (_cells.ContainsKey(index))
        {
            _cells[index].UpdateState(state);
        }
        _grid[index.x, index.y] = indexState;
    }

    public void UpdateStartGoal()
    {
        ResetPathColor();
        UpdateState(_startIndex, CellState.Start, 2);
        UpdateState(_goalIndex, CellState.Goal, 3);
    }

    public void DesTroyAllCells()
    {
        foreach (Transform cell in cellMapParent)
        {
            Destroy(cell.gameObject);
        }
    }
}
