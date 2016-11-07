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
            public WinAPI.RECT Bounds;

            public Window(IntPtr hwnd, uint style, WinAPI.RECT bounds)
            {
                Handle = hwnd;
                Style = style;
                Bounds = bounds;
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

        public bool SetWallpaper(IntPtr wp, int x = 0, int y = 0, int w = 0, int h = 0)
        {
            //Remove from wallpaper
            if (curWindow.IsValid())
            {
                Restore(curWindow);
                curWindow.Reset();
            }

            //set the next
            curWindow = Set(wp, x, y, w, h);

            return curWindow.IsValid();
        }

        public IntPtr GetWallpaperWindow()
        {
            return curWindow.Handle;
        }

        private Window Set(IntPtr hwnd, int x = 0, int y = 0, int w = 0, int h = 0)
        {
            uint style = WinAPI.GetWindowLong(hwnd, (int)WinAPI.WindowLongFlags.GWL_STYLE);
       
            WinAPI.RECT bounds;
            WinAPI.GetWindowRect(hwnd, out bounds);

            if (WinAPI.SetParent(hwnd, progman) == IntPtr.Zero)
                return new Window();


            //Remove borders
            //TODO: Should this be an option? It might break some programs
            WinAPI.SetWindowLong(hwnd, (int)WinAPI.WindowLongFlags.GWL_STYLE, 0);

            //Don't resize the window if given invalid width/height
            uint flags = WinAPI.SetWindowPosFlags.FRAMECHANGED | WinAPI.SetWindowPosFlags.SHOWWINDOW;
            if (w <= 0 || h <= 0)
            {
                flags |= WinAPI.SetWindowPosFlags.NOSIZE;
            }

            //Size window and display
            WinAPI.SetWindowPos(hwnd, IntPtr.Zero, x, y, w, h, flags);
            WinAPI.ShowWindowAsync(hwnd, 1);

            return new Window(hwnd, style, bounds);
        }

        private void Restore(Window window)
        {
            if (WinAPI.SetParent(window.Handle, IntPtr.Zero) != IntPtr.Zero)
            {
                uint flags = WinAPI.SetWindowPosFlags.FRAMECHANGED | WinAPI.SetWindowPosFlags.SHOWWINDOW;
                WinAPI.SetWindowLong(window.Handle, (int)WinAPI.WindowLongFlags.GWL_STYLE, window.Style);
                WinAPI.SetWindowPos(window.Handle, IntPtr.Zero, window.Bounds.Left, window.Bounds.Top, window.Bounds.Width, window.Bounds.Height, flags);
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
