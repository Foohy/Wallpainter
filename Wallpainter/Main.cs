using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wallpainter
{
    public partial class WallpainterMain : Form
    {
        Random rand = new Random();
        IntPtr curWindow;

        public WallpainterMain()
        {
            InitializeComponent();
        }

        private Point pointInScreen()
        {
            return new Point(rand.Next(1920), rand.Next(1080));
        }

        private void buttonAttach_Click(object sender, EventArgs e)
        {
            IntPtr wndHandle = WinAPI.FindWindow(null, textboxWindowName.Text);
            if (wndHandle == IntPtr.Zero)
            {
                MessageBox.Show("No window found!", "Failed to attach", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (curWindow != IntPtr.Zero)
            {
                WinAPI.SetParent(curWindow, IntPtr.Zero);
                WinAPI.ShowWindowAsync(curWindow, 2);
                curWindow = IntPtr.Zero;
            }

            if (WinAPI.SetParent(wndHandle, Wallpainter.GetWorkerHandle()) != IntPtr.Zero)
            {
                curWindow = wndHandle;
                buttonDetach.Enabled = true;
                WinAPI.SetWindowLong(curWindow, (int)WinAPI.WindowLongFlags.GWL_STYLE, 0);
                WinAPI.ShowWindowAsync(wndHandle, 3);
            }

        }

        private void buttonDetach_Click(object sender, EventArgs e)
        {
            if (curWindow != IntPtr.Zero)
            {
                WinAPI.SetParent(curWindow, IntPtr.Zero);
                WinAPI.ShowWindowAsync(curWindow, 2);
                curWindow = IntPtr.Zero;
            }
        }
    }
}
