Imports System.IO

Public Class frmAddEditTable

    Dim sSQL As String = ""

    Dim tableID As String = ""
    Dim areaID As String = ""
    Dim fromTableName As String = ""
    Dim fromWidth As String = ""
    Dim fromHeight As String = ""
    Dim fromControlLayout As String = ""
    Dim toControlLayout As String = ""

    Private Sub frmAddEditTable_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        Dim targetRect As New Rectangle(0, 0, Me.Width, Me.Height)
        Dim brush As New System.Drawing.Drawing2D.LinearGradientBrush(targetRect, Color.RoyalBlue, Color.White, Drawing2D.LinearGradientMode.Vertical)
        e.Graphics.FillRectangle(brush, targetRect)
    End Sub

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        With OpenFileDialog1
            .Title = "Please select an image"
            .FileName = ""
            .Filter = ".PNG Image|*.PNG"
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                Dim filename As String = .FileName.ToString()
                PictureBox1.BackgroundImage = Nothing
                PictureBox1.BackgroundImage = Image.FromFile(filename)
                PictureBox1.BackgroundImageLayout = ImageLayout.Stretch
                toControlLayout = filename
            End If
        End With
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            Dim areaName As String = Trim(Replace(Replace(txtArea.Text, ",", ""), "'", ""))

            ' Split string based on comma
            Dim tables As String() = Trim(Replace(txtName.Text, "'", "")).Split(New Char() {","c})

            ' Use For Each loop over words and display them
            Dim table As String

            For Each table In tables
                Dim tableName As String = Trim(Replace(Replace(table, ",", ""), "'", ""))
                Dim width As String = Trim(Replace(txtWidth.Text, ",", ""))
                Dim height As String = Trim(Replace(txtHeight.Text, ",", ""))
                Dim fields As String = "Required Fields : "

                If tableName = "" Then fields = fields & vbCrLf & "Table Name"
                If width = "" Then fields = fields & vbCrLf & "Width"
                If height = "" Then fields = fields & vbCrLf & "Height"

                If Not fields = "Required Fields : " Then
                    MsgBox(fields, MsgBoxStyle.Exclamation, "Validation")
                    Exit Sub
                End If

                If IsNumeric(width) = False Then
                    MsgBox("Invalid Width value!", MsgBoxStyle.Exclamation, "Validation")
                    Exit Sub
                End If

                If IsNumeric(height) = False Then
                    MsgBox("Invalid Height value!", MsgBoxStyle.Exclamation, "Validation")
                    Exit Sub
                End If

                If CDbl(width) < 40 Or CDbl(height) < 40 Then
                    MsgBox("Width and Height should not be less than 40 pixel!", MsgBoxStyle.Exclamation, "Validation")
                    Exit Sub
                End If

                Dim rsCheckName As New ADODB.Recordset
                sSQL = "SELECT ID FROM TM_Tables WHERE Name = '" & tableName & "'"
                If rsCheckName.State = 1 Then rsCheckName.Close()
                rsCheckName.Open(sSQL, cnn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

                If btnAdd.Text = "Add" Then

                    If Not rsCheckName.EOF Then
                        MsgBox("Table Name already exist! Please use another name.", MsgBoxStyle.Exclamation, "Validation")
                        Exit Sub
                    End If

                    Dim rsAreaID As New ADODB.Recordset
                    sSQL = "SELECT ID FROM TM_Areas WHERE Name = '" & areaName & "'"
                    If rsAreaID.State = 1 Then rsAreaID.Close()
                    rsAreaID.Open(sSQL, cnn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

                    Dim rsOrdeNo As New ADODB.Recordset
                    sSQL = "SELECT ISNULL(MAX(OrderNo),0) + 1 As OrderNo FROM TM_Tables WHERE AreaID = '" & rsAreaID.Fields("ID").Value & "'"
                    If rsOrdeNo.State = 1 Then rsOrdeNo.Close()
                    rsOrdeNo.Open(sSQL, cnn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

                    sSQL = "INSERT INTO TM_Tables VALUES " & _
                            "('" & rsAreaID.Fields("ID").Value & "', '" & tableName & "', '10', '10', " & _
                            "'" & width & "', '" & height & "', '', '" & rsOrdeNo.Fields("OrderNo").Value & "', '0', '1')"
                    cnn.Execute(sSQL)

                    Dim rsMaxOrderNo As New ADODB.Recordset
                    sSQL = "SELECT MAX(ID) As ID FROM TM_Tables"
                    If rsMaxOrderNo.State = 1 Then rsMaxOrderNo.Close()
                    rsMaxOrderNo.Open(sSQL, cnn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

                    Dim newfilename As String = ""

                    If toControlLayout = "" Then
                        newfilename = ""
                    Else
                        newfilename = LayoutPath & "\Table\" & rsMaxOrderNo.Fields("ID").Value & ".png"

                        My.Computer.FileSystem.CopyFile(toControlLayout, newfilename, True)
                    End If

                    sSQL = "UPDATE TM_Tables SET ImagePath = '" & newfilename & "' WHERE ID = '" & rsMaxOrderNo.Fields("ID").Value & "'"
                    cnn.Execute(sSQL)

                    frmTableManagement.CreateTableButton(tableName, 10, 10, CDbl(width), CDbl(height), newfilename)
                Else
                    If Not fromTableName = tableName Then
                        If Not rsCheckName.EOF Then
                            MsgBox("Table Name already exist! Please use another name.", MsgBoxStyle.Exclamation, "Validation")
                            Exit Sub
                        End If
                    End If

                    Dim rsCheck As New ADODB.Recordset
                    sSQL = "SELECT a.Temp, b.* FROM TM_Tables a JOIN TM_UpdateControls b  ON a.ID = b.TableID AND a.AreaID = b.AreaID WHERE b.AreaID = '" & areaID & "' AND b.TableID = '" & tableID & "'"
                    If rsCheck.State = 1 Then rsCheck.Close()
                    rsCheck.Open(sSQL, cnn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

                    If rsCheck.EOF Then
                        sSQL = "INSERT INTO TM_UpdateControls (AreaID, TableID, FromControlName, ToControlName, FromWidth, ToWidth, FromHeight, ToHeight, UserID) VALUES " & _
                                "('" & areaID & "', '" & tableID & "', '" & fromTableName & "', '" & tableName & "', " & _
                                "'" & fromWidth & "', '" & width & "', '" & fromHeight & "', '" & height & "', '99')"
                    Else
                        sSQL = "UPDATE TM_UpdateControls SET ToControlName = '" & tableName & "', ToWidth = '" & width & "', ToHeight = '" & height & "' " & _
                                "WHERE AreaID = '" & areaID & "' AND TableID = '" & tableID & "'"
                    End If
                    cnn.Execute(sSQL)

                    Dim inactive As Integer = 0

                    If cbInactive.Checked = True Then
                        inactive = 1
                    End If

                    sSQL = "UPDATE TM_Tables SET Name = '" & tableName & "', SizeW = '" & width & "', SizeH = '" & height & "', Inactive = '" & inactive & "' WHERE ID = '" & tableID & "' AND AreaID = '" & areaID & "'"
                    cnn.Execute(sSQL)

                    If rsCheck.EOF Then
                        sSQL = "UPDATE TM_Tables SET Temp = '3' WHERE ID = '" & tableID & "' AND AreaID = '" & areaID & "'"
                        cnn.Execute(sSQL)
                    Else
                        If CInt(rsCheck.Fields("Temp").Value) <> 1 Then
                            sSQL = "UPDATE TM_Tables SET Temp = '3' WHERE ID = '" & tableID & "' AND AreaID = '" & areaID & "'"
                            cnn.Execute(sSQL)
                        End If
                    End If

                    Dim newfilename As String = ""

                    If toControlLayout = "" Then
                        newfilename = ""
                    Else
                        newfilename = LayoutPath & "\Table\" & tableID & ".png"
                        My.Computer.FileSystem.CopyFile(toControlLayout, newfilename, True)
                    End If

                    sSQL = "UPDATE TM_Tables SET ImagePath = '" & newfilename & "' WHERE ID = '" & tableID & "'"
                    cnn.Execute(sSQL)

                    frmTableManagement.GenerateTableButton(areaID)
                End If
            Next
            Me.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Message")
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Dispose()
    End Sub

    Public Sub LoadTableData()
        Try
            Dim tableName As String = Trim(txtName.Text)

            Dim rsTable As New ADODB.Recordset
            sSQL = "SELECT * FROM TM_Tables WHERE Name = '" & tableName & "'"
            If rsTable.State = 1 Then rsTable.Close()
            rsTable.Open(sSQL, cnn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

            tableID = rsTable.Fields("ID").Value
            areaID = rsTable.Fields("AreaID").Value
            fromTableName = rsTable.Fields("Name").Value
            fromWidth = rsTable.Fields("SizeW").Value
            fromHeight = rsTable.Fields("SizeH").Value
            fromControlLayout = rsTable.Fields("ImagePath").Value

            If Trim(fromControlLayout) = "" Then
                PictureBox1.BackgroundImage = Nothing
            Else
                If My.Computer.FileSystem.FileExists(fromControlLayout) = True Then
                    Using str As Stream = File.OpenRead(fromControlLayout)
                        PictureBox1.BackgroundImage = Image.FromStream(str)
                        PictureBox1.BackgroundImageLayout = ImageLayout.Stretch
                        str.Close()
                    End Using

                Else
                    PictureBox1.BackgroundImage = Nothing
                End If
            End If

            txtWidth.Text = fromWidth
            txtHeight.Text = fromHeight
            cbInactive.Enabled = True

            If CInt(rsTable.Fields("Inactive").Value) = 0 Then
                cbInactive.Checked = False
            Else
                cbInactive.Checked = True
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Message")
        End Try
    End Sub
End Class