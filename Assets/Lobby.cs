using System;
using System.Collections;
using System.Collections.Generic;
using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class Lobby : MonoBehaviourPunCallbacks
{
    private float timeToStart = 15.0f;
    private bool ctdnStart = false;
    private float timer;
    
    private int playersReady = 0;
    private int playersReadyToStart = 0;
    private bool startSetup = false;

    public Image countdownImageOuter;
    public Image countdownImageInner;
    public TMP_Text countdownText;
    public TMP_Text beforeCountdownText;

    public TMP_InputField liveInput;
    public TMP_InputField evilInput;

    public Button liveReady;
    public Button evilReady;

    public SimpleScrollSnap liveAdjectives;
    public SimpleScrollSnap liveNouns;
    public SimpleScrollSnap evilAdjectives;
    public SimpleScrollSnap evilNouns;
    
    // Start is called before the first frame update
    void Start()
    {
        countdownImageInner.gameObject.SetActive(false);
        countdownImageOuter.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(false);
        beforeCountdownText.gameObject.SetActive(true);

        if (PhotonNetwork.IsMasterClient)
        {
            evilInput.placeholder.GetComponent<TMP_Text>().text = "";
            evilInput.interactable = false;
            evilReady.GetComponent<ButtonPressed>().Disable();
            evilAdjectives.gameObject.SetActive(false);
            evilNouns.gameObject.SetActive(false);
            liveReady.onClick.AddListener(() => {Ready();});
        }
        else
        {
            liveInput.placeholder.GetComponent<TMP_Text>().text = "";
            liveInput.interactable = false;
            liveReady.GetComponent<ButtonPressed>().Disable();
            liveAdjectives.gameObject.SetActive(false);
            liveNouns.gameObject.SetActive(false);
            evilReady.onClick.AddListener(() => {Ready();});
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if(PhotonNetwork.CurrentRoom.PlayerCount == 2)
        // {
        //     PhotonNetwork.LoadLevel("Dev");
        // }
        
        if (ctdnStart)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    String adjective = liveAdjectives.Content.GetChild(liveAdjectives.CurrentPanel).GetComponentInChildren<TMP_Text>()
                        .text;
                    String noun = liveNouns.Content.GetChild(liveNouns.CurrentPanel).GetComponentInChildren<TMP_Text>().text;
                    PhotonNetwork.NickName = String.Format("{0} {1} {2}", adjective, noun, liveInput.text);
                }
                else
                {
                    String adjective = evilAdjectives.Content.GetChild(evilAdjectives.CurrentPanel).GetComponentInChildren<TMP_Text>()
                        .text;
                    String noun = evilNouns.Content.GetChild(evilNouns.CurrentPanel).GetComponentInChildren<TMP_Text>().text;
                    PhotonNetwork.NickName = String.Format("{0} {1} {2}", adjective, noun, evilInput.text);
                }
                
                PhotonNetwork.LoadLevel("Dev");
                /*if (!startSetup)
                { 

                    
                    playersReadyToStart += 1;
                    if (playersReadyToStart == 2)
                    {
                        photonView.RPC("StartGame", RpcTarget.All);
                    }
                    else
                    {
                        photonView.RPC("IncrementStartCounter", RpcTarget.AllBuffered);
                    }

                    startSetup = true;
                }*/

            }
            else
            {
                countdownImageOuter.fillAmount = timer / (float) timeToStart;
                countdownImageOuter.color = Color.Lerp(new Color(0.37f, 0.12f, 0.56f), Color.yellow, countdownImageOuter.fillAmount);
                countdownImageInner.color = Color.Lerp(new Color(0.37f, 0.12f, 0.56f), Color.yellow, countdownImageOuter.fillAmount);
            }
        }
    }

    public void Ready()
    {
        
        if (PhotonNetwork.IsMasterClient)
        {
            liveReady.GetComponent<ButtonPressed>().Disable();
            PhotonNetwork.NickName = liveInput.text;
            liveInput.interactable = false;
        }
        else
        {
            evilReady.GetComponent<ButtonPressed>().Disable();
            PhotonNetwork.NickName = evilInput.text;
            evilInput.interactable = false;

        }
        
        playersReady += 1;
        if (playersReady == 2)
        {
            photonView.RPC("StartCountdown", RpcTarget.All);
        }
        else
        {
            photonView.RPC("IncrementReadyCounter", RpcTarget.AllBuffered);
        }


    }

    [PunRPC]
    public void IncrementReadyCounter()
    {
        playersReady += 1;
    }

    [PunRPC]
    public void StartCountdown()
    {
        ctdnStart = true;
        timer = timeToStart;
        var otherPlayer = PhotonNetwork.PlayerListOthers[0];
        
        if(PhotonNetwork.IsMasterClient)
            evilInput.text = otherPlayer.NickName;
        else
            liveInput.text = otherPlayer.NickName;
        
        beforeCountdownText.gameObject.SetActive(false);
        countdownImageInner.gameObject.SetActive(true);
        countdownImageOuter.gameObject.SetActive(true);
        countdownText.gameObject.SetActive(true);

    }
    
        
    [PunRPC]
    public void IncrementStartCounter()
    {
        playersReadyToStart += 1;
    }


    [PunRPC]
    public void StartGame()
    {
        PhotonNetwork.LoadLevel("Dev");
    }
}
