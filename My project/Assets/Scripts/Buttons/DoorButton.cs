using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : Button
{
    public GameObject door;
    public Vector3 localOffset;
    public float closingTime;

    private Vector3 localPos;

    public void Start()
    {
        if (door == null) return;
        localPos = door.transform.localPosition;
    }

    public override void Activate()
    {
        player.GetComponent<PlayerUIManager>().InsertToCenterBox("A DOOR HAS BEEN UNLOCKED", 1.0f);
        StartCoroutine(LowerDoor(closingTime));
    }

    private IEnumerator LowerDoor(float duration)
    {
        float time = 0;
        while (time < duration)
        {
            door.transform.localPosition = localPos + Vector3.Lerp(Vector3.zero, localOffset, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
}
