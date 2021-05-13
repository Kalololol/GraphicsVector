using System.Drawing;
using System.Drawing.Imaging;

/*Aplikacja jako argument uruchomieniowy powinna przyjąć nazwy plikow *.bmp do przetworzenia
Aplikacja powinna wykonać algorytm alpha blending na tych dwóch obrazkach i wynikowy obrazek zapisac
w folderze uruchomienia pod nazwa output.bmp
Program powinien wczytac dwa zdjecia, utworzyc nowy obiekt ktory nalozy 1sze zdjecie na drugie.
nowe zdjecie powinno byc przezroczyste oraz trzeba zapisac je w folderze uruchomienia z nazwa output.bmp
*/

namespace ZadanieRekrutacyjne {
    class Program {

        static void Main(string[] args) {

            string adressUrl = "C:\\Users\\Karol\\Desktop\\Projekty\\ZadanieRekrutacyjne\\";

            Image firstPhoto = Image.FromFile(adressUrl + "zdj1.bmp");
            Image secondPhoto = Image.FromFile(adressUrl + "zdj2.bmp");

            Run(firstPhoto, secondPhoto);
        }

        static void Run(Image firstPhoto, Image secondPhoto) {
            string adressUrl = "C:\\Users\\Karol\\Desktop\\Projekty\\ZadanieRekrutacyjne\\";

            int firstWidth = firstPhoto.Width;
            int firstHeight = firstPhoto.Height;

            int secondWidth = secondPhoto.Width;
            int secondHeight = secondPhoto.Height;

            Bitmap bmPhoto = new Bitmap(firstWidth, firstHeight); //nowa bitmapa, z wymiarami 1sz zdj
            Graphics grPhoto = Graphics.FromImage(bmPhoto);  // utworzenie bitmapy 1sz zdj

            grPhoto.DrawImage( firstPhoto, new Rectangle(0, 0, firstWidth, firstHeight), 0, 0, firstWidth, firstHeight, GraphicsUnit.Pixel);

            // Utwórz Bitmap na podstawie wcześniej zmodyfikowanego zdjęcia. Załaduj Bitmap do nowego obiektu Graphic.            
            Bitmap bmTransparent = new Bitmap(bmPhoto);
            bmTransparent.SetResolution(firstPhoto.HorizontalResolution, firstPhoto.VerticalResolution);
            Graphics grTransparent = Graphics.FromImage(bmTransparent);

             /* Aby uzyskać przezroczysty obraz, stosujemy manipulacje kolorami, definiując ImageAttributesobiekt 
              i ustawiając dwie z jego właściwości. Pierwszym krokiem jest zastąpienie  koloru tła przezroczystym 
            ( Alpha=0, R=0, G=0, B=0). Aby to zrobić, użyjemy a Colormap i zdefiniujemy a RemapTable. 
             */
            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();

            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            // Druga manipulacja kolorem służy do zmiany krycia obrazu. Odbywa się to poprzez zastosowanie macierzy 5x5, 
            //  należy ustawić odpowiednie paramety aby osiągnąć zadowalajacy poziom przezroczystosci.                         
                float[][] colorMatrixElements = {
                      new float[] {0.5f,  0.0f,  0.0f,  0.0f, 0.0f},
                      new float[] {0.0f,  0.5f,  0.0f,  0.0f, 0.0f},
                      new float[] {0.0f,  0.0f,  0.5f,  0.0f, 0.0f},
                      new float[] {0.0f,  0.0f,  0.0f,  0.5f, 0.0f},
                      new float[] {0.0f,  0.0f,  0.0f,  0.0f, 0.5f}
                };

            ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            // Po dodaniu do obiektu imageAttributes obu manipulacji, trzeba nałożyć zdjęcia na siebie.
            int xPos = 0;
            int yPos = 0;

            grTransparent.DrawImage(secondPhoto, new Rectangle(xPos, yPos, secondWidth, secondHeight), 0, 0, secondWidth, secondHeight, GraphicsUnit.Pixel, imageAttributes);            
           
            // zastąpienie oryginalnego obrazu nowym Bitmap
            firstPhoto = bmTransparent;
            firstPhoto.Save(adressUrl + "output.bmp");
        }
    }
}
