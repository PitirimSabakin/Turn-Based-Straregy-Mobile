using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MagickSpell;
using static Person;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int arrayX;
    [SerializeField] private int arrayY;

    [SerializeField] private GameObject cell;

    [SerializeField] private GameObject cellsParent;
    [SerializeField] private GameObject buttons;
    [SerializeField] private List<Sprite> listOfSprite;

    private List<MagickSpellClass> listAllMagickSpells = new List<MagickSpellClass>();


    // Start is called before the first frame update
    void Start()
    {
        SpellsInlist();
        StartRound();
    }

    //Add spells in the list of all spells in game
    void SpellsInlist()
    {
        Sprite spriteSpell = null;
        MagickSpellClass spell;
        for (int i = 0; i< listOfSprite.Count; i++)
        {
            if (listOfSprite[i].name == "fireball") spriteSpell = listOfSprite[i];   
        }
        spell = new MagickSpellClass("fireball", 15, 4, spriteSpell);
        listAllMagickSpells.Add(spell);

        Global.persons[0].AddSpell(spell);
    }

    //Start turn of the first person on the list
    void StartRound()
    {
        //Disable GUI buttons if it is the enemy turn now
        if (Global.persons[0].ObjectPerson.tag == "Enemy")
        {
            buttons.SetActive(false);
        }else buttons.SetActive(true);
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
