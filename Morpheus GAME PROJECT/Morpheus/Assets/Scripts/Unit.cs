using UnityEngine;
using System.Collections;

public class Unit {

    // Default Unit Stats
    public float health;
    public float defence;
    public float aim;
    public float mobility;
    public float damage;
    public Weapon primaryWeapon;
    // NON movement actions to be taken per turn.
    public int actionPoints;
    

    // Default Constructor
    public Unit()
    {
        health = 1;
        defence = 1;
        aim = 50;
        mobility = 2;
        damage = 0;
        actionPoints = 1;
    }
    // Custom Constructor
    public Unit(float hp, float def, float aim, float mob, float damage, int actionPts)
    {
        health = hp;
        defence = def;
        this.aim = aim;
        mobility = mob;
        this.damage = damage;
        actionPoints = actionPts;
    }

    public bool Alive()
    {
        if(health > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    





}
