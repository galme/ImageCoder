using System;
using System.IO;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;

namespace pixel_encoder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static string textFilePath = "";
        public static string imageFilePath = "";

        public static string getStringFromFile()
        {
            string result = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if ((bool)openFileDialog.ShowDialog())
            {
                System.IO.Stream fileStream = openFileDialog.OpenFile();
                textFilePath = openFileDialog.FileName.Replace(openFileDialog.SafeFileName, "");

                using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream))
                {
                    while (!reader.EndOfStream)
                        result += (char)reader.Read();
                }
                fileStream.Close();
            }
            return result;
        }


        public MainWindow()
        {
            InitializeComponent();
        }

        private void EncodeClick(object sender, RoutedEventArgs e)
        {
            string str = getStringFromFile();
            if (str.Length == 0)
                return;
            int width = Convert.ToInt32(Math.Sqrt(str.Length / 3)) + 1;
            int height = width;
            //Bitmap bitmap = new Bitmap(str.Length / 3 + 1, 1);
            Bitmap bitmap = new Bitmap(width, height);


            int c = 0;
            int r, g, b;
            while (c < str.Length)
            {
                r = str[c++];
                if (c < str.Length)
                    g = str[c++];
                else
                    g = 0;

                if (c < str.Length)
                    b = str[c++];
                else
                    b = 0;

                int x = ((c - 1) / 3) % width;
                int y = ((c - 1) / 3) / width;
                //bitmap.SetPixel((c-1) / 3, 0, Color.FromArgb(r, g, b));
                bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(r, g, b));
            }
            bitmap.Save(textFilePath + "encoded.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            bitmap.Dispose();
        }

        private void DecodeClick(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if ((bool)openFileDialog.ShowDialog())
            {
                System.Windows.Forms.PictureBox pictureBox = new System.Windows.Forms.PictureBox();
                Bitmap bitmap = new Bitmap(openFileDialog.FileName);
                int height = bitmap.Height;
                int width = bitmap.Width;
                string data = "";
                imageFilePath = openFileDialog.FileName.Replace(openFileDialog.SafeFileName, "");

                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        char r = (char)bitmap.GetPixel(j, i).R;
                        char g = (char)bitmap.GetPixel(j, i).G;
                        char b = (char)bitmap.GetPixel(j, i).B;

                        if (r == (char)0)
                            break;
                        else
                            data += r;

                        if (g == (char)0)
                            break;
                        else
                            data += g;

                        if (b == (char)0)
                            break;
                        else
                            data += b;
                    }
                }

                bitmap.Dispose();

                StreamWriter file = new StreamWriter(imageFilePath + "decoded.txt");
                file.WriteLine(data);
                file.Close();
            }
        }
    }
}
