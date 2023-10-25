﻿using Abp.Timing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace LibrarySystem.Utils
{
    public class CommonUtil
    {
        private static readonly Dictionary<byte, string> PaymentDueByListReadOnly
        = new Dictionary<byte, string>
        {
            { 0, "Last date next month" },
            { 103, "Last date next 2 month" },
            { 104, "Last date next 3 month" },
            { 15, "15th next month" },
            { 1, "1st next month" },
            { 2, "2nd next month" },
            { 3, "3rd next month" },
            { 4, "4th next month" },
            { 5, "5th next month" },
            { 6, "6th next month" },
            { 7, "7th next month" },
            { 8, "8th next month" },
            { 9, "9th next month" },
            { 10, "10th next month" },
            { 11, "11th next month" },
            { 12, "12th next month" },
            { 13, "13th next month" },
            { 14, "14th next month" },
            { 16, "16th next month" },
            { 17, "17th next month" },
            { 18, "18th next month" },
            { 19, "19th next month" },
            { 20, "20th next month" },
            { 21, "21st next month" },
            { 22, "22nd next month" },
            { 23, "23rd next month" },
            { 24, "24th next month" },
            { 25, "25th next month" },
            { 26, "26th next month" },
            { 27, "27th next month" },
            { 28, "28th next month" },
            { 29, "29th next month" },
            { 30, "30th next month" },
            { 31, "1st next 2 month" },
            { 32, "2nd next 2 month" },
            { 33, "3rd next 2 month" },
            { 34, "4th next 2 month" },
            { 35, "5th next 2 month" },
            { 36, "6th next 2 month" },
            { 37, "7th next 2 month" },
            { 38, "8th next 2 month" },
            { 39, "9th next 2 month" },
            { 40, "10th next 2 month" },
            { 41, "11th next 2 month" },
            { 42, "12th next 2 month" },
            { 43, "13th next 2 month" },
            { 44, "14th next 2 month" },
            { 45, "15th next 2 month" },
            { 46, "16th next 2 month" },
            { 47, "17th next 2 month" },
            { 48, "18th next 2 month" },
            { 49, "19th next 2 month" },
            { 50, "20th next 2 month" },
            { 51, "21st next 2 month" },
            { 52, "22nd next 2 month" },
            { 53, "23rd next 2 month" },
            { 54, "24th next 2 month" },
            { 55, "25th next 2 month" },
            { 56, "26th next 2 month" },
            { 57, "27th next 2 month" },
            { 58, "28th next 2 month" },
            { 59, "29th next 2 month" },
            { 60, "30th next 2 month" },
            { 61, "1st next 3 month" },
            { 62, "2nd next 3 month" },
            { 63, "3rd next 3 month" },
            { 64, "4th next 3 month" },
            { 65, "5th next 3 month" },
            { 66, "6th next 3 month" },
            { 67, "7th next 3 month" },
            { 68, "8th next 3 month" },
            { 69, "9th next 3 month" },
            { 70, "10th next 3 month" },
            { 71, "11th next 3 month" },
            { 72, "12th next 3 month" },
            { 73, "13th next 3 month" },
            { 74, "14th next 3 month" },
            { 75, "15th next 3 month" },
            { 76, "16th next 3 month" },
            { 77, "17th next 3 month" },
            { 78, "18th next 3 month" },
            { 79, "19th next 3 month" },
            { 80, "20th next 3 month" },
            { 81, "21st next 3 month" },
            { 82, "22nd next 3 month" },
            { 83, "23rd next 3 month" },
            { 84, "24th next 3 month" },
            { 85, "25th next 3 month" },
            { 86, "26th next 3 month" },
            { 87, "27th next 3 month" },
            { 88, "28th next 3 month" },
            { 89, "29th next 3 month" },
            { 90, "30th next 3 month" },
        };

        public static Dictionary<byte, string> PaymentDueByList()
        {
            return PaymentDueByListReadOnly;
        }
     
        public static DateTime GetNow()
        {
            return Clock.Provider.Now;
        }

        public static long NowToMilliseconds()
        {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        public static string NowToYYYYMMddHHmmss()
        {
            return GetNow().ToString("yyyyMMddHHmmss");
        }

        public enum TimeSheetUserType : byte
        {
            Staff = 0,
            Internship = 1,
            Collaborators = 2
        }

        public static int LastDateNextThan2Month = 102;


        public static double Round(double value)
        {
            return Math.Round(value);
        }
      
        public static string ConvertHtmlToPlainText(string html)
        {
            if (html == null)
            {
                return null;
            }

            var plainText = HttpUtility.HtmlDecode(
                Regex.Replace(
                    Regex.Replace(
                        Regex.Replace(
                            Regex.Replace(
                                html
                                    .Replace("\\n", "&#10;")
                                    .Replace("</br>", "\n")
                                    .Replace("<br>", "\n")
                                    .Replace("<ul>", "\n")
                                    .Replace("<li>", "• ")
                                    .Replace("</ul>", "\n")
                                    .Replace("<em>", "<i>")
                                    .Replace("</em>", "</i>")
                                    .Replace("<p>", "")
                                    .Replace("</p>", ""),
                                "<.*?>",
                                string.Empty
                            ),
                            @"&(?!(\w+|#\d+);)",
                            "&amp;"
                        ),
                        "&nbsp;",
                        " "
                    ),
                    @"<br(\s*\/)?>",
                    "\n"
                )
            );

            return plainText;
        }

        // Function to get the natural sort key of a string
        public static string GetNaturalSortKey(string value)
        {
            return Regex.Replace(value, "[0-9]+", match => match.Value.PadLeft(10, '0'));
        }

        public static List<string> SeparateMessage(string message, int maxlength, string separteStr)
        {
            if (message.Length <= maxlength)
            {
                return new List<string> { message };
            }

            var arr = message.Split(separteStr);

            return SeparateMessage(arr, maxlength, separteStr);
        }

        public static List<string> SeparateMessage(string[] arrMessage, int maxlength, string separteStr)
        {
            int totalLength = 0;
            var sb = new StringBuilder();
            var resultList = new List<string>();

            for (int i = 0; i < arrMessage.Length; i++)
            {
                var item = arrMessage[i];
                if (totalLength + item.Length < maxlength)
                {
                    sb.Append(item);
                    sb.Append(separteStr);
                    totalLength += item.Length + separteStr.Length;
                }
                else
                {
                    resultList.Add(sb.ToString());
                    sb.Clear();
                    sb.Append(item);
                    sb.Append(separteStr);
                    totalLength = item.Length + separteStr.Length;
                }
                if (i == arrMessage.Length - 1)
                {
                    resultList.Add(sb.ToString());
                }
            }

            return resultList;
        }

    }
}