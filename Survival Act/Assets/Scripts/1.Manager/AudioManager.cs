using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] 
public class AudioManager 
{
    [Header("#BGM")]
    public AudioClip BGM_Clip;
    public float BGM_Volume = 0.30f;
    private AudioSource bgmPlayer;
    private AudioHighPassFilter bgmEffect;

    [Header("SFX")]
    public AudioClip[] SFX_Clips;
    public float SFX_Volume = 0.2f;
    public int channels = 10;
    private AudioSource[] sfxPlayers;
    private int channelIndex;

    public enum SFX { Dead, Hit, LevelUp  =3, Lose, Melee, Range = 7, Select, Win, PickUp }

    public void Init()
    {
        GameObject audio = Managers.Instance.gameObject;
        audio.transform.parent = Managers.Instance.transform;
        //배경음 초기화
        GameObject bgmObject = new GameObject("BGM_Player");
        bgmObject.transform.parent = audio.transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = BGM_Volume;
        BGM_Clip = Managers.Resource.Load<AudioClip>(Managers.Data.AudioDic["BGM"].sounds[0]);
        bgmPlayer.clip = BGM_Clip;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();
        //효과음 초기화
        GameObject sfxObject = new GameObject("SFX_Player");
        sfxObject.transform.parent = audio.transform;
        sfxPlayers = new AudioSource[channels];

        for(int idx = 0; idx < sfxPlayers.Length; idx++)
        {
            sfxPlayers[idx] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[idx].playOnAwake = false;
            sfxPlayers[idx].volume = SFX_Volume;
            sfxPlayers[idx].bypassListenerEffects = true;
        }
        SFX_Clips = new AudioClip[Managers.Data.AudioDic["SFX"].sounds.Length];
        //json에 저장한 이름 기준으로 adressable에서 긁어서 저장한 데이터에서 로드
        for(int idx = 0; idx < SFX_Clips.Length; idx++)
        {
            SFX_Clips[idx] = Managers.Resource.Load<AudioClip>(Managers.Data.AudioDic["SFX"].sounds[idx]);
        }
    }

    public void PlaySFX(SFX sfx)
    {
        for(int idx = 0; idx < sfxPlayers.Length; idx++)
        {
            int loopIdx = (idx + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIdx].isPlaying)
                continue;

            int random = 0;
            if (sfx == SFX.Hit || sfx == SFX.Melee)
            {
                random = UnityEngine.Random.Range(0, 2);
            }

            channelIndex = loopIdx;
            sfxPlayers[loopIdx].clip = SFX_Clips[(int)sfx + random];
            sfxPlayers[loopIdx].Play();
            break;
        }
    }

    public void ChangeBGM(int idx)
    {
        bgmPlayer.clip = Managers.Resource.Load<AudioClip>(Managers.Data.AudioDic["BGM"].sounds[idx]);
    }

    public void PlayBGM(bool isPlay)
    {
        if (isPlay)
        {
            bgmPlayer.Play();
        }
        else
            bgmPlayer.Stop();
    }

    public void EffectBGM(bool isPlay)
    {
        //씬 이동 등으로 이펙트가 바뀐다면 재 정의. 안 할 시 missing refference 문제 발생
        if(bgmEffect == null)
            bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();
        bgmEffect.enabled = isPlay;
    }

}
