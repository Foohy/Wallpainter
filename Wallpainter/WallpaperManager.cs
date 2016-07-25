using System;
using System.Collections.Generic;
using System.Text;

namespace Wallpainter
{
    public class WallpaperManager
    {
        private struct Window
        {
            public IntPtr Handle;
            public UInt32 Style;

            public Window(IntPtr hwnd, uint style)
            {
                Handle = hwnd;
                Style = style;
            }

            public bool IsValid()
            {
                return Handle != IntPtr.Zero;
            }

            public void Reset()
            {
                Handle = IntPtr.Zero;
                Style = 0;
            }

        }

        private Window curWindow;
        private IntPtr progman = IntPtr.Zero;

        public WallpaperManager()
        {
            progman = Wallpainter.SetupWallpaper();

            if (progman == IntPtr.Zero)
                throw new InvalidOperationException("Failed to retrieve progman!");
        }

        public bool SetWallpaper(IntPtr wp)
        {
            //Remove from wallpaper
            if (curWindow.IsValid())
            {
                Restore(curWindow);
                curWindow.Reset();
            }

            //set the next
            curWindow = Set(wp);

            return curWindow.IsValid();
        }

        public IntPtr GetWallpaperWindow()
        {
            return curWindow.Handle;
        }

        private Window Set(IntPtr hwnd)
        {
            uint style = WinAPI.GetWindowLong(hwnd, (int)WinAPI.WindowLongFlags.GWL_STYLE);

            if (WinAPI.SetParent(hwnd, progman) == IntPtr.Zero)
                return new Window();

            //Remove borders
            //TODO: Should this be an option? It might break some programs
            WinAPI.SetWindowLong(hwnd, (int)WinAPI.WindowLongFlags.GWL_STYLE, 0);

            //Maximize the window
            //TODO: Fine-grained placement. This kinda sucks for multimonitor setups
            WinAPI.ShowWindowAsync(hwnd, 3);

            return new Window(hwnd, style);
        }

        private void Restore(Window window)
        {
            if (WinAPI.SetParent(window.Handle, IntPtr.Zero) != IntPtr.Zero)
            {
                WinAPI.SetWindowLong(window.Handle, (int)WinAPI.WindowLongFlags.GWL_STYLE, window.Style);
            }
        }
    }

    public static class WindowUtils
    {   
        /// <summary>
        /// Find a specific active window by its title
        /// </summary>
        /// <param name="title">The window title</param>
        /// <returns>Window handle, or null if not found</returns>
        public static IntPtr FindWindowByTitle(string title)
        {
            return WinAPI.FindWindow(null, title);
        }

        /// <summary>
        /// Find a specific window by its classname
        /// </summary>
        /// <param name="classname">Window classname</param>
        /// <returns>Window handle, or null it not found</returns>
        public static IntPtr FindWindowByClass(string classname)
        {
            return WinAPI.FindWindow(classname, null);
        }

        /// <summary>
        /// Clear the wallpaper by closing all the children of progman
        /// </summary>
        /// <param name="killChildren"></param>
        /// <returns></returns>
        public static void ClearWallpaper(bool killChildren)
        {
            IntPtr progman = Wallpainter.GetProgman();

            //TODO: Restore the style too. Saved styles aren't available in a static context
            WinAPI.EnumChildWindows(progman, new WinAPI.EnumWindowsProc((tophandle, topparamhandle) =>
            {
                WinAPI.SetParent(tophandle, IntPtr.Zero);

                //Gracefully try to close the application
                if (killChildren)
                    WinAPI.SendMessage(tophandle, WinAPI.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);

                return true;
            }), IntPtr.Zero);
        }
    }
}
