"C:\Program Files (x86)\Microsoft SQL Server\90\Tools\Publishing\1.4\sqlpubwiz.exe" script -C "Data Source=(local);Initial Catalog=SongSearch2;Integrated Security=True;" "D:\DevProjects\SongSearch\SqlScripts\SongSearch2.sql" -targetserver 2008 -f
cd SqlScripts
tar -pvczf SongSearch2.tar.gz SongSearch2.sql
rm SongSearch2.sql
cd ..