using System;
using System.IO;
using System.Linq;

namespace Launchpad
{
    internal class ProjectCopier
    {
        private static readonly string[] ignoredDirectories = {"bin", "obj", ".vs"};
        private const string templateProjectName = "ProjectTemplate";

        private readonly Options options;
        private readonly string sourceDirectory;
        private readonly string targetDirectory;

        public ProjectCopier(Options options)
        {
            this.options = options;
            sourceDirectory = Helpers.PatchDirectoryName(Path.Combine("templates", "webapi"));
            targetDirectory = Path.Combine(options.Output, options.ProjectName);
        }

        public void Execute()
        {
            var sourceDir = new DirectoryInfo(sourceDirectory);
            var targetDir = new DirectoryInfo(targetDirectory);
            if (targetDir.Exists)
                targetDir.Delete(true);
            if (!targetDir.Exists)
                targetDir.Create();
            CopyDirectory(sourceDir, targetDir);
        }

        private void CopyDirectory(DirectoryInfo sourceDir, DirectoryInfo targetDir)
        {
            foreach (var subDir in sourceDir.GetDirectories().Where(d => !ignoredDirectories.Contains(d.Name, StringComparer.OrdinalIgnoreCase)))
                CopyDirectory(subDir, targetDir.CreateSubdirectory(GetTargetDirectoryName(subDir.Name)));
            foreach (var file in sourceDir.GetFiles())
                CopyFile(file, targetDir);
        }

        private string GetTargetDirectoryName(string subDirName)
        {
            if (string.Equals(subDirName, templateProjectName, StringComparison.OrdinalIgnoreCase))
                return options.ProjectName;
            return subDirName;
        }

        private void CopyFile(FileInfo file, DirectoryInfo targetDir)
        {
            using (var reader = file.OpenText())
            using (var writer = new StreamWriter(Path.Combine(targetDir.FullName, GetTargetFileName(file.Name))))
            {
                var content = reader.ReadToEnd();
                var patchedContent = content.Replace(templateProjectName, options.ProjectName);
                writer.Write(patchedContent);
            }
        }

        private string GetTargetFileName(string fileName)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            if (string.Equals(fileNameWithoutExtension, templateProjectName, StringComparison.OrdinalIgnoreCase))
                return options.ProjectName + Path.GetExtension(fileName);
            return fileName;
        }
    }
}