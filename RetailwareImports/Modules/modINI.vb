﻿Module modINI

    Public Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Long
    Public Declare Unicode Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringW" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Int32, ByVal lpFileName As String) As Int32

    Private Const INI_FILE = "\RW_Config.INI"

    Public sRegisterNo As String = ""

    Public strOnlineServerName As String
    Public strOnlineDatabase As String
    Public strOnlineUserName As String
    Public strOnlinePassword As String
    Public strOnlineTimeout As String

    Public strUseTouchScreen As String = "No"

    Public UseDefaultReadingPrintOut As String = "Yes"

    Public LayoutPath As String = ""
    Public UseStandardTableManagement As String = "Yes"

    Public sStarttime As String = "06:00:00.000"
    Public sEndtime As String = "05:59:59.000"

    Public FileExtension As String = "rws"
    Public FileSave As String = "no"

    Public Sub ReadINIFile()
        Try
            Dim fsoINI As Object
            fsoINI = CreateObject("Scripting.FilesystemObject")

            If fsoINI.FileExists(Application.StartupPath & INI_FILE) Then
                sRegisterNo = INIRead(Application.StartupPath & INI_FILE, "Register", "RegisterNo", "0")

                strOnlineServerName = INIRead(Application.StartupPath & INI_FILE, "Online_Database", "Online_Server", "Unknown")
                strOnlineDatabase = INIRead(Application.StartupPath & INI_FILE, "Online_Database", "Online_Database", "Unknown")
                strOnlineUserName = INIRead(Application.StartupPath & INI_FILE, "Online_Database", "Online_USerName", "Unknown")
                strOnlinePassword = INIRead(Application.StartupPath & INI_FILE, "Online_Database", "Online_Password", "Unknown")
                strOnlineTimeout = INIRead(Application.StartupPath & INI_FILE, "Online_Database", "Online_Timeout ", "Unknown")

                strUseTouchScreen = INIRead(Application.StartupPath & INI_FILE, "ScreenMode", "UseTouchScreen", "No")

                UseStandardTableManagement = INIRead(Application.StartupPath & INI_FILE, "Resto", "UseStandardTableManagement", "yes")
                LayoutPath = INIRead(Application.StartupPath & INI_FILE, "Resto", "LayoutPath", "")

                sStarttime = INIRead(Application.StartupPath & INI_FILE, "POSReading", "StartTime", "06:00:00.000")
                sEndtime = INIRead(Application.StartupPath & INI_FILE, "POSReading", "EndTime", "05:59:59.000")

                FileExtension = INIRead(Application.StartupPath & INI_FILE, "Others", "FileExtension", "rws")
                FileSave = INIRead(Application.StartupPath & INI_FILE, "Others", "FileSave", "no")

                UseDefaultReadingPrintOut = INIRead(Application.StartupPath & INI_FILE, "Reading", "UseDefaultReadingPrintOut", "yes")
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error Message")
        End Try
    End Sub

    Public Function INIRead(ByVal INIPath As String, ByVal SectionName As String, ByVal KeyName As String, ByVal DefaultValue As String) As String
        ' primary version of call gets single value given all parameters 
        Dim n As Int32
        Dim sData As String
        sData = Space$(1024) ' allocate some room 
        n = GetPrivateProfileString(SectionName, KeyName, DefaultValue, _
        sData, sData.Length, INIPath)
        If n > 0 Then ' return whatever it gave us 
            INIRead = sData.Substring(0, n)
        Else
            INIRead = ""
        End If
    End Function

    Public Function WritePreferences(ByVal INISection As String, ByVal INIKey As String, ByVal NewValue As String) As Long
        'This procedure writes the user preferences to an INI file.
        'Example:      Call WritePreferences("GENERAL","EXTENSION","RCD")
        'Return Value: NONE
        Dim lRetVal As Long
        Dim cTemp As String
        cTemp = ""
        'Write Source Folder preferences.
        lRetVal = WritePrivateProfileString(INISection, INIKey, CStr(Trim(NewValue)), Application.StartupPath & "\" & INI_FILE)
        Return lRetVal
    End Function
End Module
