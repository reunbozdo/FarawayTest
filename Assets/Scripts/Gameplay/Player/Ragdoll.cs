using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private GameObject floor;
    [SerializeField] private Animator animator;
    [Space]
    [SerializeField] private List<Rigidbody> rbs;
    [SerializeField] private List<Collider> colliders;

    [Button("EnableRagdoll")]
    public void EnableRagdoll()
    {
        animator.enabled = false;
        floor.SetActive(true);

        colliders.ForEach(x => x.isTrigger = false);
        rbs.ForEach(x => x.isKinematic = false);
    }

    [Button("DisableRagdoll")]
    public void DisableRagdoll()
    {
        animator.enabled = true;
        floor.SetActive(false);

        colliders.ForEach(x => x.isTrigger = true);
        rbs.ForEach(x => x.isKinematic = true);
    }

    [Button("Fill")]
    public void Fill()
    {
        rbs = GetComponentsInChildren<Rigidbody>().ToList();
        colliders = GetComponentsInChildren<Collider>().ToList();
    }
}
