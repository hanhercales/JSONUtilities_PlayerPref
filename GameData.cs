using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
[System.Serializable]
public class GameData
{
    public int maxHp, hp, maxMp, mp, maxXp, xp, xpStore, def, atk,
        str, con, luk, sp, level, skill;
    public float crit, teamCd;

    public GameData()
    {
        //stats
        maxHp = 100;
        hp = maxHp;
        maxMp = 150;
        mp = 0;
        maxXp = 200;
        xp = 0;
        xpStore = 0;
        def = 4; 
        atk = 20;
        crit = 5;
        level = 1;
        str = 10;
        con = 5;
        luk = 3;
        sp = 0;
        
        //skill
        skill = 0;
        teamCd = 0;
    }
}
