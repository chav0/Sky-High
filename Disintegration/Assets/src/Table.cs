using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Table : MonoBehaviour
{
    [SerializeField]
    private GridSize _size;

    [SerializeField]
    private List<SignColor> StartSign;

    [SerializeField]
    private GameObject _cellPrefab;

    [SerializeField]
    private int initCell;

    public Cell[,] _table;

    private Cell _currentCell;
    private Cell _prevuousCell;

    private GameObject lineToDelete; 

    void Start()
    {
        _table = new Cell[_size.row, _size.colomn];
        for (int i = 0; i < _size.row; i++)
        {
            for (int j = 0; j < _size.colomn; j++)
            {
                _table[i, j] = Instantiate(_cellPrefab, transform).GetComponent<Cell>();
            }
        }
        _table[initCell, 0].StartCell(StartSign); //устанавливаем стартовую клетку 
        _table[initCell, 0].NewLine(0, initCell, transform); 
        Refresh();
    }


    public void Refresh()
    {
        for (int i = 1; i < _size.colomn; i++)
        {
            for (int j = 0; j < _size.row; j++)
            {
                _currentCell = _table[j, i];
                _prevuousCell = _table[j, i - 1];

                if (_currentCell._block == null)
                {
                    
                    if (_prevuousCell._block != null && _prevuousCell._block.isOutput(j) && (_currentCell._lineGO == null || _currentCell._line._startCell != i))
                    {
                        _currentCell.Copy(_prevuousCell);
                        _currentCell.NewLine(i, j, transform); 
                    } else
                    if (_prevuousCell._block != null && !_prevuousCell._block.isOutput(j))
                    {
                        _currentCell._line = null;
                        _currentCell._lineGO = null;
                        _currentCell._sign = new List<SignColor>();
                    } else 
                    if (_prevuousCell._block == null)
                    {
                        _currentCell.Copy(_prevuousCell);
                    }
                } else
                {
                    Debug.Log(i.ToString() + " " + j.ToString());
                    Recount(_currentCell, i, j);
                    _prevuousCell.RefreshLines(i - 1, j, transform);
                }
                if (i == _size.colomn - 1)
                {
                    _currentCell.RefreshLines(_size.colomn - 1, j, transform);
                }
            }
        }

        if (isWin())
        {
            Debug.Log("Win");
        }
        else Debug.Log("Lose"); 
    }

    public void Recount(Cell cell, int colomn, int row)
    {
        cell._sign = new List<SignColor>();
        Block block = cell._block; 
        if (block.type == BlockType.desintegration && block.output[row - block.row])
        {
            int inputCell = 0;
            for (int i = 0; i < block.size; i++)
            {
                if (block.input[i]) inputCell = i;
            }

            if (block.row + inputCell < _size.row)
            {

                Cell inputBlock = _table[block.row + inputCell, block.colomn - 1];
                if (inputBlock._sign.Count > 0)
                {
                    int startPos = 0; // Номер выхода на текущую клетку
                    int countInput = 0; // Количество выходов 
                    for (int j = 0; j <= row - block.row; j++)
                    {
                        if (block.output[j]) startPos++; 
                    }
                    for (int j = 0; j < block.output.Count; j++)
                    {
                        if (block.output[j]) countInput++;
                    }

                    for (int j = startPos - 1; j < inputBlock._sign.Count; j += countInput)
                    {
                        cell._sign.Add(inputBlock._sign[j]);
                    }
                }
            }
        }
        else
        if (block.type == BlockType.integration && block.output[row - block.row])
        {
            List<Cell> inputCells = new List<Cell>(); 
            for (int j = 0; j < block.size; j++)
            {
                if (block.input[j])
                {
                    inputCells.Add(_table[row + j, block.colomn - 1]);
                }
            }
            for (int j = 0; j <= 4; j++)
            {
                foreach (Cell c in inputCells)
                {
                    if (c._sign.Count > j)
                    {
                        cell._sign.Add(c._sign[j]); 
                    }
                }

            }
        }
        if (cell._sign.Count == 0 && cell._lineGO != null)
        {
            cell._line = null;
            cell._lineGO = null;
            cell._sign = null; 
        }
    }

    public bool isWin()
    {
        bool iswin = true; 
        for (int i = 0; i < _size.row; i++)
        {
            _currentCell = _table[i, _size.colomn - 1];
            iswin = iswin && (_currentCell._sign.Count <= 1); 
        }
        return iswin; 
    }

    void Update()
    {

    }

    [ContextMenu("TestOpenAndIdle")]
    public void TestOpenAndIdle()
    {
        Refresh();
    }
}

[System.Serializable]
public class GridSize
{
    public int colomn;
    public int row;
}