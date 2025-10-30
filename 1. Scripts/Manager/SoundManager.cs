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
        public enum MusicPlayingType
        { 
            None = 0,
            SourceA = 1,
            SourceB = 2,
            AtoB = 3,
            BtoA = 4
        }

        public AudioMixer mixer         = null;
        public Transform audioRoot      = null;
        public AudioSource bgmPlayerA   = null;
        public AudioSource bgmPlayerB   = null;
        public AudioSource[] sfxPlayers = null;
        public AudioSource uiPlayer = null;

        public float[] sfxPlayStartTime = null;
        private int sfxChannelCount  = 10;
        private MusicPlayingType currentPlayingType = MusicPlayingType.None;
        private bool isTicking = false;
        private SoundClip currentSound = null;
        private SoundClip lastSound = null;
        private const float minVolume   = -80.0f;
        private const float maxVolume   = 20.0f;
        

        private void Awake()
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
                bgmA.transform.localPosition = Vector3.zero;
                bgmPlayerA = bgmA.GetComponent<AudioSource>();
                bgmPlayerA.playOnAwake = false;
            }

            if (bgmPlayerB == null)
            {
                GameObject bgmB = new GameObject("BGM_B", typeof(AudioSource));
                bgmB.transform.SetParent(audioRoot);
                bgmB.transform.localPosition = Vector3.zero;
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
            if (uiPlayer == null)
            {
                GameObject ui = new GameObject("UI", typeof(AudioSource));
                ui.transform.SetParent(audioRoot);
                ui.transform.localPosition = Vector3.zero;
                uiPlayer = ui.GetComponent<AudioSource>();
                uiPlayer.playOnAwake = false;
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

        /// <summary>
        /// ratio 로 반환
        /// </summary>
        /// <returns></returns>
        public float GetMasterVolume()
        {
            if (PlayerPrefs.HasKey("Master"))
            {
                return Mathf.InverseLerp(minVolume, maxVolume, PlayerPrefs.GetFloat("Master"));
            }
            else
            {
                return 1f;
            }
        }

        public void SetBGMVolume(float ratio)
        {
            ratio = Mathf.Clamp01(ratio);
            float volume = Mathf.Lerp(minVolume, maxVolume, ratio);
            mixer.SetFloat("BGM", volume);
            PlayerPrefs.SetFloat("BGM", volume);
        }

        /// <summary>
        /// ratio 로 반환
        /// </summary>
        /// <returns></returns>
        public float GetBGMVolume()
        {
            if (PlayerPrefs.HasKey("BGM"))
            {
                return Mathf.InverseLerp(minVolume, maxVolume, PlayerPrefs.GetFloat("BGM"));
            }
            else
            {
                return 1f;
            }
        }

        public void SetSFXVolume(float ratio)
        {
            ratio = Mathf.Clamp01(ratio);
            float volume = Mathf.Lerp(minVolume, maxVolume, ratio);
            mixer.SetFloat("SFX", volume);
            PlayerPrefs.SetFloat("SFX", volume);
        }

        /// <summary>
        /// ratio 로 반환
        /// </summary>
        /// <returns></returns>
        public float GetSFXVolume()
        {
            if (PlayerPrefs.HasKey("SFX"))
            {
                return Mathf.InverseLerp(minVolume, maxVolume, PlayerPrefs.GetFloat("SFX"));
            }
            else
            {
                return 1f;
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
            source.clip = clip.GetClip();
            source.loop = clip.isLoop;
            source.volume = volume;
            source.pitch = clip.pitch;
            source.dopplerLevel = clip.dopplerLevel;
            source.rolloffMode = clip.rolloffMode;
            source.minDistance = clip.minDistance;
            source.maxDistance = clip.maxDistance;
            source.spatialBlend = clip.spartialBlend;
            source.Play();
        }

        void PlayAudioSourceAtPoint(SoundClip clip, Vector3 position, float volume)
        {
            AudioSource.PlayClipAtPoint(clip.GetClip(), position, volume);
        }
        
        public bool IsPlaying()
        {
            return (int)currentPlayingType > 0;
        }
        public bool IsDifferentSound(SoundClip clip)
        {
            if (clip == null)
            {
                return false;
            }
            if (currentSound != null && currentSound.clipName == clip.clipName && IsPlaying() && currentSound.isFadeOut == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private IEnumerator CheckRoutine()
        {
            while(isTicking && IsPlaying())
            {
                yield return new WaitForSeconds(0.05f);
                if (currentSound.HasLoop())
                {
                    if (currentPlayingType == MusicPlayingType.SourceA)
                    {
                        currentSound.CheckLoop(bgmPlayerA);
                    }
                    else if (currentPlayingType == MusicPlayingType.SourceB)
                    {
                        currentSound.CheckLoop(bgmPlayerB);
                    }
                    else if (currentPlayingType == MusicPlayingType.AtoB)
                    {
                        lastSound.CheckLoop(bgmPlayerA);
                        currentSound.CheckLoop(bgmPlayerB);
                    }
                    else if (currentPlayingType == MusicPlayingType.BtoA)
                    {
                        lastSound.CheckLoop(bgmPlayerB);
                        currentSound.CheckLoop(bgmPlayerA);
                    }
                }
            }
        }

        public void DoCheck()
        {
            StartCoroutine(CheckRoutine());
        }

        public void FadeIn(SoundClip clip, float time, Interpolate.EaseType ease)
        {
            if (IsDifferentSound(clip))
            {
                bgmPlayerA.Stop();
                bgmPlayerB.Stop();
                lastSound = currentSound;
                currentSound = clip;
                PlayAudioSource(bgmPlayerA, currentSound, 0.0f);
                currentSound.FadeIn(time, ease);
                currentPlayingType = MusicPlayingType.SourceA;
                if (currentSound.HasLoop())
                {
                    isTicking = true;
                    DoCheck();
                }
            }
        }

        public void FadeIn(int index, float time, Interpolate.EaseType ease)
        {
            FadeIn(DataManager.SoundData.GetCopy(index), time, ease);
        }

        public void FadeOut(float time, Interpolate.EaseType ease)
        {
            if (currentSound != null)
            {
                currentSound.FadeOut(time, ease);
            }
        }

        private void Update()
        {
            if (currentSound == null)
                return;
            if (currentPlayingType == MusicPlayingType.SourceA)
            {
                currentSound.DoFade(Time.deltaTime, bgmPlayerA);
            }
            else if (currentPlayingType == MusicPlayingType.SourceB)
            {
                currentSound.DoFade(Time.deltaTime, bgmPlayerB);
            }
            else if (currentPlayingType == MusicPlayingType.AtoB)
            {
                this.lastSound.DoFade(Time.deltaTime, bgmPlayerA);
                this.currentSound.DoFade(Time.deltaTime, bgmPlayerB);
            }
            else if (currentPlayingType == MusicPlayingType.BtoA)
            {
                this.lastSound.DoFade(Time.deltaTime, bgmPlayerB);
                this.currentSound.DoFade(Time.deltaTime, bgmPlayerA);
            }

            if (bgmPlayerA.isPlaying && this.bgmPlayerB.isPlaying == false)
            {
                this.currentPlayingType = MusicPlayingType.SourceA;
            }
            else if (bgmPlayerB.isPlaying && bgmPlayerA.isPlaying == false)
            {
                this.currentPlayingType = MusicPlayingType.SourceB;
            }
            else if (bgmPlayerA.isPlaying == false && bgmPlayerB.isPlaying == false)
            {
                this.currentPlayingType = MusicPlayingType.None;
            }
        }

        public void FadeTo(SoundClip clip, float time, Interpolate.EaseType ease)
        {
            if (currentPlayingType == MusicPlayingType.None)
            {
                FadeIn(clip, time, ease);
            }

            else if (IsDifferentSound(clip))
            {
                if (currentPlayingType == MusicPlayingType.AtoB)
                {
                    bgmPlayerA.Stop();
                    currentPlayingType = MusicPlayingType.SourceB;
                }
                else if (currentPlayingType == MusicPlayingType.BtoA)
                {
                    bgmPlayerB.Stop();
                    currentPlayingType = MusicPlayingType.SourceA;
                }

                lastSound = currentSound;
                currentSound = clip;
                lastSound.FadeOut(time, ease);
                currentSound.FadeIn(time, ease);
                if (currentPlayingType ==  MusicPlayingType.SourceA)
                {
                    PlayAudioSource(bgmPlayerB, currentSound, 0.0f);
                    currentPlayingType = MusicPlayingType.AtoB;
                }
                else if (currentPlayingType == MusicPlayingType.SourceB)
                {
                    PlayAudioSource(bgmPlayerA, currentSound, 0.0f);
                    currentPlayingType = MusicPlayingType.BtoA;
                }
                if (currentSound.HasLoop())
                {
                    isTicking = true;
                    DoCheck();
                }
            }
        }
        
        public void FadeTo(int index, float time, Interpolate.EaseType ease)
        {
            FadeTo(DataManager.SoundData.GetCopy(index), time, ease);
        }

        public void PlayBGM(SoundClip clip)
        {
            if (IsDifferentSound(clip))
            {
                bgmPlayerB.Stop();
                lastSound = currentSound;
                currentSound = clip;
                PlayAudioSource(bgmPlayerA, clip, clip.maxVolume);
                if (currentSound.HasLoop())
                {
                    isTicking = true;
                    DoCheck();
                }
            }
        }

        public void PlayBGM(int index)
        {
            SoundClip clip = DataManager.SoundData.GetCopy(index);
            PlayBGM(clip);
        }

        public void PlayUISound(SoundClip clip)
        {
            PlayAudioSource(uiPlayer, clip, clip.maxVolume);
        }

        public void PlayUISound(int index)
        {
            PlayUISound(DataManager.SoundData.GetCopy(index));
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
                else if (sfxPlayers[i].clip = clip.GetClip())
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
                else if (sfxPlayers[i].clip = clip.GetClip())
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

        public void Stop(bool allStop = false)
        {
            if (allStop)
            {
                bgmPlayerA.Stop();
                bgmPlayerB.Stop();
            }

            FadeOut(0.5f, Interpolate.EaseType.Linear);
            currentPlayingType = MusicPlayingType.None;
            StopAllCoroutines();
        }
    }
}


