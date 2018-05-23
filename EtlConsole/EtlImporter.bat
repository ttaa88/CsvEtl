@echo off

Rem Convert each .ETL file to .CSV
for %%i in (*.etl) do tracerpt.exe %%~nxi -o %%~ni.csv -of CSV -y

Rem Parse each .CSV and dump data to SQL table
Rem TODO: Check if Dataset name exists before processing each file
