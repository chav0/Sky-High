using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Bomb : MonoBehaviour {

    public int bomb;
    public List<GameObject> bombs;
    public List<GameObject> cubes;
    public PlayerController player;
    public int numBomb; 
    public Text txt;

    private void Start()
    {
        txt.text = numBomb.ToString(); 
    }

    public void SetBomb () {

        //Высчитываем, куда поставится бомба
        Point p = new Point((int)Mathf.Round(player.transform.position.x / 10.0f), (int)Mathf.Round(player.transform.position.z / 10.0f));

        //Если бомб хватает и клетка не занята другими бомбами 
        if (numBomb > 0 && CellGenerator.levelGrid[p.X][p.Y].GetComponent<Cube>()._color == 0)
        {
            //Уменьшаем количество доступных бомб
            numBomb -= 1; 
            txt.text = numBomb.ToString();


            //Ставим бомбу в нужное положение 
            GameObject active_bomb = Instantiate(bombs[bomb], new Vector3(p.X * 10.0f, 0, p.Y * 10.0f), Quaternion.identity);
            CellGenerator.levelGrid[p.X][p.Y].GetComponent<Cube>()._color = 4;

            //Удаление бомбы спустя 3 секунды после ее появления. 
            Destroy(active_bomb, 3.0f);

            //Уничтожаем рядом стоящие башни
            if (p.X < CellGenerator.levelGrid.Length - 1 && CellGenerator.levelGrid[p.X + 1][p.Y].GetComponent<Cube>()._color != 0 && CellGenerator.levelGrid[p.X + 1][p.Y].GetComponent<Cube>()._color != 4)
            {
                StartCoroutine(initForSeconds(p.X + 1, p.Y, bomb));
            }
            if (p.X > 0 && CellGenerator.levelGrid[p.X - 1][p.Y].GetComponent<Cube>()._color != 0 && CellGenerator.levelGrid[p.X - 1][p.Y].GetComponent<Cube>()._color != 4)
            {
                StartCoroutine(initForSeconds(p.X - 1, p.Y, bomb));
            }
            if (p.Y < CellGenerator.levelGrid[0].Length - 1 && CellGenerator.levelGrid[p.X][p.Y + 1].GetComponent<Cube>()._color != 0 && CellGenerator.levelGrid[p.X][p.Y + 1].GetComponent<Cube>()._color != 4)
            {
                StartCoroutine(initForSeconds(p.X, p.Y + 1, bomb));
            }
            if (p.Y > 0 && CellGenerator.levelGrid[p.X][p.Y - 1].GetComponent<Cube>()._color != 0 && CellGenerator.levelGrid[p.X][p.Y - 1].GetComponent<Cube>()._color != 4)
            {
                StartCoroutine(initForSeconds(p.X, p.Y - 1, bomb));
            }

            //Взрыв бомбы I
            StartCoroutine(BoomBomb(p.X, p.Y));
        }
    }

    IEnumerator initForSeconds(int i, int j, int bomb)
    {
        CellGenerator.levelGrid[i][j].GetComponent<Cube>()._hp -= Settings._bombs_dp[bomb];
        int new_cell = CellGenerator.levelGrid[i][j].GetComponent<Cube>()._hp;
        if (new_cell < 0) new_cell = 0;
        // Удаление существующей клетки 

        yield return new WaitForSeconds(3.0f);
        Destroy(CellGenerator.levelGrid[i][j]);

        // Появление на ее месте обычной белой клетки
        CellGenerator.levelGrid[i][j] = Instantiate(cubes[new_cell], new Vector3(i * 10.0f, 0, j * 10.0f), Quaternion.identity);
        CellGenerator.levelGrid[i][j].GetComponent<Cube>()._color = new_cell;
        CellGenerator.levelGrid[i][j].GetComponent<Cube>()._hp = Settings._cubes_hp[new_cell];
        CellGenerator.levelGrid[i][j].GetComponent<Cube>()._pos = new Point(i, j);

        // Наносим урон игроку 
        Point b = new Point(i, j);
        Point p = new Point((int)Mathf.Round(player.transform.position.x / 10.0f), (int)Mathf.Round(player.transform.position.z / 10.0f));
        if (b == p)
        {
            PlayerController.player_hp -= Settings._bombs_dp[bomb];
        }
    }

    IEnumerator BoomBomb(int i, int j)
    {
        yield return new WaitForSeconds(3.0f);
        CellGenerator.levelGrid[i][j].GetComponent<Cube>()._color = 0;
    }
}
