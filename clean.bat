@echo off

echo Clean bin + obj
for /F "tokens=*" %%A in ('dir /AD /B /S bin') do rmdir /S /Q "%%A" 
for /F "tokens=*" %%A in ('dir /AD /B /S obj') do rmdir /S /Q "%%A" 
echo Clean artifacts
for /F "tokens=*" %%A in ('dir /AD /B /S artifacts') do rmdir /S /Q "%%A" 
pause