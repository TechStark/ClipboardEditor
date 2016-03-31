namespace ClipboardEditor
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.comboBoxFormatType = new System.Windows.Forms.ComboBox();
            this.buttonGetData = new System.Windows.Forms.Button();
            this.buttonSetData = new System.Windows.Forms.Button();
            this.labelFormat = new System.Windows.Forms.Label();
            this.labelContent = new System.Windows.Forms.Label();
            this.textBoxContent = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonRemoveFormat = new System.Windows.Forms.Button();
            this.buttonSubmit = new System.Windows.Forms.Button();
            this.buttonUpdateFormat = new System.Windows.Forms.Button();
            this.buttonNewClone = new System.Windows.Forms.Button();
            this.textBoxPreview = new System.Windows.Forms.RichTextBox();
            this.labelPreview = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxFormatType
            // 
            this.comboBoxFormatType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxFormatType.FormattingEnabled = true;
            this.comboBoxFormatType.Location = new System.Drawing.Point(59, 9);
            this.comboBoxFormatType.Name = "comboBoxFormatType";
            this.comboBoxFormatType.Size = new System.Drawing.Size(251, 21);
            this.comboBoxFormatType.TabIndex = 0;
            // 
            // buttonGetData
            // 
            this.buttonGetData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGetData.Location = new System.Drawing.Point(316, 7);
            this.buttonGetData.Name = "buttonGetData";
            this.buttonGetData.Size = new System.Drawing.Size(75, 25);
            this.buttonGetData.TabIndex = 2;
            this.buttonGetData.Text = "Get Data";
            this.buttonGetData.UseVisualStyleBackColor = true;
            this.buttonGetData.Click += new System.EventHandler(this.buttonGetData_Click);
            // 
            // buttonSetData
            // 
            this.buttonSetData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSetData.Location = new System.Drawing.Point(397, 7);
            this.buttonSetData.Name = "buttonSetData";
            this.buttonSetData.Size = new System.Drawing.Size(75, 25);
            this.buttonSetData.TabIndex = 3;
            this.buttonSetData.Text = "Set Data";
            this.buttonSetData.UseVisualStyleBackColor = true;
            this.buttonSetData.Click += new System.EventHandler(this.buttonSetData_Click);
            // 
            // labelFormat
            // 
            this.labelFormat.AutoSize = true;
            this.labelFormat.Location = new System.Drawing.Point(12, 12);
            this.labelFormat.Name = "labelFormat";
            this.labelFormat.Size = new System.Drawing.Size(39, 13);
            this.labelFormat.TabIndex = 4;
            this.labelFormat.Text = "Format";
            // 
            // labelContent
            // 
            this.labelContent.AutoSize = true;
            this.labelContent.Location = new System.Drawing.Point(12, 31);
            this.labelContent.Name = "labelContent";
            this.labelContent.Size = new System.Drawing.Size(44, 13);
            this.labelContent.TabIndex = 5;
            this.labelContent.Text = "Content";
            // 
            // textBoxContent
            // 
            this.textBoxContent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxContent.Location = new System.Drawing.Point(14, 51);
            this.textBoxContent.Name = "textBoxContent";
            this.textBoxContent.Size = new System.Drawing.Size(458, 158);
            this.textBoxContent.TabIndex = 6;
            this.textBoxContent.Text = "";
            this.textBoxContent.WordWrap = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.buttonRemoveFormat);
            this.groupBox1.Controls.Add(this.buttonSubmit);
            this.groupBox1.Controls.Add(this.buttonUpdateFormat);
            this.groupBox1.Controls.Add(this.buttonNewClone);
            this.groupBox1.Location = new System.Drawing.Point(14, 216);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(458, 80);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Multiple Formats";
            // 
            // buttonRemoveFormat
            // 
            this.buttonRemoveFormat.Location = new System.Drawing.Point(108, 19);
            this.buttonRemoveFormat.Name = "buttonRemoveFormat";
            this.buttonRemoveFormat.Size = new System.Drawing.Size(96, 25);
            this.buttonRemoveFormat.TabIndex = 4;
            this.buttonRemoveFormat.Text = "Remove Format";
            this.buttonRemoveFormat.UseVisualStyleBackColor = true;
            this.buttonRemoveFormat.Click += new System.EventHandler(this.buttonRemoveFormat_Click);
            // 
            // buttonSubmit
            // 
            this.buttonSubmit.Location = new System.Drawing.Point(6, 50);
            this.buttonSubmit.Name = "buttonSubmit";
            this.buttonSubmit.Size = new System.Drawing.Size(96, 25);
            this.buttonSubmit.TabIndex = 3;
            this.buttonSubmit.Text = "Set Data";
            this.buttonSubmit.UseVisualStyleBackColor = true;
            this.buttonSubmit.Click += new System.EventHandler(this.buttonSubmit_Click);
            // 
            // buttonUpdateFormat
            // 
            this.buttonUpdateFormat.Location = new System.Drawing.Point(210, 19);
            this.buttonUpdateFormat.Name = "buttonUpdateFormat";
            this.buttonUpdateFormat.Size = new System.Drawing.Size(96, 25);
            this.buttonUpdateFormat.TabIndex = 2;
            this.buttonUpdateFormat.Text = "Update Format";
            this.buttonUpdateFormat.UseVisualStyleBackColor = true;
            this.buttonUpdateFormat.Click += new System.EventHandler(this.buttonUpdateFormat_Click);
            // 
            // buttonNewClone
            // 
            this.buttonNewClone.Location = new System.Drawing.Point(6, 19);
            this.buttonNewClone.Name = "buttonNewClone";
            this.buttonNewClone.Size = new System.Drawing.Size(96, 25);
            this.buttonNewClone.TabIndex = 1;
            this.buttonNewClone.Text = "Get Data";
            this.buttonNewClone.UseVisualStyleBackColor = true;
            this.buttonNewClone.Click += new System.EventHandler(this.buttonNewClone_Click);
            // 
            // textBoxPreview
            // 
            this.textBoxPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPreview.Location = new System.Drawing.Point(12, 315);
            this.textBoxPreview.Name = "textBoxPreview";
            this.textBoxPreview.Size = new System.Drawing.Size(460, 280);
            this.textBoxPreview.TabIndex = 8;
            this.textBoxPreview.Text = "";
            this.textBoxPreview.WordWrap = false;
            // 
            // labelPreview
            // 
            this.labelPreview.AutoSize = true;
            this.labelPreview.Location = new System.Drawing.Point(9, 299);
            this.labelPreview.Name = "labelPreview";
            this.labelPreview.Size = new System.Drawing.Size(45, 13);
            this.labelPreview.TabIndex = 9;
            this.labelPreview.Text = "Preview";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 608);
            this.Controls.Add(this.labelPreview);
            this.Controls.Add(this.textBoxPreview);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBoxContent);
            this.Controls.Add(this.labelContent);
            this.Controls.Add(this.labelFormat);
            this.Controls.Add(this.buttonSetData);
            this.Controls.Add(this.buttonGetData);
            this.Controls.Add(this.comboBoxFormatType);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(455, 517);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Clipboard Editor";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxFormatType;
        private System.Windows.Forms.Button buttonGetData;
        private System.Windows.Forms.Button buttonSetData;
        private System.Windows.Forms.Label labelFormat;
        private System.Windows.Forms.Label labelContent;
        private System.Windows.Forms.RichTextBox textBoxContent;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonRemoveFormat;
        private System.Windows.Forms.Button buttonSubmit;
        private System.Windows.Forms.Button buttonUpdateFormat;
        private System.Windows.Forms.Button buttonNewClone;
        private System.Windows.Forms.RichTextBox textBoxPreview;
        private System.Windows.Forms.Label labelPreview;
    }
}

