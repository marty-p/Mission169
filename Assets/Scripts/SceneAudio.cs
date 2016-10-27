using UnityEngine;
using SlugLib;

namespace Mission169 {

    public class SceneAudio : MonoBehaviour {

        private SlugAudioManager soundManager;
        private AudioSource music;

        void Awake() {
            soundManager = GetComponent<SlugAudioManager>();
            EventManager.Instance.StartListening(GlobalEvents.MissionStart, PlayMainMusic);
            EventManager.Instance.StartListening(GlobalEvents.BossStart, PlayBossMusic);
            EventManager.Instance.StartListening(GlobalEvents.BossDead, PlaySuccess);
            EventManager.Instance.StartListening(GlobalEvents.GameOver, PlayGameOver);
        }

        void PlayMainMusic() {
            music = soundManager.PlaySound(0);
        }

        void PlayBossMusic() {
            if (music!= null){
                music.Stop();
            }
            music = soundManager.PlaySound(1);
        }

        void PlaySuccess() {
            music.Stop();
            soundManager.PlaySound(2);
            soundManager.PlaySound(3);
        }

        void PlayGameOver() {
            music.Stop();
            soundManager.PlaySound(4);
        }
    }

}
