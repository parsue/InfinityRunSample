using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static GameState State { get; private set; }

    [SerializeField] private GrowUpProfile growProfile;
    [SerializeField] private TextMeshProUGUI infoMsg;
    [SerializeField] private GameObject structures;
    [SerializeField] private TextMeshProUGUI distance;
    [SerializeField] private TextMeshProUGUI topDistance;
    [SerializeField] private TextMeshProUGUI coin;
    [SerializeField] private TextMeshProUGUI topCoin;
    [Range(1, 5f)] public float trackWidth = 3f;

    [SerializeField] [Range(0, 10)] private float delayGrowTime = 5f;
    public static float playerSpeed;
    public static float spawnRatio;
    private string TopDistance => string.Format("{0:N3}m", DataCenter.SaveData.distance);
    private string RunDistance => string.Format("{0:N0}m", DataCenter.LiveData.distance);
    private string TopCoin => string.Format("{0}", DataCenter.SaveData.coin);
    private string RunCoin => string.Format("{0}", DataCenter.LiveData.coin);

    private Coroutine infoMsgCorou;
    private bool distanceRecordBroken, coinRecordBroken;

    private void Awake()
    {
        Instance = this;
        playerSpeed = 0f;
        spawnRatio = 0f;
        distanceRecordBroken = false;
        coinRecordBroken = false;
        infoMsgCorou = null;
        State = GameState.Menu;
    }

    private void Start()
    {
        StartCoroutine(MenuProcedure());
    }

    private IEnumerator MenuProcedure()
    {
        yield return new WaitUntil(() => DataCenter.ready);
        DataCenter.ResetLiveGameData();
        structures.SetActive(true);
        infoMsg.text = string.Empty;
        distance.text = RunDistance;
        topDistance.text = TopDistance;
        coin.text = RunCoin;
        topCoin.text = TopCoin;
        yield return new WaitUntil(() => State != GameState.Menu);
        yield return StartCoroutine(ReadyProcedure());
    }

    private IEnumerator ReadyProcedure()
    {
        structures.SetActive(false);
        infoMsg.text = "3";
        yield return new WaitForSeconds(0.8f);
        infoMsg.text = "2";
        yield return new WaitForSeconds(0.8f);
        infoMsg.text = "1";
        yield return new WaitForSeconds(0.8f);
        infoMsg.text = "Start!";
        yield return new WaitForSeconds(0.5f);
        infoMsg.text = string.Empty;

        State = GameState.Run;
        StartCoroutine(RecordProcedure());
        StartCoroutine(GrowProcedure());
    }

    private IEnumerator GrowProcedure()
    {
        yield return new WaitForSeconds(delayGrowTime);

        while (State == GameState.Run)
        {
            playerSpeed += growProfile.playerGrowRatio * Time.deltaTime;
            spawnRatio += growProfile.spawnGrowRatio * Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator RecordProcedure()
    {
        while (State == GameState.Run)
        {
            distance.text = RunDistance;
            if (!distanceRecordBroken && DataCenter.LiveData.distance > DataCenter.SaveData.distance && DataCenter.SaveData.distance != 0)
                BreakDistanceRecord();
            yield return null;
        }
    }

    public static void GetCoin()
    {
        DataCenter.LiveData.coin++;
        Instance.coin.text = Instance.RunCoin;
        if (!Instance.coinRecordBroken && DataCenter.LiveData.coin > DataCenter.SaveData.coin && DataCenter.SaveData.coin != 0)
            Instance.BreakCoinRecord();
    }

    private void BreakDistanceRecord()
    {
        distanceRecordBroken = true;
        if (infoMsgCorou != null) StopCoroutine(infoMsgCorou);
        infoMsgCorou = StartCoroutine(ShowInfoMsg("New Distance Record!!!"));
    }

    private void BreakCoinRecord()
    {
        coinRecordBroken = true;
        if (infoMsgCorou != null) StopCoroutine(infoMsgCorou);
        infoMsgCorou = StartCoroutine(ShowInfoMsg("New Coin Record!!!"));
    }

    private IEnumerator ShowInfoMsg(string msg)
    {
        infoMsg.text = msg;
        yield return new WaitForSeconds(3f);
        infoMsg.text = string.Empty;
    }

    public static void GameStart()
    {
        State = GameState.Ready;
    }

    public static void PlayerDead()
    {
        State = GameState.End;
        Instance.infoMsg.text = "You are dead!\nPress Space To Restart¡K";
        DataCenter.SaveGameData();
        Instance.topDistance.text = Instance.TopDistance;
        Instance.topCoin.text = Instance.TopCoin;
    }

    public static void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
