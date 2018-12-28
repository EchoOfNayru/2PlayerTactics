using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public int type = 0;

    public int tileNum;
    public bool occupied;
    public Player occupant;
    
    public Material standard, moveable, attackable;
    [SerializeField]
    public int movementColor = 0; // 0 = Standard, 1= Move, 2 = Attack

    private void Update()
    {
        if (movementColor == 1)
        {
            GetComponent<Renderer>().material = moveable;
        }
        else if (movementColor == 2)
        {
            GetComponent<Renderer>().material = attackable;
        }
        else
        {
            GetComponent<Renderer>().material = standard;
        }
    }
}
