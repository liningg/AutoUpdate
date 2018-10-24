using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static AutoPacket.VersionModel;

namespace AutoPacket
{
    public partial class Form1 : Form
    {
        private string itemName = string.Empty;
        private string packetText = string.Empty;
        private VersionModel verModle;
         public Form1()
        {
            InitializeComponent();
            verModle = new VersionModel();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            packetText = this.Text;
            listViewFileList.Columns.Clear();
            listViewFileList.Columns.Add("文件名", 120, HorizontalAlignment.Left); //一步添加  
            listViewFileList.Columns.Add("版本号", 120, HorizontalAlignment.Left); //一步添加 
            listViewFileList.Columns.Add("最后更新日期", 220, HorizontalAlignment.Left); //一步添加 
            string root = Environment.CurrentDirectory.ToString();
            if (!Directory.Exists(root + "/BackUp"))
            {
                Directory.CreateDirectory(root + "/BackUp");
            }
            //ReadConfig();
        }
        private void ReadConfig(string path)
        {
            XmlDocument xd = new XmlDocument();
            string fileName = path;
            try
            {
                verModle = new VersionModel();
                xd.Load(fileName);
                 
                verModle.LastTime = xd.SelectSingleNode("//LastUpdateTime").InnerText;
                verModle.Version = xd.SelectSingleNode("//Version").InnerText;
                verModle.PubNumber = xd.SelectSingleNode("//PubNumber").InnerText;
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
                    verModle.fileDic[name] = file;

                }
            }
            catch(Exception e)
            {
                LogClass.WriteLogFile("readconfig error:" + e.Message);
            }
           
        }

        private void buttonPublish_Click(object sender, EventArgs e)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                //Create the xml declaration first   
                xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
                //Create the root node and append into doc   
                var AutoUpdate = xmlDoc.CreateElement("AutoUpdate");
                xmlDoc.AppendChild(AutoUpdate);
                // Contact  
                XmlElement LastUpdateTime = xmlDoc.CreateElement("LastUpdateTime");
                //XmlAttribute attrID = xmlDoc.CreateAttribute("id");
                LastUpdateTime.InnerText = verModle.LastTime;
                AutoUpdate.AppendChild(LastUpdateTime);
                // Contact Name   
                XmlElement Version = xmlDoc.CreateElement("Version");
                Version.InnerText = verModle.Version;
                AutoUpdate.AppendChild(Version);

                XmlElement PubNumber = xmlDoc.CreateElement("PubNumber");
                PubNumber.InnerText = verModle.PubNumber;
                AutoUpdate.AppendChild(PubNumber);
                // Contact Gender   
                XmlElement UpdateFileList = xmlDoc.CreateElement("UpdateFileList");
                foreach (var item in verModle.fileDic)
                {
                    XmlElement File = xmlDoc.CreateElement("File");
                    XmlAttribute Ver = xmlDoc.CreateAttribute("Ver");
                    Ver.Value = item.Value.version;
                    File.Attributes.Append(Ver);
                    XmlAttribute LastTime = xmlDoc.CreateAttribute("LastTime");
                    LastTime.Value = item.Value.lastime;
                    File.Attributes.Append(LastTime);
                    XmlAttribute Name = xmlDoc.CreateAttribute("Name");
                    Name.Value = item.Key;
                    File.Attributes.Append(Name);
                    XmlAttribute pubnumber = xmlDoc.CreateAttribute("PubNumber");
                    pubnumber.Value = item.Value.pubnumber;
                    File.Attributes.Append(pubnumber);

                    UpdateFileList.AppendChild(File);
                }
                
                AutoUpdate.AppendChild(UpdateFileList);
                xmlDoc.Save(itemName + "_UpdateVersion.xml");
                if(File.Exists("BackUp/" + itemName + "_UpdateVersion.xml"))
                {
                    File.Delete("BackUp/" + itemName + "_UpdateVersion.xml");
                }
                File.Copy(itemName + "_UpdateVersion.xml", "BackUp/" + itemName + "_UpdateVersion.xml");
            }
            catch(Exception ex)
            {
                MessageBox.Show("发布失败：" + ex.Message);
                return;
            }
            MessageBox.Show("发布成功:" + itemName + "_UpdateVersion.xml");
        }

        private void buttonScanFile_Click(object sender, EventArgs e)
        {
            string root = Environment.CurrentDirectory.ToString();
            string selectPath = "";
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.SelectedPath = root;
            dialog.Description = "请选择Txt所在文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    return;
                }
            }
            else
            {
                return;
            }
            listViewFileList.Items.Clear();
            listViewFileList.Update();
            
            selectPath = dialog.SelectedPath;
            itemName = selectPath.Substring(selectPath.LastIndexOf(@"\") + 1);
            ReadConfig(itemName + "_UpdateVersion.xml");
            this.Text = packetText + "    " + selectPath;
           
            ArrayList arr = FileAccessHelper.GetAllFileName(selectPath);
            bool update = false;
            verModle.PubNumber = (Convert.ToInt32(verModle.PubNumber) + 1).ToString();
            foreach (var item in arr)
            {
                ListViewItem lvi = new ListViewItem();
                string stritem = item.ToString().Substring(root.Length, item.ToString().Length - root.Length);
                if (!verModle.fileDic.ContainsKey(stritem.ToString()))
                {
                    FileDetails file = new FileDetails();
                    file.lastime = FileAccessHelper.GetFileUpdateTime(root + stritem.ToString());
                    file.version = "1";
                    file.pubnumber = verModle.PubNumber;
                    verModle.fileDic[stritem.ToString()] = file;
                    if(!update)
                    {
                        verModle.LastTime = DateTime.Now.ToString();
                        verModle.Version = (Convert.ToInt32(verModle.Version) + 1).ToString();
                        update = true;
                        
                    }
                    lvi.BackColor = Color.Pink;

                }
                else
                {
                    string lasttime = FileAccessHelper.GetFileUpdateTime(root + stritem.ToString());
                    verModle.fileDic[stritem.ToString()].pubnumber = verModle.PubNumber;
                    if (verModle.fileDic[stritem.ToString()].lastime != lasttime)
                    {
                        verModle.fileDic[stritem.ToString()].version = (Convert.ToInt32(verModle.fileDic[stritem.ToString()].version) + 1).ToString();
                        verModle.fileDic[stritem.ToString()].lastime = lasttime;
                        if (!update)
                        {
                            verModle.LastTime = DateTime.Now.ToString();
                            verModle.Version = (Convert.ToInt32(verModle.Version) + 1).ToString();
                            update = true;
                            
                        }
                        lvi.BackColor = Color.Pink;

                    }
                }
                
                lvi.Text = stritem.ToString();
                lvi.SubItems.Add(verModle.fileDic[stritem.ToString()].version);
                lvi.SubItems.Add(verModle.fileDic[stritem.ToString()].lastime);
                listViewFileList.Items.Add(lvi);
            }
        }

        private void buttonForceUpdate_Click(object sender, EventArgs e)
        {
            //verModle.ForceUpdate = "1";
        }

        private void buttonWhole_Click(object sender, EventArgs e)
        {
            //verModle.WholeUpdate = "1";
        }

        private void buttonInitVersion_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("版本初始化将会删掉版本配置文件，确认初始化？", "提示", MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                if(File.Exists("UpdateVersion.xml"))
                {
                    File.Delete("UpdateVersion.xml");
                }
                verModle = new VersionModel();
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            listViewFileList.Items.Clear();
        }
    }
}
