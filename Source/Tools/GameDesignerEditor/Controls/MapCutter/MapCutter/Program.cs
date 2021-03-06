﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Data.SqlClient;
using System.IO;

namespace MapCutter
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            EV ev = new EV();
            //MessageBox.Show(Application.ExecutablePath);
            ev.evPath(Path.GetDirectoryName(Application.ExecutablePath));
            //MessageBox.Show("after setting env.");
            Application.Run(new FrmMain());
        }
    };

    class EV
    {
        /// <summary>
        /// 给某个目录下的所有子目录追加到path路径
        /// </summary>
        /// <param name="path">目录名</param>
        public void evPath(string path)
        {
            string spath = Environment.GetEnvironmentVariable("path");
            DirectoryInfo di = new DirectoryInfo(path);
            foreach (DirectoryInfo d in di.GetDirectories())
            {
                spath += ";" + d.FullName;
            }
            Environment.SetEnvironmentVariable("path", spath);
            foreach (DirectoryInfo d in di.GetDirectories())
            {
                evPath(d.FullName);
            }
        }
    };
}