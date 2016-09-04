using UnityEngine;
using System.Collections;

public class UnitActions {

    public string name;
    public string desc;
    public float actionPointCost;
    public bool isCombatAction;

    public UnitActions(string name, string desc, float actionPointCost, bool isCombat)
    {
        this.name = name;
        this.desc = desc;
        this.actionPointCost = actionPointCost;
        isCombatAction = isCombat;
    }
}
