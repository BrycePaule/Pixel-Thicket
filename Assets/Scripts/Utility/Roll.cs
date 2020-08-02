using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Roll
{

    public static bool Chance(int chance)
    {
        int roll = Random.Range(0, 100);

        if (roll <= chance)
        {
            return true;
        }

        return false;
    }

}
