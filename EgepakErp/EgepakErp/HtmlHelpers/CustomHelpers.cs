using System.Web;
using System.Web.Mvc;

namespace EgePakErp.HtmlHelpers
{
    public static class CustomHelpers
    {
        public static IHtmlString DateTimePicker(this HtmlHelper helper, string id)
        {
            TagBuilder tb = new TagBuilder("input");
            tb.Attributes.Add("type", "text");
            tb.Attributes.Add("class", "form-control form-control-solid datetimepicker-input");
            tb.Attributes.Add("id", id);
            tb.Attributes.Add("name", id);
            tb.Attributes.Add("placeholder", "Lütfen Tarih Seçin");          
            tb.Attributes.Add("data-toggle", "datetimepicker");
            tb.Attributes.Add("data-target", "#"+id);
            return new MvcHtmlString(tb.ToString());
        }
    }
}