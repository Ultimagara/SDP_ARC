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


    // Use this for initialization

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
        outputImage.texture = liveWebcam._rawImage.texture;
        liveWebcam.ToggleCamera();

        // Cleans up existing image(s)
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Create a gameobject that we can apply our texture to
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Renderer quadRenderer = quad.GetComponent<Renderer>() as Renderer;
        quadRenderer.material = new Material(unlitMaterial);

        quad.transform.parent = this.transform;
        quad.transform.localPosition = new Vector3(0.0f, 0.0f, 3.0f);
        quad.name = "ImageTest";
        
        targetTexture = (Texture2D)liveWebcam._rawImage.mainTexture;

        quadRenderer.material.SetTexture("_MainTex", outputImage.texture);

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
        // We should only read the screen buffer after rendering is complete
        yield return new WaitForEndOfFrame();

        // Encode texture into PNG
        //byte[] bytes = tex.EncodeToPNG();
        //Object.Destroy(tex);

        tess.SetImageToRecognize(targetTexture);// AdaptiveThreshold.AdaptiveThreshhold(targetTexture, 2, .5));
        //SetImageToRecognize(targetTexture);

        // For testing purposes, also write to a file in the project folder
        //File.WriteAllBytes("C:/SavedScreen.png", bytes);

        /*
        // Create a Web Form
        WWWForm form = new WWWForm();
        form.AddField("frameCount", Time.frameCount.ToString());
        form.AddBinaryData("fileUpload", bytes);

        // Upload to a cgi script
        var w = UnityWebRequest.Post("http://localhost/cgi-bin/env.cgi?post", form);
        yield return w.SendWebRequest();

        if (w.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(w.error);
        }
        else
        {
            Debug.Log("Finished Uploading Screenshot");
        }
        */
    }

    public void SetImageToRecognize(Texture2D img)
    {
        imageToRecognize = img;// AdaptiveThreshold.AdaptiveThreshhold(img, 1, 1);
        //tess.SetImageToRecognize(imageToRecognize);
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

