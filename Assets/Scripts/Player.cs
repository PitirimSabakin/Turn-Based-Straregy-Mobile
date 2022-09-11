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

        ChangeColorCell();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Change color of cell, which player can move on it
    void ChangeColorCell()
    {
        //clear list of cells
        cellsMoveList.Clear();

        //get coordinates cell which player stay it
        string[] xy = transform.parent.name.Split(new char[] { ' ' });
        int x = int.Parse(xy[1]);
        int y = int.Parse(xy[0]);
        for (int i = 0; i < cellsParent.transform.childCount; i++)
        {
            GameObject cell = cellsParent.transform.GetChild(i).gameObject;
            cell.GetComponent<SpriteRenderer>().color = Color.gray;

            //get coordintates cell in cycle
            string[] xy_ = cell.name.Split(new char[] { ' ' });
            int x_ = int.Parse(xy_[1]);
            int y_ = int.Parse(xy_[0]);

            //checking whether the player can move into this cell
            if ((Mathf.Abs(x - x_) + Mathf.Abs(y - y_)) <= moveSpeed && transform.parent.gameObject != cell)
            {
                cell.GetComponent<SpriteRenderer>().color = Color.white;
                cellsMoveList.Add(cell);
            }
        }
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
        ChangeColorCell();
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
