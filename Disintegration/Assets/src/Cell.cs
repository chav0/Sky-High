using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField]
    private GameObject _linePrefab; 

    public GameObject _lineGO;
    public Line _line; 
    public List<SignColor> _sign; 

    //На клетке может быть блок
    public Block _block;

    public void StartCell(List<SignColor> sign)
    {
        _sign = sign;  
    }

    public void Copy(Cell cell)
    {
        _lineGO = cell._lineGO;
        _line = cell._line; 
        _sign = cell._sign; 
    }

    public void NewLine(int colomn, int row, Transform grid)
    {
        if (_sign.Count != 0)
        {
            _lineGO = Instantiate(_linePrefab, grid);
            _line = _lineGO.GetComponent<Line>();
            _line._startCell = colomn;
            _line._row = row;
            _line._sign = _sign;
        }
    }

    public void RefreshLines(int colomn, int row, Transform grid)
    {
        if (_sign.Count != 0)
        {
            Debug.Log(row + " " + colomn);
            _line = _lineGO.GetComponent<Line>();
            _line._endCell = colomn;
            _line.Refresh();

        }
    }

    
}

public enum SignColor
{
    color1 = 0, 
    color2 = 1, 
    color3 = 2, 
    color4 = 3, 
    color5 = 4
}
