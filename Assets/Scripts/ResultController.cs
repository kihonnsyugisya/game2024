using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Sample;

/// <summary>
/// ���U���g��ʂ̐�����s���N���X�B
/// - �X�R�A��g�[�^���l�̕\�����������B
/// - ���g���C�{�^���⃌�r���[�@�\�{�^���̓����ݒ�B
/// - �C���^�[�X�e�B�V�����L��������ꂽ��ɃQ�[����ʂ֑J�ځB
/// </summary>
public class ResultController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText; // �X�R�A�\���p�e�L�X�g
    [SerializeField] private TextMeshProUGUI totalText; // �g�[�^���l�\���p�e�L�X�g
    [SerializeField] private TextMeshProUGUI maxComboText; // �ő�R���{�\���p�e�L�X�g
    [SerializeField] private Button reviewButton; // ���r���[�@�\�{�^��
    [SerializeField] private Button retryButton; // ���g���C�{�^��
    [SerializeField] private InterstitialAdController interstitialAdController; // �L���Ǘ��R���g���[��

    private const int ButtonThrottleTimeMs = 2000; // �{�^���̘A���N���b�N�h�~���� (ms)

    /// <summary>
    /// �����������B
    /// - �X�R�A�ƃg�[�^���l�̕\����ݒ�B
    /// - �{�^���̃N���b�N�C�x���g��ݒ�B
    /// - �L������̉�ʑJ�ڂ�ݒ�B
    /// </summary>
    void Start()
    {
        InitializeUI();
        ConfigureRetryButton();
        ConfigureReviewButton();
        ObserveAdClosed();
    }

    /// <summary>
    /// �X�R�A�ƃg�[�^���l�̕\�����������B
    /// </summary>
    private void InitializeUI()
    {
        scoreText.text = GameController.score.ToString();
        totalText.text = GameController.totalSpawn.ToString();
        maxComboText.text = GameController.maxCombo.ToString();
    }

    /// <summary>
    /// ���g���C�{�^���̃N���b�N�C�x���g��ݒ�B
    /// </summary>
    private void ConfigureRetryButton()
    {
        retryButton.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromMilliseconds(ButtonThrottleTimeMs))
            .TakeUntilDestroy(this)
            .Subscribe(_ => Retry());
    }

    /// <summary>
    /// ���r���[�@�\�{�^���̃N���b�N�C�x���g��ݒ�B
    /// </summary>
    private void ConfigureReviewButton()
    {
        reviewButton.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromMilliseconds(ButtonThrottleTimeMs))
            .TakeUntilDestroy(this)
            .Subscribe(_ => StartCoroutine(InAppReviewManager.RequestReview()));
    }

    /// <summary>
    /// �L��������ꂽ��̉�ʑJ�ڏ�����ݒ�B
    /// </summary>
    private void ObserveAdClosed()
    {
        interstitialAdController.isAdClosed
            .Where(value => value)
            .TakeUntilDestroy(this)
            .Subscribe(_ => SceneManager.LoadScene("TitleScene"));
    }

    /// <summary>
    /// ���g���C�����B
    /// </summary>
    private void Retry()
    {
        if (!interstitialAdController.isSkipAd)
        {
            interstitialAdController.ShowAd();
        }
        else
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}
