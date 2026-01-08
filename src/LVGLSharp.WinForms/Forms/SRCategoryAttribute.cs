
namespace LVGLSharp.Forms
{
    public class SRCategoryAttribute : Attribute
    {
        public SRCategoryAttribute()
        {
            Category = "LVGL";
        }

        public SRCategoryAttribute(string _category)
        {
            Category = _category;
        }

        public string Category { get; }
    }
}