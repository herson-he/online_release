using FluentFTP;
using OnlineRelease.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRelease.BaseClass
{
    public class FileManage
    {
        public string[] _ignoreDir = new string[] { @"\areas\helppage", @"\scripts", @"\content" };

        public string[] _ignoreFileSuffix = new string[] { ".pdb", ".xml" };

        public string[] _ignoreFileName = new string[] { "version.version", "web.config" };

        /// <summary>
        /// 忽略目录
        /// </summary>
        public string[] IgnoreDir
        {
            get; set;
        }

        /// <summary>
        /// 忽略文件的后缀
        /// </summary>
        public string[] IgnoreFileSuffix
        {
            get; set;
        }

        /// <summary>
        /// 忽略的文件
        /// </summary>
        public string[] IgnoreFileName
        {
            get; set;
        }

        /// <summary>
        /// 项目信息
        /// </summary>
        public Project ProjectInfo { get; set; }

        /// <summary>
        /// 文件版本信息
        /// </summary>
        public List<FileVersion> ListFileVersion { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="project"></param>
        public FileManage(Project project,string versionFileStore)
        {
            ProjectInfo = project;
            ListFileVersion = GetFileVersionList(versionFileStore);
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
        /// 输出需要更新文件
        /// </summary>
        public void OutUpdateFile(string fileDir, string outDir)
        {
            string[] file = Directory.GetFiles(fileDir);
            foreach (var item in file)
            {
                FileInfo fileInfo = new FileInfo(item);
                if (IgnoreFileName.Contains(fileInfo.Name.ToLower()) || IgnoreFileSuffix.Contains(fileInfo.Extension.ToLower()))
                {
                    continue;
                }
                string relativePath = item.Replace(ProjectInfo.ReleasePath, "");

                bool isJump = false;
                foreach (var item2 in IgnoreDir)
                {
                    if (relativePath.ToLower().StartsWith(item2.ToLower()))
                    {
                        isJump = true;
                        continue;
                    }
                }
                if (isJump)
                {
                    continue;
                }

                string outFilePath = outDir + item.Replace(fileDir, "");
                string hashValue = GetMD5HashFromFile(item);

                var fileVersion = ListFileVersion.FirstOrDefault(f => f.FilePath == relativePath);
                if (fileVersion == null)
                {
                    if (!Directory.Exists(outDir))
                    {
                        Directory.CreateDirectory(outDir);
                    }
                    File.Copy(item, outFilePath, true);
                    fileVersion = new FileVersion();
                    fileVersion.FilePath = relativePath;
                    fileVersion.HashValue = hashValue;
                    ListFileVersion.Add(fileVersion);
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
                OutUpdateFile(dir[i], outDir + dir[i].Replace(fileDir, ""));
            }
        }

        /// <summary>
        /// 获取文件版本列表
        /// </summary>
        /// <returns></returns>
        public List<FileVersion> GetFileVersionList(string type)
        {
            List<FileVersion> listFileVersion = new List<FileVersion>();
            if (type == "native")
            {
                string versionPath = ProjectInfo.ProjectPath + @"\version.version";
                if (File.Exists(versionPath))
                {
                    string versionContent = System.IO.File.ReadAllText(versionPath);
                    string[] arrVersion = versionContent.Split("\r\n");
                    foreach (var item2 in arrVersion)
                    {
                        if (!string.IsNullOrEmpty(item2))
                        {
                            if (listFileVersion == null)
                            {
                                listFileVersion = new List<FileVersion>();
                            }
                            string[] arrItem = item2.Split("=");
                            listFileVersion.Add(new FileVersion { FilePath = arrItem[0], HashValue = arrItem[1] });
                        }
                    }
                }
            }
            else
            {

            }
            return listFileVersion;
        }

        /// <summary>
        /// 保存版本文件
        /// </summary>
        /// <param name="listFileVersion"></param>
        /// <param name="dir"></param>
        public void SaveVersionFile(string dir)
        {
            string fileContent = string.Empty;
            foreach (var item in ListFileVersion)
            {
                fileContent += $"{item.FilePath}={item.HashValue}\r\n";
            }
            File.WriteAllText(dir + @"\version.version", fileContent);
        }

        /// <summary>
        /// 上传文件至Ftp
        /// </summary>
        public static async Task UploadToFtp(FtpClient ftp, string fileDir, string romoteDir, string prefixDir, Progress<FtpProgress> progress)
        {
            string[] file = Directory.GetFiles(fileDir);
            foreach (var item in file)
            {
                await ftp.UploadFileAsync(item, romoteDir + "/" + Path.GetFileName(item), FtpRemoteExists.Overwrite, false, FtpVerify.None, progress);
            }
            string[] dir = Directory.GetDirectories(fileDir);
            for (int i = 0; i < dir.Length; i++)
            {
                await UploadToFtp(ftp, dir[i], romoteDir + ConvertToFtpDir(dir[i], prefixDir), prefixDir, progress);
            }
        }

        /// <summary>
        /// 获取目录下所有文件
        /// </summary>
        /// <param name="fileDir"></param>
        /// <returns></returns>
        public static List<string> GetAllFile(string fileDir)
        {
            string[] file = Directory.GetFiles(fileDir);
            List<string> list = new List<string>();
            foreach (var item in file)
            {
                list.Add(item);
            }
            string[] dir = Directory.GetDirectories(fileDir);
            foreach (var item in dir)
            {
                list.AddRange(GetAllFile(item));
            }
            return list;
        }

        /// <summary>
        /// 将本地目录转化为Ftp相对目录
        /// </summary>
        /// <param name="fileDir"></param>
        /// <param name="prefixDir"></param>
        /// <returns></returns>
        public static string ConvertToFtpDir(string fileDir, string prefixDir)
        {
            return fileDir.Replace(prefixDir, "").Replace(@"\", "/");
        }
    }
}
