;NSIS Modern User Interface
;Quiz Master's Apprentice Script
;Written by Paul Voelker

;--------------------------------
;Include Modern UI

  !include "MUI2.nsh"
  
  !define MUI_ICON ".\QMA.ico"
  !define MUI_UNICON ".\QMAUninstall.ico"

;--------------------------------

  !define APP_NAME "Quiz Masters Apprentice"
  !define APP_FULL_NAME "Quiz Master's Apprentice"

;--------------------------------
;General

  !getdllversion ".\QuizMastersApprenticeApp\bin\Release\net6.0-windows\QMA.exe" MyVer_

  ;Name and file
  Name "Quiz Master's Apprentice v${MyVer_1}.${MyVer_2}"
  OutFile "QMAInstall.exe"

  VIProductVersion "${MyVer_1}.${MyVer_2}.${MyVer_3}.${MyVer_4}"
  VIAddVersionKey ProductName "Quiz Master's Apprentice Install"
  VIAddVersionKey FileVersion "${MyVer_1}.${MyVer_2}.${MyVer_3}.${MyVer_4}"
  VIAddVersionKey ProductVersion "${MyVer_1}.${MyVer_2}.${MyVer_3}.${MyVer_4}"
  VIAddVersionKey FileDescription "Installation Package for Quiz Master's Apprentice"
  VIAddVersionKey LegalCopyright "Copyright © 2022 Paul Voelker"

  ;Default installation folder
  InstallDir "$PROGRAMFILES\${APP_NAME}"
  
  ;Get installation folder from registry if available
  InstallDirRegKey HKCU "Software\${APP_NAME}" ""

  ;Request application privileges for Windows Vista
  RequestExecutionLevel admin

;--------------------------------
;Interface Settings

  !define MUI_ABORTWARNING

;--------------------------------
;Pages

  ;!insertmacro MUI_PAGE_LICENSE "${NSISDIR}\Docs\Modern UI\License.txt"
  ;!insertmacro MUI_PAGE_COMPONENTS
  !insertmacro MUI_PAGE_DIRECTORY
  !insertmacro MUI_PAGE_INSTFILES
  
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES
  
;--------------------------------
;Languages
 
  !insertmacro MUI_LANGUAGE "English"

;--------------------------------
;Installer Sections

Section

  SetOutPath "$INSTDIR"
  
  File .\QuizMastersApprenticeApp\bin\Release\net6.0-windows\*.exe
  File .\QuizMastersApprenticeApp\bin\Release\net6.0-windows\*.dll
  File .\QuizMastersApprenticeApp\bin\Release\net6.0-windows\*.runtimeconfig.json

  SetOutPath "$INSTDIR\Help"

  File .\QuizMastersApprenticeApp\bin\Release\net6.0-windows\Help\*.html
  
  ;Store installation folder
  WriteRegStr HKCU "Software\${APP_NAME}" "" $INSTDIR
  
  ;'Programs and Features' entry (http://nsis.sourceforge.net/Add_uninstall_information_to_Add/Remove_Programs)
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" \
                   "DisplayName" "Quiz Master's Apprentice"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" \
                   "Publisher" "Paul Voelker"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" \
                   "UninstallString" "$\"$INSTDIR\uninstall.exe$\""
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" \
                   "QuietUninstallString" "$\"$INSTDIR\uninstall.exe$\" /S"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" \
                   "DisplayIcon" "$\"$INSTDIR\QMA.exe$\""

  ; Start Menu
  CreateDirectory "$SMPROGRAMS\${APP_NAME}"
  CreateDirectory "$SMPROGRAMS\${APP_NAME}\Help"
  CreateShortCut "$SMPROGRAMS\${APP_NAME}\${APP_NAME}.lnk" "$INSTDIR\QMA.exe"

  ;Create uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe"

SectionEnd

;--------------------------------
;Descriptions

  ;Language strings
  ;LangString DESC_SecDummy ${LANG_ENGLISH} "A test section."

  ;Assign language strings to sections
  ;!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
  ;!insertmacro MUI_DESCRIPTION_TEXT ${SecDummy} $(DESC_SecDummy)
  ;!insertmacro MUI_FUNCTION_DESCRIPTION_END

;--------------------------------
;Uninstaller Section

Section "Uninstall"

  ; Start Menu
  Delete "$SMPROGRAMS\${APP_NAME}\${APP_NAME}.lnk"
  RmDir "$SMPROGRAMS\${APP_NAME}"

  Delete "$INSTDIR\Uninstall.exe"

  Delete "$INSTDIR\*.exe"
  Delete "$INSTDIR\*.dll"
  Delete "$INSTDIR\*.runtimeconfig.json"
  Delete "$INSTDIR\Help\*.html"

  RmDir "$INSTDIR\Help"
  RmDir "$INSTDIR"

  DeleteRegKey /ifempty HKCU "Software\${APP_NAME}"

  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}"

SectionEnd

;--------------------------------

Function .onInit
 
  ReadRegStr $R0 HKLM \
  "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}" \
  "QuietUninstallString"
  StrCmp $R0 "" done
 
  MessageBox MB_OKCANCEL|MB_ICONEXCLAMATION \
  "'${APP_FULL_NAME}' is already installed. $\n$\nClick `OK` to remove the \
  previous version or `Cancel` to cancel this upgrade." \
  IDOK uninst
  Abort
 
;Run the uninstaller
  uninst:
    ClearErrors
    Exec $R0
  done:
 
FunctionEnd

;--------------------------------
; Sign installer and uninstaller

!finalize 'signfile.bat "QMAInstall.exe"'

!uninstfinalize 'signfile.bat "%1"'