using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using OnlineRelease.Model;

namespace OnlineRelease.BaseClass
{
    /// <summary>
    /// 文件操作类
    /// </summary>
    public class FileOperation
    {
        public string[] _ignoreDir = new string[] { "" };

        public string[] _ignoreFileSuffix = new string[] { ".pdb", ".xml" };

        public string[] _ignoreFileName = new string[] { "" };

        public string _tempDir = string.Empty;

        public int _percentage = 0;

        /// <summary>
        /// 忽略目录
        /// </summary>
        public string[] IgnoreDir
        {
            get
            {
                return _ignoreDir;
            }
            set
            {
                _ignoreDir = value;
            }
        }

        /// <summary>
        /// 忽略文件的后缀
        /// </summary>
        public string[] IgnoreFileSuffix
        {
            get
            {
                return _ignoreFileName;
            }
            set
            {
                _ignoreFileName = value;
            }
        }

        /// <summary>
        /// 忽略的文件
        /// </summary>
        public string[] IgnoreFileName
        {
            get
            {
                return _ignoreFileName;
            }
            set
            {
                _ignoreFileName = value;
            }
        }

        /// <summary>
        /// 文件的临时目录
        /// </summary>
        public string TempDir
        {
            get
            {
                return _tempDir;
            }
            set
            {
                _tempDir = value;
            }
        }

        /// <summary>
        /// 文件处理进度
        /// </summary>
        public int Percentage
        {
            get
            {
                return _percentage;
            }
            set
            {
                _percentage = value;
            }
        }

        /// <summary>
        /// 获取文件总数
        /// </summary>
        /// <returns></returns>
        public void GetFileNumber(string dir, ref int fileNumber)
        {
            string[] arrDir = Directory.GetDirectories(dir);
            for (int i = 0; i < arrDir.Length; i++)
            {
                // 跳过忽略目录
                string currentDirName = GetDirectoryNameByPath(arrDir[i]).ToLower();
                if (IgnoreDir.Contains(currentDirName))
                {
                    if (currentDirName == "upload")
                    {
                        string[] arrUploadChildDir = Directory.GetDirectories(arrDir[i]);
                        for (int j = 0; j < arrUploadChildDir.Length; j++)
                        {
                            string currentUploadChildDir = GetDirectoryNameByPath(arrUploadChildDir[j]).ToLower();
                            if (currentUploadChildDir == "initialize" || currentUploadChildDir == "site")
                                GetFileNumber(arrUploadChildDir[j], ref fileNumber);
                            else
                                continue;
                        }
                    }
                    continue;
                }

                // 递归
                GetFileNumber(arrDir[i], ref fileNumber);
            }

            string[] file = Directory.GetFiles(dir);
            for (int i = 0; i < file.Length; i++)
            {
                FileInfo fi = new FileInfo(file[i]);
                if (!IgnoreFileSuffix.Contains(fi.Extension.ToLower()) && !IgnoreFileName.Contains(fi.Name.ToLower()))
                {
                    fileNumber++;
                }
            }
        }

        /// <summary>
        /// 拷贝文件
        /// </summary>
        /// <param name="sourceDir">源文件目录</param>
        /// <param name="targetDir"></param>
        public static void CopyFile(string sourceDir, string targetDir)
        {
            // 处理目录
            string[] dir = Directory.GetDirectories(sourceDir);
            for (int i = 0; i < dir.Length; i++)
            {
                // 创建目录
                string newDir = targetDir + dir[i].Replace(sourceDir, "");
                if (!Directory.Exists(newDir))
                {
                    Directory.CreateDirectory(newDir);
                }
                CopyFile(dir[i], newDir);
            }

            // 拷贝文件
            string[] file = Directory.GetFiles(sourceDir);
            for (int i = 0; i < file.Length; i++)
            {
                File.Copy(file[i], targetDir + file[i].Replace(sourceDir, ""), true);
            }
        }

        /// <summary>
        /// 生成Zip压缩文件
        /// </summary>
        /// <param name="packageDirName">压缩包目录</param>
        /// <param name="dir">压缩文件目录</param>
        /// <param name="fileTotalNumber">文件总数</param>
        /// <param name="percentage">压缩进度</param>
        public void CreateZipFile(string packageDirName, string dir, int fileTotalNumber)
        {
            string zipFileName = dir + @"\" + packageDirName + ".zip";
            using (ZipOutputStream s = new ZipOutputStream(File.Create(zipFileName)))
            {
                s.SetLevel(9);
                int completedFileNumber = 0;
                PackageDirectoryToZIP(s, TempDir, fileTotalNumber, ref completedFileNumber);
                s.Finish();
                s.Close();
            }
            Directory.Delete(TempDir, true);
        }

        /// <summary>
        /// 将目录里的文件压入S
        /// </summary>
        /// <param name="s"></param>
        public void PackageDirectoryToZIP(ZipOutputStream s, string dir, int fileTotalNumber, ref int completedFileNumber)
        {
            string[] arrDir = Directory.GetDirectories(dir);
            for (int i = 0; i < arrDir.Length; i++)
            {
                PackageDirectoryToZIP(s, arrDir[i], fileTotalNumber, ref completedFileNumber);
            }

            string[] filenames = Directory.GetFiles(dir);
            byte[] buffer = new byte[4096];

            foreach (string file in filenames)
            {
                string filePath = dir.Replace(TempDir, "") + @"\" + Path.GetFileName(file);
                ZipEntry entry = new ZipEntry(filePath);
                entry.DateTime = DateTime.Now;
                s.PutNextEntry(entry);

                using (FileStream fs = File.OpenRead(file))
                {
                    int sourceBytes;
                    do
                    {
                        sourceBytes = fs.Read(buffer, 0, buffer.Length);
                        s.Write(buffer, 0, sourceBytes);
                    } while (sourceBytes > 0);
                }

                // 设置进度条
                completedFileNumber++;
                Percentage = Convert.ToInt32(Convert.ToDouble(completedFileNumber) / Convert.ToDouble(fileTotalNumber) * 100);
                if (Percentage == 100 && completedFileNumber < fileTotalNumber)
                    Percentage = 99;
            }
        }

        /// <summary>
        /// 删除指定后缀的文件
        /// </summary>
        /// <param name="parentDir"></param>
        public void DeleteTheSuffixSpecifiedFile(string parentDir)
        {
            //获取目录文件下的所有的子文件夹
            string[] dir = Directory.GetDirectories(parentDir);
            for (int i = 0; i < dir.Length; i++)
            {
                // 递归
                DeleteTheSuffixSpecifiedFile(dir[i]);
            }

            //获取目录parentDir下的所有的文件，并删除指定后缀文件
            string[] file = Directory.GetFiles(parentDir);
            string fileContent = string.Empty;
            for (int i = 0; i < file.Length; i++)
            {
                FileInfo fi = new FileInfo(file[i]);
                if (IgnoreFileSuffix.Contains(fi.Extension.ToLower()))
                {
                    fi.Delete();
                }
            }
        }

        /// <summary>
        /// 获取目录名根据路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetDirectoryNameByPath(string path)
        {
            return path.Substring(path.LastIndexOf("\\") + 1);
        }

        /// <summary>
        /// 对比文件
        /// </summary>
        /// <param name="sourceDir">源文件目录</param>
        /// <param name="compareDir"></param>
        public void CompareFile(string sourceDir, string compareDir, string outDir)
        {
            string[] file = Directory.GetFiles(sourceDir);
            for (int j = 0; j < file.Length; j++)
            {
                string compareFilePath = compareDir + file[j].Replace(sourceDir, "");
                string outFilePath = outDir + file[j].Replace(sourceDir, "");
                if (!Directory.Exists(compareDir))
                {
                    Directory.CreateDirectory(compareDir);
                    File.Copy(file[j], compareFilePath, true);
                    Directory.CreateDirectory(outDir);
                    File.Copy(file[j], outFilePath, true);
                }
                else if (!File.Exists(file[j]))
                {
                    File.Copy(file[j], compareFilePath, true);
                    File.Copy(file[j], outFilePath, true);
                }
                else
                {
                    DateTime oldTime = File.GetLastWriteTime(file[j]);
                    DateTime compareTime = File.GetLastWriteTime(compareFilePath);
                    if (oldTime != compareTime)
                    {
                        File.Copy(file[j], compareFilePath, true);
                        File.Copy(file[j], outFilePath, true);
                    }
                }
            }

            string[] dir = Directory.GetDirectories(sourceDir);
            for (int i = 0; i < dir.Length; i++)
            {
                CompareFile(dir[i], compareDir + dir[i].Replace(sourceDir, ""), outDir + dir[i].Replace(sourceDir, ""));
            }
        }

        /// <summary>
        /// 生成版本文件
        /// </summary>
        /// <param name="dir"></param>
        public string GenerateVersionStr(string dir)
        {
            string[] file = Directory.GetFiles(dir);
            string fileContent = string.Empty;
            foreach (var item in file)
            {
                string suffix = item.Substring(item.LastIndexOf(".") + 1);
                if (!IgnoreFileSuffix.Contains(suffix))
                {
                    string hashValue = GetMD5HashFromFile(item);
                    fileContent += $"{item}={hashValue}\r\n";
                }
            }
            string[] chidrenDir = Directory.GetDirectories(dir);
            foreach (var item in chidrenDir)
            {
                if (!IgnoreDir.Contains(item))
                {
                    fileContent += GenerateVersionStr(item);
                }
            }
            return fileContent;
        }

        /// <summary>
        /// 输出需要更新文件
        /// </summary>
        public void OutNeedUpdateFile(List<FileVersion> listFileVersion, string fileDir, string outDir)
        {
            string[] file = Directory.GetFiles(fileDir);
            foreach (var item in file)
            {
                string outFilePath = outDir + item.Replace(fileDir, "");
                string hashValue = GetMD5HashFromFile(item);
                var fileVersion = listFileVersion.FirstOrDefault(f => f.FilePath == item);
                if (fileVersion == null)
                {
                    if (!Directory.Exists(outDir))
                    {
                        Directory.CreateDirectory(outDir);
                    }
                    File.Copy(item, outFilePath, true);
                    fileVersion = new FileVersion();
                    fileVersion.FilePath = item;
                    fileVersion.HashValue = hashValue;
                    listFileVersion.Add(fileVersion);
                }
                else if (fileVersion.HashValue != hashValue)
                {
                    if (!Directory.Exists(outDir))
                    {
                        Directory.CreateDirectory(outDir);
                    }
                    File.Copy(item, outFilePath, true);
                    fileVersion.HashValue = hashValue;
                }
            }
            string[] dir = Directory.GetDirectories(fileDir);
            for (int i = 0; i < dir.Length; i++)
            {
                OutNeedUpdateFile(listFileVersion, dir[i], outDir + dir[i].Replace(fileDir, ""));
            }
        }

        /// <summary>
        /// 获取文件MD5值
        /// </summary>
        /// <param name="fileName">文件绝对路径</param>
        /// <returns>MD5值</returns>
        public static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

        /// <summary>
        /// 保存版本文件
        /// </summary>
        /// <param name="listFileVersion"></param>
        /// <param name="dir"></param>
        public void SaveVersionFile(List<FileVersion> listFileVersion, string dir)
        {
            string fileContent = string.Empty;
            foreach (var item in listFileVersion)
            {
                fileContent += $"{item.FilePath}={item.HashValue}\r\n";
            }
            File.WriteAllText(dir, fileContent);
        }
    }
}
