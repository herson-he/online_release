using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRelease.BaseClass
{
    public class CreateConsole
    {
        [DllImport("Kernel32.dll")]
        public static extern bool AllocConsole(); //启动窗口  
        [DllImport("kernel32.dll", EntryPoint = "FreeConsole")]
        public static extern bool FreeConsole();      //释放窗口，即关闭  
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public extern static IntPtr FindWindow(string lpClassName, string lpWindowName);//找出运行的窗口  

        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
        public extern static IntPtr GetSystemMenu(IntPtr hWnd, IntPtr bRevert); //取出窗口运行的菜单  

        [DllImport("user32.dll", EntryPoint = "RemoveMenu")]
        public extern static IntPtr RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        [DllImport("Kernel32.dll")]
        public static extern bool SetConsoleTitle(string strMessage);  
    }
}
