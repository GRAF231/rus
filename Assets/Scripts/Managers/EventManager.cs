using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static Define;

public class EventManager
{
    public int maxWeaponLevel = 7;

    public struct ItemInfo
    {
        public int Type { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
    }

    public List<ItemInfo> SetRandomItem(PlayerStat player, int Maxcount)
    {
        int i = 0;
        int loopCounter = 0;
        List<ItemInfo> PoolList = new List<ItemInfo>();
        while (i < Maxcount && loopCounter < 200)
        {
            loopCounter++;
            ItemInfo selected;
            float random = Random.Range(0, 100);
            int rd = 0;

            Debug.Log($"Random number for levelUp : {random}");
            if (random < 30)
                rd = 0;
            else if (random < 50)
                rd = 1;
            else
                rd = 2;


            if (rd== 0)
            {
                if (player.GetWeaponDict()[player.playerStartWeapon] >= maxWeaponLevel)
                    continue;

                var weapon = Managers.Data.WeaponData[(int)player.playerStartWeapon];
                selected = new ItemInfo {
                    Type = 0,
                    ID = weapon.weaponID,
                    Name = weapon.weaponName,
                    Title = weapon.weaponTitle,
                    Desc = weapon.weaponDesc,
                };
            }
            else if (rd == 1)
            {
                Data.PlayerStatData stat = SetRandomStat();
                selected = new ItemInfo
                {
                    Type = 1,
                    ID = stat.statID,
                    Name = stat.statName,
                    Title = stat.statTitle,
                    Desc = stat.statDesc,
                };
            }
            else
            {
                var weapon = SetRandomWeapon();
                selected = new ItemInfo
                {
                    Type = 2,
                    ID = weapon.weaponID,
                    Name = weapon.weaponName,
                    Title = weapon.weaponTitle,
                    Desc = weapon.weaponDesc,
                };
                if (player.GetWeaponDict().GetValueOrDefault<Define.Weapons, int>((Define.Weapons)weapon.weaponID) >= maxWeaponLevel)
                    continue;
                if ((int)player.playerStartWeapon == weapon.weaponID)
                    continue;
                if (weapon.weaponID > 100)
                    continue;
                if (player.GetWeaponDict().Count >= 4 && !player.GetWeaponDict().ContainsKey((Define.Weapons)weapon.weaponID))
                    continue;
            }

            bool isContains = false;
            foreach (ItemInfo item in PoolList)
            {
                if(selected.Type == item.Type)
                {
                    Debug.Log($"{item.Type} is already contained");
                    isContains = true;
                    break;
                }
            }
            if (isContains)
                continue;
            PoolList.Add(selected);
            i++;
        }
        return PoolList;
    }

    public Data.PlayerStatData SetRandomStat()
    {
        int statNum = Random.Range(0, Managers.Data.PlayerStatData.Count);
        Debug.Log($"{statNum}, {Managers.Data.PlayerStatData.Count}");
        Data.PlayerStatData playerStats = Managers.Data.PlayerStatData[new List<int>(Managers.Data.PlayerStatData.Keys)[statNum]];

        return playerStats;
    }

    public Data.WeaponData SetRandomWeapon() {
        int weaponNum = Random.Range(0, Managers.Data.WeaponData.Count);
        Debug.Log($"{weaponNum}, {Managers.Data.WeaponData.Count}");
        Data.WeaponData playerWeapon = Managers.Data.WeaponData[new List<int>(Managers.Data.WeaponData.Keys)[weaponNum]];

        return playerWeapon;
    }

    public void DropItem(EnemyStat stat, Transform transform)
    {
        GameObject item = null;
        float rand = Random.Range(0, 100);
        Debug.Log($"rand dropItem : {rand}");
        if (rand < 1 || stat.MonsterType == Define.MonsterType.middleBoss)
        {
            item = Managers.Resource.Instantiate("Content/Box");
        }
        else if (rand < 4)
        {
            int rd = Random.Range(1, 11);
            if (rd < 6)
            {
                item = Managers.Resource.Instantiate("Content/Health");
            }
            else
            {
                item = Managers.Resource.Instantiate("Content/Magnet");
            }
        }
        if (item == null)
            return;
        item.transform.position = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
    }

    public Define.Weapons SetRandomWeaponInItem()
    {
        int weaponNum = Random.Range(1, System.Enum.GetValues(typeof(Define.Weapons)).Length+1 - System.Enum.GetValues(typeof(Define.PlayerStartWeapon)).Length);
        Define.Weapons playerWeapon = (Define.Weapons)weaponNum;
        return playerWeapon;
    }

    public void LevelUpEvent()
    {
        if (Managers.Game.getPlayer().GetComponent<PlayerStat>().Level < 35)
        {
            Managers.UI.ShowPopupUI<UI_LevelUp>();
            Managers.GamePause();
        }
    }

    public void LevelUpOverEvent(int itemType, string itemName)
    {
        //PlayerStatorWeaponUp
        PlayerStat player = Managers.Game.getPlayer().GetComponent<PlayerStat>();
        if (itemType == 1)
        {
            switch (itemName)
            {
                case "MaxHP":
                    player.MaxHP += 10;
                    player.HP = player.MaxHP;
                    break;
                case "MoveSpeed":
                    player.MoveSpeed += 0.5f;
                    break;
                case "Damage":
                    player.Damage += 10;
                    break;
                case "Defense":
                    player.Defense += 1;
                    break;
                case "Cooldown":
                    player.Cooldown += 10;
                    break;
                case "Amount":
                    player.Amount += 1;
                    break;
            }
            player.AddOrSetWeaponDict(player.playerStartWeapon, 0);
        }
            
        else
        {
            Define.Weapons weaponType = (Define.Weapons)System.Enum.Parse(typeof(Define.Weapons), itemName);
            player.AddOrSetWeaponDict(weaponType, 1);
        }
            

        Managers.UI.ClosePopupUI(Define.PopupUIGroup.UI_LevelUp);
    }

    public void ShowItemBoxUI()
    {
        Managers.UI.ShowPopupUI<UI_ItemBoxOpen>();
        Managers.GamePause();
    }
    public List<Define.Weapons> SetRandomWeaponfromItemBox(PlayerStat player)
    {
        bool weaponFull = true;
        if (player.GetWeaponDict().Count >= 4)
        {
            foreach(KeyValuePair<Define.Weapons, int> weapon in player.GetWeaponDict())
            {
                if (weapon.Key == player.playerStartWeapon)
                    continue;
                if(weapon.Value < maxWeaponLevel)
                {
                    weaponFull = false;
                    break;
                }
            }
            if (weaponFull)
            {
                return null;
            }
        }
        int maxCount = 3;
        int rd = Random.Range(1, maxCount+1);
        int i = 0;
        List<Define.Weapons> weaponList = new List<Define.Weapons>();
        while(i < rd)
        {
            Define.Weapons wp = SetRandomWeaponInItem();
            int weaponlevel = player.GetWeaponDict().GetValueOrDefault<Define.Weapons, int>(wp);
            if (weaponlevel >= maxWeaponLevel || (player.GetWeaponDict().Count == 4 && weaponlevel == 0))
                continue;
            weaponList.Add(wp);
            i++;
        }

        return weaponList;
    }

    public void SetLevelUpWeaponfromItemBox(List<Define.Weapons> weaponList, PlayerStat player)
    {
        if(weaponList == null)
        {
            player.HP += 30;
            return;
        }
        foreach(Define.Weapons weaponType in weaponList)
        {
            player.AddOrSetWeaponDict(weaponType, 1);
        }
        
    }

    public void PlayHitEnemyEffectSound()
    {
        int rd = Random.Range(1, 3);
        switch (rd)
        {
            case 1:
                Managers.Sound.Play("Hit0");
                break;
            case 2:
                Managers.Sound.Play("Hit1");
                break;
        }
    }

    public void PlayHitPlayerEffectSound()
    {
        Managers.Sound.Play("Hit_01");
    }

}
