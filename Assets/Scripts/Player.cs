using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private new string name = "Asshole";
    [SerializeField] private int moveSpeed = 3;

    private GameObject cellsParent;

    private List<GameObject> cellsMoveList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        cellsParent = GameObject.Find("cellsParent");

        PlayerUnit player = new PlayerUnit(name, this.gameObject);

        CellInMoveList();
    }

    //Change color of cell, which player can move on it and add in move list. Breadth-first search
    void CellInMoveList()
    {
        //clear list of cells
        CleanCells();

        //get coordinates cell which player stay it
        string[] xy = transform.parent.name.Split(new char[] { ' ' });
        int x = int.Parse(xy[1]);
        int y = int.Parse(xy[0]);

        //Add the cell on which the player is located
        cellsMoveList.Add(transform.parent.gameObject);
        int countCells = cellsMoveList.Count;

        //Breadth-first search
        //count of steps for moving to cell
        for (int step = 1; step <= moveSpeed; step++)
        {
            countCells = cellsMoveList.Count;
            for(int i = 0; i <countCells; i++)
            {
                xy = cellsMoveList[i].name.Split(new char[] { ' ' });
                x = int.Parse(xy[1]);
                y = int.Parse(xy[0]);

                for (int j = 0; j < cellsParent.transform.childCount; j++)
                {
                    GameObject cell = cellsParent.transform.GetChild(j).gameObject;

                    bool skip = false;
                    //skip iteration if on this cell stay enemy
                    for(int ch = 0; ch < cell.transform.childCount; ch++)
                    {
                        if (cell.transform.GetChild(ch).tag == "Enemy") skip = true;
                    }

                    //skip iteration if this cell already in the list
                    for(int c = 0; c < cellsMoveList.Count; c++)
                    {
                        if (cell == cellsMoveList[c]) skip = true;
                    }

                    if (skip) continue;

                    string[] xy_ = cell.name.Split(new char[] { ' ' });
                    int x_ = int.Parse(xy_[1]);
                    int y_ = int.Parse(xy_[0]);

                    if ((Mathf.Abs(x - x_) + Mathf.Abs(y - y_)) == 1)
                    {
                        cellsMoveList.Add(cell);
                        cell.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }
            }
        }
        //Remove the cell on which player stay
        cellsMoveList.RemoveAt(0);
    }

    //Make all cells gray and clean the list
    void CleanCells()
    {
        for(int i = 0; i < cellsParent.transform.childCount; i++)
        {
            cellsParent.transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.gray;
        }
        cellsMoveList.Clear();
    }

    //Checking, is can player move in this cell
    public void CheckCellInMoveList(GameObject cell)
    {
        for(int i = 0; i < cellsMoveList.Count; i++)
        {
            if(cell == cellsMoveList[i])
            {
                MovePlayerOnCell(cell);
            }
        }
    }

    //move player in cell
    private void MovePlayerOnCell(GameObject cell)
    {
        Vector3 pos = cell.transform.position;
        pos.y += 0.3f;
        transform.position = pos;
        transform.parent = cell.transform;
        CellInMoveList();
    }

    class PlayerUnit
    {
        public string name { get; private set; }    
        public GameObject objectPlayer { get; private set; }

        public PlayerUnit(string name, GameObject objectPlayer)
        {
            this.name = name;
            this.objectPlayer = objectPlayer;
        }
    }
}
