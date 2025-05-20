namespace RetailCorrector.Cashier
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            button1 = new Button();
            label1 = new Label();
            label2 = new Label();
            comSelector = new ComboBox();
            pathInput = new TextBox();
            button2 = new Button();
            progress = new ProgressBar();
            folderDialog = new FolderBrowserDialog();
            button3 = new Button();
            pathDriver = new TextBox();
            label3 = new Label();
            openFiscalDriver = new OpenFileDialog();
            toolTip1 = new ToolTip(components);
            label4 = new Label();
            bufferMin = new NumericUpDown();
            bufferMax = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)bufferMin).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bufferMax).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(713, 415);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 0;
            button1.Text = "Запуск";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Start;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(66, 15);
            label1.TabIndex = 1;
            label1.Text = "COM-порт";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 44);
            label2.Name = "label2";
            label2.Size = new Size(136, 15);
            label2.TabIndex = 2;
            label2.Text = "Директория с данными";
            // 
            // comSelector
            // 
            comSelector.FormattingEnabled = true;
            comSelector.Location = new Point(84, 12);
            comSelector.Name = "comSelector";
            comSelector.Size = new Size(150, 23);
            comSelector.TabIndex = 3;
            // 
            // pathInput
            // 
            pathInput.Location = new Point(154, 41);
            pathInput.Name = "pathInput";
            pathInput.Size = new Size(200, 23);
            pathInput.TabIndex = 4;
            // 
            // button2
            // 
            button2.Location = new Point(360, 41);
            button2.Name = "button2";
            button2.Size = new Size(24, 23);
            button2.TabIndex = 5;
            button2.Text = "...";
            button2.UseVisualStyleBackColor = true;
            button2.Click += ShowOpenDialog;
            // 
            // progress
            // 
            progress.Location = new Point(12, 415);
            progress.Name = "progress";
            progress.Size = new Size(695, 23);
            progress.TabIndex = 6;
            // 
            // folderDialog
            // 
            folderDialog.AddToRecent = false;
            folderDialog.RootFolder = Environment.SpecialFolder.MyDocuments;
            folderDialog.ShowHiddenFiles = true;
            // 
            // button3
            // 
            button3.Location = new Point(320, 70);
            button3.Name = "button3";
            button3.Size = new Size(24, 23);
            button3.TabIndex = 9;
            button3.Text = "...";
            button3.UseVisualStyleBackColor = true;
            button3.Click += ShowOpenDllDialog;
            // 
            // pathDriver
            // 
            pathDriver.Location = new Point(114, 70);
            pathDriver.Name = "pathDriver";
            pathDriver.Size = new Size(200, 23);
            pathDriver.TabIndex = 8;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 73);
            label3.Name = "label3";
            label3.Size = new Size(96, 15);
            label3.TabIndex = 7;
            label3.Text = "Путь к драйверу";
            // 
            // openFiscalDriver
            // 
            openFiscalDriver.AddToRecent = false;
            openFiscalDriver.Filter = "DLL-файл|*.dll";
            openFiscalDriver.ShowHiddenFiles = true;
            // 
            // toolTip1
            // 
            toolTip1.IsBalloon = true;
            toolTip1.ShowAlways = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 101);
            label4.Name = "label4";
            label4.Size = new Size(150, 15);
            label4.TabIndex = 10;
            label4.Text = "Пределы размера буфера";
            // 
            // bufferMin
            // 
            bufferMin.Location = new Point(168, 99);
            bufferMin.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            bufferMin.Name = "bufferMin";
            bufferMin.Size = new Size(120, 23);
            bufferMin.TabIndex = 11;
            bufferMin.Value = new decimal(new int[] { 15, 0, 0, 0 });
            // 
            // bufferMax
            // 
            bufferMax.Location = new Point(294, 99);
            bufferMax.Name = "bufferMax";
            bufferMax.Size = new Size(120, 23);
            bufferMax.TabIndex = 12;
            bufferMax.Value = new decimal(new int[] { 30, 0, 0, 0 });
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(bufferMax);
            Controls.Add(bufferMin);
            Controls.Add(label4);
            Controls.Add(button3);
            Controls.Add(pathDriver);
            Controls.Add(label3);
            Controls.Add(progress);
            Controls.Add(button2);
            Controls.Add(pathInput);
            Controls.Add(comSelector);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)bufferMin).EndInit();
            ((System.ComponentModel.ISupportInitialize)bufferMax).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label label1;
        private Label label2;
        private ComboBox comSelector;
        private TextBox pathInput;
        private Button button2;
        private ProgressBar progress;
        private FolderBrowserDialog folderDialog;
        private Button button3;
        private TextBox pathDriver;
        private Label label3;
        private OpenFileDialog openFiscalDriver;
        private ToolTip toolTip1;
        private Label label4;
        private NumericUpDown bufferMin;
        private NumericUpDown bufferMax;
    }
}
