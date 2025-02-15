using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [HideInInspector] public GameObject weapon;
    public GameObject viewAnim;
    public List<GameObject> inventory;
    
    private int index = 0;

    void Awake()
    {
        // 0 is rocket
        // 1 is machine gun
        weapon = inventory[index];

        foreach(var weapon in inventory)
        {
            weapon.GetComponent<Gun>().Initialize();
        }
    }

    void ResetView(int idx)
    {
        // viewAnim.transform.localPosition = inventory[index].GetComponent<Weapon>().localPos;
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
