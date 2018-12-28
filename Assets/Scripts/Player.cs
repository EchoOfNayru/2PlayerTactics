using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 10;
    public int attack = 14;
    public int range = 1;
    public int defense = 7;
    public int movement = 2;

    public int currentTile;

    int[] toprow;
    int[] botrow;

    public bool isPlayer;
    public Material playerActive, enemyActive, playerInactive, enemyInactive, playerFinished, enemyFinished;

    public bool canMove;
    public bool finished;

    [SerializeField]
    public Renderer rend;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    private void Start()
    {
        toprow = new int[GridGenerator.instance.width];
        botrow = new int[GridGenerator.instance.width];

        for (int i = 0; i < GridGenerator.instance.width; i++)
        {
            toprow[i] = (i * GridGenerator.instance.depth) + (GridGenerator.instance.depth - 1);
            botrow[i] = (i * GridGenerator.instance.depth);
        }

        
        if (isPlayer)
        {
            rend.material = playerInactive;
        }
        if (!isPlayer)
        {
            rend.material = enemyInactive;
        }
    }

    private void Update()
    {
        if (finished)
        {
            if (isPlayer)
            {
                rend.material = playerFinished;
            }
            else
            {
                rend.material = enemyFinished;
            }
        }
    }

    public void Select()
    {
        ShowMovement();
        if (isPlayer)
        {
            rend.material = playerActive;
        }
        if (!isPlayer)
        {
            rend.material = enemyActive;
        }
    }

    public void Deselect()
    {
        HideMovement();
        HideAttackRange();
        if (isPlayer)
        {
            rend.material = playerInactive;
        }
        else
        {
            rend.material = enemyInactive;
        }
    }

    public void ShowMovement()
    {
        GridGenerator.instance.tiles[currentTile].movementColor = 1;

        int currentRow = currentTile / 7;

        for (int i = 0; i <= movement; i++)
        {
            if (currentTile - 1 * i >= 0)
            {
                if(currentTile - (1 * i) + 1 > botrow[currentRow])
                    GridGenerator.instance.tiles[currentTile - (1 * i)].movementColor = 1;
            }
            if (currentTile + 1 * i <= GridGenerator.instance.tiles.Length - 1)
            {
                if (currentTile + (1 * i) - 1 < toprow[currentRow])
                    GridGenerator.instance.tiles[currentTile + (1 * i)].movementColor = 1;
            }
            if (currentTile - GridGenerator.instance.depth * i >= 0)
            {
                GridGenerator.instance.tiles[currentTile - (GridGenerator.instance.depth * i)].movementColor = 1;
            }
            if (currentTile + GridGenerator.instance.depth * i <= GridGenerator.instance.tiles.Length - 1) 
            {
                GridGenerator.instance.tiles[currentTile + (GridGenerator.instance.depth * i)].movementColor = 1;
            }

            if (currentTile + (GridGenerator.instance.depth + 1) * (i - 1) <= (GridGenerator.instance.width * GridGenerator.instance.width) && i > 1 && currentRow + 1 <= toprow.Length - 1)
            {
                if (currentTile + (GridGenerator.instance.depth + 1) <= toprow[currentRow + 1])
                    GridGenerator.instance.tiles[currentTile + ((GridGenerator.instance.depth + 1) * (i - 1))].movementColor = 1;
            }

            if (currentTile + (GridGenerator.instance.depth - 1) * (i - 1) <= (GridGenerator.instance.width * GridGenerator.instance.width) && i > 1 && currentRow + 1 <= toprow.Length - 1)
            {
                if (currentTile + (GridGenerator.instance.depth - 1) >= botrow[currentRow + 1])
                    GridGenerator.instance.tiles[currentTile + ((GridGenerator.instance.depth - 1) * (i - 1))].movementColor = 1;
            }

            if (currentTile - (GridGenerator.instance.depth + 1) * (i - 1) >= 0 && i > 1 && currentRow - 1 >= 0)
            {
                if (currentTile - (GridGenerator.instance.depth + 1) >= botrow[currentRow - 1])
                    GridGenerator.instance.tiles[currentTile - ((GridGenerator.instance.depth + 1) * (i - 1))].movementColor = 1;
            }
            if (currentTile - (GridGenerator.instance.depth - 1) * (i - 1) >= 0 && i > 1 && currentRow - 1 >= 0)
            {
                if (currentTile - (GridGenerator.instance.depth - 1) <= toprow[currentRow - 1])
                    GridGenerator.instance.tiles[currentTile - ((GridGenerator.instance.depth - 1) * (i - 1))].movementColor = 1;
            }
        }
    }

    public void ShowAttackRange()
    {
        int currentRow = currentTile / 7;

        int i = range;

        if (currentTile - 1 * i >= 0)
        {
            if (currentTile - (1 * i) + 1 > botrow[currentRow])
                GridGenerator.instance.tiles[currentTile - (1 * i)].movementColor = 2;
        }
        if (currentTile + 1 * i <= GridGenerator.instance.tiles.Length - 1)
        {
            if (currentTile + (1 * i) - 1 < toprow[currentRow])
                GridGenerator.instance.tiles[currentTile + (1 * i)].movementColor = 2;
        }
        if (currentTile - GridGenerator.instance.depth * i >= 0)
        {
            GridGenerator.instance.tiles[currentTile - (GridGenerator.instance.depth * i)].movementColor = 2;
        }
        if (currentTile + GridGenerator.instance.depth * i <= GridGenerator.instance.tiles.Length - 1)
        {
            GridGenerator.instance.tiles[currentTile + (GridGenerator.instance.depth * i)].movementColor = 2;
        }

        if (currentTile + (GridGenerator.instance.depth + 1) * (i - 1) <= (GridGenerator.instance.width * GridGenerator.instance.width) && i > 1 && currentRow + 1 <= toprow.Length - 2)
        {
            if (currentTile + (GridGenerator.instance.depth + 1) <= toprow[currentRow + 1])
                GridGenerator.instance.tiles[currentTile + ((GridGenerator.instance.depth + 1) * (i - 1))].movementColor = 2;
        }

        if (currentTile + (GridGenerator.instance.depth - 1) * (i - 1) <= (GridGenerator.instance.width * GridGenerator.instance.width) && i > 1 && currentRow + 1 <= toprow.Length - 2)
        {
            if (currentTile + (GridGenerator.instance.depth - 1) >= botrow[currentRow + 1])
                GridGenerator.instance.tiles[currentTile + ((GridGenerator.instance.depth - 1) * (i - 1))].movementColor = 2;
        }
     
        if (currentTile - (GridGenerator.instance.depth + 1) * (i - 1) >= 0 && i > 1 && currentRow - 1 >= 0)
        {
            if (currentTile - (GridGenerator.instance.depth + 1) >= botrow[currentRow - 1])
                GridGenerator.instance.tiles[currentTile - ((GridGenerator.instance.depth + 1) * (i - 1))].movementColor = 2;
        }
        if (currentTile - (GridGenerator.instance.depth - 1) * (i - 1) >= 0 && i > 1 && currentRow - 1 >= 0)
        {
            if (currentTile - (GridGenerator.instance.depth - 1) <= toprow[currentRow - 1])
                GridGenerator.instance.tiles[currentTile - ((GridGenerator.instance.depth - 1) * (i - 1))].movementColor = 2;
        }
    }

    public void HideMovement()
    {
        for (int i = 0; i < GridGenerator.instance.tiles.Length; i++)
        {
            GridGenerator.instance.tiles[i].movementColor = 0;
        }
    }

    public void HideAttackRange()
    {
        for (int i = 0; i < GridGenerator.instance.tiles.Length; i++)
        {
            GridGenerator.instance.tiles[i].movementColor = 0;
        }
    }

    public void Attack(Player target)
    {
        bool enemyKilled = false;

        target.health -= attack - target.defense;
        Debug.Log("Attacker did " + (attack - target.defense) + " damage.");
        if (target.health <= 0)
        {
            if (target.isPlayer)
            {
                for (int i = 0; i < GridGenerator.instance.PlayerUnits.Length; i++)
                {
                    if (GridGenerator.instance.PlayerUnits[i] == this)
                    {
                        GridGenerator.instance.PlayerUnits[i] = null;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < GridGenerator.instance.EnemyUnits.Length; i++)
                {
                    if (GridGenerator.instance.EnemyUnits[i] == this)
                    {
                        GridGenerator.instance.EnemyUnits[i] = null;
                        break;
                    }
                }
            }
            GridGenerator.instance.tiles[target.currentTile].occupied = false;
            GridGenerator.instance.tiles[target.currentTile].occupant = null;
            Destroy(target.gameObject);
            enemyKilled = true;
            Debug.Log("Defender died.");
        }

        if (enemyKilled == false && range == target.range)
        {
            health -= target.attack - defense;
            Debug.Log("Defender did " + (target.attack - defense) + " damage.");
            if (health <= 0)
            {
                if (isPlayer)
                {
                    for (int i = 0; i < GridGenerator.instance.PlayerUnits.Length; i++)
                    {
                        if (GridGenerator.instance.PlayerUnits[i] == this)
                        {
                            GridGenerator.instance.PlayerUnits[i] = null;
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < GridGenerator.instance.EnemyUnits.Length; i++)
                    {
                        if (GridGenerator.instance.EnemyUnits[i] == this)
                        {
                            GridGenerator.instance.EnemyUnits[i] = null;
                            break;
                        }
                    }
                }
                GridGenerator.instance.tiles[currentTile].occupied = false;
                GridGenerator.instance.tiles[currentTile].occupant = null;
                Destroy(gameObject);
                Debug.Log("Attacker died.");
            }
        }
        else if (range != target.range)
        {
            Debug.Log("Defender cannot respond");
        }
    }
}