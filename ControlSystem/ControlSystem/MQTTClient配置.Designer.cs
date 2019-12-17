namespace ControlSystem
{
    partial class MQTTClient配置
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbx_ip = new System.Windows.Forms.TextBox();
            this.tbx_userName = new System.Windows.Forms.TextBox();
            this.btn_connect = new System.Windows.Forms.Button();
            this.listBoxControl1 = new DevExpress.XtraEditors.ListBoxControl();
            this.listBoxControl2 = new DevExpress.XtraEditors.ListBoxControl();
            this.btn_disconnect = new System.Windows.Forms.Button();
            this.rtb_msg = new System.Windows.Forms.RichTextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btn_del_write = new DevExpress.XtraEditors.SimpleButton();
            this.btn_del_read = new DevExpress.XtraEditors.SimpleButton();
            this.btn_add_write = new DevExpress.XtraEditors.SimpleButton();
            this.btn_add_read = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbx_password = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxControl2)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(800, 450);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_del_write);
            this.groupBox2.Controls.Add(this.btn_del_read);
            this.groupBox2.Controls.Add(this.btn_add_write);
            this.groupBox2.Controls.Add(this.btn_add_read);
            this.groupBox2.Controls.Add(this.groupBox6);
            this.groupBox2.Controls.Add(this.groupBox5);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox2.Location = new System.Drawing.Point(3, 17);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(547, 430);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "配置相关";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.rtb_msg);
            this.groupBox3.Location = new System.Drawing.Point(556, 20);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(244, 424);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "运行日志";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btn_disconnect);
            this.groupBox4.Controls.Add(this.btn_connect);
            this.groupBox4.Controls.Add(this.tbx_password);
            this.groupBox4.Controls.Add(this.tbx_userName);
            this.groupBox4.Controls.Add(this.tbx_ip);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox4.Location = new System.Drawing.Point(3, 17);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(541, 147);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "连接配置";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.listBoxControl1);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox5.Location = new System.Drawing.Point(3, 164);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(213, 263);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "订阅主题";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.simpleButton1);
            this.groupBox6.Controls.Add(this.richTextBox1);
            this.groupBox6.Controls.Add(this.listBoxControl2);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox6.Location = new System.Drawing.Point(332, 164);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(212, 263);
            this.groupBox6.TabIndex = 2;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "发布主题";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "MqttServerIP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(66, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "用户名";
            // 
            // tbx_ip
            // 
            this.tbx_ip.Location = new System.Drawing.Point(143, 31);
            this.tbx_ip.Name = "tbx_ip";
            this.tbx_ip.Size = new System.Drawing.Size(210, 21);
            this.tbx_ip.TabIndex = 1;
            // 
            // tbx_userName
            // 
            this.tbx_userName.Location = new System.Drawing.Point(143, 72);
            this.tbx_userName.Name = "tbx_userName";
            this.tbx_userName.Size = new System.Drawing.Size(210, 21);
            this.tbx_userName.TabIndex = 1;
            // 
            // btn_connect
            // 
            this.btn_connect.Location = new System.Drawing.Point(439, 29);
            this.btn_connect.Name = "btn_connect";
            this.btn_connect.Size = new System.Drawing.Size(75, 23);
            this.btn_connect.TabIndex = 2;
            this.btn_connect.Text = "连接";
            this.btn_connect.UseVisualStyleBackColor = true;
            this.btn_connect.Click += new System.EventHandler(this.btn_connect_Click);
            // 
            // listBoxControl1
            // 
            this.listBoxControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.listBoxControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.listBoxControl1.Location = new System.Drawing.Point(3, 17);
            this.listBoxControl1.Name = "listBoxControl1";
            this.listBoxControl1.Size = new System.Drawing.Size(204, 243);
            this.listBoxControl1.TabIndex = 0;
            // 
            // listBoxControl2
            // 
            this.listBoxControl2.Cursor = System.Windows.Forms.Cursors.Default;
            this.listBoxControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.listBoxControl2.Location = new System.Drawing.Point(3, 17);
            this.listBoxControl2.Name = "listBoxControl2";
            this.listBoxControl2.Size = new System.Drawing.Size(206, 96);
            this.listBoxControl2.TabIndex = 0;
            // 
            // btn_disconnect
            // 
            this.btn_disconnect.Location = new System.Drawing.Point(439, 61);
            this.btn_disconnect.Name = "btn_disconnect";
            this.btn_disconnect.Size = new System.Drawing.Size(75, 23);
            this.btn_disconnect.TabIndex = 3;
            this.btn_disconnect.Text = "断开";
            this.btn_disconnect.UseVisualStyleBackColor = true;
            // 
            // rtb_msg
            // 
            this.rtb_msg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_msg.Location = new System.Drawing.Point(3, 17);
            this.rtb_msg.Name = "rtb_msg";
            this.rtb_msg.Size = new System.Drawing.Size(238, 404);
            this.rtb_msg.TabIndex = 0;
            this.rtb_msg.Text = "";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.richTextBox1.Location = new System.Drawing.Point(3, 179);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(206, 81);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // btn_del_write
            // 
            this.btn_del_write.Image = global::ControlSystem.Properties.Resources.del;
            this.btn_del_write.Location = new System.Drawing.Point(222, 283);
            this.btn_del_write.Name = "btn_del_write";
            this.btn_del_write.Size = new System.Drawing.Size(104, 23);
            this.btn_del_write.TabIndex = 3;
            this.btn_del_write.Text = "删除发布";
            // 
            // btn_del_read
            // 
            this.btn_del_read.Image = global::ControlSystem.Properties.Resources.del;
            this.btn_del_read.Location = new System.Drawing.Point(222, 254);
            this.btn_del_read.Name = "btn_del_read";
            this.btn_del_read.Size = new System.Drawing.Size(104, 23);
            this.btn_del_read.TabIndex = 3;
            this.btn_del_read.Text = "删除订阅";
            // 
            // btn_add_write
            // 
            this.btn_add_write.Image = global::ControlSystem.Properties.Resources.add;
            this.btn_add_write.Location = new System.Drawing.Point(222, 225);
            this.btn_add_write.Name = "btn_add_write";
            this.btn_add_write.Size = new System.Drawing.Size(104, 23);
            this.btn_add_write.TabIndex = 3;
            this.btn_add_write.Text = "添加发布";
            // 
            // btn_add_read
            // 
            this.btn_add_read.Image = global::ControlSystem.Properties.Resources.add;
            this.btn_add_read.Location = new System.Drawing.Point(222, 196);
            this.btn_add_read.Name = "btn_add_read";
            this.btn_add_read.Size = new System.Drawing.Size(104, 23);
            this.btn_add_read.TabIndex = 3;
            this.btn_add_read.Text = "添加订阅";
            // 
            // simpleButton1
            // 
            this.simpleButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton1.Image = global::ControlSystem.Properties.Resources.send;
            this.simpleButton1.Location = new System.Drawing.Point(131, 150);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 23);
            this.simpleButton1.TabIndex = 2;
            this.simpleButton1.Text = "发送";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(66, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "连接密码";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(66, 147);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "连接密码";
            // 
            // tbx_password
            // 
            this.tbx_password.Location = new System.Drawing.Point(143, 110);
            this.tbx_password.Name = "tbx_password";
            this.tbx_password.Size = new System.Drawing.Size(210, 21);
            this.tbx_password.TabIndex = 1;
            // 
            // MQTTClient配置
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox1);
            this.Name = "MQTTClient配置";
            this.Text = "SetMqtt";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MQTTClient配置_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listBoxControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxControl2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbx_userName;
        private System.Windows.Forms.TextBox tbx_ip;
        private DevExpress.XtraEditors.SimpleButton btn_del_write;
        private DevExpress.XtraEditors.SimpleButton btn_del_read;
        private DevExpress.XtraEditors.SimpleButton btn_add_write;
        private DevExpress.XtraEditors.SimpleButton btn_add_read;
        private DevExpress.XtraEditors.ListBoxControl listBoxControl2;
        private DevExpress.XtraEditors.ListBoxControl listBoxControl1;
        private System.Windows.Forms.Button btn_connect;
        private System.Windows.Forms.Button btn_disconnect;
        private System.Windows.Forms.RichTextBox rtb_msg;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private System.Windows.Forms.TextBox tbx_password;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
    }
}