using System;
using System.Text;
using System.Runtime.InteropServices;

namespace Wallpainter
{
    class WinAPI
    {
        /// <summary>
        /// Undocumented message for spawning a wallpaper worker on the program manager
        /// </summary>
        public static UInt32 SPAWN_WORKER = 0x052C;

        public enum WindowLongFlags : int
        {
            GWL_EXSTYLE = -20,
            GWLP_HINSTANCE = -6,
            GWLP_HWNDPARENT = -8,
            GWL_ID = -12,
            GWL_STYLE = -16,
            GWL_USERDATA = -21,
            GWL_WNDPROC = -4,
            DWLP_USER = 0x8,
            DWLP_MSGRESULT = 0x0,
            DWLP_DLGPROC = 0x4,
        }

        [Flags]
        public enum SendMessageTimeoutFlags : uint
        {
            SMTO_NORMAL = 0x0,
            SMTO_BLOCK = 0x1,
            SMTO_ABORTIFHUNG = 0x2,
            SMTO_NOTIMEOUTIFNOTHUNG = 0x8,
            SMTO_ERRORONEXIT = 0x20
        }

        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessageTimeout(IntPtr windowHandle, uint Msg, IntPtr wParam, IntPtr lParam, SendMessageTimeoutFlags flags, uint timeout, out IntPtr result);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

        //Set window min/max/normal status
        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
    }

    internal class Wallpainter
    {
        /// <summary>
        /// Spawns and hides the wallpaper worker, while returning a handle to the progman window
        /// </summary>
        /// <returns>A Handle to progman</returns>
        public static IntPtr SetupWallpaper()
        {
            //get the handle of the progman window
            IntPtr progman = WinAPI.FindWindow("Progman", null);

            //Send the spooky undocumented message to the progman, which will spawn the new worker window
            // that is in charge of fading the wallpaper background
            WinAPI.SendMessage(progman, WinAPI.SPAWN_WORKER, IntPtr.Zero, IntPtr.Zero);



            //This new worker window is a child of the SHELLDLL_DefView window, the default system shell window
            //Enumerate all the windows, looking for a "WorkerW" window that is an immediate sibling of "SHELLDLL_DefView"
            //And grab that window handle
            IntPtr workerw = IntPtr.Zero;
            WinAPI.EnumWindows(new WinAPI.EnumWindowsProc((tophandle, topparamhandle) =>
            {
                IntPtr p = WinAPI.FindWindowEx(tophandle, IntPtr.Zero, "SHELLDLL_DefView", null);

                //If we found that, look for the corresponding worker window as a sibling of that
                if (p != IntPtr.Zero)
                {
                    workerw = WinAPI.FindWindowEx(IntPtr.Zero, tophandle, "WorkerW", null);
                }

                return true;
            }), IntPtr.Zero);

            //Immediately hide the workerW window. Instead, we'll be parenting to the main progman 
            WinAPI.ShowWindowAsync(workerw, 0);

            return progman;
        }
    }
}