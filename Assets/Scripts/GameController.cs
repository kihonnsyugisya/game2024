using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<Target> targetPrefabs; // ターゲットのプレハブ
    [SerializeField] private List <AudioClip> clips; // リスポンの時関係の音
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Transform spawnPoint;    // スポーン位置
    private ObjectPool<GameObject> targetPool;        // オブジェクトプール
    public static float totalSpawn = 0;
    public static IntReactiveProperty score = new();
    public static int maxCombo = 0;
    [SerializeField] private Player player;
    private IntReactiveProperty comboCounter = new();
    [SerializeField] private ComboCanvas comboCanvas;
    [SerializeField] private int comboSpan = 5;

    private void Start()
    {
        totalSpawn = 0;
        score.Value = 0;
        maxCombo = 0;

        // オブジェクトプールの作成
        targetPool = new ObjectPool<GameObject>(
            createFunc: () => CreateTarget(),  // プレハブを生成
            actionOnGet: obj => obj.SetActive(true),      // プールから取得時
            actionOnRelease: obj => obj.SetActive(false), // プールに返却時
            actionOnDestroy: obj => Destroy(obj),         // 破棄時
            collectionCheck: true,                       // 重複チェック
            defaultCapacity: 10,                         // 初期サイズ
            maxSize: 10                                   // 最大サイズ
        );

        player.hand.OnHit.Subscribe(_ => {
            score.Value++;
            Debug.Log("score: " + score + " / " + totalSpawn);
        }).AddTo(this);
    }

    public void SpawnTarget()
    {
        // プールからターゲットを取得し、スポーン位置に配置
        GameObject target = targetPool.Get();
        target.transform.position = spawnPoint.position;
        totalSpawn++;
        PlaySe(Respown.normal);
    }

    public void DespawnTarget(GameObject target)
    {
        // ターゲットをプールに返却
        targetPool.Release(target);
    }

    private GameObject CreateTarget()
    {
        // ターゲットの生成
        Target target = Instantiate(targetPrefabs[0]);
        target.Initialize(this); // GameController を渡して初期化
        return target.gameObject;
    }


    private void PlaySe(Respown playerSE)
    {
        audioSource.PlayOneShot(clips[(int)playerSE]);
    }

    public void GotoResult()
    {
        SceneManager.LoadScene("ScoreScene");
    }

    public void PlusCombo()
    { 
        comboCounter.Value++;
        if (comboCounter.Value % comboSpan == 0)         
        {
            player.PowerUp();
            comboCanvas.ShowCombo(comboCounter.Value);
        }   
    }

    public void ResetCombo() 
    { 
        if(comboCounter.Value > maxCombo) maxCombo = comboCounter.Value;
        comboCounter.Value = 0;
        player.PowerReset();
    }

    public void SpawnMini()
    {
        // プールからターゲットを取得し、スポーン位置に配置
        GameObject target = targetPool.Get();
        target.transform.position = spawnPoint.position;

        // 現在のlocalScaleを取得
        Vector3 currentScale = target.transform.localScale;

        // すべての軸を半分にする
        target.transform.localScale = currentScale * 0.75f;

        totalSpawn++;
        PlaySe(Respown.normal);
    }



}
