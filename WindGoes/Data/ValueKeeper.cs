using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WindGoes.Data
{
    public class ValueKeeper
    {
        Dictionary<string, string> values = new Dictionary<string, string>();

        /// <summary>
        /// 将所有内容保存为一个字符串。
        /// </summary>
        /// <returns></returns>
        public string ToFileString()
        {
            StringBuilder builder = new StringBuilder();

            foreach (string key in values.Keys)
            {
                builder.Append(key + "=" + values[key] + "\r\n");
            }

            for (int i = 0; i < values.Keys.Count; i++)
            { 
                if (i != values.Keys.Count - 1)
                    builder.Append("\r\n");
            }

            return builder.ToString();
        }

        /// <summary>
        /// 从文件加载数据。
        /// </summary>
        /// <param name="path">文件所在路径。</param>
        public void LoadData(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    Console.WriteLine("File doesn't exist.");
                    //System.Windows.Forms.MessageBox.Show("Language file doesn't exist.");
                    return;
                }

                values.Clear();
                using (StreamReader sr = new StreamReader(path, Encoding.Default))
                {
                    while (!sr.EndOfStream)
                    {
                        try
                        {
                            string s = sr.ReadLine();
                            if (string.IsNullOrEmpty(s) || s.Length < 2 )
                                continue;

                            int p = s.IndexOf('=');

                            if (char.IsLetter(s[0]) || p >= 0)
                                values.Add(s.Substring(0, p), s.Substring(p+1));
                        }
                        catch { }
                    }
                }

            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.Message);
            }
        }

        /// <summary>
        /// 从指定格式的字符串中加载数据。
        /// </summary>
        /// <param name="content"></param>
        public void LoadDataFromString(string content)
        {
            values.Clear();

            try
            {
                string[] data = content.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string s in data)
                {
                    if (string.IsNullOrEmpty(s) || s.Length < 2)
                        continue;

                    int p = s.IndexOf('=');

                    if (char.IsLetter(s[0]) || p >= 0)
                        values.Add(s.Substring(0, p), s.Substring(p + 1));
                }
            }
            catch { }
        }

        /// <summary>
		/// 获取指定键的值，注意：null表示值不存在，而String.Empty表示存在，但是为空字符串, 但是Key中不能有'='号。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            if (values.ContainsKey(key))
            {
                return values[key];
            }

            return null;
        }

        /// <summary>
        /// 添加值，如果键已经存在，则改变现有键的值，但是Key中不能有'='号。
        /// </summary>
        /// <param name="key">键值。</param>
        /// <param name="value">值。</param>
        public void SetValue(string key, string value)
        {
            if (values.ContainsKey(key))
            {
                values[key] = value;
            }
            else
            {
                values.Add(key, value);
            }
        }



        /// <summary>
        /// 从指定加密的文件中加载数据。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool LoadDesString(string path)
        {
            try
            {
                string data = FileHelper.LoadFromFile(path);

                if (data == null)
                    return false;

                data = DESCrypto.Decrypt(data);

                LoadDataFromString(data);
            }
            catch { return false; }
            return true;
        }
    }
}
