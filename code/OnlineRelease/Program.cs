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

            List<Project> listProject = new List<Project>();
            listProject.Add(new Project
            {
                ProjectName = "支付中心",
                ProjectPath = @"D:\7001\充值中心\网站程序\网站程序\WHPay.Pay",
                ReleasePath = @"D:\7001\充值中心\网站发布\支付中心",
                CMD = "yarn build"
            });

            listProject.Add(new Project
            {
                ProjectName = "管理系统",
                ProjectPath = @"D:\7001\充值中心\网站程序\网站程序\WHPay.Web",
                ReleasePath = @"D:\7001\充值中心\网站发布\管理系统",
                CMD = "yarn build"
            });

            Console.WriteLine("欢迎使用WEB程序发布工具，致力于一键发布WEB程序");
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine("执行配置的CMD命令: run cmd");
            Console.WriteLine("打包Web文件: batch");
            Console.WriteLine("退出: exit");
            Console.WriteLine("----------------------------------------------------");

            while (true)
            {
                string cmd = Console.ReadLine().ToLower();
                if (cmd == "run cmd")
                {
                    BatchFrontFile(listProject);
                }
                else if (cmd == "batch")
                {
                    string outDir = $@"D:\7001\充值中心\更新记录\{DateTime.Now.ToString("yyyyMMddHHmmss")}";
                    foreach (var item in listProject)
                    {
                        FileManage fileManage = new FileManage(item);
                        string projectOutDir = outDir + $@"\{item.ProjectName}";
                        fileManage.OutUpdateFile(item.ReleasePath, projectOutDir);
                        fileManage.SaveVersionFile(projectOutDir);
                    }
                }
                else if (cmd == "create version")
                {
                    GenerateVersionFile(listProject);
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
        /// 创建版本文件
        /// </summary>
        /// <param name="listProject"></param>
        public static void GenerateVersionFile(List<Project> listProject)
        {
            foreach (var item in listProject)
            {
                var fileOperations = new FileOperation();
                string version = fileOperations.GenerateVersionStr(item.ReleasePath);
                string saveDir = $@"D:\7001\充值中心\历史版本\{item.ProjectName}.version";
                System.IO.File.WriteAllText(saveDir, version);
            }
            Console.WriteLine("创建版本文件成功");
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

        /// <summary>
        /// 输出需要更新的文件
        /// </summary>
        /// <param name="listProject"></param>
        public static void OutUpdateFile(List<Project> listProject)
        {
            string versionDir = @"D:\7001\充值中心\历史版本";
            FileOperation fileOperation = new FileOperation();
            string outDir = $@"D:\7001\充值中心\更新记录\{DateTime.Now.ToString("yyyyMMddHHmmss")}";
            foreach (var item in listProject)
            {
                List<FileVersion> listFileVersion = new List<FileVersion>();
                string versionContent = System.IO.File.ReadAllText(versionDir + "\\" + item.ProjectName + ".version");
                string[] arrVersion = versionContent.Split("\r\n");
                foreach (var item2 in arrVersion)
                {
                    if (!string.IsNullOrEmpty(item2))
                    {
                        string[] arrItem = item2.Split("=");
                        listFileVersion.Add(new FileVersion { FilePath = arrItem[0], HashValue = arrItem[1] });
                    }
                }
                string projectOutDir = outDir + $@"\{item.ProjectName}";
                fileOperation.OutNeedUpdateFile(listFileVersion, item.ReleasePath, projectOutDir);
                //foreach (var item2 in listFileVersion)
                //{
                //    item2.FilePath = item2.FilePath.Replace(item.ReleasePath, "");
                //}
                //fileOperation.SaveVersionFile(listFileVersion, projectOutDir);
            }
            Console.WriteLine("输出需要更新的文件成功");
        }
    }
}
