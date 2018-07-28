' Author: Nick Moore
' Created: Janurary 8, 2018
' Purpose: GUI for user interaction and operation of a model jet engine

Imports System
Imports System.Threading  'might get rid of Threading later
Imports System.IO.Ports
Imports System.IO
Imports System.Timers
Imports System.ComponentModel
Imports System.Text

Public Class Form1
    ' Define Global variables for the class

    ' Tachometer Variables
    Dim g As System.Drawing.Graphics
    Dim Xcen As Integer = 360     'This will define the center of the circle
    Dim Ycen As Integer = 265
    Dim point1_old As New Point(0, 0)   '  These will keep track of the previous location of the needle
    Dim point2_old As New Point(0, 0)
    Dim point3_old As New Point(0, 0)
    Dim point4_old As New Point(0, 0)

    ' Thermometer Variable
    Dim h As System.Drawing.Graphics
    Dim border1 As Integer = 200    ' This is the borderline of too cold
    Dim border2 As Integer = 325    ' This is the beginning of the green zone
    Dim border3 As Integer = 730    ' This is the end of the green zone
    Dim border4 As Integer = 800    ' This is the redline borderline
    Dim connect As Boolean = False
    Dim running As Boolean = False


    ' Other things
    Private th_num As String
    Dim myPort As Array
    Delegate Sub SetTextCallBack(ByVal [Connected] As Boolean)   ' This is to handle threading issues with the communication, will be implimented later
    Dim file_handle As System.IO.StreamWriter
    Dim startTime As Date                  ' This will be the global variable which will keep track of when times are started
    Dim last_command As Char
    Dim fileVal As String
    Dim file_closed As Boolean
    Private aTimer As System.Timers.Timer

    '// Description: This subroutine is invoked when the form (the application)   
    '// is loaded, i.e. someone double clicked on the executable file.
    '//
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TempLbl.Text = " °C"
        g = TachBox.CreateGraphics()
        h = TempBox.CreateGraphics()
        ThrottleVal.Text = "Idle "   ' show the starting position of the throttle
        th_num = ThrottleVal.Text
        Panel3.Enabled = False       ' Turn off these controls until idle has been reached    SET BACK TO FALSE

        ' Now need to initialize the datafile
        initializeDataFile()

        PictureBox1.Image = My.Resources.off_bulb   ' Initially show the off light bulb
        BatProgress.Value = 0                       ' Initially show no progress on the circular progress bar

    End Sub

    '// Description: This subroutine is invoked whenever the user clicks on the START button
    '//
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles startButton.Click
        If Not connect Then
            MessageBox.Show("Please connect to the ECU first", "Sequence Error")
        ElseIf Not running Then
            SerialPort1.Write("Or")  ' R for Run
            startButton.BackColor = Color.DarkTurquoise
            startButton.Text = "RUNNING"
            running = True
            last_command = "r"c
            file_handle.WriteLine("Starting the Engine")


        End If
        ' Now need to open a new file if one is already open
        If file_closed Then
            initializeDataFile()
            file_handle.WriteLine("Starting the Engine")
        End If

    End Sub

    '// Description: This subroutine is invoked whenever the stop button is clicked
    '//
    Private Sub stopButton_Click(sender As Object, e As EventArgs) Handles stopButton.Click
        If connect And running Then
            running = False
            SerialPort1.Write("OS")   ' s for Stop.   Lower case to further prevent bit errors
            startButton.Text = "START"
            startButton.BackColor = Color.SeaGreen
            last_command = "s"c

            ' Now I need to stop the current data file
            file_handle.Close()
            file_closed = True

        End If
    End Sub

    '// Description: This subroutine is called to redraw the tachometer whenever its position needs to change
    Private Sub TachBoxPainter()
        ' Create solid brush.
        Dim fillBrush As New SolidBrush(Color.Beige)
        Dim xpos As Integer = 190
        Dim ypos As Integer = 95
        Dim gWid As Integer = 430
        Dim R As Integer = gWid / 1.26

        Dim numTicks As Integer = 27

        ' Create rectangle for ellipse.
        Dim rect As New Rectangle(xpos, ypos, R, R)

        ' Create start and sweep angles.
        Dim startAngle As Single = 45.0
        Dim sweepAngle As Single = -270.0

        ' Fill pie to screen.
        g.FillPie(fillBrush, rect, startAngle, sweepAngle)

        ' Now fill in the red line section, this will change depending on testing or future data
        Dim redBrush As New SolidBrush(Color.IndianRed)  ' was indianred
        Dim factor As Decimal = 130000 / 260
        Dim redLine As Integer = 123000   ' THIS IS THE VALUE TO CHANGE
        Dim redAngle As Single = -265 + (redLine / factor)

        g.FillPie(redBrush, rect, 45, redAngle)

        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
    End Sub

    '// Description: This subroutine is called to assist the initial drawing of the tachometer
    Private Sub TachBox_Paint(sender As Object, e As PaintEventArgs) Handles TachBox.Paint

        ' Create solid brush.
        Dim fillBrush As New SolidBrush(Color.Beige)
        Dim thickTick As Pen
        thickTick = New Pen(Color.Black, 5)
        Dim xpos As Integer = 145
        Dim ypos As Integer = 50
        Dim gWid As Integer = 430
        Dim R As Integer = gWid / 2

        Dim numTicks As Integer = 27

        ' Create rectangle for ellipse.
        Dim rect As New Rectangle(xpos, ypos, gWid, gWid)

        ' Create start and sweep angles.
        Dim startAngle As Single = 45.0   ' this was 40.0
        Dim sweepAngle As Single = -270.0   ' this was -260.0

        ' Fill pie to screen.
        g.FillPie(fillBrush, rect, startAngle, sweepAngle)

        ' Now fill in the red line section, this will change depending on testing or future data
        Dim redBrush As New SolidBrush(Color.IndianRed)
        Dim factor As Decimal = 130000 / 260
        Dim redLine As Integer = 123000   ' THIS IS THE VALUE TO CHANGE
        Dim redAngle As Single = -265 + (redLine / factor)

        g.FillPie(redBrush, rect, startAngle, redAngle)

        ' First I need to get the degrees between each tick mark there will be.   I want 25 tick marks.  Also Convert to radians
        Dim degPer As Decimal = (Math.Abs(-260) / (numTicks - 1)) * Math.PI / 180
        Dim curDeg As Decimal = (-40.0 + 260) * Math.PI / 180

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
                    S1 = speedNum.ToString()
                Else
                    S1 = speedNum.ToString()
                End If
                endPoint(i, 0) = Xcen + 0.85 * (startPoint(i, 0) - Xcen)
                endPoint(i, 1) = Ycen + 0.85 * (startPoint(i, 1) - Ycen)
                g.DrawString(S1, New Font("Calibri", 14, FontStyle.Bold), New SolidBrush(Color.Black), endPoint(i, 0), endPoint(i, 1), sf)
            End If

            curDeg -= degPer
            If curDeg < 0 Then
                curDeg += 2 * 3.141592654
            End If
        Next
        DrawNeedle(0)
        MassFlow(0)
        TempHandler(0)
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        sf.Dispose()

    End Sub

    '// Description: This subroutine is invoked whenever the value of the port drop-down is changed
    Private Sub Port_select_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Port_select.SelectedIndexChanged

        ECU_connection(False)
        SerialPort1.Close()
        ' This is the function which will connect with the microcontroller, confirm there is a connection, and change the colored text
        SerialPort1.PortName = Port_select.Text
        SerialPort1.BaudRate = 76800   ' Figure out if this baud rate will work during testing 78600
        SerialPort1.Open()

        ' Now I need to do the connection check
        SerialPort1.Write("ACES")
        SerialPort1.ReadExisting()   ' clear the buffer

    End Sub

    Private Sub ThrottleScale_Scroll(sender As Object, e As EventArgs) Handles ThrottleScale.Scroll
        If ThrottleScale.Value = 0 Then
            ThrottleVal.Text = "Idle "
            th_num = ThrottleVal.Text
            Return
        End If
        Dim dispString As String
        dispString = CStr(ThrottleScale.Value)
        ThrottleVal.Text = dispString & " % "
        th_num = ThrottleVal.Text

        '  Now need to send the new throttle value over to the ECU
        SerialPort1.Write("T")   ' s for Stop.   Lower case to further prevent bit errors
        SerialPort1.Write(ThrottleScale.Value)   ' s for Stop.   Lower case to further prevent bit errors

        last_command = "T"c

    End Sub

    Private Sub ThrottleVal_KeyUp(sender As Object, e As KeyEventArgs) Handles ThrottleVal.KeyUp
        If e.KeyCode = Keys.Enter Then
            ' First check to make sure that it is an integer
            Dim num As Integer
            If Integer.TryParse(ThrottleVal.Text, num) Then
                ' Now need to do bounds checking
                If num >= 0 And num <= 100 Then
                    If num = 0 Then
                        ThrottleVal.Text = "Idle "
                        ThrottleScale.Value = 0
                    Else
                        ThrottleVal.Text = ThrottleVal.Text & " % "
                        ThrottleScale.Value = num
                    End If
                    '  Now need to send the new throttle value over to the ECU
                    SerialPort1.Write("T")   ' s for Stop.   Lower case to further prevent bit errors
                    SerialPort1.Write(Chr(ThrottleScale.Value))   ' s for Stop.   Lower case to further prevent bit errors

                    last_command = "T"c
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

    Private Delegate Sub needleDelegate(ByVal speed As Integer)
    Private Sub DrawNeedle(speed As Integer)
        If (InvokeRequired) Then
            Invoke(New needleDelegate(AddressOf DrawNeedle), speed)
        Else
            speed = speed * (130000.0 / 65535.0)
            Dim maxWid As Integer = 6
            Dim minWid As Integer = 1
            Dim circle_dim As Integer = 30
            Dim factor As Decimal = (130000 * 180) / (260 * Math.PI)
            Dim needleBrush As New SolidBrush(Color.Red)
            Dim circle_brush As New SolidBrush(Color.Black)
            Dim point1 As New Point(-minWid, -160)   '  This is the vertex  remember to add Xcen and Ycen back in
            Dim point2 As New Point(minWid, -160)
            Dim point3 As New Point(maxWid, 0)   '  This is the bottom left point
            Dim point4 As New Point(-maxWid, 0)   '  This is the bottom right point

            ' Now figure out the rotation angle
            Dim angle As Decimal = speed / factor
            Dim ref_angle As Decimal = 65000 / factor
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


            TachBoxPainter()
            g.FillPolygon(needleBrush, pointList)
            g.FillEllipse(circle_brush, New Rectangle(Xcen - circle_dim / 2, Ycen - circle_dim / 2, circle_dim, circle_dim))

            ' Now save the point list for future use
            point1_old = point1
            point2_old = point2
            point3_old = point3
            point4_old = point4

            RPM_lbl.Text = CStr(speed) & "     RPM"
        End If

    End Sub

    Private Delegate Sub massDelegate(ByVal flow As Decimal)
    Private Sub MassFlow(flow As Decimal)
        If (InvokeRequired) Then
            Invoke(New massDelegate(AddressOf MassFlow), flow)
        Else
            Dim flow_num As String = String.Format("{0:n2} g/sec", flow)
            flowRate.Text = flow_num
        End If
    End Sub

    Private Delegate Sub tempDelegate(ByVal Temp As Decimal)
    Private Sub TempHandler(ByVal Temp As Decimal)  ' This takes temperature in Celcius
        If (InvokeRequired) Then
            Invoke(New tempDelegate(AddressOf TempHandler), Temp)
        Else
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

            Therm.Value = Temp       ' This will be on a scale out of 1000
            TempLbl.Text = String.Format("{0:n2} °C", Temp)
        End If

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
        Try
            h.FillRectangle(fillBrush, rect)
        Catch ex As Exception                   ' This will prevent it from redrawing
            Return
        End Try

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

        h.Dispose()
    End Sub

    Private Sub SerialPort1_DataReceived(sender As Object, e As SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived
        System.Threading.Thread.Sleep(30)       ' wait for a small amount of time to allow the full message to come in
        Dim byteArray(SerialPort1.BytesToRead()) As Byte
        SerialPort1.Read(byteArray, 0, SerialPort1.BytesToRead)
        Dim returnStr As String = System.Text.Encoding.UTF8.GetString(byteArray)

        If String.Compare("DALE", returnStr) = 0 Then
            ECU_connection(True)
        ElseIf (connect = 0) Then ' this is for if the ECU is not currently connected 
            MessageBox.Show("Incorrect connection password" & Environment.NewLine & "Please check connection", "Connection Error")
            connect = -1
        ElseIf (returnStr.CompareTo("V") = 0) Then   ' this means that the last command needs to be repeated
            repeatCommand()
        Else    ' This will handle all of the data which will be read in from the ECU and then call the associated subroutines
            messageReceive(byteArray)
        End If
    End Sub

    Private Sub ECU_connection(Connected As Boolean)
        If Me.ECU_conn.InvokeRequired Then
            Dim x As New SetTextCallBack(AddressOf ECU_connection)
            Me.Invoke(x, New Object() {(Connected)})
        Else
            If Connected Then
                ECU_conn.Text = "ECU is connected"
                ECU_conn.ForeColor = Color.Green
                connect = True
                startTime = Date.Now
                SetTimer()      ' Start the timer
            ElseIf Not Connected Then
                ECU_conn.Text = "ECU is NOT connected"
                ECU_conn.ForeColor = Color.Red
                connect = False
            End If
        End If
    End Sub

    Private Sub Port_select_DropDown(sender As Object, e As EventArgs) Handles Port_select.DropDown
        myPort = IO.Ports.SerialPort.GetPortNames()
        For i = 0 To myPort.Length - 1
            If Port_select.Items.Count = 0 Then
                Port_select.Items.Add(myPort(i))
            ElseIf Not Port_select.Items.Contains(myPort(i)) Then
                Port_select.Items.Add(myPort(i))
            End If
        Next
        Port_select.Sorted = True
    End Sub

    Private Delegate Sub receiveDelegate(ByVal input As Byte())
    Private Sub messageReceive(input As Byte())
        If (InvokeRequired) Then
            Invoke(New receiveDelegate(AddressOf messageReceive), input)
        Else
            ' First I need to check the length of the message to see if it needs to be resent
            If (input.Length = 1) Then
                Return                   ' Drop messages of this length
            End If
            If Not (input.Length = 29) Then     ' This is 29 instead of 28 because Vb.net is adding a terminator
                SerialPort1.Write("R")    ' This will mean that the data needs to be resent
                ' Next I need to do the parity byte error checking
            ElseIf (Not byteCheck(input)) Then ' there was an issue with the message transmission
                RichTextBox1.AppendText("Parity Error Detected, Shutting Down Engine" & Environment.NewLine)
                file_handle.WriteLine("Parity Error Detected")
                connect = False
                byteCheck(input)

            Else ' This means that I can trust the data and need to display/save it
                aTimer.Dispose()
                SetTimer()
                saveAndDisplay(input)
                SerialPort1.Write("K")  ' K for message confirmation

            End If
        End If


    End Sub

    Private Function byteCheck(byteList As Byte()) As Boolean
        ' First I need to delete all of the odd indeces from the array


        Dim result As Boolean = False
        Dim parity1 As Byte
        Dim parity2 As Byte
        Dim parity3 As Byte
        Dim parity4 As Byte
        ' first I need to seperate all of the string into the respective substrings
        For i As Integer = 0 To 3      ' there are four parity bytes
            For j As Integer = 0 To 1   ' there are two sections to each byte
                Dim count As Integer = 0
                For k As Integer = 0 To 2   ' there are three bytes per section
                    Dim this_byte As Byte = byteList((i * 6) + (j * 3) + k)
                    While this_byte > 0
                        count += 1
                        this_byte = this_byte And (this_byte - 1)
                    End While
                Next
                ' Now take the modulo
                count = count Mod 16
                ' Now fill the values into parity_i
                If i = 0 Then
                    parity1 = parity1 Or (count << j * 4)
                ElseIf i = 1 Then
                    parity2 = parity2 Or (count << j * 4)
                ElseIf i = 2 Then
                    parity3 = parity3 Or (count << j * 4)
                Else
                    parity4 = parity4 Or (count << j * 4)
                End If
            Next

        Next
        ' Now do all of the comparison
        If parity1 = byteList(24) And parity2 = byteList(25) And parity3 = byteList(26) And parity4 = byteList(27) Then    ' All parity bytes must be correct
            result = True
        End If


        Return result
    End Function

    Private Delegate Sub displayDelegate(ByVal input As Byte())
    Private Sub saveAndDisplay(input As Byte())
        If (InvokeRequired) Then
            Invoke(New displayDelegate(AddressOf saveAndDisplay), input)
        Else
            ' First convert the first byte into a Char
            Dim state As String = Convert.ToChar(input(0))

            ' Next get the massFlow
            Dim mFlow As Decimal = Convert.ToDecimal(BitConverter.ToSingle(input, 1))

            ' Next get the RPM 
            Dim RPM As Integer = Convert.ToInt32(BitConverter.ToUInt16(input, 5))

            ' Next get the EGT
            Dim EGT As Decimal = Convert.ToDecimal(BitConverter.ToSingle(input, 7))

            ' Next get the battery voltage
            Dim batVolts As Decimal = Convert.ToDecimal(BitConverter.ToSingle(input, 11))

            ' Next get the Glow plug state
            Dim gPlug As Byte = input(15)

            ' Next get the Temperature of the ECU
            Dim ECU_Temp As Decimal = Convert.ToDecimal(BitConverter.ToSingle(input, 16))

            ' Next get the Temperature of the ESB
            Dim ESB_Temp As Decimal = Convert.ToDecimal(BitConverter.ToSingle(input, 20))

            ' Now update the data on screen
            TempHandler(EGT)
            DrawNeedle(RPM)
            MassFlow(mFlow)
            Voltage(batVolts)
            glowPlug(gPlug)
            BoardTemp(ECU_Temp, ESB_Temp)

            ' Now write the information to file
            Dim endTime As Date = Date.Now
            Dim span As TimeSpan = endTime.Subtract(startTime)

            ' The data will be formatted as follows
            ' Time since engine start command, State Char, EGT, RPM, Mass Flow
            If file_closed = False Then
                Dim writeString As String = String.Format("{0},{1},{2},{3},{4},{5},{6},{7}", span.TotalSeconds, span.Milliseconds, state, Math.Floor(EGT * (1000.0 / 65535.0)), Math.Floor(RPM * (130000.0 / 65535)), mFlow, batVolts, ECU_Temp, gPlug)
                file_handle.WriteLine(writeString)
            End If

            ' Now write important messages to the output window
            Select Case state
                Case "S"
                    RichTextBox1.AppendText("Engine shutdown requested and is being carried out" & Environment.NewLine)

                Case "r"
                    RichTextBox1.AppendText("Engine Startup in Progress" & Environment.NewLine)

                Case "t"
                    RichTextBox1.AppendText("Throttle Adjustment in progress" & Environment.NewLine)

                Case "C"
                    RichTextBox1.AppendText("Engine Entering Cooling Mode" & Environment.NewLine)

                Case "g"
                    RichTextBox1.AppendText("Exhaust Gas Thermocouple error, Special Shutdown in progress" & Environment.NewLine)

                Case "N"
                    RichTextBox1.AppendText("Engine has Reached Desired Throttle" & Environment.NewLine)

                Case "b"
                    RichTextBox1.AppendText("No Connection with ESB" & Environment.NewLine)

                Case "I"
                    RichTextBox1.AppendText("Engine has Reached Idle" & Environment.NewLine)

                Case "T"
                    RichTextBox1.AppendText("Temperature Limit Reached, Shutting Down" & Environment.NewLine)

                Case "R"
                    RichTextBox1.AppendText("RPM Limit Reached, Shutting Down" & Environment.NewLine)

                Case "P"
                    RichTextBox1.AppendText("Fuel not flowing or Flow meter not working, Shutting Down" & Environment.NewLine)
            End Select
            RichTextBox1.SelectionStart = RichTextBox1.TextLength
            RichTextBox1.ScrollToCaret()
        End If

    End Sub

    Private Sub initializeDataFile()
        ' This will first check inside of the Output file which is located in the same directory as the .exe for any txt files that are on the same day
        Dim Dir As New DirectoryInfo("Output")
        Dim FilesInDir As FileSystemInfo()
        FilesInDir = Dir.GetFileSystemInfos
        Dim today_file = FilesInDir.Where(Function(p) p.CreationTime.Date = Now.Date)
        Dim list_ver = today_file.ToList
        ' Now loop through all of the files in today_file

        Dim new_test As Integer = 0
        For index As Integer = 0 To (list_ver.Count - 1)
            Dim beg_index As Integer = list_ver(index).FullName.LastIndexOf("_") + 1
            Dim length As Integer = list_ver(index).FullName.IndexOf(".") - beg_index
            Dim temp As Integer = Integer.Parse(list_ver(index).FullName.Substring(list_ver(index).FullName.LastIndexOf("_") + 1, length)) + 1
            If temp > new_test Then
                new_test = temp
            End If
        Next
        If (list_ver.Count = 0) Then
            FileVal = Directory.GetCurrentDirectory & "\Output\EngineRun_" & DateTime.Now.ToString("M_dd_yyyy") & "_test_0"
        Else
            FileVal = list_ver(list_ver.Count - 1).FullName.Substring(0, list_ver(list_ver.Count - 1).FullName.LastIndexOf("_") + 1) & new_test.ToString()

        End If

        fileVal = fileVal & ".txt"       ' This will append the correct extension

        file_handle = My.Computer.FileSystem.OpenTextFileWriter(fileVal, True)  ' Now the file will be created
        file_handle.WriteLine(String.Format("Created on {0:d} at {0:t}", Date.Now))
        file_handle.WriteLine("Time (sec), State, EGT, RPM, mFlow, batVolts, ECU_Temp, Glowplug")
        file_closed = False

    End Sub

    Private Delegate Sub voltDelegate(ByVal batVolts As Decimal)
    Private Sub Voltage(batVolts As Decimal)
        If (InvokeRequired) Then
            Invoke(New voltDelegate(AddressOf Voltage), batVolts)
        Else
            ' This needs to update the circular progress bar change the color
            BatProgress.Value = ((batVolts - 7.5) / (10.8 - 7.5)) * 100
            ' Now update the color
            If BatProgress.Value > 50 Then
                BatProgress.ProgressColor = Color.ForestGreen
            ElseIf BatProgress.Value > 25 Then
                BatProgress.ProgressColor = Color.DarkOrange
            Else
                BatProgress.ProgressColor = Color.Crimson
            End If
            If batVolts < 10 Then
                Dim volt_num As String = String.Format(" {0:n2} V", batVolts)  ' Add the space so that the padding is good
                BatVoltsLabel.Text = volt_num
            Else
                Dim volt_num As String = String.Format("{0:n2} V", batVolts)
                BatVoltsLabel.Text = volt_num
            End If
            BatVoltsLabel.ForeColor = BatProgress.ProgressColor

        End If

    End Sub

    Private Delegate Sub glowDelegate(ByVal input As Byte)
    Private Sub glowPlug(input As Byte)
        If (InvokeRequired) Then
            Invoke(New glowDelegate(AddressOf glowPlug), input)
        Else
            'This needs to update the picture of the glowplug if it is turned on
            If input = 1 Then
                PictureBox1.Image = My.Resources.on_bulb
            Else
                PictureBox1.Image = My.Resources.off_bulb
            End If
        End If
    End Sub

    Private Sub Form1_Closing(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        Try
            SerialPort1.Close()     ' Close the serial port
            file_handle.Close()
        Catch ex As Exception  ' If it fails, then I want nothing to happen

        End Try
    End Sub

    Private Delegate Sub repeatDelegate()
    Private Sub repeatCommand()
        If (InvokeRequired) Then
            Invoke(New repeatDelegate(AddressOf repeatCommand))
        Else
            If (last_command.CompareTo("r"c) = 0) Then
                '  Now need to send the new throttle value over to the ECU
                SerialPort1.Write("Or")   ' s for Stop.   Lower case to further prevent bit errors

            ElseIf (last_command.CompareTo("S"c) = 0) Then
                SerialPort1.Write("OS")

            ElseIf (last_command.CompareTo("T"c) = 0) Then
                SerialPort1.Write("T")
                SerialPort1.Write(ThrottleScale.Value)

            End If
        End If
    End Sub

    Private Delegate Sub boardDelegate(ByVal ECU_temp As Decimal, ByVal ESB_temp As Decimal)
    Private Sub BoardTemp(ECU_temp As Decimal, ESB_temp As Decimal)
        If (InvokeRequired) Then
            Invoke(New boardDelegate(AddressOf BoardTemp), ECU_temp)
        Else
            Dim constant1 As String = "ECU Temperature:"
            Dim constant2 As String = "ESB Temperature:"
            Dim temp_num_ECU As String = String.Format(" {0:n2} °C", ECU_temp)  ' Add the space so that the padding is good
            Dim temp_num_ESB As String = String.Format(" {0:n2} °C", ESB_temp)
            temp_num_ECU.PadLeft(10)
            temp_num_ESB.PadLeft(10)
            ECUTemp.Text = constant1 & temp_num_ECU
            ESBTemp.Text = constant2 & temp_num_ESB
        End If
    End Sub

    Private Sub SetTimer()
        ' Create a timer with a one second interval.
        aTimer = New System.Timers.Timer(1000)
        ' Hook up the Elapsed event for the timer. 
        AddHandler aTimer.Elapsed, AddressOf OnTimedEvent
        aTimer.AutoReset = True
        aTimer.Enabled = True
    End Sub

    ' The event handler for the Timer.Elapsed event. 
    Private Delegate Sub timerDelegate(ByVal source As Object, ByVal e As ElapsedEventArgs)
    Private Sub OnTimedEvent(source As Object, e As ElapsedEventArgs)
        If (InvokeRequired) Then
            Invoke(New timerDelegate(AddressOf OnTimedEvent), source, e)
        Else
            ' If it makes it in here then it is assumed that the ECU has been disconnected
            aTimer.Stop()
            aTimer.Dispose()

            connect = False
            ECU_conn.Text = "ECU is NOT connected"
            ECU_conn.ForeColor = Color.Red

            running = False
            startButton.Text = "START"
            startButton.BackColor = Color.SeaGreen

            ' Now I need to stop the current data file
            file_handle.Write("ECU has been disconnected")
            file_handle.Close()
            file_closed = True

            RichTextBox1.AppendText("ECU has been disconnected" & Environment.NewLine)
            RichTextBox1.SelectionStart = RichTextBox1.TextLength
            RichTextBox1.ScrollToCaret()

            ' Now update the Port list on the drop down
            myPort = IO.Ports.SerialPort.GetPortNames()
            For i = 0 To myPort.Length - 1
                If Port_select.Items.Count = 0 Then
                    Port_select.Items.Add(myPort(i))
                ElseIf Not Port_select.Items.Contains(myPort(i)) Then
                    Port_select.Items.Add(myPort(i))
                End If
            Next
            Port_select.Sorted = True

        End If
    End Sub

    Private Sub ClearText_Click(sender As Object, e As EventArgs) Handles ClearText.Click
        RichTextBox1.ResetText()
    End Sub
End Class
