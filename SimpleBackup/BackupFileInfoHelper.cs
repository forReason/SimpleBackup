using System.Globalization;

namespace SimpleBackup
{
    /// <summary>
    /// Provides helper methods to generate and retrieve information about backup files.
    /// </summary>
    public static class BackupFileInfoHelper
    {
        /// <summary>
        /// Generates backup file information from a <see cref="FileInfo"/> object.
        /// </summary>
        /// <param name="fileInfo">The FileInfo object from which to generate backup information.</param>
        /// <returns>A <see cref="BackupFileInfo"/> object containing information about the backup.</returns>
        public static BackupFileInfo GenerateInfoFromFile(FileInfo fileInfo)
        {
            return GenerateInfoFromFile(fileInfo.Name);
        }
        /// <summary>
        /// Generates backup file information from a file name.
        /// </summary>
        /// <param name="fileName">The file name from which to generate backup information.</param>
        /// <returns>A <see cref="BackupFileInfo"/> object containing information about the backup.</returns>
        /// <exception cref="FormatException">Thrown when the file name does not contain a valid date in the expected format.</exception>
        public static BackupFileInfo GenerateInfoFromFile(string fileName)
        {
            string baseFileName = fileName;
            while (Path.HasExtension(baseFileName))
            {
                baseFileName = Path.GetFileNameWithoutExtension(baseFileName);
            }
            string[] content = baseFileName.Split('_');
            DateTime backupTime;
            if (!DateTime.TryParseExact(content[0], "yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out backupTime))
            {
                // Handle the case where the string is not in the expected format
                throw new FormatException("The file name does not contain a valid date in the expected format.");
            }
            bool isZipped = content.Contains("zipped");
            bool isEncrypted = content.Contains("encrypted");
            BackupFileInfo info = new(fileName, backupTime, isZipped, isEncrypted);
            return info;
        }
        /// <summary>
        /// Generates backup file information based on provided parameters.
        /// </summary>
        /// <param name="isZipped">Indicates whether the backup is zipped.</param>
        /// <param name="isEncrypted">Indicates whether the backup is encrypted.</param>
        /// <param name="backupTime">The time of the backup. If null, the current time is used.</param>
        /// <returns>A <see cref="BackupFileInfo"/> object containing information about the backup.</returns>
        public static BackupFileInfo GenerateInfo(bool isZipped = false, bool isEncrypted = false, DateTime? backupTime = null)
        {
            DateTime time = DateTime.Now;
            if (backupTime != null)
                time = backupTime.Value;
            string name = time.ToString("yyyy-MM-dd-HH-mm-ss");
            if (isZipped)
                name += "_zipped";
            if (isEncrypted)
                name += "_encrypted";
            if (isZipped)
            {
                name += ".zip";
                if (isEncrypted)
                    name += ".enc";
            }
            return GenerateInfoFromFile(name);
        }
        /// <summary>
        /// Retrieves an array of <see cref="BackupFileInfo"/> objects representing all backups in a given directory.
        /// </summary>
        /// <param name="backupDir">The directory to search for backup files.</param>
        /// <returns>An array of <see cref="BackupFileInfo"/> objects.</returns>
        public static BackupFileInfo[] GetBackupsFromDirectory(DirectoryInfo backupDir)
        {
            List<BackupFileInfo> backups = new List<BackupFileInfo>();
            backupDir.Refresh();
            if (!backupDir.Exists)
                return backups.ToArray();
            // fetch unzipped backups
            foreach (DirectoryInfo dir in backupDir.EnumerateDirectories())
            {
                backups.Add(GenerateInfoFromFile(dir.Name));
            }
            // fetch zipped backups
            foreach (FileInfo file in backupDir.EnumerateFiles())
            {
                backups.Add(GenerateInfoFromFile(file.Name));
            }
            // organize
            BackupFileInfo[] sortedBackups = backups.OrderBy(file => file.BackupTime).ToArray();
            return sortedBackups;
        }

    }
}
