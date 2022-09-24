using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillScript : MonoBehaviour
{

    public enum TypeDamage
    {
        Physical,
        Magick
    }

    public class Skill
    {
        public string Name { get; private set; }
        public int Damage { get; private set; }
        public int Range { get; private set; }
        public TypeDamage TypeDamage { get; private set; }
        public int Cooldown { get; private set; }
        public bool Ready { get; private set; }
        public int CurrentCD { get; private set; }
        public Sprite SpriteOfSpell { get; set; }

        public Skill(string name, int damage, int range, TypeDamage typeDamage , int cooldown,Sprite sprite)
        {
            Name = name;
            Damage = damage;
            Range = range;
            TypeDamage = typeDamage;
            Cooldown = cooldown;
            Ready = true;
            CurrentCD = 0;
            SpriteOfSpell = sprite;
        } 

        public void GoToCooldown()
        {
            CurrentCD = Cooldown;
            if (CurrentCD != 0) Ready = false;
        }

        public void DecreaseCurrentCooldown()
        {
            if (CurrentCD > 0) CurrentCD--;
            if(CurrentCD == 0) Ready = true;
        }

        public override string ToString()
        {
            return string.Join("\n", $"Название: {Name}", $"Урон: {Damage}", $"Дальность: {Range}", $"Перезарядка: {Cooldown}");
        }
    }
}
