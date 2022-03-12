Imports System.IO

Module modHelpers

    Public Function RenameFilename(ByVal strFilename As String)

        Return Path.GetFileNameWithoutExtension(strFilename) & "_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & Path.GetExtension(strFilename)
    End Function

    Public Function EnsureDirectoryExist(ByVal Path As String)

        If Not System.IO.Directory.Exists(Path) Then
            System.IO.Directory.CreateDirectory(Path)
        End If

        Return Path
    End Function

    Public Function TrimToLower(ByVal text As String)
        Return text.Trim.ToLower
    End Function

    Public Sub LoadDataGridView(ByVal sql As String, ByRef grid As DataGridView)

        Dim da As New System.Data.OleDb.OleDbDataAdapter()
        Dim ds As New DataSet
        Dim rsRecordset As New ADODB.Recordset

        If rsRecordset.State = 1 Then rsRecordset.Close()
        rsRecordset.Open(sql, cnn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

        da.Fill(ds, rsRecordset, "Data")

        With grid
            .DataSource = Nothing
            .DataSource = ds.Tables(0)

            .ClearSelection()
            .BackgroundColor = Color.White
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .ReadOnly = True
            .Refresh()
        End With
    End Sub
End Module
