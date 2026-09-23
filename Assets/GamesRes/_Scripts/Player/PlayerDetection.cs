using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public static Action<AppleTree> OnEnteredTreeZone;
    public static Action<AppleTree> OnExittedTreeZone;

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("ChunkTrigger"))
        {
            Chunk chunk = other.GetComponentInParent<Chunk>();

            chunk.TryUnlock();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out AppleTree appleTree))
            TriggerAppleTree(appleTree);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out AppleTree appleTree))
            ExitTriggerAppleTree(appleTree);
    }
    private void TriggerAppleTree(AppleTree appleTree) => OnEnteredTreeZone?.Invoke(appleTree);
    private void ExitTriggerAppleTree(AppleTree appleTree) => OnExittedTreeZone?.Invoke(appleTree);
}
