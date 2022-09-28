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
        public float HealthMAX { get; private set; }
        public float HealthCurrent { get; protected set; }
        public float Armor { get; private set; }
        public float Magresit { get; private set; }
        public HealthBar HealthBar { get; private set; }

        private List<List<Transform>> cellsInMoveListForAnomation = new List<List<Transform>>();

        public Skill[] arrSkill = new Skill[4];
        public bool haveMove = false;

        public PersonClass(string name, GameObject objectPerson, int rangeAttack, int moveSpeed, int damage, float armorPercentPenetration, float magresPercentPenetration, int healthMAX, float armor, float magresit, HealthBar healthBar)
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
            HealthBar = healthBar;
        }

        public abstract void StartRound();

        //Replace skill in array of skills which player own
        public void AddSkill(Skill spell, int index)
        {
            arrSkill[index] = spell;
        }

        public void TakeDamage(int damage)
        {
            HealthCurrent -= damage;

            HealthBar.UpdateHealthBar(HealthCurrent, HealthMAX);

            ObjectPerson.GetComponent<Animator>().SetTrigger("TakeDamage");

            if (HealthCurrent <= 0) Death();
        }

        //Destroy object
        protected abstract void Death();

        public abstract void Attack(PersonClass person);


        //When round start cd all of skills decreases by 1
        protected void DecreaseCooldown()
        {
            for (int i = 0; i < arrSkill.Length; i++)
            {
                if (arrSkill[i] is not null) arrSkill[i].DecreaseCurrentCooldown();
            }
        }

        //find the nearest player
        public Transform LookingForNearbestPerson()
        {
            int xNearbestPerson = 99;
            int yNearbestPerson = 99;
            Transform person = null;

            //get coordinates cell which enemy stay it
            string[] xy = ObjectPerson.transform.parent.name.Split(new char[] { ' ' });
            int xCurrentPerson = int.Parse(xy[1]);
            int yCurrentPerson = int.Parse(xy[0]);

            for (int i = 0; i < Global.persons.Count; i++)
            {
                if ( (ObjectPerson.tag == "Enemy" && Global.persons[i].ObjectPerson.tag == "Player")
                    || (ObjectPerson.tag == "Player" && Global.persons[i].ObjectPerson.tag == "Enemy"))
                {
                    //get coordinates cell which player stay it
                    string[] xy_ = Global.persons[i].ObjectPerson.transform.parent.name.Split(new char[] { ' ' });
                    int xNearbestPerson_ = int.Parse(xy_[1]);
                    int yNearbestPerson_ = int.Parse(xy_[0]);

                    //Checking which player closer
                    if (Mathf.Abs(xNearbestPerson_ - xCurrentPerson) + Mathf.Abs(yNearbestPerson_ - yCurrentPerson)
                        < Mathf.Abs(xNearbestPerson - xCurrentPerson) + Mathf.Abs(yNearbestPerson - yCurrentPerson))
                    {
                        xNearbestPerson = xNearbestPerson_;
                        yNearbestPerson = yNearbestPerson_;
                        person = Global.persons[i].ObjectPerson.transform;
                    }
                }
            }
            return person;
        }

        //Turn the person to face the he`s enemy
        public void ReversePerson(Transform toPerson)
        {
            
            SpriteRenderer spriteRenderer = ObjectPerson.GetComponent<SpriteRenderer>();

            //get x current person
            string[] xyCurrent = ObjectPerson.transform.parent.name.Split(new char[] { ' ' });
            int xCurrentPerson = int.Parse(xyCurrent[1]);

            //get x nearbest person
            string[] xyNearbest = toPerson.parent.name.Split(new char[] { ' ' });
            int xNearbestPerson = int.Parse(xyNearbest[1]);

            if (xCurrentPerson < xNearbestPerson && spriteRenderer.flipX) spriteRenderer.flipX = false;
            else if(xCurrentPerson > xNearbestPerson && !spriteRenderer.flipX) spriteRenderer.flipX = true;
        }

        public void ReversePerson()
        {
            Transform nearbestPerson = LookingForNearbestPerson();
            ReversePerson(nearbestPerson);
        }

        public List<Transform> CellToMoveListForAnimation(List<Transform> cellsInMoveList, Transform cellToFindPath)
        {
            List<Transform> firstList = new List<Transform> ();
            firstList.Add(ObjectPerson.transform.parent);

            cellsInMoveListForAnomation.Clear();
            cellsInMoveListForAnomation.Add(firstList);

            while (true)
            {
                int count = cellsInMoveListForAnomation.Count;
                for(int i = 0; i < count; i++)
                {
                    string[] xyCurrent = cellsInMoveListForAnomation[i][cellsInMoveListForAnomation[i].Count - 1].name.Split(new char[] { ' ' });
                    int xCurrnetStep = int.Parse(xyCurrent[1]);
                    int yCurrentSrep = int.Parse(xyCurrent[0]);

                    for (int j = 0; j < cellsInMoveList.Count; j++)
                    {
                        string[] xyNext = cellsInMoveList[j].name.Split(new char[] { ' ' });
                        int xNextStep = int.Parse(xyNext[1]);
                        int yNextStep = int.Parse(xyNext[0]);

                        bool skip = false;

                        for(int k = 0; k < cellsInMoveListForAnomation.Count; k++)
                        {
                            for(int l = 0; l < cellsInMoveListForAnomation[k].Count; l++)
                            {
                                if (cellsInMoveList[j] == cellsInMoveListForAnomation[k][l]) skip = true;
                            }
                        }

                        if(skip) continue;

                        if(Mathf.Abs(xCurrnetStep - xNextStep) + Mathf.Abs(yCurrentSrep - yNextStep) == 1)
                        {
                            List<Transform> newList = new List<Transform>(cellsInMoveListForAnomation[i]);
                            newList.Add(cellsInMoveList[j]);

                            if (cellsInMoveList[j] == cellToFindPath) 
                            {
                                newList.RemoveAt(0);
                                return newList;
                            } 

                            cellsInMoveListForAnomation.Add(newList);
                        }
                    }
                }

                for(int i = 0; i < count; i++)
                {
                    cellsInMoveListForAnomation.RemoveAt(i);
                }

            }
        }
    }
}
