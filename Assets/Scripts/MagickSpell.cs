using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagickSpell : MonoBehaviour
{

    public class MagickSpellClass
    {
        public string Name { get; private set; }
        public int Damage { get; private set; }
        public int Range { get; private set; }
        public Sprite SpriteOfSpell { get; set; }

        public MagickSpellClass(string name, int damage, int range, Sprite sprite)
        {
            Name = name;
            Damage = damage;
            Range = range;
            SpriteOfSpell = sprite;
        } 
    }
}
