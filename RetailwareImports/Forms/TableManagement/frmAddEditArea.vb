Imports System.IO

Public Class frmAddEditArea

    Dim sSQL As String = ""

    Dim controlID As String = ""
    Dim fromControlName As String = ""
    Dim fromControlLayout As String = ""
    Dim toControlName As String = ""
    Dim toControlLayout As String = ""

    Dim areaLayoutPath As String = LayoutPath & "\Area"
    Dim areaLayoutTempPath As String = LayoutPath & "\Area\Temp"

    Private Sub frmAddEditArea_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If My.Computer.FileSystem.DirectoryExists(areaLayoutPath) = False Then
                My.Computer.FileSystem.CreateDirectory(areaLayoutPath)
            End If
            If My.Computer.FileSystem.DirectoryExists(areaLayoutTempPath) = False Then
                My.Computer.FileSystem.CreateDirectory(areaLayoutTempPath)
            End If
            areaLayoutPath = areaLayoutPath & "\"
            areaLayoutTempPath = areaLayoutTempPath & "\"
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Message")
        End Try
    End Sub

    Private Sub frmAddEditArea_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        Dim targetRect As New Rectangle(0, 0, Me.Width, Me.Height)
        Dim brush As New System.Drawing.Drawing2D.LinearGradientBrush(targetRect, Color.RoyalBlue, Color.White, Drawing2D.LinearGradientMode.Vertical)
        e.Graphics.FillRectangle(brush, targetRect)
    End Sub

    Private Sub txtName_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtName.Click
        Dim originalText As String = Trim(txtName.Text)
    End Sub

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        With OpenFileDialog1
            .Title = "Please select an image"
            .FileName = ""
            .Filter = ".PNG Image|*.PNG"
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                txtLayoutPath.Text = .FileName
                PictureBox1.BackgroundImage = Image.FromFile(.FileName)
                PictureBox1.BackgroundImageLayout = ImageLayout.Stretch
                toControlLayout = .FileName
            End If
        End With
    End Sub

    Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        Try
            txtLayoutPath.Text = ""
            PictureBox1.BackgroundImage = Nothing
            toControlLayout = ""
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Message")
        End Try
    End Sub

    Private Sub txtTerminals_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTerminals.Click
        Dim originalText As String = Trim(txtTerminals.Text)
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            ' Split string based on comma
            Dim areas As String() = Trim(Replace(txtName.Text, "'", "")).Split(New Char() {","c})

            ' Use For Each loop over words and display them
            Dim area As String

            For Each area In areas
                toControlName = Trim(Replace(Replace(area, ",", ""), "'", ""))

                If toControlName = "" Then
                    MsgBox("Name should not be empty!", MsgBoxStyle.Exclamation, "Validation")
                    Exit Sub
                End If

                Dim rsArea As New ADODB.Recordset
                sSQL = "SELECT * FROM TM_Areas WHERE Name = '" & toControlName & "'"
                If rsArea.State = 1 Then rsArea.Close()
                rsArea.Open(sSQL, cnn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

                If btnAdd.Text = "Add" Then
                    If Not rsArea.EOF Then
                        MsgBox("Area Name already exist! Please use another name.", MsgBoxStyle.Exclamation, "Validation")
                        Exit Sub
                    End If

                    Dim rsMaxOrderNo As New ADODB.Recordset
                    sSQL = "SELECT ISNULL(MAX(OrderNo),0) + 1 As OrderNo FROM TM_Areas"
                    If rsMaxOrderNo.State = 1 Then rsMaxOrderNo.Close()
                    rsMaxOrderNo.Open(sSQL, cnn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

                    sSQL = "INSERT INTO TM_Areas VALUES " & _
                            "('" & toControlName & "', '', '" & rsMaxOrderNo.Fields("OrderNo").Value & "', '0', '1', '" & Trim(txtTerminals.Text) & "')"
                    cnn.Execute(sSQL)

                    sSQL = "SELECT MAX(ID) As ID FROM TM_Areas"
                    If rsMaxOrderNo.State = 1 Then rsMaxOrderNo.Close()
                    rsMaxOrderNo.Open(sSQL, cnn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

                    Dim newfilename As String = ""

                    If toControlLayout = "" Then
                        newfilename = ""
                    Else
                        newfilename = areaLayoutPath & rsMaxOrderNo.Fields("ID").Value & ".png"
                        My.Computer.FileSystem.CopyFile(toControlLayout, newfilename, True)
                    End If

                    sSQL = "UPDATE TM_Areas SET LayoutPath = '" & newfilename & "' WHERE ID = '" & rsMaxOrderNo.Fields("ID").Value & "'"
                    cnn.Execute(sSQL)

                    sSQL = "UPDATE TM_Areas SET Terminals = '" & Trim(txtTerminals.Text) & "' WHERE ID = '" & rsMaxOrderNo.Fields("ID").Value & "'"
                    cnn.Execute(sSQL)

                    frmTableManagement.CreateAreaButton(toControlName)
                Else
                    If Not fromControlName = toControlName Then
                        If Not rsArea.EOF Then
                            MsgBox("Area Name already exist! Please use another name.", MsgBoxStyle.Exclamation, "Validation")
                            Exit Sub
                        End If
                    End If

                    For Each btn As Button In frmTableManagement.FlowLayoutPanel1.Controls
                        If btn.Text = fromControlName Then
                            btn.Text = toControlName
                        End If
                    Next

                    Dim changeLayout As Boolean = False
                    Dim changeName As Boolean = False

                    If Not toControlLayout = "" Then
                        Dim newfilename As String = areaLayoutTempPath & controlID & ".png"
                        changeLayout = True
                        My.Computer.FileSystem.CopyFile(toControlLayout, newfilename, True)
                        toControlLayout = newfilename
                    End If

                    If Not fromControlName = toControlName Then
                        changeName = True
                    End If

                    Dim rsCheck As New ADODB.Recordset
                    sSQL = "SELECT a.Temp, b.* FROM TM_Areas a JOIN TM_UpdateControls b ON a.ID = b.AreaID WHERE b.AreaID = '" & controlID & "' AND b.TableID IS NULL"
                    If rsCheck.State = 1 Then rsCheck.Close()
                    rsCheck.Open(sSQL, cnn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

                    If changeLayout = True And changeName = True Then

                        If rsCheck.EOF Then
                            sSQL = "INSERT INTO TM_UpdateControls (AreaID, FromControlName, ToControlName, FromLayoutPath, ToLayoutPath, UserID) VALUES " & _
                                    "('" & controlID & "', '" & fromControlName & "', '" & toControlName & "', '" & fromControlLayout & "', '" & toControlLayout & "', '99')"
                        Else
                            sSQL = "UPDATE TM_UpdateControls SET ToControlName = '" & toControlName & "', ToLayoutPath = '" & toControlLayout & "' WHERE AreaID = '" & controlID & "' AND TableID IS NULL"
                        End If
                        cnn.Execute(sSQL)

                        sSQL = "UPDATE TM_Areas SET Name = '" & toControlName & "', LayoutPath = '" & toControlLayout & "' WHERE ID = '" & controlID & "'"
                        cnn.Execute(sSQL)
                    ElseIf changeLayout = True Then
                        If rsCheck.EOF Then
                            sSQL = "INSERT INTO TM_UpdateControls (AreaID, FromLayoutPath, ToLayoutPath, UserID) VALUES " & _
                                    "('" & controlID & "', '" & fromControlLayout & "', '" & toControlLayout & "', '99')"
                        Else
                            sSQL = "UPDATE TM_UpdateControls SET ToLayoutPath = '" & toControlLayout & "' WHERE AreaID = '" & controlID & "' AND TableID IS NULL"
                        End If
                        cnn.Execute(sSQL)

                        sSQL = "UPDATE TM_Areas SET LayoutPath = '" & toControlLayout & "' WHERE ID = '" & controlID & "'"
                        cnn.Execute(sSQL)
                    ElseIf changeName = True Then
                        If rsCheck.EOF Then
                            sSQL = "INSERT INTO TM_UpdateControls (AreaID, FromControlName, ToControlName, UserID) VALUES " & _
                                    "('" & controlID & "', '" & fromControlName & "', '" & toControlName & "', '99')"
                        Else
                            sSQL = "UPDATE TM_UpdateControls SET ToControlName = '" & toControlName & "' WHERE AreaID = '" & controlID & "' AND TableID IS NULL"
                        End If
                        cnn.Execute(sSQL)

                        sSQL = "UPDATE TM_Areas SET Name = '" & toControlName & "' WHERE ID = '" & controlID & "'"
                        cnn.Execute(sSQL)
                    End If

                    If chInactive.Checked = True Then
                        sSQL = "UPDATE TM_Areas SET Inactive = '1' WHERE ID = '" & controlID & "'"
                    Else
                        sSQL = "UPDATE TM_Areas SET Inactive = '0' WHERE ID = '" & controlID & "'"
                    End If
                    cnn.Execute(sSQL)

                    If rsCheck.EOF Then
                        sSQL = "UPDATE TM_Areas SET Temp = '3' WHERE ID = '" & controlID & "'"
                        cnn.Execute(sSQL)
                    Else
                        If CInt(rsCheck.Fields("Temp").Value) <> 1 Then
                            sSQL = "UPDATE TM_Areas SET Temp = '3' WHERE ID = '" & controlID & "'"
                            cnn.Execute(sSQL)
                        End If
                    End If

                    sSQL = "UPDATE TM_Areas SET Terminals = '" & Trim(txtTerminals.Text) & "' WHERE ID = '" & controlID & "'"
                    cnn.Execute(sSQL)

                End If

                frmTableManagement.SelectArea(toControlName)
            Next
            Me.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Message")
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Dispose()
    End Sub

    Public Sub LoadAreaData()
        Try
            Dim areaName As String = Trim(txtName.Text)
            Dim rsArea As New ADODB.Recordset
            sSQL = "SELECT * FROM TM_Areas WHERE Name = '" & areaName & "'"
            If rsArea.State = 1 Then rsArea.Close()
            rsArea.Open(sSQL, cnn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

            controlID = rsArea.Fields("ID").Value
            fromControlName = rsArea.Fields("Name").Value
            fromControlLayout = rsArea.Fields("LayoutPath").Value

            If Trim(rsArea.Fields("LayoutPath").Value) = "" Then
                txtLayoutPath.Text = ""
                PictureBox1.BackgroundImage = Nothing
            Else
                txtLayoutPath.Text = Trim(rsArea.Fields("LayoutPath").Value)

                Using str As Stream = File.OpenRead(txtLayoutPath.Text)
                    PictureBox1.BackgroundImage = Image.FromStream(str)
                    PictureBox1.BackgroundImageLayout = ImageLayout.Stretch
                    str.Close()
                End Using
            End If

            If CDbl(rsArea.Fields("Inactive").Value) = 0 Then
                chInactive.Checked = False
            Else
                chInactive.Checked = True
            End If

            txtTerminals.Text = Trim(rsArea.Fields("Terminals").Value)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Message")
        End Try
    End Sub
End Class