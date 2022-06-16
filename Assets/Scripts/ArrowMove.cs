using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMove : MonoBehaviour
{

    private Vector3 positionUp;
    private Vector3 positionDown;

    private float elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        ResetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        ResetPosition();
        elapsedTime += Time.deltaTime;
        transform.position = Vector3.Lerp(positionDown, positionUp, Mathf.PingPong(elapsedTime*2, 1));
    }

    private void ResetPosition()
    {
        positionUp = transform.parent.transform.position + new Vector3(0f, 1.2f, 0f);
        positionDown = transform.parent.transform.position + new Vector3(0f, 1f, 0f);
    }
}
