using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TowerUI : MonoBehaviour
{
    public GameObject rangeIndicator;
    public Player player;
    public GameObject basicTower;
    public GameObject sniperTower;
    public GameObject slowTower;
    public GameObject rocketTower;
    public int basicTowerCost;
    public int sniperTowerCost;
    public int slowTowerCost;
    public int rocketTowerCost;
    public GameArea gameArea;

    private GameObject towerShadow = null;
    private GameObject rangeShadow = null;
    private GameObject currentTower = null;
    private int currentTowerCost;
    
    
    private LayerMask layerMask;
    
    
    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Road", "Tower");
    }

    // Update is called once per frame
    void Update()
    {
        if (towerShadow != null)
        {
            towerShadow.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, Camera.main.nearClipPlane + 9));
            rangeShadow.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, Camera.main.nearClipPlane + 9));
            Collider2D towerCollider = towerShadow.transform.GetChild(0).GetComponent<Collider2D>();
            if (Input.GetMouseButtonDown(0))
            {

                if (!Physics2D.OverlapBox(towerShadow.transform.position, towerCollider.bounds.size, 0, layerMask) &&
                    player.buy(currentTowerCost))
                {
                    GameObject tower = PhotonNetwork.Instantiate(currentTower.name,
                        Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                            Camera.main.nearClipPlane + 9)), Quaternion.identity);
                    tower.GetComponent<Tower>().owner = player.getPlayerType();
                    ResetIndicators();
                }
            }
            else
            {
                if (Physics2D.OverlapBox(towerShadow.transform.position, towerCollider.bounds.size, 0, layerMask))
                {
                    towerShadow.GetComponent<SpriteRenderer>().color = Color.red;
                }

                if (!Physics2D.OverlapBox(towerShadow.transform.position, towerCollider.bounds.size, 0, layerMask))
                {
                    towerShadow.GetComponent<SpriteRenderer>().color =
                        currentTower.GetComponent<SpriteRenderer>().color;
                }
            }

            // cancel buying
            if (Input.GetMouseButtonDown(1))
            {
                ResetIndicators();
            }
        }


    }

    public void ResetIndicators()
    {
        Destroy(towerShadow);
        Destroy(rangeShadow);
        towerShadow = null;
        rangeShadow = null;
        currentTower = null;
    }

    public void SpawnBasicTower()
    {
        //TODO if he already holds the tower didnt place it but selects another one, that clip thing is sus
        currentTower = basicTower;
        currentTowerCost = basicTowerCost;
        InstantiateTowerShadow(currentTower);

    }

    public void SpawnSniperTower()
    {
        //TODO if he already holds the tower didnt place it but selects another one, that clip thing is sus
        currentTower = sniperTower;
        currentTowerCost = sniperTowerCost;
        InstantiateTowerShadow(currentTower);

    }

    public void SpawnSlowTower()
    {
        //TODO if he already holds the tower didnt place it but selects another one, that clip thing is sus
        currentTower = slowTower;
        currentTowerCost = slowTowerCost;
        InstantiateTowerShadow(currentTower);

    }

    public void SpawnRocketTower()
    {
        //TODO if he already holds the tower didnt place it but selects another one, that clip thing is sus
        currentTower = rocketTower;
        currentTowerCost = rocketTowerCost;
        InstantiateTowerShadow(currentTower);
    }

    public void InstantiateTowerShadow(GameObject tower)
    {
        if (towerShadow == null)
        {
            towerShadow = Instantiate(tower,
                Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                    Camera.main.nearClipPlane + 9)), Quaternion.identity);
            rangeShadow = Instantiate(rangeIndicator,
                Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                    Camera.main.nearClipPlane + 9)), Quaternion.identity);
            towerShadow.GetComponent<Tower>().enabled = false;
            towerShadow.GetComponent<Tower>().owner = PlayerType.None;
            rangeShadow.transform.localScale = new Vector3(tower.GetComponent<Tower>().range*2, tower.GetComponent<Tower>().range*2, tower.GetComponent<Tower>().range);

            //change layer from tower so it doesn't detect itself when placing
            towerShadow.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
}
