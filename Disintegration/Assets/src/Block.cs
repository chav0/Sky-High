using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
    [SerializeField]
    public int size;
    [SerializeField]
    public List<bool> input;
    [SerializeField]
    public List<bool> output;
    [SerializeField]
    public BlockType type;

    public GameObject _button; 
    public int colomn;
    public int row;

    private Table _grid; 
    
    public void TableUpdate()
    {
        _grid = transform.parent.GetComponent<Table>(); 
        for (int i = 0; i < size; i++)
        {
            _grid._table[row + i, colomn]._block = this; 
        }
        _grid.Refresh(); 
    } 

    public bool isOutput(int rowCell)
    {
        return output[rowCell - row]; 
    }
}



public enum BlockType
{
    integration = 0, 
    desintegration = 1
}
