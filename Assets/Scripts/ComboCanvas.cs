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
    private Coroutine showComboCoroutine; // �R���[�`���̊Ǘ��p

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(camera.position);
    }

    public void ShowCombo(int comboValue)
    {
        // �\�����̃R���[�`�����~���čĊJ�n
        if (showComboCoroutine != null)
        {
            StopCoroutine(showComboCoroutine);
        }
        showComboCoroutine = StartCoroutine(ShowComboRoutine(comboValue));
    }

    private IEnumerator ShowComboRoutine(int comboValue)
    {
        // �R���{�l��ݒ�
        comboNum.text = comboValue.ToString();

        // �C���[�W�ƃe�L�X�g��\��
        combo.gameObject.SetActive(true);

        // 1.5�b�ҋ@
        yield return new WaitForSeconds(1.5f);

        // �C���[�W�ƃe�L�X�g���\��
        combo.gameObject.SetActive(false);
    }

}
