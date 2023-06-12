using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static GameObject[] ReturnChildrenGos(Transform parent)
    {
        int lengthOfArr = parent.childCount;
        GameObject[] toReturn = new GameObject[lengthOfArr];

        for (int i = 0; i<lengthOfArr; i++)
        {
            toReturn[i] = parent.GetChild(i).gameObject;
        }

        return toReturn;
    }
}
