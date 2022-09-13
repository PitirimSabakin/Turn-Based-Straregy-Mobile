using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private new string name = "Dickhead";
    [SerializeField] private int health =  50;
    [SerializeField] private float armor = 0;
    [SerializeField] private int magresist = 0;

    EnemyClass enemyUnit;

    // Start is called before the first frame update
    void Start()
    {
        enemyUnit = new EnemyClass(name, health, armor, magresist, this.gameObject);
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

    class EnemyClass
    {
        public string name { get; private set; }
        public GameObject enemyOgject { get; private set; }
        public int healthMAX { get; private set; }
        public int healthCurrent { get; private set; }
        public float armor { get; private set; }
        public int magresit { get; private set; }

        public EnemyClass(string name, int health, float armor, int magresit, GameObject enemyObject)
        {
            this.name = name;
            this.healthMAX = health;
            healthCurrent = health;
            this.armor = armor;
            this.magresit = magresit;
            this.enemyOgject = enemyObject;
        }

        //the damage decreases depending on the protective indicators
        public void TakeDamage(int damage, string typeDamage ,float armorPenetrationPercent, float magPenetrationPercent)
        {
            float coefDamage = 0;
            if(typeDamage == "ad")
            {
                float newArmor = (int)(armor - armor / 100 * armorPenetrationPercent);
                coefDamage = (1f - 100f / (100f + newArmor)) * 100f;
            }
            else if(typeDamage == "ap")
            {
                float newMagres = (int)(magresit - magresit / 100 * armorPenetrationPercent);
                coefDamage = (1f - 100f / (100f + magresit)) * 100f;
            }
            
            
            
            damage = (int)(damage - damage/100f * coefDamage);
            healthCurrent -= damage;
            Debug.Log(healthCurrent);


            if (healthCurrent <= 0) Death();
        }

        //Destroy object
        private void Death()
        {
            Destroy(enemyOgject);
        }
    }
}
