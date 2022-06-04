using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    // Public fields
    public int hitPoint = 10;
    public int maxHitPoint = 10;
    public float pushRecoverySpeed = 0.2f;

    // Immunity
    protected float immuneTime = 1.0f;
    protected float lastTime;

    // Push
    protected Vector3 pushDirection;

    // All fighters can ReceiveDamage / Die
    protected virtual void ReceiveDamage(Damage dmg) 
    {
        if (Time.time - lastTime > immuneTime)
        {
            lastTime = Time.time;
            hitPoint -= dmg.damageAmount;
            pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForse;

            GameManager.instance.ShowText(dmg.damageAmount.ToString(), 
                20, Color.red, transform.position, Vector3.up * 10, 0.5f);

            if (hitPoint <= 0)
            {
                hitPoint = 0;
                Death();
            }
        }
    }

    protected virtual void Death() 
    {
    
    }
}
