using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Person;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int arrayX;
    [SerializeField] private int arrayY;

    [SerializeField] private GameObject cell;

    [SerializeField] private GameObject cellsParent;

    // Start is called before the first frame update
    void Start()
    {
        StartRound();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Start turn of the first person on the list
    void StartRound()
    {
        Global.persons[0].StartRound();
    }

    //Delete first person from the list and add him on end of the list
    public void ChangeTurn()
    {
        PersonClass buff = Global.persons[0];
        buff.haveMove = false;
        Global.persons.RemoveAt(0);
        Global.persons.Add(buff);
        StartRound();
    }

    //Spawn cells with an adjustable size
    public void SpawnCells()
    {
        for(int j = 0; j < arrayY; j++)
        {
            for(int i = 0; i < arrayX; i++)
            {
                float xPosition = cellsParent.transform.position.x + 0.9f * i;
                float yPosition = cellsParent.transform.position.y - 0.9f * j;
                Vector3 position = Vector3.zero;
                position.x = xPosition;
                position.y = yPosition;
                GameObject cellNew = Instantiate(cell, position, Quaternion.identity, cellsParent.transform);
                cellNew.name = $"{j} {i}";
                cellNew.GetComponent<SpriteRenderer>().color = Color.gray;
            }
        }
    }

    public void DestroyCells()
    {
        while(cellsParent.transform.childCount > 0)
        {
            DestroyImmediate(cellsParent.transform.GetChild(0).gameObject);
        }      
    }
}
