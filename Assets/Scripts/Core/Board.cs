using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private TileStateSO[] tileStates;

    [SerializeField] private float animationDuration = 0.1f;

    private List<Tile> tiles = new List<Tile>();

    private TileGrid grid;
    private bool isWaiting;

    private void Awake()
    {
        grid = GetComponentInChildren<TileGrid>();        
    }

    private void Start()
    {
        CreateTile();
        CreateTile();
    }

    private void Update()
    {
        if (!isWaiting)
        {

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                // MOVE UP
                MoveTiles(Vector2Int.up, 0, 1, 1, 1);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                // MOVE DOWN
                MoveTiles(Vector2Int.down, 0, 1, grid.GetHeight() - 2, -1);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                // MOVE LEFT
                MoveTiles(Vector2Int.left, 1, 1, 0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                // MOVE RIGHT
                MoveTiles(Vector2Int.right, grid.GetWidth() - 2, -1, 0, 1);
            }

        }
    }

    private void MoveTiles(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {

        bool isChanged = false;

        for(int x = startX; x >= 0 && x < grid.GetWidth(); x += incrementX)
        {
            for(int y = startY; y >= 0 && y < grid.GetHeight(); y += incrementY)
            {
                Cell cell = grid.GetCell(x, y);

                if (cell.isOccupied)
                {
                    isChanged |= MoveTile(cell.tile, direction);
                }
            }
        }

        if(isChanged)
        {
            // WAIT FOR CHANGES

            StartCoroutine(WaitForChanges());
        }
    }

    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        Cell newCell = null;
        Cell adjacentCell = grid.GetAdjacentCell(tile.cell, direction);

        while (adjacentCell != null)
        {
            if (adjacentCell.isOccupied)
            {
                // TODO : MERGE
                if (CanMerge(tile, adjacentCell.tile ))
                {
                    Merge(tile, adjacentCell.tile);
                    return true;
                }
                break;
            }

            newCell = adjacentCell;
            adjacentCell = grid.GetAdjacentCell(adjacentCell, direction);
        }

        if (newCell != null)
        {
            tile.MoveTo(newCell);

            return true;
        }

        return false;
        
    }

    private void Merge(Tile a,  Tile b)
    {
        tiles.Remove(a);
        a.Merge(b.cell);

        int index = Mathf.Clamp(IndexOf(b.tileState) + 1, 0, tileStates.Length - 1);
        int number = b.number * 2;

        b.SetState(tileStates[index], number);

        AnimateTiles(b, animationDuration);
    }

    private void AnimateTiles(Tile tileToAnimate, float animationDuration)
    {
        tileToAnimate.gameObject.transform.DOScale(1.25f, animationDuration).OnComplete(() =>
        {
            tileToAnimate.gameObject.transform.DOScale(1f, animationDuration);
        });
    }

    private bool CanMerge(Tile a, Tile b)
    {
        return a.number == b.number && !b.isLocked;
    }

    public void CreateTile()
    {
        Tile tile = Instantiate(tilePrefab, grid.transform);
        tile.SetState(tileStates[0], Consts.Numbers.NUMBER_2);
        tile.SpawnTile(grid.GetRandomEmptyCell());
        tiles.Add(tile);
    }


    private int IndexOf(TileStateSO tileState)
    {
        for(int i = 0; i < tileStates.Length; i++)
        {
            if (tileState == tileStates[i])
            {
                return i;
            }
        }
        return -1;
    }

    private IEnumerator WaitForChanges()
    {
        isWaiting = true;
        float waitingDuration = 0.1f;
        yield return new WaitForSeconds(waitingDuration);
        isWaiting = false;

        foreach(Tile tile in tiles)
        {
            tile.isLocked = false;
        }

        if (tiles.Count != grid.GetSize())
        {
            CreateTile();
        }
    }

    private bool CheckForGameOver()
    {
        if(tiles.Count != grid.GetSize())
        {
            return false;
        }


        foreach(Tile tile in tiles)
        {
            Cell upCell = grid.GetAdjacentCell(tile.cell, Vector2Int.up);
            Cell downCell = grid.GetAdjacentCell(tile.cell, Vector2Int.down);
            Cell leftCell = grid.GetAdjacentCell(tile.cell, Vector2Int.left);
            Cell rightCell = grid.GetAdjacentCell(tile.cell, Vector2Int.right);

            if (upCell != null && CanMerge(tile, upCell.tile))
            {
                return false;
            }
            if (downCell != null && CanMerge(tile, downCell.tile))
            {
                return false;
            }
            if (leftCell != null && CanMerge(tile, leftCell.tile))
            {
                return false;
            }
            if (rightCell != null && CanMerge(tile, rightCell.tile))
            {
                return false;
            }
        }
        return true;
    }                                                                    
}                                                                        
