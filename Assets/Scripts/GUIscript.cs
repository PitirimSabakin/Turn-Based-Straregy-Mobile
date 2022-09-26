using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUIscript : MonoBehaviour
{
    private Button skipTurn;
    private RectTransform pointer;

    private GameManager gameManager;

    [SerializeField] private List<GameObject> buttonsOfSkills;

    // Start is called before the first frame update
    void Awake()
    {
        //Find UI buttons
        skipTurn = GameObject.Find("SkipButton").GetComponent<Button>();

        pointer = GameObject.Find("Pointer").GetComponent<RectTransform>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        skipTurn.onClick.AddListener(PresSkipButton);
    }

    //Touch screen and choose cell
    void Update()
    {
        //Can touch if player turn
        if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended && Global.persons[0].ObjectPerson.tag == "Player")
        {
            //touch`s position
            Vector2 worldTouch = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            Collider2D collider = Physics2D.OverlapPoint(worldTouch);

            if (collider != null)
            {
                //if player touches cell
                if(collider.transform.parent.name == "cellsParent")
                {
                    Global.persons[0].ObjectPerson
                        .GetComponent<Player>()
                        .CheckCellInList(collider.transform.gameObject);
                }    
            }
        }
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

    //Skip turn
    void PresSkipButton()
    {
        Global.persons[0].ObjectPerson.GetComponent<Player>().CleanCells();
        gameManager.ChangeTurn();
    }

    public void ChooseSkill(int index)
    {
        if (Global.persons[0].arrSkill[index] != null)
        {
            Global.persons[0].Skill = Global.persons[0].arrSkill[index];
            Global.persons[0].ObjectPerson.GetComponent<Player>().CellInAttackList();

            GameObject pressButton = GameObject.Find($"Skill{index}");
            pointer.position = new Vector2(pressButton.transform.position.x, pressButton.transform.position.y + 60);
        }
            
    }

    public void StartMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

}
