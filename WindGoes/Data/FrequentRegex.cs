using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace WindGoes.Data
{
    /// <summary>
    /// 常用的字符串表达式的验证，如Email，Url，电话等。
    /// </summary>
    public class FrequentRegex
    {
        /// <summary>
        /// 根据正则表达式对指定的字符串进行验证。
        /// </summary>
        /// <param name="source">待验证的字符串。</param>
        /// <param name="regex">正则表达式。</param>
        /// <returns></returns>
        public static bool RegexValidate(string source, string regex)
        {
            try
            {
                Regex r = new Regex(regex);
                return r.Match(source).Success;
            }
            catch { }

            return false;
        }
        #region [数值验证]
        /// <summary>
        /// 非负整数（正整数 + 0）。
        /// </summary>
        /// <param name="s">待检测的字符串。</param>
        /// <returns></returns>
        public static bool IsNonNegativeNumber(string s)
        {
            return RegexValidate(s, @"^\d+$");
        }

        /// <summary>
        /// 非正整数（负整数 + 0）。
        /// </summary>
        /// <param name="s">待检测的字符串。</param>
        /// <returns></returns>
        public static bool IsNonPositiveNumber(string s)
        {
            return RegexValidate(s, @"^((-\d+)|(0+))$");
        }

        /// <summary>
        /// 正整数
        /// </summary>
        /// <param name="s">待检测的字符串。</param>
        /// <returns></returns>
        public static bool IsPositiveNumber(string s)
        {
            return RegexValidate(s, @"^[0-9]*[1-9][0-9]*$");
        }

        /// <summary>
        /// 负整数 
        /// </summary>
        /// <param name="s">待检测的字符串。</param>
        /// <returns></returns>
        public static bool IsNegativeNumber(string s)
        {
            return RegexValidate(s, @"^-[0-9]*[1-9][0-9]*$");
        }

        /// <summary>
        /// 整数 
        /// </summary>
        /// <param name="s">待检测的字符串。</param>
        /// <returns></returns>
        public static bool IsNumber(string s)
        {
            return RegexValidate(s, @"^-?\d+$");
        }

        /// <summary>
        /// 非负浮点数（正浮点数 + 0）
        /// </summary>
        /// <param name="s">待检测的字符串。</param>
        /// <returns></returns>
        public static bool IsNonNegativeFloat(string s)
        {
            return RegexValidate(s, @"^\d+(\.\d+)?$");
        }

        /// <summary>
        /// 正浮点数
        /// </summary>
        /// <param name="s">待检测的字符串。</param>
        /// <returns></returns>
        public static bool IsPositiveFloat(string s)
        {
            return RegexValidate(s, @"^(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$");
        }

        /// <summary>
        /// 非正浮点数（负浮点数 + 0）
        /// </summary>
        /// <param name="s">待检测的字符串。</param>
        /// <returns></returns>
        public static bool IsNonPositiveFloat(string s)
        {
            return RegexValidate(s, @"^((-\d+(\.\d+)?)|(0+(\.0+)?))$");
        }

        /// <summary>
        /// 负浮点数
        /// </summary>
        /// <param name="s">待检测的字符串。</param>
        /// <returns></returns>
        public static bool IsNegativeFloat(string s)
        {
            return RegexValidate(s, @"^(-(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*)))$");
        }

        /// <summary>
        /// 浮点数
        /// </summary>
        /// <param name="s">待检测的字符串。</param>
        /// <returns></returns>
        public static bool IsFloat(string s)
        {
            return RegexValidate(s, @"^(-?\d+)(\.\d+)?$");
        }
        #endregion

        /// <summary>
        /// 由26个英文字母组成的字符串
        /// </summary>
        /// <param name="s">待检测的字符串。</param>
        /// <returns></returns>
        public static bool IsLetterString(string s)
        {
            return RegexValidate(s, @"^[A-Za-z]+$");
        }

        /// <summary>
        /// 由26个英文字母的大写组成的字符串
        /// </summary>
        /// <param name="s">待检测的字符串。</param>
        /// <returns></returns>
        public static bool IsUpperLetterString(string s)
        {
            return RegexValidate(s, @"^[A-Z]+$");
        }

        /// <summary>
        /// 由26个英文字母的小写组成的字符串
        /// </summary>
        /// <param name="s">待检测的字符串。</param>
        /// <returns></returns>
        public static bool IsLowerLetterString(string s)
        {
            return RegexValidate(s, @"^[a-z]+$");
        }

        /// <summary>
        /// 由数字、26个英文字母或者下划线组成的字符串
        /// </summary>
        /// <param name="s">待检测的字符串。</param>
        /// <returns></returns>
        public static bool IsGeneralString(string s)
        {
            return RegexValidate(s, @"^\w+$");
        }

        /// <summary>
        /// 匹配日期和时间（如 12:30 PM | 2004-02-29 | 2004/3/31 02:31:35 AM）
        /// </summary>
        /// <param name="s">待检测的字符串。</param>
        /// <returns></returns>
        public static bool IsDateTime(string s)
        {
            return RegexValidate(s, @"^(?ni:(?=\d)((?'year'((1[6-9])|([2-9]\d))\d\d)(?'sep'[/.-])(?'month'0?[1-9]|1[012])\2(?'day'((?<!(\2((0?[2469])|11)\2))31)|(?<!\2(0?2)\2)(29|30)|((?<=((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(16|[2468][048]|[3579][26])00)\2\3\2)29)|((0?[1-9])|(1\d)|(2[0-8])))(?:(?=\x20\d)\x20|$))?((?<time>((0?[1-9]|1[012])(:[0-5]\d){0,2}(\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2}))?)$");
        }

        /// <summary>
        /// 是否是Email地址。
        /// </summary>
        /// <param name="s">待检测的字符串。</param>
        /// <returns></returns>
        public static bool IsEmail(string s)
        {
            if (s.IndexOf('@') > 0 && Regex.Matches(s, "@").Count == 1)
            {
                return RegexValidate(s, @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            }
            return false;
        }

        /// <summary>
        /// 是否包括中文汉字。
        /// </summary>
        /// <param name="s">待检测的字符串。</param>
        /// <returns></returns>
        public static bool ContainsChinese(string s)
        {
            return RegexValidate(s, @"[\u4E00-\u9FA5]");
        }

        /// <summary>
        /// 匹配双字节字符。
        /// </summary>
        /// <param name="s">待检测的字符串。</param>
        /// <returns></returns>
        /// <summary>
        public static bool UnicodeText(string s)
        {
            return RegexValidate(s, @"[^\x00-\xff]");
        }

        /// <summary>
        /// 是否为Internet地址。
        /// </summary>
        /// <param name="s">待检测的字符串。</param>
        /// <returns></returns>
        public static bool IsInternetURL(string s)
        {
            return RegexValidate(s, @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
        }

        /// <summary>
        /// 是否为电话号码。
        /// </summary>
        /// <param name="s">待检测的字符串。</param>
        /// <returns></returns>
        public static bool IsPhoneNumber(string s)
        {
            //判断是否为手机或者座机。
            return RegexValidate(s, @"(^(\d{3,4}-)?(\d{3,4}-)?\d{6,8}$)|^((\+?[0-9][0-9])|(00[0-9][0-9]))?(0{0,1}1[358][0-9]{9}$)");
        }

        /// <summary>
        /// 验证身份证号码（15或18位）。
        /// </summary>
        /// <param name="s">待检测的字符串。</param>
        /// <returns></returns>
        public static bool IsIDcard(string s)
        {
            return RegexValidate(s, @"(^\d{18}$)|(^\d{15}$)|(^\d{17}[xX])");
        }

        /// <summary>
        /// 验证IP地址是否正确。
        /// </summary>
        /// <param name="s">待检测的字符串。</param>
        /// <returns></returns>
        public static bool IsIPAddr(string s)
        {
            return RegexValidate(s, @"((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)");
        }

        /// <summary>
        /// 匹配帐号是否合法(字母开头，长度在m-n字节，允许字母数字下划线)
        /// </summary>
        /// <param name="s">待检测的字符串。</param>
        /// <param name="m">长度下限</param>
        /// <param name="n">长度上限</param>
        /// <returns></returns>
        /// <summary>
        public static bool BankAcct(string s, int m, int n)
        {
            if (m > 0 && n > 0 && m < n)
            {
                string pattern = String.Format("^[a-zA-Z][a-zA-Z0-9_]{{{0},{1}}}$", (m - 1).ToString(), (n - 1).ToString());
                return RegexValidate(s, pattern);
            }
            return false;
        }

        /// 匹配任意标签并提取标签中的内容。
        /// </summary>
        /// <param name="s">待检测的字符串。</param>
        /// <param name="mark_Left">左标签符号</param>
        /// <param name="mark_Right">右标签符号</param>
        /// <param name="caseSensitive">是否区分大小写</param>
        /// <returns>返回标签中的内容为ArrayList型</returns>
        public static ArrayList MatchString(string s, string mark_Left, string mark_Right,bool caseSensitive)
        {
            mark_Left = checkLRMark(mark_Left);
            mark_Right = checkLRMark(mark_Right);
            string pattern = "(?<=" + mark_Left + ").*?(?=" + mark_Right + ")";
            if (!caseSensitive)
                pattern = "(?i)" + pattern;
            ArrayList list = new ArrayList();
            MatchCollection mc = Regex.Matches(s, pattern);//满足pattern的匹配集合
            for (int i = 0; i < mc.Count; i++)
            {
                bool hasExist = false;
                string name = mc[i].ToString();
                foreach (string one in list)
                {
                    if (name == one)
                    {
                        hasExist = true;
                        break;
                    }
                }
                if (!hasExist) list.Add(name);
            }
            return list;
        }

        //检查左右标签并自动添加转义字符"\"
        public static string checkLRMark(string mark)
        {
            string checkString = @".$^{[(|)*+?\";
            for (int i = 0; i < mark.Length; i++)
            {
                char p = System.Convert.ToChar(mark.Substring(i, 1));
                if (checkString.IndexOf(p) > -1)
                {
                    mark = mark.Insert(i, @"\");
                    i++;
                    continue;
                }
            }
            return mark;
        }
    }
}