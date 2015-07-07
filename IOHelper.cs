/*
 * IOHelper
 * 20150401went
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace WTTools
{
    /// <summary>
    /// IO读写助手 
    /// </summary>
    public class IOHelper
    {

        #region "唯一实例"
        private static IOHelper _Instance;
        private static readonly object _Lock = new object();

        private IOHelper() { }

        public static IOHelper Instance
        {
            get
            {
                if(_Instance==null)
                {
                    lock(_Lock)
                    {
                        _Instance = new IOHelper();
                    }
                }
                return _Instance;
            }
        }

        ~IOHelper()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region "文件操作"
        /// <summary>
        /// 检验文件是否被占用
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static bool ChkFileOccupation(string filePath)
        {
            bool flag = false;
            try
            {
                File.Move(filePath, filePath);
            }
            catch(Exception)
            {
                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// 拷贝文件夹
        /// </summary>
        /// <param name="srcPath">源文件夹路径</param>
        /// <param name="destPath">目标路径</param>
        public void CopyDir(string srcPath, string destPath)
        {
            try
            {
                //判断路径是不是\结尾，否的话添加\到结尾
                if (destPath[destPath.Length - 1] != Path.DirectorySeparatorChar)
                {
                    destPath =destPath +  Path.DirectorySeparatorChar;
                }
                //判断目标路径是否存在
                if (!Directory.Exists(destPath))
                {
                    Directory.CreateDirectory(destPath);
                }
                //获取到所有的文件和目录
                var paths = Directory.GetFileSystemEntries(srcPath);
                //copy
                foreach(string str in paths)
                {
                    //如果是目录，调用本身copy
                    if (Directory.Exists(str))
                    {
                        this.CopyDir(str, destPath + Path.GetFileName(str));
                    }
                    else
                    {
                        //如果是文件，copy文件
                        File.Copy(str, destPath + Path.GetFileName(str), true);
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }


        /// <summary>
        /// 复制文件到指定目录
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="destFile"></param>
        public void CopyFile(string srcFile, string destFile)
        {
            File.Copy(srcFile, destFile);
        }

        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        /// <returns></returns>
        public bool CopyFolder(string src,string dest)
        {
            try
            {
                if (dest[dest.Length - 1] != Path.DirectorySeparatorChar)
                    dest = dest + Path.DirectorySeparatorChar;
                if(!Directory.Exists(dest))
                {
                    Directory.CreateDirectory(dest);
                }
                var files = Directory.GetFileSystemEntries(src);
                foreach(var file in files)
                {
                    if (Directory.Exists(file))
                    {
                        this.CopyFolder(file, dest + Path.GetFileName(file));
                    }
                    else
                    {
                        File.Copy(file, dest + Path.GetFileName(file), true);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="path"></param>
        public void CreateFolder(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            if(!di.Exists)
            {
                di.Create();
            }
        }

        /// <summary>
        /// 在指定目录下创建文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parentPath"></param>
        public void CreateFolder(string path,string parentPath)
        {
            var di = new DirectoryInfo(parentPath);
            if(di.Exists)
            {
                di.CreateSubdirectory(path);
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileFullName"></param>
        /// <returns></returns>
        public bool DeleteFile(string fileFullName)
        {
            FileInfo fi = new FileInfo(fileFullName);
            if (fi.Exists)
            {
                try
                {
                    fi.Delete();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        public void DeleteFiles(string path, string[] extenArray, int days)
        {
            //TODO:GetAllFiles完成后
        }


        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="dir"></param>
        public void DeleteFolder(string dir)
        {
            if(Directory.Exists(dir))
            {
                var files = Directory.GetFileSystemEntries(dir);
                foreach (var f in files)
                {
                    if(File.Exists(f))
                    {
                        File.Delete(f);
                    }
                    else
                    {
                        this.DeleteFolder(f);
                    }
                }
                Directory.Delete(dir, true);
            }
        }

        /// <summary>
        /// 在文件后添加字符串
        /// </summary>
        /// <param name="path"></param>
        /// <param name="strings"></param>
        public void FileAdd(string path, string strings)
        {
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.Write(strings);
                sw.Flush();
            }
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="destFile"></param>
        public void FileMove(string srcFile, string destFile)
        {
            File.Move(srcFile, destFile);
        }

        /// <summary>
        /// 根据匹配文件后缀名获取所有文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="extentArray">匹配后缀名</param>
        /// <returns></returns>
        public List<string> GetAllFiles(string path, string[] extentArray)
        {
            var fileList = new List<string>();

            if (Directory.Exists(path))
            {
                var fsEntries = Directory.GetFileSystemEntries(path);
                foreach (var f in fsEntries)
                {
                    string fullPath = Path.GetFullPath(f);
                    string fileName = Path.GetFileName(f);
                    string ext = Path.GetExtension(fullPath).ToLower();
                    if (extentArray == null || extentArray.Length == 0)
                    {
                        fileList.Add(fileName);
                    }
                    else
                    {
                        if(extentArray.Any(c=>c.ToLower()==ext || ("." + c.ToLower())==ext))
                        {
                            fileList.Add(fileName);
                        }
                    }
                }
            }
            return fileList;
        }

        /// <summary>
        /// 获取目录大小
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public long GetDirectoryLength(string dirPath)
        {
            if(!Directory.Exists(dirPath))
            {
                return 0L;
            }
            long len = 0L;
            DirectoryInfo di = new DirectoryInfo(dirPath);
            var files = di.GetFiles();
            foreach (var info in files)
            {
                len += info.Length;
            }
            var dirs = di.GetDirectories();
            if (dirs.Length > 0)
            {
                for (int i = 0; i < dirs.Length; i++)
                {
                    len += this.GetDirectoryLength(dirs[i].FullName);
                }
            }

            return len;
        }
        /// <summary>
        /// 获取文件详细信息
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string GetFileAttribute(string filePath)
        {
            string str = "";
            FileInfo fi = new FileInfo(filePath);
            str = string.Format("详细路径:{0}<br>文件名称:{1}<br>文件长度:{2}字节<br>创建时间:{3}<br>最后访问时间:{4}<br>修改时间:{5}<br>所在目录:{6}<br>扩展名:{7}",
                                fi.FullName,fi.Name,fi.Length.ToString(),fi.CreationTime.ToString("yyyy/M/d HH:mm:ss"),fi.LastAccessTime.ToString("yyyy/M/d HH:mm:ss"),
                                fi.LastWriteTime.ToString("yyyy/M/d HH:mm:ss"),fi.DirectoryName,fi.Extension
                                );

            return str;
        }


        /// <summary>
        /// 获取文件夹文件结构文本
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string GetFolderAll(string path)
        {
            string rn = "";
            DirectoryInfo di = new DirectoryInfo(path);
            return this.ListTreeShow(di, 0, rn);
        }

        public string GetFolderAll(string path,string dropName,string tplPath)
        {
            string rn = "";
            string str = "<select name=\"" + dropName + "\" id=\"" + dropName + "\"><option value=\"\">--请选择详细模板--</option>";
            DirectoryInfo di = new DirectoryInfo(path);
            rn = this.ListTreeShow(di, 0, rn, tplPath);
            return (str + rn + "</select>");
        }

        /// <summary>
        /// 文件是否可文本编辑
        /// </summary>
        /// <param name="strExtension"></param>
        /// <returns></returns>
        public bool IsFileCanEdit(string strExtension)
        {
            strExtension = strExtension.ToLower();
            if (strExtension.LastIndexOf(".") >= 0)
            {
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
            }
            else
            {
                strExtension = ".txt";
            }
            string[] strArray = new string[] { ".htm", ".html", ".txt", ".js", ".css", ".xml", ".sitemap" };
            if(strArray.Contains(strExtension))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 安全文件名
        /// </summary>
        /// <param name="strExtension"></param>
        /// <returns></returns>
        public bool IsSafeFileName(string strExtension)
        {
            strExtension = strExtension.ToLower();
            if (strExtension.LastIndexOf(".") >= 0)
            {
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
            }
            else
            {
                strExtension = ".txt";
            }
            string[] strArray = new string[] { ".htm", ".html", ".txt", ".js", ".css", ".xml", ".sitemap", ".jpg", ".gif", ".png", ".rar", ".zip" };
            if (strArray.Contains(strExtension))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 风险文件名
        /// </summary>
        /// <param name="strExtension"></param>
        /// <returns></returns>
        public bool IsUnsafeFileName(string strExtension)
        {
            strExtension = strExtension.ToLower();
            if (strExtension.LastIndexOf(".") >= 0)
            {
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
            }
            else
            {
                strExtension = ".txt";
            }
            string[] strArray = new string[] { 
                ".", ".asp", ".aspx", ".cs", ".net", ".dll", ".config", ".ascx", ".master", ".asmx", ".asax", ".cd", ".browser", ".rpt", ".ashx", ".xsd", 
                ".mdf", ".resx", ".xsd"
             };
            if (strArray.Contains(strExtension))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 移动文件夹
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="destPath"></param>
        /// <returns></returns>
        public bool MoveFolder(string srcPath, string destPath)
        {
            try
            {
                Directory.Move(srcPath, destPath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 读取文件文本
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string ReadFile(string path)
        {
            string str = "";
            if(!File.Exists(path))
            {
                return "文件不存在";
            }
            StreamReader sr = new StreamReader(path, Encoding.GetEncoding("gb2312"));
            str = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
            return str;
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        public void WriteFile(string path,string content)
        {
            if(!File.Exists(path))
            {
                FileStream stream = File.Create(path);
                stream.Close();
                stream.Dispose();
            }
            StreamWriter sw = new StreamWriter(path, true, Encoding.UTF8);
            sw.WriteLine(content);
            sw.Close();
            sw.Dispose();
        }
        #endregion

        /// <summary>
        /// 获取文件树状结构文本
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="nLevel"></param>
        /// <param name="Rn">rootnode</param>
        /// <returns></returns>
        public string ListTreeShow(DirectoryInfo dir,int nLevel,string Rn)
        {
            var dirs = dir.GetDirectories();
            foreach (var di in dirs)
            {
                if (nLevel == 0)
                {
                    Rn += "├";
                }
                else
                {
                    string str = "";
                    for (int i = 1; i < nLevel; i++)
                    {
                        str += "│&nbsp;";
                    }
                    Rn += str + "├";
                }
                Rn += "<b>" + di.Name + "</b><br />";
                foreach (var fi in di.GetFiles())
                {
                    if (nLevel == 0)
                    {
                        Rn += "│&nbsp;├";
                    }
                    else
                    {
                        string str2 = "";
                        for (int i = 1; i < nLevel; i++)
                        {
                            str2 += "│&nbsp;";
                        }
                        Rn += str2 + "│&nbsp;├";
                    }
                    Rn += fi.Name + "<br />";
                }
                Rn = this.ListTreeShow(di, nLevel + 1, Rn);
            }
            return Rn;
        }

        /// <summary>
        /// 获取文件树状结构文本
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="nLevel"></param>
        /// <param name="Rn"></param>
        /// <param name="tplPath">选择的文件</param>
        /// <returns></returns>
        public string ListTreeShow(DirectoryInfo dir, int nLevel, string Rn, string tplPath)
        {
            var dirs = dir.GetDirectories();
            foreach (var di in dirs)
            {
                Rn += "<option value=\"" + di.Name + "\"";
                if (tplPath.ToLower() == di.Name.ToLower())
                {
                    Rn+= " selected ";
                }
                Rn += ">";
                if (nLevel == 0)
                {
                    Rn += "┣";
                }
                else
                {
                    string str = "";
                    for (int i = 1; i <= nLevel; i++)
                    {
                        str += "│&nbsp;";
                    }
                    Rn += str + "┣";
                }
                Rn += di.Name + "</option>";
                foreach (FileInfo fi in di.GetFiles())
                {
                    string str3 = Rn;
                    Rn = str3 + "<option value=\"" + di.Name + "/" + fi.Name + "\"";
                    if (tplPath.ToLower() == fi.Name.ToLower())
                    {
                        Rn += " selected ";
                    }
                    Rn += ">";
                    if (nLevel == 0)
                    {
                        Rn += "│&nbsp;├";
                    }
                    else
                    {
                        string str2 = "";
                        for (int j = 1; j <= nLevel; j++)
                        {
                            str2 += "│&nbsp;";
                        }
                        Rn += str2 + "│&nbsp;├";
                    }
                    Rn += fi.Name + "</option>";
                }
                Rn = this.ListTreeShow(di, nLevel + 1, Rn, tplPath);
            }
            return Rn;
        }

    }
}
