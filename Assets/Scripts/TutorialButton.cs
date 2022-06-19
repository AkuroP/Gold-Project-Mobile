using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialButton : MonoBehaviour
{
    public void NextImage()
    {
        this.gameObject.SetActive(false);
    }

    public void FinalImageAndLauchGame()
    {
        RuneManager.instanceRM.currentWeapon = WeaponType.DAGGER;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void FinalImageAndClose()
    {
        int children = transform.parent.childCount;
        for (int i = 0; i < children; ++i)
            transform.parent.GetChild(i).gameObject.SetActive(true);
        this.transform.parent.gameObject.SetActive(false);
    }
}
