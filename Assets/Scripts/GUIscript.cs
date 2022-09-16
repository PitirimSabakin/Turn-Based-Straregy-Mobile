using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIscript : MonoBehaviour
{
    private Button up;
    private Button left;
    private Button down;
    private Button right;
    private Button move;
    private Button skipTurn;
    private Button spellbook;

    private GameObject targetCell;
    private GameManager gameManager;

    private Player playerScript;

    [SerializeField] private GameObject panelOfSpells;

    // Start is called before the first frame update
    void Start()
    {
        //Find UI buttons
        up = GameObject.Find("Up").GetComponent<Button>();
        left = GameObject.Find("Left").GetComponent<Button>();
        right = GameObject.Find("Right").GetComponent<Button>();
        down = GameObject.Find("Down").GetComponent<Button>();
        move = GameObject.Find("Move").GetComponent<Button>();
        skipTurn = GameObject.Find("SkipButton").GetComponent<Button>();
        spellbook = GameObject.Find("Spellbook").GetComponent<Button>();

        targetCell = GameObject.Find("TargetCell");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        playerScript = GameObject.Find("Player").GetComponent<Player>();

        up.onClick.AddListener(PressUpbutton);
        left.onClick.AddListener(PressLeftbutton);
        right.onClick.AddListener (PressRightbutton);
        down.onClick.AddListener(PressDownbutton);
        move.onClick.AddListener(PressMoveButton);
        skipTurn.onClick.AddListener(PresSkipButton);
        spellbook.onClick.AddListener(PressSpellBookButton);
    }

    //Moving target. Can choose need cell
    void ChooseCell(string direction)
    {
        //Definition of current cell
        string[] xy = targetCell.transform.parent.name.Split(new char[] { ' ' });
        int x = int.Parse(xy[1]);
        int y = int.Parse(xy[0]);

        string newTargetCellName = "";

        //Definition the name of the cell to which the target will be moved
        if(direction == "Up")
        {
            newTargetCellName = $"{y - 1} {x}";
        }

        if (direction == "Left")
        {
            newTargetCellName = $"{y} {x - 1}";
        }

        if (direction == "Down")
        {
            newTargetCellName = $"{y + 1} {x}";
        }

        if (direction == "Right")
        {
            newTargetCellName = $"{y} {x + 1}";
        }

        //Move target, if cell is exists
        GameObject cell = GameObject.Find(newTargetCellName);
        if (cell is not null)
        {
            targetCell.transform.position = cell.transform.position;
            targetCell.transform.parent = cell.transform;
        }
    }  

    //Open the panel with magick spells
    void PressSpellBookButton()
    {
        panelOfSpells.SetActive(!panelOfSpells.activeInHierarchy);
        if(panelOfSpells.activeInHierarchy)
        panelOfSpells.GetComponent<PanelOfSpells>().FillCellsWithSpells();
    }

    //Skip turn
    void PresSkipButton()
    {
        Global.persons[0].ObjectPerson.GetComponent<Player>().CleanCells();
        gameManager.ChangeTurn();
    }

    //When the move button is pressed, targetCell passed for checking
    void PressMoveButton()
    {    
        playerScript.CheckCellInList(targetCell.transform.parent.gameObject);
    }

    void PressUpbutton()
    {
        string direction = "Up";
        ChooseCell(direction);
    }

    void PressLeftbutton()
    {
        string direction = "Left";
        ChooseCell(direction);
    }

    void PressRightbutton()
    {
        string direction = "Right";
        ChooseCell(direction);
    }

    void PressDownbutton()
    {
        string direction = "Down";
        ChooseCell(direction);
    }
}
