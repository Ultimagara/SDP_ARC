using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveWebcam : MonoBehaviour
{    
    [SerializeField]
    public UnityEngine.UI.RawImage _rawImage;
    [SerializeField]
    public bool isOn = true;
    [SerializeField]
    private WebCamTexture camTex = null;


    private void Start()
    {
        _rawImage.enabled = true;
        StartCamera();
    }

    private void StartCamera()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        for (int i = 0; i < devices.Length; i++)
        {
            print("Webcam available: " + devices[i].name);
        }

        WebCamTexture tex = new WebCamTexture(devices[0].name);
        camTex = tex;
        this._rawImage.texture = camTex;
        camTex.Play();
        isOn = true;
    }

    private void StopCamera()
    {
        this._rawImage.texture = null;
        camTex.Stop();
        camTex = null;
        isOn = false;
    }

    public void ToggleCamera()
    {
        if (isOn)
        {
            _rawImage.enabled = false;
            StopCamera();
        }
        else
        {
            _rawImage.enabled = true;
            StartCamera();
        }
    }


}
