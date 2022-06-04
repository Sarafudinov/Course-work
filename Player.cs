using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{
    private SpriteRenderer spriteRenderer;
    private IHeroArmor heroArmor = new StandardNonArmor();
    private bool isAlive = true;
    

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void ReceiveDamage(Damage dmg)
    {
        if (!isAlive)
            return;

        base.ReceiveDamage(dmg);
        GameManager.instance.OnHitpointChange();
    }

    protected override void Death()
    {
        isAlive = false;
        GameManager.instance.deathMenuAnim.SetTrigger("show");
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        if (isAlive)
            UpdateMotor(new Vector3(x, y, 0));
        
    }

    public void SwapSprite(int skinId) 
    {
        spriteRenderer.sprite = GameManager.instance.playerSprites[skinId];
        GameManager.instance.skin = skinId;
    }

    public IHeroArmor GetArmor() { return heroArmor; }

    public void SetArmor(IHeroArmor heroArmor) { this.heroArmor = heroArmor; }

    public void OnLevelUp() 
    {
        maxHitPoint++;
        hitPoint = maxHitPoint;
        GameManager.instance.ShowText("LEVEL UP", 30, Color.yellow, GameManager.instance.player.transform.position, Vector3.up * 20, 1.5f);
    }

    public void SetLevel(int level) 
    {
        for (int i = 0; i < level; i++)
            OnLevelUp();
    }

    public void Heal(int healingAmount)
    {
        if (hitPoint == maxHitPoint)
            return;
        hitPoint += healingAmount;
        if (hitPoint > maxHitPoint)
            hitPoint = maxHitPoint;
        GameManager.instance.ShowText("+" + healingAmount.ToString() + "hp", 25, Color.green, transform.position, Vector3.up * 30, 1.0f);
        GameManager.instance.OnHitpointChange();

    }

    public void Respawn() 
    {
        Heal(maxHitPoint);
        isAlive = true;
        lastTime = Time.time;
        pushDirection = Vector3.zero;
    }
}
