using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Line : MonoBehaviour {
    [SerializeField]
    private GameObject _circlePrefab;
    [SerializeField]
    private GameObject _group; 
     
    public int _startCell;
    public int _endCell;
    public int _row;
    public List<SignColor> _sign;
    private List<GameObject> _circles;
    //private float speed = 50f; 

    private float _cellSize = 100f;

    void Awake()
    {
        _circles = new List<GameObject>();
    }

    public void Refresh()
    {
        foreach(GameObject circl in _circles)
        {
            Destroy(circl); 
        }
        _circles = new List<GameObject>();
        GetComponent<RectTransform>().anchoredPosition = new Vector2(_cellSize * _startCell, -_cellSize * _row - 50f);
        GetComponent<RectTransform>().sizeDelta = new Vector2(_cellSize * (_endCell - _startCell + 1), 100f);
        foreach(SignColor s in _sign)
        {
            GameObject circle = Instantiate(_circlePrefab, _group.transform);
            _circles.Add(circle);
            circle.GetComponent<Image>().color = circle.GetComponent<Circle>()._colors[(int)s]; 
        } 
    }
}
