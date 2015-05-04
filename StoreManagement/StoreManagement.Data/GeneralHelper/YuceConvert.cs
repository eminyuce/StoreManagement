using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace StoreManagement.Data.GeneralHelper
{
    public static class YuceConvert
    {
        private static readonly Regex CarriageRegex = new Regex(@"(\r\n|\r|\n)+");
        //remove carriage returns from the header name
        public static string RemoveCarriage(this string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return "";
            }
            return CarriageRegex.Replace(text, string.Empty).Trim();
        }
 


        public static string ToYesNoString(this bool value)
        {
            return value ? "Yes" : "No";
        }


    
        public static string ToCssClass(this string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return "";
            }
            Regex re = new Regex(@"[!""#$%&'()\*\+,\./:;<=>\?@\[\\\]^`{\|}~ ]");
            return re.Replace(str, String.Empty).ToLowerInvariant();
        }


        public static string StripHtml(string html)
        {
            html = html.ToStr();
            char[] array = new char[html.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < html.Length; i++)
            {
                char let = html[i];
                if (@let == '<')
                {
                    inside = true;
                    continue;
                }
                if (@let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = @let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }


        public static string TruncateAtSentence(this string text, int length, int lengthMin = -1,
                                        bool addEllipsis = true)
        {
            if (String.IsNullOrEmpty(text) || text.Length <= length) return text;


            const string ellChar = "…";
            const int defLengthMin = 20;
            char[] goodChars = { ',', ';' };
            char[] badChars = { ':', '-', '—', '–', ' ' };

            var r = text;
            if (lengthMin < 0 || lengthMin > length)
            {

                lengthMin = length * .8 > defLengthMin ? (length * 0.8).ToInt() : defLengthMin;

            }

            Regex rx = new Regex(@"(\S.+?[.!?])(?=\s+|$)");
            var ret = new StringBuilder();
            foreach (Match match in rx.Matches(text))
            {
                if (ret.ToString().Length + match.Value.Trim().Length + 1 <= length)
                {
                    if (ret.Length > 0)
                        ret.Append(" ");

                    ret.Append(match.Value.Trim());

                }
                else
                {
                    if (ret.Length >= lengthMin)
                        return ret.ToString();
                    else
                    {
                        if (ret.Length > 0)
                            ret.Append(" ");

                        ret.Append(match.Value.Trim());
                        break;
                    }
                }
            }

            r = ret.ToString().Trim();
            if (r.Length <= length) return r;

            int index;
            index = r.IndexOfAny(goodChars, lengthMin, length - lengthMin);
            if (index > 0)
                return addEllipsis ? r.Substring(0, index).Trim() + ellChar : r.Substring(0, index).Trim();

            index = r.IndexOfAny(badChars, lengthMin, length - lengthMin);
            if (index > 0)
                return addEllipsis ? r.Substring(0, index).Trim() + ellChar : r.Substring(0, index).Trim();

            return addEllipsis ? r.Substring(0, length).Trim() + ellChar : r.Substring(0, length).Trim();
        }






        public static int ToInt(this object arg)
        {
            int ret = 0;

            Int32.TryParse(arg.ToStr(), out ret);

            return ret;
        }


        public static int ToIntFirst(this object arg, bool allowCommasInside)
        {
            int ret = 0;

            Regex rx = allowCommasInside ? new Regex(@"(\d|,)+") : new Regex(@"(\d)+");

            var match = rx.Match(arg.ToStr());
            string val = allowCommasInside ? match.Value.Replace(",", "") : match.Value;



            Int32.TryParse(val, out ret);

            return ret;
        }


        public static long ToLong(this object arg)
        {
            long ret = 0;

            Int64.TryParse(arg.ToStr(), out ret);

            return ret;
        }
        public static float ToFloat(this object arg)
        {
            float ret = 0;


            Single.TryParse(arg.ToStr(), out ret);

            return ret;
        }

        public static double ToDouble(this object arg)
        {
            double ret = 0;


            Double.TryParse(arg.ToStr(), out ret);

            return ret;
        }


        public static string ToStr(this object arg)
        {
            string ret = String.Empty;
            if (arg != null)
            {
                ret = arg.ToString().Trim();
            }
            return ret;
        }


        public static string ToStr(this object arg, int length)
        {
            string ret = String.Empty;
            if (arg != null)
            {
                ret = arg.ToString();
            }
            if (ret.Length > length)
            {
                return ret.Substring(0, length);
            }
            else
            {
                return ret;
            }
        }

        public static string ToStr(this string text, int minLen, int maxLen)
        {
            string s = text != null ? text : "";
            if (s.Length > maxLen) s = s.Substring(0, maxLen).Trim();

            int ix = 0;
            ix = s.LastIndexOf(".");
            if (ix > minLen)
            {
                s = s.Substring(0, ix + 1).Trim();
            }
            else if ((ix = s.LastIndexOf(",")) > minLen)
            {
                s = s.Substring(0, ix).Trim();

            }
            else if ((ix = s.LastIndexOf(" ")) > minLen)
            {
                s = s.Substring(0, ix).Trim();
            }

            return s;
        }

        public static bool HasValue(this object arg)
        {
            string ret = String.Empty;
            if (arg != null)
            {
                ret = arg.ToString();
            }
            return !String.IsNullOrEmpty(ret);
        }


        public static string ToTitleCase(this string text)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            return textInfo.ToTitleCase(text);
        }

        public static string HtmlDecode(this string arg)
        {
            return WebUtility.HtmlDecode(arg);
        }
        public static string HtmlEncode(this string arg)
        {
            return WebUtility.HtmlEncode(arg);
        }
        public static string NoRepeatedSpaces(string arg)
        {

            return Regex.Replace(arg, " {2,}", " ");
        }


        public static string NoBreakLine(string arg)
        {

            return arg.Replace("\r", " ").Replace("\n", " ");
        }


        public static string ToNormal(this string arg)
        {
            return NoRepeatedSpaces(NoBreakLine(arg.HtmlDecode()));
        }


        public static bool ToBool(this object arg, bool defaultValue = false)
        {
            bool ret = defaultValue;

            if (!Boolean.TryParse(arg.ToStr(), out ret))
            {
                if (arg.ToStr().ToLower().Contains((!defaultValue).ToString().ToLower()))
                {
                    ret = !defaultValue;
                }

            }



            return ret;
        }


        static Regex _dateRegex = new Regex(@"^(19|20)(\d\d)[- /.]?(0[1-9]|1[012])[- /.]?(0[1-9]|[12][0-9]|3[01])$", RegexOptions.Compiled);




        public static DateTime? ToNullableDateTime(this object arg)
        {
            DateTime ret = DateTime.MinValue;

            if (!DateTime.TryParse(arg.ToStr(), out ret))
            {
                Match md = _dateRegex.Match(arg.ToStr());

                if (md != null && md.Groups.Count == 5)
                {
                    int year = (md.Groups[1].Value + md.Groups[2].Value).ToInt();
                    int month = (md.Groups[3].Value).ToInt();
                    int day = (md.Groups[4].Value).ToInt();
                    try
                    {
                        ret = new DateTime(year, month, day);
                    }
                    catch { }
                }


            }

            if (ret != DateTime.MinValue)
            {
                return ret;
            }
            else
            {
                return null;
            }
        }



        public static DateTime ToDateTime(this object arg)
        {
            DateTime ret = DateTime.MinValue;

            if (!DateTime.TryParse(arg.ToStr(), out ret))
            {
                Match md = _dateRegex.Match(arg.ToStr());

                if (md != null && md.Groups.Count == 5)
                {
                    int year = (md.Groups[1].Value + md.Groups[2].Value).ToInt();
                    int month = (md.Groups[3].Value).ToInt();
                    int day = (md.Groups[4].Value).ToInt();
                    try
                    {
                        ret = new DateTime(year, month, day);
                    }
                    catch { }
                }


            }



            return ret;
        }



        public static string ToFlexDateTime(this DateTime dt)
        {

            string rt = "";
            if (dt > DateTime.Now.Date)
            {
                rt = dt.ToString("h:mmtt").ToLower();
            }
            //else if (dt > DateTime.Now.AddDays(-DateTime.Now.DayOfYear))
            //{

            //    rt = dt.ToString("MMM dd");
            //}
            else if (dt > DateTime.MinValue)
            {
                rt = dt.ToString("MMM dd, yyyy ");
            }
            return rt;
        }


        public static string ToStockDateTime(this DateTime? dt)
        {

            string rt = "";
            if (!dt.HasValue)
            {
                return "";
            }

            if (dt > DateTime.Now.Date)
            {
                rt = string.Format("{0} {1}", dt.Value.ToString("h:mmtt").ToLower(), GetTzAbbreviation(dt));
                //4:00PM EST
            }

            else if (dt > DateTime.MinValue)
            {
                rt = string.Format("{0} {1}", dt.Value.ToString("MMM d, yyyy h:mmtt ").ToLower(), GetTzAbbreviation(dt));

                //rt = dt.Value.ToString("MMM d, yyyy h:mmtt ");
                // Mar 2, 2015 4:00PM EST
            }
            return rt;
        }

        public static string GetTzAbbreviation(DateTime? dt)
        {

            if (!dt.HasValue)
            {
                return "";
            }

            string timeZoneName = TimeZone.CurrentTimeZone.IsDaylightSavingTime(dt.Value)
                       ? TimeZone.CurrentTimeZone.DaylightName
                       : TimeZone.CurrentTimeZone.StandardName;


            string output = string.Empty;

            string[] timeZoneWords = timeZoneName.Split(' ');
            foreach (string timeZoneWord in timeZoneWords)
            {
                if (timeZoneWord[0] != '(')
                {
                    output += timeZoneWord[0];
                }
                else
                {
                    output += timeZoneWord;
                }
            }
            return output;
        }


        public static object DeepClone(object obj)
        {
            object objResult = null;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, obj);

                ms.Position = 0;
                objResult = bf.Deserialize(ms);
            }
            return objResult;
        }



        public static string UrlEncode(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";
            //return UrlEncodeCustom(text);
         //   return HttpUtility.UrlEncode(text.Replace(" ", "_")).ToLower();
            return UrlDencode(text, true);
            //char c;
            //((int) c).ToString("X");

        }

        public static string UrlDecode(this string text)
        {
            //return HttpUtility.UrlDecode(text);
            //return UrlDecodeCustom(text);
            // return HttpUtility.UrlDecode(text).Replace("_", " ").ToLower();

            return UrlDencode(text, false);
        }


        private static string UrlEncodeCustom(string text)
        {
            StringBuilder ret = new StringBuilder();

            foreach (var c in text)
            {
                if (c >= '0' && c <= '9' ||
                    c >= 'a' && c <= 'z' ||
                    c >= 'A' && c <= 'Z' ||
                    c == ' '
                    //|| c == '-'
                    )
                {
                    ret.Append(c);
                }
                else
                {
                    ret.Append("~" + ((int)c).ToString("X"));
                }
            }

            return ret.ToString().Replace(" ", "_");
        }
        public static string UrlDencode(this string adres, bool encode)
        {
            string[] karakter = { "<", ">", "#", "%", "{", "}", "|", @"\", "^", "~", "[", "]", "`", ";", "/", "?", ":", "@", "=", "&", "$" };
            string[] donustur = { "%3C", "%3E", "%23", "%25", "%7B", "%7D", "%7C", "%5C", "%5E", "%7E", "%5B", "%5D", "%60", "%3B", "%2F", "%3F", "%3A", "%40", "%3D", "%26", "%24" };

            if (encode)
            {
                for (int i = 0; i < karakter.Length; i++)
                {
                    adres = adres.Replace(karakter[i], donustur[i]);
                }
            }
            else
            {
                for (int i = 0; i < donustur.Length; i++)
                {
                    adres = adres.Replace(donustur[i], karakter[i]);
                }
            }
            return adres;
        }

        private static string UrlDecodeCustom(string text)
        {

            var chars = text.Replace("_", " ").ToCharArray();

            StringBuilder ret = new StringBuilder();

            int i = 0;
            while (i < chars.Length)
            {
                char c = chars[i];
                if (c >= '0' && c <= '9' ||
                    c >= 'a' && c <= 'z' ||
                    c >= 'A' && c <= 'Z' ||
                    c == ' ' || c == '-')
                {
                    ret.Append(c);
                    i++;
                }
                else if (c == '~' && i + 2 < chars.Length)
                {

                    try
                    {
                        string hexValue = chars[i + 1].ToString() + chars[i + 2].ToString();
                        int intChar = Int32.Parse(hexValue, NumberStyles.HexNumber);
                        ret.Append((char)intChar);
                    }
                    catch (Exception)
                    {
                    }

                    i = i + 3;

                }
                else
                {
                    break;
                }
            }

            return ret.ToString();
        }


        public static string TextToSearchResult(string text, string search, int length = 300,
                                                bool isWrapSearchBold = false)
        {

            if (string.IsNullOrEmpty(search)) return text.TruncateAtSentence(length);

            // TO DO: cut text on ponctuation or space
            char[] chars = { ',', '.', ' ', ';', '!', '?' };
            var start = 0;
            var end = text.Length - 1;
            var threshold = 10;
            string ret = "";

            int searchIndex = text.IndexOf(search, StringComparison.InvariantCultureIgnoreCase);

            if (searchIndex < 0 && search.Contains(" ")) //if not found take fist word
            {
                search = search.Split(' ').First();
                searchIndex = text.IndexOf(search, StringComparison.InvariantCultureIgnoreCase);

            }


            if (searchIndex <= 0 || searchIndex < length / 2 - search.Length || text.Length < length)
            {
                ret = text.ToStr(length, length + threshold);
            }
            else
            {
                if (searchIndex > threshold + (length - search.Length) / 2)
                {
                    start = Math.Max(start, text.IndexOfAny(chars, searchIndex - threshold - (length - search.Length) / 2, threshold + (length - search.Length) / 2));
                }

                var tmpEnd = searchIndex + search.Length + (length - search.Length) / 2 + threshold;

                end = Math.Min(end, tmpEnd);

                if (end == tmpEnd)
                {
                    var tmpIndex = text.IndexOfAny(chars, searchIndex + search.Length + (length - search.Length) / 2,
                                                   threshold);
                    if (tmpIndex > 0)
                        end = Math.Min(end, tmpIndex);
                }



                //ret = text.Substring(searchIndex - length / 2 + search.Length, length);
                ret = text.Substring(start, end - start);
            }



            if (isWrapSearchBold)
            {
                return ret.Replace(search, String.Format("<b>{0}</b>", search));
            }
            else
            {
                return ret;
            }


        }
    }
}
