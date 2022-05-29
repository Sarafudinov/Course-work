using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{
    private SpriteRenderer spriteRenderer;
    private IHeroArmor heroArmor = new StandardNonArmor();

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();

        DontDestroyOnLoad(gameObject);
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        UpdateMotor(new Vector3(x, y, 0));
    }

    public void SwapSprite(int skinId) 
    {
        spriteRenderer.sprite = GameManager.instance.playerSprites[skinId]; 
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

}
