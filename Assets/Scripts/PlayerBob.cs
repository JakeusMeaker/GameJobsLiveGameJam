using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBob : MonoBehaviour
{
    private Vector3 pos;

    public Vector3 upPos, downPos;

    public int delay;

    void Start()
    {
        pos = transform.position;
        StartCoroutine(Bob());
    }

    IEnumerator Bob()
    {
        yield return new WaitForSeconds(delay);

        jump:

        transform.position = new Vector3(upPos.x, upPos.y, upPos.z);
        yield return new WaitForSeconds(1);
        transform.position = new Vector3(pos.x, pos.y, pos.z);
        yield return new WaitForSeconds(1);
        transform.position = new Vector3(downPos.x, downPos.y, downPos.z);
        yield return new WaitForSeconds(1);
        transform.position = new Vector3(pos.x, pos.y, pos.z);
        yield return new WaitForSeconds(1);

        goto jump;
    }
}
