using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Wallpainter;
using System.Runtime.InteropServices;
using System.Threading;
using System.Globalization;

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
            Process p = Process.Start(op.path, op.args);
            IntPtr hwnd = tryGetMainWindow(p);
            if (hwnd == IntPtr.Zero)
            {
                Console.Error.WriteLine("Failed to retrieve main window handle");
                if (!p.HasExited) { p.Kill(); }
                return false;
            }

            bool succesful = new WallpaperManager().SetWallpaper(hwnd);
            if (!succesful)
            {
                Console.Error.WriteLine("Failed to set wallpaper");
                p.CloseMainWindow(); //Close gracefully
            }

            return succesful;
        }

        public static bool Reset(ResetOptions op)
        {
            WindowUtils.ClearWallpaper(op.killProcess);
            return true;
        }


        private static IntPtr tryGetMainWindow(Process p, int timeout = 5000)
        {
            //First try the easy way
            
            try
            {
                p.WaitForInputIdle(timeout);
                p.Refresh();

                if (p.MainWindowHandle != IntPtr.Zero)
                    return p.MainWindowHandle;
            }
            catch (InvalidOperationException ex) { }

            //It's never that easy, is it?

            //So there's not a reliable way to go from pid -> hwnd
            //So look for windows with a matching pid
            Stopwatch sw = new Stopwatch();
            sw.Start();
            IntPtr hWnd = IntPtr.Zero;
            while (sw.ElapsedMilliseconds < timeout)
            {
                WinAPI.EnumWindows(new WinAPI.EnumWindowsProc((tophandle, topparamhandle) =>
                {

                    uint pid;
                    WinAPI.GetWindowThreadProcessId(tophandle, out pid);

                    if (pid == (uint)p.Id && WinAPI.IsWindowVisible(tophandle) && !isConsoleWindow(tophandle))
                    {
                        hWnd = tophandle;
                    }

                    //Continue looping if we haven't found a window handle yet
                    return hWnd == IntPtr.Zero;
                }), IntPtr.Zero);

                if (hWnd != IntPtr.Zero) { break; }
            }

            StringBuilder sb = new StringBuilder(256);
            WinAPI.GetClassName(hWnd, sb, sb.Capacity);

            return hWnd;
        }

        private static bool isConsoleWindow(IntPtr hWnd)
        {
            StringBuilder sb = new StringBuilder(256);
            if (WinAPI.GetClassName(hWnd, sb, sb.Capacity) == 0) { return false; }

            return string.Compare(sb.ToString(), "ConsoleWindowClass", true, CultureInfo.InvariantCulture) == 0;
        }
    }
}

