using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Wallpainter
{

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct PIXELFORMATDESCRIPTOR
    {
        UInt32 nSize;
        UInt32 nVersion;
        UInt64 dwFlags;
        Byte iPixelType;
        Byte cColorBits;
        Byte cRedBits;
        Byte cRedShift;
        Byte cGreenBits;
        Byte cGreenShift;
        Byte cBlueBits;
        Byte cBlueShift;
        Byte cAlphaBits;
        Byte cAlphaShift;
        Byte cAccumBits;
        Byte cAccumRedBits;
        Byte cAccumGreenBits;
        Byte cAccumBlueBits;
        Byte cAccumAlphaBits;
        Byte cDepthBits;
        Byte cStencilBits;
        Byte cAuxBuffers;
        Byte iLayerType;
        Byte bReserved;
        UInt64 dwLayerMask;
        UInt64 dwVisibleMask;
        UInt64 dwDamageMask;
    }


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

        /// <summary>Values to pass to the GetDCEx method.</summary>
        [Flags()]
        public enum DeviceContextValues : uint
        {
            /// <summary>DCX_WINDOW: Returns a DC that corresponds to the window rectangle rather
            /// than the client rectangle.</summary>
            Window = 0x00000001,

            /// <summary>DCX_CACHE: Returns a DC from the cache, rather than the OWNDC or CLASSDC
            /// window. Essentially overrides CS_OWNDC and CS_CLASSDC.</summary>
            Cache = 0x00000002,

            /// <summary>DCX_NORESETATTRS: Does not reset the attributes of this DC to the
            /// default attributes when this DC is released.</summary>
            NoResetAttrs = 0x00000004,

            /// <summary>DCX_CLIPCHILDREN: Excludes the visible regions of all child windows
            /// below the window identified by hWnd.</summary>
            ClipChildren = 0x00000008,

            /// <summary>DCX_CLIPSIBLINGS: Excludes the visible regions of all sibling windows
            /// above the window identified by hWnd.</summary>
            ClipSiblings = 0x00000010,

            /// <summary>DCX_PARENTCLIP: Uses the visible region of the parent window. The
            /// parent's WS_CLIPCHILDREN and CS_PARENTDC style bits are ignored. The origin is
            /// set to the upper-left corner of the window identified by hWnd.</summary>
            ParentClip = 0x00000020,

            /// <summary>DCX_EXCLUDERGN: The clipping region identified by hrgnClip is excluded
            /// from the visible region of the returned DC.</summary>
            ExcludeRgn = 0x00000040,

            /// <summary>DCX_INTERSECTRGN: The clipping region identified by hrgnClip is
            /// intersected with the visible region of the returned DC.</summary>
            IntersectRgn = 0x00000080,

            /// <summary>DCX_EXCLUDEUPDATE: Unknown...Undocumented</summary>
            ExcludeUpdate = 0x00000100,

            /// <summary>DCX_INTERSECTUPDATE: Unknown...Undocumented</summary>
            IntersectUpdate = 0x00000200,

            /// <summary>DCX_LOCKWINDOWUPDATE: Allows drawing even if there is a LockWindowUpdate
            /// call in effect that would otherwise exclude this window. Used for drawing during
            /// tracking.</summary>
            LockWindowUpdate = 0x00000400,

            /// <summary>DCX_USESTYLE: Undocumented, something related to WM_NCPAINT message.</summary>
            UseStyle = 0x00010000,

            /// <summary>DCX_VALIDATE When specified with DCX_INTERSECTUPDATE, causes the DC to
            /// be completely validated. Using this function with both DCX_INTERSECTUPDATE and
            /// DCX_VALIDATE is identical to using the BeginPaint function.</summary>
            Validate = 0x00200000,
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
        public static extern IntPtr GetDCEx(IntPtr hWnd, IntPtr hrgnClip, DeviceContextValues flags);

        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

        [DllImport("gdi32.dll")]
        public static extern int GetPixelFormat(IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern int DescribePixelFormat(IntPtr hdc, int iPixelFormat, uint nBytes, ref PIXELFORMATDESCRIPTOR ppfd);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

        //Set window min/max/normal status
        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
    }


    class Wallpainter
    {

        public static IntPtr GetWorkerHandle()
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
                    workerw = WinAPI.FindWindowEx(IntPtr.Zero,  tophandle, "WorkerW", null);
                }

                return true;
            }), IntPtr.Zero);

            //Immediately hide the workerW window. Instead, we'll be parenting to the main progman 
            WinAPI.ShowWindowAsync(workerw, 0);

            return progman;
        }
    }
}
