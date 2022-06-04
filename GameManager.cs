using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;
    public static HeroStatus gameHistory = new HeroStatus();
    private void Awake() 
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            Destroy(hud);
            Destroy(menu);
            return;
        }
        PlayerPrefs.DeleteAll();

        instance = this;

        gameHistory.History.Push(SaveState());
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // On Scene Loaded
    public void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        LoadState(gameHistory.History.Pop());
    }

    // Resurses
    public List<Sprite> playerSprites;
    public List<Sprite> enemiesSprites;
    public List<Sprite> bossSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrice;
    public List<int> xpTable;

    // References
    public Player player;
    public Weapon weapon;
    public FloatingTextManager floatingTextManager;
    public RectTransform hitpointBar;
    public Animator deathMenuAnim;
    public GameObject hud;
    public GameObject menu;

    
    //logic
    public int pesos;
    public int experience;
    public int skin;

    // Floating text
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    // Upgrade Weapon
    public bool TryUpgradeWeapon()
    {
        Debug.Log("Count: " + weaponPrice.Count);
        Debug.Log("level: " + weapon.weaponLevel);
        // is the weapon max level?
        if (weaponPrice.Count - 1 <= weapon.weaponLevel) 
            return false; 
       
        if (pesos >= weaponPrice[weapon.weaponLevel])
        {
            pesos -= weaponPrice[weapon.weaponLevel];

            weapon.UpgradeWeapon();
            return true;
        }

        return false;
    }

    // Hitpoin bar
    public void OnHitpointChange() 
    {
        float ratio = (float)player.hitPoint / (float)player.maxHitPoint;
        hitpointBar.localScale = new Vector3(ratio, 1 , 1);
    }

    // Experience System
    public int GetCurrentLevel() 
    {
        int r = 0;
        int add = 0;

        while (experience >= add)
        {
            add += xpTable[r];
            r++;

            if (r == xpTable.Count) //MAX level
                return r;
        }
        return r;
    }

    public int GetXpToLevel(int level) 
    {
        int r = 0;
        int xp = 0;

        while (r < level)
        {
            xp += xpTable[r];
            r++;
        }
        return xp;
    }

    public void GrantXp(int xp) 
    {
        int currentLevel = GetCurrentLevel();
        experience += xp;
        if (currentLevel < GetCurrentLevel())
        {
            OnLevelUp();
        }
    }

    public void OnLevelUp() 
    {
        player.OnLevelUp();
        OnHitpointChange();
    }

    // Death menu and Respawn
    public void Respawn() 
    {
        deathMenuAnim.SetTrigger("hide");
        SceneManager.LoadScene("Main");
        player.Respawn();
    }

    //Save state
    public Memento SaveState()
    {
        string s = "";

        s += skin.ToString() + "|" +
             pesos.ToString() + "|" +
             experience.ToString() + "|" +
             weapon.weaponLevel.ToString();

        PlayerPrefs.SetString("SaveState", s);
        Debug.Log("SaveState");

        Debug.Log(s);

        return new Memento(s);
    }

    public void LoadState(Memento memento)
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;

        if (!PlayerPrefs.HasKey("SaveState")) 
        {
            Debug.Log("NOOOOO SaveState");
            return; 
        }

        Debug.Log("LoadStateeeee");
        Debug.Log(memento.playerSkinLevel);

        skin = memento.playerSkinLevel;

        pesos = memento.pesos;

        experience = memento.experience;
        if (GetCurrentLevel() != 1)
            player.SetLevel(GetCurrentLevel());

        weapon.weaponLevel = memento.weaponLvl;

        Debug.Log("RESTORE");
    }
}
