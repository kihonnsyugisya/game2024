using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Sample;

/// <summary>
/// リザルト画面の制御を行うクラス。
/// - スコアやトータル値の表示を初期化。
/// - リトライボタンやレビュー機能ボタンの動作を設定。
/// - インタースティシャル広告が閉じられた後にゲーム画面へ遷移。
/// </summary>
public class ResultController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText; // スコア表示用テキスト
    [SerializeField] private TextMeshProUGUI totalText; // トータル値表示用テキスト
    [SerializeField] private TextMeshProUGUI maxComboText; // 最大コンボ表示用テキスト
    [SerializeField] private Button reviewButton; // レビュー機能ボタン
    [SerializeField] private Button retryButton; // リトライボタン
    [SerializeField] private InterstitialAdController interstitialAdController; // 広告管理コントローラ

    private const int ButtonThrottleTimeMs = 2000; // ボタンの連続クリック防止時間 (ms)

    /// <summary>
    /// 初期化処理。
    /// - スコアとトータル値の表示を設定。
    /// - ボタンのクリックイベントを設定。
    /// - 広告閉鎖後の画面遷移を設定。
    /// </summary>
    void Start()
    {
        InitializeUI();
        ConfigureRetryButton();
        ConfigureReviewButton();
        ObserveAdClosed();
    }

    /// <summary>
    /// スコアとトータル値の表示を初期化。
    /// </summary>
    private void InitializeUI()
    {
        scoreText.text = GameController.score.ToString();
        totalText.text = GameController.totalSpawn.ToString();
        maxComboText.text = GameController.maxCombo.ToString();
    }

    /// <summary>
    /// リトライボタンのクリックイベントを設定。
    /// </summary>
    private void ConfigureRetryButton()
    {
        retryButton.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromMilliseconds(ButtonThrottleTimeMs))
            .TakeUntilDestroy(this)
            .Subscribe(_ => Retry());
    }

    /// <summary>
    /// レビュー機能ボタンのクリックイベントを設定。
    /// </summary>
    private void ConfigureReviewButton()
    {
        reviewButton.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromMilliseconds(ButtonThrottleTimeMs))
            .TakeUntilDestroy(this)
            .Subscribe(_ => StartCoroutine(InAppReviewManager.RequestReview()));
    }

    /// <summary>
    /// 広告が閉じられた後の画面遷移処理を設定。
    /// </summary>
    private void ObserveAdClosed()
    {
        interstitialAdController.isAdClosed
            .Where(value => value)
            .TakeUntilDestroy(this)
            .Subscribe(_ => SceneManager.LoadScene("TitleScene"));
    }

    /// <summary>
    /// リトライ処理。
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
