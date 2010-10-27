"C:\Program Files (x86)\Microsoft SQL Server\90\Tools\Publishing\1.4\sqlpubwiz.exe" script -C "Data Source=www.worldsongnet.com, 1533;Initial Catalog=SongSearch2;user=deploy;password=W@rehous3;" "C:\DevProjects\SongSearch\SqlScripts\SongSearch2_ProductionBackup.sql" -targetserver 2008 -f
cd SqlScripts
tar -pvczf SongSearch2_ProductionBackup.tar.gz SongSearch2_ProductionBackup.sql
rm SongSearch2_ProductionBackup.sql
cd ..
git add .
git commit -am"Adding latest production db backup script"
git push origin
git push github