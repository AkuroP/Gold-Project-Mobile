using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feedback : MonoBehaviour
{

    public SpriteRenderer sr;

    public float timeBeforeDestruction;
    public float fadeDuration;
    public float moveDuration;
    public float elapsedTime;
    public float fadeMultiplicator = 1;
    public float travelingTime;

    public Vector2 spawn;

    public bool fadeOut;
    public bool fadeIn;
    public bool isGoingUp;
    public bool isGrossying;
    public bool isGoingToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SelfDestruct());
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if(fadeOut)
        {
            sr.color = new Color(1f, 1f, 1f, Mathf.Lerp(1f, 0f, elapsedTime/(fadeDuration/fadeMultiplicator)));
        }
        if (isGoingUp)
        {
            transform.position = new Vector2(Mathf.Lerp(GameObject.FindWithTag("FeedbackPos").transform.position.x, GameObject.FindWithTag("FeedbackPos").transform.position.x, elapsedTime / travelingTime), Mathf.Lerp(GameObject.FindWithTag("FeedbackPos").transform.position.y, GameObject.FindWithTag("FeedbackPos").transform.position.y - 1f, elapsedTime / travelingTime));
        }
        if(fadeIn)
        {
            sr.color = new Color(1f, 1f, 1f, Mathf.Lerp(0f, 1f, elapsedTime / (fadeDuration / 4)));
            StartCoroutine(EndFadeOut());
        }
        if (isGrossying)
        {
            transform.localScale = new Vector3(Mathf.Lerp(0.6f, 0.8f, elapsedTime), Mathf.Lerp(0.6f, 0.8f, elapsedTime), 0f);
        }
        if (isGoingToPlayer)
        {
            transform.position = new Vector2(Mathf.Lerp(spawn.x, GameObject.FindWithTag("Player").transform.position.x, elapsedTime / travelingTime), Mathf.Lerp(spawn.y, GameObject.FindWithTag("Player").transform.position.y, elapsedTime / travelingTime));
        }
    }

    public void Init(bool _isEssence, int _howMuch, Vector2 _spawn)
    {
        spawn = _spawn;
        transform.position = spawn;
        if(_isEssence)
        {
            switch(_howMuch)
            {
                case 1:
                    sr.sprite = Resources.Load<Sprite>("Assets/Graphics/Feedback/1essence");
                    break;
                case 3:
                    sr.sprite = Resources.Load<Sprite>("Assets/Graphics/Feedback/3essence");
                    break;
            }
        }
        else
        {
            switch (_howMuch)
            {
                case 0:
                    sr.sprite = Resources.Load<Sprite>("Assets/Graphics/Feedback/0heart");
                    break;
                case 1:
                    sr.sprite = Resources.Load<Sprite>("Assets/Graphics/Feedback/1heart");
                    break;
                case 2:
                    sr.sprite = Resources.Load<Sprite>("Assets/Graphics/Feedback/2heart");
                    break;
                case 3:
                    sr.sprite = Resources.Load<Sprite>("Assets/Graphics/Feedback/3heart");
                    break;
                case 4:
                    sr.sprite = Resources.Load<Sprite>("Assets/Graphics/Feedback/4heart");
                    break;
            }
            sr.color = new Color(1f, 1f, 1f, 0f);
            isGrossying = true;
            isGoingUp = false;
            fadeOut = false;
            fadeIn = true;
        }
    }

    public void Init(bool _isEssence, bool _isGain, int _howMuch, Vector2 _spawn)
    {
        spawn = _spawn;
        transform.position = spawn;
        if (_isEssence)
        {
            switch (_howMuch)
            {
                case 1:
                    sr.sprite = Resources.Load<Sprite>("Assets/Graphics/Feedback/1essence");
                    break;
                case 3:
                    sr.sprite = Resources.Load<Sprite>("Assets/Graphics/Feedback/3essence");
                    break;
                case 8:
                    sr.sprite = Resources.Load<Sprite>("Assets/Graphics/Feedback/8essence");
                    break;
            }
            isGoingToPlayer = _isGain;
        }
        else
        {
            switch (_howMuch)
            {
                case 0:
                    sr.sprite = Resources.Load<Sprite>("Assets/Graphics/Feedback/0heart");
                    break;
                case 1:
                    sr.sprite = Resources.Load<Sprite>("Assets/Graphics/Feedback/1heart");
                    break;
                case 2:
                    sr.sprite = Resources.Load<Sprite>("Assets/Graphics/Feedback/2heart");
                    break;
                case 3:
                    sr.sprite = Resources.Load<Sprite>("Assets/Graphics/Feedback/3heart");
                    break;
                case 4:
                    sr.sprite = Resources.Load<Sprite>("Assets/Graphics/Feedback/4heart");
                    break;
            }
            sr.color = new Color(1f, 1f, 1f, 0f);
            isGrossying = true;
            isGoingUp = false;
            fadeOut = false;
            fadeIn = true;
        }
    }

    public IEnumerator EndFadeOut()
    {
        yield return new WaitForSeconds(0.5f);
        fadeIn = false;
        fadeOut = true;
        elapsedTime = 0;
        fadeMultiplicator = 2;
    }

    public IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(timeBeforeDestruction);
        Destroy(this.gameObject);
    }
}
