@echo off

if [%1]==[] goto usage

set arg1=%1

Rem Convert each .ETL file to .CSV
for %%i in (*.etl) do tracerpt.exe %%~nxi -o %%~ni.csv -of CSV -y

echo.
echo Conversion of .ETL files to .CSV complete
echo.

Rem Parse each .CSV and dump data to SQL table
for %%i in (*.csv) do CsvReader.exe %%~nxi %arg1%

echo.
echo Importing of CSV files into Database complete
echo.
goto :eof

:usage
@echo Usage: %0 ^<MineSite^>
exit /B 1