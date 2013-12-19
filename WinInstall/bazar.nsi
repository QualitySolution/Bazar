
;--------------------------------
!define PRODUCT_VERSION "2.1.0"
!define NETVersion "4.0"
!define NETInstaller "dotNetFx40_Full_setup.exe"

; The name of the installer
Name "QS: База арендаторов"

; The file to write
OutFile "bazar-${PRODUCT_VERSION}.exe"

!include "MUI.nsh"

; The default installation directory
InstallDir $PROGRAMFILES\Базар

; Request application privileges for Windows Vista
RequestExecutionLevel admin

;--------------------------------
; Pages

!insertmacro MUI_PAGE_LICENSE "License.rtf"
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

;--------------------------------
;Languages
 
!insertmacro MUI_LANGUAGE "Russian"

;--------------------------------
; The stuff to install
Section "Программа Базар" SecBazar

  SectionIn RO
  
  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Delete Old files
  Delete $INSTDIR\WidgetLib.dll
  Delete $INSTDIR\MySql.Ddata.Entity.dll

  ; Put file there
  File /r "Files\*.*"
  
  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Bazar" "DisplayName" "База арендаторов Базар"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Bazar" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Bazar" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Bazar" "NoRepair" 1
  WriteUninstaller "uninstall.exe"

  ; Start Menu Shortcuts
  CreateDirectory "$SMPROGRAMS\Базар"
  CreateShortCut "$SMPROGRAMS\Базар\Удаление.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0
  CreateShortCut "$SMPROGRAMS\Базар\Базар.lnk" "$INSTDIR\Bazar.exe" "" "$INSTDIR\Bazar.exe" 0
  CreateShortCut "$SMPROGRAMS\Базар\Документация.lnk" "$INSTDIR\UserGuide_ru.pdf"
  
SectionEnd

Section "MS .NET Framework v${NETVersion}" SecFramework
	SectionIn RO
	InitPluginsDir
	SetOutPath "$pluginsdir\Requires"

  IfFileExists "$WINDIR\Microsoft.NET\Framework\v${NETVersion}*" NETFrameworkInstalled 0
  File ${NETInstaller}
 
	MessageBox MB_OK "Для работы программы необходима платформа .NET Framework ${NETVersion}. Далее будет запущена установка платформы через интернет, если ваш компьютер не подключен к интернету, установите платформу вручную."
  DetailPrint "Starting Microsoft .NET Framework v${NETVersion} Setup..."
  ExecWait "$pluginsdir\Requires\${NETInstaller}"
  Return
 
  NETFrameworkInstalled:
  DetailPrint "Microsoft .NET Framework is already installed!"
 
SectionEnd

Section "GTK# 2.12.21" SecGTK
	SectionIn RO
; Delete 2.12.10
  ExecWait '"msiexec" /X{04AE3BBC-ABFF-42CC-9F90-5B35D229328A} /passive'
; Install 2.12.21
  File "gtk-sharp-2.12.21.msi"
  ExecWait '"msiexec" /i "$pluginsdir\Requires\gtk-sharp-2.12.21.msi"  /passive'
SectionEnd

Section "Ярлык на рабочий стол" SecDesktop

	SetOutPath $INSTDIR
	CreateShortCut "$DESKTOP\Базар.lnk" "$INSTDIR\Bazar.exe" "" "$INSTDIR\Bazar.exe" 0
 
SectionEnd

;--------------------------------
;Descriptions

  ;Language strings
  LangString DESC_SecBazar ${LANG_Russian} "Основные файлы программы Базар"
  LangString DESC_SecFramework ${LANG_Russian} "Для работы программы необходима платформа .NET Framework. При необходимости будет выполнена установка через интернет."
  LangString DESC_SecGTK ${LANG_Russian} "Библиотеки GTK#, необходимые для работы программы"
  LangString DESC_SecDesktop ${LANG_Russian} "Установит ярлык программы на рабочий стол"

  ;Assign language strings to sections
  !insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
    !insertmacro MUI_DESCRIPTION_TEXT ${SecBazar} $(DESC_SecBazar)
    !insertmacro MUI_DESCRIPTION_TEXT ${SecFramework} $(DESC_SecFramework)
    !insertmacro MUI_DESCRIPTION_TEXT ${SecGTK} $(DESC_SecGTK)
    !insertmacro MUI_DESCRIPTION_TEXT ${SecDesktop} $(DESC_SecDesktop)
  !insertmacro MUI_FUNCTION_DESCRIPTION_END

;--------------------------------

; Uninstaller

Section "Uninstall"
  
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Bazar"

  ; Remove files and uninstaller
  Delete $INSTDIR\*
  Delete $INSTDIR\uninstall.exe

  ; Remove shortcuts, if any
  Delete "$SMPROGRAMS\Базар\*.*"
  Delete "$DESKTOP\Базар.lnk"

  ; Remove directories used
  RMDir "$SMPROGRAMS\Базар"
  RMDir "$INSTDIR"

  ; Remove GTK#
  MessageBox MB_YESNO "Удалить библиотеки GTK#? Они были установлены для Базар, но могут использоваться другими приложениями." /SD IDYES IDNO endGTK
    ExecWait '"msiexec" /X{71109D19-D8C1-437D-A6DA-03B94F5187FB} /passive'
  endGTK:
SectionEnd
