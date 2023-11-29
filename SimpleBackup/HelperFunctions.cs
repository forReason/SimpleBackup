using System.Security;

namespace SimpleBackup
{
    public class HelperFunctions
    {
        /// <summary>
        /// Copes a directory from a to be allowing to encrypt it as well
        /// </summary>
        /// <remarks>
        /// encryption functionalities are only enables when a password is provided<br/>
        /// when both encrypt and decrypt are active or null, 
        /// it assumes a plain copy because encrypting and decrypting would result in a plain copy
        /// </remarks>
        /// <param name="sourceDir">the directory to copy</param>
        /// <param name="targetDir">the output directory</param>
        /// <param name="password">optional password to encrypt</param>
        /// <param name="encrypt">should the directory be encrypted?</param>
        /// <param name="decrypt">should the directory be decrypted?</param>
        public static void CopyDirectory(
            DirectoryInfo sourceDir, 
            DirectoryInfo targetDir, 
            SecureString? password = null, 
            bool encrypt = false, 
            bool decrypt = false)
        {
            // Make sure the target directory exists
            if (!targetDir.Exists)
            {
                targetDir.Create();
            }

            // Use CrawlDirectory to get all files and their relative paths
            List<(FileInfo File, string RelativePath)> filesToCopy = CrawlDirectory(sourceDir);
            foreach (var (file, relativePath) in filesToCopy)
            {
                // Determine the target file path
                string targetFilePath = Path.Combine(targetDir.FullName, relativePath);

                // Create the directory if it doesn't exist
                DirectoryInfo targetFileDir = new DirectoryInfo(Path.GetDirectoryName(targetFilePath));
                if (!targetFileDir.Exists)
                {
                    targetFileDir.Create();
                }

                if (password == null || encrypt == decrypt) // either both are false (no change) or encrypt and then decrypt back (no change)
                    file.CopyTo(targetFilePath, overwrite: true);
                else if (decrypt)
                {
                    FlowEncrypt.EncryptFiles.Decrypt(file.FullName, targetFilePath, password);
                }
                else
                {
                    FlowEncrypt.EncryptFiles.Encrypt(file.FullName, targetFilePath, password);
                }
            }
        }
        /// <summary>
        /// Crawls a directory in order to retrieve all files and folders to be backed up in it
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        internal static List<(FileInfo File, string RelativePath)> CrawlDirectory(DirectoryInfo directory)
        {
            Queue<(DirectoryInfo Dir, string BasePath)> directories = new Queue<(DirectoryInfo, string)>();
            directories.Enqueue((directory, string.Empty));
            var filesList = new List<(FileInfo, string)>();

            while (directories.Count > 0)
            {
                var (currentDir, basePath) = directories.Dequeue();

                foreach (FileInfo file in currentDir.GetFiles())
                {
                    string relativePath = Path.Combine(basePath, file.Name);
                    filesList.Add((file, relativePath));
                }

                foreach (DirectoryInfo subDir in currentDir.GetDirectories())
                {
                    directories.Enqueue((subDir, Path.Combine(basePath, subDir.Name)));
                }
            }

            return filesList;
        }
    }
}
