using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonController : WeaponController
{

    bool _isCool = false;
    private float _deg = 0;

    public override int _weaponType { get { return (int)Define.Weapons.Poison; } }

    private void Update()
    {
        if (!_isCool)
        {
            StartCoroutine(DamageCoolTime());
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll((Vector2)transform.position, _size, LayerMask.GetMask("Enemy"));
            foreach(Collider2D coll in collider2Ds)
            {
                GameObject go = coll.gameObject;
                go.GetComponent<BaseController>().OnDamaged(_damage, _force);
            }
        }

    }

    protected override void SetWeaponStat()
    {
        base.SetWeaponStat();
        Transform DamageZonEffect = transform.GetChild(0);
        DamageZonEffect.localScale = Vector3.one * _size * 2;
    }

    IEnumerator DamageCoolTime()
    {
        _isCool = true;
        yield return new WaitForSeconds(_cooldown);
        _isCool = false;
    }


    void FixedUpdate()
    {
        _deg += Time.deltaTime;
        if (_deg < 360)
        {
            transform.rotation = Quaternion.Euler(0, 0, _deg * 50f);
        }
        else
        {
            _deg = 0;
        }
    }
}
