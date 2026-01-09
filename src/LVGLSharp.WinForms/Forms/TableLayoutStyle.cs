using System.ComponentModel;

namespace LVGLSharp.Forms
{
    public class TableLayoutStyle
    {
        //
        // 摘要:
        //     Initializes a new instance of the System.Windows.Forms.TableLayoutStyle class.
        protected TableLayoutStyle()
        {
            SizeType = SizeType.AutoSize;
        }

        //
        // 摘要:
        //     Gets or sets a flag indicating how a row or column should be sized relative to
        //     its containing table.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.SizeType values that specifies how rows or columns
        //     of user interface (UI) elements should be sized relative to their container.
        //     The default is System.Windows.Forms.SizeType.AutoSize.
        [DefaultValue(SizeType.AutoSize)]
        public SizeType SizeType { get; set; }

    }
}