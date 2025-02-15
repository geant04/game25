using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public Gun weapon;
    public GameObject[] projectiles;
    public GameObject viewAnim;
    public List<Gun> inventory;
    
    private int index = 1;

    void Awake()
    {
        // 0 is rocket
        // 1 is machine gun

        inventory = new List<Gun>();
        inventory.Add(new RocketLauncher(projectiles[0], viewAnim));
        inventory.Add(new MachineGun(projectiles[1], viewAnim));
        weapon = inventory[index];
    }

    void ResetView(int idx)
    {
        viewAnim.transform.localPosition = inventory[index].localPos;
        // swap whatever models are stored at that point
    }

    // Update is called once per frame
    void Update()
    {
        int idxChange = 0;

        if (Input.GetKeyDown(KeyCode.Q)) idxChange = -1;
        if (Input.GetKeyDown(KeyCode.E)) idxChange = 1;

        if (idxChange != 0)
        {
            int temp = index;
            index = (index + idxChange) % inventory.Count;
            if (index < 0) index += inventory.Count;

            weapon = inventory[index];
            ResetView(temp);
        }

    }
}
