using Newtonsoft.Json;
using OnlineRelease.BaseClass;
using OnlineRelease.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRelease
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "WEB发布工具";

            string configPath = System.Environment.CurrentDirectory + @"\config.json";
            Config config = null;
            if (!File.Exists(configPath))
            {
                Console.WriteLine("缺少配置文件config.json");
                goto lbWhile;
            }
            string strConfig = File.ReadAllText(configPath);
            config = JsonConvert.DeserializeObject<Config>(strConfig);
            if (config == null)
            {
                Console.WriteLine("缺少配置");
                goto lbWhile;
            }

            Console.WriteLine("欢迎使用WEB程序发布工具，致力于一键发布WEB程序");
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine("执行配置的CMD命令: run cmd");
            Console.WriteLine("打包Web文件: batch");
            Console.WriteLine("退出: exit");
            Console.WriteLine("----------------------------------------------------");

        lbWhile:
            while (true)
            {
                string cmd = Console.ReadLine().ToLower();
                if (cmd == "run cmd")
                {
                    BatchFrontFile(config.Projects);
                }
                else if (cmd == "batch")
                {
                    string currentOutDir = config.OutDir + $@"\{DateTime.Now.ToString("yyyyMMdd")}";
                    int i = 1;
                    while (Directory.Exists(currentOutDir))
                    {
                        if (i > 1)
                        {
                            currentOutDir = currentOutDir.Substring(0, currentOutDir.LastIndexOf('-'));
                        }
                        currentOutDir += "-" + i;
                        i++;
                    }
                    foreach (var item in config.Projects)
                    {
                        FileManage fileManage = new FileManage(item);
                        fileManage.IgnoreDir = config.IgnoreDir;
                        fileManage.IgnoreFileName = config.IgnoreFileName;
                        fileManage.IgnoreFileSuffix = config.IgnoreFileSuffix;
                        string projectOutDir = currentOutDir + $@"\{item.ProjectName}";
                        fileManage.OutUpdateFile(item.ReleasePath, projectOutDir);
                        fileManage.SaveVersionFile(projectOutDir);
                    }
                    Console.WriteLine("打包完毕");
                }
                else if (cmd == "help")
                {
                    Console.WriteLine("run cmd         执行配置的CMD命令");
                    Console.WriteLine("batch           输出需要更新的文件");
                    Console.WriteLine("exit            退出");
                }
                else if (cmd == "exit")
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"'{cmd}'无效命名 输入help查看命令");
                }
            }
        }

        /// <summary>
        /// 打包前端文件
        /// </summary>
        /// <param name="listProject"></param>
        public static void BatchFrontFile(List<Project> listProject)
        {
            var tasklst = new List<Task>();
            if (listProject.Any(p => p.CMD != ""))
            {
                foreach (var item in listProject)
                {
                    tasklst.Add(Task.Factory.StartNew(() => ExecuteCMD(item.ProjectPath, item.CMD)));
                }
                Task.WaitAll(tasklst.ToArray());
                Console.WriteLine("CMD命令执行完毕");
            }

            Console.WriteLine("开始Copy文件，请稍后...");
            var tasklst2 = new List<Task>();
            foreach (var item in listProject)
            {
                if (System.IO.Directory.Exists(item.ReleasePath + @"\Assets\dist"))
                {
                    System.IO.Directory.Delete(item.ReleasePath + @"\Assets\dist", true);
                }
                FileOperation.CopyFile(item.ProjectPath + @"\Assets\dist", item.ReleasePath + @"\Assets\dist");
            }
            Task.WaitAll(tasklst2.ToArray());
            Console.WriteLine("Copy文件完毕");
        }

        /// <summary>
        /// 执行CMD命令
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cmd"></param>
        public static void ExecuteCMD(string path, string cmd)
        {
            Console.WriteLine($"{path} 的 {cmd} 命令正在执行，请稍后...");
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

            //等待程序执行完退出进程
            process.WaitForExit();
            process.Close();
        }
    }
}
