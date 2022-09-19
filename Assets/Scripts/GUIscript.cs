using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIscript : MonoBehaviour
{
    private Button move;
    private Button skipTurn;

    private GameObject targetCell;
    private GameManager gameManager;

    private Player playerScript;

    [SerializeField] private List<GameObject> buttonsOfSkills;

    // Start is called before the first frame update
    void Awake()
    {
        //Find UI buttons
        move = GameObject.Find("Move").GetComponent<Button>();
        skipTurn = GameObject.Find("SkipButton").GetComponent<Button>();

        targetCell = GameObject.Find("TargetCell");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        playerScript = GameObject.Find("Player").GetComponent<Player>();

        move.onClick.AddListener(PressMoveButton);
        skipTurn.onClick.AddListener(PresSkipButton);
    }

    //Fill the panel with skills
    public void FillPanelOfSkills()
    {
        for (int i = 0; i < Global.persons[0].arrSkill.Length; i++)
        {
            if(Global.persons[0].arrSkill[i] != null)
            {
                buttonsOfSkills[i].transform.GetChild(0)
                .GetComponent<Image>().sprite = Global.persons[0].arrSkill[i].SpriteOfSpell;

                //if skill is not ready, button is not interactable
                if (Global.persons[0].arrSkill[i].Ready == false)
                {
                    buttonsOfSkills[i].GetComponent<Button>().interactable = false;
                    buttonsOfSkills[i].transform.GetChild(0).GetComponent<Image>().color = Color.gray;
                }
                else
                {
                    buttonsOfSkills[i].GetComponent<Button>().interactable = true;
                    buttonsOfSkills[i].transform.GetChild(0).GetComponent<Image>().color = Color.white;
                }
            }
            
        }
    }

    //Moving target. Can choose need cell
    public void ChooseCell(string direction)
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

    public void ChooseSkill(int index)
    {
        if (Global.persons[0].arrSkill[index] != null)
        {
            Global.persons[0].Skill = Global.persons[0].arrSkill[index];
            Global.persons[0].ObjectPerson.GetComponent<Player>().CellInAttackList();
        }
            
    }
}
