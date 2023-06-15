using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Transform _progress;

    public void SetPercent (float value)
    {
        _progress.localScale = new Vector3(value / 100f, 1, 1);
    }

}
