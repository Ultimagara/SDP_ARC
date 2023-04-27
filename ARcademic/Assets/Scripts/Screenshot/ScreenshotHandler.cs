using System.Diagnostics;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ScreenshotHandler : MonoBehaviour
{
    Texture2D targetTexture = null;    
    Resolution cameraResolution;
    [SerializeField]
    Material unlitMaterial;
    [SerializeField]
    TesseractDemoScript tess;
    [SerializeField]
    RawImage outputImage;
    [SerializeField]
    LiveWebcam liveWebcam;
    [SerializeField] 
    private Texture2D imageToRecognize;
    [SerializeField] 
    private Button takePictureButton;
    [SerializeField] 
    public string deviceName;
    [SerializeField] 
    WebCamTexture wct;


   private void StartImageCapture() 
   {
        
        // Create a gameobject that we can apply our texture to
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Renderer quadRenderer = quad.GetComponent<Renderer>() as Renderer;
        quadRenderer.material = new Material(unlitMaterial);

        // quad.transform.parent = this.transform;
        // quad.transform.localPosition = new Vector3(0.0f, 0.0f, 3.0f);
        // quad.name = "ImageTest";
        // quadRenderer.material.SetTexture("_MainTex", outputImage.texture);
        
        WebCamDevice[] devices = WebCamTexture.devices;
        deviceName = devices[0].name;
        UnityEngine.Debug.Log("Screenshot using " + deviceName);
        wct = liveWebcam.GetWebCamTexture();
        // quadRenderer.material.SetTexture("_MainTex", wct);

        targetTexture = new Texture2D(wct.width, wct.height);
        targetTexture.SetPixels(wct.GetPixels());
        targetTexture.Apply();

        wct.Stop();

        // targetTexture = (liveWebcam._rawImage.texture) as Texture2D;
        // quadRenderer.material.SetTexture("_MainTex", targetTexture);

   }

    public void TakePicture()
    {
        StartProcess();
    }

    private void StartProcess()
    {
        takePictureButton.interactable = false;
        if (!liveWebcam.isOn)
        {
            liveWebcam.ToggleCamera();
            takePictureButton.interactable = true;
            return;
        }
        
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        outputImage.texture = liveWebcam._rawImage.texture;
        StartImageCapture();
        liveWebcam.ToggleCamera();

        // Cleans up existing image(s)


        StartCoroutine(StartPNG());

        // Deactivate our camera
        takePictureButton.interactable = true;
    }

    private IEnumerator StartPNG()
    {
        yield return UploadPNG(targetTexture);
    }

    private IEnumerator UploadPNG(Texture2D tex)
    {
        yield return new WaitForEndOfFrame();

        tess.SetImageToRecognize(targetTexture);
    }

    public void SetImageToRecognize(Texture2D img)
    {
        imageToRecognize = img;
        SetImageDisplay();
    }

    private void SetImageDisplay()
    {
        Texture2D textureRes = new Texture2D(imageToRecognize.width, imageToRecognize.height, TextureFormat.ARGB32, false);
        textureRes.SetPixels32(imageToRecognize.GetPixels32());
        textureRes.Apply();

        outputImage.texture = textureRes;
    }

}

