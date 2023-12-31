using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LightningController : WeaponController
{

    bool _isAttack = false;
    private GameObject _playerUI = null;

    private PlayerController _playerController;
    private Vector2 lastEnemyPos;
    private List<GameObject> nearbyEnemies = new List<GameObject>();

    public override int _weaponType { get { return (int)Define.Weapons.Lightning; } }

    void Start()
    {
        _playerController = _player.GetComponent<PlayerController>();
    }

    protected override void SetWeaponStat()
    {
        base.SetWeaponStat();
    }

    void Update()
    {
         if (!_isAttack)
            {
                StartCoroutine(DamageCoolTime());
                StartCoroutine(LightnigEffect());

                //Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(Camera.main.ScreenToWorldPoint(Managers.Game.MousePos), _size, LayerMask.GetMask("Enemy"));
                Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(GetNearbyEnemyPos(), _size, LayerMask.GetMask("Enemy"));

                foreach (Collider2D coll in collider2Ds)
                {
                    GameObject go = coll.gameObject;
                    go.GetComponent<BaseController>().OnDamaged(_damage, _force);
                }
        }
    }

    bool IsEnemiesInRange()
    {
        return nearbyEnemies.Count() > 0;
    }

    Vector2 GetNearbyEnemyPos()
    {
        if (nearbyEnemies.Count() == 0)
        {
            //_playerController.DirectionFlipX();
            return _player.transform.position;
        }
        //Sort Enemies by distance to Weapon
        nearbyEnemies = nearbyEnemies.OrderBy(
            x => Vector2.Distance(this.transform.position, x.transform.position))
            .ToList();
        _playerController._viewSpriteRenderer.flipX = nearbyEnemies[0].transform.position.x > _player.transform.position.x;
        return nearbyEnemies[0].transform.position;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<EnemyStat>() && !nearbyEnemies.Contains(other.gameObject))
        {
            nearbyEnemies.Add(other.gameObject);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        nearbyEnemies.Remove(other.gameObject);
    }


    IEnumerator DamageCoolTime()
    {
        _isAttack = true;
        float currentCooltime = _cooldown;
        while (currentCooltime > 0f)
        {
            currentCooltime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();

        }
        _isAttack = false;
    }
    IEnumerator LightnigEffect()
    {
        GameObject lightnigEffect = Managers.Game.Spawn(Define.WorldObject.Unknown, "Weapon/Lightning");

        //lightnigEffect.transform.position = Camera.main.ScreenToWorldPoint(Managers.Game.MousePos) - new Vector3(0,0, Camera.main.transform.position.z);
        lightnigEffect.transform.position = GetNearbyEnemyPos();

        yield return new WaitForSeconds(0.5f);
        Managers.Resource.Destroy(lightnigEffect);
    }
}
