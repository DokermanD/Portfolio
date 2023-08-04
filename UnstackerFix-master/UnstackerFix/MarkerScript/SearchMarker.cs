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
        private readonly Dictionary<string, Image<Bgr, byte>> _imgList;
        private PictureBox ImgScreen { get; }
        private PictureBox ImgMarker { get; }
        private Bitmap ResultImageBitmap { get; set; }

        public SearchMarker(PictureBox imgScreen, PictureBox imgMarker)
        {
            ImgScreen = imgScreen;
            ImgMarker = imgMarker;
            _imgList = new Dictionary<string, Image<Bgr, byte>>();
            _imgList.Add("marker", new Bitmap(ImgScreen.Image).ToImage<Bgr, byte>());
        }
        
        public Bitmap MarkerSearchMethod()
        {
            try
            {


                if (ImgScreen.Image != null || ImgMarker != null)
                {
                    var imgScena = _imgList["marker"].Clone();
                    var imgMaska = new Bitmap(ImgMarker.Image).ToImage<Bgr, byte>();

                    var imgout = new Mat();

                    CvInvoke.MatchTemplate(imgScena, imgMaska, imgout, TemplateMatchingType.CcoeffNormed);

                    var minVal = 0.0;
                    var maxVal = 0.0;
                    var minLoc = new Point();
                    var maxLoc = new Point();

                    CvInvoke.MinMaxLoc(imgout, ref minVal, ref maxVal, ref minLoc, ref maxLoc);

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