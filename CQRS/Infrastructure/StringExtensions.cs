// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Project Attack Ltd">
//   2012 Project Attack Ltd
// </copyright>
// <summary>
//   The string extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Infrastructure
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;

    /// <summary>
    /// The string extensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// The hash tag pattern.
        /// </summary>
        private const string HashTagPattern = @"#([A-Za-z0-9\-_&;]+)";

        /// <summary>
        /// The hyper link pattern.
        /// </summary>
        private const string HyperLinkPattern = @"(http://\S+)\s?";

        /// <summary>
        /// The screen name pattern.
        /// </summary>
        private const string ScreenNamePattern = @"@([A-Za-z0-9\-_&;]+)";

        /// <summary>
        /// The clean.
        /// </summary>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <returns>
        /// The clean.
        /// </returns>
        public static string Clean(this string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return string.Empty;
            }

            // to lowercase, trim extra spaces
            title = title.ToLower().Trim();

            int len = title.Length;
            var sb = new StringBuilder(len);
            bool prevdash = false;
            char c;

            for (int i = 0; i < title.Length; i++)
            {
                c = title[i];
                if (c == ',' || c == '/' || c == '.' || c == '\\' || c == '-')
                {
                    if (!prevdash)
                    {
                        sb.Append('-');
                        prevdash = true;
                    }
                }
                else if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') || c == ' ' || c == 'á')
                {
                    sb.Append(c);
                    prevdash = false;
                }

                if (i == 80)
                {
                    break;
                }
            }

            title = sb.ToString();

            // remove trailing dash, if there is one
            if (title.EndsWith("-"))
            {
                title = title.Substring(0, title.Length - 1);
            }

            return title;
        }

        /// <summary>
        /// turns HelloWorld into Hello World
        /// </summary>
        /// <param name="text">
        /// </param>
        /// <returns>
        /// The de camel.
        /// </returns>
        public static string DeCamel(this string text)
        {
            return Regex.Replace(text, @"([A-Z])", @" $&").Trim();
        }

        /// <summary>
        /// The de camel test.
        /// </summary>
        public static void DeCamelTest()
        {
            Console.WriteLine("HelloWorldIAmYourNemesis".DeCamel());
        }

        /// <summary>
        /// The join.
        /// </summary>
        /// <param name="values">
        /// The values.
        /// </param>
        /// <param name="joinText">
        /// The join text.
        /// </param>
        /// <returns>
        /// The join.
        /// </returns>
        public static string Join(this string[] values, 
                                  string joinText)
        {
            var result = new StringBuilder();

            if (values.Length == 0)
            {
                return string.Empty;
            }

            result.Append(values[0]);

            for (int i = 1; i < values.Length; i++)
            {
                result.Append(joinText);
                result.Append(values[i]);
            }

            return result.ToString();
        }

        /// <summary>
        /// prettily renders property names
        /// </summary>
        /// <param name="text">
        /// </param>
        /// <returns>
        /// The pretty.
        /// </returns>
        public static string Pretty(this string text)
        {
            return DeCamel(text).Replace("_", " ");
        }

        /// <summary>
        /// The pretty test.
        /// </summary>
        public static void PrettyTest()
        {
            Console.WriteLine("hello_worldIAmYourNemesis".Pretty());
        }

        /// <summary>
        /// The replace links.
        /// </summary>
        /// <param name="arg">
        /// The arg.
        /// </param>
        /// <returns>
        /// The replace links.
        /// </returns>
        public static string ReplaceLinks(this string arg)
        {
            if (!string.IsNullOrEmpty(arg))
            {
                var urlregex = new Regex(@"(^|[\n ])(?<url>(www|ftp)\.[^ ,""\s<]*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                arg = urlregex.Replace(arg, " <a href=\"http://${url}\" target=\"_blank\">${url}</a>");
                var httpurlregex = new Regex(@"(^|[\n ])(?<url>(http://www\.|http://|https://)[^ ,""\s<]*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                arg = httpurlregex.Replace(arg, " <a href=\"${url}\" target=\"_blank\">${url}</a>");
                var emailregex = new Regex(@"([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
                arg = emailregex.Replace(arg, " <a href='mailto:$1@$2$6'>$1@$2$6</a> ");

                // Regex twitterUsernameRegex = new Regex(@" @([A-Za-z0-9\-_&;]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                // arg = twitterUsernameRegex.Replace("@" + arg, "<a href=\"http://twitter.com/$dfg4\">@$dfg4</a>");

                // Regex twitterHashtagRegex = new Regex(@"#([A-Za-z0-9\-_&;]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                // arg = twitterHashtagRegex.Replace("#" + arg, "<a href=\"http://twitter.com/search?q=$dsgfs33\">#$dsgfs33</a>");
            }

            return arg;
        }

        /// <summary>
        /// The to friendly url string.
        /// </summary>
        /// <param name="stringToClean">
        /// The string to clean.
        /// </param>
        /// <returns>
        /// The to friendly url string.
        /// </returns>
        public static string ToFriendlyUrlString(this string stringToClean)
        {
            stringToClean = (stringToClean ?? string.Empty).Trim().ToLower();

            var url = new StringBuilder();

            foreach (char ch in stringToClean)
            {
                switch (ch)
                {
                    case ' ':
                        url.Append('-');
                        break;
                    case '&':
                        url.Append("and");
                        break;
                    case '\'':
                        break;
                    default:
                        if ((ch >= '0' && ch <= '9') || (ch >= 'a' && ch <= 'z'))
                        {
                            url.Append(ch);
                        }
                        else
                        {
                            url.Append('-');
                        }

                        break;
                }
            }

            return url.ToString();
        }

        /// <summary>
        /// The to title case.
        /// </summary>
        /// <param name="sentence">
        /// The sentence.
        /// </param>
        /// <returns>
        /// The to title case.
        /// </returns>
        public static string ToTitleCase(this string sentence)
        {
            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(sentence);
        }

        /// <summary>
        /// The trim with elipsis.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <param name="length">
        /// The length.
        /// </param>
        /// <returns>
        /// The trim with elipsis.
        /// </returns>
        public static string TrimWithElipsis(this string text, 
                                             int length)
        {
            if (text.Length <= length)
            {
                return text;
            }

            return text.Substring(0, length) + "...";
        }

        /// <summary>
        /// replacement for String.Format
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The with.
        /// </returns>
        public static string With(this string format, 
                                  params object[] args)
        {
            return string.Format(CultureInfo.CurrentUICulture,  format, args);
        }
    }
}
