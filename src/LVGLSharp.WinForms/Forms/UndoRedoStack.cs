namespace LVGLSharp.Forms
{
    /// <summary>
    /// 文本编辑操作记录
    /// </summary>
    internal class TextEditAction
    {
        public string Text { get; set; } = string.Empty;
        public int CursorPosition { get; set; }
        public int SelectionLength { get; set; }
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// 撤销/重做栈管理器
    /// </summary>
    internal class UndoRedoStack
    {
        private readonly Stack<TextEditAction> _undoStack = new();
        private readonly Stack<TextEditAction> _redoStack = new();
        private readonly int _maxCapacity;
        private TextEditAction? _lastAction;

        public bool CanUndo => _undoStack.Count > 0;
        public bool CanRedo => _redoStack.Count > 0;

        public UndoRedoStack(int maxCapacity = 100)
        {
            _maxCapacity = maxCapacity;
        }

        /// <summary>
        /// 记录新的编辑操作
        /// </summary>
        public void Push(string text, int cursorPosition, int selectionLength)
        {
            var action = new TextEditAction
            {
                Text = text,
                CursorPosition = cursorPosition,
                SelectionLength = selectionLength,
                Timestamp = DateTime.Now
            };

            // 如果在短时间内连续输入，合并操作
            if (_lastAction != null && 
                (action.Timestamp - _lastAction.Timestamp).TotalMilliseconds < 500 &&
                Math.Abs(action.Text.Length - _lastAction.Text.Length) == 1)
            {
                // 更新最后的操作
                _lastAction.Text = action.Text;
                _lastAction.CursorPosition = action.CursorPosition;
                _lastAction.SelectionLength = action.SelectionLength;
                _lastAction.Timestamp = action.Timestamp;
                return;
            }

            _undoStack.Push(action);
            _lastAction = action;

            // 清空重做栈
            _redoStack.Clear();

            // 限制栈大小
            if (_undoStack.Count > _maxCapacity)
            {
                var temp = new Stack<TextEditAction>(_undoStack.Reverse().Take(_maxCapacity));
                _undoStack.Clear();
                foreach (var item in temp.Reverse())
                {
                    _undoStack.Push(item);
                }
            }
        }

        /// <summary>
        /// 撤销操作
        /// </summary>
        public TextEditAction? Undo(string currentText, int cursorPosition, int selectionLength)
        {
            if (!CanUndo) return null;

            // 保存当前状态到重做栈
            _redoStack.Push(new TextEditAction
            {
                Text = currentText,
                CursorPosition = cursorPosition,
                SelectionLength = selectionLength,
                Timestamp = DateTime.Now
            });

            return _undoStack.Pop();
        }

        /// <summary>
        /// 重做操作
        /// </summary>
        public TextEditAction? Redo(string currentText, int cursorPosition, int selectionLength)
        {
            if (!CanRedo) return null;

            // 保存当前状态到撤销栈
            _undoStack.Push(new TextEditAction
            {
                Text = currentText,
                CursorPosition = cursorPosition,
                SelectionLength = selectionLength,
                Timestamp = DateTime.Now
            });

            return _redoStack.Pop();
        }

        /// <summary>
        /// 清空栈
        /// </summary>
        public void Clear()
        {
            _undoStack.Clear();
            _redoStack.Clear();
            _lastAction = null;
        }
    }
}
