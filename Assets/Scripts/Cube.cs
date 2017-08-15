using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

    public Transform white_cube;
    public int _color;
    public int _hp;
    public Point _pos; 

    public Cube(int clr, GameObject obj)
    {
        _color = clr;
        _hp = Settings._cubes_hp[_color]; 
    }

    void OnMouseDown()
    {
        Vector3 pos = transform.position;
        if (_color == 0)
        {
            Target._target = _pos;
            Debug.Log(Target._target.X.ToString() + ", " + Target._target.Y.ToString());
            Target._new = true;
            
        }
    }
}
