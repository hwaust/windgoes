using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Xml;

namespace WindGoes.IO
{
    /// <summary>
    /// 给定文件路径，返回路径下的所有文件的相对路径，大小和MD，并保存为XML文件。还能对XML进行差异比较。
    /// </summary>
    public class FileInfoSaver : ObjectBase
    {
        #region 属性
        private ArrayList alFullPath = null;//保存所有读取的文件绝对路径
        /// <summary>
        /// 保存所有读取的文件绝对路径
        /// </summary>
        public ArrayList AlFullPath
        {
            get { return alFullPath; }
            set { alFullPath = value; }
        }

        private string path = "";
        /// <summary>
        /// 需要提取文件信息的路径
        /// </summary>
        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        private string xmlPath = "";
        /// <summary>
        /// Xml文件保存的路径
        /// </summary>
        public string XmlPath
        {
            get { return xmlPath; }
            set { xmlPath = value; }
        }

        private string[] colName = { "FilePath", "FileSize", "FileMD5" };


        #endregion

        #region 构造函数
        /// <summary>
        /// 给定文件路径，返回路径下的所有文件的相对路径，大小和MD，并保存为XML文件。还能对XML进行差异比较。
        /// </summary>
        public FileInfoSaver() { }

        /// <summary>
        /// 给定文件路径，返回路径下的所有文件的相对路径，大小和MD，并保存为XML文件。还能对XML进行差异比较。
        /// </summary>
        /// <param name="path">需要处理的文件路径。</param>
        /// <param name="xmlFileName">导出的XML文件路径。</param>
        public FileInfoSaver(string path, string xmlFileName)
        {
            this.path = path;
            this.xmlPath = path + "\\" + xmlFileName;
            alFullPath = new ArrayList();
            GetAllDirList(path);
        }
        #endregion


        /// <summary>
        /// 获取strBaseDir路径下所有文件（包括子目录下）的据对路径，保存在AlFullPath数组中
        /// </summary>
        /// <param name="strBaseDir">需要提取文件信息的路径</param>
        private void GetAllDirList(string strBaseDir)
        {

            DirectoryInfo di = new DirectoryInfo(strBaseDir);
            DirectoryInfo[] diA = di.GetDirectories();
            for (int i = 0; i < diA.Length; i++)
            {
                alFullPath.Add(diA[i].FullName);
                //diA[i].FullName是某个子目录的绝对地址，把它记录在ArrayList中
                GetAllDirList(diA[i].FullName);
                //注意：递归了。逻辑思维正常的人应该能反应过来
            }
        }

        /// <summary>
        /// 根据所给的文件目录，获取该目录下所有文件的相对路径
        /// </summary>
        /// <param name="dir">目录</param>
        /// <returns>文件相对路径数组</returns>
        public string[] ReadFile(string dir)
        {
            DirectoryInfo di = new DirectoryInfo(dir);
            string[] filepath = new string[di.GetFiles().Length];
            int i = 0;
            foreach (FileSystemInfo fi in di.GetFileSystemInfos())
            {
                if (!(fi is DirectoryInfo))
                {
                    filepath[i++] = fi.FullName.Substring(path.Length);
                    //clbcUpdateFileName.Items.Add();
                }
            }
            return filepath;
        }

        // 比较两个文件中的xn节点的内容是否相同，如果相同就返回1，不相同就返回0
        private int Compare(XmlNode xn, string xmlPath)
        {
            int value = 0;
            try
            {
                XmlNodeList oldxnlist = ReadXml(xmlPath, "/Update/FileInfos/FileInfo");
                if (oldxnlist != null)
                    foreach (XmlNode item in oldxnlist)
                    {
                        if (item.Attributes[0].Value == xn.Attributes[0].Value)
                            if (item.Attributes[1].Value == xn.Attributes[1].Value &&
                                item.Attributes[2].Value == xn.Attributes[2].Value)
                            {
                                value = 1;
                                continue;
                            }
                            else
                                value = 0;
                    }
            }
            catch
            {
            }
            return value;
        }

        /// <summary>
        /// 根据xml的文件路径，读取更新时间
        /// </summary>
        /// <param name="xmlPath">xml文件路径</param>
        /// <returns>时间字符串</returns>
        private string ReadUpdateDateTime(string xmlPath)
        {
            XmlNodeList xnlist = ReadXml(xmlPath, "/Update/Head");
            string str = "";
            if (xnlist != null)
                foreach (XmlNode item in xnlist)
                {
                    XmlElement xe = (XmlElement)item;
                    str = xe.GetAttribute("UpdateDateTime");
                    //str=item.Attributes[0].Value
                }
            return str;
        }

        /// <summary>
        /// 根据文件路径，读取xml文件中的xml节点路径xpath
        /// </summary>
        /// <param name="xmlPath">xml文件路径</param>
        /// <param name="xpath">xml文件中的节点路径</param>
        /// <returns></returns>
        private XmlNodeList ReadXml(string xmlPath, string xpath)
        {
            XmlNodeList xnlist = null;
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(xmlPath);
                xnlist = xdoc.SelectNodes(xpath);
                return xnlist;
            }
            catch
            {
                return xnlist;
            }
        }

        /// <summary>
        /// 根据所给文件相对路径数组，把相关信息写入Xml文件中
        /// </summary>
        /// <param name="fileFullName">文件的相对路径（包含文件全名）</param>
        public void WriteXml(string[] fileFullName)
        {
            XmlDocument doc = new XmlDocument();

            XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "gb2312", "yes");
            doc.AppendChild(declaration);
            XmlElement docRoot = doc.CreateElement("Update");
            doc.AppendChild(docRoot);

            XmlNode nodehead = doc.CreateElement("Head");
            XmlAttribute nodeAtth = doc.CreateAttribute("UpdateDateTime");
            nodeAtth.Value = DateTime.Now.ToString();
            nodehead.Attributes.Append(nodeAtth);
            docRoot.AppendChild(nodehead);

            XmlElement el = doc.CreateElement("FileInfos");
            docRoot.AppendChild(el);

            for (int i = 0; i < fileFullName.Length; i++)
            {
                try
                {
                    string filepath = path + fileFullName[i];
                    FileInfo fi = new FileInfo(filepath);

                    XmlNode node = doc.CreateElement("FileInfo");
                    XmlAttribute nodeAtt1 = doc.CreateAttribute(colName[0]);
                    nodeAtt1.Value = fileFullName[i];
                    XmlAttribute nodeAtt2 = doc.CreateAttribute(colName[1]);
                    nodeAtt2.Value = fi.Length.ToString();
                    XmlAttribute nodeAtt3 = doc.CreateAttribute(colName[2]);
                    nodeAtt3.Value = WindGoes.Data.MDFive.FileHash(filepath);
                    node.Attributes.Append(nodeAtt1);
                    node.Attributes.Append(nodeAtt2);
                    node.Attributes.Append(nodeAtt3);
                    el.AppendChild(node);
                }
                catch { }
            }
            //保存xml文档
            doc.Save(XmlPath);

        }

        /// <summary>
        /// 根据所给文件相对路径数组，把相关信息写入Xml文件中
        /// </summary>
        /// <param name="fileFullName">文件的相对路径（包含文件全名），string[]类型</param>
        /// <param name="filesize">文件大小，string[]类型</param>
        /// <param name="fileMD5">文件的MD5值，string[]类型</param>
        public void WriteXml(string[] fileFullName, string[] filesize, string[] fileMD5)
        {
            XmlDocument doc = new XmlDocument();

            XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "gb2312", "yes");
            doc.AppendChild(declaration);
            XmlElement docRoot = doc.CreateElement("Update");
            doc.AppendChild(docRoot);

            XmlNode nodehead = doc.CreateElement("Head");
            XmlAttribute nodeAtth = doc.CreateAttribute("UpdateDateTime");
            nodeAtth.Value = DateTime.Now.ToString();
            nodehead.Attributes.Append(nodeAtth);
            docRoot.AppendChild(nodehead);

            XmlElement el = doc.CreateElement("FileInfos");
            docRoot.AppendChild(el);

            for (int i = 0; i < fileFullName.Length; i++)
            {
                try
                {
                    XmlNode node = doc.CreateElement("FileInfo");
                    XmlAttribute nodeAtt1 = doc.CreateAttribute(colName[0]);
                    nodeAtt1.Value = fileFullName[i];
                    XmlAttribute nodeAtt2 = doc.CreateAttribute(colName[1]);
                    nodeAtt2.Value = filesize[i];
                    XmlAttribute nodeAtt3 = doc.CreateAttribute(colName[2]);
                    nodeAtt3.Value = fileMD5[i];
                    node.Attributes.Append(nodeAtt1);
                    node.Attributes.Append(nodeAtt2);
                    node.Attributes.Append(nodeAtt3);
                    el.AppendChild(node);
                }
                catch { }
            }
            //保存xml文档
            doc.Save(XmlPath);
        }

        /// <summary>
        /// 根据所给文件相对路径数组，把相关信息写入Xml文件中
        /// </summary>
        /// <param name="fileFullName">文件的相对路径（包含文件全名），ArrayList类型</param>
        /// <param name="filesize">文件大小，ArrayList类型</param>
        /// <param name="fileMD5">文件的MD5值，ArrayList类型</param>
        public void WriteXml(List<string> fileFullName, List<string> filesize, List<string> fileMD5)
        {
            XmlDocument doc = new XmlDocument();

            XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "gb2312", "yes");
            doc.AppendChild(declaration);
            XmlElement docRoot = doc.CreateElement("Update");
            doc.AppendChild(docRoot);

            XmlNode nodehead = doc.CreateElement("Head");
            XmlAttribute nodeAtth = doc.CreateAttribute("UpdateDateTime");
            nodeAtth.Value = DateTime.Now.ToString();
            nodehead.Attributes.Append(nodeAtth);
            docRoot.AppendChild(nodehead);

            XmlElement el = doc.CreateElement("FileInfos");
            docRoot.AppendChild(el);

            for (int i = 0; i < fileFullName.Count; i++)
            {
                try
                {
                    XmlNode node = doc.CreateElement("FileInfo");
                    XmlAttribute nodeAtt1 = doc.CreateAttribute(colName[0]);
                    nodeAtt1.Value = fileFullName[i];
                    XmlAttribute nodeAtt2 = doc.CreateAttribute(colName[1]);
                    nodeAtt2.Value = filesize[i];
                    XmlAttribute nodeAtt3 = doc.CreateAttribute(colName[2]);
                    nodeAtt3.Value = fileMD5[i];
                    node.Attributes.Append(nodeAtt1);
                    node.Attributes.Append(nodeAtt2);
                    node.Attributes.Append(nodeAtt3);
                    el.AppendChild(node);
                }
                catch { }
            }
            //保存xml文档
            doc.Save(XmlPath);
        }

        /// <summary>
        /// 更新文件，检查那些文件是否要更新的
        /// </summary>
        /// <param name="oldXmlPath">本机的Xml文件路径</param>
        /// <param name="newXmlPath">最新下载的Xml文件路径</param>
        public string[] GetFilePath(string oldXmlPath, string newXmlPath)
        {
            List<string> filePath = new List<string>();
            if (!ReadUpdateDateTime(newXmlPath).Equals(""))
            {
                if (!ReadUpdateDateTime(xmlPath).Equals(ReadUpdateDateTime(oldXmlPath)))
                {
                    XmlNodeList oldxnlist = ReadXml(newXmlPath, "/Update/FileInfos/FileInfo");

                    if (oldxnlist != null)
                        foreach (XmlNode item in oldxnlist)
                        {
                            if (Compare(item, oldXmlPath) == 0)
                            {
                                filePath.Add(item.Attributes[0].Value);
                                //该文件已修改，需要更新
                            }
                        }

                }
            }
            return filePath.ToArray();
        }
    }
}
