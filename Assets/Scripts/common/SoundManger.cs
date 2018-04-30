using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Komugi
{
    public class SoundManger : SingletonMonoBehaviour<SoundManger>
    {
        private AudioClip[] seClips;
        private AudioClip[] bgmClips;

        private Dictionary<string, int> seIndexes = new Dictionary<string, int>();
        private Dictionary<string, int> bgmIndexes = new Dictionary<string, int>();

        const int cNumChannel = 6;
        private AudioSource bgmSource;
        private AudioSource[] seSources = new AudioSource[cNumChannel];

        Queue<int> seRequestQueue = new Queue<int>();

        override protected void Awake()
        {
            // 子クラスでAwakeを使う場合は
            // 必ず親クラスのAwakeをCallして
            // 複数のGameObjectにアタッチされないようにします.
            base.Awake();

            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;

            for (int i = 0; i < seSources.Length; i++)
            {
                seSources[i] = gameObject.AddComponent<AudioSource>();
            }

            seClips = Resources.LoadAll<AudioClip>("Audio/SE");
            bgmClips = Resources.LoadAll<AudioClip>("Audio/BGM");

            for (int i = 0; i < seClips.Length; ++i)
            {
                seIndexes[seClips[i].name] = i;
            }

            for (int i = 0; i < bgmClips.Length; ++i)
            {
                bgmIndexes[bgmClips[i].name] = i;
            }
        }

        void Update()
        {
            

            int count = seRequestQueue.Count;
            if (count != 0)
            {
                int sound_index = seRequestQueue.Dequeue();
                playSeImpl(sound_index);
            }
        }

        //------------------------------------------------------------------------------
        private void playSeImpl(int index)
        {
            if (0 > index || seClips.Length <= index)
            {
                return;
            }

            foreach (AudioSource source in seSources)
            {
                if (false == source.isPlaying)
                {
                    source.clip = seClips[index];
                    source.Play();
                    return;
                }
            }
        }

        //------------------------------------------------------------------------------
        public int GetSeIndex(string name)
        {
            return seIndexes[name];
        }

        //------------------------------------------------------------------------------
        public int GetBgmIndex(string name)
        {
            return bgmIndexes[name];
        }

        //------------------------------------------------------------------------------
        public void PlayBgm(string name)
        {
            int index = bgmIndexes[name];
            PlayBgm(index);
        }

        //------------------------------------------------------------------------------
        public void PlayBgm(int index)
        {
            if (0 > index || bgmClips.Length <= index)
            {
                return;
            }

            if (bgmSource.clip == bgmClips[index])
            {
                return;
            }

            bgmSource.Stop();
            bgmSource.clip = bgmClips[index];
            bgmSource.Play();
        }

        //------------------------------------------------------------------------------
        public void StopBgm()
        {
            bgmSource.Stop();
            bgmSource.clip = null;
        }

        //------------------------------------------------------------------------------
        public void PlaySe(string name)
        {
            PlaySe(GetSeIndex(name));
        }

        //一旦queueに溜め込んで重複を回避しているので
        //再生が1frame遅れる時がある
        //------------------------------------------------------------------------------
        public void PlaySe(int index)
        {
            if (!seRequestQueue.Contains(index))
            {
                seRequestQueue.Enqueue(index);
            }
        }

        //------------------------------------------------------------------------------
        public void StopSe()
        {
            ClearAllSeRequest();
            foreach (AudioSource source in seSources)
            {
                source.Stop();
                source.clip = null;
            }
        }

        //------------------------------------------------------------------------------
        public void ClearAllSeRequest()
        {
            seRequestQueue.Clear();
        }
    }
}
