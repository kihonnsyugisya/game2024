using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    [SerializeField] private Button okButton;
    // Start is called before the first frame update
    void Start()
    {
        okButton.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromMilliseconds(2000))
            .TakeUntilDestroy(this)
            .Subscribe(_ => SceneManager.LoadScene("GameScene")).AddTo(this);
    }

}
