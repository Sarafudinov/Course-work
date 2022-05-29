using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// pattern factory

abstract class Creator
{
    public abstract IDungeon FactoryMethod();

    public string CreateMap()
    {
        var product = FactoryMethod();
        var result = product.SetSceneName();

        return result;
    }
}

class ConcreteDungeon1 : Creator
{
    public override IDungeon FactoryMethod()
    {
        return new Dungeon1();
    }
}

class ConcreteDungeon2 : Creator
{
    public override IDungeon FactoryMethod()
    {
        return new Dungeon2();
    }
}

public interface IDungeon
{
    string SetSceneName();
}

class Dungeon1 : IDungeon
{
    public string SetSceneName()
    {
        return "Dungeon_1";
    }
}

class Dungeon2 : IDungeon
{
    public string SetSceneName()
    {
        return "Dungeon_2";
    }
}


public class Portal : Collidable
{
    public string[] dungeons;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player")
        {

            Creator creator;
            

            int check = Random.Range(0, dungeons.Length);
            string sceneName = SceneManager.GetActiveScene().name;
               
            Debug.Log("TRY SAVE");
            
            GameManager.instance.SaveState();
        
            if (sceneName.Equals("Main"))
            {
                creator = new ConcreteDungeon1();
                sceneName = creator.CreateMap(); 
                SceneManager.LoadScene(sceneName);
            }
            else if (sceneName.Equals("Dungeon_1")) 
            {
                creator = new ConcreteDungeon2();
                sceneName = creator.CreateMap();
                SceneManager.LoadScene(sceneName);
            }
            else if (sceneName.Equals("Dungeon_2"))
            {

                sceneName = "Main";
                SceneManager.LoadScene(sceneName);
            }

        }
    }
}
