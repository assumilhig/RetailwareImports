<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Me.btnImportNewItems = New System.Windows.Forms.Button
        Me.btnExit = New System.Windows.Forms.Button
        Me.btnTableManagement = New System.Windows.Forms.Button
        Me.btnReprintReading = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'btnImportNewItems
        '
        Me.btnImportNewItems.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnImportNewItems.Location = New System.Drawing.Point(12, 12)
        Me.btnImportNewItems.Name = "btnImportNewItems"
        Me.btnImportNewItems.Size = New System.Drawing.Size(135, 38)
        Me.btnImportNewItems.TabIndex = 0
        Me.btnImportNewItems.Text = "&Import New Items"
        Me.btnImportNewItems.UseVisualStyleBackColor = True
        '
        'btnExit
        '
        Me.btnExit.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExit.Location = New System.Drawing.Point(12, 186)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(135, 38)
        Me.btnExit.TabIndex = 1
        Me.btnExit.Text = "&Exit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnTableManagement
        '
        Me.btnTableManagement.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTableManagement.Location = New System.Drawing.Point(153, 12)
        Me.btnTableManagement.Name = "btnTableManagement"
        Me.btnTableManagement.Size = New System.Drawing.Size(135, 38)
        Me.btnTableManagement.TabIndex = 2
        Me.btnTableManagement.Text = "&Table Management"
        Me.btnTableManagement.UseVisualStyleBackColor = True
        '
        'btnReprintReading
        '
        Me.btnReprintReading.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnReprintReading.Location = New System.Drawing.Point(294, 12)
        Me.btnReprintReading.Name = "btnReprintReading"
        Me.btnReprintReading.Size = New System.Drawing.Size(178, 38)
        Me.btnReprintReading.TabIndex = 3
        Me.btnReprintReading.Text = "New &Reprint Reading"
        Me.btnReprintReading.UseVisualStyleBackColor = True
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(484, 236)
        Me.Controls.Add(Me.btnReprintReading)
        Me.Controls.Add(Me.btnTableManagement)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnImportNewItems)
        Me.MaximizeBox = False
        Me.Name = "frmMain"
        Me.ShowIcon = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Main"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnImportNewItems As System.Windows.Forms.Button
    Friend WithEvents btnExit As System.Windows.Forms.Button
    Friend WithEvents btnTableManagement As System.Windows.Forms.Button
    Friend WithEvents btnReprintReading As System.Windows.Forms.Button
End Class
