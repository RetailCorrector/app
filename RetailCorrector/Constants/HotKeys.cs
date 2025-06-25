using System.Windows.Input;

namespace RetailCorrector.Constants
{
    public static class HotKeys
    {
        public readonly static KeyGesture Undo = new(Key.Z, ModifierKeys.Control);
        public readonly static KeyGesture Redo = new(Key.Y, ModifierKeys.Control);

        public readonly static KeyGesture AddReceipt = new(Key.P, ModifierKeys.Alt);
        public readonly static KeyGesture ParseReceipts = new(Key.P, ModifierKeys.Control);

        public readonly static KeyGesture OpenPluginManager = new(Key.O, ModifierKeys.Control);
        public readonly static KeyGesture OpenReportEditor = new(Key.Q, ModifierKeys.Control);
        public readonly static KeyGesture OpenCashier = new(Key.B, ModifierKeys.Alt);
        public readonly static KeyGesture OpenSettings = new(Key.S, ModifierKeys.Alt);
        public readonly static KeyGesture OpenConsole = new(Key.OemTilde, 0, "`");
        public readonly static KeyGesture OpenLogDir = new(Key.OemTilde, ModifierKeys.Shift, "Shift+`");
        public readonly static KeyGesture OpenAbout = new(0);
        public readonly static KeyGesture OpenDocs = new(0);

        public readonly static KeyGesture Clear = new(Key.N, ModifierKeys.Control);

        public readonly static KeyGesture ExitDialog = new(Key.Escape);

    }
}
