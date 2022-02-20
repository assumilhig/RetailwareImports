<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmImportNewItems
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.btnClose = New System.Windows.Forms.Button
        Me.btnRemoveEntry = New System.Windows.Forms.Button
        Me.DataGridView2 = New System.Windows.Forms.DataGridView
        Me.DataGridView3 = New System.Windows.Forms.DataGridView
        Me.DataGridView4 = New System.Windows.Forms.DataGridView
        Me.btnReload = New System.Windows.Forms.Button
        Me.btnRemoveAll = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.btnImportNew = New System.Windows.Forms.Button
        Me.Label12 = New System.Windows.Forms.Label
        Me.btnUpdateDatabase = New System.Windows.Forms.Button
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.txtSearch = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.btnFind = New System.Windows.Forms.Button
        Me.btnClear = New System.Windows.Forms.Button
        Me.DataGridView5 = New System.Windows.Forms.DataGridView
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel
        Me.LinkLabel2 = New System.Windows.Forms.LinkLabel
        Me.LinkLabel3 = New System.Windows.Forms.LinkLabel
        Me.LinkLabel4 = New System.Windows.Forms.LinkLabel
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(19, 47)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(1124, 265)
        Me.DataGridView1.TabIndex = 0
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(1028, 639)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(107, 30)
        Me.btnClose.TabIndex = 7
        Me.btnClose.Text = "&Exit"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnRemoveEntry
        '
        Me.btnRemoveEntry.Location = New System.Drawing.Point(132, 318)
        Me.btnRemoveEntry.Name = "btnRemoveEntry"
        Me.btnRemoveEntry.Size = New System.Drawing.Size(107, 23)
        Me.btnRemoveEntry.TabIndex = 5
        Me.btnRemoveEntry.Text = "Re&move Entry"
        Me.btnRemoveEntry.UseVisualStyleBackColor = True
        '
        'DataGridView2
        '
        Me.DataGridView2.AllowUserToAddRows = False
        Me.DataGridView2.AllowUserToDeleteRows = False
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Location = New System.Drawing.Point(327, 370)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.Size = New System.Drawing.Size(233, 266)
        Me.DataGridView2.TabIndex = 3
        '
        'DataGridView3
        '
        Me.DataGridView3.AllowUserToAddRows = False
        Me.DataGridView3.AllowUserToDeleteRows = False
        Me.DataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView3.Location = New System.Drawing.Point(578, 369)
        Me.DataGridView3.Name = "DataGridView3"
        Me.DataGridView3.Size = New System.Drawing.Size(270, 266)
        Me.DataGridView3.TabIndex = 4
        '
        'DataGridView4
        '
        Me.DataGridView4.AllowUserToAddRows = False
        Me.DataGridView4.AllowUserToDeleteRows = False
        Me.DataGridView4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView4.Location = New System.Drawing.Point(865, 369)
        Me.DataGridView4.Name = "DataGridView4"
        Me.DataGridView4.Size = New System.Drawing.Size(278, 204)
        Me.DataGridView4.TabIndex = 5
        '
        'btnReload
        '
        Me.btnReload.Location = New System.Drawing.Point(923, 318)
        Me.btnReload.Name = "btnReload"
        Me.btnReload.Size = New System.Drawing.Size(107, 23)
        Me.btnReload.TabIndex = 10
        Me.btnReload.Text = "&Reload / Refresh"
        Me.btnReload.UseVisualStyleBackColor = True
        '
        'btnRemoveAll
        '
        Me.btnRemoveAll.Location = New System.Drawing.Point(19, 318)
        Me.btnRemoveAll.Name = "btnRemoveAll"
        Me.btnRemoveAll.Size = New System.Drawing.Size(107, 23)
        Me.btnRemoveAll.TabIndex = 4
        Me.btnRemoveAll.Text = "Remove &All"
        Me.btnRemoveAll.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Location = New System.Drawing.Point(368, 23)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(84, 13)
        Me.Label4.TabIndex = 15
        Me.Label4.Text = "Recordcount : 0"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Location = New System.Drawing.Point(324, 353)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(84, 13)
        Me.Label5.TabIndex = 16
        Me.Label5.Text = "Recordcount : 0"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Location = New System.Drawing.Point(557, 353)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(84, 13)
        Me.Label6.TabIndex = 17
        Me.Label6.Text = "Recordcount : 0"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Location = New System.Drawing.Point(862, 353)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(84, 13)
        Me.Label7.TabIndex = 18
        Me.Label7.Text = "Recordcount : 0"
        '
        'btnImportNew
        '
        Me.btnImportNew.Location = New System.Drawing.Point(1036, 318)
        Me.btnImportNew.Name = "btnImportNew"
        Me.btnImportNew.Size = New System.Drawing.Size(107, 23)
        Me.btnImportNew.TabIndex = 0
        Me.btnImportNew.Text = "&Import New"
        Me.btnImportNew.UseVisualStyleBackColor = True
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.BackColor = System.Drawing.Color.Transparent
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(16, 353)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(91, 13)
        Me.Label12.TabIndex = 35
        Me.Label12.Text = "Validation Results"
        '
        'btnUpdateDatabase
        '
        Me.btnUpdateDatabase.Location = New System.Drawing.Point(865, 639)
        Me.btnUpdateDatabase.Name = "btnUpdateDatabase"
        Me.btnUpdateDatabase.Size = New System.Drawing.Size(157, 30)
        Me.btnUpdateDatabase.TabIndex = 6
        Me.btnUpdateDatabase.Text = "&Update Database"
        Me.btnUpdateDatabase.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(59, 18)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(189, 20)
        Me.txtSearch.TabIndex = 1
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.BackColor = System.Drawing.Color.Transparent
        Me.Label13.Location = New System.Drawing.Point(16, 21)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(41, 13)
        Me.Label13.TabIndex = 38
        Me.Label13.Text = "Search"
        '
        'btnFind
        '
        Me.btnFind.Location = New System.Drawing.Point(254, 18)
        Me.btnFind.Name = "btnFind"
        Me.btnFind.Size = New System.Drawing.Size(51, 23)
        Me.btnFind.TabIndex = 2
        Me.btnFind.Text = "&Find"
        Me.btnFind.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(311, 18)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(51, 23)
        Me.btnClear.TabIndex = 3
        Me.btnClear.Text = "&Clear"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'DataGridView5
        '
        Me.DataGridView5.AllowUserToAddRows = False
        Me.DataGridView5.AllowUserToDeleteRows = False
        Me.DataGridView5.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView5.Location = New System.Drawing.Point(19, 369)
        Me.DataGridView5.Name = "DataGridView5"
        Me.DataGridView5.Size = New System.Drawing.Size(286, 266)
        Me.DataGridView5.TabIndex = 41
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Location = New System.Drawing.Point(535, 639)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(25, 13)
        Me.LinkLabel1.TabIndex = 9
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "Edit"
        '
        'LinkLabel2
        '
        Me.LinkLabel2.AutoSize = True
        Me.LinkLabel2.Location = New System.Drawing.Point(823, 639)
        Me.LinkLabel2.Name = "LinkLabel2"
        Me.LinkLabel2.Size = New System.Drawing.Size(25, 13)
        Me.LinkLabel2.TabIndex = 10
        Me.LinkLabel2.TabStop = True
        Me.LinkLabel2.Text = "Edit"
        '
        'LinkLabel3
        '
        Me.LinkLabel3.AutoSize = True
        Me.LinkLabel3.Location = New System.Drawing.Point(1118, 576)
        Me.LinkLabel3.Name = "LinkLabel3"
        Me.LinkLabel3.Size = New System.Drawing.Size(25, 13)
        Me.LinkLabel3.TabIndex = 45
        Me.LinkLabel3.TabStop = True
        Me.LinkLabel3.Text = "Edit"
        '
        'LinkLabel4
        '
        Me.LinkLabel4.AutoSize = True
        Me.LinkLabel4.Location = New System.Drawing.Point(1118, 31)
        Me.LinkLabel4.Name = "LinkLabel4"
        Me.LinkLabel4.Size = New System.Drawing.Size(25, 13)
        Me.LinkLabel4.TabIndex = 8
        Me.LinkLabel4.TabStop = True
        Me.LinkLabel4.Text = "Edit"
        '
        'frmImportNewItems
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1155, 676)
        Me.Controls.Add(Me.LinkLabel4)
        Me.Controls.Add(Me.LinkLabel3)
        Me.Controls.Add(Me.LinkLabel2)
        Me.Controls.Add(Me.LinkLabel1)
        Me.Controls.Add(Me.DataGridView5)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.btnFind)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.txtSearch)
        Me.Controls.Add(Me.btnUpdateDatabase)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.btnImportNew)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.btnRemoveAll)
        Me.Controls.Add(Me.btnReload)
        Me.Controls.Add(Me.DataGridView4)
        Me.Controls.Add(Me.DataGridView3)
        Me.Controls.Add(Me.DataGridView2)
        Me.Controls.Add(Me.btnRemoveEntry)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.DataGridView1)
        Me.MaximizeBox = False
        Me.Name = "frmImportNewItems"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Import New Items - Utility"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnRemoveEntry As System.Windows.Forms.Button
    Friend WithEvents DataGridView2 As System.Windows.Forms.DataGridView
    Friend WithEvents DataGridView3 As System.Windows.Forms.DataGridView
    Friend WithEvents DataGridView4 As System.Windows.Forms.DataGridView
    Friend WithEvents btnReload As System.Windows.Forms.Button
    Friend WithEvents btnRemoveAll As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btnImportNew As System.Windows.Forms.Button
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents btnUpdateDatabase As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents btnFind As System.Windows.Forms.Button
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents DataGridView5 As System.Windows.Forms.DataGridView
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabel2 As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabel3 As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabel4 As System.Windows.Forms.LinkLabel
End Class
