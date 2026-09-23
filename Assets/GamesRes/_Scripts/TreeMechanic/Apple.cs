using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Apple : MonoBehaviour
{
    enum State { Ready, Growing}

    [Header("Elements")]
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    [SerializeField] private Renderer renderer;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword 
    private Rigidbody rb;
    private State state;

    [Header("Settings")]
    [SerializeField] private float shakeMultiplier;
    private Vector3 initialPos;
    private Quaternion initialRot;
    //[SerializeField] private bool hasBeenCollected;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        initialPos = transform.position;
        initialRot = transform.rotation;
    }
    private void Start()
    {
        state = State.Ready;
    }
    //private void Update()
    //{
    //    Debug.Log(state);
    //}
    //private void OnTriggerEnter(Collider other)
    //{
    //    //if (hasBeenCollected) return;

    //    if (!other.GetComponent<Player>())
    //    {
    //        Debug.Log(other.gameObject.name);
    //        return;
    //    }

    //    Debug.Log("Collider called");
    //    //hasBeenCollected = true;

    //    InventoryManager.Instance.AddCrop(CropType.Apple);
    //    Destroy(gameObject);
    //}
    public void Shake(float magnitude)
    {
        float realShakeMagnitude = magnitude * shakeMultiplier;

        renderer.material.SetFloat("_Magnitude", realShakeMagnitude);
    }
    public void DropApple()
    {
        rb.isKinematic = false;

        state = State.Growing;

        renderer.material.SetFloat("_Magnitude", 0);
    }
    public void Reset() => LeanTween.scale(gameObject, Vector3.zero, 1).setDelay(2).setOnComplete(ForceReset);
    private void ForceReset()
    {
        transform.position = initialPos;
        transform.rotation = initialRot;

        rb.isKinematic = true;

        // Scale up

        float randomScale = UnityEngine.Random.Range(5f, 10f);
        LeanTween.scale(gameObject, Vector3.one, randomScale).setOnComplete(SetReady);
    }
    private void SetReady()
    {
        state = State.Ready;
    }
    public bool IsDropped() => !rb.isKinematic;
    public bool IsReady() => state == State.Ready;
}
