using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : Enemy
{
    public float[] fireballSpeed = { 2.5f, -2.5f };
    public float distance = 0.25f;
    public Transform[] fireballs;
    private string sceneName;

    protected override void Start()
    {
        base.Start();
        sceneName = SceneManager.GetActiveScene().name;
        BossParam(sceneName);
    }

    private void Update()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            fireballs[i].position = transform.position + new Vector3(-Mathf.Cos(Time.time * fireballSpeed[i]) * distance, // x direction 
                                                                  Mathf.Sin(Time.time * fireballSpeed[i]) * distance, // y direction
                                                                  0);
        }
    }

    private void BossParam(string sceneName) 
    {
        switch (sceneName)
        {
            case "Dungeon_1":
                GetComponent<SpriteRenderer>().sprite = GameManager.instance.bossSprites[0];
                hitPoint = 25;
                maxHitPoint = 25;
                xpValue = 25;
                                
                break;
            case "Dungeon_2":
                GetComponent<SpriteRenderer>().sprite = GameManager.instance.bossSprites[1];
                hitPoint = 50;
                maxHitPoint = 50;
                xpValue = 50;
                break;
        }
    }
}
