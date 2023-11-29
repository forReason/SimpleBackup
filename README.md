# SimpleBackup
<img src="https://raw.githubusercontent.com/forReason/SimpleBackup/master/SimplebackupLogo.png" width="200" height="200">

SimpleBlackup is an easy to use, simple and lightweight backup and restore library with the following functions:

- Define Source, Backup and Restore Folders
- Plain Backup (Folders and Files)
- Compressed Backup (zip file)
- Encrypted Backup (Password)
- Zipped & Encrypted backup (zipped and then encryped)
- Roll over defining how many backups should be kept in every stage. This minimizes the disk space by only keeping a certain amount of backups, thinning down the older they get (interim, daily, weekly, monthly, yearly).
- Works efficiently, eg zipping & compressing in one go, no temporary storage on disk required

# Setup & Usage
For a simple backup, you need the following:
```
// the path with the files which should be tracked
var sourceDir = new DirectoryInfo(@"Source");
// the path where backups are beeing stored
var backupDir = new DirectoryInfo(@"Backup");
// the path where restored files are created
var restoreDir = new DirectoryInfo(@"Restore");

// create backup class instance
var backup = new SimpleBackup.Backup(sourceDir, backupDir);

// Store backup
backup.StoreBackup();

restore backup to a target Directory
backup.RestoreBackup(backup.BackupFiles[0], restoreDir);
```

This example is fore creating a compressen & encrypted backup
```
var sourceDir = new DirectoryInfo(@"Source");
var backupDir = new DirectoryInfo(@"Backup");
var restoreDir = new DirectoryInfo(@"Restore");
var backup = new SimpleBackup.Backup(sourceDir, backupDir, true, password: "Hello world");
// Act
backup.StoreBackup();
backup.RestoreBackup(backup.BackupFiles[0], restoreDir);
```

# Importing Backups
You can import Backups which you created manually or by other means in the past.
This requires that you have a main folder, where only backups are inside:
```
MyBackups
- Backup 1
- Backup 2
- backup 3
...
```
the name is irrelevant. the Library uses the last Folder write time as backup time.
Make sure to only import the backups once, Behaviour when the backups are already existing is untested 
