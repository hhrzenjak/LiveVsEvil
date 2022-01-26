using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class GoalScript : MonoBehaviourPunCallbacks
{
    public Image towerDamage;
    public Slider towerDamageIndicator;
    public TMP_Text damageTextIndicatorLive;
    public TMP_Text damageTextIndicatorEvil;

    public int liveUnits = 12; //half of max
    public int maxUnits = 24; //even number
    public AudioClip victoryAudio;
    public AudioClip defeatAudio;
    public AudioClip alertAudio;
    private bool haswon = false;
    public GameObject defeatText;
    public GameObject victoryText;
    public GameObject evilVictoryScreen;
    public GameObject liveVictoryScreen;
    public GameObject defeatScreen;

    public ParticleSystem liveSideParticle;
    public ParticleSystem darkSideParticle;

    public RectTransform UITabs;
    

    private void Start()
    {
        UITabs.gameObject.SetActive(true);
        UpdateIndicators();
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (!haswon)
            {
                photonView.RPC("addEnemy", RpcTarget.All, other.gameObject.GetComponent<Enemy>().type);
            }
        }
    }

    void Update()
    {
        UpdateIndicators();
    }


    [PunRPC]
    public void addEnemy(PlayerType type)
    {
        if (type == PlayerType.Live)
        {
            liveUnits++;
            if(liveUnits >= maxUnits)
            {
                showVictory(PlayerType.Live);
            }
            else
            {
                playAlertSound();
            }
        }
        else
        {
            liveUnits--;
            if (liveUnits <= 0)
            {
                showVictory(PlayerType.Evil);
            }
            else
            {
                playAlertSound();
            }
        }

        UpdateIndicators();
    }

    public void UpdateIndicators()
    {
        //towerDamage.fillAmount = (float)liveUnits / (float)maxUnits;
        towerDamageIndicator.value = (float)liveUnits / (float)maxUnits;
        if (liveUnits > maxUnits / 2)
        {
            if (!liveSideParticle.isPlaying)
            {
                if (darkSideParticle.isPlaying)
                {
                    darkSideParticle.Stop();
                }
                liveSideParticle.Play();
            }
        }
        else if(liveUnits == maxUnits/2)
        {
            if (liveSideParticle.isPlaying)
            {
                liveSideParticle.Stop();
            }

            if (darkSideParticle.isPlaying)
            {
                darkSideParticle.Stop();

            }
        }
        else
        {
            if (!darkSideParticle.isPlaying)
            {
                if (liveSideParticle.isPlaying)
                {
                    liveSideParticle.Stop();
                }
                darkSideParticle.Play();
            }
        }
        
        damageTextIndicatorLive.text = liveUnits.ToString();
        damageTextIndicatorEvil.text = (maxUnits - liveUnits).ToString();
    }
    
    public void showVictory(PlayerType victor)
    {
        if (victor == PlayerType.Evil)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                defeatText.active = true;
                defeatScreen.active = true;
                playDefeatSound();
            }
            else
            {
                victoryText.active = true;
                evilVictoryScreen.active = true;
                playVictorySound();
            }
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                victoryText.active = true;
                liveVictoryScreen.active = true;
                playVictorySound();
            }
            else
            {
                defeatText.active = true;
                defeatScreen.active = true;
                playDefeatSound();
            }
        }
        haswon = true;
        UITabs.gameObject.SetActive(false);
        Invoke("backToLobby", 15);
    }

    public void playVictorySound()
    {
        foreach (GameObject music in GameObject.FindGameObjectsWithTag("Music"))
        {
            AudioSource a = music.GetComponent<AudioSource>();
            a.Stop();
        }

        GameObject audioGO = GameObject.FindGameObjectWithTag("Audio");
        AudioSource audio = audioGO.GetComponent<AudioSource>();
        audio.PlayOneShot(victoryAudio);
    }
    public void playDefeatSound()
    {
        foreach (GameObject music in GameObject.FindGameObjectsWithTag("Music"))
        {
            AudioSource a = music.GetComponent<AudioSource>();
            a.Stop();
        }
        GameObject audioGO = GameObject.FindGameObjectWithTag("Audio");
        AudioSource audio = audioGO.GetComponent<AudioSource>();
        audio.PlayOneShot(defeatAudio);
    }
    
    public void playAlertSound()
    {
        GameObject audioGO = GameObject.FindGameObjectWithTag("Audio");
        AudioSource audio = audioGO.GetComponent<AudioSource>();
        audio.PlayOneShot(alertAudio);
    }

    public void backToLobby()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("LobbyBrowser");
    }
}
