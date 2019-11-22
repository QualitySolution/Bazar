#!/bin/bash
set -e

cd "$(dirname "$0")"

ProjectName="bazar"
BinDir=../Bazar/bin/Release

# Сборка релиза
msbuild /p:Configuration=Release /p:Platform=x86 ../${ProjectName}.sln

# Очистка от лишних файлов
rm -v -f ${BinDir}/*.mdb
rm -v -f ${BinDir}/*.pdb
rm -v -f -R ./Files/*

mkdir -p Files
cp -r -v ExtraFiles/* ./Files
cp -r -v ${BinDir}/* ./Files

if [ ! -f "gtk-sharp-2.12.21.msi" ]; then
    wget https://xamarin.azureedge.net/GTKforWindows/Windows/gtk-sharp-2.12.21.msi
fi

wine ~/.wine/drive_c/Program\ Files\ \(x86\)/NSIS/makensis.exe /INPUTCHARSET UTF8 ${ProjectName}.nsi
