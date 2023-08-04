using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace UnstackerFix.MarkerScript
{
    public class SearchMarker
    {
        private Image ImgScreen { get; }
        private Image ImgMarker { get; }
        private Bitmap ResultImageBitmap { get; set; }

        public SearchMarker(Image imgScreen, Image imgMarker)
        {
            ImgScreen = imgScreen;
            ImgMarker = imgMarker;
        }
        
        public Bitmap MarkerSearchMethod()
        {
            try
            {
                if (ImgScreen != null || ImgMarker != null)
                {
                    var imgScena = new Bitmap(ImgScreen).ToImage<Bgr, byte>();
                    var imgMaska = new Bitmap(ImgMarker).ToImage<Bgr, byte>();

                    var imgout = new Mat();

                    CvInvoke.MatchTemplate(imgScena, imgMaska, imgout, TemplateMatchingType.CcoeffNormed);

                    var minVal = 0.0;
                    var maxVal = 0.0;
                    var minLoc = new Point();
                    var maxLoc = new Point();

                    CvInvoke.MinMaxLoc(imgout, ref minVal, ref maxVal, ref minLoc, ref maxLoc);
                    if (maxVal < 0.9)
                    {
                        ResultImageBitmap = null;
                        return null;
                    }



                    var r = new Rectangle(maxLoc, imgMaska.Size);
                    CvInvoke.Rectangle(imgScena, r, new MCvScalar(124, 252, 0), 1);

                    ResultImageBitmap = imgScena.AsBitmap();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка на поиске маркера - {e.Message}");
            }

            return ResultImageBitmap;
        }
    }
}