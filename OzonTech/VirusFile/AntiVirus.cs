namespace OzonTech.VirusFile;

public class AntiVirus
{
    private const string VirusExtension = ".hack";

    public static int VirusCount(Folder folder)
    {
        var virusFilesCount = 0;

        var virusIsFound = false;

        if (folder.files != null)
        {
            if (folder.files.Any(file => file.EndsWith(VirusExtension)))
            {
                virusIsFound = true;
                virusFilesCount += CountFilesToFolder(folder);
            }
        }

        if (!virusIsFound)
        {
            if (folder.folders != null)
            {
                foreach (var f in folder.folders)
                {
                    virusFilesCount += VirusCount(f);
                }
            }
        }


        return virusFilesCount;
    }

    private static int CountFilesToFolder(Folder folder)
    {
        var countFiles = 0;

        if (folder.files != null)
        {
            countFiles += folder.files.Count;
        }

        if (folder.folders != null)
        {
            foreach (var f in folder.folders)
            {
                countFiles += CountFilesToFolder(f);
            }
        }

        return countFiles;
    }
}