using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class ShotgunController : WeaponController
{
    [SerializeField] private GameObject gunhole;

    public GameObject shotgun;

    private PlayerController _playerController;
    private List<GameObject> nearbyEnemies = new List<GameObject>();
    private Vector2 lastEnemyPos = new Vector2(0, 0); 

    private bool _isCool = false;
    private float _bulletTargetRange = 60f;

    Vector2 startScale;

    public override int _weaponType { get { return (int)Define.Weapons.Shotgun; } }

    void Start()
    {
        _playerController = _player.GetComponent<PlayerController>();
        startScale = shotgun.transform.localScale;
    }

    void Update()
    {
        if (!IsEnemiesInRange())
        {
            return;
        }
        shotgun.transform.rotation = Quaternion.Slerp(shotgun.transform.rotation,
            Quaternion.AngleAxis(SetAngleFromHandToNearbyEnemy(), Vector3.forward), 10f * Time.deltaTime);
        if (!_isCool)
        {
            StartCoroutine(ShotCoolTime());
        }
    }

    void SetBulletAngle()
    {
        float bulletAngle = _bulletTargetRange / (_countPerCreate+1);
        float angle = SetAngleFromHandToNearbyEnemy();
        float startBulletAngle = angle - (_bulletTargetRange/2);
        for (int i = 1; i <= _countPerCreate; i++)
        {
            CreateBullet(i, bulletAngle, startBulletAngle);
        }
        
    }

    void CreateBullet(int num, float bulletAngle, float startBulletAngle)
    {
        GameObject bullet = Managers.Game.Spawn(Define.WorldObject.Unknown, "Weapon/Bullet", gunhole.transform.position);
        //set damage, dir 
        Bullet bulletStat = bullet.GetOrAddComponent<Bullet>();
        float _ang = startBulletAngle + bulletAngle * num + Random.Range(-5f, 5f);
        Vector3 bulletDir = new Vector3(Mathf.Cos(_ang * Mathf.Deg2Rad), Mathf.Sin(_ang * Mathf.Deg2Rad), 0);
        bulletStat.SetBulletDir(bulletDir);
        bulletStat._damage = _damage;
        bulletStat._movSpeed = _movSpeed;
        bulletStat._penetrate = _penetrate;
        bulletStat._force = _force;
    }

    bool IsEnemiesInRange()
    {
        return nearbyEnemies.Count() > 0;
    }

    float SetAngleFromHandToCursor()
    {
        Vector3 dirVec = (Managers.Game.WorldMousePos - transform.position).normalized;
        return Mathf.Atan2(dirVec.y, dirVec.x) * Mathf.Rad2Deg;
    }

    float SetAngleFromHandToNearbyEnemy()
    {
        Vector3 dirVec;
        if (IsEnemiesInRange())
        {
            dirVec = GetNearbyEnemyPos();
        }
        else
        {
            dirVec = lastEnemyPos;
            _playerController.DirectionFlipX();
        }
        dirVec = (dirVec - transform.position).normalized;
        return Mathf.Atan2(dirVec.y, dirVec.x) * Mathf.Rad2Deg;
    }

    IEnumerator ShotCoolTime()
    {
        _isCool = true;
        Managers.Sound.Play("Shoot_02");
        SetBulletAngle();
        yield return new WaitForSeconds(_cooldown);

        _isCool = false;
    }

    void FlipX(bool dir)
    {
        if (dir)
        {
            shotgun.transform.localScale = startScale;
        }
        else
        {
            shotgun.transform.localScale = startScale * new Vector2(1, -1);
        }
        _playerController._viewSpriteRenderer.flipX = dir;
    }

    Vector2 GetNearbyEnemyPos()
    {
        //Sort Enemies by distance to Weapon
        nearbyEnemies = nearbyEnemies.OrderBy(
            x => Vector2.Distance(this.transform.position, x.transform.position))
            .ToList();

        lastEnemyPos = nearbyEnemies[0].transform.position;
        FlipX(lastEnemyPos.x > _player.transform.position.x);
        return lastEnemyPos;
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
}
