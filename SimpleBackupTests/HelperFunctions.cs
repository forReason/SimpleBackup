using NuGet.Frameworks;

namespace SimpleBackupTests
{
    internal static class HelperFunctions
    {
        internal static void PrepareDirectories()
        {
            // Create source dir
            DirectoryInfo sourceDir = new DirectoryInfo("Source");
            if (sourceDir.Exists)
                sourceDir.Delete(true);
            sourceDir.Create();

            // Create subdir
            DirectoryInfo subDir = sourceDir.CreateSubdirectory("SubDirectory");
            DirectoryInfo subSubDir = subDir.CreateSubdirectory("SubSubDirectory");

            // Create files
            File.WriteAllText(Path.Combine(sourceDir.FullName, "sourcefile.txt"), "this is a source file");
            File.WriteAllText(Path.Combine(subDir.FullName, "subDirectorySourcefile.txt"), "this is a source file in a subDirectory");
            File.WriteAllText(Path.Combine(subSubDir.FullName, "subSubDirectorySourcefile.txt"), "this is a source file in a subSubDirectory");

            // delete other directories
            DirectoryInfo backupDir = new DirectoryInfo("Backup");
            if (backupDir.Exists)
                backupDir.Delete(true);
            DirectoryInfo restoreDir = new DirectoryInfo("Restore");
            if (restoreDir.Exists)
                restoreDir.Delete(true);
        }
        internal static void ValidateRestore()
        {
            DirectoryInfo sourceDir = new DirectoryInfo("Source");
            // validate Directories exist
            DirectoryInfo[] subDirectories = sourceDir.GetDirectories();
            Assert.Equal("SubDirectory", subDirectories[0].Name);
            DirectoryInfo[] subSubDirectories = subDirectories[0].GetDirectories();
            Assert.Equal("SubDirectory", subDirectories[0].Name);
            Assert.Equal("SubSubDirectory", subSubDirectories[0].Name);

            // validate files exist
            FileInfo sourceFile = new FileInfo(Path.Combine(sourceDir.FullName, "sourcefile.txt"));
            Assert.True(sourceFile.Exists);
            FileInfo subDirectorySourceFile = new FileInfo(Path.Combine(subDirectories[0].FullName, "subDirectorySourcefile.txt"));
            Assert.True(subDirectorySourceFile.Exists);
            FileInfo subSubDirectorySourceFile = new FileInfo(Path.Combine(subSubDirectories[0].FullName, "subSubDirectorySourcefile.txt"));
            Assert.True(subSubDirectorySourceFile.Exists);

            // validate file contents
            string sourceFileText = File.ReadAllText(sourceFile.FullName);
            Assert.Equal("this is a source file", sourceFileText);
            string subDirectorySourceFileText = File.ReadAllText(subDirectorySourceFile.FullName);
            Assert.Equal("this is a source file in a subDirectory", subDirectorySourceFileText);
            string subSubDirectorySourceFileText = File.ReadAllText(subSubDirectorySourceFile.FullName);
            Assert.Equal("this is a source file in a subSubDirectory", subSubDirectorySourceFileText);
        }
    }
}
