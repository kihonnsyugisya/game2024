using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<Target> targetPrefabs; // �^�[�Q�b�g�̃v���n�u
    [SerializeField] private List <AudioClip> clips; // ���X�|���̎��֌W�̉�
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Transform spawnPoint;    // �X�|�[���ʒu
    private ObjectPool<GameObject> targetPool;        // �I�u�W�F�N�g�v�[��
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

        // �I�u�W�F�N�g�v�[���̍쐬
        targetPool = new ObjectPool<GameObject>(
            createFunc: () => CreateTarget(),  // �v���n�u�𐶐�
            actionOnGet: obj => obj.SetActive(true),      // �v�[������擾��
            actionOnRelease: obj => obj.SetActive(false), // �v�[���ɕԋp��
            actionOnDestroy: obj => Destroy(obj),         // �j����
            collectionCheck: true,                       // �d���`�F�b�N
            defaultCapacity: 10,                         // �����T�C�Y
            maxSize: 10                                   // �ő�T�C�Y
        );

        player.hand.OnHit.Subscribe(_ => {
            score.Value++;
            Debug.Log("score: " + score + " / " + totalSpawn);
        }).AddTo(this);
    }

    public void SpawnTarget()
    {
        // �v�[������^�[�Q�b�g���擾���A�X�|�[���ʒu�ɔz�u
        GameObject target = targetPool.Get();
        target.transform.position = spawnPoint.position;
        totalSpawn++;
        PlaySe(Respown.normal);
    }

    public void DespawnTarget(GameObject target)
    {
        // �^�[�Q�b�g���v�[���ɕԋp
        targetPool.Release(target);
    }

    private GameObject CreateTarget()
    {
        // �^�[�Q�b�g�̐���
        Target target = Instantiate(targetPrefabs[0]);
        target.Initialize(this); // GameController ��n���ď�����
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
        // �v�[������^�[�Q�b�g���擾���A�X�|�[���ʒu�ɔz�u
        GameObject target = targetPool.Get();
        target.transform.position = spawnPoint.position;

        // ���݂�localScale���擾
        Vector3 currentScale = target.transform.localScale;

        // ���ׂĂ̎��𔼕��ɂ���
        target.transform.localScale = currentScale * 0.75f;

        totalSpawn++;
        PlaySe(Respown.normal);
    }



}
