using System.Windows.Input;

namespace RetailCorrector.Constants
{
    public static class HotKeys
    {
        public readonly static KeyGesture Undo = new(Key.Z, ModifierKeys.Control);
        public readonly static KeyGesture Redo = new(Key.Y, ModifierKeys.Control);

        public readonly static KeyGesture AddReceipt = new(Key.P, ModifierKeys.Alt);
        public readonly static KeyGesture ParseReceipts = new(Key.P, ModifierKeys.Control);
        public readonly static KeyGesture PasteReceipt = new(Key.V, ModifierKeys.Control);
        public readonly static KeyGesture DuplicateReceipts = new(Key.D, ModifierKeys.Control);
        public readonly static KeyGesture RemoveReceipts = new(Key.Delete);

        public readonly static KeyGesture OpenPluginManager = new(Key.O, ModifierKeys.Control);
        public readonly static KeyGesture OpenReportEditor = new(Key.Q, ModifierKeys.Control);
        public readonly static KeyGesture OpenCashier = new(Key.B, ModifierKeys.Alt);
        public readonly static KeyGesture OpenSettings = new(Key.S, ModifierKeys.Alt);
        public readonly static KeyGesture OpenConsole = new(Key.OemTilde);
        public readonly static KeyGesture OpenAbout = new(Key.OemTilde, ModifierKeys.Alt);
        public readonly static KeyGesture OpenDocs = new(Key.OemTilde, ModifierKeys.Control);

        public readonly static KeyGesture Clear = new(Key.N, ModifierKeys.Control);

        public readonly static KeyGesture InvertSelection = new(Key.I, ModifierKeys.Control);

        public readonly static KeyGesture InvertOperation = new(Key.I, ModifierKeys.Alt);

    }
}
