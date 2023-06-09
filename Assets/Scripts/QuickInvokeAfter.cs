using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class QuickInvokeAfter : MonoBehaviour
{
    [SerializeField] private UnityEvent action;
    [SerializeField] private GameObject[] instantiateOnAction;
    [SerializeField] private UnityEvent subAction;
    [SerializeField] private GameObject[] instantiateOnSubAction;
    [SerializeField] private bool destroyOnDisable;

    public void CallAction()
    {
        InstantiateGameObjects(instantiateOnAction);
        action.Invoke();
    }

    public void CallSubAction()
    {
        InstantiateGameObjects(instantiateOnSubAction);
        subAction.Invoke();
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

