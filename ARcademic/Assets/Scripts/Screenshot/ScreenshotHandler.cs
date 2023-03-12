using UnityEngine.Networking;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ScreenshotHandler : MonoBehaviour
{

    UnityEngine.Windows.WebCam.PhotoCapture photoCaptureObject = null;
    Texture2D targetTexture = null;    
    Resolution cameraResolution;
    [SerializeField]
    Material unlitMaterial;
    // [SerializeField]
    // TesseractDemoScript tess;
    [SerializeField]
    RawImage outputImage;
    [SerializeField] 
    private Texture2D imageToRecognize;


    // Use this for initialization

    public void TakePicture()
    {
        StartProcess();
    }

    private void StartProcess()
    {
        cameraResolution = UnityEngine.Windows.WebCam.PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);

        // Create a PhotoCapture object
        UnityEngine.Windows.WebCam.PhotoCapture.CreateAsync(false, delegate (UnityEngine.Windows.WebCam.PhotoCapture captureObject)
        {
            photoCaptureObject = captureObject;
            UnityEngine.Windows.WebCam.CameraParameters cameraParameters = new UnityEngine.Windows.WebCam.CameraParameters();
            cameraParameters.hologramOpacity = 0.0f;
            cameraParameters.cameraResolutionWidth = cameraResolution.width;
            cameraParameters.cameraResolutionHeight = cameraResolution.height;
            cameraParameters.pixelFormat = UnityEngine.Windows.WebCam.CapturePixelFormat.BGRA32;

            // Activate the camera
            photoCaptureObject.StartPhotoModeAsync(cameraParameters, delegate (UnityEngine.Windows.WebCam.PhotoCapture.PhotoCaptureResult result)
            {
                // Take a picture
                photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
            });
        });
    }

    private void OnCapturedPhotoToMemory(UnityEngine.Windows.WebCam.PhotoCapture.PhotoCaptureResult result, UnityEngine.Windows.WebCam.PhotoCaptureFrame photoCaptureFrame)
    {
        // Cleans up existing image(s)
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Copy the raw image data into our target texture
        photoCaptureFrame.UploadImageDataToTexture(targetTexture);

        // Create a gameobject that we can apply our texture to
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Renderer quadRenderer = quad.GetComponent<Renderer>() as Renderer;
        quadRenderer.material = new Material(unlitMaterial);

        quad.transform.parent = this.transform;
        quad.transform.localPosition = new Vector3(0.0f, 0.0f, 3.0f);
        quad.name = "ImageTest";

        quadRenderer.material.SetTexture("_MainTex", targetTexture);

        StartCoroutine(StartPNG());

        // Deactivate our camera
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
    }

    private IEnumerator StartPNG()
    {
        yield return UploadPNG(targetTexture);
    }

    private void OnStoppedPhotoMode(UnityEngine.Windows.WebCam.PhotoCapture.PhotoCaptureResult result)
    {
        // Shutdown our photo capture resource
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }

    private IEnumerator UploadPNG(Texture2D tex)
    {
        // We should only read the screen buffer after rendering is complete
        yield return new WaitForEndOfFrame();

        // Encode texture into PNG
        //byte[] bytes = tex.EncodeToPNG();
        //Object.Destroy(tex);

        /*tess.*/SetImageToRecognize(targetTexture);

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

