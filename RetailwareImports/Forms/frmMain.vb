Public Class frmMain

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Text = String.Format("Main - {0} Version {1}", My.Application.Info.ProductName, My.Application.Info.Version.ToString)
    End Sub

    Private Sub btnImportNewItems_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportNewItems.Click

        frmImportNewItems.ShowDialog()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click

        Application.Exit()
    End Sub
End Class