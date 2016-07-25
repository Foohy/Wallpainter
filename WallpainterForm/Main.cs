using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Wallpainter;

namespace WallpainterForm
{
    public partial class WallpainterMain : Form
    {
        WallpaperManager mgr;

        public WallpainterMain()
        {
            InitializeComponent();

            mgr = new WallpaperManager();
        }

        private void buttonAttach_Click(object sender, EventArgs e)
        {
            //TODO: Better window picker
            //Also ability to startup programs to set
            
            IntPtr wndHandle = WindowUtils.FindWindowByTitle(textboxWindowName.Text);
            if (wndHandle == IntPtr.Zero)
            {
                MessageBox.Show("No window found!", "Failed to attach", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if (mgr.SetWallpaper(wndHandle))
            {
                buttonDetach.Enabled = true;
            }
        }

        private void buttonDetach_Click(object sender, EventArgs e)
        {
            mgr.SetWallpaper(IntPtr.Zero);
            buttonDetach.Enabled = false;
        }

        private void WallpainterMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Reset the wallpaper on exit
            mgr.SetWallpaper(IntPtr.Zero);
        }
    }
}
