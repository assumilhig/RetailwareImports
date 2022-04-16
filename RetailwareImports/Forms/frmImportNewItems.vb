Imports System.Text

Public Class frmImportNewItems

    Public sSql As String

    Private Sub frmImportNewItems_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = String.Format("Import New Items - Utility - {0} Version {1}", My.Application.Info.ProductName, My.Application.Info.Version.ToString)

        Try
            sSql = "CREATE TABLE [dbo].[ImportNewItems]( " & _
                "[ID] [bigint] NULL, " & _
                "[Itemcode] [nvarchar](100) NULL, " & _
                "[Description] [nvarchar](500) NULL, " & _
                "[ExtendedDesc] [nvarchar](500) NULL, " & _
                "[Department] [nvarchar](500) NULL, " & _
                "[Category] [nvarchar](250) NULL, " & _
                "[SubCategory] [nvarchar](250) NULL, " & _
                "[Price] [money] NULL, " & _
                "[PriceA] [money] NULL, " & _
                "[PriceB] [money] NULL, " & _
                "[PriceC] [money] NULL, " & _
                "[SCFlag] [smallint] NULL, " & _
                "[RUOM] [nvarchar](100) NULL, " & _
                "[Taxable] [varchar](3) NULL, " & _
                "[PackingQty] [float] NULL, " & _
                "[WUOM] [nvarchar](100) NULL, " & _
                "[DepartmentID] [int] NULL, " & _
                "[CategoryID] [int] NULL, " & _
                "[SubCategoryID] [int] NULL " & _
            ") ON [PRIMARY]"
            cnn.Execute(sSql)
        Catch ex As Exception
        End Try

        Try
            sSql = "CREATE TABLE [dbo].[ImportNewItemFindings]( " & _
                "[Code] [nvarchar](100) NULL, " & _
                "[Validation] [nvarchar](100) NULL, " & _
                "[Result] [float] NULL, " & _
                "[Error] [int] NULL " & _
                ") ON [PRIMARY]"
            cnn.Execute(sSql)
        Catch ex As Exception
        End Try

        Try
            cnn.Execute("DROP PROCEDURE Proc_Import_NewItems")
        Catch ex As Exception
        End Try

        Try
            Dim Proc_Import_NewItems As String = My.Resources.Proc_Import_NewItems.ToString()
            cnn.Execute(Proc_Import_NewItems)
        Catch ex As Exception
        End Try

        Try
            cnn.Execute("DROP PROCEDURE Proc_Import_NewItemsValidate")
        Catch ex As Exception
        End Try

        Try
            Dim Proc_Import_NewItemsValidate As String = My.Resources.Proc_Import_NewItemsValidate.ToString()
            cnn.Execute(Proc_Import_NewItemsValidate)
        Catch ex As Exception
        End Try

        Try
            cnn.Execute("DROP PROCEDURE Proc_Import_NewItemsProcess")
        Catch ex As Exception
        End Try

        Try
            Dim Proc_Import_NewItemsProcess As String = My.Resources.Proc_Import_NewItemsProcess.ToString()
            cnn.Execute(Proc_Import_NewItemsProcess)
        Catch ex As Exception
        End Try

        ValidateNewItems()

    End Sub

    Private Sub btnImportNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportNew.Click
        Dim strFilename As String

        Dim importPath As String = "C:\ImportNewItem"
        Dim importFile As String
        OpenFileDialog1.Filter = "CSV File (*.csv)|*.csv"
        OpenFileDialog1.DefaultExt = "csv"

        Try
            If (OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK) Then

                strFilename = OpenFileDialog1.FileName

                EnsureDirectoryExist(importPath)

                importFile = importPath & "\" & RenameFilename(strFilename)

                My.Computer.FileSystem.CopyFile(strFilename, importFile)

                sSql = "Proc_Import_NewItems '" & importFile & "', '" & strOnlineServerName & "', '" & strOnlineDatabase & "', '" & strOnlineUserName & "', '" & strOnlinePassword & "'"
                cnn.Execute(sSql)

                My.Computer.FileSystem.DeleteFile(importFile)

                ValidateNewItems()
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Message")
        End Try
    End Sub

    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtSearch.Text = ""
        txtSearch.Focus()

        LoadImportNewItems("")
    End Sub

    Private Sub btnFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFind.Click
        Try
            LoadImportNewItems(txtSearch.Text)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Message")
        End Try
    End Sub

    Private Sub txtSearch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSearch.KeyPress
        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 13 Then
            Try
                LoadImportNewItems(txtSearch.Text)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Message")
            End Try
        End If
    End Sub

    Private Sub btnRemoveEntry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveEntry.Click
        Dim ans As Long

        If DataGridView1.Rows.Count = 0 Then
            Exit Sub
        End If

        Try
            ans = MsgBox("Are you sure to remove entry with itemcode - " & Trim(DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(1).Value), MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Import New Item")
            If ans <> 6 Then Exit Sub

            sSql = "DELETE FROM ImportNewItems WHERE Itemcode = '" & Trim(DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(1).Value) & "' AND ID = " & Trim(DataGridView1.Rows(DataGridView1.CurrentRow.Index).Cells(0).Value) & ""
            cnn.Execute(sSql)

            DataGridView1.Rows.RemoveAt(DataGridView1.CurrentRow.Index)

            ValidateNewItems()
        Catch ex As Exception
            MsgBox("Please select an item?", MsgBoxStyle.Exclamation, "Message")
        End Try
    End Sub

    Private Sub btnRemoveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveAll.Click
        Dim ans As Long

        If DataGridView1.Rows.Count = 0 Then
            Exit Sub
        End If

        ans = MsgBox("Are you sure to remove all entries?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Import New Item")
        If ans <> 6 Then Exit Sub

        sSql = "DELETE FROM ImportNewItems"
        cnn.Execute(sSql)

        Try
            ValidateNewItems()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Message")
        End Try
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Dispose()
    End Sub

    Private Sub btnReload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReload.Click
        Try
            ValidateNewItems()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Message")
        End Try
    End Sub

    Private Sub LoadImportNewItems(ByVal Search As String)
        Try
            If Trim(Search) = "" Then
                sSql = "SELECT * FROM ImportNewItems ORDER BY ID"
            Else
                sSql = "SELECT * FROM ImportNewItems WHERE CONCAT(Itemcode,Description,ExtendedDesc,Department,Category,SubCategory) like '%" & Search & "%' ORDER BY ID"
            End If

            LoadDataGridView(sSql, DataGridView1)

            With DataGridView1
                .Columns(1).Frozen = True
                .Columns(2).Frozen = True
                .Columns(1).DefaultCellStyle.BackColor = Color.LightYellow
                .Columns(2).DefaultCellStyle.BackColor = Color.LightYellow
                .Columns("DepartmentID").Visible = False
                .Columns("CategoryID").Visible = False
                .Columns("SubCategoryID").Visible = False
                .Columns(0).Visible = False
            End With

            Label4.Text = "Record Count : " & CDbl(DataGridView1.Rows.Count).ToString("#,##0")

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Message")
        End Try
    End Sub

    Private Sub LoadProductStructure()
        loadDepartment()
        loadCategory("")
        loadSubCategory("")
    End Sub

    Private Sub loadDepartment()
        Try
            sSql = "SELECT ISNULL(DepartmentID,0) AS ID, Department AS OriginalName, Department AS Name FROM ImportNewItems GROUP BY Department, DepartmentID ORDER BY Department"

            LoadDataGridView(sSql, DataGridView2)
            With DataGridView2
                .Columns("ID").Visible = False
                .Columns("OriginalName").Visible = False
            End With
            Label5.Text = "Record Count : " & CDbl(DataGridView2.Rows.Count).ToString("#,##0")
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Message")
        End Try
    End Sub

    Private Sub loadCategory(ByVal Filter As String)
        Try
            If Filter = "" Then
                sSql = "SELECT ISNULL(CategoryID,0) AS ID, Department, Category AS OriginalName, Category AS Name FROM ImportNewItems GROUP BY Department, Category, CategoryID ORDER BY Department, Category"
            Else
                sSql = "SELECT ISNULL(CategoryID,0) AS ID, Department, Category AS OriginalName, Category AS Name FROM ImportNewItems WHERE Department = '" & Filter & "' GROUP BY Department, Category, CategoryID ORDER BY Department, Category"
            End If

            LoadDataGridView(sSql, DataGridView3)
            With DataGridView3
                .Columns("ID").Visible = False
                .Columns("Department").Visible = False
                .Columns("OriginalName").Visible = False
            End With
            Label6.Text = "Record Count : " & CDbl(DataGridView3.Rows.Count).ToString("#,##0")
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Message")
        End Try
    End Sub

    Private Sub loadSubCategory(ByVal Filter As String)
        Try
            If Filter = "" Then
                sSql = "SELECT ISNULL(SubCategoryID,0) AS ID, Category, SubCategory AS OriginalName, SubCategory AS Name FROM ImportNewItems GROUP BY Category, SubCategory, SubCategoryID ORDER BY Category, SubCategory"
            Else
                sSql = "SELECT ISNULL(SubCategoryID,0) AS ID, Category, SubCategory AS OriginalName, SubCategory AS Name FROM ImportNewItems WHERE Category = '" & Filter & "' GROUP BY Category, SubCategory, SubCategoryID ORDER BY Category, SubCategory"
            End If

            LoadDataGridView(sSql, DataGridView4)
            With DataGridView4
                .Columns("ID").Visible = False
                .Columns("Category").Visible = False
                .Columns("OriginalName").Visible = False
            End With
            Label7.Text = "Record Count : " & CDbl(DataGridView4.Rows.Count).ToString("#,##0")
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Message")
        End Try
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        If LinkLabel1.Text = "Edit" Then

            LinkLabel1.Text = "Update"
            LinkLabel1.Location = New Point(518, 639)

            With DataGridView2
                .ReadOnly = False
            End With

        Else
            Try
                Try
                    For i As Integer = 0 To Me.DataGridView1.Rows.Count - 1
                        sSql = "UPDATE ImportNewItems SET Department = '" & Trim(DataGridView2.Rows(i).Cells(2).Value.ToString()) & "' WHERE Department = '" & Trim(DataGridView2.Rows(i).Cells(1).Value.ToString()) & "' AND ISNULL(DepartmentID,0) = 0"
                        cnn.Execute(sSql)
                    Next
                Catch ex As Exception
                End Try

                Try
                    ValidateNewItems()

                Catch ex As Exception
                End Try

                LinkLabel1.Text = "Edit"
                LinkLabel1.Location = New Point(535, 639)
                DataGridView2.ReadOnly = True

            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Message")
            End Try
        End If
    End Sub

    Private Sub LinkLabel2_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        If LinkLabel2.Text = "Edit" Then
            LinkLabel2.Text = "Update"
            LinkLabel2.Location = New Point(806, 639)
            DataGridView3.ReadOnly = False
        Else
            Try
                Try
                    For i As Integer = 0 To Me.DataGridView3.Rows.Count - 1
                        sSql = "UPDATE ImportNewItems SET Category = '" & Trim(DataGridView3.Rows(i).Cells(3).Value.ToString()) & "' WHERE Department = '" & Trim(DataGridView3.Rows(i).Cells(1).Value.ToString()) & "' AND Category = '" & Trim(DataGridView3.Rows(i).Cells(2).Value.ToString()) & "'"
                        cnn.Execute(sSql)

                    Next
                Catch ex As Exception
                End Try

                Try
                    ValidateNewItems()

                Catch ex As Exception
                    MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Message")
                End Try

                LinkLabel2.Text = "Edit"
                LinkLabel2.Location = New Point(823, 639)
                DataGridView3.ReadOnly = True

            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Message")
            End Try
        End If
    End Sub

    Private Sub LinkLabel3_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        If LinkLabel3.Text = "Edit" Then
            LinkLabel3.Text = "Update"
            LinkLabel3.Location = New Point(1101, 576)
            DataGridView4.ReadOnly = False
        Else
            Try
                Try
                    For i As Integer = 0 To Me.DataGridView4.Rows.Count - 1
                        sSql = "UPDATE ImportNewItems SET SubCategory = '" & Trim(DataGridView4.Rows(i).Cells(3).Value.ToString()) & "' WHERE Category = '" & Trim(DataGridView4.Rows(i).Cells(1).Value.ToString()) & "' AND SubCategory = '" & Trim(DataGridView4.Rows(i).Cells(2).Value.ToString()) & "'"
                        cnn.Execute(sSql)

                    Next
                Catch ex As Exception
                End Try

                Try
                    ValidateNewItems()

                Catch ex As Exception
                End Try

                LinkLabel3.Text = "Edit"
                LinkLabel3.Location = New Point(1118, 576)
                DataGridView4.ReadOnly = True

            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Message")
            End Try
        End If
    End Sub

    Private Sub ValidateNewItems()
        Try

            LoadImportNewItems("")

            sSql = "Proc_Import_NewItemsValidate '" & strUseTouchScreen & "'"
            cnn.Execute(sSql)

            sSql = "SELECT * FROM ImportNewItemFindings "
            LoadDataGridView(sSql, DataGridView5)

            With DataGridView5
                .Columns("Code").Visible = False
                .Columns("Error").Visible = False
            End With

            LoadProductStructure()
            UpdateButtonStatus()

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Message")
        End Try
    End Sub

    Private Sub FilterNewItemValidationResult(ByVal FilterBy As String)
        Try

            Select Case Trim(FilterBy)
                Case "01"
                    sSql = "SELECT * FROM ImportNewItems WHERE itemcode in (SELECT itemcode from ImportNewItems GROUP BY itemcode HAVING COUNT(itemcode) > 1) ORDER BY description"
                Case "02"
                    sSql = "SELECT * FROM ImportNewItems WHERE description in (SELECT description from ImportNewItems GROUP BY description HAVING COUNT(description) > 1) ORDER BY description"
                Case "03"
                    sSql = "SELECT * FROM ImportNewItems WHERE Itemcode in (SELECT Itemlookupcode FROM Item)"
                Case "04"
                    sSql = "SELECT * FROM ImportNewItems WHERE ISNULL(Itemcode,'') = ''"
                Case "05"
                    sSql = "SELECT * FROM ImportNewItems WHERE ISNULL(Description,'') = ''"
                Case "06"
                    sSql = "SELECT * FROM ImportNewItems WHERE ISNULL(Department,'') = ''"
                Case "07"
                    sSql = "SELECT * FROM ImportNewItems WHERE ISNULL(Category,'') = ''"
                Case "08"
                    sSql = "SELECT * FROM ImportNewItems WHERE ISNULL(SubCategory,'') = ''"
                Case "09"
                    sSql = "SELECT * FROM ImportNewItems WHERE ISNULL(Price,0) = 0"
                Case "10"
                    sSql = "SELECT * FROM ImportNewItems WHERE LEN(Itemcode) > (select ISNULL(CHARACTER_MAXIMUM_LENGTH,0) AS CHARACTER_MAXIMUM_LENGTH from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'Item' AND COLUMN_NAME = 'ItemLookupCode')"
                Case "11"
                    sSql = "SELECT * FROM ImportNewItems WHERE LEN(Description) > (select ISNULL(CHARACTER_MAXIMUM_LENGTH,0) AS CHARACTER_MAXIMUM_LENGTH from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'Item' AND COLUMN_NAME = 'Description')"
                Case "12"
                    sSql = "SELECT * FROM ImportNewItems WHERE LEN(Department) > (select ISNULL(CHARACTER_MAXIMUM_LENGTH,0) AS CHARACTER_MAXIMUM_LENGTH from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'Department' AND COLUMN_NAME = 'Name')"
                Case "13"
                    sSql = "SELECT * FROM ImportNewItems WHERE LEN(Category) > (select ISNULL(CHARACTER_MAXIMUM_LENGTH,0) AS CHARACTER_MAXIMUM_LENGTH from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'Category' AND COLUMN_NAME = 'Name')"
                Case "14"
                    sSql = "SELECT * FROM ImportNewItems WHERE LEN(SubCategory) > (select ISNULL(CHARACTER_MAXIMUM_LENGTH,0) AS CHARACTER_MAXIMUM_LENGTH from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'SubCategory' AND COLUMN_NAME = 'Name')"
                Case "15"
                    sSql = "SELECT * FROM ImportNewItems WHERE LOWER(Taxable) <> 'yes' and LOWER(Taxable) <> 'no' OR ISNULL(Taxable,'') = ''"
                Case Else
                    Exit Sub
            End Select

            LoadDataGridView(sSql, DataGridView1)

            With DataGridView1
                .Columns(1).Frozen = True
                .Columns(2).Frozen = True
                .Columns(1).DefaultCellStyle.BackColor = Color.LightYellow
                .Columns(2).DefaultCellStyle.BackColor = Color.LightYellow
                .Columns("DepartmentID").Visible = False
                .Columns("CategoryID").Visible = False
                .Columns("SubCategoryID").Visible = False
                .Columns(0).Visible = False
            End With
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Message")
        End Try
    End Sub

    Private Sub DataGridView5_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView5.CellClick
        Try
            Dim ResultDesc As String

            ResultDesc = Trim(DataGridView5.Rows(DataGridView5.CurrentRow.Index).Cells(0).Value.ToString())

            Select Case ResultDesc
                Case "01"
                    FilterNewItemValidationResult("01")
                Case "02"
                    FilterNewItemValidationResult("02")
                Case "03"
                    FilterNewItemValidationResult("03")
                Case "04"
                    FilterNewItemValidationResult("04")
                Case "05"
                    FilterNewItemValidationResult("05")
                Case "06"
                    FilterNewItemValidationResult("06")
                Case "07"
                    FilterNewItemValidationResult("07")
                Case "08"
                    FilterNewItemValidationResult("08")
                Case "09"
                    FilterNewItemValidationResult("09")
                Case "10"
                    FilterNewItemValidationResult("10")
                Case "11"
                    FilterNewItemValidationResult("11")
                Case "12"
                    FilterNewItemValidationResult("12")
                Case "13"
                    FilterNewItemValidationResult("13")
                Case "14"
                    FilterNewItemValidationResult("14")
                Case "15"
                    FilterNewItemValidationResult("15")
                Case Else
                    Exit Sub
            End Select
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Message")
        End Try
    End Sub

    Private Sub UpdateButtonStatus()
        Try
            Dim rs As New ADODB.Recordset

            sSql = "SELECT * FROM ImportNewItemFindings WHERE Error = 1"
            If rs.State = 1 Then rs.Close()
            rs.Open(sSql, cnn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

            If rs.RecordCount > 0 Then
                btnUpdateDatabase.Enabled = False
            Else
                btnUpdateDatabase.Enabled = True
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Message")
        End Try
    End Sub

    Private Sub DataGridView2_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView2.CellClick
        Try
            Dim ResultDesc As String

            ResultDesc = Trim(DataGridView2.Rows(DataGridView2.CurrentRow.Index).Cells(2).Value.ToString())

            loadCategory(ResultDesc)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Message")
        End Try
    End Sub

    Private Sub DataGridView3_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView3.CellClick
        Try
            Dim ResultDesc As String

            ResultDesc = Trim(DataGridView3.Rows(DataGridView3.CurrentRow.Index).Cells(2).Value.ToString())

            loadSubCategory(ResultDesc)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Message")
        End Try
    End Sub

    Private Sub LinkLabel4_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel4.LinkClicked
        If LinkLabel4.Text = "Edit" Then
            LinkLabel4.Text = "Update"
            LinkLabel4.Location = New Point(1101, 31)
            DataGridView1.ReadOnly = False
        Else
            Try
                Try
                    For i As Integer = 0 To Me.DataGridView1.Rows.Count - 1
                        Dim ID As String = Trim(DataGridView1.Rows(i).Cells(0).Value.ToString())
                        
                        sSql = "UPDATE ImportNewItems SET Itemcode = '" & Trim(DataGridView1.Rows(i).Cells(1).Value.ToString()) & "', Description = '" & Trim(DataGridView1.Rows(i).Cells(2).Value.ToString()) & "', ExtendedDesc = '" & Trim(DataGridView1.Rows(i).Cells(3).Value.ToString()) & "', Department = '" & Trim(DataGridView1.Rows(i).Cells(4).Value.ToString()) & "', Category = '" & Trim(DataGridView1.Rows(i).Cells(5).Value.ToString()) & "', SubCategory = '" & Trim(DataGridView1.Rows(i).Cells(6).Value.ToString()) & "', Price = '" & Trim(DataGridView1.Rows(i).Cells(7).Value.ToString()) & "', PriceA = '" & Trim(DataGridView1.Rows(i).Cells(8).Value.ToString()) & "', PriceB = '" & Trim(DataGridView1.Rows(i).Cells(9).Value.ToString()) & "', PriceC = '" & Trim(DataGridView1.Rows(i).Cells(10).Value.ToString()) & "', SCFlag ='" & Trim(DataGridView1.Rows(i).Cells(11).Value.ToString()) & "', RUOM ='" & Trim(DataGridView1.Rows(i).Cells(12).Value.ToString()) & "', Taxable ='" & Trim(DataGridView1.Rows(i).Cells(13).Value.ToString()) & "',PackingQty ='" & Trim(DataGridView1.Rows(i).Cells(14).Value.ToString()) & "', WUOM ='" & Trim(DataGridView1.Rows(i).Cells(15).Value.ToString()) & "' WHERE ID = '" & ID & "'"
                        cnn.Execute(sSql)
                    Next

                Catch ex As Exception
                    MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Message")
                End Try

                Try
                    ValidateNewItems()

                Catch ex As Exception
                End Try

                LinkLabel4.Text = "Edit"
                LinkLabel4.Location = New Point(1118, 31)
                DataGridView1.ReadOnly = True

            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Message")
            End Try
        End If
    End Sub

    Private Sub btnUpdateDatabase_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdateDatabase.Click
        Try
            Dim ans As Long

            ans = MsgBox("Update database?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Are you sure?")
            If ans <> 6 Then Exit Sub

            sSql = "Proc_Import_NewItemsProcess '" & strUseTouchScreen & "'"
            cnn.Execute(sSql)

            MsgBox("Database has been updated with new items.", MsgBoxStyle.Information, "Import New Item")

            Me.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Message")
        End Try
    End Sub
End Class