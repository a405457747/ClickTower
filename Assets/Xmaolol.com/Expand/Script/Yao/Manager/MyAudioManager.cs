
//using QFramework;
using System.Collections;
using UnityEngine;

namespace xmaolol.com
{

    public class MyAudioManager : MonoBehaviour
    {
        public static MyAudioManager Instance;
        public string ResourceDir = "Audio";

        bool musicOn = true;
        bool soundOn = true;
        private float _musicVolume;
        private float _soundVolume;
        private GameObject obj;
        private AudioSource mainMusic;
        private ArrayList sounds = new ArrayList();

        public float musicVolume
        {
            get { return (float)MySaveManager.Instance.SaveMapping.musicVolume; }
            set
            {
                if (value != _musicVolume)
                {
                    _musicVolume = value;
                    mainMusic.volume = value;
                }
            }
        }

        public float soundVolume
        {
            get {  return (float)MySaveManager.Instance.SaveMapping.soundVolume; }
            set
            {
                if (_soundVolume != value)
                {
                    _soundVolume = value;
                    foreach (AudioSource src in sounds)
                    {
                        src.volume = value;
                    }
                }
            }
        }

        public bool MusicOn
        {
            get
            {
                return musicOn;
            }

            set
            {
                musicOn = value;
            }
        }

        public bool SoundOn
        {
            get
            {
                return soundOn;
            }

            set
            {
                soundOn = value;
            }
        }

        public static MyAudioManager GetInstance()
        {
            if (Instance == null)
            {
                GameObject obj = new GameObject("AudioManager");
                DontDestroyOnLoad(obj);
                Instance = obj.AddComponent<MyAudioManager>();
                Instance.obj = obj;
                Instance.mainMusic = obj.AddComponent<AudioSource>();
            }
            return Instance;
        }

        AudioClip LoadAudio(string audioName)
        {
            string path;
            if (string.IsNullOrEmpty(ResourceDir))
            {
                path = "";
            }
            else
            {
                path = ResourceDir + "/" + audioName;
            }
            //加载音乐
            AudioClip clip = Resources.Load<AudioClip>(path);
            //播放
            if (clip != null)
            {
                return clip;
            }
            else
            {
                return null;
            }
        }

        public void PlayMusic(string audioName)
        {
            PlayMusic(audioName, true);
        }

        public void PlayMusic(string audioName, bool loop)
        {
            mainMusic.Stop();
            mainMusic.clip = LoadAudio(audioName);
            mainMusic.volume = musicVolume;
            mainMusic.loop = loop;
            if (MusicOn && Time.timeScale != 0)
            {
                mainMusic.Play();
            }
        }

        public void StopMusic()
        {
            mainMusic.Stop();
        }

        public void PauseMusic()
        {
            mainMusic.Pause();
        }

        public void ResumeMusic()
        {
            if (MusicOn && Time.timeScale != 0)
            {
                mainMusic.Play();
            }
        }

        public AudioSource PlaySound(string audioName)
        {
            return PlaySound(audioName, false);
        }

        public AudioSource PlaySound(string audioName, bool loop)
        {
            AudioSource source = obj.AddComponent<AudioSource>();
            AudioClip sound = LoadAudio(audioName);
            source.clip = sound;
            source.volume = soundVolume;
            source.loop = loop;
            sounds.Add(source);
            if (SoundOn && Time.timeScale != 0)
            {
                source.Play();
            }
            if (!loop)
            {
                StartCoroutine(DoDestroy(source, sound.length));
            }

            return source;
        }

        IEnumerator DoDestroy(AudioSource sound, float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(sound);
            sounds.Remove(sound);
        }

        public void StopSound(AudioSource sound)
        {
            StartCoroutine(DoDestroy(sound, 0));
        }

        public void PauseAllSounds()
        {
            foreach (AudioSource src in sounds)
            {
                src.Pause();
            }
        }

        public void ResumeAllSounds()
        {
            if (SoundOn && Time.timeScale != 0)
            {
                foreach (AudioSource src in sounds)
                {
                    src.Play();
                }
            }
        }

        public void Clear()
        {
            foreach (AudioSource src in sounds)
            {
                Destroy(src);
            }
            sounds.Clear();
        }
    }
}

