using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseAudioManager : MonoBehaviour
{
    public GameObject soundGO;
    public GameObject musicGO;
    public AudioMixer music;
    public AudioMixer sfx;

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
            sfx.SetFloat("volume", -80f);
        }
        else
        {
            newSpriteState.pressedSprite = allSprites[1];
            soundGO.GetComponent<Button>().spriteState = newSpriteState;
            soundGO.GetComponent<Image>().sprite = allSprites[0];
            hasSound = true;
            sfx.SetFloat("volume", 0f);
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
            music.SetFloat("volume", -80f);
        }
        else
        {
            newSpriteState.pressedSprite = allSprites[1];
            musicGO.GetComponent<Button>().spriteState = newSpriteState;
            musicGO.GetComponent<Image>().sprite = allSprites[0];
            hasMusic = true;
            music.SetFloat("volume", 0f);
        }
    }
}
