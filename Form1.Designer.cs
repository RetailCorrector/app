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
            button1 = new Button();
            label1 = new Label();
            label2 = new Label();
            comSelector = new ComboBox();
            pathInput = new TextBox();
            button2 = new Button();
            progress = new ProgressBar();
            openFileDialog = new OpenFileDialog();
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
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(progress);
            Controls.Add(button2);
            Controls.Add(pathInput);
            Controls.Add(comSelector);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
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
        private OpenFileDialog openFileDialog;
    }
}
