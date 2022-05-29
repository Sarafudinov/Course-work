using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    // Text fields
    public Text levelText, hitpointText, pesosText, upgradeCostText, xpText;

    // Logic
    private int currentCharacterSelection = 0;
    private int prevCharacterSelection = 0;
    public Image characterSelectionSprite;
    public Image weaponSprite;
    public RectTransform xpBar;
    private IHeroArmor heroArmor;
    static private bool flag = false;

    // Character Selection
    public void OnArrowClick(bool right) 
    {
        OnSelectionChanged(); 
    }

    private void OnSelectionChanged() 
    {
        int currLevel = GameManager.instance.GetCurrentLevel();
        
        // sprite move in game
        if (currLevel == 1)
        {
            currentCharacterSelection = 1;
        }
        if (currLevel == 2)
        {
            currentCharacterSelection = 2;
        }
        if (currLevel <= 5 && currLevel > 2)    // [ 3, 4, 5]
        {
            currentCharacterSelection = 3;
        }
        if (currLevel <= 8 && currLevel > 5)    // [ 6, 7, 8]
        {
            currentCharacterSelection = 4;
        }
        if (currLevel <= 10 && currLevel > 8)    // [ 9, 10 ]
        {
            currentCharacterSelection = 5;
        }

        if (currentCharacterSelection - prevCharacterSelection == 0) return;
        else prevCharacterSelection = currentCharacterSelection;

        switch (currentCharacterSelection)
        {
            case 1:
                heroArmor = new LeatherArmor(GameManager.instance.player.GetArmor());
                GameManager.instance.player.hitPoint += 2;
                break;
            case 2:
                heroArmor = new YellowFeatherKnight(GameManager.instance.player.GetArmor());
                GameManager.instance.player.hitPoint += 3;
                break;
            case 3:
                heroArmor = new GreenFeatherKnight(GameManager.instance.player.GetArmor());
                GameManager.instance.player.hitPoint += 4;
                GameManager.instance.weapon.damagePoint[GameManager.instance.weapon.weaponLevel] += 1;

                break;
            case 4:
                heroArmor = new BlueFeatherKnight(GameManager.instance.player.GetArmor());
                GameManager.instance.player.hitPoint += 5;
                GameManager.instance.weapon.damagePoint[GameManager.instance.weapon.weaponLevel] += 2;

                break;
            case 5:
                heroArmor = new GoldenKnight(GameManager.instance.player.GetArmor());
                GameManager.instance.player.hitPoint += 6;
                GameManager.instance.weapon.damagePoint[GameManager.instance.weapon.weaponLevel] += 3;

                break;
            default:
                break;
        }

        
        characterSelectionSprite.sprite = GameManager.instance.playerSprites[currentCharacterSelection];
        heroArmor.SetArmorSkin();
        GameManager.instance.player.SetArmor(heroArmor);
    }

    // Weapon upgrade
    public void OnUpgradeClick() 
    {
        if (GameManager.instance.TryUpgradeWeapon())
            UpdateMenu();
    }

    // Update the character information
    public void UpdateMenu()
    {
        // Weapon
        weaponSprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.weapon.weaponLevel];
        if (GameManager.instance.weapon.weaponLevel == GameManager.instance.weaponPrice.Count - 1)
            upgradeCostText.text = "MAX";
        else
            upgradeCostText.text = GameManager.instance.weaponPrice[GameManager.instance.weapon.weaponLevel].ToString();


        // Meta
        levelText.text = GameManager.instance.GetCurrentLevel().ToString();
        hitpointText.text = GameManager.instance.player.hitPoint.ToString() + "/" + GameManager.instance.player.maxHitPoint.ToString();
        pesosText.text = GameManager.instance.pesos.ToString();


        // xp Bar
        int currLevel = GameManager.instance.GetCurrentLevel();
        if (currLevel == GameManager.instance.xpTable.Count)
        {
            xpText.text = GameManager.instance.experience.ToString() + " total experience points";
            xpBar.localScale = Vector3.one;
        }
        else
        {
            int prevLevelXp = GameManager.instance.GetXpToLevel(currLevel - 1);
            int currLevelXp = GameManager.instance.GetXpToLevel(currLevel);

            int diff = currLevelXp - prevLevelXp;
            int currXpIntoLevel = GameManager.instance.experience - prevLevelXp;

            float completionRatio = (float)currXpIntoLevel / (float)diff;

            xpBar.localScale = new Vector3(completionRatio, 1, 1);
            xpText.text = currXpIntoLevel.ToString() + " / " + diff;
        }
    }

}

// Decorater pattern

public abstract class IHeroArmor
{
    public abstract void SetArmorSkin();
}

public class StandardNonArmor : IHeroArmor
{
    public override void SetArmorSkin()
    {
        GameManager.instance.player.SwapSprite(0);
    }
}

public abstract class Decorator : IHeroArmor
{
    protected IHeroArmor heroArmor;

    public Decorator(IHeroArmor heroArmor)
    {
        this.heroArmor = heroArmor;
    }

    public void SetHeroArmor(IHeroArmor heroArmor)
    {
        this.heroArmor = heroArmor;
    }

    public override void SetArmorSkin()
    {
        this.heroArmor.SetArmorSkin();
    }
}

public class LeatherArmor : Decorator
{

    public LeatherArmor(IHeroArmor heroArmor) : base(heroArmor)
    {
    }

    public override void SetArmorSkin()
    {
        base.SetArmorSkin();
        GameManager.instance.player.SwapSprite(1);
        
    }
}

public class YellowFeatherKnight : Decorator
{

    public YellowFeatherKnight(IHeroArmor heroArmor) : base(heroArmor)
    {
    }

    public override void SetArmorSkin()
    {
        base.SetArmorSkin();
        GameManager.instance.player.SwapSprite(2);
    }
}

public class GreenFeatherKnight : Decorator
{

    public GreenFeatherKnight(IHeroArmor heroArmor) : base(heroArmor)
    {
    }

    public override void SetArmorSkin()
    {
        base.SetArmorSkin();
        GameManager.instance.player.SwapSprite(3);
    }
}

public class BlueFeatherKnight : Decorator
{

    public BlueFeatherKnight(IHeroArmor heroArmor) : base(heroArmor)
    {
    }

    public override void SetArmorSkin()
    {
        base.SetArmorSkin();
        GameManager.instance.player.SwapSprite(4);
    }
}

public class GoldenKnight : Decorator
{

    public GoldenKnight(IHeroArmor heroArmor) : base(heroArmor)
    {
    }

    public override void SetArmorSkin()
    {
        base.SetArmorSkin();
        GameManager.instance.player.SwapSprite(5);
    }
}

