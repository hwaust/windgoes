using System;
using System.Collections.Generic;
using System.Text; 

namespace WindGoes.Info
{
    /// <summary>
    /// 提供了用于查询车牌，电话区号的方法。
    /// </summary>
    public class CodeInfo : ObjectBase
    {
        /// <summary>
        /// 给定城市名称，返回城市的电话编号，如果名称不存在，则返回为String.Empty
        /// </summary>
        /// <param name="cityName">城市名称。</param>
        /// <returns></returns>
        public static string GetPhoneCode(string cityName)
        {
            for (int i = 0; i < InnerData.CityCodes.Length; i++)
            {
                if (InnerData.CityCodes[i].IndexOf(cityName) >= 0)
                {
                    return InnerData.CityCodes[i].Split(',')[1];
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 给定电话区号，返回城市名称，则返回为String.Empty
        /// </summary>
        /// <param name="code">城市名称。</param>
        /// <returns></returns>
        public static string GetCityNameByPhoneNumber(string code)
        {
            if (string.IsNullOrEmpty(code) || code.Length < 2)
                return string.Empty;

            if (code[0] == '0')
            {
                code = code.Substring(1);
            }

            for (int i = 0; i < InnerData.CityCodes.Length; i++)
            {
                if (InnerData.CityCodes[i].IndexOf(code) >= 0)
                {
                    return InnerData.CityCodes[i].Split(',')[0];
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 根据车牌首号，获得所属于地区。如“皖A”对应 “合肥”
        /// </summary>
        /// <param name="cn">车牌号的前2位。</param>
        /// <returns></returns>
        public static string GetCityNameByCarNumber(string cn)
        {
            //为空或长度不足2那么退出。
            if (string.IsNullOrEmpty(cn) || cn.Length < 2)
                return string.Empty;

            //转换成大写以便转换成整形索引，i表示第几行，加1是因为第0行为头。
            cn = cn.ToUpper();
            int i = InnerData.ShortProvinceNames.IndexOf(cn[0]) + 1;

            //注，28表示数组每行的个数，包括1个列名，26个字母 1个PQ，
            //(int)cn[1] - 64是表示大写字母转换成索引，如A对应1，B对应2
            return (i > 0) ? InnerData.CarNumbers[i * 28 + (int)cn[1] - 64] : string.Empty;
        }

        /// <summary>
        /// 给定城市名，获得车牌号。如“合肥”对应 “皖A”
        /// </summary>
        /// <param name="cityName">车牌号的前2位。</param>
        /// <returns></returns>
        public static string GetCarNumber(string cityName)
        {
            //为空或长度不足2那么退出。
            if (string.IsNullOrEmpty(cityName) || cityName.Length < 2)
                return string.Empty;

            for (int i = 28; i < InnerData.CarNumbers.Length; i++)
            {
                int p = InnerData.CarNumbers[i].IndexOf(cityName);
                if (p >= 0)
                {
                    int row = i / 28 - 1;
                    int col = i % 28 - 1;

                    return InnerData.ShortProvinceNames[row] + ((char)(col + 65)).ToString();
                }
            } 
 
            return  string.Empty;
        }


        /// <summary>
        /// 根省或直辖市的缩写，获得其全称。
        /// </summary>
        /// <param name="sn">省或直辖市的缩写。</param>
        /// <returns></returns>
        public static string GetFullName(string sn)
        {
            int p = InnerData.ShortProvinceNames.IndexOf(sn);
            return p < 0 ? string.Empty : InnerData.FullProvinceNames[p];
        }

        /// <summary>
        /// 根省或直辖市的全称，获得其缩写。
        /// </summary>
        /// <param name="fn">全称。</param>
        /// <returns></returns>
        public static string GetShortName(string fn)
        {
            for (int i = 0; i < InnerData.FullProvinceNames.Length; i++)
            {
                int p = InnerData.FullProvinceNames[i].IndexOf(fn);
                if (p >= 0)
                {
                    return InnerData.ShortProvinceNames[i].ToString();
                }
            }
            return string.Empty;
        }
    }
}
