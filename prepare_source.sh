#!/bin/bash
echo "Что делаем?"
echo "2) nuget restore"
echo "3) cleanup packages directories"
echo "4) cleanup bin directories"
echo "Можно вызывать вместе, например git+nuget=12"
read case;

case $case in
    *4*)
rm -v -f -R ./Bazar/bin/*
rm -v -f -R ../QSProjects/*/bin/*
rm -v -f -R ../My-FyiReporting/*/bin/*
;;&
    *3*)
rm -v -f -R ./packages/*
rm -v -f -R ../QSProjects/packages/*
rm -v -f -R ../My-FyiReporting/packages/*
;;&
    *2*)
nuget restore bazar.sln;
cd QSProjects;
nuget restore QSProjectsLib.sln;
cd ..;
nuget restore ../My-FyiReporting/MajorsilenceReporting-Linux-GtkViewer.sln
;;&
esac


read -p "Press enter to exit"
