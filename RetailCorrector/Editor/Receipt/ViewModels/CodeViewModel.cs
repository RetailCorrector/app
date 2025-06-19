namespace RetailCorrector.Editor.Receipt.ViewModels
{
    public partial class CodeViewModel
    {
        [NotifyUpdated] private string _unknown = "";
        [NotifyUpdated] private string _EAN8 = "";
        [NotifyUpdated] private string _EAN13 = "";
        [NotifyUpdated] private string _ITF14 = "";
        [NotifyUpdated] private string _GS1_0 = "";
        [NotifyUpdated] private string _GS1_M = "";
        [NotifyUpdated] private string _KMK = "";
        [NotifyUpdated] private string _MI = "";
        [NotifyUpdated] private string _EGAIS2 = "";
        [NotifyUpdated] private string _EGAIS3 = "";
        [NotifyUpdated] private string _f1 = "";
        [NotifyUpdated] private string _f2 = "";
        [NotifyUpdated] private string _f3 = "";
        [NotifyUpdated] private string _f4 = "";
        [NotifyUpdated] private string _f5 = "";
        [NotifyUpdated] private string _f6 = "";

        public static implicit operator CodeViewModel(PositionCode ro) => new()
        {
            Unknown = ro.Unknown,
            EAN8 = ro.EAN8,
            EAN13 = ro.EAN13,
            ITF14 = ro.ITF14,
            GS1_0 = ro.GS1_0,
            GS1_M = ro.GS1_M,
            KMK = ro.KMK,
            MI = ro.MI,
            EGAIS2 = ro.EGAIS2,
            EGAIS3 = ro.EGAIS3,
            F1 = ro.F1,
            F2 = ro.F2,
            F3 = ro.F3,
            F4 = ro.F4,
            F5 = ro.F5,
            F6 = ro.F6,
        };

        public static implicit operator PositionCode(CodeViewModel rw) => new()
        {
            Unknown = rw.Unknown,
            EAN8 = rw.EAN8,
            EAN13 = rw.EAN13,
            ITF14 = rw.ITF14,
            GS1_0 = rw.GS1_0,
            GS1_M = rw.GS1_M,
            KMK = rw.KMK,
            MI = rw.MI,
            EGAIS2 = rw.EGAIS2,
            EGAIS3 = rw.EGAIS3,
            F1 = rw.F1,
            F2 = rw.F2,
            F3 = rw.F3,
            F4 = rw.F4,
            F5 = rw.F5,
            F6 = rw.F6,
        };
    }
}
