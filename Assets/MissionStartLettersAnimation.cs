using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MissionStartLettersAnimation : MonoBehaviour
{
    [SerializeField] GameObject topLine;
    [SerializeField] GameObject bottomLineStart;
    [SerializeField] GameObject bottomLineComplete;

    GameObject bottomLine;
    Vector2 canvasSize;
    int topLineLettersCount;

    private List<Image> letters = new List<Image>();

	void Awake ()
    {
        Rect canvasRect = GetComponent<RectTransform>().rect;
        canvasSize = new Vector2(canvasRect.width, canvasRect.height);

        topLine.SetActive(false);
        bottomLineComplete.SetActive(false);
        bottomLineStart.SetActive(false);
    }
	
	void Update ()
    {
	    if (Input.GetKeyDown("a"))
        {
            StartAnim();
        }
	}

    public void SetComplete()
    {
        bottomLine = bottomLineComplete;
        letters.Clear();
    }

    public void SetStart()
    {
        bottomLine = bottomLineStart;
        letters.Clear();
    }

    public void StartAnim()
    {
        bottomLine.SetActive(true);
        topLine.SetActive(true);

        StartCoroutine(AnimCoroutine());
    }

    IEnumerator AnimCoroutine()
    {
        PositionLetters();
        HideAllLetters();
        RevealAllLetters();

        yield return new WaitForSeconds(1.2f);

        yield return BlinkStart();

        DisperseLetters();
    }

    void PositionLetters()
    {
        topLine.GetComponent<HorizontalLayoutGroup>().enabled = true;
        LayoutRebuilder.ForceRebuildLayoutImmediate(topLine.GetComponent<RectTransform>());
        topLine.GetComponent<HorizontalLayoutGroup>().enabled = false;

        bottomLine.GetComponent<HorizontalLayoutGroup>().enabled = true;
        LayoutRebuilder.ForceRebuildLayoutImmediate(bottomLine.GetComponent<RectTransform>());
        bottomLine.GetComponent<HorizontalLayoutGroup>().enabled = false;
    }

    void HideAllLetters()
    {
        for (int i=0; i < Letters().Count; i++)
        {
            Letters()[i].enabled = false;
        }
    }

    void RevealAllLetters()
    {
        for (int i = 0; i < Letters().Count; i++)
        {
            AnimLetter(Letters()[i].GetComponent<RectTransform>(), i);
        }
    }

    void AnimLetter(RectTransform letter, int index)
    {
        float duration = 4f/30f;
        float delay = index * 2f / 25f;

        float w = letter.sizeDelta.y * index;

        Vector3 offset = new Vector3(-w, letter.sizeDelta.y, 0);
        letter.DOMove(offset, duration)
            .SetDelay(delay)
            .OnStart(() => { letter.GetComponent<Image>().enabled = true;})
            .From(true);

        float durationRotation = 8f / 30f;
        Vector3 angle = new Vector3(0, 180, 0);
        letter.DORotate(angle, durationRotation)
            .SetDelay(delay)
            .SetEase(Ease.Linear)
            .From();
    }

    IEnumerator BlinkStart()
    {
        bool visible = true;
        int blink = 0;
        int blinkCount = 5;

        while (blink < blinkCount)
        {
            SetBottomLineVisible(visible);
            yield return new WaitForSeconds(4f / 30f);
            visible = !visible;
            blink++;
        }
    }

    void SetBottomLineVisible(bool visible)
    {
        for (int i = topLineLettersCount; i< Letters().Count; i++)
        {
            Letters()[i].enabled = visible;
        }
    }

    void DisperseLetters()
    {
        int endOfFirstLine = topLineLettersCount;
        float angleStep = Mathf.PI / (endOfFirstLine+1);

        for (int i=0; i<endOfFirstLine; i++)
        {
            float angle = (i+1) * angleStep - Mathf.PI;

            float x = canvasSize.x / 2f * Mathf.Cos(angle);

            Letters()[i].GetComponent<RectTransform>().DOLocalMoveX(x, 0.5f)
                .SetRelative(true)
                .SetEase(Ease.Linear);

            float y = - canvasSize.x / 2f * Mathf.Sin(angle);

            Letters()[i].GetComponent<RectTransform>().DOLocalMoveY(y, 0.5f)
                .SetRelative(true)
                .SetEase(Ease.Linear);
        }

        //DUPLICATED CODE PARTY
        for (int i = endOfFirstLine; i < Letters().Count; i++)
        {
            float angle = (i - endOfFirstLine + 2) * angleStep - Mathf.PI;

            float x = canvasSize.x / 2f * Mathf.Cos(angle);

            Letters()[i].GetComponent<RectTransform>().DOLocalMoveX(x, 0.5f)
                .SetRelative(true)
                .SetEase(Ease.Linear);

            float y = canvasSize.x / 2f * Mathf.Sin(angle);

            Letters()[i].GetComponent<RectTransform>().DOLocalMoveY(y, 0.5f)
                .SetRelative(true)
                .SetEase(Ease.Linear);
        }
    }


    List<Image> Letters()
    {
        if (letters == null || letters.Count == 0)
        {
            Image[] lettersTop = topLine.GetComponentsInChildren<Image>();
            Image[] lettersBottom = bottomLine.GetComponentsInChildren<Image>();

            topLineLettersCount = lettersTop.Length;

            letters.AddRange(lettersTop);
            letters.AddRange(lettersBottom);
        }

        return letters;
    }
}
