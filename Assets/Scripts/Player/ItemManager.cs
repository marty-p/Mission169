using UnityEngine;
using SlugLib;

public class ItemManager : MonoBehaviour {

    public MarcoAttackManager attackManager;
    public FlashUsingMaterial flashBlue;
    public SlugAudioManager audioManager;


    public void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Collectible") {
            CollectibleDef item = col.GetComponent<CollectibleDef>();
            if (item!= null) {
                PickUpItem(item);
            }
        }
    }

    private void PickUpItem(CollectibleDef item) {
        if (item.attackID > 0) {
            EventManager.Instance.TriggerEvent(GlobalEvents.ItemPickedUp);
            attackManager.SetAttack(item.attackID, item.animController);
            attackManager.UpdateBulletCount(item.bulletCount);
        }
        flashBlue.FlashForXSecs(0.18f);
        audioManager.PlaySoundByClip(item.weaponNameAudio);
    }

}
