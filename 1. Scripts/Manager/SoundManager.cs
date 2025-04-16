using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace KJ
{
    /// <summary>
    /// 사운드 관리용 매니저
    /// 볼륨 조절
    /// 배경음악 재생, 중단
    /// 효과음 재생, 중단
    /// </summary>
    public class SoundManager : SingletonMonoBehaviour<SoundManager>
    {
        public AudioMixer mixer         = null;
        public Transform audioRoot      = null;
        public AudioSource bgmPlayerA   = null;
        public AudioSource bgmPlayerB   = null;
        public AudioSource[] sfxPlayers = null;

        private int sfxChannelCount  = 10;

        private const float minVolume   = -80.0f;
        private const float maxVolume   = 20.0f;

        private void Start()
        {
            if (mixer == null)
            { 
                mixer = Resources.Load("AudioMixer") as AudioMixer;
            }

            if (audioRoot == null)
            {
                audioRoot = new GameObject("AudioRoot").transform;
                audioRoot.SetParent(transform);
            }

            if (bgmPlayerA == null)
            {
                GameObject bgmA = new GameObject("BGM_A", typeof(AudioSource));
                bgmA.transform.SetParent(audioRoot);
                bgmPlayerA = bgmA.GetComponent<AudioSource>();
                bgmPlayerA.playOnAwake = false;
            }

            if (bgmPlayerB == null)
            {
                GameObject bgmB = new GameObject("BGM_B", typeof(AudioSource));
                bgmB.transform.SetParent(audioRoot);
                bgmPlayerB = bgmB.GetComponent<AudioSource>();
                bgmPlayerB.playOnAwake = false;
            }

            if (sfxPlayers == null || sfxPlayers.Length == 0)
            {
                sfxPlayers = new AudioSource[sfxChannelCount];
                for (int i = 0; i < sfxPlayers.Length; i++)
                {
                    GameObject sfx = new GameObject("SFX_" + i.ToString(), typeof(AudioSource));
                    sfx.transform.SetParent(audioRoot);
                    sfxPlayers[i] = sfx.GetComponent<AudioSource>();
                    sfxPlayers[i].playOnAwake = false;
                }
            }

            if (mixer != null)
            {
                bgmPlayerA.outputAudioMixerGroup = mixer.FindMatchingGroups("BGM")[0];
                bgmPlayerB.outputAudioMixerGroup = mixer.FindMatchingGroups("BGM")[0];
                for (int i = 0; i < sfxChannelCount; i++)
                {
                    sfxPlayers[i].outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
                }
            }

            InitVolume();

        }

        public void SetMasterVolume(float ratio)
        {
            ratio = Mathf.Clamp01(ratio);
            float volume = Mathf.Lerp(minVolume, maxVolume, ratio);
            Debug.Log(volume);
            mixer.SetFloat("Master", volume);
            PlayerPrefs.SetFloat("Master", volume);
        }

        // ratio 로 반환
        public float GetMasterVolume()
        {
            if (PlayerPrefs.HasKey("Master"))
            {
                return Mathf.InverseLerp(minVolume, maxVolume, PlayerPrefs.GetFloat("Master"));
            }
            else
            {
                return 0f;
            }
        }

        public void SetBGMVolume(float ratio)
        {
            ratio = Mathf.Clamp01(ratio);
            float volume = Mathf.Lerp(minVolume, maxVolume, ratio);
            mixer.SetFloat("BGM", volume);
            PlayerPrefs.SetFloat("BGM", volume);
        }

        // ratio 로 반환
        public float GetBGMVolume()
        {
            if (PlayerPrefs.HasKey("BGM"))
            {
                return Mathf.InverseLerp(minVolume, maxVolume, PlayerPrefs.GetFloat("BGM"));
            }
            else
            {
                return 0f;
            }
        }

        public void SetSFXVolume(float ratio)
        {
            ratio = Mathf.Clamp01(ratio);
            float volume = Mathf.Lerp(minVolume, maxVolume, ratio);
            mixer.SetFloat("SFX", volume);
            PlayerPrefs.SetFloat("SFX", volume);
        }

        // ratio 로 반환
        public float GetSFXVolume()
        {
            if (PlayerPrefs.HasKey("SFX"))
            {
                return Mathf.InverseLerp(minVolume, maxVolume, PlayerPrefs.GetFloat("SFX"));
            }
            else
            {
                return 0f;
            }
        }

        public void InitVolume()
        {
            if (mixer != null)
            {
                mixer.SetFloat("Master", GetMasterVolume());
                mixer.SetFloat("BGM", GetBGMVolume());
                mixer.SetFloat("SFX", GetSFXVolume());
            }
        }

        void PlayAudioSource(AudioSource source, SoundClip clip, float volume)
        {
            if (source == null || clip == null)
                return;

            source.Stop();
            source.clip = clip.GetClip;
            source.loop = clip.isLoop;
            source.volume = volume;
            source.minDistance = clip.minDistance;
            source.maxDistance = clip.maxDistance;
            source.Play();
        }
        void PlayAudioSourceAtPoint(SoundClip clip, Vector3 position, float volume)
        {
            AudioSource.PlayClipAtPoint(clip.GetClip, position, volume);
        }

        public void PlayEffectSound(SoundClip clip)
        {
            for (int i = 0; i < sfxChannelCount; i++)
            {
                if (sfxPlayers[i].isPlaying == false)
                {
                    PlayAudioSource(sfxPlayers[i], clip, 1.0f);
                    break;
                }
                else if (sfxPlayers[i].clip = clip.GetClip)
                {
                    sfxPlayers[i].Stop();
                    PlayAudioSource(sfxPlayers[i], clip, 1.0f);
                    break;
                }
            }
        }
        public void PlayEffectSound(SoundClip clip, Vector3 position, float volume)
        {
            for (int i = 0; i < sfxChannelCount; i++)
            {
                if (sfxPlayers[i].isPlaying == false)
                {
                    PlayAudioSourceAtPoint(clip, position, volume);
                    break;
                }
                else if (sfxPlayers[i].clip = clip.GetClip)
                {
                    sfxPlayers[i].Stop();
                    PlayAudioSourceAtPoint(clip, position, volume);
                    break;
                }
            }
        }

        public void PlayOneShotEffect(SoundList soundList, Vector3 position, float volume)
        {
            SoundClip clip = DataManager.SoundData.GetCopy((int)soundList);
            clip.PreLoad();

            PlayEffectSound(clip, position, volume);
        }
    }
}


