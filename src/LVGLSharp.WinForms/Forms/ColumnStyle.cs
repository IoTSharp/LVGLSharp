using System.Diagnostics.CodeAnalysis;
namespace LVGLSharp.Forms
{
   
    //
    // 摘要:
    //     Represents the look and feel of a column in a table layout.
    public class ColumnStyle : TableLayoutStyle
    {
        //
        // 摘要:
        //     Initializes a new instance of the System.Windows.Forms.ColumnStyle class to its
        //     default state.
        public ColumnStyle()
        {

        }
        //
        // 摘要:
        //     Initializes a new instance of the System.Windows.Forms.ColumnStyle class using
        //     the supplied System.Windows.Forms.SizeType value.
        //
        // 参数:
        //   sizeType:
        //     A System.Windows.Forms.TableLayoutStyle.SizeType indicating how the column should
        //     be should be sized relative to its containing table.
        public ColumnStyle(SizeType sizeType)
        {
            SizeType=sizeType;
        }
        //
        // 摘要:
        //     Initializes a new instance of the System.Windows.Forms.ColumnStyle class using
        //     the supplied System.Windows.Forms.SizeType and width values.
        //
        // 参数:
        //   sizeType:
        //     A System.Windows.Forms.TableLayoutStyle.SizeType indicating how the column should
        //     be should be sized relative to its containing table.
        //
        //   width:
        //     The preferred width, in pixels or percentage, depending on the sizeType parameter.
        //
        //
        // 异常:
        //   T:System.ArgumentOutOfRangeException:
        //     width is less than 0.
        public ColumnStyle(SizeType sizeType, float width)
        {
            SizeType = sizeType;
            Width=width;
        }

        //
        // 摘要:
        //     Gets or sets the width value for a column.
        //
        // 返回结果:
        //     The preferred width, in pixels or percentage, depending on the System.Windows.Forms.TableLayoutStyle.SizeType
        //     property.
        //
        // 异常:
        //   T:System.ArgumentOutOfRangeException:
        //     The value is less than 0 when setting this property.
        public float Width { get; set; }
    }
}