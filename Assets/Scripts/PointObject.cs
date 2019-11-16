using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointObject : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log(gameObject.GetComponent<Transform>().position.ToString());
        GameObject script = GameObject.Find("script");
        script.GetComponent<DrawLine>().OnPointClick(gameObject);
    }
}
