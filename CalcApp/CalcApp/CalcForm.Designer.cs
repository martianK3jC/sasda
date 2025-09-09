namespace CalcApp
{
    partial class CalcForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalcForm));
            this.textDisplay = new System.Windows.Forms.TextBox();
            this.decimalBtn = new System.Windows.Forms.Button();
            this.btn0 = new System.Windows.Forms.Button();
            this.rmBtn = new System.Windows.Forms.Button();
            this.equalBtn = new System.Windows.Forms.Button();
            this.btn3 = new System.Windows.Forms.Button();
            this.btn2 = new System.Windows.Forms.Button();
            this.btn1 = new System.Windows.Forms.Button();
            this.btn6 = new System.Windows.Forms.Button();
            this.btn5 = new System.Windows.Forms.Button();
            this.btn4 = new System.Windows.Forms.Button();
            this.powerBtn = new System.Windows.Forms.Button();
            this.clearBtn = new System.Windows.Forms.Button();
            this.signBtn = new System.Windows.Forms.Button();
            this.percentBtn = new System.Windows.Forms.Button();
            this.sqrtBtn = new System.Windows.Forms.Button();
            this.btn9 = new System.Windows.Forms.Button();
            this.btn8 = new System.Windows.Forms.Button();
            this.btn7 = new System.Windows.Forms.Button();
            this.minusBtn = new System.Windows.Forms.Button();
            this.divideBtn = new System.Windows.Forms.Button();
            this.multiplyBtn = new System.Windows.Forms.Button();
            this.addBtn = new System.Windows.Forms.Button();
            this.heartBtn = new System.Windows.Forms.Button();
            this.textExpression = new System.Windows.Forms.TextBox();
            this.EraseWholeNumBtn = new System.Windows.Forms.Button();
            this.dlModeBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textDisplay
            // 
            this.textDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textDisplay.ForeColor = System.Drawing.SystemColors.Menu;
            this.textDisplay.Location = new System.Drawing.Point(25, 80);
            this.textDisplay.Name = "textDisplay";
            this.textDisplay.ReadOnly = true;
            this.textDisplay.Size = new System.Drawing.Size(402, 116);
            this.textDisplay.TabIndex = 0;
            this.textDisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // decimalBtn
            // 
            this.decimalBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.decimalBtn.Location = new System.Drawing.Point(229, 691);
            this.decimalBtn.Name = "decimalBtn";
            this.decimalBtn.Size = new System.Drawing.Size(96, 74);
            this.decimalBtn.TabIndex = 1;
            this.decimalBtn.Text = ".";
            this.decimalBtn.UseVisualStyleBackColor = true;
            this.decimalBtn.Click += new System.EventHandler(this.Decimal_Click);
            // 
            // btn0
            // 
            this.btn0.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn0.Location = new System.Drawing.Point(127, 691);
            this.btn0.Name = "btn0";
            this.btn0.Size = new System.Drawing.Size(96, 74);
            this.btn0.TabIndex = 2;
            this.btn0.Text = "0";
            this.btn0.UseVisualStyleBackColor = true;
            this.btn0.Click += new System.EventHandler(this.Number_Click);
            // 
            // rmBtn
            // 
            this.rmBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rmBtn.Location = new System.Drawing.Point(331, 291);
            this.rmBtn.Name = "rmBtn";
            this.rmBtn.Size = new System.Drawing.Size(96, 74);
            this.rmBtn.TabIndex = 3;
            this.rmBtn.Text = "⌫";
            this.rmBtn.UseVisualStyleBackColor = true;
            this.rmBtn.Click += new System.EventHandler(this.rmBtn_Click);
            // 
            // equalBtn
            // 
            this.equalBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.equalBtn.Location = new System.Drawing.Point(331, 611);
            this.equalBtn.Name = "equalBtn";
            this.equalBtn.Size = new System.Drawing.Size(96, 154);
            this.equalBtn.TabIndex = 4;
            this.equalBtn.Text = "=";
            this.equalBtn.UseVisualStyleBackColor = true;
            this.equalBtn.Click += new System.EventHandler(this.btnEquals_Click);
            // 
            // btn3
            // 
            this.btn3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn3.Location = new System.Drawing.Point(229, 611);
            this.btn3.Name = "btn3";
            this.btn3.Size = new System.Drawing.Size(96, 74);
            this.btn3.TabIndex = 7;
            this.btn3.Text = "3";
            this.btn3.UseVisualStyleBackColor = true;
            this.btn3.Click += new System.EventHandler(this.Number_Click);
            // 
            // btn2
            // 
            this.btn2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn2.Location = new System.Drawing.Point(127, 611);
            this.btn2.Name = "btn2";
            this.btn2.Size = new System.Drawing.Size(96, 74);
            this.btn2.TabIndex = 6;
            this.btn2.Text = "2";
            this.btn2.UseVisualStyleBackColor = true;
            this.btn2.Click += new System.EventHandler(this.Number_Click);
            // 
            // btn1
            // 
            this.btn1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn1.Location = new System.Drawing.Point(25, 611);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(96, 74);
            this.btn1.TabIndex = 5;
            this.btn1.Text = "1";
            this.btn1.UseVisualStyleBackColor = true;
            this.btn1.Click += new System.EventHandler(this.Number_Click);
            // 
            // btn6
            // 
            this.btn6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn6.Location = new System.Drawing.Point(229, 531);
            this.btn6.Name = "btn6";
            this.btn6.Size = new System.Drawing.Size(96, 74);
            this.btn6.TabIndex = 10;
            this.btn6.Text = "6";
            this.btn6.UseVisualStyleBackColor = true;
            this.btn6.Click += new System.EventHandler(this.Number_Click);
            // 
            // btn5
            // 
            this.btn5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn5.Location = new System.Drawing.Point(127, 531);
            this.btn5.Name = "btn5";
            this.btn5.Size = new System.Drawing.Size(96, 74);
            this.btn5.TabIndex = 9;
            this.btn5.Text = "5";
            this.btn5.UseVisualStyleBackColor = true;
            this.btn5.Click += new System.EventHandler(this.Number_Click);
            // 
            // btn4
            // 
            this.btn4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn4.Location = new System.Drawing.Point(25, 531);
            this.btn4.Name = "btn4";
            this.btn4.Size = new System.Drawing.Size(96, 74);
            this.btn4.TabIndex = 8;
            this.btn4.Text = "4";
            this.btn4.UseVisualStyleBackColor = true;
            this.btn4.Click += new System.EventHandler(this.Number_Click);
            // 
            // powerBtn
            // 
            this.powerBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.powerBtn.Location = new System.Drawing.Point(229, 291);
            this.powerBtn.Name = "powerBtn";
            this.powerBtn.Size = new System.Drawing.Size(96, 74);
            this.powerBtn.TabIndex = 12;
            this.powerBtn.Text = "^";
            this.powerBtn.UseVisualStyleBackColor = true;
            this.powerBtn.Click += new System.EventHandler(this.Operator_Click);
            // 
            // clearBtn
            // 
            this.clearBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clearBtn.Location = new System.Drawing.Point(127, 291);
            this.clearBtn.Name = "clearBtn";
            this.clearBtn.Size = new System.Drawing.Size(96, 74);
            this.clearBtn.TabIndex = 11;
            this.clearBtn.Text = "C";
            this.clearBtn.UseVisualStyleBackColor = true;
            this.clearBtn.Click += new System.EventHandler(this.clearBtn_Click);
            // 
            // signBtn
            // 
            this.signBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.signBtn.Location = new System.Drawing.Point(25, 691);
            this.signBtn.Name = "signBtn";
            this.signBtn.Size = new System.Drawing.Size(96, 74);
            this.signBtn.TabIndex = 16;
            this.signBtn.Text = "±";
            this.signBtn.UseVisualStyleBackColor = true;
            this.signBtn.Click += new System.EventHandler(this.signBtn_Click);
            // 
            // percentBtn
            // 
            this.percentBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.percentBtn.Location = new System.Drawing.Point(127, 371);
            this.percentBtn.Name = "percentBtn";
            this.percentBtn.Size = new System.Drawing.Size(96, 74);
            this.percentBtn.TabIndex = 15;
            this.percentBtn.Text = "%";
            this.percentBtn.UseVisualStyleBackColor = true;
            this.percentBtn.Click += new System.EventHandler(this.Percent_Click);
            // 
            // sqrtBtn
            // 
            this.sqrtBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sqrtBtn.Location = new System.Drawing.Point(25, 371);
            this.sqrtBtn.Name = "sqrtBtn";
            this.sqrtBtn.Size = new System.Drawing.Size(96, 74);
            this.sqrtBtn.TabIndex = 14;
            this.sqrtBtn.Text = "√";
            this.sqrtBtn.UseVisualStyleBackColor = true;
            this.sqrtBtn.Click += new System.EventHandler(this.btnSqrt_Click);
            // 
            // btn9
            // 
            this.btn9.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn9.Location = new System.Drawing.Point(229, 451);
            this.btn9.Name = "btn9";
            this.btn9.Size = new System.Drawing.Size(96, 74);
            this.btn9.TabIndex = 19;
            this.btn9.Text = "9";
            this.btn9.UseVisualStyleBackColor = true;
            this.btn9.Click += new System.EventHandler(this.Number_Click);
            // 
            // btn8
            // 
            this.btn8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn8.Location = new System.Drawing.Point(127, 451);
            this.btn8.Name = "btn8";
            this.btn8.Size = new System.Drawing.Size(96, 74);
            this.btn8.TabIndex = 18;
            this.btn8.Text = "8";
            this.btn8.UseVisualStyleBackColor = true;
            this.btn8.Click += new System.EventHandler(this.Number_Click);
            // 
            // btn7
            // 
            this.btn7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn7.Location = new System.Drawing.Point(25, 451);
            this.btn7.Name = "btn7";
            this.btn7.Size = new System.Drawing.Size(96, 74);
            this.btn7.TabIndex = 17;
            this.btn7.Text = "7";
            this.btn7.UseVisualStyleBackColor = true;
            this.btn7.Click += new System.EventHandler(this.Number_Click);
            // 
            // minusBtn
            // 
            this.minusBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.minusBtn.Location = new System.Drawing.Point(331, 451);
            this.minusBtn.Name = "minusBtn";
            this.minusBtn.Size = new System.Drawing.Size(96, 74);
            this.minusBtn.TabIndex = 23;
            this.minusBtn.Text = "-";
            this.minusBtn.UseVisualStyleBackColor = true;
            this.minusBtn.Click += new System.EventHandler(this.Operator_Click);
            // 
            // divideBtn
            // 
            this.divideBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.divideBtn.Location = new System.Drawing.Point(229, 371);
            this.divideBtn.Name = "divideBtn";
            this.divideBtn.Size = new System.Drawing.Size(96, 74);
            this.divideBtn.TabIndex = 22;
            this.divideBtn.Text = "÷";
            this.divideBtn.UseVisualStyleBackColor = true;
            this.divideBtn.Click += new System.EventHandler(this.Operator_Click);
            // 
            // multiplyBtn
            // 
            this.multiplyBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.multiplyBtn.Location = new System.Drawing.Point(331, 371);
            this.multiplyBtn.Name = "multiplyBtn";
            this.multiplyBtn.Size = new System.Drawing.Size(96, 74);
            this.multiplyBtn.TabIndex = 21;
            this.multiplyBtn.Text = "x";
            this.multiplyBtn.UseVisualStyleBackColor = true;
            this.multiplyBtn.Click += new System.EventHandler(this.Operator_Click);
            // 
            // addBtn
            // 
            this.addBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addBtn.Location = new System.Drawing.Point(331, 531);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(96, 74);
            this.addBtn.TabIndex = 20;
            this.addBtn.Text = "+";
            this.addBtn.UseVisualStyleBackColor = true;
            this.addBtn.Click += new System.EventHandler(this.Operator_Click);
            // 
            // heartBtn
            // 
            this.heartBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.heartBtn.Location = new System.Drawing.Point(331, 211);
            this.heartBtn.Name = "heartBtn";
            this.heartBtn.Size = new System.Drawing.Size(96, 74);
            this.heartBtn.TabIndex = 25;
            this.heartBtn.Text = "𓆩❤︎𓆪";
            this.heartBtn.UseVisualStyleBackColor = true;
            this.heartBtn.Click += new System.EventHandler(this.heartBtn_Click);
            // 
            // textExpression
            // 
            this.textExpression.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textExpression.ForeColor = System.Drawing.Color.Transparent;
            this.textExpression.Location = new System.Drawing.Point(25, 30);
            this.textExpression.Name = "textExpression";
            this.textExpression.ReadOnly = true;
            this.textExpression.Size = new System.Drawing.Size(402, 48);
            this.textExpression.TabIndex = 26;
            this.textExpression.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // EraseWholeNumBtn
            // 
            this.EraseWholeNumBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EraseWholeNumBtn.Location = new System.Drawing.Point(25, 291);
            this.EraseWholeNumBtn.Name = "EraseWholeNumBtn";
            this.EraseWholeNumBtn.Size = new System.Drawing.Size(96, 74);
            this.EraseWholeNumBtn.TabIndex = 27;
            this.EraseWholeNumBtn.Text = "CE";
            this.EraseWholeNumBtn.UseVisualStyleBackColor = true;
            this.EraseWholeNumBtn.Click += new System.EventHandler(this.EraseWholeNumBtn_Click);
            // 
            // dlModeBtn
            // 
            this.dlModeBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dlModeBtn.ForeColor = System.Drawing.Color.DarkGreen;
            this.dlModeBtn.Location = new System.Drawing.Point(25, 211);
            this.dlModeBtn.Name = "dlModeBtn";
            this.dlModeBtn.Size = new System.Drawing.Size(300, 74);
            this.dlModeBtn.TabIndex = 28;
            this.dlModeBtn.Text = "⋆ﾟ☁︎⏾⋆☁︎｡⋆";
            this.dlModeBtn.UseVisualStyleBackColor = true;
            this.dlModeBtn.Click += new System.EventHandler(this.dlModeBtn_Click_1);
            // 
            // CalcForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(452, 786);
            this.Controls.Add(this.dlModeBtn);
            this.Controls.Add(this.EraseWholeNumBtn);
            this.Controls.Add(this.textExpression);
            this.Controls.Add(this.heartBtn);
            this.Controls.Add(this.minusBtn);
            this.Controls.Add(this.divideBtn);
            this.Controls.Add(this.multiplyBtn);
            this.Controls.Add(this.addBtn);
            this.Controls.Add(this.btn9);
            this.Controls.Add(this.btn8);
            this.Controls.Add(this.btn7);
            this.Controls.Add(this.signBtn);
            this.Controls.Add(this.percentBtn);
            this.Controls.Add(this.sqrtBtn);
            this.Controls.Add(this.powerBtn);
            this.Controls.Add(this.clearBtn);
            this.Controls.Add(this.btn6);
            this.Controls.Add(this.btn5);
            this.Controls.Add(this.btn4);
            this.Controls.Add(this.btn3);
            this.Controls.Add(this.btn2);
            this.Controls.Add(this.btn1);
            this.Controls.Add(this.equalBtn);
            this.Controls.Add(this.rmBtn);
            this.Controls.Add(this.btn0);
            this.Controls.Add(this.decimalBtn);
            this.Controls.Add(this.textDisplay);
            this.Name = "CalcForm";
            this.Text = "𓆩❤︎𓆪143 Calculator";
            this.Load += new System.EventHandler(this.CalcForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textDisplay;
        private System.Windows.Forms.Button decimalBtn;
        private System.Windows.Forms.Button btn0;
        private System.Windows.Forms.Button rmBtn;
        private System.Windows.Forms.Button equalBtn;
        private System.Windows.Forms.Button btn3;
        private System.Windows.Forms.Button btn2;
        private System.Windows.Forms.Button btn1;
        private System.Windows.Forms.Button btn6;
        private System.Windows.Forms.Button btn5;
        private System.Windows.Forms.Button btn4;
        private System.Windows.Forms.Button powerBtn;
        private System.Windows.Forms.Button clearBtn;
        private System.Windows.Forms.Button signBtn;
        private System.Windows.Forms.Button percentBtn;
        private System.Windows.Forms.Button sqrtBtn;
        private System.Windows.Forms.Button btn9;
        private System.Windows.Forms.Button btn8;
        private System.Windows.Forms.Button btn7;
        private System.Windows.Forms.Button minusBtn;
        private System.Windows.Forms.Button divideBtn;
        private System.Windows.Forms.Button multiplyBtn;
        private System.Windows.Forms.Button addBtn;
        private System.Windows.Forms.Button heartBtn;
        private System.Windows.Forms.TextBox textExpression;
        private System.Windows.Forms.Button EraseWholeNumBtn;
        private System.Windows.Forms.Button dlModeBtn;
    }
}

