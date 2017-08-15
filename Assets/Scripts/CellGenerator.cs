using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGenerator : MonoBehaviour {

    [RangeAttribute(1, 20)] 
    public int width; //ширина поля 
    [RangeAttribute(1, 20)]
    public int high; // высота поля 
    [RangeAttribute(0.0f, 1.0f)]
    public float white_percentage; // количество белых клеток в поле для генерации поля 
    [Space(10)]
    public List<GameObject> cubes; // список префабов кубов 

    public static GameObject[][] levelGrid; // поле игры

    public GameObject player; 

    private int net_size = 2; 

    private int _iwhite, _jwhite; 

    void Start () {
        levelGrid = new GameObject[width][];
        for (int i = 0; i < width; i++)
        {
            levelGrid[i] = new GameObject[high];

            for (int j = 0; j < high; j++)
            {
                int color = 0;
                if (i % net_size == 0 && j % net_size == 0)
                {
                    color = 4;
                }
                else
                {
                    color = randomColor();
                    if (color == 0)
                    {
                        _iwhite = i;
                        _jwhite = j;
                    }
                }
                levelGrid[i][j] = Instantiate(cubes[color], new Vector3(i * 10.0f, 0, j * 10.0f), Quaternion.identity);
                levelGrid[i][j].GetComponent<Cube>()._color = color;
                levelGrid[i][j].GetComponent<Cube>()._hp = Settings._cubes_hp[color];
                levelGrid[i][j].GetComponent<Cube>()._pos = new Point(i, j); 
            }
        }

        player.transform.position = new Vector3(_iwhite * 10.0f, 0, _jwhite * 10.0f);
    }

    int randomColor()
    {
        float rand = Random.value;
        if (rand <= white_percentage) return 0; 
        if (rand <= white_percentage + (1.0f - white_percentage)/3) return 1; 
        if (rand <= white_percentage + 2.0f * (1.0f - white_percentage) / 3) return 2;
        if (rand <= white_percentage + 3.0f * (1.0f - white_percentage) / 3) return 3;
        return 0; 
    }
	
    void BlackCellGenerate(int net_size)
    {
        for (int i = 0; i < width/3; i++)
        {
            for (int j = 0; j < high/3; j++)
            {
                Destroy(levelGrid[i][j]);
                int color = randomColor();
                if (color == 0)
                {
                    _iwhite = i;
                    _jwhite = j;
                }
                levelGrid[i][j] = Instantiate(cubes[color], new Vector3(i * 10.0f, 0, j * 10.0f), Quaternion.identity);
                levelGrid[i][j].GetComponent<Cube>()._color = color;
                levelGrid[i][j].GetComponent<Cube>()._hp = Settings._cubes_hp[color];
                levelGrid[i][j].GetComponent<Cube>()._pos = new Point(i, j);
            }
        }
    }

	void Update () {
		
	}
}
