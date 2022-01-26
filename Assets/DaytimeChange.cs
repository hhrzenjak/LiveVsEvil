using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class DaytimeChange : MonoBehaviourPunCallbacks, IPunObservable
{
    public float duration = 10;
    [Tooltip("Starting part of the day")] public Daypart partOfTheDay = Daypart.Day;


    [Header("UI stuff")]
    public Animator nightPanelAnimator;
    public Image countdownImage;
    public Image countdownImageDay;
    public Image countdownImageNight;


    public AudioSource dayAudio;
    public AudioSource nightAudio;

    private float time = 10.0f;
    private bool change = false;

    // Start is called before the first frame update
    void Start()
    {
        photonView.RPC("SetupNewTimeOfTheDay", RpcTarget.AllBuffered, partOfTheDay);
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            time -= Time.deltaTime;
            
            if (time <= 0.0f)
            {
                partOfTheDay = (partOfTheDay == Daypart.Day) ? Daypart.Night : Daypart.Day;
                photonView.RPC("SetupNewTimeOfTheDay", RpcTarget.AllBuffered, partOfTheDay);
            }

        }
        countdownImage.fillAmount = time / duration;
        countdownImageDay.fillAmount = time / duration;
        countdownImageNight.fillAmount = time / duration;
        
    }

    [PunRPC]
    private void SetupNewTimeOfTheDay(Daypart newPartOfTheDay)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            time = duration;
        }
        if (newPartOfTheDay == Daypart.Day)
        {
            nightPanelAnimator.SetBool("IsDay", true);
            playDayAudio();
            Empower(PlayerType.Live);
            countdownImage.color = Color.yellow;
            countdownImageDay.gameObject.SetActive(true);
            countdownImageNight.gameObject.SetActive(false);
            countdownImageDay.fillAmount = 1.0f;
        }
        else
        {
            nightPanelAnimator.SetBool("IsDay", false);
            playNightAudio();
            Empower(PlayerType.Evil);
            countdownImage.color = new Color(0.5f, 0f, 0.5f);
            countdownImageDay.gameObject.SetActive(false);
            countdownImageNight.gameObject.SetActive(true);
            countdownImageNight.fillAmount = 1.0f;
        }

        countdownImage.fillAmount = 1.0f;

    }

    public void Empower(PlayerType type)
    {
        foreach (Tower t in FindObjectsOfType<Tower>()){
            if (t.owner != PlayerType.None)
            {
                if (t.owner == type)
                {
                    t.Empower();
                }
                else
                {
                    t.PowerDown();
                }
            }
        }
    }

    public void playDayAudio()
    {
        nightAudio.Stop();
        dayAudio.Play();
    }

    public void playNightAudio()
    {
        dayAudio.Stop();
        nightAudio.Play();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(time);
            stream.SendNext(duration);
        }
        else
        {
            time = (float) stream.ReceiveNext();
            duration = (float) stream.ReceiveNext();
        }
    }
}