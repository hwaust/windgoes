using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WindGoes
{
    /// <summary>
    /// 字符串的功能函数。
    /// </summary>
    public class StringHelper
    {
        static Random rnd = new Random();
        /// <summary>
        /// 随机类的对象。
        /// </summary>
        public static Random Rnd
        {
            get { return StringHelper.rnd; }
            set { StringHelper.rnd = value; }
        }

		/// <summary>
		/// 生成文件格式大小。
		/// </summary>
		/// <param name="byteCount"></param>
		/// <returns></returns>
        public static string ToFileSizeString(int byteCount)
        {
            return ToFileSizeString(Convert.ToInt64(byteCount));
        }

        /// <summary>
        /// 将文件生成指定格式的字符串。如1024字符=> 1K等等。
        /// </summary>
        /// <param name="byteCount"></param>
        /// <returns></returns>
        public static string ToFileSizeString(long byteCount)
        {
            decimal num = byteCount;
            if (num > 1073741824M)
            {
                decimal num2 = num / 1073741824M;
                return Convert.ToString(Math.Round(num2, 3).ToString("N") + "G").Replace(".00", "");
            }
            if (num > 1048576M)
            {
                decimal num3 = num / 1048576M;
                return Convert.ToString(Math.Round(num3, 3).ToString("N") + "M").Replace(".00", "");
            }
            decimal d = num / 1024M;
            return Convert.ToString(Math.Round(d, 3).ToString("N") + "K").Replace(".00", "");
        }



        /// <summary>
        /// 以某分隔符为界限，重新排序字符串。如："a = b;"  以'='为分隔符调用：ChangeOrder("a = b; ", '=');  返回结果为：  "b = a;"
        /// </summary>
        /// <param name="s">待转换的字符串。</param>
        /// <param name="c">分隔符。</param>
        /// <returns></returns>
        public static string ChangeStringOrder(string s, char c)
        {
            string[] lines = s.Replace('\r', ' ').Split('\n');
            StringBuilder sb = new StringBuilder();
            foreach (string str in lines)
            {
                try
                {
                    StringBuilder bd = new StringBuilder(str);
                    //找开始和结束位置。
                    int start = 0, end = bd.Length - 1;
                    for (int i = 0; i < str.Length; i++)
                    {
                        if (char.IsLetter(bd[i]))
                        {
                            start = i;
                            break;
                        }
                    }
                    for (int i = bd.Length - 1; i >= 0; i--)
                    {
                        if (char.IsLetter(bd[i]) || char.IsDigit(bd[i]))
                        {
                            end = i;
                            break;
                        }
                    }

                    int p = str.IndexOf(c);
                    if (p > 0 && p < str.Length)
                    {
                        string left = str.Substring(start, p - start);
                        string right = str.Substring(p + 1, end - p);
                        sb.Append(right.Trim() + " " + c + " " + left.Trim() + ";");
                    }
                    else
                        sb.Append(str);
                }
                catch { sb.Append(str); }
                sb.Append("\r\n");
            }

            return sb.ToString();
        }


        /// <summary>
        /// 返回一个字符串的编号，前提是这个字符的格式是数字+.+名称，如： 3.淮南 。如果字符串不合法，返回-1.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int GetIDFromString(string s)
        {
            int p = s.IndexOf('.');
            return p >= 0 ? int.Parse(s.Substring(0, p)) : -1;
        }

        /// <summary>
        /// 返回一个字符串的编号，前提是这个字符的格式是数字+分隔符+后缀字符串。
        /// </summary>
        /// <param name="s">需要拆分的字符串。</param>
        /// <param name="sp">分隔符。</param>
        /// <returns></returns>
        public static int GetIDFromString(string s, char sp)
        {
            int p = s.IndexOf('.');
            return p >= 0 ? int.Parse(s.Substring(0, p)) : -1;
        }

        /// <summary>
        /// 统计子串在指定字符串中的个数, 0，-1表示不存在。
        /// </summary>
        /// <param name="source">源字符串。</param>
        /// <param name="substring">子串。</param>
        /// <returns></returns>
        public static int SubStringCount(string source, string substring)
        {
            return source.Split(new string[] { substring }, StringSplitOptions.None).Length - 1;
        } 

        /// <summary> 
        /// 半角字串转为全角字符串，ETC表示：English To Chinese。
        /// </summary> 
        /// <param name="englishString">英文的半角字符串。</param> 
        /// <returns>全角字符串</returns> 
        ///<remarks> 
        ///全角空格为12288，半角空格为32 
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248 
        ///</remarks> 
        public static string ETC(string englishString)
        {
            //半角转全角： 
            char[] c = englishString.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }



        /// <summary> 
        /// 全角字串转为半角字符串，CTE表示：Chinese To English。 
        /// </summary> 
        /// <param name="chineseString">中文的全角字符串。</param> 
        /// <returns>半角字符串</returns> 
        ///<remarks> 
        ///全角空格为12288，半角空格为32 
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248 
        ///</remarks> 
        public static string CTE(string chineseString)
        {
            char[] c = chineseString.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

        /// <summary> 
        /// 将数字表示的人民币金额转换为中文表示的金额，如 33.36 转换成  叁拾叁元叁角陆分
        /// </summary> 
        /// <param name="num">金额</param> 
        /// <returns>返回大写形式</returns> 
        public static string RMBConverter(double num)
        {
            string str1 = "零壹贰叁肆伍陆柒捌玖";            //0-9所对应的汉字 
            string str2 = "万仟佰拾亿仟佰拾万仟佰拾元角分"; //数字位所对应的汉字 
            string str3 = "";    //从原num值中取出的值 
            string str4 = "";    //数字的字符串形式 
            string str5 = "";  //人民币大写金额形式 
            int i;    //循环变量 
            int j;    //num的值乘以100的字符串长度 
            string ch1 = "";    //数字的汉语读法 
            string ch2 = "";    //数字位的汉字读法 
            int nzero = 0;  //用来计算连续的零值是几个 
            int temp;            //从原num值中取出的值 

            num = Math.Round(Math.Abs(num), 2);    //将num取绝对值并四舍五入取2位小数 
            str4 = ((long)(num * 100)).ToString();        //将num乘100并转换成字符串形式 
            j = str4.Length;      //找出最高位 
            if (j > 15) { return "溢出"; }
            str2 = str2.Substring(15 - j);   //取出对应位数的str2的值。如：200.55,j为5所以str2=佰拾元角分 

            //循环取出每一位需要转换的值 
            for (i = 0; i < j; i++)
            {
                str3 = str4.Substring(i, 1);       //取出需转换的某一位的值 
                temp = Convert.ToInt32(str3);      //转换为数字 
                if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                {
                    //当所取位数不为元、万、亿、万亿上的数字时 
                    if (str3 == "0")
                    {
                        ch1 = "";
                        ch2 = "";
                        nzero = nzero + 1;
                    }
                    else
                    {
                        if (str3 != "0" && nzero != 0)
                        {
                            ch1 = "零" + str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                    }
                }
                else
                {
                    //该位是万亿，亿，万，元位等关键位 
                    if (str3 != "0" && nzero != 0)
                    {
                        ch1 = "零" + str1.Substring(temp * 1, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (str3 != "0" && nzero == 0)
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (str3 == "0" && nzero >= 3)
                            {
                                ch1 = "";
                                ch2 = "";
                                nzero = nzero + 1;
                            }
                            else
                            {
                                if (j >= 11)
                                {
                                    ch1 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    ch1 = "";
                                    ch2 = str2.Substring(i, 1);
                                    nzero = nzero + 1;
                                }
                            }
                        }
                    }
                }
                if (i == (j - 11) || i == (j - 3))
                {
                    //如果该位是亿位或元位，则必须写上 
                    ch2 = str2.Substring(i, 1);
                }
                str5 = str5 + ch1 + ch2;

                if (i == j - 1 && str3 == "0")
                {
                    //最后一位（分）为0时，加上“整” 
                    str5 = str5 + '整';
                }
            }
            if (num == 0)
            {
                str5 = "零元整";
            }
            return str5;
        }

        /**/
        /// <summary> 
        /// 将字符串先转换成数字在调用RMBConverter(double num) 
        /// </summary> 
        /// <param name="num">用户输入的金额，字符串形式未转成decimal</param> 
        /// <returns></returns> 
        public static string RMBConverter(string numstr)
        {
            try
            {
                double num = Convert.ToDouble(numstr);
                return RMBConverter(num);
            }
            catch
            {
                return "非数字形式！";
            }
        }


		/// <summary>
		/// 给定字符串，返回从向其右侧的N个字符。
		/// <example>
		/// 
		/// </example>
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public string SubStringR(string s, int n)
		{
			return s.Substring(s.Length - n, n);
		}



    }
}
