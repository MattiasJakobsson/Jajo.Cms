﻿using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using FubuMVC.Core.Runtime.Files;
using ImageProcessor;
using Jajo.Cms.Files;

namespace Jajo.Cms.FubuMVC1.Files
{
    public class DefaultFileUploader : IUploadFiles
    {
        private readonly IFubuApplicationFiles _fubuApplicationFiles;

        public DefaultFileUploader(IFubuApplicationFiles fubuApplicationFiles)
        {
            _fubuApplicationFiles = fubuApplicationFiles;
        }

        public string Upload(UploadFile file)
        {
            var name = $"{file.Category}/{file.Name}";

            var basePath = ConfigurationManager.AppSettings["Files.BasePath"];

            var filePath = $"{_fubuApplicationFiles.GetApplicationPath()}/{basePath}/{name}";

            file.Data.Position = 0;

            var directory = Path.GetDirectoryName(filePath);

            if (directory != null && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using (var fileLocation = File.Create(filePath))
                file.Data.CopyTo(fileLocation);

            return name;
        }

        public string GetPath(string file, ITransformFiles transformer = null)
        {
            var basePath = ConfigurationManager.AppSettings["Files.BasePath"];

            var original = $"{_fubuApplicationFiles.GetApplicationPath()}/{basePath}/{file}";

            if (transformer == null)
                return $"{basePath}/{file}";

            var settings = transformer.GetTransformationSettings().ToList();

            var maxWidth = settings.OfType<MaxWidthTransformationSetting>().Select(x => x.MaxWidth).FirstOrDefault();
            var maxHeight = settings.OfType<MaxHeightTransformationSetting>().Select(x => x.MaxHeight).FirstOrDefault();

            var fileName = file;

            var extension = Path.GetExtension(file);

            if (!string.IsNullOrEmpty(extension))
                fileName = fileName.Replace(extension, "");

            if (maxWidth > 0)
                fileName = string.Concat(fileName, maxWidth, "x");

            if (maxHeight > 0)
                fileName = string.Concat(fileName, maxHeight);

            var filePath =
                $"{_fubuApplicationFiles.GetApplicationPath()}/{basePath}/{fileName}{Path.GetExtension(file)}";

            if (File.Exists(filePath))
                return $"{basePath}/{fileName}{Path.GetExtension(file)}";

            if (!File.Exists(original))
                return $"{basePath}/{fileName}{Path.GetExtension(file)}";

            using (var fileOnDisk = File.Create(filePath))
            {
                Transform(File.Open(original, FileMode.Open), new Size(maxWidth, maxHeight)).CopyTo(fileOnDisk);
                fileOnDisk.Flush();
            }

            return $"{basePath}/{fileName}{Path.GetExtension(file)}";
        }

        public bool Exists(string file)
        {
            var basePath = ConfigurationManager.AppSettings["Files.BasePath"];

            var path = $"{_fubuApplicationFiles.GetApplicationPath()}/{basePath}/{file}";

            return File.Exists(path);
        }

        private static Stream Transform(Stream input, Size size)
        {
            var fileStream = new MemoryStream();
            using (input)
            {
                input.Position = 0;
                input.CopyTo(fileStream);
            }

            using (var imageFactory = new ImageFactory())
            {
                var outputStream = new MemoryStream();

                imageFactory.Load(fileStream)
                    .Resize(size)
                    .Save(outputStream);

                outputStream.Position = 0;

                return outputStream;
            }
        }
    }
}