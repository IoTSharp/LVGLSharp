

using LVGLSharp.Forms;

public class TableLayoutPanel : Control
{
    public int ColumnCount { get; set; }
    public List<ColumnStyle> ColumnStyles { get; set; }
    public int RowCount { get; set; }
    public RowStyle RowStyles { get; set; }

   

    public void SetColumnSpan(FlowLayoutPanel recv_container, int v)
    {
    }
}