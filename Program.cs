using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

/*Aplikacja jako argument uruchomieniowy powinna przyjąć nazwy plikow *.bmp do przetworzenia
Aplikacja powinna wykonać algorytm alpha blending na tych dwóch obrazkach i wynikowy obrazek zapisac
w folderze uruchomienia pod nazwa output.bmp
Program powinien wczytac dwa zdjecia, utworzyc nowy obiekt ktory nalozy 1sze zdjecie na drugie.
nowe zdjecie powinno byc przezroczyste oraz trzeba zapisac je w folderze uruchomienia z nazwa output.bmp
*/

namespace ZadanieRekrutacyjne {
    class Program {

        static void Main(string[] args) {

            string adresUrl = Directory.GetCurrentDirectory();

            Image firstPhoto = Image.FromFile(adresUrl + "\\zdj1.bmp");
            Image secondPhoto = Image.FromFile(adresUrl + "\\zdj2.bmp");

            int firstWidth = firstPhoto.Width;
            int firstHeight = firstPhoto.Height;

            int secondWidth = secondPhoto.Width;
            int secondHeight = secondPhoto.Height;

            Bitmap bmPhoto = new Bitmap(firstWidth, firstHeight);
            Graphics grPhoto = Graphics.FromImage(bmPhoto);

            grPhoto.DrawImage(firstPhoto, new Rectangle(0, 0, firstWidth, firstHeight), 0, 0, firstWidth, firstHeight, GraphicsUnit.Pixel);

            Bitmap bmTransparent = new Bitmap(bmPhoto);
            bmTransparent.SetResolution(firstPhoto.HorizontalResolution, firstPhoto.VerticalResolution);
            Graphics grTransparent = Graphics.FromImage(bmTransparent);

            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();

            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            float[][] colorMatrixElements = {
                      new float[] {0.5f,  0.0f,  0.0f,  0.0f, 0.0f},
                      new float[] {0.0f,  0.5f,  0.0f,  0.0f, 0.0f},
                      new float[] {0.0f,  0.0f,  0.5f,  0.0f, 0.0f},
                      new float[] {0.0f,  0.0f,  0.0f,  0.5f, 0.0f},
                      new float[] {0.0f,  0.0f,  0.0f,  0.0f, 0.5f}
                };

            ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            int xPos = 0;
            int yPos = 0;

            grTransparent.DrawImage(secondPhoto, new Rectangle(xPos, yPos, secondWidth, secondHeight), 0, 0, secondWidth, secondHeight, GraphicsUnit.Pixel, imageAttributes);

            firstPhoto = bmTransparent;
            firstPhoto.Save("output.bmp");
        }
    }
}

