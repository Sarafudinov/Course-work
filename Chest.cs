using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    public Sprite emptyChest;
    public int pesosAmount;

    protected override void Start()
    {
        base.Start();
        pesosAmount = (int)Random.Range(50, 100);
    }

    protected override void OnCollect()
    {
        if (!collected)
        {
            collected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            GameManager.instance.pesos += pesosAmount;
            GameManager.instance.ShowText("+" + pesosAmount + " pesos!",
                35, Color.yellow, transform.position, Vector3.up * 50, 1.0f);
        }
    }
}
