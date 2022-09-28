using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillScript;

public class Player : MonoBehaviour
{
    [SerializeField] private new string name = "Asshole";
    [SerializeField] private int moveSpeed = 3;
    [SerializeField] private int rangeAttack = 1;
    [SerializeField] private int damage = 20;
    [SerializeField] private int armorPercentPenetration = 20;
    [SerializeField] private int magresPercentPenetration = 0;
    [SerializeField] private int health = 50;
    [SerializeField] private float armor = 0;
    [SerializeField] private int magresist = 0;
    [SerializeField] private List<Sprite> listOfSprite;

    private bool isMoving = false;
    List<Transform> cellsToMoveForAnimaion = new List<Transform>();
    private float elapsedTime = 0f;
    private GameObject toCell;

    private GameObject cellsParent;
    private GUIscript canvas;

    private List<Transform> cellsMoveList = new List<Transform>();
    private List<GameObject> cellsAttackList = new List<GameObject>();

    PlayerClass player;
    GameManager gameManager;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Awake()
    {      
        canvas = GameObject.Find("Canvas").GetComponent<GUIscript>();
        cellsParent = GameObject.Find("cellsParent");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        HealthBar healthBar = transform.GetChild(0).GetChild(0).GetComponent<HealthBar>();

        player = new PlayerClass(name,
                                 gameObject,
                                 rangeAttack,
                                 moveSpeed,
                                 damage,
                                 armorPercentPenetration,
                                 magresPercentPenetration,
                                 health,
                                 armor,
                                 magresist,
                                 healthBar);

        Skill skill = new Skill("sword", 10, 1, TypeDamage.Physical, 0, listOfSprite[0]);        
        player.AddSkill(skill, 0);
        skill = new Skill("fireball", 15, 3, TypeDamage.Magick, 2, listOfSprite[1]);
        player.AddSkill(skill, 1);

        //Setup initial health in healthbar
        healthBar.NumberInHealthBar(health);

        Global.persons.Add(player);

        StartCoroutine(wait());
    }

    void Update()
    {
        if (isMoving)
        {
            MoveAnimation();
        }
    }

    //Move person to each cell in path
    void MoveAnimation()
    {
        if (cellsToMoveForAnimaion.Count != 0)
        {
            if (transform.parent.position.x < cellsToMoveForAnimaion[0].position.x)
            {
                if (spriteRenderer.flipX) spriteRenderer.flipX = false;
                transform.Translate(Vector2.right * Time.deltaTime * 1.5f);
            }
            else if (transform.parent.position.x > cellsToMoveForAnimaion[0].position.x)
            {
                if (!spriteRenderer.flipX) spriteRenderer.flipX = true;
                transform.Translate(Vector2.left * Time.deltaTime * 1.5f);
            }
            else if (transform.parent.position.y < cellsToMoveForAnimaion[0].position.y)
            {
                transform.Translate(Vector2.up * Time.deltaTime * 1.5f);
            }
            else if (transform.parent.position.y > cellsToMoveForAnimaion[0].position.y)
            {
                transform.Translate(Vector2.down * Time.deltaTime * 1.5f);
            }

            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 0.6)
            {
                transform.parent = cellsToMoveForAnimaion[0];
                cellsToMoveForAnimaion.RemoveAt(0);
                elapsedTime = 0;
            }
        }
        else
        {
            isMoving = false;
            MovePlayerOnCell(toCell);
        }
    }

    IEnumerator wait()
    {
        yield return new WaitForEndOfFrame();
        player.ReversePerson();
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
        cellsMoveList.Add(transform.parent);

        //Breadth-first search
        //count of steps for moving to cell
        for (int step = 1; step <= player.MoveSpeed; step++)
        {
            int countCells = cellsMoveList.Count;
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
                        if (cell.transform.GetChild(ch).tag == "Enemy"
                            || cell.transform.GetChild(ch).tag == "Player") skip = true;
                    }

                    //skip iteration if this cell already in the list
                    for(int c = 0; c < cellsMoveList.Count; c++)
                    {
                        if (cell.transform == cellsMoveList[c]) skip = true;
                    }

                    if (skip) continue;

                    string[] xy_ = cell.name.Split(new char[] { ' ' });
                    int x_ = int.Parse(xy_[1]);
                    int y_ = int.Parse(xy_[0]);

                    if ((Mathf.Abs(x - x_) + Mathf.Abs(y - y_)) == 1)
                    {
                        cellsMoveList.Add(cell.transform);
                        cell.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }
            }
        }
        //Remove the cell on which player stay
        cellsMoveList.RemoveAt(0);
    }

    //Make all cells gray and clean the list
    public void CleanCells()
    {
        for(int i = 0; i < cellsParent.transform.childCount; i++)
        {
            cellsParent.transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.gray;
        }
        cellsMoveList.Clear();
        cellsAttackList.Clear();
    }

    //Checking, is can player move in this cell
    public void CheckCellInList(GameObject cell)
    {
        for(int i = 0; i < cellsMoveList.Count; i++)
        {
            if(cell.transform == cellsMoveList[i])
            {
                //MovePlayerOnCell(cell);
                toCell = cell;
                cellsToMoveForAnimaion = player.CellToMoveListForAnimation(cellsMoveList, cell.transform);
                elapsedTime = 0f;
                GetComponent<Animator>().SetBool("Move", true);
                CleanCells();
                isMoving = true;
                return;
            }
        }

        for (int i = 0; i < cellsAttackList.Count; i++)
        {
            if (cell == cellsAttackList[i])
            {
                for(int j = 0;  j < cell.transform.childCount; j++)
                {
                    if(cell.transform.GetChild(j).tag == "Enemy")
                    {
                        Person.PersonClass enemy = Global.persons.Find(p => p.ObjectPerson == cell.transform.GetChild(j).gameObject);
                        player.Attack(enemy);
                        CleanCells();
                        //Turn go the next person after attack
                    }
                }
                return;
            }
        }
    }

    public void CellInAttackList()
    {
        for(int i = 0; i < cellsAttackList.Count; i++)
        {
            cellsAttackList[i].GetComponent<SpriteRenderer>().color = Color.gray;
        }

        cellsAttackList.Clear();

        //get coordinates cell which player stay it
        string[] xy = transform.parent.name.Split(new char[] { ' ' });
        int x = int.Parse(xy[1]);
        int y = int.Parse(xy[0]);

        for (int i = 0; i < cellsParent.transform.childCount; i++)
        {
            GameObject cell = cellsParent.transform.GetChild(i).gameObject;

            string[] xy_ = cell.name.Split(new char[] { ' ' });
            int x_ = int.Parse(xy_[1]);
            int y_ = int.Parse(xy_[0]);

            for (int c = 0; c < cell.transform.childCount; c++)
            {
                if(cell.transform.GetChild(c).tag == "Enemy"
                    && (Mathf.Abs(x - x_) + Mathf.Abs(y - y_)) <= player.Skill.Range)
                {
                    cellsAttackList.Add(cell);
                    cell.GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
        }
    }

    //move player in cell
    private void MovePlayerOnCell(GameObject cell)
    {
        Vector3 pos = cell.transform.position;
        pos.y += 0.4f;
        transform.position = pos;
        transform.parent = cell.transform;
        GetComponent<Animator>().SetBool("Move", false);

        //Player can only attack after movement
        player.haveMove = false;
        CleanCells();
        CellInAttackList();
    }

    //Change turn after playing animation
    public void ChangeTurnAfterAttack()
    {
        gameManager.CheckGameOver();
    }

    //Destroy object after playing animation
    public void Death()
    {
        Destroy(gameObject);
    }

    class PlayerClass: Person.PersonClass
    {
        public PlayerClass(string name,
                           GameObject objectPlayer,
                           int rangeAttack,
                           int moveSpeed,
                           int damage,
                           float armorPercentPenetration,
                           float magresPercentPenetration,
                           int healtMAX,
                           float armor,
                           float magresist,
                           HealthBar healthBar) : base(name,
                                                       objectPlayer,
                                                       rangeAttack,
                                                       moveSpeed,
                                                       damage,
                                                       armorPercentPenetration,
                                                       magresPercentPenetration,
                                                       healtMAX,
                                                       armor,
                                                       magresist,
                                                       healthBar)
        {
        }        

        //Actions what player do when start his turn
        public override void StartRound()
        {
            haveMove = true;
            ObjectPerson.GetComponent<Player>().CellInMoveList();
            ObjectPerson.GetComponent<Player>().canvas.FillPanelOfSkills();
            Skill = arrSkill[0];
            DecreaseCooldown();
        }

        public override void Attack(Person.PersonClass person)
        {
            ReversePerson(person.ObjectPerson.transform);
            person.TakeDamage(Skill.Damage);
            Skill.GoToCooldown();

            if (Skill.Range == 1) ObjectPerson.GetComponent<Animator>().SetTrigger("Attack");
            else ObjectPerson.GetComponent<Animator>().SetTrigger("SuperAttack");
        }

        protected override void Death()
        {
            ObjectPerson.transform.parent = null;
            for (int i = 0; i < Global.persons.Count; i++)
            {
                if (Global.persons[i].ObjectPerson == ObjectPerson) Global.persons.RemoveAt(i);
            }
            ObjectPerson.GetComponent<Animator>().SetTrigger("Death");
        }
    }
}
