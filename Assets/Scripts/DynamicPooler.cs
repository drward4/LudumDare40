using UnityEngine;
using System;
using System.Collections.Generic;

public class DynamicPooler<T> where T : MonoBehaviour
{
    protected List<T> ActiveObjects;
    protected List<T> InactiveObjects;

    public T ObjectSpawner;
    public Transform ParentTransform;

    public DynamicPooler(T objectSpawner)
        : this(objectSpawner, 0)
    {
    }

    public DynamicPooler(T objectSpawner, int capacity)
    {
        if (objectSpawner == null)
        {
            throw new ArgumentException("Spawner cannot be null");
        }

        this.ObjectSpawner = objectSpawner;
        this.ActiveObjects = new List<T>(capacity);
        this.InactiveObjects = new List<T>(capacity);
    }


    protected void AddInstance()
    {
        T clone = (T)GameObject.Instantiate(this.ObjectSpawner);
        if (this.ParentTransform != null)
        {
            clone.transform.SetParent(this.ParentTransform, false);
        }
        this.InactiveObjects.Add(clone);
    }


    public T ActivateNext()
    {
        if (this.InactiveObjects.Count == 0)
        {
            this.AddInstance();
        }
        T ret = this.InactiveObjects[0];
        if (this.ParentTransform != null)
        {
            ret.transform.SetParent(this.ParentTransform, false);
        }
        this.InactiveObjects.RemoveAt(0);
        this.ActiveObjects.Add(ret);
        ret.gameObject.SetActive(true);

        return ret;
    }


    public void Deactivate(T objectToDeactivate)
    {
        // Will ignore an invalid request to deactivate an object if already inactive, but not request on object that
        // was not created by the pooler.
        if (!this.ActiveObjects.Contains(objectToDeactivate))
        {
            if (!this.InactiveObjects.Contains(objectToDeactivate))
            {
                Debug.LogError("Object was not created by pooler and cannot be deactivated.");
            }
        }
        else
        {
            objectToDeactivate.gameObject.SetActive(false);
            this.ActiveObjects.Remove(objectToDeactivate);
            this.InactiveObjects.Add(objectToDeactivate);
        }
    }


    public void DeactivateAll()
    {
        T[] items = this.ActiveObjects.ToArray();

        foreach (T item in items)
        {
            this.Deactivate(item);
        }
    }
}
