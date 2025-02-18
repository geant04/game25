using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCrate : Pickups
{
    public SoundData ammoPickup;

    public override bool BonusPerk(PlayerWeaponManager playerWeaponManager) 
    {
        // Refill weapons
        int totalRefilled = 0;

        foreach (GameObject gunObj in playerWeaponManager.inventory)
        {
            Gun gun = gunObj.GetComponent<Gun>();
            if (gun)
            {
                int delta = (int) Mathf.Min(gun.maxAmmo / 2.5f, gun.maxAmmo - gun.ammo);
                totalRefilled += delta;
                gun.RefillAmmo(delta);
            }
        }

        if (totalRefilled > 0)
        {
            SoundManager.Instance.CreateSound()
                .WithSoundData(ammoPickup)
                .WithPosition(transform.position)
                .Play();
            return true;
        }

        return false;
    }
}
