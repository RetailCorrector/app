namespace RetailCorrector.Cashier
{
    internal static class Program
    {
        public static Forms.Main Form { get; private set; }
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Form = new Forms.Main();
            Application.Run(Form);
        }
    }
}