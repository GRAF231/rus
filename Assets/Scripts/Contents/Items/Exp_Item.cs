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
        player.Exp += _exp * _expMul;
        player.Score += _expMul * 3;
    }
}
