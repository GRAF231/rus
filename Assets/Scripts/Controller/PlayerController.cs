using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : BaseController
{
    protected PlayerStat _stat;
    public RuntimeAnimatorController animeCon;
    public Sprite[] playerSprites;
    [SerializeField] Vector2 _inputVec;
    public SpriteRenderer _viewSpriteRenderer;
    [SerializeField] Animator _viewAnimator;
    [SerializeField] public Vector2 _lastDirVec = new Vector2(1, 0);
    bool _isDamaged = false;
    bool _isMobile = false;
    float _invincibility_time = 0.2f;
    Joystick _joystick;
    Slider _slider;

    protected override void Init()
    {
        _stat = GetComponent<PlayerStat>();
        _rigid = GetComponent<Rigidbody2D>();
        _type = Define.WorldObject.Player;
        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform, "UI_HPBar");

        _isMobile = Util.isMobile();

        if (_isMobile)
        {
            _joystick = GameObject.FindWithTag("Joystick").GetComponent<Joystick>();
        }
    }
    

    void Update()
    {
        if (_isMobile)
        {
            if (_joystick.Horizontal > 0.1f || _joystick.Horizontal < -0.1f)
            {
                _inputVec.x = _joystick.Horizontal;
            }
            else
            {
                _inputVec.x = Input.GetAxisRaw("Horizontal");
            }

            if (_joystick.Vertical > 0.1f || _joystick.Vertical < -0.1f)
            {
                _inputVec.y = _joystick.Vertical;
            }
            else
            {
                _inputVec.y = Input.GetAxisRaw("Vertical");
            }
        }
        else
        {
            _inputVec.x = Input.GetAxisRaw("Horizontal");
            _inputVec.y = Input.GetAxisRaw("Vertical");
        }

    }

    private void FixedUpdate()
    {
        Vector2 nextVec = _inputVec.normalized * (_stat.MoveSpeed * Time.fixedDeltaTime);
        //Position change the player to last direction;
        _rigid.MovePosition(_rigid.position + nextVec);

        if (_inputVec.normalized.magnitude != 0)
        {
            _lastDirVec = _inputVec.normalized;
        }
    }

    private void LateUpdate()
    {
        _viewAnimator.SetFloat("speed", _inputVec.magnitude);
 
    }
    public void DirectionFlipX()
    {
        if (_inputVec.x != 0)
        {
            _viewSpriteRenderer.flipX = _inputVec.x >= 0;
        }
    }


    public void Init(Data.Player playerData)
    {
        _viewSpriteRenderer.sprite = playerSprites[playerData.id - 1];
        _viewAnimator.runtimeAnimatorController = animeCon;
        _stat.MaxHP = playerData.maxHp;
        _stat.HP = playerData.maxHp;
        _stat.Damage = playerData.damage;
        _stat.Defense = playerData.defense;
        _stat.MoveSpeed = playerData.moveSpeed;
        _stat.Cooldown = playerData.coolDown;
        _stat.Amount = playerData.amount;
        _stat.AddOrSetWeaponDict((Define.Weapons)playerData.weaponID, 1);
    }
    public void OnDamaged(Collision2D collision)
    {
        _isDamaged = true;
        Stat EnemyStat = collision.transform.GetComponent<EnemyStat>();


        _stat.HP -= Mathf.Max(EnemyStat.Damage - _stat.Defense, 1);
        
        if (_stat.HP <= 0)
            OnDead();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            float currentTime = Managers.GameTime;
            if (!_isDamaged)
            {
                OnDamaged(collision);
                StartCoroutine(OnDamagedColor());
            }
        }
    }

    public override void OnDamaged(int damage, float force = 0)
    {
        Managers.Event.PlayHitPlayerEffectSound();
        _stat.HP -= Mathf.Max(damage - _stat.Defense, 1);
        OnDead();
    }

    IEnumerator OnDamagedColor()
    {
        _viewSpriteRenderer.color = Color.red;

        yield return new WaitForSeconds(_invincibility_time);

        _isDamaged = false;
        _viewSpriteRenderer.color = Color.white;
    }


    public override void OnDead()
    {
        if(_stat.HP < 0)
        {
            _viewAnimator.SetTrigger("dead");
            _stat.HP = 0;
            Managers.UI.ShowPopupUI<UI_GameOver>();
            Managers.GamePause();
        }
            
    }
    

}