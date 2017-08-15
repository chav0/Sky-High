using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public CellGenerator grid;
    public PlayerController player; 

    private int width;
    private int high;
    private float H; 

	void Start () {
        width = grid.width;
        high = grid.high;
        H = (width + high) / 2;
    }
	
	// Update is called once per frame
	void Update () {
        

	}
}
