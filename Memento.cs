public class Memento
{
    public string[] s;
    public int pesos;
    public int experience;
    public int weaponLvl;
    public int playerSkinLevel = 0;

    public Memento(string s)
    {
        this.s = s.Split('|');
        this.playerSkinLevel = int.Parse(this.s[0]);
        this.pesos = int.Parse(this.s[1]);
        this.experience = int.Parse(this.s[2]);
        this.weaponLvl = int.Parse(this.s[3]);
    }
}

