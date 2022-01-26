using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class SetPlayerNames : MonoBehaviour
{
    public TMP_Text liveSidePlayer;
    public TMP_Text evilSidePlayer;

    private float counter = 0f;
    private bool namesSet = false;

    // Start is called before the first frame update
    void Start()
    {
        SetNames();
    }

    private void Update()
    {
        if (!namesSet)
        {
            counter += Time.deltaTime;
            SetNames();
            if (counter > 5f)
            {
                namesSet = true;
            }
        }
    }

    private void SetNames()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            liveSidePlayer.text = PhotonNetwork.NickName;
            evilSidePlayer.text = PhotonNetwork.PlayerListOthers[0].NickName;
        }
        else
        {
            liveSidePlayer.text = PhotonNetwork.PlayerListOthers[0].NickName;
            evilSidePlayer.text = PhotonNetwork.NickName;
        }
    }
}
