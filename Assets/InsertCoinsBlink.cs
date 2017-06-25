using UnityEngine;
using DG.Tweening;
using TMPro;

public class InsertCoinsBlink : MonoBehaviour {

    private TextMeshProUGUI insertCoins;

    void Start()
    {
        insertCoins = GetComponent<TextMeshProUGUI>();

        DOVirtual.DelayedCall(1, HideShow).SetLoops(-1);
    }

    void HideShow()
    {
        insertCoins.enabled = !insertCoins.enabled;
    }

	void Update () {

	
	}
}
