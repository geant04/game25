using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public GameObject viewModel;
    public List<GameObject> inventory;
    [HideInInspector] public GameObject weapon;

    private int index = 0;

    void Awake()
    {
        weapon = inventory[index];

        foreach(var weapon in inventory)
        {
            weapon.GetComponent<Gun>().Initialize();
        }

        ResetView();
    }

    void ResetView()
    {
        Transform parent = viewModel.transform.parent;
        Destroy(viewModel); // this is bad. This is bad. Don't do this. - Anthony
        viewModel = Instantiate(weapon.GetComponent<Gun>().viewModel, parent);
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
            ResetView();
        }

    }
}
