Imports System.Data.SqlClient

Module modConnection

    Public cnn As New ADODB.Connection
    Public rsLogin As New ADODB.Recordset
    Public rsConfiguration As New ADODB.Recordset
    'Public con As SqlConnection

    Public Sub Connect()

        ReadINIFile()

        Try
            If cnn.State = 1 Then cnn.Close()
            cnn.CommandTimeout = strOnlineTimeout

            cnn.Open("Provider=SQLOLEDB.1;Password=" & strOnlinePassword & ";Persist Security Info=True;User ID=" & strOnlineUserName & ";Initial Catalog=" & strOnlineDatabase & ";Data Source=" & strOnlineServerName & "")
            cnn.CursorLocation = ADODB.CursorLocationEnum.adUseClient

            'con = New SqlConnection("server=" & strOnlineServerName & ";uid=" & strOnlineUserName & ";pwd= " & strOnlinePassword & ";database=" & strOnlineDatabase & "")
            'con.Open()

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub
End Module
