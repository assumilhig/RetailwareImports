Imports System.IO

Public Class frmReprintReading

    Dim rsLoadreading As New ADODB.Recordset
    Public Reprint As Boolean = True
    Public Type, RegisterNo As String
    Public sDateFrom, sToDate As String

    Delegate Sub SetProgressBar_Delegate(ByVal [ProgressBar] As ProgressBar, ByVal [RecordCount] As Integer, ByVal [Count] As Long)

    Private Sub SetProgressBar_ThreadSafe(ByVal [ProgressBar] As ProgressBar, ByVal [RecordCount] As Integer, ByVal [Count] As Long)
        If [ProgressBar].InvokeRequired Then
            Dim MyDelegate As New SetProgressBar_Delegate(AddressOf SetProgressBar_ThreadSafe)
            Me.Invoke(MyDelegate, New Object() {[ProgressBar], [RecordCount], [Count]})
        Else
            With [ProgressBar]
                .Visible = True
                .Minimum = 0
                .Maximum = RecordCount
                .Value = Count
            End With
        End If
    End Sub

    Private Sub frmReprintReading_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        TextBox1.Text = sRegisterNo
        ShowReadingList()
        Me.Enabled = True
    End Sub

    Public Sub ShowReadingList()
        Dim sSQL As String

        sDateFrom = Format(DateTimePicker1.Value, "yyyyMMdd")
        sToDate = Format(DateTimePicker2.Value, "yyyyMMdd")

        If Trim(ComboBox1.Text) = "Cashier Reading" Then
            sSQL = "Select RegisterNo,BatchNo,FullName as Cashier,DateTime,CashierID from Accountability a left outer join users b on a.cashierid = b.id where registerno = '" & Trim(TextBox1.Text) & "' and Reading = 'Cashier' and Convert(char(8),datetime,112) between '" & sDateFrom & "' and '" & sToDate & "' ORDER BY datetime"
            If rsLoadreading.State = 1 Then rsLoadreading.Close()
            rsLoadreading.Open(sSQL, cnn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

            LoadDataGridView(sSQL, DataGridView1)
            With DataGridView1
                .Columns("Registerno").Width = 75
                .Columns("BatchNo").Width = 75
                .Columns("Cashier").Width = 140
                .Columns("DateTime").Width = 150
                .Columns("CashierID").Visible = False
            End With
        ElseIf Trim(ComboBox1.Text) = "Register Reading" Then
            sSQL = "Select RegisterNo,FullName as Cashier,DateTime,CashierID from Accountability a left outer join users b on a.cashierid = b.id where registerno = '" & Trim(TextBox1.Text) & "' and Reading = 'Register' and Convert(char(8),datetime,112) between '" & sDateFrom & "' and '" & sToDate & "' and ReadingFlag = 1 ORDER BY datetime"
            If rsLoadreading.State = 1 Then rsLoadreading.Close()
            rsLoadreading.Open(sSQL, cnn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

            LoadDataGridView(sSQL, DataGridView1)
            With DataGridView1
                .Columns("Registerno").Width = 75
                .Columns("Cashier").Width = 140
                .Columns("DateTime").Width = 150
                .Columns("CashierID").Visible = False
            End With
        ElseIf Trim(ComboBox1.Text) = "Z-Reading" Then
            sSQL = "Select RegisterNo,DBTimeStamp as Datetime,Fullname as Performedby,Status,TransNo_Start,TransNo_End,ControlNo,CashierID from Series a left outer join Users b on a.CashierID = b.id where Registerno = '" & Trim(TextBox1.Text) & "' and Convert(char(8),dbtimestamp,112) between '" & sDateFrom & "' and '" & sToDate & "' and Status = 'Close' ORDER BY dbtimestamp"
            If rsLoadreading.State = 1 Then rsLoadreading.Close()
            rsLoadreading.Open(sSQL, cnn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

            LoadDataGridView(sSQL, DataGridView1)
            With DataGridView1
                .Columns("Registerno").Width = 60
                .Columns("Performedby").Width = 140
                .Columns("DateTime").Width = 120
                .Columns("Status").Width = 60
                .Columns("TransNo_Start").Width = 80
                .Columns("TransNo_End").Width = 80
                .Columns("ControlNo").Width = 80
                .Columns("CashierID").Visible = False
            End With
        End If
    End Sub

    Private Sub btnFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        ShowReadingList()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Dispose()
    End Sub

    Private Sub btnPrintAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintAll.Click
        Type = Trim(ComboBox1.Text)
        RegisterNo = Trim(TextBox1.Text)

        ProgressBarPrinting.Visible = True

        If bgWorkerPrintAll.IsBusy = False Then
            Me.Enabled = False
            bgWorkerPrintAll.RunWorkerAsync()
        End If
    End Sub

    Private Sub createReadingFile(ByVal Directory As String, ByVal Filename As String)
        Dim sSQL As String
        Dim RegisterNo As String = Trim(TextBox1.Text)
        Try
            Dim rsBCP As New ADODB.Recordset
            sSQL = "SELECT [LineNo], RTRIM(Value) as Value FROM Reading_Export WHERE RegisterNo = '" & RegisterNo & "' ORDER BY CAST([LineNo] as int)"
            If rsBCP.State = 1 Then rsBCP.Close()
            rsBCP.Open(sSQL, cnn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

            EnsureDirectoryExist(Directory)
            Filename = Directory & "\" & Filename

            Dim fs As New FileStream(Filename & "." & TrimToLower(FileExtension), FileMode.Append, FileAccess.Write)
            Dim io As New StreamWriter(fs)

            If rsBCP.EOF Then
                io.WriteLine("")
            Else
                While Not rsBCP.EOF
                    io.WriteLine(rsBCP.Fields(1).Value)
                    rsBCP.MoveNext()
                End While
            End If
            io.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Message")
        End Try
    End Sub

    Private Sub bgWorkerPrintAll_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bgWorkerPrintAll.DoWork
        Dim strReprint As Integer = 0
        Dim count As Long = 0

        Dim sSQL As String
        Dim Dir, JournalTransNo As String
        Dim DateFrom, DateTo As String

        If Reprint = True Then strReprint = 1

        sDateFrom = Format(DateTimePicker1.Value, "yyyyMMdd")
        sToDate = Format(DateTimePicker2.Value, "yyyyMMdd")

        Dim rsReading As New ADODB.Recordset

        If Type = "Cashier Reading" Then
            sSQL = "Select RegisterNo, BatchNo, FullName as Cashier ,DateTime, CashierID from Accountability a left outer join users b on a.cashierid = b.id WHERE RegisterNo = '" & RegisterNo & "' and Reading = 'Cashier' and Convert(char(8),datetime,112) between '" & sDateFrom & "' and '" & sToDate & "' ORDER BY datetime"
            If rsReading.State = 1 Then rsReading.Close()
            rsReading.Open(sSQL, cnn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

            Try
                While Not rsReading.EOF
                    Dim DateTime As Object = rsReading.Fields("DateTime").Value

                    count = count + 1
                    DateFrom = Format(DateTime, "yyyy-MM-dd " & sStarttime)
                    DateTo = Format(DateTime.AddDays(1), "yyyy-MM-dd " & sEndtime)

                    sSQL = "Proc_Reading 'X', '" & strReprint & "', '" & RegisterNo & "', '" & rsReading.Fields("BatchNo").Value & "', '" & rsReading.Fields("CashierID").Value & "', '" & DateFrom & "', '" & DateTo & "'"
                    cnn.Execute(sSQL)

                    JournalTransNo = "CashierReading_" & RegisterNo & "_" & rsReading.Fields("BatchNo").Value & "_" & RegisterNo & CDate(DateTime).ToString("yyyy-MM-dd hhmmss")
                    Dir = Application.StartupPath & "\Journal\CashierReading\" & RegisterNo & "\" & Format(DateTime, "yyyy") & "\" & Format(DateTime, "MM")

                    If TrimToLower(FileSave) = "yes" Then
                        createReadingFile(Dir, JournalTransNo)
                    End If

                    SetProgressBar_ThreadSafe(ProgressBarPrinting, rsReading.RecordCount, count)

                    rsReading.MoveNext()
                End While
                MsgBox("Finished extracting of Cashier Reading")
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Message")
            End Try

        ElseIf Type = "Register Reading" Then
            sSQL = "Select RegisterNo, FullName as Cashier, DateTime, CashierID from Accountability a left outer join users b on a.cashierid = b.id where registerno = '" & RegisterNo & "' and Reading = 'Register' and Convert(char(8),datetime,112) between '" & sDateFrom & "' and '" & sToDate & "' and ReadingFlag = 1 ORDER BY datetime"
            If rsReading.State = 1 Then rsReading.Close()
            rsReading.Open(sSQL, cnn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

            Try
                While Not rsReading.EOF
                    Dim DateTime As Object = rsReading.Fields("DateTime").Value

                    count = count + 1
                    DateFrom = Format(DateTime, "yyyy-MM-dd " & sStarttime)
                    DateTo = Format(DateTime.AddDays(1), "yyyy-MM-dd " & sEndtime)

                    sSQL = "Proc_Reading 'Y', '" & strReprint & "', '" & RegisterNo & "', '0', '" & rsReading.Fields("CashierID").Value & "', '" & DateFrom & "', '" & DateTo & "'"
                    cnn.Execute(sSQL)

                    JournalTransNo = "RegisterReading_" & RegisterNo & CDate(DateTime).ToString("yyyy-MM-dd hhmmss")
                    Dir = Application.StartupPath & "\Journal\RegisterReading\" & RegisterNo & "\" & Format(DateTime, "yyyy") & "\" & Format(DateTime, "MM")

                    If TrimToLower(FileSave) = "yes" Then
                        createReadingFile(Dir, JournalTransNo)
                    End If

                    SetProgressBar_ThreadSafe(ProgressBarPrinting, rsReading.RecordCount, count)

                    rsReading.MoveNext()
                End While
                MsgBox("Finished extracting of Register Reading")
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Message")
            End Try
        ElseIf Type = "Z-Reading" Then
            sSQL = "Select * from Series where RegisterNo = '" & RegisterNo & "' and CONVERT(char(8),DBTimeStamp,112) between '" & sDateFrom & "' and '" & sToDate & "' ORDER BY DBTimeStamp"
            If rsReading.State = 1 Then rsReading.Close()
            rsReading.Open(sSQL, cnn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

            Try
                While Not rsReading.EOF
                    Dim DateTime As Object = rsReading.Fields("DBTimeStamp").Value

                    count = count + 1
                    DateFrom = Format(DateTime, "yyyy-MM-dd " & sStarttime)
                    DateTo = Format(DateTime.AddDays(1), "yyyy-MM-dd " & sEndtime)

                    sSQL = "Proc_Reading 'Z', '" & strReprint & "', '" & RegisterNo & "', '0', '" & rsReading.Fields("CashierID").Value & "', '" & DateFrom & "', '" & DateTo & "'"
                    cnn.Execute(sSQL)

                    JournalTransNo = "ZReading_" & RegisterNo & CDate(DateTime).ToString("yyyy-MM-dd hhmmss")
                    Dir = Application.StartupPath & "\Journal\ZReading\" & RegisterNo & "\" & Format(DateTime, "yyyy") & "\" & Format(DateTime, "MM")

                    If TrimToLower(FileSave) = "yes" Then
                        createReadingFile(Dir, JournalTransNo)
                    End If

                    SetProgressBar_ThreadSafe(ProgressBarPrinting, rsReading.RecordCount, count)

                    rsReading.MoveNext()
                End While
                MsgBox("Finished extracting of ZReading")
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Message")
            End Try
        End If
    End Sub

    Private Sub bgWorkerPrintAll_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgWorkerPrintAll.RunWorkerCompleted
        Me.Enabled = True
        ProgressBarPrinting.Visible = False
    End Sub
End Class