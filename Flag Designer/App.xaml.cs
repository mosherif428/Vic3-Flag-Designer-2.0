using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

/*
 * VICTORIA 3 FLAG DESIGNER
 * 
 * HOW IT WORKS
 * ------------
 * On startup, you are asked to pick the game directory and the mod files directory. Then, you're asked to restart
 * the game with the mod enabled. The tool takes the country from metadata, then finds the flag in the source files
 * and overwrites it in the mod through the flag definitions file with the new flag. the changes hotload upon saving.
 */



namespace Flag_Designer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static void Launch()
        {
            /*If first time then ask for paths*/
            /*
             * string docsFolderPath = txtdocsfolder.Text;
             * string gameFolderPath = txtgamefolder.Text;
             */

           
        }

        

    }

}
