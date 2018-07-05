using System;
using System.Collections.Generic;
using System.Text;

namespace WindGoes.Info
{
    /// <summary>
    /// 身份证号处理，用于身份证的处理。
    /// </summary>
    public class IDCard: ObjectBase
    { 
        /// <summary>
        /// 身份证的长度。
        /// </summary>
        public int Length
        {
            get { return idNumber.Length; } 
        }


        string idNumber = string.Empty;
        /// <summary>
        /// 身份证号码。
        /// </summary>
        public string IdNumber
        {
            get { return idNumber; }
            set
            { 
                idNumber = value;
                LoadID(idNumber); 
            }
        }

        bool correct = false;
        /// <summary>
        /// 身份证号码是否正确。
        /// </summary>
        public bool Correct
        {
            get { return correct; } 
        }

        DateTime birthday;
        /// <summary>
        /// 出生日期。
        /// </summary>
        public DateTime Birthday
        {
            get { return birthday; } 
        }

        string gender = "男";
        /// <summary>
        /// 是否是男性。
        /// </summary>
        public string Gender
        {
            get { return gender; } 
        }

        string region = string.Empty;
        /// <summary>
        /// 身份证号码对应的地区。
        /// </summary>
        public string Region
        {
            get { return region; } 
        }

        string endCode = string.Empty;
        /// <summary>
        /// 末尾的结束码。
        /// </summary>
        public string EndCode
        {
            get { return endCode; } 
        }



        public IDCard(string id)
        {
            LoadID(id);
        }

        private void LoadID(string id)
        {
            idNumber = id;

            correct = IsCorrect(id);

            if (id.Length != 15 && id.Length != 18)
                return;

            for (int i = 0; i < id.Length - 1; i++)
            {
                if (!Char.IsDigit(id[i]))
                    return;
            }

            int t = 0, year = 0, month = 0, day = 0;

            //取地区
            region = GetFullRegion(id.Substring(0, 6));

            //取生日
            if (id.Length == 18)
            {
                try
                {

                    year = int.Parse(id.Substring(6, 4));
                    month = int.Parse(id.Substring(10, 2));
                    day = int.Parse(id.Substring(12, 2));
                    birthday = new DateTime(year, month, day);
                }
                catch { return; } 

                //第17位（即倒数第2位）表示性别。   
                t = int.Parse(id.Substring(16, 1));

                //结束码
                endCode = id.Substring(14, 4);
            }
            else
            {
                try
                {
                    year = int.Parse("19" + id.Substring(6, 2));
                    month = int.Parse(id.Substring(8, 2));
                    day = int.Parse(id.Substring(10, 2));
                    birthday = new DateTime(year, month, day);
                }
                catch { return; }

                //第15位（即末尾位）表示性别。
                t = int.Parse(id.Substring(14, 1));

                //结束码
                endCode = id.Substring(12, 3);
            }

            gender = t % 2 == 1 ? "男" : "女";

        }

        /// <summary>
        /// 15位身份证号码升级为18位号码的计算公式。
        /// </summary>
        /// <param name="id">15位的旧身份证号码。</param>
        /// <returns></returns>
        public static string ID15To18(string id)
        {
            int iS = 0;
            //加权因子常数  
            int[] iW = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };

            //校验码常数 
            string LastCode = "10X98765432";
            //新身份证号 
            string perIDNew;

            perIDNew = id.Substring(0, 6);
            //填在第6位及第7位上填上‘1’，‘9’两个数字 
            perIDNew += "19";

            perIDNew += id.Substring(6, 9);

            //进行加权求和 
            for (int i = 0; i < 17; i++)
            {
                iS += int.Parse(perIDNew.Substring(i, 1)) * iW[i];
            }

            //取模运算，得到模值 
            int iY = iS % 11;
            //从LastCode中取得以模为索引号的值，加到身份证的最后一位，即为新身份证号。 
            perIDNew += LastCode.Substring(iY, 1);

            return perIDNew;
        }

        /// <summary>
        /// 身份证号码的验证，判断是否为真正的身份证号还是假的号码。
        /// </summary>
        /// <param name="id">需要判断的身份证号码，应该为18位的数字。</param>
        /// <returns></returns>
        public static bool IsCorrect(string id)
        {
            //若不是长度为18的字符串，那么返回为False
            if (string.IsNullOrEmpty(id) || id.Length != 18)
            {
                return false;
            }
            
            //若前17位不是数字，同样返回为False
            for (int i = 0; i < id.Length - 1; i++)
            {
                if (!char.IsDigit(id[i]))
                {
                    return false;
                }
            }

            //若是小写字母，则转换成大写字母。否则若不是大写的X，返回False。
            if (id[17] == 'x') 
                id = id.ToUpper(); 

            //加权因子常数  
            int[] iW = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };

            //校验码常数 
            string LastCode = "10X98765432";

            //进行加权求和 
            int iS = 0;
            for (int i = 0; i < 17; i++)
            {
                iS += int.Parse(id.Substring(i, 1)) * iW[i];
            }

            //取模运算，得到模值 
            int iY = iS % 11;

            return id.Substring(id.Length - 1, 1) == LastCode.Substring(iY, 1);
        }

        /// <summary>
        /// 根据身份证首6位，获得对应的完全的"XX省XX市XX区”格式的地址信息。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetFullRegion(string id)
        {
            if (id.Length < 6)
                return string.Empty;

            return GetRegion(id.Substring(0, 2) + "0000") + GetRegion(id.Substring(0, 4) + "00") + GetRegion(id.Substring(0, 6));
        }

        /// <summary>
        /// 根据给定的6位ID查询对应的行政单位编号，可能是省，市或地区。
        /// </summary>
        /// <param name="id">给定的6位编号。</param>
        /// <returns></returns>
        public static string GetRegion(string id)
        {
            for (int i = 0; i < InnerData.RegionCodes.GetLength(0); i++)
            {
                if (InnerData.RegionCodes[i,0] == id)
                {
                    return InnerData.RegionCodes[i,1];
                }
            }
            return string.Empty;
        }


    }
}
