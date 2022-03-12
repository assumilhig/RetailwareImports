<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTableManagement
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
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.btnAddArea = New System.Windows.Forms.Button
        Me.btnRemoveArea = New System.Windows.Forms.Button
        Me.btnAddTable = New System.Windows.Forms.Button
        Me.btnRemoveTable = New System.Windows.Forms.Button
        Me.btnEditMode = New System.Windows.Forms.Button
        Me.btnClose = New System.Windows.Forms.Button
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.BackColor = System.Drawing.Color.Transparent
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.SplitContainer1.Panel1.Controls.Add(Me.FlowLayoutPanel1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label3)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label2)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label1)
        Me.SplitContainer1.Panel2.Controls.Add(Me.Panel1)
        Me.SplitContainer1.Panel2.Controls.Add(Me.btnAddArea)
        Me.SplitContainer1.Panel2.Controls.Add(Me.btnRemoveArea)
        Me.SplitContainer1.Panel2.Controls.Add(Me.btnAddTable)
        Me.SplitContainer1.Panel2.Controls.Add(Me.btnRemoveTable)
        Me.SplitContainer1.Panel2.Controls.Add(Me.btnEditMode)
        Me.SplitContainer1.Panel2.Controls.Add(Me.btnClose)
        Me.SplitContainer1.Size = New System.Drawing.Size(1008, 730)
        Me.SplitContainer1.SplitterDistance = 235
        Me.SplitContainer1.TabIndex = 1
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.AutoScroll = True
        Me.FlowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Padding = New System.Windows.Forms.Padding(3)
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(235, 730)
        Me.FlowLayoutPanel1.TabIndex = 0
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 614)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(39, 13)
        Me.Label3.TabIndex = 19
        Me.Label3.Text = "Label3"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 596)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(39, 13)
        Me.Label2.TabIndex = 18
        Me.Label2.Text = "Label2"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 577)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 13)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Label1"
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(769, 571)
        Me.Panel1.TabIndex = 16
        '
        'btnAddArea
        '
        Me.btnAddArea.Location = New System.Drawing.Point(520, 577)
        Me.btnAddArea.Name = "btnAddArea"
        Me.btnAddArea.Size = New System.Drawing.Size(75, 50)
        Me.btnAddArea.TabIndex = 14
        Me.btnAddArea.Text = "Add Area"
        Me.btnAddArea.UseVisualStyleBackColor = True
        '
        'btnRemoveArea
        '
        Me.btnRemoveArea.Location = New System.Drawing.Point(520, 633)
        Me.btnRemoveArea.Name = "btnRemoveArea"
        Me.btnRemoveArea.Size = New System.Drawing.Size(75, 50)
        Me.btnRemoveArea.TabIndex = 13
        Me.btnRemoveArea.Text = "Remove Area"
        Me.btnRemoveArea.UseVisualStyleBackColor = True
        '
        'btnAddTable
        '
        Me.btnAddTable.Location = New System.Drawing.Point(601, 577)
        Me.btnAddTable.Name = "btnAddTable"
        Me.btnAddTable.Size = New System.Drawing.Size(75, 50)
        Me.btnAddTable.TabIndex = 12
        Me.btnAddTable.Text = "Add Table"
        Me.btnAddTable.UseVisualStyleBackColor = True
        '
        'btnRemoveTable
        '
        Me.btnRemoveTable.Location = New System.Drawing.Point(601, 633)
        Me.btnRemoveTable.Name = "btnRemoveTable"
        Me.btnRemoveTable.Size = New System.Drawing.Size(75, 50)
        Me.btnRemoveTable.TabIndex = 11
        Me.btnRemoveTable.Text = "Remove Table"
        Me.btnRemoveTable.UseVisualStyleBackColor = True
        '
        'btnEditMode
        '
        Me.btnEditMode.Location = New System.Drawing.Point(682, 577)
        Me.btnEditMode.Name = "btnEditMode"
        Me.btnEditMode.Size = New System.Drawing.Size(75, 50)
        Me.btnEditMode.TabIndex = 9
        Me.btnEditMode.Text = "Edit Mode"
        Me.btnEditMode.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(682, 633)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 50)
        Me.btnClose.TabIndex = 8
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'frmTableManagement
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1008, 730)
        Me.ControlBox = False
        Me.Controls.Add(Me.SplitContainer1)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmTableManagement"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Table Management"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.Panel2.PerformLayout()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnAddArea As System.Windows.Forms.Button
    Friend WithEvents btnRemoveArea As System.Windows.Forms.Button
    Friend WithEvents btnAddTable As System.Windows.Forms.Button
    Friend WithEvents btnRemoveTable As System.Windows.Forms.Button
    Friend WithEvents btnEditMode As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
