using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Resources {

    public  readonly float wood;
    public  readonly float gunpowder;


    public Resources(float w, float g)
    {
        wood = w;
        gunpowder = g;
    }
	
    public static Resources operator +(Resources r1, Resources r2)
    {
        return new Resources(r1.wood + r2.wood, r1.gunpowder + r2.gunpowder);
    }

    public static Resources operator -(Resources r1, Resources r2)
    {
        return new Resources(r1.wood - r2.wood, r1.gunpowder - r2.gunpowder);
    }

    public override string ToString()
    {
        return "wood: " + wood + " | gunpowder: " + gunpowder;
    }

}
