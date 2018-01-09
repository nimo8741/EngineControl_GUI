<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ECU_conn = New System.Windows.Forms.Label()
        Me.Port_select = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.flowRate = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.ThrottleScale = New System.Windows.Forms.TrackBar()
        Me.ThrottleVal = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.stopButton = New System.Windows.Forms.Button()
        Me.startButton = New System.Windows.Forms.Button()
        Me.TempBox = New System.Windows.Forms.Panel()
        Me.Therm = New System.Windows.Forms.ProgressBar()
        Me.TempLbl = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TachBox = New System.Windows.Forms.Panel()
        Me.Panel7 = New System.Windows.Forms.Panel()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Panel6 = New System.Windows.Forms.Panel()
        Me.RPM_lbl = New System.Windows.Forms.Label()
        Me.SerialPort1 = New System.IO.Ports.SerialPort(Me.components)
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.ThrottleScale, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel4.SuspendLayout()
        Me.TempBox.SuspendLayout()
        Me.TachBox.SuspendLayout()
        Me.Panel7.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.Add(Me.ECU_conn)
        Me.Panel1.Controls.Add(Me.Port_select)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Location = New System.Drawing.Point(1022, 110)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(219, 115)
        Me.Panel1.TabIndex = 0
        '
        'ECU_conn
        '
        Me.ECU_conn.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ECU_conn.ForeColor = System.Drawing.Color.Red
        Me.ECU_conn.Location = New System.Drawing.Point(25, 80)
        Me.ECU_conn.Name = "ECU_conn"
        Me.ECU_conn.Size = New System.Drawing.Size(173, 20)
        Me.ECU_conn.TabIndex = 2
        Me.ECU_conn.Text = "ECU is NOT connected"
        Me.ECU_conn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Port_select
        '
        Me.Port_select.FormattingEnabled = True
        Me.Port_select.Location = New System.Drawing.Point(49, 46)
        Me.Port_select.Name = "Port_select"
        Me.Port_select.Size = New System.Drawing.Size(121, 21)
        Me.Port_select.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.Label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(65, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(94, 24)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "COM PORT"
        Me.Label1.UseCompatibleTextRendering = True
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel2.Controls.Add(Me.flowRate)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Location = New System.Drawing.Point(962, 290)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(331, 51)
        Me.Panel2.TabIndex = 1
        '
        'flowRate
        '
        Me.flowRate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.flowRate.AutoSize = True
        Me.flowRate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.flowRate.ForeColor = System.Drawing.Color.White
        Me.flowRate.Location = New System.Drawing.Point(192, 13)
        Me.flowRate.Name = "flowRate"
        Me.flowRate.Size = New System.Drawing.Size(92, 20)
        Me.flowRate.TabIndex = 1
        Me.flowRate.Text = "0.00 g/sec"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(60, 13)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(120, 20)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Fuel Flow Rate:"
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel3.Controls.Add(Me.ThrottleScale)
        Me.Panel3.Controls.Add(Me.ThrottleVal)
        Me.Panel3.Controls.Add(Me.Label5)
        Me.Panel3.Location = New System.Drawing.Point(962, 414)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(331, 100)
        Me.Panel3.TabIndex = 2
        '
        'ThrottleScale
        '
        Me.ThrottleScale.LargeChange = 10
        Me.ThrottleScale.Location = New System.Drawing.Point(48, 41)
        Me.ThrottleScale.Maximum = 100
        Me.ThrottleScale.Name = "ThrottleScale"
        Me.ThrottleScale.Size = New System.Drawing.Size(260, 45)
        Me.ThrottleScale.TabIndex = 2
        '
        'ThrottleVal
        '
        Me.ThrottleVal.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ThrottleVal.Location = New System.Drawing.Point(182, 16)
        Me.ThrottleVal.Name = "ThrottleVal"
        Me.ThrottleVal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ThrottleVal.Size = New System.Drawing.Size(100, 22)
        Me.ThrottleVal.TabIndex = 1
        Me.ThrottleVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(63, 16)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(67, 20)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Throttle:"
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel4.Controls.Add(Me.stopButton)
        Me.Panel4.Controls.Add(Me.startButton)
        Me.Panel4.Location = New System.Drawing.Point(962, 584)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(331, 100)
        Me.Panel4.TabIndex = 3
        '
        'stopButton
        '
        Me.stopButton.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.stopButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.stopButton.Location = New System.Drawing.Point(177, 22)
        Me.stopButton.Name = "stopButton"
        Me.stopButton.Size = New System.Drawing.Size(138, 53)
        Me.stopButton.TabIndex = 1
        Me.stopButton.Text = "STOP"
        Me.stopButton.UseVisualStyleBackColor = False
        '
        'startButton
        '
        Me.startButton.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.startButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.startButton.Location = New System.Drawing.Point(14, 22)
        Me.startButton.Name = "startButton"
        Me.startButton.Size = New System.Drawing.Size(138, 53)
        Me.startButton.TabIndex = 0
        Me.startButton.Text = "START"
        Me.startButton.UseVisualStyleBackColor = False
        '
        'TempBox
        '
        Me.TempBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TempBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.TempBox.Controls.Add(Me.Therm)
        Me.TempBox.Controls.Add(Me.TempLbl)
        Me.TempBox.Controls.Add(Me.Label2)
        Me.TempBox.Location = New System.Drawing.Point(73, 536)
        Me.TempBox.Name = "TempBox"
        Me.TempBox.Size = New System.Drawing.Size(720, 179)
        Me.TempBox.TabIndex = 2
        '
        'Therm
        '
        Me.Therm.ForeColor = System.Drawing.Color.MidnightBlue
        Me.Therm.Location = New System.Drawing.Point(38, 113)
        Me.Therm.Maximum = 1000
        Me.Therm.Name = "Therm"
        Me.Therm.Size = New System.Drawing.Size(640, 44)
        Me.Therm.Step = 1
        Me.Therm.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.Therm.TabIndex = 2
        '
        'TempLbl
        '
        Me.TempLbl.AutoSize = True
        Me.TempLbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TempLbl.ForeColor = System.Drawing.Color.White
        Me.TempLbl.Location = New System.Drawing.Point(403, 28)
        Me.TempLbl.Name = "TempLbl"
        Me.TempLbl.Size = New System.Drawing.Size(75, 29)
        Me.TempLbl.TabIndex = 1
        Me.TempLbl.Text = "69 0C"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(238, 26)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(159, 29)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Temperature:"
        '
        'TachBox
        '
        Me.TachBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(70, Byte), Integer))
        Me.TachBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.TachBox.Controls.Add(Me.Panel7)
        Me.TachBox.Controls.Add(Me.Panel6)
        Me.TachBox.Location = New System.Drawing.Point(73, 57)
        Me.TachBox.Name = "TachBox"
        Me.TachBox.Size = New System.Drawing.Size(720, 460)
        Me.TachBox.TabIndex = 4
        '
        'Panel7
        '
        Me.Panel7.BackColor = System.Drawing.Color.FromArgb(CType(CType(150, Byte), Integer), CType(CType(150, Byte), Integer), CType(CType(150, Byte), Integer))
        Me.Panel7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel7.Controls.Add(Me.Label6)
        Me.Panel7.Location = New System.Drawing.Point(278, 355)
        Me.Panel7.Name = "Panel7"
        Me.Panel7.Size = New System.Drawing.Size(158, 41)
        Me.Panel7.TabIndex = 1
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.White
        Me.Label6.Location = New System.Drawing.Point(25, -1)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(105, 40)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Tachometer" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(RPM x 1000)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel6
        '
        Me.Panel6.BackColor = System.Drawing.Color.FromArgb(CType(CType(150, Byte), Integer), CType(CType(150, Byte), Integer), CType(CType(150, Byte), Integer))
        Me.Panel6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel6.Controls.Add(Me.RPM_lbl)
        Me.Panel6.Location = New System.Drawing.Point(278, 179)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(158, 41)
        Me.Panel6.TabIndex = 0
        '
        'RPM_lbl
        '
        Me.RPM_lbl.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RPM_lbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RPM_lbl.ForeColor = System.Drawing.Color.White
        Me.RPM_lbl.Location = New System.Drawing.Point(15, 7)
        Me.RPM_lbl.Name = "RPM_lbl"
        Me.RPM_lbl.Size = New System.Drawing.Size(123, 23)
        Me.RPM_lbl.TabIndex = 0
        Me.RPM_lbl.Text = "000000     RPM"
        Me.RPM_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'SerialPort1
        '
        Me.SerialPort1.BaudRate = 57600
        '
        'Timer1
        '
        Me.Timer1.Interval = 5000
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(45, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(45, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1448, 744)
        Me.Controls.Add(Me.TachBox)
        Me.Controls.Add(Me.TempBox)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form1"
        Me.Text = "ACES Engine Controller"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.ThrottleScale, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel4.ResumeLayout(False)
        Me.TempBox.ResumeLayout(False)
        Me.TempBox.PerformLayout()
        Me.TachBox.ResumeLayout(False)
        Me.Panel7.ResumeLayout(False)
        Me.Panel7.PerformLayout()
        Me.Panel6.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents Port_select As ComboBox
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents Panel4 As Panel
    Friend WithEvents TempBox As Panel
    Friend WithEvents startButton As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents stopButton As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents ECU_conn As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents ThrottleVal As TextBox
    Friend WithEvents flowRate As Label
    Friend WithEvents TempLbl As Label
    Friend WithEvents ThrottleScale As TrackBar
    Friend WithEvents TachBox As Panel
    Friend WithEvents Panel6 As Panel
    Friend WithEvents RPM_lbl As Label
    Friend WithEvents Panel7 As Panel
    Friend WithEvents Label6 As Label
    Friend WithEvents SerialPort1 As IO.Ports.SerialPort
    Friend WithEvents Therm As ProgressBar
    Friend WithEvents Timer1 As Timer
End Class
