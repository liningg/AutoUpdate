using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Configuration;
using System.Xml;
using static AutoUpdate.VersionModel;
using System.Collections;
using System.IO;
using System.Diagnostics;

namespace AutoUpdate
{
    public partial class Form1 : Form
    {
        private string updateProcess = string.Empty;
        private string updateFile = string.Empty;
        private string ftpaddress = string.Empty;
        private string ftpaccount = string.Empty;
        private string ftppassword = string.Empty;
        private string updateProcessPath = string.Empty;
        private string isStartAfterUpdate = string.Empty;
        private VersionModel verModle = new VersionModel();
        private FTPHelper ftp = null;
        private ArrayList arrUpdate = null;
        private VersionModel verStdModle = new VersionModel();
        /// <summary>
        /// 1, 0:不显示没有更新提示框   1. 显示没有更新的提示框
        ///     
        /// </summary>
        private string[] args = null;
        public Form1()
        {
            InitializeComponent();
        }
        public Form1(string[] args)
        {
            InitializeComponent();
            this.args = args;

        }


        private void Form1_Load(object sender, EventArgs e)
        {
            ftpaddress = ConfigurationManager.AppSettings["FtpAddress"];
            ftpaccount = ConfigurationManager.AppSettings["FtpAccount"];
            ftppassword = ConfigurationManager.AppSettings["FtpPassword"];
            updateFile = ConfigurationManager.AppSettings["UpdateFile"];
            updateProcess = ConfigurationManager.AppSettings["UpdateProcess"];
            updateProcessPath = ConfigurationManager.AppSettings["UpdateProcessPath"];
            isStartAfterUpdate = ConfigurationManager.AppSettings["IsStartAfterUpdate"];
            string path = AppDomain.CurrentDomain.BaseDirectory;
            if(!Directory.Exists(path+"BackUp"))
            {
                Directory.CreateDirectory(path + "BackUp");
            }
            //MessageBox.Show(path);
            verModle = ReadConfig(path  + "UpdateVersion.xml");
            
            string passive = ConfigurationManager.AppSettings["Passive"];
            //下载更新文件
            ftp = new FTPHelper(ftpaddress, "", ftpaccount, ftppassword, passive== "1"?true:false);
            bool ret = ftp.Download(path, updateFile);
            if (ret)
            {
                listBoxTip.Items.Clear();
                //开始解析文件，查看哪些文件需要更新
                arrUpdate = ParseVersionFile(path + updateFile);
                foreach(var item in arrUpdate)
                {
                    listBoxTip.Items.Add(item);
                }
                progressBar1.Maximum = arrUpdate.Count;
                if (arrUpdate.Count > 0)
                {
                    DialogResult dr = MessageBox.Show("有更新文件，确认更新？", "提示", MessageBoxButtons.OKCancel);
                    if (dr == DialogResult.OK)
                    {
                        if(KillProcess())
                        {
                            Thread.Sleep(2000);
                            Thread td = new Thread(WorkThread);
                            td.Start();
                        }
                        else
                        {
                            MessageBox.Show("请关闭更新进程,然后手动执行更新程序");
                        }
                       
                    }
                    else
                    {
                        this.Close();
                    }
                }
                else
                {
                    if (File.Exists(path + updateFile))
                    {
                        File.Delete(path + updateFile);
                    }
                    //File.Move(path + updateFile, path + "UpdateVersion.xml");
                    
                    if(args != null && args.Length >0 && args[0] == "1")
                    {
                        MessageBox.Show("项目工程没有更新！");
                    }
                    
                    this.Close();
                }
                
            }
            else
            {
                MessageBox.Show("下载文件出错" + updateFile);
            }
            
        }
        private bool KillProcess()
        {
            bool ret = false;
            try
            {
                
                Process[] processes = Process.GetProcesses();
                foreach (Process p in processes)
                {
                    if (updateProcess.IndexOf(p.ProcessName)>=0)
                    {
                        p.Kill();
                    }
                }
                ret = true;
            }
            catch (Exception e)
            {
                MessageBox.Show("强制关闭程序失败：" + e.Message);
                ret = false;
            }
            return ret;
        }
        private VersionModel ReadConfig(string filepath)
        {
            VersionModel version = new VersionModel();
            XmlDocument xd = new XmlDocument();
            string fileName = filepath;
            try
            {
                xd.Load(fileName);

                version.LastTime = xd.SelectSingleNode("//LastUpdateTime").InnerText;
                version.Version = xd.SelectSingleNode("//Version").InnerText;
                version.PubNumber = xd.SelectSingleNode("//PubNumber").InnerText;
                XmlNodeList FileList = xd.SelectNodes("//UpdateFileList/File");
                foreach (XmlNode xn in FileList)
                {
                    string ver = xn.Attributes["Ver"].Value;
                    string lasttime = xn.Attributes["LastTime"].Value;
                    string name = xn.Attributes["Name"].Value;
                    string pubnumber = xn.Attributes["PubNumber"].Value;
                    FileDetails file = new FileDetails();
                    file.lastime = lasttime;
                    file.version = ver;
                    file.pubnumber = pubnumber;
                    version.fileDic[name] = file;

                }
            }
            catch (Exception e)
            {
                LogClass.WriteLogFile("readconfig error:" + e.Message);
            }
            return version;
        }
        private ArrayList ParseVersionFile(string file)
        {
            ArrayList arr = new ArrayList();
            string root = AppDomain.CurrentDomain.BaseDirectory;
            //string root = Environment.CurrentDirectory.ToString();
            DirectoryInfo di1 = new DirectoryInfo(root);
            DirectoryInfo di2 = di1.Parent.Parent;
            root = di2.FullName;
            try
            {
                verStdModle = ReadConfig(file);
              
                //判断文件主版本是否已经改变，需要更新
                if (verStdModle.Version != verModle.Version)
                {
                    foreach (var item in verStdModle.fileDic)
                    {
                        if (verStdModle.PubNumber == item.Value.pubnumber)
                        {
                            if (verModle.fileDic.ContainsKey(item.Key.ToString()))
                            {

                                if (verModle.fileDic[item.Key.ToString()].version != item.Value.version.ToString())
                                {
                                    arr.Add(item.Key.ToString());
                                }

                            }
                            else
                            {
                                arr.Add(item.Key.ToString());
                            }
                        }
                        else
                        {
                            //发布计数不相等，需要删除这个文件
                            string ext = Path.GetExtension(root + item.Key.ToString());
                            if (string.IsNullOrEmpty(ext))
                            {
                                if (Directory.Exists(root + item.Key.ToString()))
                                {
                                    DirectoryInfo di = new DirectoryInfo(root + item.Key.ToString());
                                    if (di.GetFiles().Length > 0)
                                    {
                                        //di.Delete(true);
                                    }
                                    else
                                    {
                                        //Directory.Delete(root + item.Key.ToString());
                                    }
                                }
                            }
                            else
                            {
                                if (File.Exists(root + item.Key.ToString()))
                                {
                                    //File.Delete(root + item.Key.ToString());
                                }

                            }


                        }

                    }
                }
                
                
            }
            catch(Exception e)
            {
                LogClass.WriteLogFile("ParseVersionFile error :" + e.Message);
            }
            return arr;
        }
        private void WorkThread()
        {
            try
            {
                string root1 = AppDomain.CurrentDomain.BaseDirectory;
                DirectoryInfo di1 = new DirectoryInfo(root1);
                DirectoryInfo di2 = di1.Parent.Parent;
                string root = di2.FullName;
                //Directory.SetCurrentDirectory(root);
                bool update = false;
                int i = 0;
                foreach (var item in arrUpdate)
                {
                    //判断是否是目录
                    string ext = Path.GetExtension(item.ToString());
                    if (string.IsNullOrEmpty(ext))
                    {
                        if (!Directory.Exists(root + item.ToString()))
                        {
                            Directory.CreateDirectory(root + item.ToString());
                            this.Invoke((EventHandler)delegate
                            {
                                listBoxTip.Items.Add(item.ToString());
                            });
                        }
                    }
                    else
                    {
                        string path = Path.GetDirectoryName(item.ToString()) + "\\";
                        string name = Path.GetFileName(item.ToString());
                        if(File.Exists(path + item.ToString()))
                        {
                            File.Delete(path + item.ToString());
                        }
                        bool ret = ftp.Download(root, item.ToString());
                        if (ret)
                        {
                            this.Invoke((EventHandler)delegate
                            {
                                listBoxTip.Items.Add(item.ToString());
                            });
                        }
                        else
                        {
                            LogClass.WriteLogFile("download file error:" + item.ToString());
                        }
                    }
                    i++;
                    this.Invoke((EventHandler)delegate
                    {
                        progressBar1.Value = i;
                    });
                    listBoxTip.TopIndex = listBoxTip.Items.Count - 1;
                }
                this.Invoke((EventHandler)delegate
                {
                    MessageBox.Show("更新完成");
                    if(File.Exists(root1 + "UpdateVersion.xml"))
                    {
                        File.Delete(root1 + "UpdateVersion.xml");
                    }
                    Thread.Sleep(100);
                    File.Move(root1 + updateFile, root1 + "UpdateVersion.xml");
                   
                    
                    if(File.Exists(root1 + "BackUp/UpdateVersion.xml"))
                    {
                        File.Delete(root1 + "BackUp/UpdateVersion.xml");
                    }
                    File.Copy(root1 + "UpdateVersion.xml", root1 + "BackUp/UpdateVersion.xml");
                    if(isStartAfterUpdate == "1")
                    {
                        Process.Start(root1 + updateProcessPath);
                    }
                    
                    this.Close();
                });
            }
            catch (Exception e)
            {
                LogClass.WriteLogFile("thread error:" + e.Message);
            }
         
        }
    }
}
