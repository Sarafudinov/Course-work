using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake() 
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            return;
        }
        PlayerPrefs.DeleteAll();

        instance = this;
        SceneManager.sceneLoaded += LoadState;
        Debug.Log("Load");
        DontDestroyOnLoad(gameObject);
    }

    // Resurses
    public List<Sprite> playerSprites;
    public List<Sprite> enemiesSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrice;
    public List<int> xpTable;

    // References
    public Player player;
    public IWeapon iweapon;
    public MeleeWeapon weapon;
    public FloatingTextManager floatingTextManager;

    //logic
    public int pesos;
    public int experience;

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
    }

    //Save state
    /*
    int preferedSkin
    int pesos
    int experience
    int weaponLevel
    */
    public void SaveState()
    {
        string s = "";

        s += "0" + "|" +
             pesos.ToString() + "|" +
             experience.ToString() + "|" +
             weapon.weaponLevel.ToString();

        PlayerPrefs.SetString("SaveState", s);

        Debug.Log("SaveState");
    }

    public void LoadState(Scene s, LoadSceneMode mode)
    {
        Debug.Log("LoadState");


        if (!PlayerPrefs.HasKey("SaveState")) 
        {
            Debug.Log("NOOOOO SaveState");
            return; 
        }

        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        // Change player skin
        pesos = int.Parse(data[1]);

        // Experience
        experience = int.Parse(data[2]);
        if (GetCurrentLevel() != 1)
            player.SetLevel(GetCurrentLevel());

        //change the weapon level
        weapon.SetWeaponLevel(int.Parse(data[3]));

        Debug.Log(GameObject.Find("SpawnPoint").transform.position);
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;

    }
}