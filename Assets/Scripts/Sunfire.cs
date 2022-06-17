using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunfire : MonoBehaviour
{

    public IEnumerator RandomDisplayFire(int maxTime)
    {
        int random = Random.Range(0, maxTime);
        yield return new WaitForSeconds(random);
    }

    public void DestroyFire()
    {
        Destroy(this.gameObject);
    }
}
