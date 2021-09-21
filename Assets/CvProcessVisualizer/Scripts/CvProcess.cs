using System;
using System.Collections.Generic;
using System.Linq;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using UnityEngine;

namespace CvProcessViewer
{
    public class CvProcess : MonoBehaviour
    {
        public List<Texture2D> _textures;

        public static CvProcess I { get; private set; }

        public void Log(Texture2D tex, string message = null, Scalar color = null)
        {
            if (!string.IsNullOrEmpty(message))
            {
                var mat = new Mat(tex.height, tex.width, CvType.CV_8UC3);
                Utils.texture2DToMat(tex, mat);
                Log(mat, true, message, color);
                return;
            }

            _textures.Add(tex);
        }

        public void Log(Mat mat, bool flip = false, string message = null, Scalar color = null)
        {
            var process = mat.clone();
            if (!string.IsNullOrEmpty(message))
            {
                if (color == null) color = new Scalar(0, 0, 0, 255);
                Imgproc.putText(process, message, new Point(10, 30), Imgproc.FONT_HERSHEY_SIMPLEX, 1, color);
            }

            var tex = new Texture2D(process.cols(), process.rows(), TextureFormat.RGBA32, false);
            OpenCVForUnity.UnityUtils.Utils.matToTexture2D(process, tex, flip);
            Log(tex);
        }


        private void Awake()
        {
            if (I == null)
            {
                I = this;
            }
            else
            {
                Destroy(this);
            }
        }
    }
}