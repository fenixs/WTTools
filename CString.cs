using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace WTTools
{
    public class CString
    {

        #region "唯一实例"
        private static CString _Instance;
        private static readonly object _lock = new object();

        private CString()
        {

        }

        ~CString()
        {
            Dispose();
        }

        private void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public static CString Instance
        {
            get
            {
                if(_Instance==null)
                {
                    lock(_lock)
                    {
                        _Instance = new CString();
                    }
                }
                return _Instance;
            }
        }

        #endregion
        

        /// <summary>
        /// 检测是否Html中的合法文本
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool CheckValidity(string s)
        {
            string str = s;
            return ((((str.IndexOf("'") <= 0) && (str.IndexOf("&") <= 0)) && ((str.IndexOf("%") <= 0) && (str.IndexOf("+") <= 0))) && (((str.IndexOf("\"") <= 0) && (str.IndexOf("=") <= 0)) && (str.IndexOf("!") <= 0)));
        }

        /// <summary>
        /// 拼接字符
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string[] Concat(params string[][] array)
        {
            Hashtable ht = new Hashtable();
            if (array != null)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    if(array[i]!=null)
                    {
                        for(int j=0;j<array[i].Length;j++)
                        {
                            if (array[i][j] != null && ht.ContainsKey(array[i][j]))
                            {
                                ht.Add(array[i][j], array[i][j]);
                            }
                        }
                    }
                }
            }
            string[] strs = new string[ht.Count];
            IDictionaryEnumerator enumerator = ht.GetEnumerator();
            for(int i=0;enumerator.MoveNext();i++)
            {
                strs[i] = enumerator.Key.ToString();
            }
            return strs;
        }


        public static string ConvertPinYinFirst(string text)
        {
            StringBuilder builder = new StringBuilder(text.Length);
            foreach (char ch2 in text)
            {
                char ch = ch2;
                byte[] bytes = Encoding.Default.GetBytes(new char[] { ch2 });
                if (bytes.Length == 2)
                {
                    int num = (bytes[0] * 0x100) + bytes[1];
                    if (num < 0xb0a1)
                    {
                        ch = ch2;
                    }
                    else if (num < 0xb0c5)
                    {
                        ch = 'a';
                    }
                    else if (num < 0xb2c1)
                    {
                        ch = 'b';
                    }
                    else if (num < 0xb4ee)
                    {
                        ch = 'c';
                    }
                    else if (num < 0xb6ea)
                    {
                        ch = 'd';
                    }
                    else if (num < 0xb7a2)
                    {
                        ch = 'e';
                    }
                    else if (num < 0xb8c1)
                    {
                        ch = 'f';
                    }
                    else if (num < 0xb9fe)
                    {
                        ch = 'g';
                    }
                    else if (num < 0xbbf7)
                    {
                        ch = 'h';
                    }
                    else if (num < 0xbfa6)
                    {
                        ch = 'g';
                    }
                    else if (num < 0xc0ac)
                    {
                        ch = 'k';
                    }
                    else if (num < 0xc2e8)
                    {
                        ch = 'l';
                    }
                    else if (num < 0xc4c3)
                    {
                        ch = 'm';
                    }
                    else if (num < 0xc5b6)
                    {
                        ch = 'n';
                    }
                    else if (num < 0xc5be)
                    {
                        ch = 'o';
                    }
                    else if (num < 0xc6da)
                    {
                        ch = 'p';
                    }
                    else if (num < 0xc8bb)
                    {
                        ch = 'q';
                    }
                    else if (num < 0xc8f6)
                    {
                        ch = 'r';
                    }
                    else if (num < 0xcbfa)
                    {
                        ch = 's';
                    }
                    else if (num < 0xcdda)
                    {
                        ch = 't';
                    }
                    else if (num < 0xcef4)
                    {
                        ch = 'w';
                    }
                    else if (num < 0xd1b9)
                    {
                        ch = 'x';
                    }
                    else if (num < 0xd4d1)
                    {
                        ch = 'y';
                    }
                    else if (num < 0xd7fa)
                    {
                        ch = 'z';
                    }
                }
                builder.Append(ch);
            }
            return builder.ToString();
        }

        /// <summary>
        /// 获取字符汉语拼音
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ConvertPinYinFull(string text)
        {
            int[] numArray = new int[] { 
                -20319, -20317, -20304, -20295, -20292, -20283, -20265, -20257, -20242, -20230, -20051, -20036, -20032, -20026, -20002, -19990, 
                -19986, -19982, -19976, -19805, -19784, -19775, -19774, -19763, -19756, -19751, -19746, -19741, -19739, -19728, -19725, -19715, 
                -19540, -19531, -19525, -19515, -19500, -19484, -19479, -19467, -19289, -19288, -19281, -19275, -19270, -19263, -19261, -19249, 
                -19243, -19242, -19238, -19235, -19227, -19224, -19218, -19212, -19038, -19023, -19018, -19006, -19003, -18996, -18977, -18961, 
                -18952, -18783, -18774, -18773, -18763, -18756, -18741, -18735, -18731, -18722, -18710, -18697, -18696, -18526, -18518, -18501, 
                -18490, -18478, -18463, -18448, -18447, -18446, -18239, -18237, -18231, -18220, -18211, -18201, -18184, -18183, -18181, -18012, 
                -17997, -17988, -17970, -17964, -17961, -17950, -17947, -17931, -17928, -17922, -17759, -17752, -17733, -17730, -17721, -17703, 
                -17701, -17697, -17692, -17683, -17676, -17496, -17487, -17482, -17468, -17454, -17433, -17427, -17417, -17202, -17185, -16983, 
                -16970, -16942, -16915, -16733, -16708, -16706, -16689, -16664, -16657, -16647, -16474, -16470, -16465, -16459, -16452, -16448, 
                -16433, -16429, -16427, -16423, -16419, -16412, -16407, -16403, -16401, -16393, -16220, -16216, -16212, -16205, -16202, -16187, 
                -16180, -16171, -16169, -16158, -16155, -15959, -15958, -15944, -15933, -15920, -15915, -15903, -15889, -15878, -15707, -15701, 
                -15681, -15667, -15661, -15659, -15652, -15640, -15631, -15625, -15454, -15448, -15436, -15435, -15419, -15416, -15408, -15394, 
                -15385, -15377, -15375, -15369, -15363, -15362, -15183, -15180, -15165, -15158, -15153, -15150, -15149, -15144, -15143, -15141, 
                -15140, -15139, -15128, -15121, -15119, -15117, -15110, -15109, -14941, -14937, -14933, -14930, -14929, -14928, -14926, -14922, 
                -14921, -14914, -14908, -14902, -14894, -14889, -14882, -14873, -14871, -14857, -14678, -14674, -14670, -14668, -14663, -14654, 
                -14645, -14630, -14594, -14429, -14407, -14399, -14384, -14379, -14368, -14355, -14353, -14345, -14170, -14159, -14151, -14149, 
                -14145, -14140, -14137, -14135, -14125, -14123, -14122, -14112, -14109, -14099, -14097, -14094, -14092, -14090, -14087, -14083, 
                -13917, -13914, -13910, -13907, -13906, -13905, -13896, -13894, -13878, -13870, -13859, -13847, -13831, -13658, -13611, -13601, 
                -13406, -13404, -13400, -13398, -13395, -13391, -13387, -13383, -13367, -13359, -13356, -13343, -13340, -13329, -13326, -13318, 
                -13147, -13138, -13120, -13107, -13096, -13095, -13091, -13076, -13068, -13063, -13060, -12888, -12875, -12871, -12860, -12858, 
                -12852, -12849, -12838, -12831, -12829, -12812, -12802, -12607, -12597, -12594, -12585, -12556, -12359, -12346, -12320, -12300, 
                -12120, -12099, -12089, -12074, -12067, -12058, -12039, -11867, -11861, -11847, -11831, -11798, -11781, -11604, -11589, -11536, 
                -11358, -11340, -11339, -11324, -11303, -11097, -11077, -11067, -11055, -11052, -11045, -11041, -11038, -11024, -11020, -11019, 
                -11018, -11014, -10838, -10832, -10815, -10800, -10790, -10780, -10764, -10587, -10544, -10533, -10519, -10331, -10329, -10328, 
                -10322, -10315, -10309, -10307, -10296, -10281, -10274, -10270, -10262, -10260, -10256, -10254
             };
            string[] strArray = new string[] { 
                "a", "ai", "an", "ang", "ao", "ba", "bai", "ban", "bang", "bao", "bei", "ben", "beng", "bi", "bian", "biao", 
                "bie", "bin", "bing", "bo", "bu", "ca", "cai", "can", "cang", "cao", "ce", "ceng", "cha", "chai", "chan", "chang", 
                "chao", "che", "chen", "cheng", "chi", "chong", "chou", "chu", "chuai", "chuan", "chuang", "chui", "chun", "chuo", "ci", "cong", 
                "cou", "cu", "cuan", "cui", "cun", "cuo", "da", "dai", "dan", "dang", "dao", "de", "deng", "di", "dian", "diao", 
                "die", "ding", "diu", "dong", "dou", "du", "duan", "dui", "dun", "duo", "e", "en", "er", "fa", "fan", "fang", 
                "fei", "fen", "feng", "fo", "fou", "fu", "ga", "gai", "gan", "gang", "gao", "ge", "gei", "gen", "geng", "gong", 
                "gou", "gu", "gua", "guai", "guan", "guang", "gui", "gun", "guo", "ha", "hai", "han", "hang", "hao", "he", "hei", 
                "hen", "heng", "hong", "hou", "hu", "hua", "huai", "huan", "huang", "hui", "hun", "huo", "ji", "jia", "jian", "jiang", 
                "jiao", "jie", "jin", "jing", "jiong", "jiu", "ju", "juan", "jue", "jun", "ka", "kai", "kan", "kang", "kao", "ke", 
                "ken", "keng", "kong", "kou", "ku", "kua", "kuai", "kuan", "kuang", "kui", "kun", "kuo", "la", "lai", "lan", "lang", 
                "lao", "le", "lei", "leng", "li", "lia", "lian", "liang", "liao", "lie", "lin", "ling", "liu", "long", "lou", "lu", 
                "lv", "luan", "lue", "lun", "luo", "ma", "mai", "man", "mang", "mao", "me", "mei", "men", "meng", "mi", "mian", 
                "miao", "mie", "min", "ming", "miu", "mo", "mou", "mu", "na", "nai", "nan", "nang", "nao", "ne", "nei", "nen", 
                "neng", "ni", "nian", "niang", "niao", "nie", "nin", "ning", "niu", "nong", "nu", "nv", "nuan", "nue", "nuo", "o", 
                "ou", "pa", "pai", "pan", "pang", "pao", "pei", "pen", "peng", "pi", "pian", "piao", "pie", "pin", "ping", "po", 
                "pu", "qi", "qia", "qian", "qiang", "qiao", "qie", "qin", "qing", "qiong", "qiu", "qu", "quan", "que", "qun", "ran", 
                "rang", "rao", "re", "ren", "reng", "ri", "rong", "rou", "ru", "ruan", "rui", "run", "ruo", "sa", "sai", "san", 
                "sang", "sao", "se", "sen", "seng", "sha", "shai", "shan", "shang", "shao", "she", "shen", "sheng", "shi", "shou", "shu", 
                "shua", "shuai", "shuan", "shuang", "shui", "shun", "shuo", "si", "song", "sou", "su", "suan", "sui", "sun", "suo", "ta", 
                "tai", "tan", "tang", "tao", "te", "teng", "ti", "tian", "tiao", "tie", "ting", "tong", "tou", "tu", "tuan", "tui", 
                "tun", "tuo", "wa", "wai", "wan", "wang", "wei", "wen", "weng", "wo", "wu", "xi", "xia", "xian", "xiang", "xiao", 
                "xie", "xin", "xing", "xiong", "xiu", "xu", "xuan", "xue", "xun", "ya", "yan", "yang", "yao", "ye", "yi", "yin", 
                "ying", "yo", "yong", "you", "yu", "yuan", "yue", "yun", "za", "zai", "zan", "zang", "zao", "ze", "zei", "zen", 
                "zeng", "zha", "zhai", "zhan", "zhang", "zhao", "zhe", "zhen", "zheng", "zhi", "zhong", "zhou", "zhu", "zhua", "zhuai", "zhuan", 
                "zhuang", "zhui", "zhun", "zhuo", "zi", "zong", "zou", "zu", "zuan", "zui", "zun", "zuo"
             };
            byte[] bytes = new byte[2];
            string str = "";
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            char[] chArray = text.ToCharArray();
            for (int i = 0; i < chArray.Length; i++)
            {
                bytes = Encoding.Default.GetBytes(chArray[i].ToString());
                num2 = bytes[0];
                num3 = bytes[1];
                num = ((num2 * 0x100) + num3) - 0x10000;
                if ((num > 0) && (num < 160))
                {
                    str = str + chArray[i];
                }
                else
                {
                    for (int j = numArray.Length - 1; j >= 0; j--)
                    {
                        if (numArray[j] < num)
                        {
                            str = str + strArray[j];
                            break;
                        }
                    }
                }
            }
            return str;
        }

        /// <summary>
        /// 截取字符(带汉字)，用...代替后面
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CutLen(string input,int length)
        {
            int num = 0;
            int num2 = 0;
            foreach (var ch in input)
            {
                if(ch>'\x007f')
                {
                    num += 2;       //汉字长度为2
                }
                else
                {
                    num++;
                }
                if(num>length)
                {
                    input = input.Substring(0, num2 - 2) + "...";
                    return input;
                }
                num2++;
            }
            return input;
        }

        /// <summary>
        /// 普通截取字符
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <param name="type">1用...结尾，0用空字符结尾</param>
        /// <returns></returns>
        public static string CutStringNormal(string input,int length,int type)
        {
            if(length>input.Length)
            {
                return input.Trim();
            }
            return (input.Trim().Substring(0,length) + ((type==1)? "...": ""));
        }

        /// <summary>
        /// 截断字符
        /// </summary>
        /// <param name="input"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string CutString(string input, int len)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            int num = 0;
            string str = "";
            byte[] bytes = encoding.GetBytes(input);
            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] == 0x3f)
                {
                    num += 2;
                }
                else
                {
                    num++;
                }
                try
                {
                    str = str + input.Substring(i, 1);
                }
                catch
                {
                    return str;
                }
                if (num > len)
                {
                    return str;
                }
            }
            return str;
        }


        /// <summary>
        /// 截断字符
        /// </summary>
        /// <param name="input"></param>
        /// <param name="startTag"></param>
        /// <param name="endTag"></param>
        /// <returns></returns>
        public static string CutString(string input, string startTag, string endTag)
        {
            return CutString(input, startTag, endTag, false);
        }
        /// <summary>
        /// 截断字符
        /// </summary>
        /// <param name="input"></param>
        /// <param name="startTag"></param>
        /// <param name="endTag"></param>
        /// <param name="enableRegular"></param>
        /// <returns></returns>
        public static string CutString(string input, string startTag, string endTag, bool enableRegular)
        {
            int index = 0;
            if (!input.Contains(startTag) && enableRegular)
            {
                foreach (Match match in Regex.Matches(input, startTag, RegexOptions.Compiled | RegexOptions.IgnoreCase))
                {
                    if (!string.IsNullOrEmpty(match.Value))
                    {
                        startTag = match.Value;
                        break;
                    }
                }
            }
            index = input.IndexOf(startTag);
            string str = "";
            int startIndex = index + startTag.Length;
            if (index == -1)
            {
                return "";
            }
            if (startTag == "0")
            {
                startIndex = 0;
            }
            try
            {
                int length = input.IndexOf(endTag, startIndex) - startIndex;
                if (endTag == "0")
                {
                    length = input.Length;
                }
                str = input.Substring(startIndex, length).Trim();
            }
            catch (Exception)
            {
            }
            return str;
        }

        public static string FilterSymbolStr(string theString)
        {
            string[] strArray = new string[] { 
                "'", "\"", "\r", "\n", "<", ">", "%", "?", ",", ".", "=", "-", "_", ";", "|", "[", 
                "]", "&", "/"
             };
            for (int i = 0; i < strArray.Length; i++)
            {
                theString = theString.Replace(strArray[i], string.Empty);
            }
            return theString;
        }

        /// <summary>
        /// 获取汉字长度
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int GetLen(string input)
        {
            return Encoding.GetEncoding("gb2312").GetBytes(input).Length;
        }

        /// <summary>
        /// 获取匹配次数
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int OccurNum(string pattern, string input)
        {
            try
            {
                return Regex.Matches(input, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase).Count;
            }
            catch (Exception)
            {
                return 0;
            }
        }


        #region "获取随机中文"
        /// <summary>
        /// 获取随机中文
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string RandomChinese(int len)
        {
            Encoding encoding = Encoding.GetEncoding("gb2312");
            object[] objArray = RandomChineseCode(len);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                string str = encoding.GetString((byte[])Convert.ChangeType(objArray[i], typeof(byte[])));
                builder.Append(str);
            }
            return builder.ToString();
        }
        /// <summary>
        /// 获取随机中文
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string RandomChinese2(int len)
        {
            string[] strArray = new string[len];
            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                int num2;
                int num = random.Next(0x10, 0x58);
                if (num == 0x37)
                {
                    num2 = random.Next(1, 90);
                }
                else
                {
                    num2 = random.Next(1, 0x5e);
                }
                strArray[i] = Encoding.GetEncoding("GB2312").GetString(new byte[] { Convert.ToByte((int)(num + 160)), Convert.ToByte((int)(num2 + 160)) });
            }
            StringBuilder builder = new StringBuilder();
            for (int j = 0; j < strArray.Length; j++)
            {
                builder.Append(strArray[j]);
            }
            return builder.ToString();
        }

        private static object[] RandomChineseCode(int len)
        {
            string[] strArray = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };
            Random random = new Random();
            object[] objArray = new object[len];
            for (int i = 0; i < len; i++)
            {
                int num3;
                int num5;
                int index = random.Next(11, 14);
                string str = strArray[index].Trim();
                random = new Random((index * ((int)DateTime.Now.Ticks)) + i);
                if (index == 13)
                {
                    num3 = random.Next(0, 7);
                }
                else
                {
                    num3 = random.Next(0, 0x10);
                }
                string str2 = strArray[num3].Trim();
                random = new Random((num3 * ((int)DateTime.Now.Ticks)) + i);
                int num4 = random.Next(10, 0x10);
                string str3 = strArray[num4].Trim();
                random = new Random((num4 * ((int)DateTime.Now.Ticks)) + i);
                switch (num4)
                {
                    case 10:
                        num5 = random.Next(1, 0x10);
                        break;

                    case 15:
                        num5 = random.Next(0, 15);
                        break;

                    default:
                        num5 = random.Next(0, 0x10);
                        break;
                }
                string str4 = strArray[num5].Trim();
                byte num6 = Convert.ToByte(str + str2, 0x10);
                byte num7 = Convert.ToByte(str3 + str4, 0x10);
                byte[] buffer = new byte[] { num6, num7 };
                objArray.SetValue(buffer, i);
            }
            return objArray;
        }

        #endregion

        #region "随机数字和字符"

        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string RandomNum(int Length)
        {
            return RandomNum(Length, false);
        }
        /// <summary>
        /// 获取随机数字字符
        /// </summary>
        /// <param name="len"></param>
        /// <param name="sleep"></param>
        /// <returns></returns>
        public static string RandomNum(int len, bool sleep)
        {
            if (sleep)
            {
                Thread.Sleep(3);
            }
            string str = "";
            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                str = str + random.Next(10).ToString();
            }
            return str;
        }
        /// <summary>
        /// 随机数字和字母 字符
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string RandomNumSTR(int len)
        {
            string str = "";
            string[] strArray = new string[] { 
                "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f", 
                "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", 
                "w", "x", "y", "z"
             };
            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                str = str + strArray[random.Next(0x24)];
            }
            return str;
        }
        /// <summary>
        /// 随机字母
        /// </summary>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string RandomPureLetter(int Length)
        {
            return RandomPureLetter(Length, false);
        }
        /// <summary>
        /// 随机字母
        /// </summary>
        /// <param name="len"></param>
        /// <param name="sleep"></param>
        /// <returns></returns>
        public static string RandomPureLetter(int len, bool sleep)
        {
            if (sleep)
            {
                Thread.Sleep(3);
            }
            string[] strArray = new string[] { 
                "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", 
                "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"
             };
            string str = "";
            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                str = str + strArray[random.Next(0x1a)];
            }
            return str;
        }
        /// <summary>
        /// 随机字符
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string RandomSTR(int len)
        {
            string str = "";
            string[] strArray = new string[] { 
                "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", 
                "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"
             };
            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                str = str + strArray[random.Next(0x1a)];
            }
            return str;
        }

        #endregion

        #region "从文件中读取文本"
        /// <summary>
        /// 从文件中读取文本
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadFile(string path)
        {
            return ReadFile(path, null);
        }

        /// <summary>
        /// 从文件中读取文本
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pageEncode"></param>
        /// <returns></returns>
        public static string ReadFile(string path, string pageEncode)
        {
            string str = "";
            if (string.IsNullOrEmpty(pageEncode))
            {
                pageEncode = "GB2312";
            }
            try
            {
                if (!File.Exists(path))
                {
                    return str;
                }
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding(pageEncode)))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception)
            {
                return "";
            }
            return str;
        }
        #endregion

        /// <summary>
        /// 反转str
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ReverseStr(string input)
        {
            char[] array = input.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }


        public static void XmlFilter(ref string xml)
        {
            List<string> list = new List<string>();
            int num = OccurNum(@"<!\[CDATA\[", xml);
            if (num > 0)
            {
                string str = "<![CDATA[";
                string str2 = "]]>";
                int startIndex = 0;
                int index = 0;
                for (int i = 0; i < num; i++)
                {
                    startIndex = xml.IndexOf(str);
                    if (startIndex != -1)
                    {
                        index = xml.IndexOf(str2, startIndex);
                        string item = xml.Substring(startIndex, (index - startIndex) + str2.Length);
                        list.Add(item);
                        xml = string.Concat(new object[] { xml.Substring(0, startIndex), "{$$", i, "}", xml.Substring(index + str2.Length) });
                    }
                }
            }
            string pattern = @"[^\x0D\x20-\xD7FF\xE000-\xFFFD\x10000-x10FFFF\u4e00-\u9fa5]";
            xml = Regex.Replace(xml, pattern, "");
            if (num > 0)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    xml = xml.Replace("{$$" + j + "}", list[j]);
                }
            }
        }
    }
}
