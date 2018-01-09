Imports System
Imports System.Threading  'might get rid of Threading later
Imports System.IO.Ports
Imports System.ComponentModel

Public Class Form1
    ' Define Global variables for the class

    ' Tachometer Variables
    Dim g As System.Drawing.Graphics
    Dim Xcen As Integer = 360     'This will define the center of the circle
    Dim Ycen As Integer = 265

    ' Thermometer Variable
    Dim h As System.Drawing.Graphics
    Dim border1 As Integer = 200    ' This is the borderline of too cold
    Dim border2 As Integer = 650    ' This is the beginning of the green zone
    Dim border3 As Integer = 730    ' This is the end of the green zone
    Dim border4 As Integer = 800    ' This is the redline borderline

    ' Other things
    Private th_num As String
    Dim myPort As Array
    Delegate Sub SetTextCallBack(ByVal [text] As String)   ' This is to handle threading issues with the communication, will be implimented later

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TempLbl.Text = " °C"
        g = TachBox.CreateGraphics()
        h = TempBox.CreateGraphics()
        myPort = IO.Ports.SerialPort.GetPortNames()
        Port_select.Items.AddRange(myPort)
        ThrottleVal.Text = "0 % "   ' show the starting position of the throttle
        th_num = ThrottleVal.Text
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles startButton.Click

    End Sub

    Private Sub stopButton_Click(sender As Object, e As EventArgs) Handles stopButton.Click

    End Sub

    Private Sub TachBox_Paint(sender As Object, e As PaintEventArgs) Handles TachBox.Paint

        ' Create solid brush.
        Dim fillBrush As New SolidBrush(Color.Beige)
        Dim thickTick As Pen
        thickTick = New Pen(Color.Black, 5)
        Dim xpos As Integer = 145
        Dim ypos As Integer = 50
        Dim gWid As Integer = 430
        Dim R As Integer = gWid / 2

        Dim numTicks As Integer = 25

        ' Create rectangle for ellipse.
        Dim rect As New Rectangle(xpos, ypos, gWid, gWid)

        ' Create start and sweep angles.
        Dim startAngle As Single = 40.0
        Dim sweepAngle As Single = -260.0

        ' Fill pie to screen.
        g.FillPie(fillBrush, rect, startAngle, sweepAngle)

        ' Now fill in the red line section, this will change depending on testing or future data
        Dim redBrush As New SolidBrush(Color.IndianRed)
        Dim factor As Decimal = 120000 / 260
        Dim redLine As Integer = 107000   ' THIS IS THE VALUE TO CHANGE
        Dim redAngle As Single = sweepAngle + (redLine / factor)

        g.FillPie(redBrush, rect, startAngle, redAngle)

        ' First I need to get the degrees between each tick mark there will be.   I want 25 tick marks.  Also Convert to radians
        Dim degPer As Decimal = (Math.Abs(sweepAngle) / (numTicks - 1)) * Math.PI / 180
        Dim curDeg As Decimal = (-startAngle - sweepAngle) * Math.PI / 180

        ' Now define a matrix to hold the starting point of each of the line segments 
        Dim startPoint(numTicks - 1, 1) As Integer
        Dim endPoint(numTicks - 1, 1) As Integer
        Dim speedNum As Integer
        Dim S1 As String

        Dim sf As New StringFormat
        sf.LineAlignment = StringAlignment.Center
        sf.Alignment = StringAlignment.Center

        ' Now do the looping
        For i = 0 To numTicks - 1
            ' The incrementing of the angle will happen at the end of the loop
            startPoint(i, 0) = Xcen + R * Math.Cos(curDeg)
            startPoint(i, 1) = Ycen - R * Math.Sin(curDeg)
            endPoint(i, 0) = 0.1 * Xcen + 0.9 * startPoint(i, 0)
            endPoint(i, 1) = 0.1 * Ycen + 0.9 * startPoint(i, 1)
            ' Now that I have both of the points, I just need to display them
            g.DrawLine(thickTick, startPoint(i, 0), startPoint(i, 1), endPoint(i, 0), endPoint(i, 1))

            ' Now I need to display the static numbers
            If (i Mod 2) = 0 Then
                speedNum = i / 2
                If i <> 0 Then
                    S1 = speedNum.ToString() & "0"
                Else
                    S1 = speedNum.ToString()
                End If
                endPoint(i, 0) = 0.18 * Xcen + 0.82 * startPoint(i, 0)
                endPoint(i, 1) = 0.18 * Ycen + 0.82 * startPoint(i, 1)
                g.DrawString(S1, New Font("Calibri", 14, FontStyle.Bold), New SolidBrush(Color.Black), endPoint(i, 0), endPoint(i, 1), sf)
            End If

            curDeg -= degPer
            If curDeg < 0 Then
                curDeg += 2 * 3.141592654
            End If
        Next
        DrawNeedle(18462)
        MassFlow(6.99)
        TempHandler(69)
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        sf.Dispose()

    End Sub

    Private Sub Port_select_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Port_select.SelectedIndexChanged
        ' This is the function which will connect with the microcontroller, confirm there is a connection, and change the colored text
        SerialPort1.PortName = Port_select.Text
        SerialPort1.BaudRate = 57600   ' Figure out if this baud rate will work during testing
        SerialPort1.Open()

        ' Now I need to do the connection check
        SerialPort1.Write("ACES" & vbCr)
        Timer1.Start()  ' Start 5 second timer to get the receive code



    End Sub

    Private Sub ThrottleScale_Scroll(sender As Object, e As EventArgs) Handles ThrottleScale.Scroll
        Dim dispString As String
        dispString = CStr(ThrottleScale.Value)
        ThrottleVal.Text = dispString & " % "
        th_num = ThrottleVal.Text

    End Sub

    Private Sub ThrottleVal_KeyUp(sender As Object, e As KeyEventArgs) Handles ThrottleVal.KeyUp
        If e.KeyCode = Keys.Enter Then
            ' First check to make sure that it is an integer
            Dim num As Integer
            If Integer.TryParse(ThrottleVal.Text, num) Then
                ' Now need to do bounds checking
                If num >= 0 And num <= 100 Then
                    ThrottleVal.Text = ThrottleVal.Text & " % "
                    ThrottleScale.Value = num
                    th_num = ThrottleVal.Text
                Else
                    MessageBox.Show("Please input an integer 0-100", "Bounds Error")
                    ThrottleVal.Text = th_num
                End If
            Else
                ' It was not a number
                MessageBox.Show("Please input an integer 0-100", "Input Type Error")
                ThrottleVal.Text = th_num
            End If
        End If
        e.SuppressKeyPress = False
    End Sub

    Private Sub ThrottleVal_KeyDown(sender As Object, e As KeyEventArgs) Handles ThrottleVal.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub DrawNeedle(speed As Integer)
        Dim maxWid As Integer = 6
        Dim minWid As Integer = 1
        Dim circle_dim As Integer = 30
        Dim factor As Decimal = (120000 * 180) / (260 * Math.PI)
        Dim needleBrush As New SolidBrush(Color.Red)
        Dim circle_brush As New SolidBrush(Color.Black)
        Dim point1 As New Point(-minWid, -170)   '  This is the vertex  remember to add Xcen and Ycen back in
        Dim point2 As New Point(minWid, -170)
        Dim point3 As New Point(maxWid, 0)   '  This is the bottom left point
        Dim point4 As New Point(-maxWid, 0)   '  This is the bottom right point

        ' Now figure out the rotation angle
        Dim angle As Decimal = speed / factor
        Dim ref_angle As Decimal = 60000 / factor
        Dim tempVal As Integer = point1.X
        angle = angle - ref_angle   ' this is the angle to rotate
        point1.X = point1.X * Math.Cos(angle) - point1.Y * Math.Sin(angle) + Xcen
        point1.Y = tempVal * Math.Sin(angle) + point1.Y * Math.Cos(angle) + Ycen

        tempVal = point2.X
        point2.X = point2.X * Math.Cos(angle) - point2.Y * Math.Sin(angle) + Xcen
        point2.Y = tempVal * Math.Sin(angle) + point2.Y * Math.Cos(angle) + Ycen

        tempVal = point3.X
        point3.X = point3.X * Math.Cos(angle) - point3.Y * Math.Sin(angle) + Xcen
        point3.Y = tempVal * Math.Sin(angle) + point3.Y * Math.Cos(angle) + Ycen

        tempVal = point4.X
        point4.X = point4.X * Math.Cos(angle) - point4.Y * Math.Sin(angle) + Xcen
        point4.Y = tempVal * Math.Sin(angle) + point4.Y * Math.Cos(angle) + Ycen
        Dim pointList As Point() = {point1, point2, point3, point4}

        g.FillPolygon(needleBrush, pointList)
        g.FillEllipse(circle_brush, New Rectangle(Xcen - circle_dim / 2, Ycen - circle_dim / 2, circle_dim, circle_dim))

        RPM_lbl.Text = CStr(speed) & "     RPM"


    End Sub

    Private Sub MassFlow(flow As Decimal)
        Dim flow_num As String
        flow_num = CStr(flow)
        flow_num = String.Format(flow_num, "#.##") & " g/sec"
        flowRate.Text = flow_num

    End Sub

    Private Sub TempHandler(Temp As Integer)  ' This take temperature in Celcius

        If Temp <= border1 Then
            Therm.ForeColor = Color.MidnightBlue
        ElseIf Temp > border1 And Temp <= border2 Then
            Therm.ForeColor = Color.Beige
        ElseIf Temp > border2 And Temp <= border3 Then
            Therm.ForeColor = Color.Green
        ElseIf Temp > border3 And Temp <= border4 Then
            Therm.ForeColor = Color.Beige
        ElseIf Temp > border4 Then
            Therm.ForeColor = Color.Red
        End If

        Therm.Value = Temp
        TempLbl.Text = CStr(Temp) & " °C"
    End Sub

    Private Sub TempBox_Paint(sender As Object, e As PaintEventArgs) Handles TempBox.Paint
        ' First I need to draw all of the colored box regions 

        Dim recHeight As Integer = 10
        Dim region1 As Integer = (Therm.Width / 1000) * border1
        Dim region2 As Integer = (Therm.Width / 1000) * (border2 - border1)
        Dim region3 As Integer = (Therm.Width / 1000) * (border3 - border2)
        Dim region4 As Integer = (Therm.Width / 1000) * (border4 - border3)
        Dim region5 As Integer = (Therm.Width / 1000) * (1000 - border4)

        ' Do the first (Blue) region
        Dim fillBrush As New SolidBrush(Color.MidnightBlue)
        Dim rect As New Rectangle(Therm.Location.X, Therm.Location.Y - recHeight, region1, recHeight)
        h.FillRectangle(fillBrush, rect)

        ' Do the second (white) region
        fillBrush.Color = Color.Beige
        rect.X = rect.X + region1
        rect.Width = region2
        h.FillRectangle(fillBrush, rect)

        ' Do the third (green) region
        fillBrush.Color = Color.Green
        rect.X = rect.X + region2
        rect.Width = region3
        h.FillRectangle(fillBrush, rect)

        ' Do the fourth (white) region
        fillBrush.Color = Color.Beige
        rect.X = rect.X + region3
        rect.Width = region4
        h.FillRectangle(fillBrush, rect)

        ' Do the fifth (red) region
        fillBrush.Color = Color.Red
        rect.X = rect.X + region4
        rect.Width = region5
        h.FillRectangle(fillBrush, rect)


        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Now draw the tick marks
        Dim numTicks As Integer = 21
        Dim startPoint(numTicks - 1, 1) As Integer
        Dim endPoint(numTicks - 1, 1) As Integer
        Dim cur_x As Integer = Therm.Location.X
        Dim bigTick As New Pen(Color.Black, 4)
        Dim smallTick As New Pen(Color.Black, 2)


        For i = 0 To numTicks - 1
            startPoint(i, 0) = cur_x
            startPoint(i, 1) = Therm.Location.Y
            endPoint(i, 0) = cur_x

            If i Mod 2 = 0 Then
                endPoint(i, 1) = Therm.Location.Y - 20
                h.DrawLine(bigTick, startPoint(i, 0), startPoint(i, 1), endPoint(i, 0), endPoint(i, 1))
            Else
                endPoint(i, 1) = Therm.Location.Y - 15
                h.DrawLine(smallTick, startPoint(i, 0), startPoint(i, 1), endPoint(i, 0), endPoint(i, 1))
            End If
            cur_x = cur_x + (Therm.Width / (numTicks - 1))

        Next

        ''''''''''''''''''''''''''''''''''''''''''''''''''
        '' Now for the numbers on the scale

        Dim sf As New StringFormat
        sf.LineAlignment = StringAlignment.Center
        sf.Alignment = StringAlignment.Near
        Dim SaveCos As Decimal = Math.Cos(Math.PI / 4)
        Dim SaveSin As Decimal = Math.Sin(Math.PI / 4)
        Dim str As String
        h.RotateTransform(-45)   ' rotate to make the labels look good

        For i = 0 To numTicks - 1
            If i = 0 Then
                str = "0"
            Else
                str = CStr(i / 2) & "00"
            End If
            If i Mod 2 = 0 Then
                h.DrawString(str, New Font("Calibri", 10), New SolidBrush(Color.White),
                             endPoint(i, 0) * SaveCos - endPoint(i, 1) * SaveSin, endPoint(i, 1) * SaveSin + endPoint(i, 0) * SaveCos - 7, sf)
            End If
        Next


    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        
    End Sub

    Private Sub SerialPort1_DataReceived(sender As Object, e As SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived

    End Sub
End Class
