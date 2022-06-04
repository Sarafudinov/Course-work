using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mover
{
    // Experience
    public int xpValue = 1;

    // Logic
    public float triggerLenght = 1;
    public float chaseLenght = 5;
    private bool chasing;
    private bool collidingWithPlayer;
    private Transform playerTransform;
    private Vector3 startingPosition;

    // Hitbox
    public ContactFilter2D filter;
    private BoxCollider2D hitBox;
    private Collider2D[] hits = new Collider2D[10];
   
    protected override void Start()
    {
        base.Start();
        playerTransform = GameManager.instance.player.transform;
        startingPosition = transform.position;
        hitBox = transform.GetChild(0).GetComponent<BoxCollider2D>();

        Director director = new Director();
        var buildOrcPet = new ConcreteOrcPet();
        var buildOrcWarrior = new ConcreteOrcWarrior();
        var buildTrollWarrior = new ConcreteTrollWarrior();


        switch ((int)Random.Range(0, 2))
        {
            case 0:
                director.constructOrcPet(buildOrcPet);

                hitPoint = buildOrcPet.GetEnemy().GetHp();
                xpValue = buildOrcPet.GetEnemy().GetXp();
                GetComponent<SpriteRenderer>().sprite = GameManager.instance.enemiesSprites[0];

                break;
            case 1:
                director.constructOrcWarrior(buildOrcWarrior);

                hitPoint = buildOrcWarrior.GetEnemy().GetHp();
                xpValue = buildOrcWarrior.GetEnemy().GetXp();
                GetComponent<SpriteRenderer>().sprite = GameManager.instance.enemiesSprites[1];

                break;
            case 2:
                director.constructTrollWarrior(buildTrollWarrior);

                hitPoint = buildTrollWarrior.GetEnemy().GetHp();
                xpValue = buildTrollWarrior.GetEnemy().GetXp();
                GetComponent<SpriteRenderer>().sprite = GameManager.instance.enemiesSprites[2];

                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        // Is the player in range?
        if (Vector3.Distance(playerTransform.position, startingPosition) < chaseLenght)
        {
            if (Vector3.Distance(playerTransform.position, startingPosition) < triggerLenght)
            {
                chasing = true;
            }

            if (chasing)
            {
                if (!collidingWithPlayer)
                {
                    UpdateMotor((playerTransform.position - transform.position).normalized);
                }
            }
            else
            {
                UpdateMotor(startingPosition - transform.position);
            }
        }
        else
        {
            UpdateMotor(startingPosition - transform.position);
            chasing = false;
        }

        // Check for overlaps
        collidingWithPlayer = false;
        // Collision work
        boxCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
                continue;

            if (hits[i].tag == "Fighter" && hits[i].name == "Player")
            {
                collidingWithPlayer = true;
            }

            // cleanid array ourself
            hits[i] = null;

        }
    }

    protected override void Death()
    {

        Destroy(gameObject);
        GameManager.instance.GrantXp(xpValue);
        GameManager.instance.ShowText("+" + xpValue + " xp", 30,
            Color.magenta, transform.position, Vector3.up * 40, 1.0f);
    }
}

//************************************************************************************************************//
// Builder Pattern
// Builder interface IBuilder
public interface IBuilderEnemy
{
    void SetHp(int hp);

    void SetXp(int xp);

}

// ConcretBuilder Orc pet
public class ConcreteOrcPet : IBuilderEnemy
{
    private int hp;
    private int xp;

    public ProductEnemy GetEnemy()
    {
        return new ProductEnemy(hp, xp);
    }

    public void SetHp(int hp)
    {
        this.hp = hp + 1;
    }

    public void SetXp(int xp)
    {
        this.xp = xp;
    }
}

// ConcretBuilder Orc Warrior
public class ConcreteOrcWarrior : IBuilderEnemy
{
    private int hp;
    private int xp;
    public ProductEnemy GetEnemy()
    {
        return new ProductEnemy(hp, xp);
    }

    public void SetHp(int hp)
    {
        this.hp = hp + 2;
    }

    public void SetXp(int xp)
    {
        this.xp = xp;
    }
}

// ConcretBuilder Orc Warrior
public class ConcreteTrollWarrior : IBuilderEnemy
{
    private int hp;
    private int xp;

    public ProductEnemy GetEnemy()
    {
        return new ProductEnemy(hp, xp);
    }

    public void SetHp(int hp)
    {
        this.hp = hp + 3;
    }

    public void SetXp(int xp)
    {
        this.xp = xp;
    }
}

public class ProductEnemy
{
    private int hp;
    private int xp;
    public ProductEnemy() { }

    public ProductEnemy(int expirience, int hitpoint) 
    {
        this.hp = hitpoint;
        this.xp = expirience;
    }

    public int GetHp()
    {
        return this.hp;
    }

    public int GetXp()
    {
        return this.xp;
    }
}

// Director

public class Director 
{
    public void constructOrcPet(IBuilderEnemy builder) 
    {
        builder.SetHp((int)Random.Range(5, 10));
        builder.SetXp((int)Random.Range(1, 5));
    }
    public void constructOrcWarrior(IBuilderEnemy builder)
    {
        builder.SetHp((int)Random.Range(10, 15));
        builder.SetXp((int)Random.Range(5, 10));
    }
    public void constructTrollWarrior(IBuilderEnemy builder)
    {
        builder.SetHp((int)Random.Range(15, 40));
        builder.SetXp((int)Random.Range(10, 20));
    }
}

