using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public void DestroyObject()
    {
        Destroy(this.transform.parent.gameObject);
    }
}
