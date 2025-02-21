using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Button : MonoBehaviour
{
    public KeyCode keyCode;
    public float inset;
    public bool isReusable;
    public float cooldown;
    public SoundData soundFx;
    public Player player;

    private Vector3 localPosition;
    private SphereCollider sphereCollider;
    private bool active;

    public virtual void Activate()
    {
        Debug.Log("Boop");
        return;
    }

    public virtual void InsertButton(bool BackOrIn)
    {
        transform.localPosition = BackOrIn ? localPosition : transform.forward * -inset;
    }

    private void Awake()
    {
        active = true;
        localPosition = transform.localPosition;
        sphereCollider = GetComponent<SphereCollider>();
    }

    private IEnumerator Reactivate(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        active = true;
        InsertButton(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            player = player ? player : other.GetComponent<Player>();
            float dot = Vector3.Dot(player.viewAnim.transform.forward, transform.forward);

            if (dot > 0 || !active) return;

            // activate GUI popup code here
            if (Input.GetKeyDown(keyCode))
            {
                Debug.Log("Activated!");
                Activate();
                SoundManager.Instance.CreateSound()
                    .WithSoundData(soundFx)
                    .WithPosition(transform.position)
                    .Play();
                active = false;
                InsertButton(false);
                if (isReusable) StartCoroutine(Reactivate(cooldown));
            }
        }
    }
}
