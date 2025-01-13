using System.Collections.Generic;
using Cinemachine;
using UniRx;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator animator; // Animator�R���|�[�l���g���Z�b�g
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List <AudioClip>  clips; // �Ō���
    [SerializeField] private AudioClip levelUpclip; // ���x���A�b�v
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
        if (Input.GetMouseButtonDown(0)) // �}�E�X���N���b�N�܂��̓^�b�v����
        {
            Punch(); // �p���`�����s
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Punch(); // �p���`�����s
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
        animator.Play(PunchStateName, 0, 0); // 0�t���[���ڂ���Đ�
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

        // �V�����l��ݒ�
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
