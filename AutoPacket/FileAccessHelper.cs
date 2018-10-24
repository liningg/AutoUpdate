using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPacket
{
    class FileAccessHelper
    {
        private static ArrayList dirs = new ArrayList();
        //获取所有文件名
        private static ArrayList GetFileName(string dirPath)
        {
            ArrayList list = new ArrayList();

            if (Directory.Exists(dirPath))
            {
                list.Add(dirPath);
                list.AddRange(Directory.GetFiles(dirPath));
            }
            return list;
        }
        //获取所有文件夹及子文件夹
        private static void GetDirs(string dirPath)
        {
            if (Directory.GetDirectories(dirPath).Length > 0)
            {
                foreach (string path in Directory.GetDirectories(dirPath))
                {
                    dirs.Add(path);
                    GetDirs(path);
                }
            }
        }
        /// <summary>
        /// 获取给出文件夹及其子文件夹下的所有文件名
        /// （文件名为路径加文件名及后缀,
        /// 使用的时候GetAllFileName().ToArray()方法可以转换成object数组
        /// 之后再ToString()分别得到文件名）
        /// </summary>
        /// <param name="rootPath">文件夹根目录</param>
        /// <returns></returns>
        public static ArrayList GetAllFileName(string rootPath)
        {
            dirs.Clear();
            dirs.Add(rootPath);
            GetDirs(rootPath);
            object[] allDir = dirs.ToArray();
            ArrayList list = new ArrayList();
            foreach (object o in allDir)
            {
                list.AddRange(GetFileName(o.ToString()));
            }
            return list;
        }
        public static string GetFileUpdateTime(string filepath)
        {
            string ext = Path.GetExtension(filepath);
            if(string.IsNullOrEmpty(ext))
            {
                DirectoryInfo dir = new DirectoryInfo(filepath);
                return dir.LastWriteTime.ToString();
            }
            else
            {
                FileInfo fi = new FileInfo(filepath);
                return fi.LastWriteTime.ToString();

            }
           
        }
    }
}
