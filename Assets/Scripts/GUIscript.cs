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

    private GameObject cellNow;

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

    void ChooseCellOnPanelOfSpells(string direction)
    {
        //Definition of current cell
        string[] xy = cellNow.name.Split(new char[] { ' ' });
        int x = int.Parse(xy[1]);
        int y = int.Parse(xy[0]);

        string newTargetCellName = "";

        //Definition the name of the cell to which the target will be moved
        if (direction == "Up")
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

        for(int i = 0; i < cellNow.transform.parent.childCount; i++)
        {
            GameObject cell = cellNow.transform.parent.GetChild(i).gameObject;
            if(cell.name == newTargetCellName)
            {
                cellNow.GetComponent<Image>().color = Color.gray;
                cellNow = cell;
                cellNow.GetComponent<Image>().color = Color.white;
            }
        }
    }

    //Open the panel with magick spells
    void PressSpellBookButton()
    {
        panelOfSpells.SetActive(!panelOfSpells.activeInHierarchy);
        if (panelOfSpells.activeInHierarchy)
        {
            panelOfSpells.GetComponent<PanelOfSpells>().FillCellsWithSpells();
            
            //Make the first cell white
            cellNow = panelOfSpells.transform.GetChild(0).GetChild(0).gameObject;
            cellNow.GetComponent<Image>().color = Color.white;
        }else cellNow.GetComponent<Image>().color = Color.gray;
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
        if (panelOfSpells.activeInHierarchy)
        {
            for(int i = 0; i < cellNow.transform.childCount; i++)
            {
                for(int j = 0; j < Global.persons[0].listMagickSpells.Count; j++)
                {
                    if (cellNow.transform.GetChild(i).name == Global.persons[0].listMagickSpells[j].Name)
                    {
                        Global.persons[0].ObjectPerson.GetComponent<Player>().CellsInMagickList(Global.persons[0].listMagickSpells[j]);
                        panelOfSpells.SetActive(false);
                    }
                }
                
            }
        }else
        playerScript.CheckCellInList(targetCell.transform.parent.gameObject);
    }

    void PressUpbutton()
    {
        string direction = "Up";
        if (panelOfSpells.activeInHierarchy) ChooseCellOnPanelOfSpells(direction);
        else ChooseCell(direction);
    }

    void PressLeftbutton()
    {
        string direction = "Left";
        if (panelOfSpells.activeInHierarchy) ChooseCellOnPanelOfSpells(direction);
        else ChooseCell(direction);
    }

    void PressRightbutton()
    {
        string direction = "Right";
        if (panelOfSpells.activeInHierarchy) ChooseCellOnPanelOfSpells(direction);
        else ChooseCell(direction);
    }

    void PressDownbutton()
    {
        string direction = "Down";
        if (panelOfSpells.activeInHierarchy) ChooseCellOnPanelOfSpells(direction);
        else ChooseCell(direction);
    }
}
