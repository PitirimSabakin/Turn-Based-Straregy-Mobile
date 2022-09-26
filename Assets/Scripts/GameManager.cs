using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillScript;
using static Person;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int arrayX;
    [SerializeField] private int arrayY;

    [SerializeField] private GameObject cell;

    [SerializeField] private GameObject cellsParent;
    [SerializeField] private GameObject buttons;
    [SerializeField] private GameObject panelOfGameOver;

    // Start is called before the first frame update
    void Start()
    {       
        StartCoroutine(waitStart());
    }

    IEnumerator waitStart()
    {
        yield return new WaitForEndOfFrame();
        StartRound();
    }

    //Start turn of the first person on the list
    void StartRound()
    {
        //Disable GUI buttons if it is the enemy turn now
        if (Global.persons[0].ObjectPerson.tag == "Enemy")
        {
            buttons.SetActive(false);
        }
        else buttons.SetActive(true);
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

    public void CheckGameOver()
    {
        bool havePlayer = false;
        bool haveEnemy = false;
        for (int i = 0; i < Global.persons.Count; i++)
        {
            if (Global.persons[i].ObjectPerson.tag == "Player") havePlayer = true;
            if (Global.persons[i].ObjectPerson.tag == "Enemy") haveEnemy = true;
        }
        if (!havePlayer || !haveEnemy)
        {
            StartCoroutine(waitLastDeath());
            if (!havePlayer) panelOfGameOver.transform.GetChild(0).GetComponent<TMP_Text>().text = "Поражение";
            else panelOfGameOver.transform.GetChild(0).GetComponent<TMP_Text>().text = "Победа";
        }
        else StartCoroutine(waitChange());
    }

    IEnumerator waitLastDeath()
    {
        yield return new WaitForSeconds(1);
        panelOfGameOver.SetActive(true);       
    }

    IEnumerator waitChange()
    {
        yield return new WaitForEndOfFrame();
        ChangeTurn();
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
