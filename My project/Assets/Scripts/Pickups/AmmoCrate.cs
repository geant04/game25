using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCrate : Pickups
{
    public override void BonusPerk(PlayerWeaponManager playerWeaponManager) 
    {
        // Refill weapons
        foreach (GameObject gunObj in playerWeaponManager.inventory)
        {
            Gun gun = gunObj.GetComponent<Gun>();
            if (gun)
            {
                gun.RefillAmmo(gun.maxAmmo / 3);
            }
        }
    }
}
