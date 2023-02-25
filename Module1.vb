Module Module1
    Private Conn As New SqlClient.SqlConnection
    Private cmd As SqlClient.SqlCommand

    Sub Main()
        'Dim mTimer As System.Timers.Timer

        'Conn.ConnectionString = "server=118.69.81.103;uid=user_ras;pwd=VietHealthy@170172#;database=RAS12"
        'Conn.Open()
        'cmd = Conn.CreateCommand

        RestartAOPs()

        'mTimer = New System.Timers.Timer(600000)
        'AddHandler mTimer.Elapsed, AddressOf OnTimedEvent
        'mTimer.AutoReset = True
        'mTimer.Enabled = True
        'Console.WriteLine("AUTO RUN AOP PROGRAMS")
        'Console.WriteLine("Press Enter to Exit!")
        'Console.ReadLine()
    End Sub

    Sub OnTimedEvent(source As Object, e As EventArgs)
        Console.WriteLine(String.Format("{0}: Check AopQueue", {Format(Now), "dd-MMM-yyyy hh:mm"}))
        cmd.CommandText = "select count(*) from AopQueue where Status='OK'"
        If cmd.ExecuteScalar < 100 And Process.GetProcessesByName("ImportAop").Count > 0 Then Exit Sub

        Console.WriteLine(String.Format("{0}: Restart Aop programs", {Format(Now), "dd-MMM-yyyy hh:mm"}))
        RestartAOPs()
    End Sub

    Private Sub RestartAOPs()
        Dim mStr, mAopAppLinks(), mArrStr(), mAopApp As String
        Dim i, j As Integer

        mStr = My.Computer.FileSystem.ReadAllText(AppDomain.CurrentDomain.BaseDirectory & "\AopApps.txt")
        mAopAppLinks = Split(mStr, vbLf)
        For i = 0 To mAopAppLinks.Length - 1
            If mAopAppLinks(i) = "" Then Continue For

            mArrStr = Split(mAopAppLinks(i), "\")
            mAopApp = mArrStr(mArrStr.Length - 1)

            For j = Process.GetProcessesByName(mAopApp).Count - 1 To 0 Step -1
                Process.GetProcessesByName(mAopApp)(j).Kill()
            Next
        Next

        For i = 0 To mAopAppLinks.Length - 1
            mArrStr = Split(mAopAppLinks(i), "\")
            mAopApp = mArrStr(mArrStr.Length - 1)
            Process.Start(mAopAppLinks(i))
            If i = 0 Then
                Threading.Thread.Sleep(20000)

                AppActivate(Process.GetProcessesByName(mAopApp)(0).Id)
                My.Computer.Keyboard.SendKeys("{Esc}", True)
                AppActivate(Process.GetProcessesByName(mAopApp)(0).Id)
                My.Computer.Keyboard.SendKeys("{Enter}", True)
                AppActivate(Process.GetProcessesByName(mAopApp)(0).Id)
                My.Computer.Keyboard.SendKeys("Abcd@1234", True)
                My.Computer.Keyboard.SendKeys("{Enter}", True)
                Process.GetProcessesByName(mAopApp)(0).WaitForInputIdle()

                Threading.Thread.Sleep(10000)
            End If
        Next
    End Sub
End Module
