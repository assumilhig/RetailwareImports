Public Class frmLogin
    Public sSql As String
    Public rsLogin As New ADODB.Recordset
    Public rsConfiguration As New ADODB.Recordset

    Private Sub frmLogin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Text = String.Format("Login - {0} Version {1}", My.Application.Info.ProductName, My.Application.Info.Version.ToString)
        Connect()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Application.Exit()
    End Sub

    Private Sub btnLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogin.Click

        Login()
    End Sub

    Private Sub Login()

        Try
            sSql = "SELECT TOP 1 * FROM Configuration"
            If rsConfiguration.State = 1 Then rsConfiguration.Close()
            rsConfiguration.Open(sSql, cnn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

            If rsConfiguration.EOF Then
                MsgBox("The configuration has not been set up.", MsgBoxStyle.Critical)

                If rsConfiguration.EOF Then Exit Sub
            End If

            sSql = "SELECT ID, FullName, PrivelegesLevel, Inactive FROM [Users] WHERE Username = '" & Trim(txtUsername.Text) & "' AND Password = '" & Trim(txtPassword.Text) & "' AND StoreCode = '" & rsConfiguration.Fields("StoreCode").Value & "' AND Inactive = 0"
            If rsLogin.State = 1 Then rsLogin.Close()
            rsLogin.Open(sSql, cnn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

            If rsLogin.EOF Then
                MsgBox("These credentials do not match our records or the account is inactive", MsgBoxStyle.Exclamation, "Login Message")

                txtPassword.Text = ""
                txtPassword.Focus()

                If rsLogin.EOF Then Exit Sub
            Else
                Me.Hide()
                frmMain.Show()
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
            Exit Sub
        End Try
    End Sub

    Private Sub txtUsername_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtUsername.KeyPress
        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 13 Then
            Login()
        End If
    End Sub

    Private Sub txtPassword_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPassword.KeyPress
        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 13 Then
            Login()
        End If
    End Sub
End Class