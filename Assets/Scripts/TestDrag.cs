using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDrag : MonoBehaviour
{
    public GameObject LineRenderer;
    public GameObject OtherObject;
    string Tag = "";
    // Start is called before the first frame update
    void Start()
    {
        Tag = gameObject.tag;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseDrag()
    {
        Vector3 endPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        endPosition.z = 0;
        if(Vector3.Distance(OtherObject.GetComponent<Transform>().position, endPosition) < .5)
        {
            endPosition = OtherObject.GetComponent<Transform>().position;
        }
        LineRenderer.GetComponent<LineRenderer>().SetPositions(new Vector3[] { gameObject.GetComponent<Transform>().position,  endPosition});
        Debug.Log(Tag + " : " + Input.mousePosition.ToString());
    }
}
