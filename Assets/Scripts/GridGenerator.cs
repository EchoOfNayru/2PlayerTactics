using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridGenerator : MonoBehaviour
{
    public static GridGenerator instance;


    public int width;
    public int depth;

    public int players;
    public int enemies;

    public GameObject tile;
    public Camera cam;
    public GameObject player;

    public TileScript[] tiles;

    [HideInInspector]
    public Player currentTarget;

    int startPos;

    public Player[] PlayerUnits;
    public Player[] EnemyUnits;

    bool PlayerTurn;

    public int walls;
    public GameObject wall;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        width = Random.Range(3, 10);
        depth = Random.Range(3, 10);

        tiles = new TileScript[width * depth];
        int tileNum = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < depth; j++)
            {
                GameObject thisTile = Instantiate(tile);
                thisTile.transform.position = new Vector3(i, 0, j);
                thisTile.transform.parent = gameObject.transform;
                thisTile.GetComponent<TileScript>().tileNum = tileNum;
                tiles[tileNum] = thisTile.GetComponent<TileScript>();
                tileNum++;
            }
        }

        cam.transform.position = new Vector3(width/2, 10, depth/2);

        walls = Random.Range(4, 11);

        for (int i = 0; i < walls; i++)
        {
            startPos = Random.Range(0, width * depth);
            while (tiles[startPos].occupied == true)
            {
                startPos = Random.Range(0, width * depth);
            }
            GameObject thisWall = Instantiate(wall);
            thisWall.transform.position = new Vector3(tiles[startPos].transform.position.x, 0.3f, tiles[startPos].transform.position.z);
            tiles[startPos].occupied = true;
        }

        PlayerUnits = new Player[players];

        for (int i = 0; i < players; i++)
        {
            startPos = Random.Range(0, (width * depth) / 2);
            while (tiles[startPos].occupied == true)
            {
                startPos = Random.Range(0, (width * depth) / 2);
            }
            GameObject thisPlayer = Instantiate(player);
            thisPlayer.GetComponent<Player>().movement = Random.Range(1, 3); // -----
            if (thisPlayer.GetComponent<Player>().movement == 1)
                thisPlayer.GetComponent<Player>().range = 2;
            thisPlayer.transform.position = new Vector3(tiles[startPos].transform.position.x, 0.3f, tiles[startPos].transform.position.z);
            thisPlayer.GetComponent<Player>().currentTile = startPos;
            thisPlayer.GetComponent<Player>().isPlayer = true;
            tiles[thisPlayer.GetComponent<Player>().currentTile].occupied = true;
            tiles[thisPlayer.GetComponent<Player>().currentTile].occupant = thisPlayer.GetComponent<Player>();
            for (int j = 0; j < PlayerUnits.Length; j++)
            {
                if (PlayerUnits[j] == null)
                {
                    PlayerUnits[j] = thisPlayer.GetComponent<Player>();
                    break;
                }
            }
        }

        EnemyUnits = new Player[enemies];

        for (int i = 0; i < enemies; i++)
        {
            startPos = Random.Range((width * depth) / 2, (width * depth));
            while (tiles[startPos].occupied == true)
            {
                startPos = Random.Range((width * depth) / 2, (width * depth));
            }
            GameObject thisPlayer = Instantiate(player);
            thisPlayer.GetComponent<Player>().movement = Random.Range(1, 3); // -----
            if (thisPlayer.GetComponent<Player>().movement == 1)
                thisPlayer.GetComponent<Player>().range = 2;
            thisPlayer.transform.position = new Vector3(tiles[startPos].transform.position.x, 0.3f, tiles[startPos].transform.position.z);
            thisPlayer.GetComponent<Player>().currentTile = startPos;
            thisPlayer.GetComponent<Player>().isPlayer = false;
            tiles[thisPlayer.GetComponent<Player>().currentTile].occupied = true;
            tiles[thisPlayer.GetComponent<Player>().currentTile].occupant = thisPlayer.GetComponent<Player>();
            for (int j = 0; j < EnemyUnits.Length; j++)
            {
                if (EnemyUnits[j] == null)
                {
                    EnemyUnits[j] = thisPlayer.GetComponent<Player>();
                    break;
                }
            }
        }

        int coinFlip = Random.Range(1, 3);
        if (coinFlip == 1)
        {
            PlayerTurn = false;
            EndTurn();
        }
        if (coinFlip == 2)
        {
            PlayerTurn = true;
            EndTurn();
        }
    }

    private void Update()
    {
        Ray hoverRay = GridGenerator.instance.cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hoverHit;
        if (Physics.Raycast(hoverRay, out hoverHit))
        {
            if (hoverHit.collider.GetComponent<Player>() != null && hoverHit.collider.GetComponent<Player>() != currentTarget)
            {
                hoverHit.collider.GetComponent<Player>().ShowAttackRange();
            }
            else
            {
                for (int i = 0; i < PlayerUnits.Length; i++)
                {
                    if (PlayerUnits[i] != null)
                    {
                        PlayerUnits[i].HideMovement();
                        break;
                    }
                }
                if (currentTarget != null)
                {
                    if (currentTarget.canMove)
                    {
                        currentTarget.ShowMovement();
                    }
                    else if (currentTarget.finished == false)
                    {
                        currentTarget.ShowAttackRange();
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < PlayerUnits.Length; i++)
            {
                if (PlayerUnits[i] != null)
                {
                    PlayerUnits[i].HideMovement();
                    break;
                }
            }
            if (currentTarget != null)
            {
                if (currentTarget.canMove)
                {
                    currentTarget.ShowMovement();
                }
                else if (currentTarget.finished == false)
                {
                    currentTarget.ShowAttackRange();
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = GridGenerator.instance.cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.GetComponent<TileScript>() != null && hit.collider.GetComponent<TileScript>().movementColor == 1)
                {
                    tiles[currentTarget.currentTile].occupied = false;
                    tiles[currentTarget.currentTile].occupant = null;
                    if (hit.collider.GetComponent<TileScript>().occupied == false)
                    {
                        currentTarget.currentTile = hit.collider.GetComponent<TileScript>().tileNum;
                        tiles[currentTarget.currentTile].occupied = true;
                        tiles[currentTarget.currentTile].occupant = currentTarget;
                        currentTarget.transform.position = new Vector3(tiles[currentTarget.currentTile].transform.position.x, 0.3f, tiles[currentTarget.currentTile].transform.position.z);
                        currentTarget.canMove = false;
                        currentTarget.HideMovement();
                        currentTarget.ShowAttackRange();
                    }
                }
                if (hit.collider.GetComponent<Player>() != null && hit.collider.GetComponent<Player>() == currentTarget)
                {
                    
                }

                    if (hit.collider.GetComponent<TileScript>() != null && hit.collider.GetComponent<TileScript>().movementColor == 2)
                {
                    if (hit.collider.GetComponent<TileScript>().occupant != null)
                    {
                        currentTarget.Attack(hit.collider.GetComponent<TileScript>().occupant);
                        currentTarget.finished = true;
                    }
                    else
                    {
                        currentTarget.finished = true;
                    }
                }
                if (hit.collider.GetComponent<Player>() != null && tiles[hit.collider.GetComponent<Player>().currentTile].movementColor == 2)
                {
                    if (hit.collider.GetComponent<Player>().isPlayer != currentTarget.isPlayer && tiles[hit.collider.GetComponent<Player>().currentTile].movementColor == 2)
                    {
                        currentTarget.Attack(hit.collider.GetComponent<Player>());
                        currentTarget.finished = true;
                    }
                }

                if (currentTarget != null)
                {
                    if (currentTarget.finished)
                    {
                        ClearCurrentTarget();
                    }
                }
                
                if (hit.collider.gameObject.GetComponent<Player>() != null)
                {
                    if (hit.collider.gameObject.GetComponent<Player>().finished == false)
                    {
                        if (currentTarget != null)
                        {
                            if (currentTarget.canMove != true)
                            {
                                currentTarget.finished = true;
                            }
                        }
                        ClearCurrentTarget();
                        currentTarget = hit.collider.gameObject.GetComponent<Player>();
                        currentTarget.Select();
                    }
                }
            }
        }

        if (PlayerTurn)
        {
            for (int i = 0; i < PlayerUnits.Length; i++)
            {
                if (PlayerUnits[i] != null)
                {
                    if (PlayerUnits[i].finished == false)
                    {
                        break;
                    }
                }
                if (i == PlayerUnits.Length - 1)
                {
                    EndTurn();
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < EnemyUnits.Length; i++)
            {
                if (EnemyUnits[i] != null)
                {
                    if (EnemyUnits[i].finished == false)
                    {
                        break;
                    }
                }
                if (i == EnemyUnits.Length - 1)
                {
                    EndTurn();
                    break;
                }
            }
        }

        EndGame();
    }

    private void ClearCurrentTarget()
    {
        if (currentTarget != null)
        {
            currentTarget.Deselect();

            currentTarget = null;
        }
    }
    
    private void StartTurn()
    {
        if (PlayerTurn)
        {
            for (int i = 0; i < PlayerUnits.Length; i++)
            {
                if (PlayerUnits[i] != null)
                {
                    PlayerUnits[i].canMove = true;
                    PlayerUnits[i].finished = false;
                }
            }
        }
        else
        {
            for (int i = 0; i < EnemyUnits.Length; i++)
            {
                EnemyUnits[i].canMove = true;
                EnemyUnits[i].finished = false;
            }
        }
    }

    private void EndTurn()
    {
        Debug.Log(PlayerTurn);
        if (PlayerTurn)
        {
            for (int i = 0; i < PlayerUnits.Length; i++)
            {
                if (PlayerUnits[i] != null)
                {
                    PlayerUnits[i].finished = true;
                }
            }

            PlayerTurn = false;

            for (int i = 0; i < EnemyUnits.Length; i++)
            {
                if (EnemyUnits[i] != null)
                {
                    EnemyUnits[i].canMove = true;
                    EnemyUnits[i].finished = false;
                    EnemyUnits[i].rend.material = EnemyUnits[i].enemyInactive;
                }
            }

            StartTurn();
        }
        else
        {
            for (int i = 0; i < EnemyUnits.Length; i++)
            {
                if (EnemyUnits[i] != null)
                {
                    EnemyUnits[i].finished = true;
                }
            }

            PlayerTurn = true;

            for (int i = 0; i < PlayerUnits.Length; i++)
            {
                if (PlayerUnits[i] != null)
                {
                    PlayerUnits[i].canMove = true;
                    PlayerUnits[i].finished = false;
                    PlayerUnits[i].rend.material = PlayerUnits[i].playerInactive;
                }
            }

            StartTurn();
        }
    }

    void EndGame()
    {
        for (int i = 0; i < PlayerUnits.Length; i++)
        {
            if (PlayerUnits[i] != null)
            {
                break;
            }
            if (i == PlayerUnits.Length - 1)
            {
                SceneManager.LoadScene("Red Wins");
            }
        }

        for (int i = 0; i < EnemyUnits.Length; i++)
        {
            if (EnemyUnits[i] != null)
            {
                break;
            }
            if (i == EnemyUnits.Length - 1)
            {
                SceneManager.LoadScene("Blue Wins");
            }
        }
    }
}
