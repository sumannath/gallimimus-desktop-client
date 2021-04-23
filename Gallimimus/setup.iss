; -- gallimimus.iss --
; Create executable for gallimimus.

[Setup]
AppName=Gallimimus
AppVersion=0.1
WizardStyle=modern
CreateAppDir=no
DisableProgramGroupPage=yes
DefaultDirName={tmp}\Gallimimus
Compression=lzma2
SolidCompression=yes
Uninstallable=no
CreateUninstallRegKey=no
DisableWelcomePage=yes
DisableFinishedPage=yes
OutputBaseFilename=gallimimus

[Files]
Source: "bin\Release\netcoreapp3.1\publish\Gallimimus.exe"; DestDir: "{tmp}\Gallimimus"
Source: "appsettings.json"; DestDir: "{tmp}\Gallimimus\"

[Run]
Filename: "{tmp}\Gallimimus\Gallimimus.exe"; Flags: hidewizard

[Code]
procedure CurPageChanged(CurPageID: Integer);
begin
  if CurPageID = wpReady then
    WizardForm.NextButton.Caption := 'Run'
  else
    WizardForm.NextButton.Caption := SetupMessage(msgButtonNext);
end;

