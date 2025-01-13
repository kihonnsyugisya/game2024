using System.Collections.Generic;
using Cinemachine;
using UniRx;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator animator; // Animatorコンポーネントをセット
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List <AudioClip>  clips; // 打撃音
    [SerializeField] private AudioClip levelUpclip; // レベルアップ
    [SerializeField] private CinemachineImpulseSource impulseSource;
    [SerializeField] private GameObject statusEffect;
    [SerializeField] private GameObject punchEffect;
    [SerializeField] private Transform punchEffectPos;
    public PlayerHand hand;

    public static readonly string PunchStateName = "Boxer_Punch_1"; 
    public static readonly string HitStateName = "Boxer_Hit";

    private int status = 0;
    private PlayerSE currentPunchSE = PlayerSE.punchHitS;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // マウス左クリックまたはタップ入力
        {
            Punch(); // パンチを実行
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Punch(); // パンチを実行
        //}
    }

    private void Start()
    {
        hand.OnHit.Subscribe(_ => {
            PlaySe(currentPunchSE);
            impulseSource.GenerateImpulse();
            if (status > 2) 
            {
                ShowPunchEffect();
            }
        }).AddTo(this);
    }

    public void Punch()
    {
        //Debug.Log("Punch triggered");        
        animator.Play(PunchStateName, 0, 0); // 0フレーム目から再生
        PlaySe(PlayerSE.punch);
    }

    public void PlaySe(PlayerSE playerSE) 
    {        
        audioSource.PlayOneShot(clips[(int)playerSE]);
    }

    public void PowerUp()
    {
        audioSource.PlayOneShot(levelUpclip);
        Vector3 currentVelocity = impulseSource.m_DefaultVelocity;
        currentVelocity += new Vector3(0.2f, 0.2f, 0.2f);

        // 新しい値を設定
        impulseSource.m_DefaultVelocity = currentVelocity;

        Instantiate(statusEffect, gameObject.transform);

        status++;
        if (status == 1)
        {
            currentPunchSE = PlayerSE.punchHitM;
        }
        else if (status > 2)
        {
            currentPunchSE = PlayerSE.punchHitL;
        }
    }

    public void PowerReset()
    {
        impulseSource.m_DefaultVelocity = Vector3.zero;
        currentPunchSE = PlayerSE.punchHitS;
        status = 0;
    }

    private void ShowPunchEffect()
    { 
        Instantiate(punchEffect, punchEffectPos.position, punchEffectPos.rotation, gameObject.transform);
    }
}
