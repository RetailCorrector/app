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

        public static implicit operator CodeViewModel(PositionCode[] ro)
        {
            var vm = new CodeViewModel();
            foreach(var code in ro)
            {
                switch (code.Type)
                {
                    case Enums.PositionCodeType.Unknown:
                        vm.Unknown = code.Value;
                        break;
                    case Enums.PositionCodeType.EAN8:
                        vm.EAN8 = code.Value;
                        break;
                    case Enums.PositionCodeType.EAN13:
                        vm.EAN13 = code.Value;
                        break;
                    case Enums.PositionCodeType.ITF14:
                        vm.ITF14 = code.Value;
                        break;
                    case Enums.PositionCodeType.GS1_0:
                        vm.GS1_0 = code.Value;
                        break;
                    case Enums.PositionCodeType.GS1_M:
                        vm.GS1_M = code.Value;
                        break;
                    case Enums.PositionCodeType.KMK:
                        vm.KMK = code.Value;
                        break;
                    case Enums.PositionCodeType.MI:
                        vm.MI = code.Value;
                        break;
                    case Enums.PositionCodeType.EGAIS2:
                        vm.EGAIS2 = code.Value;
                        break;
                    case Enums.PositionCodeType.EGAIS3:
                        vm.EGAIS3 = code.Value;
                        break;
                    case Enums.PositionCodeType.F1:
                        vm.F1 = code.Value;
                        break;
                    case Enums.PositionCodeType.F2:
                        vm.F2 = code.Value;
                        break;
                    case Enums.PositionCodeType.F3:
                        vm.F3 = code.Value;
                        break;
                    case Enums.PositionCodeType.F4:
                        vm.F4 = code.Value;
                        break;
                    case Enums.PositionCodeType.F5:
                        vm.F5 = code.Value;
                        break;
                    case Enums.PositionCodeType.F6:
                        vm.F6 = code.Value;
                        break;
                }
            }
            return vm;
        }

        public static implicit operator PositionCode[](CodeViewModel rw)
        {
            List<PositionCode> codes = [];
            if (!string.IsNullOrWhiteSpace(rw.Unknown))
                codes.Add(new PositionCode(rw.Unknown, Enums.PositionCodeType.Unknown));
            if (!string.IsNullOrWhiteSpace(rw.EAN8))
                codes.Add(new PositionCode(rw.EAN8, Enums.PositionCodeType.EAN8));
            if (!string.IsNullOrWhiteSpace(rw.EAN13))
                codes.Add(new PositionCode(rw.EAN13, Enums.PositionCodeType.EAN13));
            if (!string.IsNullOrWhiteSpace(rw.ITF14))
                codes.Add(new PositionCode(rw.ITF14, Enums.PositionCodeType.ITF14));
            if (!string.IsNullOrWhiteSpace(rw.GS1_0))
                codes.Add(new PositionCode(rw.GS1_0, Enums.PositionCodeType.GS1_0));
            if (!string.IsNullOrWhiteSpace(rw.GS1_M))
                codes.Add(new PositionCode(rw.GS1_M, Enums.PositionCodeType.GS1_M));
            if (!string.IsNullOrWhiteSpace(rw.KMK))
                codes.Add(new PositionCode(rw.KMK, Enums.PositionCodeType.KMK));
            if (!string.IsNullOrWhiteSpace(rw.MI))
                codes.Add(new PositionCode(rw.MI, Enums.PositionCodeType.MI));
            if (!string.IsNullOrWhiteSpace(rw.EGAIS2))
                codes.Add(new PositionCode(rw.EGAIS2, Enums.PositionCodeType.EGAIS2));
            if (!string.IsNullOrWhiteSpace(rw.EGAIS3))
                codes.Add(new PositionCode(rw.EGAIS3, Enums.PositionCodeType.EGAIS3));
            if (!string.IsNullOrWhiteSpace(rw.F1))
                codes.Add(new PositionCode(rw.F1, Enums.PositionCodeType.F1));
            if (!string.IsNullOrWhiteSpace(rw.F2))
                codes.Add(new PositionCode(rw.F2, Enums.PositionCodeType.F2));
            if (!string.IsNullOrWhiteSpace(rw.F3))
                codes.Add(new PositionCode(rw.F3, Enums.PositionCodeType.F3));
            if (!string.IsNullOrWhiteSpace(rw.F4))
                codes.Add(new PositionCode(rw.F4, Enums.PositionCodeType.F4));
            if (!string.IsNullOrWhiteSpace(rw.F5))
                codes.Add(new PositionCode(rw.F5, Enums.PositionCodeType.F5));
            if (!string.IsNullOrWhiteSpace(rw.F6))
                codes.Add(new PositionCode(rw.F6, Enums.PositionCodeType.F6));
            return [.. codes];
        }
    }
}
