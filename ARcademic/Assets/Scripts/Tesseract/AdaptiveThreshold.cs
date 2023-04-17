using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class AdaptiveThreshold
{ 
    public static Texture2D AdaptiveThreshhold(Texture2D OriginalImage, double Scale, double range)
    {
        float timer = Time.unscaledTime;
        int width = OriginalImage.width;
        int height = OriginalImage.height;

        Texture2D NewImage = new Texture2D(width, height, TextureFormat.ARGB32, false);
        int bytes = 32 * height;

        byte[] buffer = new byte[bytes];
        byte[] result = new byte[bytes];

        double GlobalMean = 0;

        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                float Grey = OriginalImage.GetPixel(x, y).grayscale;
                NewImage.SetPixel(x, y, new Color(Grey,Grey,Grey,Grey));
                GlobalMean += Grey;
            }
        }
        Debug.Log("Image GreyScaled\nTook" + ( Time.unscaledTime - timer).ToString());
        timer = Time.unscaledTime;
        //return NewImage;
        
        GlobalMean /= (width * height);
        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                double[] histogram = new double[256];
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        histogram[(int) OriginalImage.GetPixel(x+i,y+j).grayscale]++;
                    }
                }
                histogram = histogram.Select(l => l / (width * height)).ToArray();
                double mean = 0;
                for(int i = 0; i < 256; i++)
                {
                    mean += i * histogram[i];
                }

                double StandardDeviation = 0;
                for(int i = 0; i < 256; i++)
                {
                    StandardDeviation += Mathf.Pow((float) (i - mean), 2) * histogram[i];
                }
                StandardDeviation = Mathf.Sqrt((float) StandardDeviation);
                double threshold = Scale * StandardDeviation + range * GlobalMean;
                Color P = OriginalImage.GetPixel(x, y).grayscale > threshold ? Color.white : Color.black;
                NewImage.SetPixel(x, y, P);
            }
        }
        Debug.Log("Finished Photo in " + (Time.unscaledTime- timer).ToString());
        return NewImage;
    }


}
