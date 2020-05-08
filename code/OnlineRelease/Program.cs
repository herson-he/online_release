using System;

namespace OnlineRelease
{
    class Program
    {
        public string[] ProjectUrl = new string[] { };

        static void Main(string[] args)
        {
            Console.Title = "在线发布系统";
            string path = @"D:\公司项目\支付系统\网站程序\WHPay.Web";
            ExecuteCMD(path, "yarn build", "打包脚本");

            string path2 = @"D:\公司项目\支付系统\网站程序\WHPay.Pay";
            ExecuteCMD(path2, "yarn build", "打包脚本");
        }

        public static void ExecuteCMD(string path, string cmd, string name = "")
        {
            Console.WriteLine(name + "命令正在执行...");
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.UseShellExecute = false;          //是否使用操作系统shell启动
            startInfo.RedirectStandardInput = true;     //接受来自调用程序的输入信息
            startInfo.RedirectStandardOutput = true;    //由调用程序获取输出信息
            startInfo.RedirectStandardError = true;     //重定向标准错误输出
            startInfo.CreateNoWindow = true;            //不显示程序窗口
            process.StartInfo = startInfo;
            process.Start();

            process.StandardInput.WriteLine(@"cd /d " + path);
            process.StandardInput.WriteLine(cmd + "&exit");
            process.StandardInput.AutoFlush = true;

            //获取cmd窗口的输出信息
            string output = process.StandardOutput.ReadToEnd();
            Console.WriteLine(output);
            //if (output.ToLower().Contains(exitStr.ToLower()))
            //{
            //    break;
            //}

            //等待程序执行完退出进程
            Console.WriteLine(name + "命令执行成功");
            process.WaitForExit();
            process.Close();
        }
    }
}
