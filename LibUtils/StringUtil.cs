using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibUtils
{
    public class StringUtil
    {
        private static readonly string[] VietnameseSigns = new string[15]
        {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
        };

        public static string InjectionString(string str)
        {
            try
            {
                string text = StringUtil.KillChar(str).Replace("'", "''");
                return str;
            }
            catch
            {
                return "";
            }
        }

        private static string KillChar(string strInput)
        {
            try
            {
                string[] array = new string[7]
                {
                "select",
                "drop",
                ";",
                "--",
                "insert",
                "delete",
                "xp_"
                };
                string text = strInput.Trim();
                for (int i = 0; i < array.Length; i++)
                {
                    text = text.Replace(array[i], "");
                }
                return text;
            }
            catch
            {
                return "";
            }
        }

        public static string RemoveEndChar(string s)
        {
            if (s.Length > 1 && s.EndsWith(","))
            {
                s = s.Substring(0, s.Length - 1);
            }
            return s;
        }

        public bool IsNumber(string str)
        {
            bool result = true;
            foreach (char c in str)
            {
                if (!char.IsDigit(c))
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        public static string Left(string Str, int Pos)
        {
            string result = "";
            try
            {
                Str = Str.Trim();
                result = ((Pos > 0) ? ((Str.Length > Pos) ? (Str.Substring(0, Pos) + "...") : Str) : Str);
            }
            catch
            {
            }
            return result;
        }

        public static string long2String(long n, string delimiter)
        {
            string text = "";
            long num = 0L;
            int num2 = 0;
            try
            {
                long num3 = n;
                n = Math.Abs(n);
                while (n > 0)
                {
                    num2++;
                    num = n % 10;
                    n /= 10;
                    if (num2 == 4)
                    {
                        text = delimiter + text;
                        num2 = 1;
                    }
                    text = num.ToString() + text;
                }
                if (num3 < 0)
                {
                    text = "-" + text;
                }
            }
            catch
            {
                text = "ERROR";
            }
            return text;
        }

        public static string Float2String(double d)
        {
            string delimiter = ",";
            return StringUtil.Float2String(d, delimiter);
        }

        public static string Float2String(double d, string delimiter)
        {
            string decimalDelimiter = ".";
            return StringUtil.Float2String(d, delimiter, decimalDelimiter);
        }

        public static string Float2String(double d, string delimiter, string decimalDelimiter)
        {
            string text = "";
            double num = 0.0;
            double num2 = 0.0;
            try
            {
                num = Math.Truncate(d);
                num2 = Math.Abs(d - num);
                num = Math.Abs(num);
                text = StringUtil.long2String(Convert.ToInt64(num), delimiter);
                if (num2 > 0.0)
                {
                    string str = Math.Abs(d).ToString().Substring(num.ToString().Length + 1);
                    text = text + decimalDelimiter + str;
                }
                if (d < 0.0)
                {
                    text = "-" + text;
                }
            }
            catch
            {
                text = "ERROR";
            }
            return text;
        }

        public static string DefaultDelimiter()
        {
            return "#";
        }

        public static byte DefaultReplicated()
        {
            return 1;
        }

        public static string FloatStringToStringWithDelimeter(string floatStr)
        {
            string text = "";
            float num = 0f;
            try
            {
                float.TryParse(floatStr, out num);
                return StringUtil.FloatToStringWithDelimeter(num);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string FloatToStringWithDelimeter(float num)
        {
            return StringUtil.IntToStringWithDelimeter(num, ",");
        }

        public static string IntToStringWithDelimeter(float num, string delimeter)
        {
            string result = "";
            try
            {
                result = num.ToString();
            }
            catch
            {
            }
            return result;
        }

        public static string ConnectionStringIpReplace(string connStr, string ipReplace)
        {
            string text = "";
            try
            {
                string text2 = StringUtil.ExactValue(connStr, "server", ';');
                if (text2 == ipReplace)
                {
                    return connStr;
                }
                return connStr.Replace(text2, ipReplace);
            }
            catch
            {
                return "";
            }
        }

        public static string GetQuote(string odString)
        {
            string[] array = odString.Split(',');
            string text = "";
            for (int i = 0; i < array.Length; i++)
            {
                text = text + "'" + array[i] + "',";
            }
            return text.Substring(0, text.Length - 1);
        }

        public static string ExactValue(string str, string objName, char delimiter)
        {
            string[] array = str.Split(delimiter);
            string result = "";
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Contains(objName))
                {
                    result = array[i].Replace(objName, "");
                    result = result.Replace("=", "").Trim();
                    i = array.Length;
                }
            }
            return result;
        }

        public static bool isNumeric(string s)
        {
            try
            {
                Convert.ToInt32(s);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static byte StrToByte(string s)
        {
            byte result = 0;
            try
            {
                result = Convert.ToByte(s);
            }
            catch
            {
            }
            return result;
        }

        public static short StrToInt16(string s)
        {
            short result = 0;
            try
            {
                result = Convert.ToInt16(s);
            }
            catch
            {
            }
            return result;
        }

        public static int StrToInt32(string s)
        {
            int result = 0;
            try
            {
                result = Convert.ToInt32(s);
            }
            catch
            {
            }
            return result;
        }

        public static string RemoveSign4VietnameseString(string str)
        {
            for (int i = 1; i < StringUtil.VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < StringUtil.VietnameseSigns[i].Length; j++)
                {
                    str = str.Replace(StringUtil.VietnameseSigns[i][j], StringUtil.VietnameseSigns[0][i - 1]);
                }
            }
            return str;
        }

        public static string UCS2Convert(string sContent)
        {
            string text = "a|á|à|ả|ã|ạ|ă|ắ|ằ|ẳ|ẵ|ặ|â|ấ|ầ|ẩ|ẫ|ậ|đ|e|é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ|i|í|ì|ỉ|ĩ|ị|o|ó|ò|ỏ|õ|ọ|ô|ố|ồ|ổ|ỗ|ộ|ơ|ớ|ờ|ở|ỡ|ợ|u|ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự|y|ý|ỳ|ỷ|ỹ|ỵ";
            string text2 = "A|Á|À|Ả|Ã|Ạ|Ă|Ắ|Ằ|Ẳ|Ẵ|Ặ|Â|Ấ|Ầ|Ẩ|Ẫ|Ậ|Đ|E|É|È|Ẻ|Ẽ|Ẹ|Ê|Ế|Ề|Ể|Ễ|Ệ|I|Í|Ì|Ỉ|Ĩ|Ị|O|Ó|Ò|Ỏ|Õ|Ọ|Ô|Ố|Ồ|Ổ|Ỗ|Ộ|Ơ|Ớ|Ờ|Ở|Ỡ|Ợ|U|Ú|Ù|Ủ|Ũ|Ụ|Ư|Ứ|Ừ|Ử|Ữ|Ự|Y|Ý|Ỳ|Ỷ|Ỹ|Ỵ";
            string text3 = "a|a|a|a|a|a|a|a|a|a|a|a|a|a|a|a|a|a|d|e|e|e|e|e|e|e|e|e|e|e|e|i|i|i|i|i|i|o|o|o|o|o|o|o|o|o|o|o|o|o|o|o|o|o|o|u|u|u|u|u|u|u|u|u|u|u|u|y|y|y|y|y|y";
            string text4 = "A|A|A|A|A|A|A|A|A|A|A|A|A|A|A|A|A|A|D|E|E|E|E|E|E|E|E|E|E|E|E|I|I|I|I|I|I|O|O|O|O|O|O|O|O|O|O|O|O|O|O|O|O|O|O|U|U|U|U|U|U|U|U|U|U|U|U|Y|Y|Y|Y|Y|Y";
            string[] array = text.Split('|');
            string[] array2 = text2.Split('|');
            string[] array3 = text3.Split('|');
            string[] array4 = text4.Split('|');
            int upperBound = array.GetUpperBound(0);
            for (int i = 1; i <= upperBound; i++)
            {
                sContent = sContent.Replace(array[i], array3[i]);
                sContent = sContent.Replace(array2[i], array4[i]);
            }
            return sContent;
        }

        public static string DecimalConvert(string InputString, bool Flow)
        {
            string str = "a a a a a A A A A A a a a a a a A A A A A A a a a a a a A A A A A A ";
            str += "o o o o o O O O O O o o o o o o O O O O O O o o o o o o O O O O O O ";
            str += "e e e e e E E E E E e e e e e e E E E E E E ";
            str += "u u u u u U U U U U u u u u u u U U U U U U ";
            str += "i i i i i I I I I I y y y y y Y Y Y Y Y d D ";
            str += "D “ ” – a a a o o o o e e e u u i i u o o i a a o a y y y";
            string str2 = "&#224; &#225; &#7843; &#227; &#7841; &#192; &#193; &#7842; &#195; &#7840; &#226; &#7847; &#7845; &#7849; &#7851; &#7853; &#194; &#7846; &#7844; &#7848; &#7850; &#7852; &#259; &#7857; &#7855; &#7859; &#7861; &#7863; &#258; &#7856; &#7854; &#7858; &#7860; &#7862; ";
            str2 += "&#242; &#243; &#7887; &#245; &#7885; &#210; &#211; &#7886; &#213; &#7884; &#244; &#7891; &#7889; &#7893; &#7895; &#7897; &#212; &#7890; &#7888; &#7892; &#7894; &#7896; &#417; &#7901; &#7899; &#7903; &#7905; &#7907; &#416; &#7900; &#7898; &#7902; &#7904; &#7906; ";
            str2 += "&#232; &#233; &#7867; &#7869; &#7865; &#200; &#201; &#7866; &#7868; &#7864; &#234; &#7873; &#7871; &#7875; &#7877; &#7879; &#202; &#7872; &#7870; &#7874; &#7876; &#7878; ";
            str2 += "&#249; &#250; &#7911; &#361; &#7909; &#217; &#218; &#7910; &#360; &#7908; &#432; &#7915; &#7913; &#7917; &#7919; &#7921; &#431; &#7914; &#7912; &#7916; &#7918; &#7920; ";
            str2 += "&#236; &#237; &#7881; &#297; &#7883; &#204; &#205; &#7880; &#296; &#7882; &#7923; &#253; &#7927; &#7929; &#7925; &#7922; &#221; &#7926; &#7928; &#7924; &#273; &#272; ";
            str2 += "&#208; &#34; &#34; &#45; à á â ó ò ô &#7887; è é ê ú ù ì í u&#769; o&#777; o&#769; i&#769; ã a&#777; o&#803; a&#769; ý &#7923; &#7927;";
            string[] array = str2.Split(' ');
            string[] array2 = str.Split(' ');
            string text = InputString;
            int upperBound = array2.GetUpperBound(0);
            if (Flow)
            {
                for (int i = 1; i <= upperBound; i++)
                {
                    text = text.Replace(array[i], array2[i]);
                }
            }
            else
            {
                for (int i = 1; i <= upperBound; i++)
                {
                    text = text.Replace(array2[i], array[i]);
                }
            }
            return text;
        }

        //public static string GetVisibleAndSelectedValue(DropDownList mDropDownList, ref bool Visible)
        //{
        //    string text = "";
        //    try
        //    {
        //        if (mDropDownList.Items.Count > 0)
        //        {
        //            text = mDropDownList.SelectedValue.ToString();
        //            if (mDropDownList.Items.Count > 1)
        //            {
        //                Visible = true;
        //            }
        //            else
        //            {
        //                Visible = false;
        //            }
        //        }
        //        else
        //        {
        //            Visible = false;
        //            text = "";
        //        }
        //    }
        //    catch
        //    {
        //        Visible = false;
        //        text = "";
        //    }
        //    return text;
        //}

        //public static string GetChecked(CheckBoxList chkList)
        //{
        //    return StringUtil.GetChecked(chkList, StringUtil.DefaultDelimiter());
        //}

        //public static string GetChecked(CheckBoxList chkList, string delimiter)
        //{
        //    string text = "";
        //    try
        //    {
        //        delimiter = delimiter.Trim();
        //        if (delimiter.Length <= 0)
        //        {
        //            delimiter = StringUtil.DefaultDelimiter();
        //        }
        //        for (int i = 0; i < chkList.Items.Count; i++)
        //        {
        //            if (chkList.Items[i].Selected)
        //            {
        //                text = text + chkList.Items[i].Value + delimiter;
        //            }
        //        }
        //        if (text.EndsWith(delimiter))
        //        {
        //            text = text.Substring(0, text.Length - 1);
        //        }
        //    }
        //    catch
        //    {
        //    }
        //    return text;
        //}

        //public static void PutChecked(string strList, ref CheckBoxList chkList)
        //{
        //    try
        //    {
        //        if (strList.Length > 0)
        //        {
        //            int num = 0;
        //            for (int i = 0; i < chkList.Items.Count; i++)
        //            {
        //                num = strList.IndexOf(chkList.Items[i].Value.ToString().Trim());
        //                if (num >= 0)
        //                {
        //                    chkList.Items[i].Selected = true;
        //                }
        //                else
        //                {
        //                    chkList.Items[i].Selected = false;
        //                }
        //            }
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}

        public static string ConvertStringToHex(string input)
        {
            byte[] bytes = Encoding.Default.GetBytes(input);
            StringBuilder stringBuilder = new StringBuilder(bytes.Length * 2);
            byte[] array = bytes;
            foreach (byte b in array)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }

        public static string RemoveSignature(string input)
        {
            if (input == null)
            {
                return "";
            }
            for (int i = 1; i < StringUtil.VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < StringUtil.VietnameseSigns[i].Length; j++)
                {
                    input = input.Replace(StringUtil.VietnameseSigns[i][j], StringUtil.VietnameseSigns[0][i - 1]);
                }
            }
            return input;
        }

        public static string removeSpecialSignatures(string inputString)
        {
            string text = "~!@#$%^&*()_+|\\]}[{\"':/?.>,<`”“…=-";
            if (!string.IsNullOrEmpty(inputString))
            {
                for (int i = 0; i < text.Length; i++)
                {
                    inputString = inputString.Replace(text[i].ToString(), "");
                }
            }
            return inputString;
        }

        public static string RemoveSignatureForURL(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            input = StringUtil.RemoveSignature(input);
            input = input.ToLower().Trim();
            input = StringUtil.removeSpecialSignatures(input);
            while (input.Contains("  "))
            {
                input = input.Replace("  ", " ");
            }
            input = input.Replace(" ", "-");
            return input;
        }

        public static string TitleFormat(string title)
        {
            string text = "";
            for (int i = 0; i < title.Length; i++)
            {
                if (char.IsWhiteSpace(title[i]) || char.IsLetterOrDigit(title[i]))
                {
                    text += title[i].ToString();
                }
            }
            text = text.Trim();
            text = text.Replace("  ", " ");
            text = text.Replace(" ", "-");
            text = text.Replace("/", "");
            text = text.Replace("?", "");
            text = text.Replace(",", "-");
            text = text.Replace("&", "-");
            text = text.ToLower();
            return StringUtil.UCS2Convert(text);
        }

        public static string removeEndChar(string s)
        {
            if (s.Length > 1 && s.EndsWith(","))
            {
                s = s.Substring(0, s.Length - 1);
            }
            return s;
        }

        public static DateTime ConvertToDateTime(string strDateTime)
        {
            string name = "vi-VN";
            try
            {
                return DateTime.Parse(strDateTime, CultureInfo.CreateSpecificCulture(name));
            }
            catch
            {
                return DateTime.Now;
            }
        }
    }
}
