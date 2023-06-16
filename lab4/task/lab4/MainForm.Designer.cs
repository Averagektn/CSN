namespace lab4
{
    partial class MainForm
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
            tbSrc = new TextBox();
            tbAnswer = new TextBox();
            cbMethod = new ComboBox();
            lblAddress = new Label();
            lblAnswer = new Label();
            lblMethod = new Label();
            btnStart = new Button();
            tbPath = new TextBox();
            label1 = new Label();
            SuspendLayout();
            // 
            // tbSrc
            // 
            tbSrc.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            tbSrc.Location = new Point(12, 44);
            tbSrc.Name = "tbSrc";
            tbSrc.Size = new Size(454, 32);
            tbSrc.TabIndex = 0;
            tbSrc.Text = "http://localhost:8080/";
            // 
            // tbAnswer
            // 
            tbAnswer.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            tbAnswer.Location = new Point(-2, 170);
            tbAnswer.Multiline = true;
            tbAnswer.Name = "tbAnswer";
            tbAnswer.ScrollBars = ScrollBars.Vertical;
            tbAnswer.Size = new Size(804, 434);
            tbAnswer.TabIndex = 1;
            tbAnswer.DoubleClick += Answer_DoubleClick;
            // 
            // cbMethod
            // 
            cbMethod.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            cbMethod.FormattingEnabled = true;
            cbMethod.Items.AddRange(new object[] { "GET", "POST", "HEAD" });
            cbMethod.Location = new Point(552, 43);
            cbMethod.Name = "cbMethod";
            cbMethod.Size = new Size(197, 33);
            cbMethod.TabIndex = 2;
            // 
            // lblAddress
            // 
            lblAddress.AutoSize = true;
            lblAddress.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            lblAddress.Location = new Point(12, 9);
            lblAddress.Name = "lblAddress";
            lblAddress.Size = new Size(79, 25);
            lblAddress.TabIndex = 3;
            lblAddress.Text = "Address";
            // 
            // lblAnswer
            // 
            lblAnswer.AutoSize = true;
            lblAnswer.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            lblAnswer.Location = new Point(12, 142);
            lblAnswer.Name = "lblAnswer";
            lblAnswer.Size = new Size(74, 25);
            lblAnswer.TabIndex = 4;
            lblAnswer.Text = "Answer";
            // 
            // lblMethod
            // 
            lblMethod.AutoSize = true;
            lblMethod.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            lblMethod.Location = new Point(552, 15);
            lblMethod.Name = "lblMethod";
            lblMethod.Size = new Size(82, 25);
            lblMethod.TabIndex = 5;
            lblMethod.Text = "Method:";
            // 
            // btnStart
            // 
            btnStart.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            btnStart.Location = new Point(12, 98);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(132, 41);
            btnStart.TabIndex = 6;
            btnStart.Text = "Send";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += Start_Click;
            // 
            // tbPath
            // 
            tbPath.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            tbPath.Location = new Point(552, 107);
            tbPath.Name = "tbPath";
            tbPath.Size = new Size(197, 32);
            tbPath.TabIndex = 7;
            tbPath.Text = "index.html";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(552, 79);
            label1.Name = "label1";
            label1.Size = new Size(53, 25);
            label1.TabIndex = 8;
            label1.Text = "Path:";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 604);
            Controls.Add(label1);
            Controls.Add(tbPath);
            Controls.Add(btnStart);
            Controls.Add(lblMethod);
            Controls.Add(lblAnswer);
            Controls.Add(lblAddress);
            Controls.Add(cbMethod);
            Controls.Add(tbAnswer);
            Controls.Add(tbSrc);
            Name = "MainForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbSrc;
        private TextBox tbAnswer;
        private ComboBox cbMethod;
        private Label lblAddress;
        private Label lblAnswer;
        private Label lblMethod;
        private Button btnStart;
        private TextBox tbPath;
        private Label label1;
    }
}