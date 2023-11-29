namespace SimpleBackup
{
    
    /// <summary>
    /// Represents information about a backup file.
    /// </summary>
    public struct BackupFileInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BackupFileInfo"/> struct.
        /// </summary>
        /// <param name="name">The name of the backup file.</param>
        /// <param name="backupTime">The time at which the backup was taken.</param>
        /// <param name="isZipped">Indicates whether the backup is zipped.</param>
        /// <param name="isEncrypted">Indicates whether the backup is encrypted.</param>
        public BackupFileInfo(string name, DateTime backupTime, bool isZipped, bool isEncrypted) 
        { 
            this.Name = name;
            this.BackupTime = backupTime;
            this.IsZipped = isZipped;
            this.IsEncrypted = isEncrypted;
        }
        /// <summary>
        /// Gets the name of the backup file.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the time at which the backup was taken.
        /// </summary>
        public DateTime BackupTime { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the backup is zipped.
        /// </summary>
        public bool IsZipped { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the backup is encrypted.
        /// </summary>
        public bool IsEncrypted { get; private set; }
    }
}
