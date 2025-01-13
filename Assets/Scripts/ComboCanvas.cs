using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboCanvas : MonoBehaviour
{
    [SerializeField] private Transform camera;
    [SerializeField] private Image combo;
    [SerializeField] private TextMeshProUGUI comboNum;
    private Coroutine showComboCoroutine; // コルーチンの管理用

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(camera.position);
    }

    public void ShowCombo(int comboValue)
    {
        // 表示中のコルーチンを停止して再開始
        if (showComboCoroutine != null)
        {
            StopCoroutine(showComboCoroutine);
        }
        showComboCoroutine = StartCoroutine(ShowComboRoutine(comboValue));
    }

    private IEnumerator ShowComboRoutine(int comboValue)
    {
        // コンボ値を設定
        comboNum.text = comboValue.ToString();

        // イメージとテキストを表示
        combo.gameObject.SetActive(true);

        // 1.5秒待機
        yield return new WaitForSeconds(1.5f);

        // イメージとテキストを非表示
        combo.gameObject.SetActive(false);
    }

}
