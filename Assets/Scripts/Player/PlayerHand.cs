using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private float punchPow;

    private Subject<Unit> onHit = new Subject<Unit>();
    public IObservable<Unit> OnHit => onHit; // 外部に購読可能なインターフェースを公開

    private void OnCollisionEnter(Collision collision)
    {
        onHit.OnNext(Unit.Default); // ヒット通知
        AddForceOnHit(collision.rigidbody);
    }

    private void OnTriggerEnter(Collider other)
    {
        onHit.OnNext(Unit.Default); // ヒット通知
        AddForceOnHit(other.attachedRigidbody);
    }

    private void AddForceOnHit(Rigidbody rb)
    {
        // 現在の速度を取得
        Vector3 currentVelocity = rb.velocity;

        // 現在の速度ベクトルの大きさを計算
        float currentSpeed = currentVelocity.magnitude;

        // 新しい速度の大きさを計算
        float newSpeed = currentSpeed * punchPow;

        // 方向を保持して新しい速度を適用
        rb.velocity = currentVelocity.normalized * newSpeed;

        //Debug.Log($"速度を均等に拡大: 元の速度: {currentSpeed}, 新しい速度: {newSpeed}");
    }
}
