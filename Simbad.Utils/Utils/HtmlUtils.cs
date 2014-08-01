using System.Text;

namespace Simbad.Utils.Utils
{
    public static class HtmlUtils
    {
        public static StringBuilder StartHtml(this StringBuilder sb, string title = "", string contentType = "text/html; charset=UTF-8")
        {
            sb.Append("<html>");
            sb.Append("<head>");
            sb.AppendFormat("<meta http-equiv=\"Content-Type\" content=\"{0}\">", contentType);
            sb.Append("</head>");
            sb.Append("<body>");

            return sb;
        }

        public static StringBuilder InjectCss(this StringBuilder sb, string cssFileName)
        {
            sb.AppendFormat("<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}\">", cssFileName);

            return sb;
        }

        public static StringBuilder InjectJs(this StringBuilder sb, string jsFileName)
        {
            sb.AppendFormat("<script type=\"text/javascript\" src=\"{0}\"></script>", jsFileName);

            return sb;
        }

        public static StringBuilder Js(this StringBuilder sb, string code)
        {
            sb.Append("<script type=\"text/javascript\">");
            sb.Append(code);
            sb.Append("</script>");
            return sb;
        }

        public static StringBuilder EndHtml(this StringBuilder sb)
        {
            sb.Append("</body>");
            sb.Append("</html>");

            return sb;
        }

        public static StringBuilder StartTable(this StringBuilder sb, string cssClass = null)
        {
            sb.AppendFormat("<table{0}>", GetCssClassString(cssClass));

            return sb;
        }

        public static StringBuilder TableHeader(this StringBuilder sb, params string[] columnNames)
        {
            return sb.TableRowInternal("th", columnNames);
        }

        public static StringBuilder TableRow(this StringBuilder sb, params string[] contents)
        {
            return sb.TableRowInternal("td", contents);
        }

        private static StringBuilder TableRowInternal(this StringBuilder sb, string tag, params string[] contents)
        {
            sb.Append("<tr>");

            foreach (var content in contents)
            {
                sb.SurroundWith(content, tag);
            }

            sb.Append("</tr>");

            return sb;
        }

        public static StringBuilder H1(this StringBuilder sb, string header)
        {
            return sb.SurroundWith(header, "h1");
        }

        public static StringBuilder H2(this StringBuilder sb, string header)
        {
            return sb.SurroundWith(header, "h2");
        }

        public static StringBuilder H3(this StringBuilder sb, string header)
        {
            return sb.SurroundWith(header, "h3");
        }

        public static StringBuilder StartSpan(this StringBuilder sb, string cssClass = null)
        {
            return StartTag(sb, "span", cssClass);
        }

        public static StringBuilder EndSpan(this StringBuilder sb)
        {
            return EndTag(sb, "span");
        }

        public static StringBuilder StartDiv(this StringBuilder sb, string cssClass = null)
        {
            return StartTag(sb, "div", cssClass);
        }

        public static StringBuilder EndDiv(this StringBuilder sb)
        {
            return EndTag(sb, "div");
        }

        public static StringBuilder StartParagraph(this StringBuilder sb, string cssClass = null)
        {
            return StartTag(sb, "p", cssClass);
        }

        public static StringBuilder EndParagraph(this StringBuilder sb)
        {
            return EndTag(sb, "p");
        }

        public static StringBuilder SurroundWith(this StringBuilder sb, string content, string tag)
        {
            sb.AppendFormat("<{1}>{0}</{1}>", content, tag);

            return sb;
        }

        public static StringBuilder EndTable(this StringBuilder sb)
        {
            sb.Append("</table>");

            return sb;
        }

        private static StringBuilder StartTag(this StringBuilder sb, string tag, string cssClass = null)
        {
            sb.AppendFormat("<{0}{1}>", tag, GetCssClassString(cssClass));

            return sb;
        }

        private static StringBuilder EndTag(this StringBuilder sb, string tag)
        {
            sb.AppendFormat("</{0}>", tag);

            return sb;
        }

        private static string GetCssClassString(string cssClass)
        {
            if (string.IsNullOrWhiteSpace(cssClass))
            {
                return string.Empty;
            }

            return string.Concat(" class=\"", cssClass, "\"");
        }
    }
}