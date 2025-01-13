using System;
using System.Collections;
using UniRx;
using UnityEngine;

public class Target : MonoBehaviour
{
    private GameController gameController;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float requiredContactTime = 4.0f; // 接触が必要な時間
    private bool isHit = false;

    private void OnEnable()
    {
        StartCoroutine(WaitAndReset());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Hand")
        {
            gameController.PlusCombo();
            isHit = true;
            return;
        }

        if (isHit == false && collision.gameObject.tag == "Ground")
        {
            isHit = false;
            gameController.ResetCombo();
        }
    }

    public void Initialize(GameController controller)
    {
        gameController = controller;
    }

    private IEnumerator WaitAndReset()
    {
        yield return new WaitForSeconds(requiredContactTime); // 指定時間待つ
        Reset(); // 待機後にリセット処理を実行
    }

    private void Reset()
    {
        if (rigidbody != null)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
        isHit = false;
        transform.root.rotation = Quaternion.identity;
        StopAllCoroutines();
        gameController.DespawnTarget(gameObject);
    }
}
