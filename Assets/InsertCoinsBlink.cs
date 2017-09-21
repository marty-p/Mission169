using UnityEngine;
using DG.Tweening;
using TMPro;

public class InsertCoinsBlink : MonoBehaviour {

    private TextMeshProUGUI insertCoins;
    private Tween tween;

    void Start()
    {
        insertCoins = GetComponent<TextMeshProUGUI>();

        tween = DOVirtual.DelayedCall(1, HideShow).SetLoops(-1);
    }

    void HideShow()
    {
        insertCoins.enabled = !insertCoins.enabled;
    }

    void OnDestroy()
    {
        if (tween != null)
        {
            tween.Kill();
        }
    }

	void Update () {

	
	}
}
