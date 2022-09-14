using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private new string name = "Dickhead";
    [SerializeField] private int moveSpeed = 3;
    [SerializeField] private int rangeAttack = 1;
    [SerializeField] private int damage = 20;
    [SerializeField] private int armorPercentPenetration = 20;
    [SerializeField] private int magresPercentPenetration = 0;
    [SerializeField] private int health = 50;
    [SerializeField] private float armor = 0;
    [SerializeField] private int magresist = 0;


    EnemyClass enemyUnit;
    static GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        enemyUnit = new EnemyClass(name,
                                 gameObject,
                                 rangeAttack,
                                 moveSpeed,
                                 damage,
                                 armorPercentPenetration,
                                 magresPercentPenetration,
                                 health,
                                 armor,
                                 magresist);

        Global.persons.Add(enemyUnit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Get damage from player and send to EnemyClass
    public void TakeDamage(int damage, string typeDamage, float armorPenetrationPercent, float magPenetrationPercent)
    {
        enemyUnit.TakeDamage(damage, typeDamage, armorPenetrationPercent, magPenetrationPercent);
    }

    //Method for the test of changing turn
    void qwe()
    {
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(2);
        gameManager.ChangeTurn();
        Debug.Log("Change");
    }

    class EnemyClass : Person.PersonClass
    {
        public EnemyClass(string name,
                           GameObject objectPerson,
                           int rangeAttack,
                           int moveSpeed,
                           int damage,
                           float armorPercentPenetration,
                           float magresPercentPenetration,
                           int healtMAX,
                           float armor,
                           float magresist) : base(name,
                                                   objectPerson,
                                                   rangeAttack,
                                                   moveSpeed,
                                                   damage,
                                                   armorPercentPenetration,
                                                   magresPercentPenetration,
                                                   healtMAX,
                                                   armor,
                                                   magresist)
        {
        }

        //the damage decreases depending on the protective indicators
        public void TakeDamage(int damage, string typeDamage ,float armorPenetrationPercent, float magPenetrationPercent)
        {
            float coefDamage = 0;
            if(typeDamage == "ad")
            {
                float newArmor = (int)(Armor - Armor / 100 * armorPenetrationPercent);
                coefDamage = (1f - 100f / (100f + newArmor)) * 100f;
            }
            else if(typeDamage == "ap")
            {
                float newMagres = (int)(Magresit - Magresit / 100 * armorPenetrationPercent);
                coefDamage = (1f - 100f / (100f + newMagres)) * 100f;
            }
            
            
            
            damage = (int)(damage - damage/100f * coefDamage);
            HealthCurrent -= damage;
            Debug.Log(HealthCurrent);


            if (HealthCurrent <= 0) Death();
        }

        //Destroy object
        private void Death()
        {
            Destroy(ObjectPerson);
        }

        //the actions of the enemy when his turn begins
        public override void StartRound()
        {
            Debug.Log("Enemy turn");
            ObjectPerson.GetComponent<Enemy>().qwe();
        }

        
    }
}
