using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charicter : MonoBehaviour
{
    public static float HP
    {
        get
        {
            switch (Managers.Game.PlayerId)
            {
                case 4:
                    return 3f;
                default:
                    return 1f;
            }
        }
    }

    public static float Damage
    {
        get
        {
            switch (Managers.Game.PlayerId)
            {
                case 2:
                    return 1.1f;
                case 5:
                    return 3f;
                default:
                    return 1f;
            }
        }
    }

    public static int Count
    {
        get
        {
            switch (Managers.Game.PlayerId)
            {
                case 1:
                    return 1;
                default:
                    return 0;
            }
        }
    }


    public static float Speed
    {
        get
        {
            switch (Managers.Game.PlayerId)
            {
                case 3:
                    return 1.2f;
                default:
                    return 1f;
            }
        }
    }

    public static float WeaponSpeed
    {
        get
        {
            switch (Managers.Game.PlayerId)
            {
                case 0:
                    return 1f;
                case 1:
                    return 1.1f;
                default:
                    return 1f;
            }
        }
    }

    public static float WeaponRate
    {
        get
        {
            switch (Managers.Game.PlayerId)
            {
                case 0:
                    return 0.9f;
                default:
                    return 1f;
            }
        }
    }
}
