using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallpainter
{
    class WallpaperManager
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
            progman = Wallpainter.GetWorkerHandle();

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
}
