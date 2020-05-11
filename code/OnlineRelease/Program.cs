using Newtonsoft.Json;
using OnlineRelease.BaseClass;
using OnlineRelease.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using FluentFTP;
using System.Threading;

namespace OnlineRelease
{
    class Program
    {
        static async Task Main(string[] args)
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
                    //BatchFrontFile(config.Projects);
                    Console.WriteLine("请输入您版本文件存放地，值为native或remote，native为项目目录下，remote将通过Ftp远程获取");
                    string versionFileStore = "";
                    while (true)
                    {
                        versionFileStore = Console.ReadLine().ToLower();
                        if (versionFileStore == "native" || versionFileStore == "remote")
                        {
                            break;
                        }
                        Console.WriteLine("文件存放地不合法，请输入native或remote");
                    }

                    Console.WriteLine("请输入您需要输出的类型，值为zip或ftp，zip为打包成zip压缩文件，ftp将自动上传至您配置的ftp目录");
                    string outType = "";
                    while (true)
                    {
                        outType = Console.ReadLine().ToLower();
                        if (outType == "zip" || outType == "ftp")
                        {
                            break;
                        }
                        Console.WriteLine("输出类型不合法，请输入zip或者ftp");
                    }

                    PrintPrompt("开始文件版本比对，请稍后....");
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
                    PrintPrompt("文件版本比对完毕");

                    if (outType == "zip")
                    {
                        PrintPrompt("开始压缩文件，请稍后....");
                        string zipName = string.Empty;
                        if (i == 1)
                        {
                            zipName = $"Web更新包{DateTime.Now.ToString("yyyyMMdd")}.zip";
                        }
                        else
                        {
                            zipName = $"Web更新包{DateTime.Now.ToString("yyyyMMdd")}-{i - 1}.zip";
                        }
                        ZipFile.CreateFromDirectory(currentOutDir, config.OutDir + "\\" + zipName);
                        PrintPrompt("开始压缩文件完毕，恭喜您，打包已全部完成");
                    }
                    else
                    {
                        PrintPrompt("开始通过FTP上传文件，请稍后....");
                        DateTime statDT = DateTime.Now; 
                        foreach (var item in config.Projects)
                        {
                            FtpConfig ftpConfig = config.PublicFtpConfig;
                            if (item.ProjectFtpConfig != null)
                            {
                                ftpConfig = item.ProjectFtpConfig;
                            }
                            var token = new CancellationToken();
                            using (var ftp = new FtpClient(ftpConfig.Host, ftpConfig.Port, ftpConfig.User, ftpConfig.Pass))
                            {
                                await ftp.ConnectAsync(token);
                                Progress<FtpProgress> progress = new Progress<FtpProgress>(p =>
                                {
                                    if (p.Progress == 1)
                                    {

                                    }
                                    else
                                    {
                                        Console.WriteLine($"正在上传项目{item.ProjectName},总共{p.FileCount}个文件，正在上传第{p.FileIndex + 1}个文件， {p.LocalPath}:{ (p.Progress).ToString("#0") }%");
                                    }
                                });
                                string projectOutDir = currentOutDir + $@"\{item.ProjectName}";
                                List<string> allFile = FileManage.GetAllFile(projectOutDir);
                                await ftp.UploadFilesAsync(allFile, item.FtpDir, FtpRemoteExists.Overwrite, true, FtpVerify.None, FtpError.None, token, progress);
                            }
                        }
                        PrintPrompt($"上传文件完成，FTP耗时{ (int)((DateTime.Now - statDT).TotalSeconds)}秒");
                    }
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
        /// 打印提示
        /// </summary>
        /// <param name="msg"></param>
        public static void PrintPrompt(string msg)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {msg}");
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
