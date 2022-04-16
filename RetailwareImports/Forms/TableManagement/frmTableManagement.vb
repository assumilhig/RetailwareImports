Imports System.IO

Public Class frmTableManagement

    Dim sSQL As String = ""

    Dim inEditMode As Boolean = False
    Dim firstArea As String = ""
    Dim selectedControl As String = ""
    Dim firstSelectedArea As String = ""

    Dim point As New System.Drawing.Point
    Dim X, Y As Integer
    Dim isDrag As Boolean = False
    Dim panelX As Integer
    Dim panelY As Integer

    Private Sub frmTableManagement_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If LayoutPath = "" Then
            LayoutPath = Application.StartupPath & "\Resources\TableManagement\"
        End If

        Try
            sSQL = "ALTER TABLE TM_Areas ADD Terminals nvarchar(30);"
            cnn.Execute(sSQL)
        Catch ex As Exception
        End Try

        Try
            sSQL = "UPDATE TM_Areas SET Terminals = '' WHERE Terminals IS NULL"
            cnn.Execute(sSQL)
        Catch ex As Exception
        End Try

        inEditMode = False
        firstArea = ""
        selectedControl = ""
        firstSelectedArea = ""
        GenerateAreaButton()
        If Not firstArea = "" Then SelectArea(firstArea)
        EnableEditControl(False)

        Try
            sSQL = "CREATE TABLE TransactionHoldNoPC ([TableNo] [nvarchar](10))"
            cnn.Execute(sSQL)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub frmTableManagementUI_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        Dim targetRect As New Rectangle(0, 0, Me.Width, Me.Height)
        Dim brush As New System.Drawing.Drawing2D.LinearGradientBrush(targetRect, Color.RoyalBlue, Color.White, Drawing2D.LinearGradientMode.Vertical)
        e.Graphics.FillRectangle(brush, targetRect)
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Dispose()
    End Sub

    Private Sub btnAddArea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddArea.Click
        Try
            With frmAddEditArea
                .Text = "Add New Area"
                .txtName.Text = ""
                .txtName.Select()
                .txtLayoutPath.Text = ""
                .PictureBox1.BackgroundImage = Nothing
                .btnAdd.Text = "Add"
                .ShowDialog()
            End With
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Message")
        End Try
    End Sub

    Private Sub btnAddTable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddTable.Click
        Try
            Dim area As String = ""
            For Each btn As Button In Me.FlowLayoutPanel1.Controls
                If btn.BackColor = Color.White Then
                    If area = "" Then
                        area = btn.Text
                        Exit For
                    End If
                End If
            Next

            With frmAddEditTable
                .Text = "Add New Table"
                .txtArea.Text = area
                .txtName.Text = ""
                .txtName.Select()
                .txtWidth.Text = 91
                .txtHeight.Text = 65
                .cbInactive.Checked = False
                .cbInactive.Enabled = False
                .btnAdd.Text = "Add"
                .ShowDialog()
            End With
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Message")
        End Try
    End Sub

    Private Sub btnEditMode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditMode.Click
        Try
            If btnEditMode.Text = "Edit Mode" Then
                EnableEditControl(True)
            Else
                sSQL = "DELETE b FROM TM_Areas a JOIN TM_Tables b ON a.ID = b.AreaID WHERE b.Temp = 2"
                cnn.Execute(sSQL)

                sSQL = "DELETE FROM TM_Areas WHERE Temp = 2"
                cnn.Execute(sSQL)

                sSQL = "UPDATE TM_Areas SET Temp = 0"
                cnn.Execute(sSQL)

                sSQL = "UPDATE TM_Tables SET Temp = 0"
                cnn.Execute(sSQL)

                sSQL = "DELETE FROM TM_UpdateControls"
                cnn.Execute(sSQL)

                MsgBox("Records updated successfully!", MsgBoxStyle.Information, "Message")

                EnableEditControl(False)
            End If

            firstArea = ""
            selectedControl = ""
            firstSelectedArea = ""
            GenerateAreaButton()
            SelectArea(firstArea)

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Message")
        End Try
    End Sub

    Private Sub btnRemoveArea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveArea.Click
        Try
            For Each btn As Button In Me.FlowLayoutPanel1.Controls
                If btn.BackColor = Color.White Then
                    sSQL = "UPDATE TM_Areas SET Temp = '2' WHERE Name = '" & btn.Text & "'"
                    cnn.Execute(sSQL)
                End If
            Next

            firstArea = ""
            selectedControl = ""
            firstSelectedArea = ""
            GenerateAreaButton()
            SelectArea(firstArea)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Message")
        End Try
    End Sub

    Private Sub btnRemoveTable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveTable.Click
        Try
            If Me.Panel1.Controls.Count > 0 Then
                Dim areaID As Double = 0

                For Each btn As Button In Me.Panel1.Controls
                    If areaID = 0 Then
                        Dim rsAreaID As New ADODB.Recordset
                        sSQL = "SELECT AreaID FROM TM_Tables WHERE Name = '" & btn.Text & "' GROUP BY AreaID"
                        If rsAreaID.State = 1 Then rsAreaID.Close()
                        rsAreaID.Open(sSQL, cnn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)
                        areaID = CDbl(rsAreaID.Fields("AreaID").Value)
                    End If

                    If btn.BackColor = Color.White Then

                        sSQL = "UPDATE TM_Tables SET Temp = '2' WHERE Name = '" & btn.Text & "'"
                        cnn.Execute(sSQL)
                    End If
                Next

                firstArea = ""
                selectedControl = ""
                firstSelectedArea = ""
                GenerateTableButton(areaID)
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Message")
        End Try
    End Sub

    '######################################## FUNCTIONS ########################################'

    Private Sub GenerateAreaButton()
        Try
            Me.FlowLayoutPanel1.Controls.Clear()

            Dim rsArea As New ADODB.Recordset

            If inEditMode = True Then
                sSQL = "SELECT * FROM TM_Areas WHERE Temp <> '2' ORDER BY OrderNo ASC"
            Else
                sSQL = "SELECT * FROM TM_Areas WHERE Inactive = '0' AND Temp = '0' ORDER BY OrderNo ASC"
            End If

            If rsArea.State = 1 Then rsArea.Close()
            rsArea.Open(sSQL, cnn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

            If Not rsArea.EOF Then
                While Not rsArea.EOF
                    If inEditMode = False Then
                        If IsDBNull(rsArea.Fields("Terminals").Value) Or Trim(rsArea.Fields("Terminals").Value) = "" Then
                            If firstArea = "" Then
                                firstArea = rsArea.Fields("Name").Value
                            End If

                            CreateAreaButton(rsArea.Fields("Name").Value)
                        Else
                            Dim terminals As String = Replace(rsArea.Fields("Terminals").Value, " ", "")
                            Dim xd As New List(Of String)
                            xd.AddRange(terminals.Split(",").ToList)
                            Dim x As String = Trim(sRegisterNo)

                            If xd.Contains(x) Then
                                If firstArea = "" Then
                                    firstArea = rsArea.Fields("Name").Value
                                End If

                                CreateAreaButton(rsArea.Fields("Name").Value)
                            End If
                        End If
                    Else
                        If firstArea = "" Then
                            firstArea = rsArea.Fields("Name").Value
                        End If

                        CreateAreaButton(rsArea.Fields("Name").Value)
                    End If
                    rsArea.MoveNext()
                End While
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Message")
        End Try
    End Sub

    Public Sub CreateAreaButton(ByVal areaName As String)
        Try
            Dim newArea As New Button
            Dim newBold As New Font(newArea.Font.FontFamily, newArea.Font.Size, FontStyle.Bold)

            newArea.Visible = True

            newArea.Font = newBold
            newArea.Text = areaName
            newArea.TextAlign = ContentAlignment.MiddleCenter

            newArea.Size = New Size(198, 40)

            newArea.UseVisualStyleBackColor = True
            newArea.FlatAppearance.BorderSize = 0

            FlowLayoutPanel1.Controls.Add(newArea)

            newArea.ForeColor = Color.Black
            newArea.BackColor = Color.LightSkyBlue
            newArea.FlatStyle = FlatStyle.Standard

            AddHandler newArea.Click, AddressOf AreaButton_Click
            AddHandler newArea.MouseDown, AddressOf AreaButton_MouseDown
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Message")
        End Try
    End Sub

    Public Sub GenerateTableButton(ByVal areaID As Integer)
        Try
            Me.Panel1.Controls.Clear()

            Dim rsTables As New ADODB.Recordset

            If inEditMode = True Then
                sSQL = "SELECT * FROM TM_Tables WHERE AreaID = '" & areaID & "' AND Temp <> '2'"
            Else
                sSQL = "SELECT * FROM TM_Tables WHERE AreaID = '" & areaID & "' AND Inactive = '0' AND Temp = '0'"
            End If

            If rsTables.State = 1 Then rsTables.Close()
            rsTables.Open(sSQL, cnn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

            If Not rsTables.EOF Then
                While Not rsTables.EOF
                    CreateTableButton(rsTables.Fields("Name").Value, rsTables.Fields("LocationX").Value, rsTables.Fields("LocationY").Value, rsTables.Fields("SizeW").Value, rsTables.Fields("SizeH").Value, rsTables.Fields("ImagePath").Value)

                    rsTables.MoveNext()
                End While
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Message")
        End Try
    End Sub

    Public Sub CreateTableButton(ByVal tableName As String, ByVal x As Double, ByVal y As Double, ByVal width As Double, ByVal height As Double, ByVal imagePath As String)
        Try
            Dim newTable As New Button
            Dim newBold As New Font(newTable.Font.FontFamily, newTable.Font.Size, FontStyle.Bold)

            newTable.Visible = True

            newTable.Font = newBold
            newTable.Text = tableName
            newTable.TextAlign = ContentAlignment.MiddleCenter

            newTable.Location = New Point(x, y)
            newTable.Size = New Size(width, height)

            newTable.UseVisualStyleBackColor = True
            newTable.FlatStyle = FlatStyle.Flat
            newTable.ForeColor = Color.Black
            newTable.FlatAppearance.BorderSize = 0

            Panel1.Controls.Add(newTable)
            newTable.BringToFront()

            AddHandler newTable.Click, AddressOf TableButton_Click
            AddHandler newTable.MouseDown, AddressOf TableButton_MouseDown
            AddHandler newTable.MouseMove, AddressOf TableButton_MouseMove
            AddHandler newTable.MouseUp, AddressOf TableButton_MouseUp

            Dim editMode As Boolean = True

            If btnClose.Text = "Close" Then
                editMode = False
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Message")
        End Try
    End Sub

    Private Sub AreaButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Dim areaName As String = Trim(CType(CType(sender, System.Windows.Forms.Button).Text, String))

            SelectArea(areaName)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Message")
        End Try
    End Sub

    Private Sub AreaButton_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Try
            If e.Clicks = 2 Then
                Dim areaName As String = Trim(CType(CType(sender, System.Windows.Forms.Button).Text, String))

                If inEditMode = True Then
                    With frmAddEditArea
                        .Text = "Edit Area"
                        .txtName.Text = areaName
                        .txtName.Select()
                        .LoadAreaData()
                        .btnAdd.Text = "Change"
                        .ShowDialog()
                    End With
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Message")
        End Try
    End Sub

    Private Sub TableButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Dim tableName As String = Trim(CType(CType(sender, System.Windows.Forms.Button).Text, String))

            If inEditMode = True Then
                If tableName.Contains(",") Then
                    MsgBox("Selected Table is currently joined to other table(s)!" & vbCrLf & _
                           "Detached this table first before editing.", MsgBoxStyle.Exclamation, "Message")
                    Exit Sub
                End If

                For Each btn As Button In Me.Panel1.Controls
                    btn.FlatAppearance.BorderSize = 2
                    If btn.Text = tableName Then
                        btn.ForeColor = Color.Red
                        btn.FlatAppearance.BorderColor = Color.Red
                    Else
                        btn.ForeColor = Color.Black
                        btn.FlatAppearance.BorderColor = Color.Black
                    End If
                Next
            Else
                For Each btn As Button In Me.Panel1.Controls
                    If btn.Text = tableName Then

                        btn.ForeColor = Color.Black

                        btn.FlatAppearance.BorderSize = 2
                        btn.FlatAppearance.BorderColor = Color.Yellow
                        btn.BringToFront()
                    Else
                        btn.ForeColor = Color.Black
                        btn.FlatAppearance.BorderSize = 0
                    End If
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Message")
        End Try
    End Sub

    Private Sub TableButton_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Try
            If e.Clicks = 2 Then
                Dim tableName As String = Trim(CType(CType(sender, System.Windows.Forms.Button).Text, String))
                Dim tableWidth As Double = CType(CType(sender, System.Windows.Forms.Button).Width, Double)
                Dim tableHeight As Double = CType(CType(sender, System.Windows.Forms.Button).Height, Double)

                Dim rsCheckSC As New ADODB.Recordset
                sSQL = "SELECT * FROM transactionhold where tableno = '" & Trim(tableName) & "' and isnull(status,'') = '' and chequebankname = 'SC'"
                If rsCheckSC.State = 1 Then rsCheckSC.Close()
                rsCheckSC.Open(sSQL, cnn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

                If Not rsCheckSC.EOF Then
                    MsgBox("To Add additional items to this table. Please remove first Senior/PWD Discount.", MsgBoxStyle.Exclamation, "POS Validation")
                    Exit Sub
                End If

                If inEditMode = True Then
                    If tableName.Contains(",") Then
                        MsgBox("Unable to edit. Selected Table is currently joined to other table(s)!" & vbCrLf & _
                               "Detached this table first before editing.", MsgBoxStyle.Exclamation, "Message")
                        Exit Sub
                    End If
                    Dim area As String = ""
                    For Each btn As Button In Me.FlowLayoutPanel1.Controls
                        If btn.BackColor = Color.White Then
                            If area = "" Then
                                area = btn.Text
                                Exit For
                            End If
                        End If
                    Next

                    With frmAddEditTable
                        .Text = "Edit Table"
                        .txtArea.Text = area
                        .txtName.Text = tableName
                        .txtName.Select()
                        .LoadTableData()
                        .btnAdd.Text = "Change"
                        .ShowDialog()
                    End With
                End If
            Else
                If inEditMode = True Then
                    X = Control.MousePosition.X - CType(sender, System.Windows.Forms.Button).Location.X
                    Y = Control.MousePosition.Y - CType(sender, System.Windows.Forms.Button).Location.Y
                    Label2.Text = "Start Location - Button = X: " & CType(sender, System.Windows.Forms.Button).Location.X & " | Y: " & CType(sender, System.Windows.Forms.Button).Location.Y
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Message")
        End Try
    End Sub

    Private Sub Panel1_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Panel1.MouseMove
        panelX = e.X
        panelY = e.Y
        Label1.Text = "Panel1 Cursor Location = X: " & panelX & " | Y: " & panelY
    End Sub

    Private Sub TableButton_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        If inEditMode = True Then
            If e.Button = Windows.Forms.MouseButtons.Left Then
                isDrag = True
                CType(sender, System.Windows.Forms.Button).Cursor = Cursors.SizeAll
                point = Control.MousePosition
                point.X = point.X - (X)
                point.Y = point.Y - (Y)
                CType(sender, System.Windows.Forms.Button).Location = point
            End If
        End If
    End Sub

    Private Sub TableButton_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        If inEditMode = True Then
            If isDrag = True Then
                Dim tablename As String = CType(sender, System.Windows.Forms.Button).Text
                sSQL = "UPDATE TM_Tables SET LocationX = '" & CType(sender, System.Windows.Forms.Button).Location.X & "', LocationY = '" & CType(sender, System.Windows.Forms.Button).Location.Y & "' WHERE Name = '" & tablename & "'"
                cnn.Execute(sSQL)

                Label3.Text = "End Location - Button = X: " & CType(sender, System.Windows.Forms.Button).Location.X & " | Y: " & CType(sender, System.Windows.Forms.Button).Location.Y
                isDrag = False
            End If
            CType(sender, System.Windows.Forms.Button).Cursor = Cursors.Default
        End If
    End Sub

    Public Sub SelectArea(ByVal areaName As String)
        Try
            Panel1.Visible = False

            Dim rsArea As New ADODB.Recordset
            sSQL = "SELECT * FROM TM_Areas WHERE Name = '" & areaName & "'"
            If rsArea.State = 1 Then rsArea.Close()
            rsArea.Open(sSQL, cnn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

            Try
                For Each btn As Button In Me.FlowLayoutPanel1.Controls
                    If btn.Text = areaName Then
                        btn.ForeColor = Color.Red
                        btn.BackColor = Color.White
                    Else
                        btn.ForeColor = Color.Black
                        btn.BackColor = Color.LightSkyBlue
                    End If
                Next
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

            Me.Panel1.Controls.Clear()

            GenerateTableButton(rsArea.Fields("ID").Value)

            If Trim(rsArea.Fields("LayoutPath").Value) = "" Then
                Panel1.BackgroundImage = Nothing
            Else
                Dim imagePath As String = Trim(rsArea.Fields("LayoutPath").Value)
                Using str As Stream = File.OpenRead(imagePath)
                    Panel1.BackgroundImage = Image.FromStream(str)
                    Panel1.BackgroundImageLayout = ImageLayout.Stretch
                    str.Close()
                End Using
            End If
            Panel1.Visible = True
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Message")
        End Try
    End Sub

    Public Sub EnableEditControl(ByVal opt As Boolean)
        firstArea = ""
        btnAddArea.Visible = opt
        btnAddArea.Enabled = opt
        btnRemoveArea.Visible = opt
        btnRemoveArea.Enabled = opt
        btnAddTable.Visible = opt
        btnAddTable.Enabled = opt
        btnRemoveTable.Visible = opt
        btnRemoveTable.Enabled = opt

        Dim opt2 As Boolean

        If opt = True Then
            opt2 = False
        Else
            opt2 = True
        End If

        Dim ctrl As Control = Nothing
        For i As Integer = Me.Panel1.Controls.Count - 1 To 0
            ctrl = Me.Panel1.Controls(i)
            If ctrl.Name = "frmSplitTable" Then
                Me.Panel1.Controls.RemoveAt(i)
                ctrl.Dispose()
                Exit For
            End If
        Next

        If opt = True Then
            Me.Text = "Table Management - Edit Mode"
            inEditMode = True
            btnEditMode.Text = "Save"
            btnClose.Text = "Cancel"
        Else
            Me.Text = "Table Management"
            inEditMode = False
            btnEditMode.Text = "Edit Mode"
            btnClose.Text = "Close"
        End If

        Me.btnEditMode.Enabled = True
    End Sub
End Class