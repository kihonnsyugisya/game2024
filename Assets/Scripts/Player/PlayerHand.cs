using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private float punchPow;

    private Subject<Unit> onHit = new Subject<Unit>();
    public IObservable<Unit> OnHit => onHit; // �O���ɍw�ǉ\�ȃC���^�[�t�F�[�X�����J

    private void OnCollisionEnter(Collision collision)
    {
        onHit.OnNext(Unit.Default); // �q�b�g�ʒm
        AddForceOnHit(collision.rigidbody);
    }

    private void OnTriggerEnter(Collider other)
    {
        onHit.OnNext(Unit.Default); // �q�b�g�ʒm
        AddForceOnHit(other.attachedRigidbody);
    }

    private void AddForceOnHit(Rigidbody rb)
    {
        // ���݂̑��x���擾
        Vector3 currentVelocity = rb.velocity;

        // ���݂̑��x�x�N�g���̑傫�����v�Z
        float currentSpeed = currentVelocity.magnitude;

        // �V�������x�̑傫�����v�Z
        float newSpeed = currentSpeed * punchPow;

        // ������ێ����ĐV�������x��K�p
        rb.velocity = currentVelocity.normalized * newSpeed;

        //Debug.Log($"���x���ϓ��Ɋg��: ���̑��x: {currentSpeed}, �V�������x: {newSpeed}");
    }
}
