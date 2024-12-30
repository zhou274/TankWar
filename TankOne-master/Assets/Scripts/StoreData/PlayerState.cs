using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState 
{
    public int score;
    public int gem;
    public int rewarded;
    public int speedMultiplier = 1, powerMultiplier=1;
    public bool isMusic, isSound,isVibrate;
    public List<int> boughtTanks=new List<int>();

}
