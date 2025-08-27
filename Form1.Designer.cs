namespace CEIS400_ECS
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
            textBox = new TextBox();
            equipmentIDTextBoxLabel = new Label();
            scanButton = new Button();
            enterButton = new Button();
            availableListBox = new ListBox();
            navigationMenuPanel = new Panel();
            generateReportsButton = new Button();
            trackInventoryButton = new Button();
            navigationPanelLabel = new Label();
            updateInventoryButton = new Button();
            checkInButton = new Button();
            checkOutButton = new Button();
            availableListBoxLabel = new Label();
            navigationMenuPanel.SuspendLayout();
            SuspendLayout();
            // 
            // textBox
            // 
            textBox.Location = new Point(258, 61);
            textBox.Name = "textBox";
            textBox.Size = new Size(273, 23);
            textBox.TabIndex = 0;
            // 
            // equipmentIDTextBoxLabel
            // 
            equipmentIDTextBoxLabel.AutoSize = true;
            equipmentIDTextBoxLabel.Location = new Point(170, 65);
            equipmentIDTextBoxLabel.Name = "equipmentIDTextBoxLabel";
            equipmentIDTextBoxLabel.Size = new Size(82, 15);
            equipmentIDTextBoxLabel.TabIndex = 1;
            equipmentIDTextBoxLabel.Text = "Equipment ID:";
            equipmentIDTextBoxLabel.Click += label1_Click;
            // 
            // scanButton
            // 
            scanButton.Location = new Point(545, 61);
            scanButton.Name = "scanButton";
            scanButton.Size = new Size(75, 23);
            scanButton.TabIndex = 1;
            scanButton.Text = "Scan";
            scanButton.UseVisualStyleBackColor = true;
            scanButton.Click += button1_Click;
            // 
            // enterButton
            // 
            enterButton.Location = new Point(545, 90);
            enterButton.Name = "enterButton";
            enterButton.Size = new Size(75, 23);
            enterButton.TabIndex = 2;
            enterButton.Text = "Enter";
            enterButton.UseVisualStyleBackColor = true;
            // 
            // availableListBox
            // 
            availableListBox.AccessibleDescription = "";
            availableListBox.FormattingEnabled = true;
            availableListBox.ItemHeight = 15;
            availableListBox.Location = new Point(170, 128);
            availableListBox.Name = "availableListBox";
            availableListBox.Size = new Size(450, 289);
            availableListBox.TabIndex = 3;
            availableListBox.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // navigationMenuPanel
            // 
            navigationMenuPanel.BackColor = SystemColors.ControlLight;
            navigationMenuPanel.BorderStyle = BorderStyle.FixedSingle;
            navigationMenuPanel.Controls.Add(generateReportsButton);
            navigationMenuPanel.Controls.Add(trackInventoryButton);
            navigationMenuPanel.Controls.Add(navigationPanelLabel);
            navigationMenuPanel.Controls.Add(updateInventoryButton);
            navigationMenuPanel.Controls.Add(checkInButton);
            navigationMenuPanel.Controls.Add(checkOutButton);
            navigationMenuPanel.Location = new Point(26, 25);
            navigationMenuPanel.Name = "navigationMenuPanel";
            navigationMenuPanel.Size = new Size(117, 392);
            navigationMenuPanel.TabIndex = 5;
            navigationMenuPanel.Paint += panel1_Paint;
            // 
            // generateReportsButton
            // 
            generateReportsButton.Location = new Point(3, 184);
            generateReportsButton.Name = "generateReportsButton";
            generateReportsButton.Size = new Size(109, 35);
            generateReportsButton.TabIndex = 4;
            generateReportsButton.Text = "Generate Reports";
            generateReportsButton.UseVisualStyleBackColor = true;
            generateReportsButton.Click += generateReportsButton_Click;
            // 
            // trackInventoryButton
            // 
            trackInventoryButton.Location = new Point(3, 143);
            trackInventoryButton.Name = "trackInventoryButton";
            trackInventoryButton.Size = new Size(109, 35);
            trackInventoryButton.TabIndex = 3;
            trackInventoryButton.Text = "Track Inventory";
            trackInventoryButton.UseVisualStyleBackColor = true;
            trackInventoryButton.Click += trackInventoryButton_Click;
            // 
            // navigationPanelLabel
            // 
            navigationPanelLabel.AutoSize = true;
            navigationPanelLabel.Location = new Point(0, 0);
            navigationPanelLabel.Name = "navigationPanelLabel";
            navigationPanelLabel.Size = new Size(65, 15);
            navigationPanelLabel.TabIndex = 7;
            navigationPanelLabel.Text = "Navigation";
            // 
            // updateInventoryButton
            // 
            updateInventoryButton.Location = new Point(3, 102);
            updateInventoryButton.Name = "updateInventoryButton";
            updateInventoryButton.Size = new Size(109, 35);
            updateInventoryButton.TabIndex = 2;
            updateInventoryButton.Text = "Update Inventory";
            updateInventoryButton.UseVisualStyleBackColor = true;
            updateInventoryButton.Click += updateInventoryButton_Click;
            // 
            // checkInButton
            // 
            checkInButton.Location = new Point(3, 61);
            checkInButton.Name = "checkInButton";
            checkInButton.Size = new Size(109, 35);
            checkInButton.TabIndex = 1;
            checkInButton.Text = "Check In";
            checkInButton.UseVisualStyleBackColor = true;
            checkInButton.Click += checkInButton_Click;
            // 
            // checkOutButton
            // 
            checkOutButton.Location = new Point(3, 20);
            checkOutButton.Name = "checkOutButton";
            checkOutButton.Size = new Size(109, 35);
            checkOutButton.TabIndex = 0;
            checkOutButton.Text = "Check Out";
            checkOutButton.UseVisualStyleBackColor = true;
            checkOutButton.Click += checkOutButton_Click;
            // 
            // availableListBoxLabel
            // 
            availableListBoxLabel.AutoSize = true;
            availableListBoxLabel.Location = new Point(170, 110);
            availableListBoxLabel.Name = "availableListBoxLabel";
            availableListBoxLabel.Size = new Size(58, 15);
            availableListBoxLabel.TabIndex = 6;
            availableListBoxLabel.Text = "Available:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(647, 441);
            Controls.Add(availableListBoxLabel);
            Controls.Add(navigationMenuPanel);
            Controls.Add(availableListBox);
            Controls.Add(enterButton);
            Controls.Add(scanButton);
            Controls.Add(equipmentIDTextBoxLabel);
            Controls.Add(textBox);
            Name = "Form1";
            Text = "Equipment Checkout System";
            Load += Form1_Load;
            navigationMenuPanel.ResumeLayout(false);
            navigationMenuPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox;
        private Label equipmentIDTextBoxLabel;
        private Button scanButton;
        private Button enterButton;
        private ListBox availableListBox;
        private Panel navigationMenuPanel;
        private Label availableListBoxLabel;
        private Button checkInButton;
        private Button checkOutButton;
        private Button updateInventoryButton;
        private Label navigationPanelLabel;
        private Button trackInventoryButton;
        private Button generateReportsButton;
    }
}
