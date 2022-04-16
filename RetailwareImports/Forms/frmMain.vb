Public Class frmMain

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = String.Format("Main - {0} Version {1}", My.Application.Info.ProductName, My.Application.Info.Version.ToString)

        If TrimToLower(UseStandardTableManagement) = "yes" Then
            btnTableManagement.Enabled = False
        End If

        If TrimToLower(UseDefaultReadingPrintOut) = "yes" Then
            btnReprintReading.Enabled = False
        End If
    End Sub

    Private Sub btnImportNewItems_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportNewItems.Click
        frmImportNewItems.ShowDialog()
    End Sub

    Private Sub btnTableManagement_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTableManagement.Click
        frmTableManagement.ShowDialog()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Application.Exit()
    End Sub

    Private Sub btnReprintReading_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReprintReading.Click
        frmReprintReading.ShowDialog()
    End Sub
End Class