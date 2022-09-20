using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillScript;

public class Person : MonoBehaviour
{
    public abstract class PersonClass
    {
        public string Name { get; protected set; }
        public GameObject ObjectPerson { get; protected set; }
        public int MoveSpeed { get; private set; }
        public Skill Skill { get; set; }
        public float ArmorPercentPenetration { get; private set; }
        public float MagresPercentPenetration { get; private set; }
        public int HealthMAX { get; private set; }
        public int HealthCurrent { get; protected set; }
        public float Armor { get; private set; }
        public float Magresit { get; private set; }

        public Skill[] arrSkill = new Skill[4];
        public bool haveMove = false;

        public PersonClass(string name, GameObject objectPerson, int rangeAttack, int moveSpeed, int damage, float armorPercentPenetration, float magresPercentPenetration, int healthMAX, float armor, float magresit)
        {
            Name = name;
            ObjectPerson = objectPerson;
            MoveSpeed = moveSpeed;
            ArmorPercentPenetration = armorPercentPenetration;
            MagresPercentPenetration = magresPercentPenetration;
            HealthMAX = healthMAX;
            HealthCurrent = healthMAX;
            Armor = armor;
            Magresit = magresit;
        }

        public abstract void StartRound();

        public abstract void AddSkill(Skill spell, int index);

        //the damage decreases depending on the protective indicators
        public void TakeDamage(int damage, SkillScript.TypeDamage typeDamage, float armorPenetrationPercent, float magPenetrationPercent)
        {
            float coefDamage = 0;
            if (typeDamage == SkillScript.TypeDamage.Physical)
            {
                float newArmor = (int)(Armor - Armor / 100 * armorPenetrationPercent);
                coefDamage = (1f - 100f / (100f + newArmor)) * 100f;
            }
            else if (typeDamage == SkillScript.TypeDamage.Magick)
            {
                float newMagres = (int)(Magresit - Magresit / 100 * magPenetrationPercent);
                coefDamage = (1f - 100f / (100f + newMagres)) * 100f;
            }



            damage = (int)(damage - damage / 100f * coefDamage);
            HealthCurrent -= damage;
            Debug.Log(HealthCurrent);


            if (HealthCurrent <= 0) Death();
        }

        //Destroy object
        protected abstract void Death();

        public void Attack(PersonClass person)
        {
            person.TakeDamage(Skill.Damage, Skill.TypeDamage, ArmorPercentPenetration, MagresPercentPenetration);
        }
    }
}
