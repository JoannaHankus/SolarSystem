using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject objectCanvas;
        GameObject objectName;
        GameObject objectMass;
        GameObject slider;
        GameObject background;
        GameObject fillArea;
        GameObject fill;
        GameObject handleSlideArea;
        GameObject handle;
        GameObject userController;
        Canvas myCanvas;
        Text textName;
        Text textMass;
        RectTransform rectTransform_mass;
        RectTransform rectTransform_name;

        // Canvas
        objectCanvas = new GameObject();
        objectCanvas.layer = 5;

        objectCanvas.name = "TestCanvas";
        objectCanvas.AddComponent<Canvas>();
        objectCanvas.GetComponent<RectTransform>().position = new Vector3(0, 0, 0);

        myCanvas = objectCanvas.GetComponent<Canvas>();
        myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        objectCanvas.AddComponent<CanvasScaler>();
        objectCanvas.AddComponent<GraphicRaycaster>();

        // Text
        objectName = new GameObject();
        objectName.layer = 5;
        objectName.transform.SetParent(objectCanvas.transform);
        objectName.name = "celestialName";
        textName = objectName.AddComponent<Text>();
        textName.font = (Font)Resources.Load("Fonts/arial");
        textName.fontSize = 20;

        objectMass = new GameObject();
        objectMass.layer = 5;
        objectMass.transform.SetParent(objectCanvas.transform);
        objectMass.name = "celestialMass";
        textMass = objectMass.AddComponent<Text>();
        textMass.font = (Font)Resources.Load("Fonts/arial");

        textMass.fontSize = 20;

        rectTransform_name = textName.GetComponent<RectTransform>();
        rectTransform_name.localPosition = new Vector3(0, -74, 0);
        rectTransform_name.anchorMin = new Vector2(0.5f, 1);
        rectTransform_name.anchorMax = new Vector2(0.5f, 1);
        rectTransform_name.sizeDelta = new Vector2(160, 30);

        rectTransform_mass = textMass.GetComponent<RectTransform>();
        rectTransform_mass.localPosition = new Vector3(0, -106.3f, 0);
        rectTransform_mass.anchorMin = new Vector2(0.5f, 1);
        rectTransform_mass.anchorMax = new Vector2(0.5f, 1);
        rectTransform_mass.sizeDelta = new Vector2(160, 30);

        //Slider
        slider = new GameObject();
        slider.layer = 5;
        slider.transform.SetParent(objectCanvas.transform);
        slider.name = "Slider";
        slider.AddComponent<Slider>();
        var sliderRectTransform = slider.GetComponent<RectTransform>();
        sliderRectTransform.localPosition = new Vector3(0, 66, 0);
        sliderRectTransform.anchorMin = new Vector2(0.5f, 0);
        sliderRectTransform.anchorMax = new Vector2(0.5f, 0);
        sliderRectTransform.sizeDelta = new Vector2(160, 20);

        //Slider Background
        background = new GameObject();
        background.layer = 5;
        background.transform.SetParent(slider.transform);
        var backgroundRectTransform = background.AddComponent<RectTransform>();
        backgroundRectTransform.localPosition = new Vector3(0,0,0);
        backgroundRectTransform.anchorMin = new Vector2(0,0.25f);
        backgroundRectTransform.anchorMax = new Vector2(1,0.75f);
        backgroundRectTransform.sizeDelta = new Vector2(0,0);
        background.name = "Background";
        var background_image = background.AddComponent<Image>();
        background_image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Background.psd");
        background_image.type = Image.Type.Sliced;

        //Fill area
        fillArea = new GameObject();
        fillArea.transform.position = new Vector3(0, 0, 0);
        fillArea.layer = 5;
        fillArea.transform.SetParent(slider.transform);
        var fillAreaRectTransform = fillArea.AddComponent<RectTransform>();
        fillAreaRectTransform.localPosition = new Vector3(5,0,0);
        fillAreaRectTransform.anchorMin = new Vector2(0, 0.25f);
        fillAreaRectTransform.anchorMax = new Vector2(1, 0.75f);
        fillAreaRectTransform.sizeDelta = new Vector2(15, 0);
        fillArea.name = "FillArea";

        //Fill
        fill = new GameObject();
        fill.transform.position = new Vector3(0, 0, 0);
        fill.layer = 5;
        fill.transform.SetParent(fillArea.transform);
        fill.name = "Fill";
        var fill_image = fill.AddComponent<Image>();
        fill.GetComponent<RectTransform>().localPosition = new Vector3(-5, 0, 0);
        fill.GetComponent<RectTransform>().sizeDelta = new Vector2(-5, 0);
        fill_image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        fill_image.type = Image.Type.Sliced;

        //Handle Slide Area
        handleSlideArea = new GameObject();
        handleSlideArea.transform.position = new Vector3(0, 0, 0);
        handleSlideArea.layer = 5;
        handleSlideArea.transform.SetParent(slider.transform);
        var handleSlideRectTransform = handleSlideArea.AddComponent<RectTransform>();
        handleSlideRectTransform.localPosition = new Vector3(10, 0, 0);
        handleSlideRectTransform.anchorMin = new Vector2(0, 0);
        handleSlideRectTransform.anchorMax = new Vector2(1, 1);
        handleSlideRectTransform.sizeDelta = new Vector2(10, 0);
        handleSlideArea.name = "HandleSlideArea";

        //Handle
        handle = new GameObject();
        handle.transform.position = new Vector3(0, 0, 0);
        handle.layer = 5;
        handle.transform.SetParent(handleSlideArea.transform);
        handle.name = "Handle";
        var handle_image = handle.AddComponent<Image>();
        handle_image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
        var handleRectTransform = handle.GetComponent<RectTransform>();
        handleRectTransform.localPosition = new Vector3(0, 0, 0);
        handleRectTransform.anchorMin = new Vector2(0.08f, 0);
        handleRectTransform.anchorMax = new Vector2(0.08f, 1);
        handleRectTransform.sizeDelta = new Vector2(20, 0);

        //User Controller
        userController = new GameObject();
        userController.name = "UserController";
        userController.AddComponent<UserController>();

        //Slider setup
        var sliderComp = slider.GetComponent<Slider>();
        sliderComp.minValue = 0.2f;
        sliderComp.maxValue = 10f;
        sliderComp.targetGraphic = handle_image;
        sliderComp.fillRect = fill.GetComponent<RectTransform>();
        sliderComp.handleRect = handle.GetComponent<RectTransform>();
        sliderComp.direction = Slider.Direction.LeftToRight;
        sliderComp.onValueChanged.AddListener(delegate { userController.GetComponent<UserController>().OnSliderChange(); });



    }

}
