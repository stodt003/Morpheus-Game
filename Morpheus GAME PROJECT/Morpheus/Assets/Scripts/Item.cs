using UnityEngine;
using System.Collections;

public class Item  {

    public string name;
    public string desc;
    public bool canBeEquipped;
    public bool canBeConsumed;

    public Item()
    {
        name = "Item";
        desc = "Basic Item";
        canBeEquipped = true;
        canBeConsumed = false;
    }

    public Item(string name, string desc, bool equippable, bool consumable)
    {
        this.name = name;
        this.desc = desc;
        canBeEquipped = equippable;
        canBeConsumed = consumable;
    }

}
