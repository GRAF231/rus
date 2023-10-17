using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Exp_Item : Base_Item
{
    public Sprite[] _sprite;
    public long _exp;
    public long _expMul= 1;

    public override void OnItemEvent(PlayerStat player)
    {
       /* long percentExp = (long)Math.Truncate((double)player.MaxExp / 100);
        if (percentExp > _exp)
            _exp = percentExp;*/
        player.Exp += _exp * _expMul;
        UnityEngine.Debug.Log(player.MaxExp);
        player.Score += _expMul * 3;
    }
}
