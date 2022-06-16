using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseAudioManager : MonoBehaviour
{
    public GameObject soundGO;
    public GameObject musicGO;
    
    public bool hasSound = true;
    public bool hasMusic = true;

    public void SoundOnOff()
    {
        Sprite[] allSprites = Resources.LoadAll<Sprite>("Assets/GA/UX&UI/bouton_son (1)");
        SpriteState newSpriteState = new SpriteState();
        if(hasSound)
        {
            newSpriteState.pressedSprite = allSprites[3];
            soundGO.GetComponent<Button>().spriteState = newSpriteState;
            soundGO.GetComponent<Image>().sprite = allSprites[2];
            hasSound = false;
        }
        else
        {
            newSpriteState.pressedSprite = allSprites[1];
            soundGO.GetComponent<Button>().spriteState = newSpriteState;
            soundGO.GetComponent<Image>().sprite = allSprites[0];
            hasSound = true;
        }
    }

    public void MusicOnOff()
    {
        Sprite[] allSprites = Resources.LoadAll<Sprite>("Assets/GA/UX&UI/bouton_musique (1)");
        SpriteState newSpriteState = new SpriteState();
        if(hasMusic)
        {
            newSpriteState.pressedSprite = allSprites[3];
            musicGO.GetComponent<Button>().spriteState = newSpriteState;
            musicGO.GetComponent<Image>().sprite = allSprites[2];
            hasMusic = false;
        }
        else
        {
            newSpriteState.pressedSprite = allSprites[1];
            musicGO.GetComponent<Button>().spriteState = newSpriteState;
            musicGO.GetComponent<Image>().sprite = allSprites[0];
            hasMusic = true;
        }
    }
}
