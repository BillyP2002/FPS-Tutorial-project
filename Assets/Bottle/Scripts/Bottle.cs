using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    public List<Rigidbody> AllParts = new List<Rigidbody>();

    public void Shatter()
    {
        foreach (Rigidbody part in AllParts)
        {
            part.isKinematic = false;
        }
    }
}
