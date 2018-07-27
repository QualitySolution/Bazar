#!/bin/bash
set -e

ProjectName="bazar"
BinDir=../newaprashka/bin/Release

# Сборка релиза
msbuild /p:Configuration=Release /p:Platform=x86 ../${ProjectName}.sln

# Очистка от лишних файлов
rm -v -f ${BinDir}/*.mdb
rm -v -f ${BinDir}/*.pdb
rm -v -f -R ./Files/*

cp -r -v ExtraFiles/* ./Files
cp -r -v ${BinDir}/* ./Files

wine ~/.wine/drive_c/Program\ Files\ \(x86\)/NSIS/makensis.exe /INPUTCHARSET UTF8 ${ProjectName}.nsi
