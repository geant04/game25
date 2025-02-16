using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    public GameObject detailBox;
    public float rotSpeed;
    public float amplitude;
    public float frequency;
    public float offset;

    private Vector3 ogPos;
    private float t = 0.0f;

    public virtual void BonusPerk(PlayerWeaponManager playerWeaponManager) { }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Entered");
        if (collider.transform.tag == "Player")
        {
            PlayerWeaponManager player = collider.transform.GetComponent<PlayerWeaponManager>();
            if (player)
            {
                BonusPerk(player);
                Destroy(gameObject);
            }
        }
    }

    private void Awake()
    {
        ogPos = detailBox.transform.position;
    }

    private void Update()
    {
        float dy = amplitude * Mathf.Sin(frequency * t);
        detailBox.transform.position = ogPos + transform.up * dy;
        detailBox.transform.Rotate(transform.up, rotSpeed * Time.deltaTime);
        t += Time.deltaTime;
    }
}
