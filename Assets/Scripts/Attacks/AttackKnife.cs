using UnityEngine;
using SlugLib;

public class AttackKnife : MonoBehaviour, IAttack {

    public Animator anim;
    public AreaOfEffectProjectile knife;
    public SlugAudioManager audioManager;
    private bool attackSwitch;
    public TimeUtils timeUtils;

    public void Execute(string victimTag, Vector3 unused, Vector3 unused2) {
        if (attackSwitch) {
            anim.SetTrigger("knife2");
            audioManager.PlaySound(4);
        } else {
            anim.SetTrigger("knife");
            audioManager.PlaySound(5);
        }
        attackSwitch = !attackSwitch;

        knife.CastAOE(victimTag, transform.position);
        EventManager.Instance.TriggerEvent(GlobalEvents.KnifeUsed);
        timeUtils.TimeDelay(0.3f, () => { anim.SetBool("knifeing", false); });
    }

    public bool InProgress() {
        return anim.GetBool("knifeing");
    }

}
