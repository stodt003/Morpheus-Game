using UnityEngine;
using System.Collections;

public class Weapon : Item {

    public float weaponDamage;
    public float range;
    public float ammoClipSize;
    public bool meleeWeapon;

    // Default (Ranged) Weapon Constructor
    public Weapon()
    {
        canBeEquipped = true;
        canBeConsumed = false;
        name = "Basic Ranged Weapon";
        desc = "I'm a Basic Ranged Weapon";

        weaponDamage = 1;
        range = 5;
        ammoClipSize = 1;
        meleeWeapon = false;

    }
    // Ranged Weapon Constructor
    public Weapon(string name, string desc, float damage, float range, float ammoClip)
    {
        canBeEquipped = true;
        canBeConsumed = false;
        this.name = name;
        this.desc = desc;

        weaponDamage = damage;
        this.range = range;
        ammoClipSize = ammoClip;
        meleeWeapon = false;

    }
    // Melee Weapon Constructor
    public Weapon(string name, string desc, float damage)
    {
        canBeEquipped = true;
        canBeConsumed = false;
        this.name = name;
        this.desc = desc;
        
        weaponDamage = damage;
        range = 0;
        ammoClipSize = 0;
        meleeWeapon = true;
    }

}
