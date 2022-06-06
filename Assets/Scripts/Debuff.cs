using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Debuff
{
    
    public enum Status
    {
        BLEED,
        FREEZE,
        POISON
    }
    public Status debuffStatus;

    public int debuffCD;

    public Debuff(Status _debuffStatus, int _debuffCD)
    {
        debuffStatus = _debuffStatus;
        debuffCD = _debuffCD;
    }
}
