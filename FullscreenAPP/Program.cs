using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullscreenAPP
{
    class Program
    {
        const string currentArgument = "/V /S";
        const string listArgument = "/V /L";
        const string changeArgument = "/X:{0} /Y:{1} /D";
        const string qResPath = @"..\\QRes.exe";

        static Process proc = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = qResPath,
                Arguments = currentArgument,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };


        static char[] splitChar = { 'x', ',' };
        static string currX = "";
        static string currY = "";
        static List<string> resolutionsX = new List<string>();
        static List<string> resolutionsY = new List<string>();

        static void Main(string[] args)
        {
            //GET THE CURRENT WIDTH AND HEIGHT IN PIXELS AND SAVE
            startQres(saveResolution);

            //GET THE AVAILABLE MODES OS WIDHT AN HEIGHT
            proc.StartInfo.Arguments = listArgument;
            startQres(addResolutionsArray);

            //SET THE "BEST" RESOLUTION AVAILABLE (OR ALLOW USER TO PICK ONE)
            int index = resolutionsX.FindIndex("1024".Equals);
            proc.StartInfo.Arguments = String.Format(changeArgument, resolutionsX[index], resolutionsY[index]);
            proc.Start();
            proc.WaitForExit();

            //OPEN THE GAME AND WAIT FOR EXIT
            //TODO:
            System.Threading.Thread.Sleep(5000);

            //WHEN GAME CLOSES, RETURN TO THE SAVED WIDTH AND HEIGHT
            proc.StartInfo.Arguments = String.Format(changeArgument, currX, currY);
            proc.Start();
            proc.WaitForExit();

            Console.WriteLine("teste - pressione enter para finalizar...");
            Console.ReadLine();
        }

        static void startQres(Action<string, string> myAction)
        {
            proc.Start();
            while (!proc.StandardOutput.EndOfStream)
            {
                string line = proc.StandardOutput.ReadLine();
                string[] words = line.Split(splitChar);

                if (words.Length > 1)
                {
                    myAction(words[0], words[1]);
                }
            }
        }

        static void addResolutionsArray(string x, string y)
        {
            resolutionsX.Add(x);
            resolutionsY.Add(y);
        }

        static void saveResolution(string x, string y)
        {
            currX = x;
            currY = y;
        }
    }
}
