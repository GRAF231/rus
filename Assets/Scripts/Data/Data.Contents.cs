using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	#region Character

	[Serializable]
	public class Player
	{
		public int id;
		public string name;
		public int weaponID;
		public int maxHp;
		public int damage;
		public int defense;
		public float moveSpeed;
		public int coolDown;
		public int amount;
	}

	[Serializable]
	public class PlayerData : ILoader<int, Player>
	{
		public List<Player> players = new List<Player>();

		public Dictionary<int, Player> MakeDict()
		{
			Dictionary<int, Player> dict = new Dictionary<int, Player>();
			foreach (Player player in players)
				dict.Add(player.id, player);
			return dict;
		}
	}

    #endregion

    #region Monster

	[Serializable]
	public class Monster
    {
		public int id;
		public string name;
		public int maxHp;
		public int damage;
		public int defense;
		public float moveSpeed;
		public int expMul;
	}

    public class MonsterData : ILoader<int, Monster>
    {
		public List<Monster> monsters = new List<Monster>();
        public Dictionary<int, Monster> MakeDict()
        {
			Dictionary<int, Monster> dict = new Dictionary<int, Monster>();
			foreach (Monster monster in monsters)
				dict.Add(monster.id, monster);
			return dict;
		}
    }


    #endregion
    #region Weapon

    [Serializable]
	public class WeaponData
    {
        public int weaponID;
        public string weaponName;
        public string weaponTitle;
        public string weaponDesc;
        public string weaponTitleEn;
        public string weaponDescEn;
        public string weaponTitleRu;
        public string weaponDescRu;
        public List<WeaponLevelData> weaponLevelData = new List<WeaponLevelData>();

	}

	[Serializable]
	public class WeaponLevelData
	{
		public int level;
		public int damage;
		public float movSpeed;
		public float force;
		public float cooldown;
		public float size;
		public int penetrate;
		public int countPerCreate;
	}

	[Serializable]
	public class WeaponDataLoader : ILoader<int, WeaponData>
	{
		public List<WeaponData> weapons = new List<WeaponData>();

        private WeaponData LocalizeWeapon(WeaponData data)
        {
            if (Managers.I18n.Lang == I18NManager.Language.en)
            {
                data.weaponTitle = data.weaponTitleEn;
                data.weaponDesc = data.weaponDescEn;
            }
            else if (Managers.I18n.Lang == I18NManager.Language.ru)
            {
                data.weaponTitle = data.weaponTitleRu;
                data.weaponDesc = data.weaponDescRu;
            }

            return data;
        }

        public Dictionary<int, WeaponData> MakeDict()
        {
            Dictionary<int, WeaponData> dict = new Dictionary<int, WeaponData>();
            foreach (WeaponData weapon in weapons)
                dict.Add(weapon.weaponID, LocalizeWeapon(weapon));
            return dict;
        }
    }
    #endregion


    [Serializable]
    public class PlayerStatData
    {
        public int statID;
        public string statName;
        public string statTitle;
        public string statDesc;
        public string statTitleEn;
        public string statDescEn;
        public string statTitleRu;
        public string statDescRu;
    }

    public class PlayerStatDataLoader : ILoader<int, PlayerStatData>
    {
        public List<PlayerStatData> playerStats = new List<PlayerStatData>();

		private PlayerStatData LocalizeStat(PlayerStatData data)
		{
			if (Managers.I18n.Lang == I18NManager.Language.en)
            {
                data.statTitle = data.statTitleEn;
                data.statDesc = data.statDescEn;
            }
			else if (Managers.I18n.Lang == I18NManager.Language.ru)
            {
                data.statTitle = data.statDescRu;
				data.statDesc = data.statDescRu;
            }

            return data;
		}

        public Dictionary<int, PlayerStatData> MakeDict()
        {
            Dictionary<int, PlayerStatData> dict = new Dictionary<int, PlayerStatData>();
			foreach (PlayerStatData item in playerStats)
			{
				dict.Add(item.statID, LocalizeStat(item));
			}
            return dict;
        }
    }
}