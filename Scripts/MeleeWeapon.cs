using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Collidable, IWeapon
{
    // Damage struct
    public int[] damagePoint = { 1, 2, 3, 4, 5, 6 };
    public float[] pushForce = { 2.0f, 2.2f, 2.4f, 2.6f, 2.8f, 3.0f };

    // Upgrade
    public int weaponLevel = 0;
    private SpriteRenderer spriteRenderer;

    // Swing
    private Animator anim;
    private float cooldown = 0.5f;
    private float lastSwing;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // realisation strategy pattern

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.E))
        {
            SwingAttack swing = new SwingAttack();
            DelayCheck(swing);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            DirectAttack directAttack = new DirectAttack();
            DelayCheck(directAttack);
        }
    }

    private void DelayCheck(IAttackStrategy strategy) 
    {
        if (Time.time - lastSwing > cooldown)
        {
            lastSwing = Time.time;
            strategy.Attack(anim);
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "Fighter")
        {
            if (coll.name == "Player")
                return;

            // Create a new damage object, then we'll send it to the fighter we've hit
            Damage dmg = new Damage
            {
                damageAmount = damagePoint[weaponLevel],
                origin = transform.position,
                pushForse = pushForce[weaponLevel]
            };

            coll.SendMessage("ReceiveDamage", dmg);
            
        }
    }

    public void UpgradeWeapon() 
    {
        weaponLevel++;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
    }

    public void SetWeaponLevel(int level) 
    {
        weaponLevel = level;
        spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
    }

    public int GetWeaponLevel()
    {
        return weaponLevel;
    }
}


//strategy pattern
public interface IAttackStrategy
{
    void Attack(Animator anim);
}

class SwingAttack : IAttackStrategy
{
    public void Attack(Animator anim)
    {
        anim.SetTrigger("Swing");
    }
}

class DirectAttack : IAttackStrategy
{
    public void Attack(Animator anim)
    {
        anim.SetTrigger("DirectSwing");
    }
}