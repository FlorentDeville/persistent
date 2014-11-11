using UnityEngine;
using System.Collections;

namespace CinemaDirector
{
    public class ScreenshotCapture : MonoBehaviour
    {
        public string Folder = "CaptureOutput";
        public int FrameRate = 24;

        void Start()
        {
            Time.captureFramerate = FrameRate;
            System.IO.Directory.CreateDirectory(Folder);
        }

        void Update()
        {
            string name = string.Format("{0}/shot {1:D04}.png", Folder, Time.frameCount);

            Application.CaptureScreenshot(name);
        }
    }
}