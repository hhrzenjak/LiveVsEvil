using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;


// Class for Player Information. Should be used for all player specific Information
// e.g. Player Name, Side, Money etc.
public class Player : MonoBehaviour
{

    public int money;
    public int moneyIncrease;
    public TMP_Text moneyTextElement;
    public TMP_Text incomeTextElement;
    public GameObject LiveRestriction;
    public GameObject EvilRestriction;
    public int secondsBetweenIncome;


    public void setupGameArea()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Destroy(LiveRestriction);
        }
        else
        {
            Destroy(EvilRestriction);
        }
    }



    //should be called when spending money, returns false when not enough money available
    //returns true when money is available AND REDUCES money amount
    public bool buy(int cost)
    {
        if (cost <= money)
        {
            money -= cost;
            updateMoneyUI();
            return true;
        }
        
        return false;
    }

    public bool canBuy()
    {
        return false;
    }

    public PlayerType getPlayerType()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            return PlayerType.Live;
        }
        else
        {
            return PlayerType.Evil;
        }
    }

    public void Start()
    {
        updateMoneyUI();
        Invoke("receiveMoney", secondsBetweenIncome);
        setupGameArea();
    }

    public void receiveMoney()
    {
        money += moneyIncrease;
        updateMoneyUI();
        Invoke("receiveMoney", secondsBetweenIncome);
    }

    public void updateMoneyUI()
    {
        moneyTextElement.text = money + " Gold";
        incomeTextElement.text = "+" + moneyIncrease;
    }
}

