using System;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public event EventHandler<OnEnterEventArgs> OnEnter;
    public class OnEnterEventArgs
    {
        public GameObject HitGameObject;
        public Transform HitGameObjectTransform;
    }

    private void OnTriggerEnter(Collider other)
    {
        OnEnter?.Invoke(this, new OnEnterEventArgs
        {
            HitGameObject = other.gameObject,
            HitGameObjectTransform = other.transform
        });
    }
}
