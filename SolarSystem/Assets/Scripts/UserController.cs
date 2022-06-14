using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserController : MonoBehaviour
{

    private Text celestialName;
    private Text celestialMass;
    private Text[] texts;
    private Slider celestialSlider;
    private Celestial celestial;

    // Start is called before the first frame update
    void Start()
    {
        celestialSlider = FindObjectOfType<Slider>();
        texts = FindObjectsOfType<Text>();
        foreach (Text t in texts)
        {
            if (t.name == "celestialName") celestialName = t;
            else if (t.name == "celestialMass") celestialMass = t;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseClick();
        }
    }


    void OnMouseClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {              
            if (hit.transform.gameObject.GetComponent<Celestial>() != null)
            {
                celestialName.text = hit.transform.name;
                Debug.Log(celestialName.text);
                celestial = hit.transform.gameObject.GetComponent<Celestial>();
                celestialSlider.value = celestial.scaleFactor;
            }
        }
    }

    public void OnSliderChange()
    {
        if(celestial != null)
        {
            celestial.ScaleCelestial(celestialSlider.value);
            celestialMass.text = celestial.rb.mass.ToString();

        }
    }
}

