using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IWeapon
{
    public void UpgradeWeapon();
    public void SetWeaponLevel(int level);
    public int GetWeaponLevel();
}

