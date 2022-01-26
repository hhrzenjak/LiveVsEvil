using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabSwitch : MonoBehaviour
{
    public Button enemiesTabHandle;
    public Button towersTabHandle;
    
    public GameObject enemiesPanel;
    public GameObject towersPanel;

    private void Awake()
    {
        enemiesPanel.SetActive(true);
        towersPanel.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        enemiesTabHandle.onClick.AddListener(ShowEnemiesPanel);
        towersTabHandle.onClick.AddListener(ShowTowersPanel);

    }

    private void ShowEnemiesPanel()
    {
        towersPanel.GetComponent<TowerUI>().ResetIndicators();
        towersPanel.SetActive(false);
        enemiesPanel.SetActive(true);
    }
    
    private void ShowTowersPanel()
    {
        enemiesPanel.SetActive(false);
        towersPanel.SetActive(true);
    }
}
