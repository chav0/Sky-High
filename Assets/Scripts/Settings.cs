using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings {
    //---------GRID----------
    public static int wight;
    public static int high; 

    //---------PLAYER-----------
    public static int _player_hp = 100;
    public static int _player_sp;

    //---------BOMBS-----------
    public static List<int> _bombs_dp = new List<int>{ 1, 2, 3 };
    public static List<int> _bombs_r = new List<int>{ 1, 1, 2 };

    //---------MOBS-----------
    public static List<int> _mobs_hp;
    public static List<int> _mobs_dp;
    public static List<int> _mobs_sp;

    //---------CUBES-----------
    public static List<int> _cubes_hp = new List<int> { 0, 1, 2, 3, 90000 };
}
