using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ImageMagick;
using SharpGL;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Cameras;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Primitives;
using SharpGL.WPF;
using Path = System.IO.Path;
using System.Security.RightsManagement;
using SharpGL.SceneGraph.Assets;

/* COLORS FOR REFERENCE (IN THAT ORDER):
             * BLACK:         #100C10
             * WHITE:         #E6E7E6
             * BLUE:          #0B1B4A
             * GREEN:         #085934
             * RED:           #A51310
             * ORANGE:        #B54500
             * YELLOW:        #DEAA21
             * PURPLE:        #630B52
             * BEIGE:         #979073
             * PEACH:         #E6B68C
             * BROWN:         #3C2710
             * PINK:          #BA5291
             * SAFFRON:       #CE7808
             * GRAY:          #707370
             * TAN:           #6E4124
             * AZURE:         #163D70
             * PEARL:         #C5CCCE
             * TEAL:          #036863
             * LIGHT_BLUE:    #4D809C
             * LIGHT_GREEN:   #0E7F4D
             * LIGHT_RED:     #A51310
             * LIGHT_BROWN:   #7E5029
             * LIGHT_BLACK:   #242724
             * LIGHT_YELLOW:  #F1C13A
             * DARK_YELLOW:   #BD8A19
             * DARK_RED:      #5A0408
             * DARK_GREEN:    #033E29
             * FAINT_RED:     #E47F7B
             * GREY_OBSERVER: #AAB2AF
             * HIGHLIGHT:     #N/A
             */


namespace Flag_Designer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            OnLaunch(); // for everything
        }

        public void OnLaunch()
        {
            // define paths
            const string gamePath = "E:/Games/SteamLibrary/steamapps/common/Victoria 3/";
            const string patternsPath = gamePath + "game/gfx/coat_of_arms/patterns/";
            const string coloredEmblemsPath = gamePath + "game/gfx/coat_of_arms/colored_emblems/";
            const string texturedEmblemsPath = gamePath + "game/gfx/coat_of_arms/textured_emblems/";
            const string countryTag = Window1.countryTag;
            // makes the pattern buttons
            PopulatePatternList(patternsPath);
            // starting flag
            if (countryTag == null)
            {
                // start with default flag
                string defaultPattern = patternsPath + "pattern_solid.tga";
                string defaultColor1 = "#A51310";
                string defaultEmblem1 = coloredEmblemsPath + "ce_star_04.dds";
                string defaultEmblemColor1 = "#E6E7E6";
                DrawDefaultFlag(defaultPattern, defaultEmblem1, ref gl) ;
            }
            else
            {
                // start with saved flag
                DrawSavedFlag(countryTag);
            }

        }

        // Can execute
        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        // Minimize
        private void CommandBinding_Executed_Minimize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }
        // Close
        private void CommandBinding_Executed_Close(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        private void PopulatePatternList(string patternsPath)
        {
            string folderPath = patternsPath;

            if (Directory.Exists(folderPath))
            {
                List<string> fileList = GetFileList(folderPath);

                foreach (string file in fileList)
                {
                    if (Path.GetExtension(file) == ".dds" || Path.GetExtension(file) == ".tga")
                    {
                        Button patternButton = new Button();

                        // Create an Image control and set its source to the ".dds" file
                        Image image = new Image();
                        BitmapSource bitmap = LoadDDSImage(Path.Combine(folderPath, file));
                        image.Source = bitmap;

                        // Create an Image control
                        Image imageControl = new Image();
                        imageControl.Source = bitmap;
                        imageControl.Stretch = Stretch.Uniform;

                        // Set the Image as the button's content
                        patternButton.Content = imageControl;

                        patternButton.Width = 45;
                        patternButton.Height = 30;
                        patternButton.Margin = new Thickness(3);
                        patternButton.Name = file.Replace('.', '_');

                        patternButton.Click += (sender, args) =>
                        {
                            OnClickPatternbtn(imageControl, folderPath, file);
                            Close();
                        };

                        PatternsWrapPanel.Children.Add(patternButton);
                    }
                }
            }
            else
            {
                MessageBox.Show("Invalid folder path. Please provide a valid path.");
            }
        }

        static List<string> GetFileList(string folderPath)
        {
            List<string> fileList = new List<string>();

            try
            {
                string[] files = Directory.GetFiles(folderPath);

                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    fileList.Add(fileName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return fileList;
        }


        private BitmapSource LoadDDSImage(string filePath)
        {
            try
            {
                // Read the DDS image
                using (MagickImage image = new MagickImage(filePath))
                {
                    // Convert the image to PNG format
                    image.Format = MagickFormat.Png;

                    // Save the image to a MemoryStream
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        image.Write(memoryStream);
                        memoryStream.Position = 0;

                        // Create a BitmapImage
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = memoryStream;
                        bitmapImage.EndInit();
                        return bitmapImage;
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading the DDS image: {ex.Message}");
                return null;
            }
        }

        public void OnClickPatternbtn(Image imageControl, string folderPath, string file)
        {
            

        }

        public void DrawDefaultFlag(string pattern, string emblem1, ref OpenGL gl)
        {
 

            BitmapSource patternimage = LoadDDSImage(pattern);

            gl.GenTextures(1, texture);
            gl.BindTexture(OpenGL.GL_TEXTURE_2D, texture[0]);
            gl.TexImage2D(OpenGL.GL_TEXTURE_2D, 0, (int)OpenGL.GL_RGB8, patternimage.Width, patternimage.Height, 0,
                OpenGL.GL_BGR_EXT, OpenGL.GL_UNSIGNED_BYTE, patternimage.Scan0);

            uint[] array = new uint[] { OpenGL.GL_NEAREST };
            gl.TexParameterI(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, array);
            gl.TexParameterI(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, array);


        }

        private void openGLCtrl_OpenGLDraw(object sender, OpenGLRoutedEventArgs args)
        {
            //  Get the OpenGL instance that's been passed to us.
            OpenGL gl = args.OpenGL;

            //  Clear the color and depth buffers.
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            //  Reset the modelview matrix.
            gl.LoadIdentity();

            //  Move the geometry into a fairly central position.
            //gl.Translate(0f, 0.0f, -6.0f);

            // DRAW THE FUCKING FLAG RIGHT HERE
            gl.Begin(OpenGL.GL_QUADS);

            DrawDefaultFlag(pattern, emblem1);

            gl.End();

            //  Flush OpenGL.
            gl.Flush();
            /*
            //  Clear the color and depth buffers.
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            //  Reset the modelview matrix.
            gl.LoadIdentity(); */
        }


        private void openGLCtrl_OpenGLInitialized(object sender, OpenGLRoutedEventArgs args)
        {
            args.OpenGL.Enable(OpenGL.GL_DEPTH_TEST);
        }

    }  
}