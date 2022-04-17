using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImportantInfoController : MonoBehaviour
{
    Image info;
    Text infoText;
    // Start is called before the first frame update
    void Start()
    {
        info = GetComponent<Image>();
        

    }

    // Update is called once per frame
    void Update()
    {
        //
    }

    public void InitText(string text)
    {
        infoText.text = text;
    }
}
