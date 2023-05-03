using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvokeAfter : MonoBehaviour
{
    [SerializeField] private UnityEvent action;
    [SerializeField] private UnityEvent subAction;
    [SerializeField] private float timeToAction;
    [SerializeField] private float timeToSubAction;
    [SerializeField] private GameObject[] instantiateOnAction;
    [SerializeField] private GameObject[] instantiateOnSubAction;
    [SerializeField] private bool destroyOnDisable = true;

    private void Start()
    {
        if (timeToAction > 0)
        {
            StartCoroutine("InvokeAfterSeconds");
        }
    }

    private IEnumerator InvokeAfterSeconds()
    {
        yield return new WaitForSeconds(timeToAction);
        InstantiateGameObjects(instantiateOnAction);
        action.Invoke();
    }

    private IEnumerator ActionSubAction()
    {
        InstantiateGameObjects(instantiateOnAction);
        action.Invoke();
        yield return new WaitForSeconds(timeToSubAction);
        InstantiateGameObjects(instantiateOnSubAction);
        subAction.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollisionAction();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CollisionAction();
        }
    }

    private void CollisionAction()
    {
        if (timeToSubAction <= 0)
        {
            InstantiateGameObjects(instantiateOnAction);
            action.Invoke();
        }
        else
        {
            StartCoroutine("ActionSubAction");
        }
    }

    private void InstantiateGameObjects(GameObject[] instantiate)
    {
        if (instantiate.Length > 0)
        {
            foreach (GameObject obj in instantiate) 
            {
                Instantiate(obj, transform.position, transform.rotation);
            }
        }
    }

    private void OnDisable()
    {
        if (destroyOnDisable) Destroy(gameObject);
    }
}
