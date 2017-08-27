using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public CellGenerator grid;
    public PlayerController player;
    public Camera cam;

    private int width;
    private int high;
    private float H;
    private float W; 

    private float max = 260.17f;
    private float min = 27.9f;

    private float x_pos_min = -50.0f;
    private float x_pos_max = 50.0f;

    private float cam_size;
    private float x_pos;
    private float z_pos;

    public Vector3 normal_cam_pos_local;
    public Vector3 normal_cam_pos;

    private Vector3 offset;
    private float normal_size = 73.76f;

    void Start () {
        offset = new Vector3(-55.61499f, 60.0f, -56.29499f); 

        width = grid.width;
        high = grid.high;
        H = (width + high) / 2;
        W = width - high; 

        cam_size = (max - min) / 18.0f * H + (max - (max - min) / 18.0f * 20.0f);
        x_pos = (x_pos_max - x_pos_min) / 18.0f * H + (x_pos_max  - (x_pos_max - x_pos_min) / 18.0f * 20.0f) ;
        z_pos = (-127.04f * W) / 36.0f;

        normal_cam_pos_local = new Vector3(x_pos, 0.0f, z_pos);
        cam.transform.localPosition = normal_cam_pos_local;
        cam.orthographicSize = normal_size; 
        normal_cam_pos = cam.transform.position;

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Space))
        {
            cam.orthographicSize = cam_size;
            cam.transform.localPosition = normal_cam_pos_local; 
        } 
        else
        {
            cam.transform.position = player.transform.position + offset;
            cam.orthographicSize = normal_size;
        }


    }
}
