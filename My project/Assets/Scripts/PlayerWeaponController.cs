using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class PlayerWeaponController : MonoBehaviour
{
    // TO DO: if you want to abstract this further, just make it base class inherit from a weapon class
    public Gun weapon;
    public PlayerController playerController;
    public PlayerWeaponManager playerWeaponManager;
    public GameObject originPoint;
    public GameObject viewAnim;
    public GameObject flash;
    [SerializeField] private TextMeshProUGUI ammoText;

    void Awake()
    { }

    private void Update()
    {
        weapon = playerWeaponManager.weapon.GetComponent<Gun>();

        if (Input.GetKey(KeyCode.Mouse0))
        {
            bool fired = weapon.Attack(originPoint.transform.position, originPoint.transform.forward);
            if (fired)
            {
                weapon.Animate(viewAnim, this);
                GameObject fx = Instantiate(flash, originPoint.transform.position, Quaternion.LookRotation(originPoint.transform.forward));
                fx.transform.Rotate(transform.forward, Random.Range(0.0f, 360.0f));
                Destroy(fx, 0.1f); // do a pool system honestly
            }
        }
            if (Input.GetKeyDown(KeyCode.R))
        {
            weapon.Reload();
        }

        ammoText.SetText($"{weapon.ammo} / {weapon.maxAmmo}");
        Debug.DrawRay(originPoint.transform.position, originPoint.transform.forward, Color.cyan);
        weapon.GunUpdate();
    }   
}

