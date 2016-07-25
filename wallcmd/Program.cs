using System;
using System.Collections.Generic;
using System.Text;
using Wallpainter;

namespace wallcmd
{
    class Program
    {
        static int Main(string[] args)
        {
            string invokedVerb = "";
            object invokedVerbOptions = null;
            if (!CommandLine.Parser.Default.ParseArguments(args, new Options(),
                (verb, verbOptions) =>
                {
                    invokedVerb = verb;
                    invokedVerbOptions = verbOptions;
                }))
            {
                return CommandLine.Parser.DefaultExitCodeFail;
            }

            switch (invokedVerb)
            {
                case Options.VERB_SET:
                    Set((SetOptions)invokedVerbOptions);
                    break;

                case Options.VERB_START:
                    Start((StartOptions)invokedVerbOptions);
                    break;

                case Options.VERB_RESET:
                    Reset((ResetOptions)invokedVerbOptions);
                    break;

                default:
                    return 1;
            }

            return 0;
        }

        public static bool Set(SetOptions op)
        {
            //Attempt to grab any window that matches
            IntPtr wndHandle = WindowUtils.FindWindowByTitle(op.windowName);
            if (wndHandle == IntPtr.Zero)
                wndHandle = WindowUtils.FindWindowByClass(op.className);

            //Fail early if we didn't find anything
            if (wndHandle == IntPtr.Zero)
            {
                Console.Error.WriteLine("Failed to find matching window.");
                return false;
            }

            //Set the wallpaper
            bool succesful = new WallpaperManager().SetWallpaper(wndHandle);
            if (!succesful)
                Console.Error.WriteLine("Failed to set wallpaper");

            return succesful;
        }

        public static bool Start(StartOptions op)
        {
            throw new NotImplementedException();
        }

        public static bool Reset(ResetOptions op)
        {
            WindowUtils.ClearWallpaper(op.killProcess);
            return true;
        }
    }
}
