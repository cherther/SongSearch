"C:\Program Files (x86)\Microsoft SQL Server\90\Tools\Publishing\1.2\sqlpubwiz.exe" script -C "Data Source=(local);Initial Catalog=SongSearch2;Integrated Security=True;" "C:\inetpub\wwwroot\DeploymentScripts\SongSearch2Backup.sql" -targetserver 2008 -f
tar -pvczf SongSearch2Backup.tar.gz SongSearch2Backup.sql
rm SongSearch2Backup.sql