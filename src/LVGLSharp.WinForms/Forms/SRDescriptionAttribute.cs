
namespace LVGLSharp.Forms
{
    public class SRDescriptionAttribute : Attribute
    {
        public string Description { set; get; }
        public SRDescriptionAttribute() { }
        public SRDescriptionAttribute(string _description)
        {
            Description = _description;


        }

    }
}